using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolDTOS;
using SchoolManagement.Data;
using System.Text.Json;
using System.Web;

namespace SchoolManagement.Service
{
	public class Account : ComponentBase
	{
		[Inject]
		private IConfiguration config { get; set; }

		[Inject]
		private IHttpClientFactory httpClient { get; set; }

		[Inject]
		private IJSRuntime jS { get; set; }

		[Inject]
		private NavigationManager navigation { get; set; }

		public Account(IConfiguration config, IHttpClientFactory httpClient, IJSRuntime JS)
		{
			this.config = config;
			this.httpClient = httpClient;
			jS = JS;
		}

		public Account()
		{

		}

		public IEnumerable<AccountDTO> accounts = Array.Empty<AccountDTO>();
		public AccountData acc = new AccountData();

		protected override async Task OnInitializedAsync()
		{
			await RefreshList();
		}

		protected async Task RefreshList()
		{
            var request = new HttpRequestMessage(HttpMethod.Get, config["API_URL"] + "account/" + acc.Username);
			var client = httpClient.CreateClient();
			var response = await client.SendAsync(request);
			using var responseStream = await response.Content.ReadAsStreamAsync();
			accounts = await JsonSerializer.DeserializeAsync<IEnumerable<AccountDTO>>(responseStream);
		}

		protected async Task CreateClick()
		{
			var account = new AccountDTO { Username = acc.Username, Password = acc.Password };
			var request = new HttpRequestMessage(HttpMethod.Post, config["API_URL"] + "account");
			request.Headers.Add("Accept", "application/json");
			request.Content = new StringContent(JsonSerializer.Serialize(account), null, "application/json");
			var client = httpClient.CreateClient();
			var response = await client.SendAsync(request);
			using var responseStream = await response.Content.ReadAsStreamAsync();

			string res = await JsonSerializer.DeserializeAsync<string>(responseStream);
			await jS.InvokeVoidAsync("alert", res);
			await RefreshList();
		}

		protected void Checklogin(string username, string password)
		{
			try
			{
				username = acc.Username;
				password = acc.Password;
				RefreshList();
				if (accounts.Count() == 0)
					jS.InvokeVoidAsync("alert", "Loading... Try again!");
				else
				{
                    foreach (var account in accounts)
                    {
                        if (account.Username.Equals(username))
                        {
							if (account.Password.Equals(password))
								navigation.NavigateTo("/index");
							else
								jS.InvokeVoidAsync("alert", "Password is incorrect");
                        }
                        else
                            jS.InvokeVoidAsync("alert", "Account doesn't exist");
                    }
                }
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
