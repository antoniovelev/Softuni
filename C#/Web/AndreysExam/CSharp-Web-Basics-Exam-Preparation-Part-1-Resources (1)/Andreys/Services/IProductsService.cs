using Andreys.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andreys.Services
{
    public interface IProductsService
    {
        void Add(string name, string description, string imgUrl, string category, string gender, decimal price);

        IEnumerable<Product> GetAllProducts();
    }
}
