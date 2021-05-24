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
    public class CharacteristicValueController : ControllerBase
    {

        private CharcterisiticValueService _characteristicValueService;

        public CharacteristicValueController(CharcterisiticValueService characteristicValueService)
        {
            _characteristicValueService = characteristicValueService;
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.CharacteristicValue.UPDATE_CHARACTERISTIC_VALUE)]
        public async Task<IActionResult> UpdateCharacteristicValue(CharacteristicValue characteristicValue)
        {
            await _characteristicValueService.UpdateCharacteristicValueAsync(characteristicValue);
            return Ok();
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.CharacteristicValue.ADD_CHARACTERISTIC_VALUE)]
        public async Task<IActionResult> AddCharacteristicValue(CharacteristicValue characteristicValue)
        {
            await _characteristicValueService.AddCharacteristicValueAsync(characteristicValue);
            return Ok(characteristicValue);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.CharacteristicValue.REMOVE_CHARACTERISTIC_VALUE)]
        public async Task<IActionResult> RemoveCharacteristicValue(CharacteristicValue characteristicValue)
        {
            await _characteristicValueService.RemoveCharacteristicValueAsync(characteristicValue);
            return Ok();
        }
    }
}