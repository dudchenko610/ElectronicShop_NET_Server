using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class ManufacturerService
    {
        private ManufacturerRepository _manufacturerRepository;

        public ManufacturerService(ManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<Manufacturer>> GetManufacturersAsync()
        {
            return await _manufacturerRepository.GetAsync();
        }

        public async Task<Manufacturer> GetManufacturerAsync(Manufacturer manufacturer)
        {
            return await _manufacturerRepository.GetByIdAsync(manufacturer.Id);
        }

        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            await _manufacturerRepository.AddAsync(manufacturer);
        }

        public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            Manufacturer originalMan = await _manufacturerRepository.GetByIdAsync(manufacturer.Id);

            if (!string.IsNullOrEmpty(manufacturer.Name))
            {
                originalMan.Name = manufacturer.Name;
            }

            await _manufacturerRepository.UpdateAsync(manufacturer);
        }

        public async Task RemoveManufacturerAsync(Manufacturer manufacturer)
        {
            Manufacturer manuf = await _manufacturerRepository.GetByIdAsync(manufacturer.Id);
            await _manufacturerRepository.RemoveAsync(manuf);
        }
    
    }
}
