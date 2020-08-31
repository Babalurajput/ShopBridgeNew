using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridge.Models
{
    public class ItemRepository : IItemRepository
    {
        private ShopBridgeContext _db = new ShopBridgeContext();
        public IEnumerable<Sale> GetAllItem()
        {
            return _db.Sales.ToList();
        }

        public void CreateNewItem(Sale SaleToCreate)
        {
            _db.Sales.Add(SaleToCreate);
            _db.SaveChanges();

        }

        public void DeleteItem(int id)
        {
            var conToDel = GetItemByID(id);
            _db.Sales.Remove(conToDel);
            _db.SaveChanges();

        }

        public Sale GetItemByID(int id)
        {
            return _db.Sales.FirstOrDefault(d => d.Id == id);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }

    public interface IItemRepository
    {
        IEnumerable<Sale> GetAllItem();
        void CreateNewItem(Sale ItemToCreate);
        void DeleteItem(int id);
        Sale GetItemByID(int id);
        int SaveChanges();
    }
}
