using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class CharacteristicRepository : GenericRepositorySql<Characteristic>
    {
        public CharacteristicRepository(DbContextSql context) : base(context)
        {
        }

        public async Task<List<Characteristic>> GetCharacteristicsAsync()
        {
            List<Characteristic> characteristics = await _dbSet
                .Include(c => c.CharacteristicValues)
                .ToListAsync();

            foreach (Characteristic characteristic in characteristics) 
            {
                 foreach (CharacteristicValue val in characteristic.CharacteristicValues)
                 {
                     val.Characteristic = null;
                 }
            }

            return characteristics;
        }

        public async Task<List<Characteristic>> GetCharacteristicsAsync(List<int> ids) 
        {
            List<Characteristic> characteristics = await _dbSet
                .Where(c => ids.Contains(c.Id)).ToListAsync();

            return characteristics;
        }

        public async Task<Characteristic> GetCharacteristicAsync(int characteristicId)
        {
            Characteristic characteristic = await _dbSet
                .Include(ch => ch.CharacteristicValues)
                .FirstOrDefaultAsync(ch => ch.Id == characteristicId);

            foreach (CharacteristicValue ch in characteristic.CharacteristicValues)
            {
                ch.Characteristic = null;
            }

            return characteristic;
        }
    }
}
