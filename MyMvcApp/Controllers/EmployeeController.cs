using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.IO.Pipes;
namespace MyMvcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<Employee> employees = new List<Employee>();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"SELECT EmployeeID, EmployeeName, Department,
                CONVERT(VARCHAR(10), DateOfJoining, 120) as DateOfJoining, PhotoFileName
                FROM dbo.Employee";

                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            EmployeeID = (int)myReader["EmployeeID"],
                            EmployeeName = myReader["EmployeeName"].ToString(),
                            Department = myReader["Department"].ToString(),
                            DateOfJoining = myReader["DateOfJoining"].ToString(),
                            PhotoFileName = myReader["PhotoFileName"].ToString()
                        };
                        employees.Add(employee);
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult(employees);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"INSERT INTO dbo.Employee (EmployeeName, Department, DateOfJoining, PhotoFileName)
                                 VALUES ('" + emp.EmployeeName + @"', 
                                         '" + emp.Department + @"', 
                                         '" + emp.DateOfJoining + @"', 
                                         '" + emp.PhotoFileName + @"')";

                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"UPDATE dbo.Employee SET 
                                 EmployeeName = '" + emp.EmployeeName + @"', 
                                 Department = '" + emp.Department + @"', 
                                 DateOfJoining = '" + emp.DateOfJoining + @"', 
                                 PhotoFileName = '" + emp.PhotoFileName + @"' 
                                 WHERE EmployeeID = " + emp.EmployeeID;

                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"DELETE FROM dbo.Employee 
                                 WHERE EmployeeID = " + id;

                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open();
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("Deleted successfully");
        }



        [Route("saveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;

                // בדיקה אם יש קבצים מצורפים לבקשה
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];
                    string fileName = Path.GetFileName(postedFile.FileName); // הבטחת קבלת שם קובץ בטוח

                    // הגדרת נתיב פיזי לשמירת הקובץ
                    var physicalPath = Path.Combine(_env.ContentRootPath, "Photos", fileName);

                    // בדיקת קיום התיקייה אם היא לא קיימת
                    if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "Photos")))
                    {
                        Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Photos"));
                    }

                    // שמירת הקובץ במיקום הרצוי
                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    // החזרת שם הקובץ שנשמר
                    return new JsonResult(fileName);
                }
                else
                {
                    return new JsonResult("No file was uploaded");
                }
            }
            catch (Exception ex)
            {
                // החזרת שם ברירת מחדל במקרה של שגיאה
                return new JsonResult("anonymous.jpg");
            }
        }

        [Route("getAllDepartmentsNames")]
        [HttpGet]
        public JsonResult getAllDepartmentsNames()
        {
            List<string> departmentNames = new List<string>(); // שימוש ברשימה של מיתרים לשמות המחלקות
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"SELECT DepartmentName FROM dbo.Department"; // ביצוע השאילתה לשמות המחלקות

                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        // הוסף את שם המחלקה לרשימה
                        departmentNames.Add(myReader["DepartmentName"].ToString());
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult(departmentNames); // החזרת שמות המחלקות
        }



    }
}
