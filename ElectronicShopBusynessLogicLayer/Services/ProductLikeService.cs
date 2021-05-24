using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using Shared.Constants;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class ProductLikeService
    {
        private ProductLikeRepository _productLikeRepository;
        private ProductsRepository _productsRepository;
        private UserService _userService;
        public ProductLikeService(ProductLikeRepository productLikeRepository, UserService userService, ProductsRepository productsRepository)
        {
            _productLikeRepository = productLikeRepository;
            _userService = userService;
            _productsRepository = productsRepository;
        }

        public async Task<ProductLike> LikeProductAsync(Product p)
        {

            User meUser = await _userService.GetMyUserAsync();
            ProductLike productLike = await _productLikeRepository.GetProductLikeAsync(p.Id, meUser.Id);

            if (productLike == null)
            {
                await _productLikeRepository.IncrementLikeCounterSafeAsync(p.Id);
                ProductLike pl = await _productLikeRepository.LikeProductAsync(p.Id, meUser.Id, true);
                return pl;
            }

            if (!productLike.Like)
            {
                await _productLikeRepository.IncrementLikeDescrementDislikeCountersSafeAsync(p.Id);

                ProductLike pl = await _productLikeRepository.LikeProductAsync(p.Id, meUser.Id, true);
                return pl;
            }

            throw new ServerException(Constants.Errors.Product.YOU_HAVE_LIKED_THIS_PRODUCT_ALREADY);

        }

        public async Task<ProductLike> DislikeProductAsync(Product p)
        {
            User meUser = await _userService.GetMyUserAsync();
            ProductLike productLike = await _productLikeRepository.GetProductLikeAsync(p.Id, meUser.Id);

            if (productLike == null)
            {
                await _productLikeRepository.IncrementDislikeCounterSafeAsync(p.Id);
                ProductLike pl = await _productLikeRepository.LikeProductAsync(p.Id, meUser.Id, false);
                return pl;
            }

            if (productLike.Like)
            {
                await _productLikeRepository.IncrementDislikeDescrementLikeCountersSafeAsync(p.Id);

                ProductLike pl = await _productLikeRepository.LikeProductAsync(p.Id, meUser.Id, false);
                return pl;
            }

            throw new Exception(Constants.Errors.Product.YOU_HAVE_DISLIKED_THIS_PRODUCT_ALREADY);
        }

        public async Task TakeReactionBackAsync(Product p)
        {
            User meUser = await _userService.GetMyUserAsync();
            ProductLike productLike = await _productLikeRepository.GetProductLikeAsync(p.Id, meUser.Id);

            if (productLike != null)
            {
                if (productLike.Like)
                { 
                    await _productLikeRepository.DecrementLikeCounterSafeAsync(p.Id);
                }
                else
                {
                    await _productLikeRepository.DecrementDislikeCounterSafeAsync(p.Id);
                }
                await _productLikeRepository.RemoveAsync(productLike);
            }
        }
        
    }
}
