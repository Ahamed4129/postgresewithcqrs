using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using postgrestewithproceduremethod.Model;
using System;
using System.Data;

namespace postgrestewithproceduremethod.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT 
                    EmployeeId AS ""EmployeeId"",
                    EmployeeName AS ""EmployeeName"",
                    Department AS ""Department"",
                    DateofJoining AS ""DateofJoining""
                FROM Employee";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    using (NpgsqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                INSERT INTO Employee (EmployeeName, Department, DateofJoining) 
                VALUES (@EmployeeName, @Department, @DateofJoining)";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateofJoining", Convert.ToDateTime(employee.DateofJoining));
                    myCommand.ExecuteNonQuery();
                }
                myConn.Close();
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                UPDATE Employee 
                SET EmployeeName = @EmployeeName,
                    Department = @Department,
                    DateofJoining = @DateofJoining
                WHERE EmployeeId = @EmployeeId";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateofJoining", Convert.ToDateTime(employee.DateofJoining));
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
                DELETE FROM Employee 
                WHERE EmployeeId = @EmployeeId";

            string sqlDataSource = _configuration.GetConnectionString("FullConnectionstring");
            using (NpgsqlConnection myConn = new NpgsqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myCommand.ExecuteNonQuery();
                }
                myConn.Close();
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
