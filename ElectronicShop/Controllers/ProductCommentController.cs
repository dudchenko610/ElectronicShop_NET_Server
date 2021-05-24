using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ElectronicShopBusinessLogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using ElectronicShopDataAccessLayer.Models;
using Shared.Constants;
using ElectronicShopDataAccessLayer.ViewModels.Product.Comments;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductCommentController : ControllerBase
    {

        private ProductCommentService _pcService;
        public ProductCommentController(ProductCommentService pcService)
        {
            _pcService = pcService;
        }

        [Authorize]
        [Route(Constants.Routes.ProductComment.ADD_PRODUCT_COMMENT)]
        public async Task<IActionResult> AddProductComment(ProductComment productComment)
        {
            ProductComment pc = await _pcService.AddProductCommentAsync(productComment);
            ProductComment p = new ProductComment 
            {
                Id = pc.Id,
                Content = pc.Content,
                ProductId = pc.ProductId,
                DateTime = pc.DateTime,
                UserId = pc.UserId
            };
            return Ok(pc);
        }

        [Authorize]
        [Route(Constants.Routes.ProductComment.REMOVE_PRODUCT_COMMENT)]
        public async Task<IActionResult> RemoveProductComment(ProductComment productComment)
        {
            await _pcService.RemoveProductCommentAsync(productComment);
            return Ok();
        }

        [HttpGet]
        [Route(Constants.Routes.ProductComment.GET_PRODUCT_COMMENTS)]
        public async Task<IActionResult> GetProductComments([FromQuery] GetCommentsRequest filter)
        {
            GetCommentsResponse gcr = await _pcService.GetProductCommentsAsync(filter);
            return Ok(gcr);
        }

    }
}