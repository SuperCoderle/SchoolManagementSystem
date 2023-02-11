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
    public class ReportController : ControllerBase
    {
        public ReportController() { }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT ID, Classroom, SubjectName, Letter, Point, StudentID FROM Report";
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
                string strSql = "SELECT ID, Classroom, SubjectName, Letter, Point, StudentID FROM Report WHERE StudentID = @ID";
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
        public JsonResult Post(Report rep) 
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "INSERT INTO Report(Classroom, SubjectName, Letter, Point) VALUES(@Classroom, @SubjectName, @Letter, @Point)";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@Classroom", Value = rep.Classroom },
                    new SqlParameter { ParameterName = "@SubjectName", Value = rep.SubjectName },
                    new SqlParameter { ParameterName = "@Letter", Value = rep.Letter },
                    new SqlParameter { ParameterName = "@Point", Value = rep.Point });
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
        public JsonResult Put(Report rep)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "UPDATE Report SET Classroom = @Classroom, SubjectName = @SubjectName, Letter = @Letter, Point = @Point WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = rep.ID},
                    new SqlParameter { ParameterName = "@Classroom", Value = rep.Classroom },
                    new SqlParameter { ParameterName = "@SubjectName", Value = rep.SubjectName },
                    new SqlParameter { ParameterName = "@Letter", Value = rep.Letter },
                    new SqlParameter { ParameterName = "@Point", Value = rep.Point });
                if (result == 1)
                    return new JsonResult("Your data has been update.");
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
                Provider prv = new Provider();
                string strSql = "DELETE FROM Report WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = ID });
                if (result == 1)
                    return new JsonResult("Your data has been deleted.");
                else
                    throw new Exception("Wrong ID or ID has not been supplied!!");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
