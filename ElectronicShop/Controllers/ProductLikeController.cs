using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductLikeController : Controller
    {
        private ProductLikeService _plService;
        public ProductLikeController(ProductLikeService plService)
        {
            _plService = plService;
        }

        [Authorize(Roles = Constants.AuthRoles.CLIENT)]
        [Route(Constants.Routes.ProductLike.LIKE_PRODUCT)]
        public async Task<IActionResult> LikeProduct(Product productLike)
        {
            ProductLike pl = await _plService.LikeProductAsync(productLike);
            return Ok(pl);
        }

        [Authorize(Roles = Constants.AuthRoles.CLIENT)]
        [Route(Constants.Routes.ProductLike.DISLIKE_PRODUCT)]
        public async Task<IActionResult> DislikeProduct(Product productLike)
        {
            ProductLike pl = await _plService.DislikeProductAsync(productLike);
            return Ok(pl);
        }

        [Authorize(Roles = Constants.AuthRoles.CLIENT)]
        [Route(Constants.Routes.ProductLike.TAKE_REACTION_BACK)]
        public async Task<IActionResult> TakeReactionBack(Product productLike)
        {
            await _plService.TakeReactionBackAsync(productLike);
            return Ok();
        }

    }
}
