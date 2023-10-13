using BooksEcommece.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksEcommece.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void SaveChanges();
        void Update(Product product);
    }
}
