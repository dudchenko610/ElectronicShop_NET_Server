using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.Services;
using Shared.Constants;
using Shared.Exceptions;
using Shared.ViewModels.Files;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.Product.Filter;

namespace ElectronicShopBusinessLogicLayer.Services
{

    public class ProductsWithCount
    {
        public List<Product> Products { get; set; }
        public int ProductsCount { get; set; } 
    } 

    public class ProductsService
    {
        private ProductsRepository _productsRepository;
        private CharacteristicValueRepository _characteristicValueRepository;
        private FileService _fileService;
        private ProductPhotoRepository _productPhotoRepository;
        private UserService _userService;
        private N_N_ProductCharacteristicValueRepository _n_n_ProductCharacteristicValueRepository;
        private ProductLikeRepository _productLikeRepository;


        public ProductsService(ProductsRepository productsRepository, 
            CharacteristicValueRepository characteristicValueRepository, 
            FileService fileService, 
            ProductPhotoRepository productPhotoRepository,
            UserService userService,
            N_N_ProductCharacteristicValueRepository n_n_ProductCharacteristicValueRepository,
            ProductLikeRepository productLikeRepository
            )
        {
            _productsRepository = productsRepository;
            _characteristicValueRepository = characteristicValueRepository;
            _fileService = fileService;
            _productPhotoRepository = productPhotoRepository;
            _userService = userService;
            _n_n_ProductCharacteristicValueRepository = n_n_ProductCharacteristicValueRepository;
            _productLikeRepository = productLikeRepository;
        }

        public async Task<ProductPhoto> UploadProductPhotoAsync(FileModel<ProductPhoto> fileModel)
        {
            _fileService.CheckFileModelConsistency(fileModel);


            ProductPhoto productPhoto = fileModel.Optional; // productId here already
            productPhoto.FileName = fileModel.FileName;

            await _productPhotoRepository.AddAsync(productPhoto);
            await _fileService.SaveFileAsync(fileModel.FormFile, "not_auth/products/" + productPhoto.ProductId + "/photo" + productPhoto.Id + "/", fileModel.FileName);

            if (productPhoto.IsMain)
            {
                await _productsRepository.SetMainPhotoIdAsync(productPhoto.ProductId, productPhoto.Id);
            }

            return new ProductPhoto { Id = productPhoto.Id };
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await CheckNNConsistencyAsync(product);
            await _productsRepository.AddAsync(product);

            return new Product { Id = product.Id};
        }

        public async Task UpdateProductAsync(Product product)
        {

            // 1. Photos

            List<ProductPhoto> ppsToRemove = product.ProductPhotos;

            if (product.ProductMainPhotoId != 0)
            {
                foreach (ProductPhoto pp in ppsToRemove)
                {
                    if (product.ProductMainPhotoId == pp.Id)
                    {
                        product.ProductMainPhotoId = 0;
                        break;
                    }
                }
            }

            // more or less safe file deleting
            try
            {
                foreach (ProductPhoto pp in ppsToRemove)
                {
                    _fileService.DeleteFile("not_auth/products/" + pp.ProductId + "/photo" + pp.Id + "/", pp.FileName);
                }
            }
            finally
            {
                await _productPhotoRepository.RemoveRangeAsync(ppsToRemove);
            }

            // 2. N to N

            await _n_n_ProductCharacteristicValueRepository.RemoveCharacteristicValuesForProductAsync(product.Id);

            await CheckNNConsistencyAsync(product);
            await _productsRepository.AddNNConnectionsAsync(product);


            // 3. Update product
            await _productsRepository.UpdatePlainProductAsync(product);



        }

        public async Task<PagedResponse<List<Product>>> GetProductsAsync(PaginationFilter paginationFilter, ProductFilter productFilter)
        {
            PaginationFilter validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            ProductFilter validProductFilter = new ProductFilter(productFilter);

            return await _productsRepository.GetProductsAsync(paginationFilter, productFilter);
        }

        public async Task<Product> GetProductAsync(Product pr)
        {
            User myUser = await _userService.GetMyUserAsync();
            string userId = myUser == null ? "" : myUser.Id;

            Product product = await _productsRepository.GetProductWithPhotosCharacterisitcsAsync(pr.Id);
            ProductPhoto mainPhoto = await _productsRepository.GetProductMainPhotoAsync(product.ProductMainPhotoId);

            product.ProductMainPhoto = mainPhoto;


            if (userId != "")
            {
                ProductLike myLike = await _productLikeRepository.GetProductLikeAsync(pr.Id, userId);

                if (myLike != null)
                {
                    myLike.User = null;
                    myLike.Product = null;

                    product.ProductLike = myLike;
                }
                else
                {
                    product.ProductLike = new ProductLike();
                }

            }
            else
            {
                product.ProductLike = new ProductLike();
            }

            foreach (ProductPhoto ph in product.ProductPhotos)
            {
                ph.Product = null;
            }

            foreach (N_N_Product_CharacteristicValue nn in product.N_N_Product_Characteristics)
            {
                nn.Product = null;
                nn.CharacteristicValue.N_N_Product_Characteristics = null;
            }

            product.Category = null;

            return product;
        }

        public async Task RemoveProductAsync(Product product)
        {
            Product p = await _productsRepository.GetByIdAsync(product.Id);
            await _productsRepository.RemoveAsync(p);
        }

        private async Task CheckNNConsistencyAsync(Product product)
        {
            List<int> charactValIds = new List<int>();

            if (product.N_N_Product_Characteristics != null)
            {
                foreach (N_N_Product_CharacteristicValue n_n in product.N_N_Product_Characteristics)
                {
                    charactValIds.Add(n_n.CharacteristicValueId);
                }
            }


            List<CharacteristicValue> characteristicValues
                = await _characteristicValueRepository.GetCharacteristicValuesAsync(charactValIds);

            foreach (CharacteristicValue val in characteristicValues)
            {
                if (val.Characteristic.CategoryId != product.CategoryId)
                {
                    throw new ServerException(Constants.Errors.Product.CHARACTERISTIC_VALUES_OF_DIFERENT);
                }
            }
        }
    }
}
