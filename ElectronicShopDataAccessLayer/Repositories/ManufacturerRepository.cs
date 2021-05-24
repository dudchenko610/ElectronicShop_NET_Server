using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class ManufacturerRepository : GenericRepositorySql<Manufacturer>
    {
        public ManufacturerRepository(DbContextSql context) : base(context)
        {
        }
    }
}
