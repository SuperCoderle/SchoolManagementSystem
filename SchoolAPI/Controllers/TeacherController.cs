using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Connect;
using System.Data.SqlClient;
using System.Data;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IWebHostEnvironment web;

        public TeacherController(IWebHostEnvironment web)
        {
            this.web = web;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT TeacherID, FirstName, LastName, CONVERT(VARCHAR(10), DateOfBirth, 120) AS DateOfBirth, Gender , Experience, Email ,Phone ,Photo FROM Teacher";
                DataTable dt = prv.Select(CommandType.Text, strSql);
                return new JsonResult(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Teacher tc)
        {
            try
            {
                int result = 0;
                Provider prv = new Provider();
                string strSql = "INSERT INTO Teacher(FirstName, LastName, DateOfBirth , Gender , Experience, Email ,Phone ,Photo) VALUES (@FirstName, @LastName ,@DateOfBirth ,@Gender ,@Experience ,@Email ,@Phone ,@Photo)";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@FirstName", Value = tc.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = tc.LastName },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = tc.DateOfBirth },
                    new SqlParameter { ParameterName = "@Gender", Value = tc.Gender },
                    new SqlParameter { ParameterName = "@Experience", Value = tc.Experience },
                    new SqlParameter { ParameterName = "@Email", Value = tc.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = tc.Phone },
                    new SqlParameter { ParameterName = "@Photo", Value = tc.Photo });
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
        public JsonResult Put(Teacher tc)
        {
            try
            {
                int result = 0;
                Provider prv = new Provider();
                string strSql = "UPDATE Teacher SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Gender = @Gender, Experience = @Experience, Email = @Email, Phone = @Phone, Photo = @Photo WHERE TeacherID = @TeacherID";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@TeacherID", Value = tc.TeacherID },
                    new SqlParameter { ParameterName = "@FirstName", Value = tc.FirstName },
                    new SqlParameter { ParameterName = "@LastName", Value = tc.LastName },
                    new SqlParameter { ParameterName = "@DateOfBirth", Value = tc.DateOfBirth },
                    new SqlParameter { ParameterName = "@Gender", Value = tc.Gender },
                    new SqlParameter { ParameterName = "@Experience", Value = tc.Experience },
                    new SqlParameter { ParameterName = "@Email", Value = tc.Email },
                    new SqlParameter { ParameterName = "@Phone", Value = tc.Phone },
                    new SqlParameter { ParameterName = "@Photo", Value = tc.Photo });
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
                string strSql = "DELETE FROM Teacher WHERE TeacherID = @TeacherID";
                result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@TeacherID", Value = ID });
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
                var physicalPath = web.ContentRootPath + "/Photos/Teachers/" + fileName;

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
