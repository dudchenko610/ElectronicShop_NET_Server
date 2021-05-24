using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.ViewModels;
using ElectronicShopDataAccessLayer.ViewModels.Product.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class ProductCommentService
    {

        private ProductCommentRepository _productCommentRepository;
        private UserService _userService;

        public ProductCommentService(ProductCommentRepository productCommentRepository, UserService userService)
        {
            _productCommentRepository = productCommentRepository;
            _userService = userService;
        }

        public async Task<ProductComment> AddProductCommentAsync(ProductComment productComment)
        {
            User meUser = await _userService.GetMyUserAsync();
            productComment.UserId = meUser.Id;

            await _productCommentRepository.AddAsync(productComment);

            return productComment;
        }

        public async Task RemoveProductCommentAsync(ProductComment productComment)
        {
            ProductComment productC = await _productCommentRepository.GetByIdAsync(productComment.Id);
            await _productCommentRepository.RemoveAsync(productC);
        }

        public async Task<GetCommentsResponse> GetProductCommentsAsync(GetCommentsRequest filter)
        {
            return await _productCommentRepository.GetProductCommentsAsync(filter);
        }
    }
}
