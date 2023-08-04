using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class HttpRequest
{
    public async Task<string> CallApiAndGetResponse(string apiUrl, string jsonData)
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Set the base address of the API
                httpClient.BaseAddress = new Uri(apiUrl);

                // Set the content type to "application/json"
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Create the HTTP content with the JSON data
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Make the POST request to the API and get the response
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Request failed, return an error message or handle the response accordingly
                    return "API request failed with status code: " + response.StatusCode;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur during the API call
            return "Error occurred during API call: " + ex.Message;
        }
    }
}
