using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        [Authorize(Roles = Constants.AuthRoles.CLIENT)]
        [Route(Constants.Routes.Order.ADD_ORDER)]
        public async Task AddAuthOrder(OrderAuth orderAuth)
        {
            
        }

        [Route(Constants.Routes.Order.ADD_NOT_AUTH_ORDER)]
        public async Task AddNotauthOrder(OrderNotauth orderNotauth)
        {

        }

        [Authorize(Roles = Constants.AuthRoles.CLIENT)]
        [Route(Constants.Routes.Order.GET_MY_ORDERS)]
        public async Task<ActionResult<List<OrderAuth>>> GetMyOrders()
        {
            return null;
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Order.GET_AUTH_ORDERS)]
        public async Task<ActionResult<List<OrderAuth>>> GetAuthOrders()
        {
            return null;
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Order.GET_NOT_AUTH_ORDERS)]
        public async Task<ActionResult<List<OrderAuth>>> GetNotauthOrders()
        {
            return null;
        }
    }
}