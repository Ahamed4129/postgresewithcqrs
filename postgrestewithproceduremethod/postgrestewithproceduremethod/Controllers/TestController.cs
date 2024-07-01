using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using postgrestewithproceduremethod.Model;
using System.Data;

namespace postgrestewithproceduremethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IConfiguration _configuration;
        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public JsonResult Post(Test test)
        {
            string query = "CALL insert_into_test(@name)";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");

            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.CommandType = CommandType.Text; // Set command type to stored procedure
                    myCommand.Parameters.AddWithValue("@name", test.name); // Use correct parameter name

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
    
        [HttpPut("{id}")]
        public JsonResult Put(int id, Test test)
        {
            string query = "CALL update_test(@id, @name)";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");

            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@name", test.name);

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = "CALL delete_from_test(@id)";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");

            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.CommandType = CommandType.Text;
                    myCommand.Parameters.AddWithValue("@id", id);

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            string query = "SELECT * FROM get_all_tests()";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");

            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myConn.Open();
                    NpgsqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

        // GET: api/test/5
        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            string query = "SELECT * FROM get_test_by_id1(@p_id)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");

            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("p_id", id); // Changed to "p_id" without the "@" symbol

                    myConn.Open();
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                        myReader.Close();
                    }
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}


