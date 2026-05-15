using Newtonsoft.Json;
using System.Text;

namespace SinavSistemi.MVC.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    // ================= GET =================
    public async Task<T?> Get<T>(string url)
    {
        try
        {
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default;
        }
    }

    // ================= POST (EXISTING - KORUNDU) =================
    public async Task<(bool Basarili, string Mesaj)> Post<T>(string url, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return (false, body);

            return (true, body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // ================= NEW (EKLENDİ - LOGIN İÇİN) =================
    public async Task<TResponse?> Post<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var body = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(body);
        }
        catch
        {
            return default;
        }
    }

    // ================= PUT =================
    public async Task<(bool Basarili, string Mesaj)> Put<T>(string url, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PutAsync(url, content);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return (false, body);

            return (true, body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    // ================= DELETE =================
    public async Task<(bool Basarili, string Mesaj)> Delete(string url)
    {
        try
        {
            var response = await _http.DeleteAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return (false, body);

            return (true, body);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
    public async Task<T?> Post<T>(string url, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return default;

        return JsonConvert.DeserializeObject<T>(body);
    }
    public async Task<byte[]?> GetBytes(string url)
    {
        try
        {
            var response = await _http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var hata = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"PDF Hata: {response.StatusCode} - {hata}");
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PDF Exception: {ex.Message}");
            return null;
        }
    }

}