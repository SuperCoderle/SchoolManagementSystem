using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service.Client
{
    public class Book : ComponentBase
    {
        [Inject]
        private IHttpClientFactory httpClient { get; set; }

        [Inject]
        private IConfiguration config { get; set; }

        [Inject]
        private SweetAlertService swal { get; set; }

        public Book(IHttpClientFactory httpClient, IConfiguration config, SweetAlertService swal)
        {
            this.httpClient = httpClient;
            this.config = config;
            this.swal = swal;
        }

        public Book()
        {

        }

        public IEnumerable<BookDTO> books = Array.Empty<BookDTO>();
        public BookData book = new BookData();
        private string errorMessage { get; set; }

        [Parameter]
        public string CategoryName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            book.Path = config["PHOTOS_URL"];
            book.Photo = "anonymous.png";

            await RefreshList();
        }

        protected async Task RefreshList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "book/" + CategoryName);
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                books = await JsonSerializer.DeserializeAsync<IEnumerable<BookDTO>>(responseStream);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From Book.cs" + errorMessage, SweetAlertIcon.Error);
            }
        }

        protected void RandomColor()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            book.RandomColor = color;
        }
    }
}
