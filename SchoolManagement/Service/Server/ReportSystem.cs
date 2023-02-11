using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Collections;
using System.Text.Json;

namespace SchoolManagement.Service.Server
{
    public class ReportSystem : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public ReportSystem(IConfiguration config, IHttpClientFactory httpClient, SweetAlertService swal)
        {
            this.config = config;
            this.httpClient = httpClient;
            this.swal = swal;
        }

        public ReportSystem() {

        }

        public IEnumerable<ReportDTO> reports = Array.Empty<ReportDTO>();
        public IEnumerable<StudentDTO> students = Array.Empty<StudentDTO>(); 
        
        public ReportData rep = new ReportData();
        private string errorMessage { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string FirstName { get; set; }

        [Parameter]
        public string LastName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshList();
        }



        protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "report");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                reports = await JsonSerializer.DeserializeAsync<IEnumerable<ReportDTO>>(responseStream);
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From ReportSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected async Task CreateClick()
        {
            try
            {
                var report = new ReportDTO { Classroom = rep.Classroom, SubjectName = rep.SubjectName, Letter = rep.Letter, Point = rep.Point, StudentID = rep.StudentID };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "report");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(report), null, "application/json");
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
                await swal.FireAsync("Error!", "From ReportSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected async Task UpdateClick()
        {
            try
            {
                var report = new ReportDTO { ID = rep.ID, Classroom = rep.Classroom, SubjectName = rep.SubjectName, Letter = rep.Letter, Point = rep.Point, StudentID = rep.StudentID };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "report");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(report), null, "application/json");
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
                await swal.FireAsync("Error!", "From ReportSystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "report/" + ID);
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
                await swal.FireAsync("Error!", "From ReportSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected void AddClick()
        {
            rep.Title = "Add New Report";
            rep.Classroom = string.Empty;
            rep.SubjectName = string.Empty;
            rep.Letter = string.Empty;
            rep.Point = 0;
            rep.StudentID = 0;
        }

        protected void EditClick(ReportDTO report)
        {
            rep.Title = "Update Report";
            rep.Classroom = report.Classroom;
            rep.SubjectName = report.SubjectName;
            rep.Letter = report.Letter;
            rep.Point = report.Point;
            rep.StudentID = report.StudentID;
        }
    }
}
