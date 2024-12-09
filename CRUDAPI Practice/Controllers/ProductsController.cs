using CRUDAPI_Practice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using CRUDAPI_Practice.Models;

namespace CRUDAPI_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string connectionString;

        public ProductsController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        //Method to create the poducts (Create)
        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //Conecting to the database
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    //Sql command
                    string sql = "INSERT INTO products " + "(name, brand, category, price, description) VALUES " +
                        "(@name, @brand, @category, @price, @description)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Products", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        //Method to read the products
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>();

            //Connecting to the database
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sql = "Select * from Products";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("Products", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok(products);
        }

        //Read the products by id
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            Product product = new Product();

            //Connecting to the databse to read the product with the id just provided.
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sql = "Select * from products where id=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Products", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok(product);
        }

        //Method to update a record
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Update products set name=@name , brand=@brand, category=@category, " +
                        "price=@price, description=@description where id=@id";

                    using (var command = new SqlCommand())
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Products", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        //Method to allow us to delete the product
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Delete from products where id=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Products", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
