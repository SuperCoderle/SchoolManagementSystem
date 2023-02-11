using BlazorInputFile;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service.Client
{
    public class Library : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }


        public Library(IHttpClientFactory httpClient, IConfiguration config, SweetAlertService swal)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.swal = swal;
        }

        public Library()
        {

        }

        public IEnumerable<LibraryDTO> libraries = Array.Empty<LibraryDTO>();
        public LibraryData lib = new LibraryData();
        private string errorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            lib.Path = config["PHOTOS_URL"] + "/Libraries/";
            await RefreshList();
        }

        protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "library");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                libraries = await JsonSerializer.DeserializeAsync<IEnumerable<LibraryDTO>>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From Library.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }
    }
}
