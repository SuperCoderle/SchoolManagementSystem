using BlazorInputFile;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using SchoolManagement.Service.Client;
using System.Text.Json;

namespace SchoolManagement.Service
{
    public class BookSystem : ComponentBase
    {
		[Inject]
		private IConfiguration config { get; set; }

		[Inject]
		private IHttpClientFactory httpClient { get; set; }

		[Inject]
		private IJSRuntime jS { get; set; }

		[Inject]
		private SweetAlertService swal { get; set; }

        public BookSystem(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS, SweetAlertService swal)
		{
			this.config = config;
			this.httpClient = httpClient;
			jS = JS;
			this.swal = swal;
		}

		public BookSystem()
		{

		}

		public IEnumerable<BookDTO> books = Array.Empty<BookDTO>();
		public IEnumerable<LibraryDTO> libraries = Array.Empty<LibraryDTO>();
		public IEnumerable<StudentDTO> students = Array.Empty<StudentDTO>();
		public BookData bk = new BookData();
		private string errorMessage { get; set; }

		protected override async Task OnInitializedAsync()
		{
			bk.Path = config["PHOTOS_URL"];
			bk.Photo = "anonymous.png";
			await RefreshList();
		}



		protected async Task RefreshList()
		{
			try
			{
                var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "book");
                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                books = await JsonSerializer.DeserializeAsync<IEnumerable<BookDTO>>(responseStream);

                var request2 = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "library");
                var client2 = httpClient.CreateClient();
                var response2 = await client2.SendAsync(request2);
                using var responseStream2 = await response2.Content.ReadAsStreamAsync();
                libraries = await JsonSerializer.DeserializeAsync<IEnumerable<LibraryDTO>>(responseStream2);

				var request3 = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "student");
				var client3 = httpClient.CreateClient();
				var response3 = await client3.SendAsync(request3);
				using var responseStream3 = await response3.Content.ReadAsStreamAsync();
				students = await JsonSerializer.DeserializeAsync<IEnumerable<StudentDTO>>(responseStream3);
			}
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From BookSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task CreateClick()
		{
			try
			{
                var book = new BookDTO { Name = bk.Name, CategoryName = bk.CategoryName, Description = bk.Description, IsActive = bk.IsActive, BookLoanDay = bk.BookLoanDay.ToString("yyyy-MM-dd"), BookReturnDay = bk.BookReturnDay.ToString("yyyy-MM-dd"), StudentName = bk.StudentName, Photo = bk.Photo };
                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "book");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(book), null, "application/json");
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
                await swal.FireAsync("Error!", "From BookSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task UpdateClick()
		{
			try
			{
                var book = new BookDTO { ID = bk.ID, Name = bk.Name, CategoryName = bk.CategoryName, Description = bk.Description, IsActive = bk.IsActive, BookLoanDay = bk.BookLoanDay.ToString("yyyy-MM-dd"), BookReturnDay = bk.BookReturnDay.ToString("yyyy-MM-dd"), StudentName = bk.StudentName, Photo = bk.Photo };
                var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "book");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(book), null, "application/json");
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
                await swal.FireAsync("Error!", "From BookSystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "book/" + ID);
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
                await swal.FireAsync("Error!", "From BookSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected void AddClick()
		{
			bk.Title = "Add New Book";
			bk.ID = 0;
			bk.Name = string.Empty;
			bk.CategoryName = string.Empty;
			bk.Description = string.Empty;
			bk.IsActive = string.Empty;
			bk.BookLoanDay = DateTime.Now;
			bk.BookReturnDay = DateTime.Now;
			bk.StudentName = "";
			bk.Photo = "anonymous.png";
		}

		protected void EditClick(BookDTO book)
		{
			bk.Title = "Update Book";
			bk.ID = book.ID;
			bk.Name = book.Name;
			bk.CategoryName = book.CategoryName;
			bk.Description = book.Description;
			bk.IsActive = book.IsActive;
			bk.BookLoanDay = Convert.ToDateTime(book.BookLoanDay);
			bk.BookReturnDay = Convert.ToDateTime(book.BookReturnDay);
			bk.StudentName = book.StudentName;
			bk.Photo = book.Photo;
		}

		protected async Task UploadFile(IFileListEntry[] files)
		{
			try
			{
                var file = files.FirstOrDefault();
                var ms = new MemoryStream();
                await file.Data.CopyToAsync(ms);

                var content = new MultipartFormDataContent { { new ByteArrayContent(ms.GetBuffer()), "\"file\"", file.Name } };

                var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "book/SaveFile");
                request.Content = content;

                var client = httpClient.CreateClient();
                var response = await client.SendAsync(request);
                using var responseStream = await response.Content.ReadAsStreamAsync();
                bk.Photo = await JsonSerializer.DeserializeAsync<string>(responseStream);
            }
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From BookSystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}
	}
}
