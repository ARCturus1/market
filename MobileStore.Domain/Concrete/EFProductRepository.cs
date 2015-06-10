using MobileStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        DataLayer.MarketDbContext context = new DataLayer.MarketDbContext();

        public IEnumerable<Entities.Product> Products
        {
            get { return context.Products; }
        }
    }
}
