using BlazorInputFile;
using CurrieTechnologies.Razor.SweetAlert2;
using IronXL;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service
{
	public class ExamSystem : ComponentBase
	{
		[Inject]
		private IConfiguration config { get; set; }

		[Inject]
		private IHttpClientFactory httpClient { get; set; }

		[Inject]
		private IJSRuntime jS { get; set; }

		[Inject]
		private SweetAlertService swal { get; set; }

		public ExamSystem(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS, SweetAlertService swal)
		{
			this.config = config;
			this.httpClient = httpClient;
			jS = JS;
			this.swal = swal;
		}

		public ExamSystem()
		{

		}

		public IEnumerable<ExamDTO> exams = Array.Empty<ExamDTO>();
		public IEnumerable<ExamDTO> another = Array.Empty<ExamDTO>();
		public ExamData examination = new ExamData();
		public QuestionData question = new QuestionData();
		private string errorMessage { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await RefreshList();
		}

		protected async Task RefreshList()
		{
			try
			{
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "exam");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                another = await JsonSerializer.DeserializeAsync<IEnumerable<ExamDTO>>(responseStream);
                exams = another;
            }
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task CreateClick()
		{
			try
			{
                var exam = new ExamDTO { Title = examination.Title, Description = examination.Description, Type = examination.Type, Score = examination.Score, TimeStart = examination.TimeStart.ToString("HH:mm"), TimeFinish = examination.TimeFinish.ToString("HH:mm"), NumberQuest = examination.NumberQuest, Coef = examination.Coef };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "exam");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(exam), null, "application/json");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();

                string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                await swal.FireAsync("Successfully", res, SweetAlertIcon.Success);
                await RefreshList();
            }
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task CreateClickQuestion()
		{
			try
			{
				var quest = new QuestionDTO { Question = question.Question, Type = question.Type, RightAnswer = question.RightAnswer, WrongAnswer1 = question.WrongAnswer1, WrongAnswer2 = question.WrongAnswer2, WrongAnswer3 = question.WrongAnswer3 };
				var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "Question");
				request.Headers.Add("Accept", "application/json");
				request.Content = new StringContent(JsonSerializer.Serialize(quest), null, "application/json");
				var client = httpClient.CreateClient();
				var response = await client.SendAsync(request);
				using var responseStream = await response.Content.ReadAsStreamAsync();

				string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
				await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
			}
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
			
		}

		protected async Task UpdateClick()
		{
			try
			{
                var exam = new ExamDTO { ExamID = examination.ExamID, Title = examination.Title, Description = examination.Description, Type = examination.Type, Score = examination.Score, TimeStart = examination.TimeStart.ToString("HH:mm"), TimeFinish = examination.TimeFinish.ToString("HH:mm"), NumberQuest = examination.NumberQuest, Coef = examination.Coef };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "exam");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(exam), null, "application/json");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();

                string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                await RefreshList();
            }
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task DeleteClick(int ID)
		{
			try
			{
                SweetAlertResult result = await swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Are you sure?",
                    Text = "Your data's going to be deleted, you'll not be able to recover it!",
                    ShowCancelButton = true,
                    ConfirmButtonText = "Yes, i want to delete it.",
                    CancelButtonText = "No, i'll think again."
                });
                if (!string.IsNullOrEmpty(result.Value))
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "exam/" + ID);
                    var client = httpClient.CreateClient();
                    var response = await client.SendAsync(request);
                    using var responseStream = await response.Content.ReadAsStreamAsync();

                    string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                    await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                }
                else if (result.Dismiss == DismissReason.Cancel)
                {
                    await swal.FireAsync("Cancelled!", "Your data is safe.", SweetAlertIcon.Error);
                }
                await RefreshList();
            }
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected void AddClick()
		{
			examination.Modal_Title = "Add New Exam";
			examination.Title = "";
			examination.Description = "";
			examination.Type = "";
			examination.Score = 0;
			examination.TimeStart = DateTime.Now;
			examination.TimeFinish = DateTime.Now;
			examination.NumberQuest = 0;
			examination.Coef = 0;
			question.Title = "Add New Question";
			question.ID = 0;
			question.Question = "";
			question.Type = string.Empty;
			question.RightAnswer = "";
			question.WrongAnswer1 = "";
			question.WrongAnswer2 = "";
			question.WrongAnswer3 = "";
		}

		protected void EditClick(ExamDTO exam)
		{
			examination.Modal_Title = "Update Exam";
			examination.ExamID = exam.ExamID;
			examination.Title = exam.Title;
			examination.Description = exam.Description;
			examination.Type = exam.Type;
			examination.Score = exam.Score;
			examination.TimeStart = Convert.ToDateTime(exam.TimeStart);
			examination.TimeFinish = Convert.ToDateTime(exam.TimeFinish);
			examination.NumberQuest = exam.NumberQuest;
			examination.Coef = exam.Coef;
		}

		protected void ExcelExport()
		{
			try
			{
				byte[] fileContents;
				WorkBook xlsxWorkbook = WorkBook.Create(IronXL.ExcelFileFormat.XLSX);
				xlsxWorkbook.Metadata.Author = "IronXL";

				WorkSheet xlsxSheet = xlsxWorkbook.CreateWorkSheet("new_sheet");

				xlsxSheet["A1"].Value = "Exam ID";
				xlsxSheet["B1"].Value = "Title";
				xlsxSheet["C1"].Value = "Description";
				xlsxSheet["D1"].Value = "Type";
				xlsxSheet["E1"].Value = "Score";
				xlsxSheet["F1"].Value = "Time Start";
				xlsxSheet["G1"].Value = "Time Finish";
				xlsxSheet["H1"].Value = "Number Question";
				xlsxSheet["I1"].Value = "Coef";

				xlsxSheet["A1:I1"].Style.Font.Bold = true;

				int index = 2;

				foreach (var obj in exams)
				{
					xlsxSheet["A" + index.ToString()].Value = obj.ExamID.ToString();
					xlsxSheet["B" + index.ToString()].Value = obj.Title;
					xlsxSheet["C" + index.ToString()].Value = obj.Description;
					xlsxSheet["D" + index.ToString()].Value = obj.Title;
					xlsxSheet["E" + index.ToString()].Value = obj.Score.ToString();
					xlsxSheet["F" + index.ToString()].Value = obj.TimeStart;
					xlsxSheet["G" + index.ToString()].Value = obj.TimeFinish;
					xlsxSheet["H" + index.ToString()].Value = obj.NumberQuest.ToString();
					xlsxSheet["I" + index.ToString()].Value = obj.Coef.ToString();
					index++;
				}

				fileContents = xlsxWorkbook.ToByteArray();
				jS.InvokeAsync<ExamSystem>(
					"saveAsExcelFile",
					"Examinations.xlxs",
					Convert.ToBase64String(fileContents)
				);
			}
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected void PdfExport()
		{
			try
			{
				string fileName = "Examination.pdf";
				var Renderer = new IronPdf.ChromePdfRenderer();
				var pdf = Renderer.RenderUrlAsPdf("https://localhost:7095/exam");
				jS.InvokeAsync<ExamSystem>("saveAsPdfFile", fileName, Convert.ToBase64String(pdf.Stream.ToArray()));
			}
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From ExamSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		public string Name = "";
		public void FindByName()
		{
			exams = Array.Empty<ExamDTO>();
			exams = another.Where(
				c => c.Title.ToLower().Contains(Name.ToLower()) || c.Type.ToLower().Contains(Name.ToLower())
			);
		}
	}
}
