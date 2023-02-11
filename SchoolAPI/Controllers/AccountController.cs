using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Connect;
using SchoolAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace SchoolAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{

		[HttpGet("{Username}")]
		public JsonResult Get(string Username)
		{
			try
			{
				Provider prv = new Provider();
				string strSql = ("SELECT * FROM Account WHERE Username LIKE @Username");
				DataTable dt = prv.Select(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@Username", Value = Username });
				return new JsonResult(dt);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpPost]
		public JsonResult Post(Account acc)
		{
			try
			{
				Provider prv = new Provider();
				string strSql = "INSERT INTO Account VALUES(@Username, @Password)";
				int result = prv.ExcuteNonQuery(CommandType.Text, strSql,
					new SqlParameter { ParameterName = "@Username", Value = acc.Username},
					new SqlParameter { ParameterName = "@Password", Value = acc.Password});
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
	}
}
