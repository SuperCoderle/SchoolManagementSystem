using BlazorInputFile;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;

namespace SchoolManagement.Service
{
    public class LibrarySystem : ComponentBase
    {
		[Inject]
		private IHttpClientFactory httpClient { get; set; }

		[Inject]
		private IConfiguration config { get; set; }

		[Inject]
		private IJSRuntime jS { get; set; }

		[Inject]
		private SweetAlertService swal { get; set; }

		public LibrarySystem(IHttpClientFactory httpClient, IConfiguration config, IJSRuntime jS, SweetAlertService swal)
		{
			this.httpClient = httpClient;
			this.config = config;
			this.jS = jS;
			this.swal = swal;
		}

		public LibrarySystem()
		{

		}

		public IEnumerable<LibraryDTO> libraries = Array.Empty<LibraryDTO>();
		public LibraryData lib = new LibraryData();
		private string errorMessage { get; set; }

		protected override async Task OnInitializedAsync()
		{
			lib.Photo = "anonymous.png";
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
                await swal.FireAsync("Error!", "From LibrarySystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task CreateClick()
		{
			try
			{
				var library = new LibraryDTO { CategoryName = lib.CategoryName, Photo = lib.Photo, Description = lib.Description };
				var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "library");
				request.Headers.Add("Accept", "application/json");
				request.Content = new StringContent(JsonSerializer.Serialize(library), null, "application/json");
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
                await swal.FireAsync("Error!", "From LibrarySystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected async Task UpdateClick()
		{
			try
			{
				var library = new LibraryDTO { ID = lib.ID, CategoryName = lib.CategoryName, Photo = lib.Photo, Description = lib.Description };
				var request = new HttpRequestMessage(HttpMethod.Put, config["API_URL"] + "library");
				request.Headers.Add("Accept", "application/json");
				request.Content = new StringContent(JsonSerializer.Serialize(library), null, "application/json");
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
                await swal.FireAsync("Error!", "From LibrarySystem.cs" + errorMessage, SweetAlertIcon.Error);
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
                    var request = new HttpRequestMessage(HttpMethod.Delete, config["API_URL"] + "library/" + ID);
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
                await swal.FireAsync("Error!", "From LibrarySystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}

		protected void AddClick()
		{
			lib.Title = "Add New Category";
			lib.ID = 0;
			lib.CategoryName = string.Empty;
			lib.Photo = string.Empty;
			lib.Description = string.Empty;
		}

		protected void EditClick(LibraryDTO library)
		{
			lib.Title = "Update Category";
			lib.ID = library.ID;
			lib.CategoryName = library.CategoryName;
			lib.Photo = library.Photo;
			lib.Description = library.Description;
		}

		protected async Task UploadFile(IFileListEntry[] files)
		{
			try
			{
				var file = files.FirstOrDefault();
				var ms = new MemoryStream();
				await file.Data.CopyToAsync(ms);

				var content = new MultipartFormDataContent { { new ByteArrayContent(ms.GetBuffer()), "\"file\"", file.Name } };

				var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "Library/SaveFile");
				request.Content = content;

				var client = httpClient.CreateClient();
				var response = await client.SendAsync(request);
				using var responseStream = await response.Content.ReadAsStreamAsync();
				lib.Photo = await JsonSerializer.DeserializeAsync<string>(responseStream);
			}
			catch (Exception ex)
			{
                errorMessage = ex.Message;
                await swal.FireAsync("Error!", "From LibrarySystem.cs" + errorMessage, SweetAlertIcon.Error);
            }
		}
	}
}
