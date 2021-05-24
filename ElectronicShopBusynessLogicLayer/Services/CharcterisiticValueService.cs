using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class CharcterisiticValueService
    {

        private CharacteristicValueRepository _characteristicValueRepository;

        public CharcterisiticValueService(CharacteristicValueRepository characteristicValueRepository)
        {
            _characteristicValueRepository = characteristicValueRepository;
        }

        public async Task<List<CharacteristicValue>> GetCharacteristicValuesAsync(int characteristicId)
        {
            return await _characteristicValueRepository.GetCharacteristicValuesAsync(characteristicId);
        }

        public async Task AddCharacteristicValueAsync(CharacteristicValue characteristicValue)
        {
            await _characteristicValueRepository.AddAsync(characteristicValue);
        }

        public async Task UpdateCharacteristicValueAsync(CharacteristicValue model)
        {
            CharacteristicValue originalCV = await _characteristicValueRepository.GetByIdAsync(model.Id);

            if (!string.IsNullOrWhiteSpace(model.Name))
            { 
                originalCV.Name = model.Name;
            }

            await _characteristicValueRepository.UpdateAsync(originalCV);
        }

        public async Task RemoveCharacteristicValueAsync(CharacteristicValue characteristicValue)
        {
            CharacteristicValue chVal = await _characteristicValueRepository.GetByIdAsync(characteristicValue.Id);
            await _characteristicValueRepository.RemoveAsync(chVal);
        }
    }
}
