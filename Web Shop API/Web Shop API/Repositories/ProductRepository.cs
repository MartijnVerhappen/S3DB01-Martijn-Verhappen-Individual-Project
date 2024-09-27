using Microsoft.EntityFrameworkCore;
using Web_Shop_API.IRepositories;
using Web_Shop_API.Entities;
using MySql.Data.MySqlClient;
using System.Linq;

namespace Web_Shop_API.Repositories
{
    
    public class ProductRepository : IProductRepository
    {
        string connectionstring;
        public ProductRepository(string connectionstring) 
        {
            this.connectionstring = connectionstring;
        }

        public List<ProductEntity> GetAllProducts()
        {
            ProductEntity product = null;
            List<ProductEntity> products = null;
            MySqlConnection connection = new MySqlConnection(connectionstring);
            connection.Open();

            MySqlCommand getAllProductsCommand = connection.CreateCommand();

            getAllProductsCommand.CommandText = "SELECT Id, ProductType, ProductNaam, ProductPrijs, ProductKorting FROM Product";

            using (MySqlDataReader executeCommand = getAllProductsCommand.ExecuteReader())
                while (executeCommand.Read()) 
                { 
                    int id = executeCommand.GetInt32("Id");
                    string type = executeCommand.GetString("ProductType");
                    string naam = executeCommand.GetString("ProductNaam");
                    double prijs = executeCommand.GetDouble("ProductPrijs");
                    int korting = executeCommand.GetInt32("ProductKorting");

                    product = new ProductEntity(id, type, naam, prijs, korting);
                    products.Add(product);
                }
            return products;
            throw new NotImplementedException();
        }

        public ProductEntity GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
