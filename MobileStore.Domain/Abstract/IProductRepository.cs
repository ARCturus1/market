using MobileStore.Domain.Entities;
using System.Collections.Generic;

namespace MobileStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
