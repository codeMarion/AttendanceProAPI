using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services
{
    public class Auth0Service : IAuth0Service
    {
        private readonly HttpClient client;
        private Auth0Settings auth0Settings;

        public Auth0Service(HttpClient client, IOptions<Auth0Settings> auth0Settings)
        {
            this.client = client;
            this.auth0Settings = auth0Settings.Value;
        }
        /// <summary>
        /// This method is used to update the user contact information in Auth0
        /// </summary>
        public async Task<IActionResult> UpdateUserDetails(string id, UserUpdate user)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri= new Uri($"https://attendancepro.eu.auth0.com/api/v2/users/{id}"),
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8,"application/json"),
            };
            request.Headers.Add("Authorization", $"Bearer {auth0Settings.Auth0ManagementAPIKey}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
        /// <summary>
        /// This method is used to update the user metadata in Auth0
        /// </summary>
        public async Task<IActionResult> UpdateUserMetaData(string id, string metadata)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://attendancepro.eu.auth0.com/api/v2/users/{id}"),
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonConvert.SerializeObject(new { user_metadata=new { students=metadata } }), Encoding.UTF8, "application/json"),
            };
            request.Headers.Add("Authorization", $"Bearer {auth0Settings.Auth0ManagementAPIKey}");
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
        /// <summary>
        /// This method is used to retrieve user metadata from Auth0
        /// </summary>
        public async Task<IActionResult> GetUserMetaData(string id)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://attendancepro.eu.auth0.com/api/v2/users/{id}"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Authorization", $"Bearer {auth0Settings.Auth0ManagementAPIKey}");
            HttpResponseMessage response = await client.SendAsync(request);
            string jsonString = await response.Content.ReadAsStringAsync();
            UserUpdateWithMetadata user = JsonConvert.DeserializeObject<UserUpdateWithMetadata>(jsonString);
            return new OkObjectResult(user);
        }
    }
}
