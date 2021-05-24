using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using System;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class ProductPhotoRepository : GenericRepositorySql<ProductPhoto>
    {
        public ProductPhotoRepository(DbContextSql context) : base(context)
        {
        }

    }
}
