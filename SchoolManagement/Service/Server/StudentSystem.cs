using BlazorInputFile;
using CurrieTechnologies.Razor.SweetAlert2;
using IronXL;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Linq;
using System.Text.Json;

namespace SchoolManagement.Service
{
    public class StudentSystem : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IJSRuntime jS { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public StudentSystem(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS, SweetAlertService swal)
        {
            this.config = config;
            this.httpClient = httpClient;
            jS = JS;
            this.swal = swal;
        }

        public StudentSystem()
        {

        }

        public IEnumerable<StudentDTO> students = Array.Empty<StudentDTO>();
        public IEnumerable<StudentDTO> another = Array.Empty<StudentDTO>();
        public StudentData std = new StudentData();
        private string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            std.Path = config["PHOTOS_URL"] + "/Students/";
            await RefreshList();
        }

		protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "student");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                another = await JsonSerializer.DeserializeAsync<IEnumerable<StudentDTO>>(responseStream);
                students = another;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

		protected async Task CreateClick()
        {
            try
            {
                var student = new StudentDTO { FirstName = std.FirstName, LastName = std.LastName, Class = std.Class, Section = std.Section, Gender = std.Gender, DateOfBirth = std.DateOfBirth.ToString("HH:mm"), Email = std.Email, Phone = std.Phone, Photo = std.Photo };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "Student");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(student), null, "application/json");
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
                await swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }

        }

		protected async Task UpdateClick()
        {
            try
            {
                var student = new StudentDTO { StudentID = std.StudentID, FirstName = std.FirstName, LastName = std.LastName, Class = std.Class, Section = std.Section, Gender = std.Gender, DateOfBirth = std.DateOfBirth.ToString("HH:mm"), Email = std.Email, Phone = std.Phone, Photo = std.Photo };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "student");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(student), null, "application/json");
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
                await swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "student/" + ID);
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
                await swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

		protected void AddClick()
        {
            std.Title = "Add New Student";
            std.StudentID = 0;
            string.IsNullOrWhiteSpace(std.FirstName);
            string.IsNullOrWhiteSpace(std.LastName);
            string.IsNullOrWhiteSpace(std.Class);
            string.IsNullOrWhiteSpace(std.Section);
            string.IsNullOrWhiteSpace(std.Gender);
            std.DateOfBirth = DateTime.Now;
            string.IsNullOrWhiteSpace(std.Email);
            string.IsNullOrWhiteSpace(std.Phone);
            std.Photo = "anonymous.png";
        }

		protected void EditClick(StudentDTO student)
        {
            std.Title = "Update Student";
            std.StudentID = student.StudentID;
            std.FirstName = student.FirstName;
            std.LastName = student.LastName;
            std.Class = student.Class;
            std.Section = student.Section;
            std.Gender = student.Gender;
            std.DateOfBirth = Convert.ToDateTime(student.DateOfBirth);
            std.Email = student.Email;
            std.Phone = student.Phone;
            std.Photo = student.Photo;
        }

        protected async Task UploadFile(IFileListEntry[] files)
        {
            try
            {
                var file = files.FirstOrDefault();
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var content = new MultipartFormDataContent { { new ByteArrayContent(ms.GetBuffer()), "\"file\"", file.Name } };

                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "Student/SaveFile");
                request.Content = content;

                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                std.Photo = await JsonSerializer.DeserializeAsync<string>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected void ExcelExport()
        {
            try
            {
				byte[] fileContents;
				WorkBook xlsxWorkbook = WorkBook.Create(IronXL.ExcelFileFormat.XLSX);
				xlsxWorkbook.Metadata.Author = "IronXL";

				WorkSheet xlsxSheet = xlsxWorkbook.CreateWorkSheet("new_sheet");

				xlsxSheet["A1"].Value = "Student ID";
				xlsxSheet["B1"].Value = "First Name";
				xlsxSheet["C1"].Value = "Last Name";
				xlsxSheet["D1"].Value = "Class";
				xlsxSheet["E1"].Value = "Section";
				xlsxSheet["F1"].Value = "Gender";
				xlsxSheet["G1"].Value = "Date Of Birth";
				xlsxSheet["H1"].Value = "Email";
				xlsxSheet["I1"].Value = "Phone";

				xlsxSheet["A1:I1"].Style.Font.Bold = true;

				int index = 2;

				foreach (var obj in students)
				{
					xlsxSheet["A" + index.ToString()].Value = obj.StudentID.ToString();
					xlsxSheet["B" + index.ToString()].Value = obj.FirstName;
					xlsxSheet["C" + index.ToString()].Value = obj.LastName;
					xlsxSheet["D" + index.ToString()].Value = obj.Class;
					xlsxSheet["E" + index.ToString()].Value = obj.Section;
					xlsxSheet["F" + index.ToString()].Value = obj.Gender;
					xlsxSheet["G" + index.ToString()].Value = obj.DateOfBirth;
					xlsxSheet["H" + index.ToString()].Value = obj.Email;
					xlsxSheet["I" + index.ToString()].Value = obj.Phone;
					index++;
				}

				fileContents = xlsxWorkbook.ToByteArray();
				jS.InvokeAsync<StudentSystem>(
					"saveAsExcelFile",
					"Students.xlxs",
					Convert.ToBase64String(fileContents)
				);
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

        protected void PdfExport()
        {
            try
            {
                string fileName = "Student.pdf";
                var Renderer = new IronPdf.ChromePdfRenderer();
                var pdf = Renderer.RenderUrlAsPdf("https://localhost:7095/student");
                jS.InvokeAsync<StudentSystem>("saveAsPdfFile", fileName, Convert.ToBase64String(pdf.Stream.ToArray()));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From StudentSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        public string Name = "";
        public void FindByName()
        {
            students = Array.Empty<StudentDTO>();
            students = another.Where(
                c => c.FirstName.ToLower().Contains(Name.ToLower()) || c.LastName.ToLower().Contains(Name.ToLower())
            );
        }
    }
}
