using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using postgrestewithproceduremethod.Model;
using System.Data;

namespace postgrestewithproceduremethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController (IConfiguration configuration)
        {
            _configuration= configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                          select DepartmentId as""DepartmentId"",
                           DepartmentName as""DepartmentName""from Department  ";
            DataTable table=new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using(NpgsqlCommand myCommand =new NpgsqlCommand(query,myConn))
                {
                    myReader=myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);   
        }
        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"
                INSERT INTO Department (DepartmentName) 
                VALUES (@DepartmentName)";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myCommand.ExecuteNonQuery();
                }
                myConn.Close();
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"
                UPDATE Department 
                SET DepartmentName = @DepartmentName
                WHERE DepartmentId = @DepartmentId";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myCommand.ExecuteNonQuery();
                }
                myConn.Close();
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM Department 
                WHERE DepartmentId = @DepartmentId";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myCommand.ExecuteNonQuery();
                }
                myConn.Close();
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
