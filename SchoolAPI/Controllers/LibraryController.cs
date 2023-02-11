using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Connect;
using SchoolAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IWebHostEnvironment web;

        public LibraryController(IWebHostEnvironment web)
        {
            this.web = web;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT * FROM Library";
                DataTable dt = prv.Select(CommandType.Text, strSql);
                return new JsonResult(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Library lib)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "INSERT INTO Library(CategoryName, Photo, Description) VALUES (@CategoryName, @Photo, @Description)";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@CategoryName", Value = lib.CategoryName},
                    new SqlParameter { ParameterName = "@Photo", Value = lib.Photo },
                    new SqlParameter { ParameterName = "@Description", Value = lib.Description });
                if (result == 1)
                    return new JsonResult("Your data has been added.");
                else
                    throw new Exception("Your data has not been supplied");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        public JsonResult Put(Library lib)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "UPDATE Library SET CategoryName = @CategoryName, Photo = @Photo, Description = @Description WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = lib.ID },
                    new SqlParameter { ParameterName = "@CategoryName", Value = lib.CategoryName },
                    new SqlParameter { ParameterName = "@Photo", Value = lib.Photo },
                    new SqlParameter { ParameterName = "@Description", Value = lib.Description });
                if (result == 1)
                    return new JsonResult("Your data has been updated.");
                else
                    throw new Exception("Your data has not been supplied");
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
                Provider prv = new Provider();
                string strSql = "DELETE FROM Library WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = ID });
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
                var physicalPath = web.ContentRootPath + "/Photos/Libraries/" + fileName;

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
