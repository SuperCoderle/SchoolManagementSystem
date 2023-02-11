using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using SchoolDTOS;
using CurrieTechnologies.Razor.SweetAlert2;

namespace SchoolManagement.Service.Client
{
    public class Dashboard : ComponentBase
    {
        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IJSRuntime jS { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }


        public Dashboard(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS, SweetAlertService swal)
        {
            this.config = config;
            this.httpClient = httpClient;
            jS = JS;
            this.swal = swal;
        }

        public Dashboard()
        {

        }

        public IEnumerable<ExamDTO> exams = Array.Empty<ExamDTO>();
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
                exams = await JsonSerializer.DeserializeAsync<IEnumerable<ExamDTO>>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From Dashboard.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }
    }
}
