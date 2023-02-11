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
    public class TeacherSystem : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IJSRuntime jS { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public TeacherSystem(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS, SweetAlertService swal)
        {
            this.config = config;
            this.httpClient = httpClient;
            jS = JS;
            this.swal = swal;
        }

        public TeacherSystem()
        {

        }

        public IEnumerable<TeacherDTO> teachers = Array.Empty<TeacherDTO>();
        public IEnumerable<TeacherDTO> another = Array.Empty<TeacherDTO>();
        public TeacherData tc = new TeacherData();

        private string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tc.Path = config["PHOTOS_URL"] + "/Teachers/";
            tc.Photo = "anonymous.png";
            await RefreshList();
        }

		protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "teacher");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                another = await JsonSerializer.DeserializeAsync<IEnumerable<TeacherDTO>>(responseStream);
                teachers = another;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

		protected async Task CreateClick()
        {
            try
            {
                var teacher = new TeacherDTO { FirstName = tc.FirstName, LastName = tc.LastName, DateOfBirth = tc.DateOfBirth.ToString("yyyy-MM-dd"), Gender = tc.Gender, Experience = tc.Experience, Email = tc.Email, Phone = tc.Phone, Photo = tc.Photo };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "teacher");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(teacher), null, "application/json");
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
                await swal.FireAsync("Error!", "From TeacherSystem.cs: " + errorMessage, SweetAlertIcon.Error);
            } 
        }

		protected async Task UpdateClick()
        {
            try
            {
                var teacher = new TeacherDTO { TeacherID = tc.TeacherID, FirstName = tc.FirstName, LastName = tc.LastName, DateOfBirth = tc.DateOfBirth.ToString("yyyy-MM-dd"), Gender = tc.Gender, Experience = tc.Experience, Email = tc.Email, Phone = tc.Phone, Photo = tc.Photo };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "teacher");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(teacher), null, "application/json");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();

                string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
                await swal.FireAsync("Successfully!", res, SweetAlertIcon.Success);
                await RefreshList();
            }
            catch (Exception ex)
            {
                errorMessage= ex.Message;
                await swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "teacher/" + ID);
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
                await swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

		protected void AddClick()
        {
            tc.Title = "Add New Teacher";
            tc.TeacherID = 0;
            string.IsNullOrWhiteSpace(tc.FirstName);
            string.IsNullOrWhiteSpace(tc.LastName);
            tc.DateOfBirth = DateTime.Now;
            string.IsNullOrWhiteSpace(tc.Gender);
            string.IsNullOrWhiteSpace(tc.Experience);
            string.IsNullOrWhiteSpace(tc.Email);
            string.IsNullOrWhiteSpace(tc.Phone);
            tc.Photo = "anonymous.png";
        }

		protected void EditClick(TeacherDTO teacher)
        {
            tc.Title = "Update Teacher";
            tc.TeacherID = teacher.TeacherID;
            tc.FirstName = teacher.FirstName;
            tc.LastName = teacher.LastName;
            tc.DateOfBirth = Convert.ToDateTime(teacher.DateOfBirth);
            tc.Gender = teacher.Gender;
            tc.Experience = teacher.Experience;
            tc.Email = teacher.Email;
            tc.Phone = teacher.Phone;
            tc.Photo = teacher.Photo;
        }

        protected async Task UploadFile(IFileListEntry[] files)
        {
            try
            {
                var file = files.FirstOrDefault();
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var content = new MultipartFormDataContent { { new ByteArrayContent(ms.GetBuffer()), "\"file\"", file.Name } };

                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "Teacher/SaveFile");
                request.Content = content;

                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                tc.Photo = await JsonSerializer.DeserializeAsync<string>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
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

                xlsxSheet["A1"].Value = "Teacher ID";
                xlsxSheet["B1"].Value = "First Name";
                xlsxSheet["C1"].Value = "Last Name";
                xlsxSheet["D1"].Value = "Date Of Birth";
                xlsxSheet["E1"].Value = "Gender";
                xlsxSheet["F1"].Value = "Experience";
                xlsxSheet["G1"].Value = "Email";
                xlsxSheet["H1"].Value = "Phone";

                xlsxSheet["A1:H1"].Style.Font.Bold = true;

                int index = 2;

                foreach (var obj in teachers)
                {
                    xlsxSheet["A" + index.ToString()].Value = obj.TeacherID.ToString();
                    xlsxSheet["B" + index.ToString()].Value = obj.FirstName;
                    xlsxSheet["C" + index.ToString()].Value = obj.LastName;
                    xlsxSheet["D" + index.ToString()].Value = obj.DateOfBirth;
                    xlsxSheet["E" + index.ToString()].Value = obj.Gender;
                    xlsxSheet["F" + index.ToString()].Value = obj.Experience;
                    xlsxSheet["G" + index.ToString()].Value = obj.Email;
                    xlsxSheet["H" + index.ToString()].Value = obj.Phone;
                    index++;
                }

                fileContents = xlsxWorkbook.ToByteArray();
                jS.InvokeAsync<TeacherSystem>(
                    "saveAsExcelFile",
                    "Teacher.xlxs",
                    Convert.ToBase64String(fileContents)
                );
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected void PdfExport()
        {
            try
            {
                string fileName = "Teacher.pdf";
                var Renderer = new IronPdf.ChromePdfRenderer();
                var pdf = Renderer.RenderUrlAsPdf("https://localhost:7095/teacher");
                jS.InvokeAsync<TeacherSystem>("saveAsPdfFile", fileName, Convert.ToBase64String(pdf.Stream.ToArray()));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                swal.FireAsync("Error!", "From TeacherSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        public string Name = "";
        public void FindByName()
        {
            teachers = Array.Empty<TeacherDTO>();
            teachers = another.Where(
                c => c.FirstName.ToLower().Contains(Name.ToLower()) || c.LastName.ToLower().Contains(Name.ToLower())
            );
        }
    }
}
