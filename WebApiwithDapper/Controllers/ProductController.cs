using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiwithDapper.Models;

namespace WebApiwithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string connectionString;
        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConString")!;
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Products (Name, Brand, Category, Price, Description) OUTPUT INSERTED.*  VALUES (@Name, @Brand, @Category, @Price, @Description)";
                  var  Product = new Product()
                    {
                        Name = productDto.Name,
                        Brand = productDto.Brand,
                        Category = productDto.Category,
                        Price = productDto.Price,
                        Description = productDto.Description,
                    };
                  var newProduct =  connection.QuerySingleOrDefault<Product>(sql, Product);
                    if (newProduct != null) { 
                        return Ok(newProduct);
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("" + ex.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult GetProducts() {
            List<Product> products = new List<Product>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Products";
                    var data = connection.Query<Product>(sql);
                    products = data.ToList();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                return BadRequest();
            }
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Products where id = @Id";
                  var product =  connection.QuerySingleOrDefault<Product>(sql, new { Id = id });
                    if (product != null) { 
                    return Ok(product);
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("" + ex.Message);
                return BadRequest();
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE PRODUCTS SET Name = @Name, Brand = @Brand, Category = @Category, Description = @Description, PRICE = @Price WHERE id = @id";

                    var Product = new Product()
                    {
                        Id = id,
                        Name = productDto.Name,
                        Brand = productDto.Brand,
                        Category = productDto.Category,
                        Price = productDto.Price,
                        Description = productDto.Description,
                    };
                    int count = connection.Execute(sql, Product);
                    if (count < 1) {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                return BadRequest();
            }
            return GetProduct(id);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id) {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM PRODUCTS WHERE id = @id";
                    int count = connection.Execute(sql, new {Id = id});
                    if (count < 1)
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("" + ex.Message);
                return BadRequest(); 
            }
            return Ok();
        }
    }
}
