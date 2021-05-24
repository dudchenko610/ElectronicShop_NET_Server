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
    public class CharacteristicValueRepository : GenericRepositorySql<CharacteristicValue>
    {
        public CharacteristicValueRepository(DbContextSql context) : base(context)
        {
        }

        public async Task<List<CharacteristicValue>> GetCharacteristicValuesAsync(int characteristicId)
        {
            return await _dbSet.Where(v => v.CharacteristicId == characteristicId).ToListAsync();
        }

        public async Task<List<CharacteristicValue>> GetCharacteristicValuesAsync(List<int> ids)
        {
            return await _context.CharacteristicValues
                .Where(cv => ids.Contains(cv.Id))
                .Include(cv => cv.Characteristic)
                .ToListAsync();
        }

    }
}
