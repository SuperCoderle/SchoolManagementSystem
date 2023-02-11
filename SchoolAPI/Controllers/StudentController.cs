using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Connect;
using System.Data;
using System.Data.SqlClient;
using SchoolAPI.Modal;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IWebHostEnvironment Web;

		public StudentController(IWebHostEnvironment web)
        {
			Web = web;
		}

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT StudentID, FirstName, LastName, Class ,Section ,Gender ,CONVERT(VARCHAR(10), DateOfBirth, 120) AS DateOfBirth, Email ,Phone ,Photo FROM Student";
                DataTable dt = prv.Select(CommandType.Text, strSql);
                return new JsonResult(dt);
            }                                      
            catch (Exception)                      
            {                                      
                throw;                             
            }                                                                           
        }

        [HttpGet("{ID}")]
        public JsonResult Get(int ID)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT StudentID, FirstName, LastName, Class ,Section ,Gender ,CONVERT(VARCHAR(10), DateOfBirth, 120) AS DateOfBirth, Email ,Phone ,Photo FROM Student WHERE StudentID = @ID";
                DataTable dt = prv.Select(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = ID});
                return new JsonResult(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Student std)
        {
            try
            {
                int result = 0;
                Provider prv = new Provider();
                string strSql = "INSERT INTO Student(FirstName, LastName, Class ,Section ,Gender ,DateOfBirth , Email ,Phone ,Photo) VALUES (@FirstName, @LastName ,@Class ,@Section ,@Gender ,@DateOfBirth ,@Email ,@Phone ,@Photo)";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@FirstName", Value = std.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = std.LastName },
                    new SqlParameter { ParameterName = "@Class", Value = std.Class },
                    new SqlParameter { ParameterName = "@Section", Value = std.Section },
                    new SqlParameter { ParameterName = "@Gender", Value = std.Gender },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = std.DateOfBirth },
                    new SqlParameter { ParameterName = "@Email", Value = std.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = std.Phone },
                    new SqlParameter { ParameterName = "@Photo", Value = std.Photo });
                if (result == 1)
                    return new JsonResult("Your data has been added.");
                else
                    throw new Exception("Your data has not been supplied!!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public JsonResult Put(Student std)
        {
            try
            {
                int result = 0;
                Provider prv = new Provider();
                string strSql = "UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Class = @Class, Section = @Section, Gender = @Gender, DateOfBirth = @DateOfBirth, Email = @Email, Phone = @Phone, Photo = @Photo WHERE StudentID = @StudentID";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@StudentID", Value = std.StudentID },
                    new SqlParameter { ParameterName = "@FirstName", Value = std.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = std.LastName },
                    new SqlParameter { ParameterName = "@Class", Value = std.Class },
                    new SqlParameter { ParameterName = "@Section", Value = std.Section },
                    new SqlParameter { ParameterName = "@Gender", Value = std.Gender },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = std.DateOfBirth },
                    new SqlParameter { ParameterName = "@Email", Value = std.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = std.Phone },
                    new SqlParameter { ParameterName = "@Photo", Value = std.Photo });
                if (result == 1)
                    return new JsonResult("Your data has been updated.");
                else
                    throw new Exception("Your data has not been supplied!!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            try
            {
                int result = 0;
                Provider prv = new Provider();
                string strSql = "DELETE FROM Student WHERE StudentID = @StudentID";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@StudentID", Value = ID});
                if (result == 1)
                    return new JsonResult("Your data has been deleted.");
                else
                    throw new Exception("Wrong ID or ID was deleted before!!");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var HttpRequest = Request.Form;
                var postedFile = HttpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = Web.ContentRootPath + "/Photos/Students/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }                                              
}
