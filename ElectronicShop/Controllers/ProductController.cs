using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.ViewModels.Files;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.Product.Filter;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private ProductsService _productsService;

        public ProductController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Consumes(Constants.Attributes.MULTIPART_FORM_DATA)]
        [Route(Constants.Routes.Product.UPLOAD_PRODUCT_PHOTO)]
        public async Task<IActionResult> UploadProductPhoto([FromForm] FileModel<ProductPhoto> fileModel)
        {
            fileModel.Deserialize();
            ProductPhoto ph = await _productsService.UploadProductPhotoAsync(fileModel);

            return Ok(ph);
        }


        [HttpGet]
        [Route(Constants.Routes.Product.GET_PRODUCTS)]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationFilter paginationFilter, [FromQuery] ProductFilter productFilter)
        {
            //  string route = Request.Path.Value;
            PagedResponse<List<Product>> res = await _productsService.GetProductsAsync(paginationFilter, productFilter);
            return Ok(res);
        }

        [HttpGet]
        [Route(Constants.Routes.Product.GET_PRODUCT)]
        public async Task<IActionResult>  GetProduct([FromQuery] int productId)
        {
            Product product = await _productsService.GetProductAsync(new Product { Id = productId });
            return Ok(product);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Product.ADD_PRODUCT)]
        public async Task<IActionResult> AddProduct(Product pr)
        {
            Product product = await _productsService.AddProductAsync(pr);
            return Ok(product);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Product.UPDATE_PRODUCT)]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            await _productsService.UpdateProductAsync(product);
            return Ok();
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Product.REMOVE_PRODUCT)]
        public async Task<IActionResult> RemoveProduct(Product product)
        {
            await _productsService.RemoveProductAsync(product);
            return Ok();
        }

    }
}