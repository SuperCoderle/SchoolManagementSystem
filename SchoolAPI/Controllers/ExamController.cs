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
	public class ExamController : ControllerBase
	{

		public ExamController()
		{

		}

		[HttpGet]
		public JsonResult Get()
		{
			try
			{
				Provider prv = new Provider();
				string strSql = "SELECT ExamID, Title, Description, Type, Score, CONVERT(VARCHAR(5), TimeStart, 108) AS TimeStart, CONVERT(VARCHAR(5), TimeFinish, 108) AS TimeFinish, NumberQuest, Coef FROM Exam";
				DataTable dt = prv.Select(CommandType.Text, strSql);
				return new JsonResult(dt);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpPost]
		public JsonResult Post(Exam exam)
		{
			try
			{
				int result = 0;
				Provider prv = new Provider();
				string strSql = "INSERT INTO Exam(Title, Description, Type, Score, TimeStart, TimeFinish, NumberQuest, Coef) VALUES (@Title, @Description, @Type, @Score, @TimeStart, @TimeFinish, @NumberQuest, @Coef)";
				result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@Title", Value = exam.Title },
					new SqlParameter { ParameterName = "@Description", Value = exam.Description },
					new SqlParameter { ParameterName = "@Type", Value = exam.Type },
					new SqlParameter { ParameterName = "@Score", Value = exam.Score },
					new SqlParameter { ParameterName = "@TimeStart", Value = exam.TimeStart },
					new SqlParameter { ParameterName = "@TimeFinish", Value = exam.TimeFinish },
					new SqlParameter { ParameterName = "@NumberQuest", Value = exam.NumberQuest },
					new SqlParameter { ParameterName = "@Coef", Value = exam.Coef });
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
		public JsonResult Put(Exam exam)
		{
			try
			{
				int result = 0;
				Provider prv = new Provider();
				string strSql = "UPDATE Exam SET Title = @Title, Description = @Description, Type = @Type, Score = @Score, TimeStart = @TimeStart, TimeFinish = @TimeFinish, NumberQuest = @NumberQuest, Coef = @Coef WHERE ExamID = @ExamID";
				result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@ExamID", Value = exam.ExamID },
					new SqlParameter { ParameterName = "@Title", Value = exam.Title },
					new SqlParameter { ParameterName = "@Description", Value = exam.Description },
					new SqlParameter { ParameterName = "@Type", Value = exam.Type },
					new SqlParameter { ParameterName = "@Score", Value = exam.Score },
					new SqlParameter { ParameterName = "@TimeStart", Value = exam.TimeStart },
					new SqlParameter { ParameterName = "@TimeFinish", Value = exam.TimeFinish },
					new SqlParameter { ParameterName = "@NumberQuest", Value = exam.NumberQuest },
					new SqlParameter { ParameterName = "@Coef", Value = exam.Coef });
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
				string strSql = "DELETE FROM Exam WHERE ExamID = @ExamID";
				result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@ExamID", Value = ID });
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
	}
}
