using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Services;
using ElectronicShopDataAccessLayer.ViewModels.Product.Query;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.Product.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{

   

    public class ProductsRepository : GenericRepositorySql<Product>
    {

        private ProductPhotoRepository _productPhotoRepository;

        public ProductsRepository(DbContextSql context, ProductPhotoRepository productPhotoRepository) : base(context)
        {
            _productPhotoRepository = productPhotoRepository;
        }

        public async Task<PagedResponse<List<Product>>> GetProductsAsync(PaginationFilter paginationFilter, ProductFilter productFilter)
        {

            string sqlRequest = "SELECT p.Id, p.Name, p.ManufacturerId, p.CategoryId, p.ProductMainPhotoId, p.Price, pp.FileName\n" +
                                "FROM Products AS p\n";

            if (productFilter.CharacteristicValuesIds.Count != 0)
            {

                // 1. Group CharacteristicValues by Characteristics

                var charValGroups = (await _context.CharacteristicValues
                    .Include(chVal => chVal.Characteristic)
                    .Where(chVal => productFilter.CharacteristicValuesIds.Any(chValId => chValId == chVal.Id))
                    .ToListAsync())
                    .GroupBy(chVal => chVal.CharacteristicId);

                string characteristicValRequest = "";

                int j = 0;

                foreach (IGrouping<int, CharacteristicValue> chValIdsPerCharacteristic in charValGroups)
                {
                    characteristicValRequest += "JOIN N_N_Product_Characteristics AS nn" + (j + 1) + " ON p.Id = nn" + (j + 1) + ".ProductId\nAND ( \n";

                    int i = 0;

                    foreach (CharacteristicValue charVal in chValIdsPerCharacteristic)
                    {
                        string or = i != (chValIdsPerCharacteristic.Count() - 1) ? " OR" : " )";

                        characteristicValRequest += "nn" + (j + 1) + ".CharacteristicValueId = " + charVal.Id + or + "\n";

                        i++;
                    }


                    j++;
                }

                

                sqlRequest += characteristicValRequest;
            }

            sqlRequest += "LEFT JOIN ProductPhotos AS pp ON pp.Id = p.ProductMainPhotoId\n";
            sqlRequest += "WHERE p.CategoryId = " + productFilter.CategoryId + " AND p.Price >= " + productFilter.MinPrice + " AND p.Price <= " + productFilter.MaxPrice + "\n";

            if (productFilter.ManufacturerIds.Count != 0)
            {
                string manufacturerIds = "AND (";

                for (int i = 0; i < productFilter.ManufacturerIds.Count - 1; i++)
                {
                    manufacturerIds += "p.ManufacturerId = " + productFilter.ManufacturerIds[i] + " OR\n";
                }

                manufacturerIds += "p.ManufacturerId = " + productFilter.ManufacturerIds[productFilter.ManufacturerIds.Count - 1] + ")\n";

                sqlRequest += manufacturerIds;
            }

            sqlRequest += ";";

            Console.WriteLine(sqlRequest);

            List<ProductJoined> joinedProducts = await _context.ProductJoineds.FromSqlRaw(sqlRequest).ToListAsync();
            List<ProductJoined> pagedJoined = joinedProducts
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();


            List<Product> pagedProducts = new List<Product>();


            foreach(ProductJoined pj in pagedJoined)
            {
                Product p = new Product
                {
                    Id = pj.Id,
                    Name = pj.Name,
                    ManufacturerId = pj.ManufacturerId,
                    CategoryId = pj.CategoryId,
                    ProductMainPhotoId = pj.ProductMainPhotoId,
                    Price = pj.Price
                };

                if (pj.ProductMainPhotoId != 0)
                {
                    p.ProductMainPhoto = new ProductPhoto
                    {
                        Id = pj.ProductMainPhotoId,
                        FileName = pj.FileName
                    };
                }

                pagedProducts.Add(p);
            }

            

            return PaginationHelper.CreatePagedReponse<Product>(pagedProducts, paginationFilter, joinedProducts.Count);
            
        }

        public async Task AddNNConnectionsAsync(Product product)
        {

            foreach (N_N_Product_CharacteristicValue nn in product.N_N_Product_Characteristics)
            {
                nn.ProductId = product.Id;
            }

            await _context.N_N_Product_Characteristics.AddRangeAsync(product.N_N_Product_Characteristics);
            await _context.SaveChangesAsync();
        }



        public async Task UpdatePlainProductAsync(Product p)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == p.Id);

            // maybe mapper should be used here
            product.Name = p.Name;
            product.ManufacturerId = p.ManufacturerId;
            product.ProductMainPhotoId = p.ProductMainPhotoId;
            product.Price = p.Price;
            product.Count = p.Count;
            product.Description = p.Description;

            UpdateSafe
            (
                product,
                (propertyName, proposedValue, databaseValue) =>
                {
                    switch (propertyName)
                    {
                        case "Name":
                        case "ManufacturerId":
                        case "ProductMainPhotoId":
                        case "Price":
                        case "Description":
                            return proposedValue;
                        default:
                            return databaseValue;
                    }

                }
            );

        }

        public async Task SetMainPhotoIdAsync(int productId, int mainPhotoId)
        { 
            Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            product.ProductMainPhotoId = mainPhotoId;

            UpdateSafe
            (
                product,
                (propertyName, proposedValue, databaseValue) =>
                {
                    switch (propertyName)
                    {
                        case "ProductMainPhotoId":
                            return proposedValue;
                        default:
                            return databaseValue;
                    }

                }
            );

        }

        

        public async Task<ProductPhoto> GetProductMainPhotoAsync(int productMainPhotoId)
        {
            ProductPhoto mainPhoto = await _context.ProductPhotos
                .FirstOrDefaultAsync(ph => ph.Id == productMainPhotoId);

            return mainPhoto;
        }

        public async Task<Product> GetProductWithPhotosCharacterisitcsAsync(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductPhotos)
                .Include("N_N_Product_Characteristics.CharacteristicValue")
                .FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }

        public async Task RemoveProductsByCategoryIdAsync(int categoryId)
        {
            await RemoveRangeAsync(_dbSet.Where(p => p.CategoryId == categoryId));
        }

    }
}
