using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CharacteristicController : Controller
    {
        private CharacteristicService _characteristicService;

        public CharacteristicController(CharacteristicService characteristicService)
        {
            _characteristicService = characteristicService;
        }


        [Route(Constants.Routes.Characteristic.GET_CHARACTERISTICS)]
        public async Task<IActionResult> GetCharacteristics()
        {
            List<Characteristic> characteristics = await _characteristicService.GetCharacteristicsAsync();
            return Ok(characteristics);
        }

        [Route(Constants.Routes.Characteristic.GET_CHARACTERISTIC)]
        public async Task<IActionResult> GetCharacteristic([FromQuery] int characteristicId)
        {
            Characteristic ch = await _characteristicService.GetCharacteristicAsync(characteristicId);
            return Ok(ch);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Characteristic.UPDATE_CHARACTERISTIC)]
        public async Task<IActionResult> UpdateCharacteristic(Characteristic characteristic)
        {
            await _characteristicService.UpdateCharacteristicAsync(characteristic);
            return Ok();
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Characteristic.ADD_CHARACTERISTIC)]
        public async Task<IActionResult> AddCharacteristic(Characteristic characteristic)
        {
            await _characteristicService.AddCharacteristicAsync(characteristic);
            return Ok(characteristic);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Characteristic.REMOVE_CHARACTERISTIC)]
        public async Task<IActionResult> RemoveCharacteristic(Characteristic characteristic)
        {
            await _characteristicService.RemoveCharacteristicAsync(characteristic);
            return Ok();
        }
    }
}