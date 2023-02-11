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
    public class SubjectController : ControllerBase
    {
        public SubjectController() 
        {
        
        }

        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "SELECT ID, Name, Credits, CONVERT(VARCHAR(5), StartTime, 108) AS StartTime, CONVERT(VARCHAR(5), FinishTime, 108) AS FinishTime, SchoolDay, Teacher, Classroom FROM Subject";
                DataTable dt = prv.Select(CommandType.Text, strSql);
                return new JsonResult(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Post(Subject sub) 
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "INSERT INTO Subject(Name, Credits, StartTime, FinishTime, SchoolDay, Teacher, Classroom) VALUES(@Name, @Credits, @StartTime, @FinishTime, @SchoolDay, @Teacher, @Classroom)";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@Name", Value = sub.Name },
                    new SqlParameter { ParameterName = "@Credits", Value = sub.Credits },
                    new SqlParameter { ParameterName = "@StartTime", Value = sub.StartTime },
                    new SqlParameter { ParameterName = "@FinishTime", Value = sub.FinishTime },
                    new SqlParameter { ParameterName = "@SchoolDay", Value = sub.SchoolDay },
                    new SqlParameter { ParameterName = "@Teacher", Value = sub.Teacher},
                    new SqlParameter { ParameterName = "@Classroom", Value = sub.Classroom });
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
        public JsonResult Put(Subject sub)
        {
            try
            {
                Provider prv = new Provider();
                string strSql = "UPDATE Subject SET Name = @Name, Credits = @Credits, StartTime = @StartTime, FinishTime = @FinishTime, SchoolDay = @SchoolDay, Teacher = @Teacher, Classroom = @Classroom WHERE ID = @ID";
                int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
                    new SqlParameter { ParameterName = "@ID", Value = sub.ID},
                    new SqlParameter { ParameterName = "@Name", Value = sub.Name },
                    new SqlParameter { ParameterName = "@Credits", Value = sub.Credits },
                    new SqlParameter { ParameterName = "@StartTime", Value = sub.StartTime },
                    new SqlParameter { ParameterName = "@FinishTime", Value = sub.FinishTime },
                    new SqlParameter { ParameterName = "@SchoolDay", Value = sub.SchoolDay },
                    new SqlParameter { ParameterName = "@Teacher", Value = sub.Teacher },
                    new SqlParameter { ParameterName = "@Classroom", Value = sub.Classroom });
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
                Provider prv = new Provider();
                string strSql = "DELETE FROM Subject WHERE ID = @ID";
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
