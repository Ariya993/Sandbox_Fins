using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;
namespace Sandbox_Calc.Controllers
{
    [ApiController]
    [Route("api/fcm")]
    public class FCMController : Controller
    {
      //  private readonly Services _services;
        private readonly AppDbContext _appDbContext; 
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        public FCMController(IConfiguration configuration, AppDbContext appDbContext,  IHttpClientFactory httpClientFactory)
        {
            _config = configuration;
            _appDbContext = appDbContext; 
            _httpClientFactory = httpClientFactory;
        }
        [HttpPost("set")]
        public async Task<IActionResult> SetFcmToken([FromBody] APPUSER_FCM fcmData)
        {
            if (string.IsNullOrWhiteSpace(fcmData.Username))
                return BadRequest("Username is required.");

            var existing = await _appDbContext.APPUSER_FCM
                .FirstOrDefaultAsync(x => x.Username == fcmData.Username);

            if (existing != null)
            {
                existing.token_fcm = fcmData.token_fcm;
                _appDbContext.APPUSER_FCM.Update(existing);
            }
            else
            {
                _appDbContext.APPUSER_FCM.Add(fcmData);
            }

            await _appDbContext.SaveChangesAsync();

            return Ok(new { message = "FCM token updated successfully." });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] FCMDTO request)
        {
            var projectId = _config["Firebase:ProjectId"];
            var serviceAccountPath = _config["Firebase:ServiceAccountPath"];

            if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(serviceAccountPath))
                return BadRequest("Missing Firebase configuration.");

            try
            {
                var accessToken = await GetAccessTokenAsync(serviceAccountPath);
                object message;
                if (request.Type == "topic")
                {
                    message = new
                    {
                        message = new
                        {
                            topic = request.Device,
                            notification = new
                            {
                                title = request.Title,
                                body = request.Message
                            },
                            data = new
                            {
                                title = request.Title,
                                body = request.Message
                            }
                        }
                    };
                }
                else
                {
                    message = new
                    {
                        message = new
                        {
                            token = request.Device,
                            notification = new
                            {
                                title = request.Title,
                                body = request.Message
                            },
                            data = new
                            {
                                title = request.Title,
                                body = request.Message
                            }
                        }
                    };
                }

                var client = _httpClientFactory.CreateClient();

                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send")
                {
                    Content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json")
                };

                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, responseContent);

                return Ok(JsonDocument.Parse(responseContent));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private async Task<string> GetAccessTokenAsync(string serviceAccountPath)
        {
            var jsonKey = await System.IO.File.ReadAllTextAsync(serviceAccountPath);
            var credential = GoogleCredential.FromJson(jsonKey)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }

    }
}
