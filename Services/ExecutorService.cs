using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ExecutorService : IExecutorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExecutorService> _logger;

        private readonly string _thingsboardBaseUrl = "http://100.92.93.89:8080";
        private readonly string _username = "sysadmin@thingsboard.org";
        private readonly string _password = "sysadmin";
        private readonly string _tenantPassword = "tenant";
        public ExecutorService(HttpClient httpClient, ILogger<ExecutorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }




         
        public async Task ExecuteScheduleAsync(Schedule schedule)
        {
            _logger.LogInformation($"Starting execution for schedule: {schedule.Name}");
            
            var tenantId = schedule.TenantId;
            var admintoken = await GetJwtTokenAsync(_username, _password);
            try
            {
            if (string.IsNullOrEmpty(admintoken))
            {
                _logger.LogError("Failed to get ThingsBoard token.");
                return;
            }
            var tenantUser = await GetTenantUserEmailAsync(tenantId, admintoken);
            if (string.IsNullOrEmpty(tenantUser))
                {
                    _logger.LogError("Failed to get ThingsBoard token.");
                    return;
                }
            
                var token = await GetJwtTokenAsync(tenantUser, _tenantPassword);

                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogError("Failed to get ThingsBoard token.");
                    return;
                }
                //============================== here we put the attributes we want to change and call the thingsboard and send the updates by
                //using SendAttributeToDeviceAsync() function
                foreach (var deviceSetting in schedule.DeviceSettings)
                {

                    foreach (var attr in deviceSetting.Attributes)
                    {
                        var attributes = new Dictionary<string, object>();
                        attributes[attr.Key] = attr.Value;
                        _logger.LogInformation($"Sending attributes to device {deviceSetting.DeviceId}: {string.Join(", ", attributes.Select(kv => $"{kv.Key}={kv.Value}"))}");
                        await SendAttributeToDeviceAsync(deviceSetting.DeviceId, attributes, token);
                        await Task.Delay(10000); // Delay to give the device time to process

                    }

                }

                _logger.LogInformation($"Finished execution for schedule: {schedule.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing schedule: {schedule.Name}");
            }
        }

        //============================== here we put the attributes we want to change and call the thingsboard and send the updates 










        // ========================== Here where we login to thingsboard and this function if go well returns tokken====================
        public async Task<string?> GetJwtTokenAsync(string name, string pass)
        {
            _logger.LogInformation("Authenticating with ThingsBoard...");

            var loginUrl = $"{_thingsboardBaseUrl}/api/auth/login";

            var loginData = new
            {
                username = name,
                password = pass
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(loginUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Authentication failed with status code: {response.StatusCode}");
                return null;
            }

            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Authentication succeeded.");

            var result = JsonSerializer.Deserialize<JwtResponse>(responseString);
            return result?.token;
        }

        // ========================== Here where we login to thingsboard and this function if go well returns tokken====================









        //=========================== this is the method that responsable to send attributes to the device that exist in thingsboard and update it
        public async Task SendAttributeToDeviceAsync(string deviceId, Dictionary<string, object> attributes, string token)
        {
            _logger.LogInformation($"Preparing to send attributes to device {deviceId}.");

            var url = $"{_thingsboardBaseUrl}/api/plugins/telemetry/DEVICE/{deviceId}/attributes/SHARED_SCOPE";

            var json = JsonSerializer.Serialize(attributes);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Attributes sent successfully to device {deviceId}.");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to send attributes to device {deviceId}. StatusCode: {response.StatusCode}, Response: {content}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while sending attributes to device {deviceId}.");
            }
        }

        //=========================== this is the method that responsable to send attributes to the device that exist in thingsboard and update it



        public async Task<string?> GetTenantUserEmailAsync(string tenantId, string token)
        {
            var url = $"{_thingsboardBaseUrl}/api/tenant/{tenantId}/users?pageSize=100&page=0";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var jsonDoc = JsonDocument.Parse(jsonString);
                    var users = jsonDoc.RootElement.GetProperty("data");

                    foreach (var user in users.EnumerateArray())
                    {
                        var email = user.GetProperty("email").GetString();
                        if (email != null && email.StartsWith("tenant", StringComparison.OrdinalIgnoreCase))
                        {
                            return email; //  Return first match
                        }
                    }

                    return null; // No match found
                }
                else
                {
                    _logger.LogError($"Request failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching tenant users.");
                return null;
            }
        }

        private class JwtResponse
        {
            public string token { get; set; } = string.Empty;
        }
    }
}
