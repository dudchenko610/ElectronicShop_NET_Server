using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class CharacteristicService
    {
        private CharacteristicRepository _characteristicRepository;

        public CharacteristicService(CharacteristicRepository characteristicRepository)
        {
            _characteristicRepository = characteristicRepository;
        }

        public async Task<List<Characteristic>> GetCharacteristicsAsync()
        {
            return await _characteristicRepository.GetCharacteristicsAsync();
        }

        public async Task<Characteristic> GetCharacteristicAsync(int characteristicId)
        {
            return await _characteristicRepository.GetCharacteristicAsync(characteristicId);
        }

        public async Task AddCharacteristicAsync(Characteristic characteristic)
        {
            await _characteristicRepository.AddAsync(characteristic);

            foreach (CharacteristicValue val in characteristic.CharacteristicValues)
            {
                val.Characteristic = null;
            }

        }

        public async Task UpdateCharacteristicAsync(Characteristic model)
        {
            Characteristic originalC = await _characteristicRepository.GetByIdAsync(model.Id);

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                originalC.Name = model.Name;
            }

            await _characteristicRepository.UpdateAsync(originalC);
        }

        public async Task RemoveCharacteristicAsync(Characteristic characteristic)
        {
            Characteristic ch = await _characteristicRepository.GetByIdAsync(characteristic.Id);
            await _characteristicRepository.RemoveAsync(ch);
        }
    }
}
