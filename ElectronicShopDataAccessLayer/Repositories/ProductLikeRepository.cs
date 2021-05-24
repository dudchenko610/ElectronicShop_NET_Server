using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class ProductLikeRepository : GenericRepositorySql<ProductLike>
    {

        private ProductsRepository _productsRepository;

        public ProductLikeRepository(DbContextSql context, ProductsRepository productsRepository) : base(context)
        {
            _productsRepository = productsRepository;
        }

        public async Task<ProductLike> GetProductLikeAsync(int productId, string userId)
        {
            ProductLike productLike = await _dbSet
                .Include(pl => pl.Product)
                .FirstOrDefaultAsync(pl => pl.ProductId == productId && pl.UserId == userId);

            return productLike;
        }

        private async Task ChangeLikeCountersSafeAsync(int productId, int likeCounter, int dislikeCounter)
        {
            Product product = await _productsRepository.GetByIdAsync(productId);

            _productsRepository.UpdateSafe
            (
                product,
                (propertyName, proposedValue, databaseValue) =>
                {
                    switch (propertyName)
                    {
                        case "LikeCount":
                            return (int)(databaseValue) + likeCounter;
                        case "DislikeCount":
                            return (int)(databaseValue) + dislikeCounter;
                    }

                    return null;
                }
            );

        }

        public async Task IncrementLikeCounterSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, 1, 0);
        }

        public async Task IncrementDislikeCounterSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, 0, 1);
        }


        public async Task DecrementLikeCounterSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, -1, 0);
        }

        public async Task DecrementDislikeCounterSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, 0, -1);
        }


        public async Task IncrementLikeDescrementDislikeCountersSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, 1, -1);
        }

        public async Task IncrementDislikeDescrementLikeCountersSafeAsync(int productId)
        {
            await ChangeLikeCountersSafeAsync(productId, -1, 1);
        }

        public async Task<ProductLike> LikeProductAsync(int productId, string userId, bool like)
        {

            ProductLike productLike = await GetProductLikeAsync(productId, userId);

            if (productLike == null)
            {
                ProductLike pl = new ProductLike
                {
                    ProductId = productId,
                    UserId = userId,
                    Like = like
                };

                await AddAsync(pl);
                productLike = pl;
            }
            else
            {
                productLike.Like = like;
                await UpdateAsync(productLike);
            }

            productLike.User = null;
            productLike.Product = null;

            return productLike;
        }
    }
}
