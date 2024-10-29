using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MyMvcApp.Models; // הוספת using למודל Department


namespace MyMvcApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<Department> departments = new List<Department>();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // שימוש בחיבור למסד הנתונים
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"SELECT DepartmentID, DepartmentName FROM dbo.Department";
                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open(); // פותח את החיבור
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read()) // קורא את השורות
                    {
                        Department department = new Department()
                        {
                            DepartmentID = (int)myReader["DepartmentID"],
                            DepartmentName = myReader["DepartmentName"].ToString()
                        };
                        departments.Add(department);
                    }
                    myReader.Close(); // סוגר את הקורא
                }
                catch (Exception ex)
                {
                    // הוסף לוג במקרה של שגיאה
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult(departments); // מחזיר את המחלקות שנשלפו
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            List<Department> departments = new List<Department>();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // שימוש בחיבור למסד הנתונים
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"insert into dbo.Department values
                 ('"+dep.DepartmentName+ @"')";
                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open(); // פותח את החיבור
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read()) // קורא את השורות
                    {
                        Department department = new Department()
                        {
                            DepartmentID = (int)myReader["DepartmentID"],
                            DepartmentName = myReader["DepartmentName"].ToString()
                        };
                        departments.Add(department);
                    }
                    myReader.Close(); // סוגר את הקורא
                }
                catch (Exception ex)
                {
                    // הוסף לוג במקרה של שגיאה
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("added secsses"); // מחזיר את המחלקות שנשלפו
        }
        [HttpPut]
        public JsonResult Put(Department dep)
        {
            List<Department> departments = new List<Department>();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // שימוש בחיבור למסד הנתונים
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"
                update dbo.Department set
                DepartmentName ='" +dep.DepartmentName + @"'
                where DepartmentId =" +dep.DepartmentID + @"
                  ";
 ;
                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open(); // פותח את החיבור
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read()) // קורא את השורות
                    {
                        Department department = new Department()
                        {
                            DepartmentID = (int)myReader["DepartmentID"],
                            DepartmentName = myReader["DepartmentName"].ToString()
                        };
                        departments.Add(department);
                    }
                    myReader.Close(); // סוגר את הקורא
                }
                catch (Exception ex)
                {
                    // הוסף לוג במקרה של שגיאה
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("updated secsses"); // מחזיר את המחלקות שנשלפו
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            List<Department> departments = new List<Department>();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            // שימוש בחיבור למסד הנתונים
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                string query = @"
                delete from dbo.Department 
                where DepartmentId =" + id + @"
                  ";
                ;
                SqlCommand myCommand = new SqlCommand(query, myCon);

                try
                {
                    myCon.Open(); // פותח את החיבור
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read()) // קורא את השורות
                    {
                        Department department = new Department()
                        {
                            DepartmentID = (int)myReader["DepartmentID"],
                            DepartmentName = myReader["DepartmentName"].ToString()
                        };
                        departments.Add(department);
                    }
                    myReader.Close(); // סוגר את הקורא
                }
                catch (Exception ex)
                {
                    // הוסף לוג במקרה של שגיאה
                    return new JsonResult("Error: " + ex.Message);
                }
            }

            return new JsonResult("delete"); // מחזיר את המחלקות שנשלפו
        }
    }
}
