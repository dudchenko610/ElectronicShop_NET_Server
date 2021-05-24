using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShopBusinessLogicLayer.Services;
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
    public class ManufacturerController : Controller
    {
        private ManufacturerService _manufacturerService;

        public ManufacturerController(ManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [Route(Constants.Routes.Manufacturer.GET_MANUFACTURERS)]
        public async Task<ActionResult<List<Manufacturer>>> GetManufacturers()
        {
            return await _manufacturerService.GetManufacturersAsync();
        }

        [HttpGet]
        [Route(Constants.Routes.Manufacturer.GET_MANUFACTURER)]
        public async Task<ActionResult<Manufacturer>> GetManufacturer([FromQuery] int manufacturerId)
        {
            return await _manufacturerService.GetManufacturerAsync(new Manufacturer { Id = manufacturerId });
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Manufacturer.UPDATE_MANUFACTURER)]
        public async Task<ActionResult<object>> UpdateCategory(Manufacturer manufacturer)
        {
            await _manufacturerService.UpdateManufacturerAsync(manufacturer);
            return new object();
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Manufacturer.ADD_MANUFACTURER)]
        public async Task<ActionResult<Manufacturer>> AddManufacturer(Manufacturer manufacturer)
        {
            await _manufacturerService.AddManufacturerAsync(manufacturer);
            return manufacturer;
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Manufacturer.REMOVE_MANUFACTURER)]
        public async Task<ActionResult<object>> RemoveCategory(Manufacturer manufacturer)
        {
            await _manufacturerService.RemoveManufacturerAsync(manufacturer);
            return new object();
        }
    }
}