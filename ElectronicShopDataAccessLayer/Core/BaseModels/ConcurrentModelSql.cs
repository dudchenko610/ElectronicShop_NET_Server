using System.ComponentModel.DataAnnotations;

namespace ElectronicShopDataAccessLayer.Core.BaseModels
{
    public class ConcurrentModelSql : Entity
    {
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
