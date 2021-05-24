using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class N_N_ProductCharacteristicValueRepository : GenericRepositorySql<N_N_Product_CharacteristicValue>
    {
        public N_N_ProductCharacteristicValueRepository(DbContextSql context) : base(context)
        { 
        }

        public async Task RemoveCharacteristicValuesForProductAsync(int productId)
        {
            await RemoveRangeAsync(_dbSet.Where(nn => nn.ProductId == productId));
        }

    }
}
