using Andreys.Data;
using Andreys.Models;
using Andreys.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andreys.Services
{
    public class ProductService : IProductsService
    {
        private readonly AndreysDbContext db;

        public ProductService(AndreysDbContext db)
        {
            this.db = db;
        }

        public void Add(string name, string description, string imgUrl, string category, string gender, decimal price)
        {
            var currentCategory = Enum.Parse<Category>(category);
            var currentGender = Enum.Parse<Gender>(gender);

            var product = new Product
            {
                Name = name,
                Description = description,
                ImageUrl = imgUrl,
                Category = currentCategory,
                Gender = currentGender,
                Price = price
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = this.db.Products.ToList();

            return products;
        }
    }
}
