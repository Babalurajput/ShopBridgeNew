using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Tests.Models
{
    class InmemoryItemRepository : IItemRepository
    {
        private List<Sale> _db = new List<Sale>();
        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<Sale> GetAllItem()
        {
            return _db.ToList();
        }

        public Sale GetItemByID(int id)
        {
            return _db.FirstOrDefault(d => d.Id  == id);
        }

        public void CreateNewItem(Sale ItemToCreate)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            _db.Add(ItemToCreate);
        }

        public void SaveChanges(Sale ItemToUpdate)
        {

            foreach (Sale Item in _db)
            {
                if (Item.Id== ItemToUpdate.Id)
                {
                    _db.Remove(Item);
                    _db.Add(ItemToUpdate);
                    break;
                }
            }
        }

        public void Add(Sale ItemToAdd)
        {
            _db.Add(ItemToAdd);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

    }
}
