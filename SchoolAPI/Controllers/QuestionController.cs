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
	public class QuestionController : ControllerBase
	{
		public QuestionController() 
		{
		
		}

		[HttpGet]
		public JsonResult Get()
		{
			try
			{
				Provider prv = new Provider();
				string strSql = "SELECT Q.ID, Q.Question, E.Type, Q.RightAnswer, Q.WrongAnswer1, Q.WrongAnswer2, Q.WrongAnswer3 FROM Question AS Q JOIN Exam AS E ON Q.Type = E.Type";
				DataTable dt = prv.Select(CommandType.Text, strSql);
				return new JsonResult(dt);
			}
			catch (Exception)
			{ 
				throw;
			}
		}

		[HttpPost]
		public JsonResult Post(Quest quest)
		{
			try
			{
				Provider prv = new Provider();
				string strSql = "INSERT INTO Question(Question, Type, RightAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3) VALUES(@Question, @Type, @RightAnswer, @WrongAnswer1, @WrongAnswer2, @WrongAnswer3)";
				int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@Question", Value = quest.Question},
					new SqlParameter { ParameterName = "@Type", Value = quest.Type },
					new SqlParameter { ParameterName = "@RightAnswer", Value = quest.RightAnswer },
					new SqlParameter { ParameterName = "@WrongAnswer1", Value = quest.WrongAnswer1 },
					new SqlParameter { ParameterName = "@WrongAnswer2", Value = quest.WrongAnswer2 },
					new SqlParameter { ParameterName = "@WrongAnswer3", Value = quest.WrongAnswer3});
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
		public JsonResult Put(Quest quest)
		{
			try
			{
				Provider prv = new Provider();
				string strSql = "UPDATE Question SET Question = @Question, Type = @Type, RightAnswer = @RightAnswer, WrongAnswer1 = @WrongAnswer1, WrongAnswer2 = @WrongAnswer2, WrongAnswer3 = @WrongAnswer3 WHERE ID = @ID";
				int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@ID", Value = quest.ID},
					new SqlParameter { ParameterName = "@Question", Value = quest.Question },
					new SqlParameter { ParameterName = "@Type", Value = quest.Type },
					new SqlParameter { ParameterName = "@RightAnswer", Value = quest.RightAnswer },
					new SqlParameter { ParameterName = "@WrongAnswer1", Value = quest.WrongAnswer1 },
					new SqlParameter { ParameterName = "@WrongAnswer2", Value = quest.WrongAnswer2 },
					new SqlParameter { ParameterName = "@WrongAnswer3", Value = quest.WrongAnswer3 });
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
				string strSql = "DELETE FROM Question WHERE ID = @ID";
				int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@Type", Value = ID });
				if(result == 1)
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
