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
    public class BookController : ControllerBase
    {
        private readonly IWebHostEnvironment web;

        public BookController(IWebHostEnvironment web)
        {
            this.web = web;
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT ID, Name, CategoryName, Description, IsActive, CONVERT(VARCHAR(10), BookLoanDay, 120) AS BookLoanDay, CONVERT(VARCHAR(10), BookReturnDay, 120) AS BookReturnDay, StudentName, Photo FROM Book";
                DataTable dt = prv.Select(CommandType.Text, strSql);
                return new JsonResult(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{CategoryName}")]
        public JsonResult Get(string CategoryName)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT ID, Name, CategoryName, Description, IsActive, CONVERT(VARCHAR(10), BookLoanDay, 120) AS BookLoanDay, CONVERT(VARCHAR(10), BookReturnDay, 120) AS BookReturnDay, StudentName, Photo FROM Book WHERE CategoryName = @CategoryName";
                DataTable dt = prv.Select(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@CategoryName", Value = CategoryName});
                return new JsonResult(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Book book)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "INSERT INTO Book(Name, CategoryName, Description, IsActive, BookLoanDay, BookReturnDay, StudentName, Photo) VALUES(@Name, @CategoryName, @Description, @IsActive, @BookLoanDay, @BookReturnDay, @StudentName, @Photo)";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@Name", Value = book.Name},
                    new SqlParameter { ParameterName = "@CategoryName", Value = book.CategoryName },
                    new SqlParameter { ParameterName = "@Description", Value = book.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = book.IsActive },
                    new SqlParameter { ParameterName = "@BookLoanDay", Value = book.BookLoanDay },
                    new SqlParameter { ParameterName = "@BookReturnDay", Value = book.BookReturnDay },
                    new SqlParameter { ParameterName = "@StudentName", Value = book.StudentName },
                    new SqlParameter { ParameterName = "@Photo", Value = book.Photo });
                if(result == 1)
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
        public JsonResult Put(Book book)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "UPDATE Book SET Name = @Name, CategoryName = @CategoryName, Description = @Description, IsActive = @IsActive, BookLoanDay = @BookLoanDay, BookReturnDay = @BookReturnDay, StudentName = @StudentName, Photo = @Photo WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = book.ID },
                    new SqlParameter { ParameterName = "@Name", Value = book.Name },
                    new SqlParameter { ParameterName = "@CategoryName", Value = book.CategoryName },
                    new SqlParameter { ParameterName = "@Description", Value = book.Description },
                    new SqlParameter { ParameterName = "@IsActive", Value = book.IsActive },
                    new SqlParameter { ParameterName = "@BookLoanDay", Value = book.BookLoanDay },
                    new SqlParameter { ParameterName = "@BookReturnDay", Value = book.BookReturnDay },
                    new SqlParameter { ParameterName = "@StudentName", Value = book.StudentName },
                    new SqlParameter { ParameterName = "@Photo", Value = book.Photo });
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

        [HttpDelete("{id}")]
        public JsonResult Delete(int ID)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "DELETE FROM Book WHERE ID = @ID";
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
                var physicalPath = web.ContentRootPath + "/Photos/" + fileName;

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
