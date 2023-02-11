using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service.Server
{
    public class SubjectSystem : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        public SweetAlertService swal { get; set; }

        public SubjectSystem(IConfiguration config, IHttpClientFactory httpClient, SweetAlertService swal) 
        {
            this.config = config;
            this.httpClient = httpClient;
            this.swal = swal;
        }

        public SubjectSystem()
        {

        }

        public IEnumerable<SubjectDTO> subjects = Array.Empty<SubjectDTO>();
        public SubjectData sub = new SubjectData();

        private string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RefreshList();
        }

        protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "subject");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                subjects = await JsonSerializer.DeserializeAsync<IEnumerable<SubjectDTO>>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From SubjectSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected async Task CreateClick()
        {
            try
            {
                var subject = new SubjectDTO { Name = sub.Name, Credits = sub.Credits, StartTime = sub.StartTime.ToString("HH:mm"), FinishTime = sub.FinishTime.ToString("HH:mm"), SchoolDay = sub.SchoolDay, Teacher = sub.Teacher, Classroom = sub.Classroom };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "subject");
                request.Content = new StringContent(JsonSerializer.Serialize(subject), null, "application/json");
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
                await swal.FireAsync("Error!", "From SubjectSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected async Task UpdateClick()
        {
            try
            {
                var subject = new SubjectDTO { ID = sub.ID, Name = sub.Name, Credits = sub.Credits, StartTime = sub.StartTime.ToString("HH:mm"), FinishTime = sub.FinishTime.ToString("HH:mm"), SchoolDay = sub.SchoolDay, Teacher = sub.Teacher, Classroom = sub.Classroom };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "subject");
                request.Content = new StringContent(JsonSerializer.Serialize(subject), null, "application/json");
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
                await swal.FireAsync("Error!", "From SubjectSystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "subject/" + ID);
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
                await swal.FireAsync("Error!", "From SubjectSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected void AddClick()
        {
            sub.Title = "Add New Subject";
            sub.ID = 0;
            sub.Name = string.Empty;
            sub.Credits = 0;
            sub.StartTime = DateTime.Now;
            sub.FinishTime = DateTime.Now;
            sub.SchoolDay = string.Empty;
            sub.Teacher = string.Empty;
            sub.Classroom = string.Empty;
        }

        protected void EditClick(SubjectDTO subject)
        {
            sub.Title = "Update Subject";
            sub.ID = subject.ID;
            sub.Name = subject.Name;
            sub.Credits = subject.Credits;
            sub.StartTime = Convert.ToDateTime(subject.StartTime);
            sub.FinishTime = Convert.ToDateTime(subject.FinishTime);
            sub.SchoolDay = subject.SchoolDay;
            sub.Teacher = subject.Teacher;
            sub.Classroom = subject.Classroom;
        }
    }
}
