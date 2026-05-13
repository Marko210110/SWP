using System.Text.Json;
using System.Text.Json.Serialization;
using BlazorApp1.Models;

namespace BlazorApp1.Services;

public class ExerciseService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://exercisedb.p.rapidapi.com";
    private const string ApiKey = "9d13172ca7msh47630397baf59b8p15f9b0jsn019676d8c574";
    private const string ApiHost = "exercisedb.p.rapidapi.com";

    public ExerciseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", ApiKey);
        _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", ApiHost);
    }

    private JsonSerializerOptions JsonOptions => new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<List<Exercise>> SearchExercisesByNameAsync(string name)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/name/{Uri.EscapeDataString(name)}");
            if (!response.IsSuccessStatusCode) return new List<Exercise>();
            var json = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(json, JsonOptions);
            return exercises ?? new List<Exercise>();
        }
        catch
        {
            return new List<Exercise>();
        }
    }

    public async Task<List<Exercise>> GetExercisesByBodyPartAsync(string bodyPart)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/bodyPart/{Uri.EscapeDataString(bodyPart)}");
            if (!response.IsSuccessStatusCode) return new List<Exercise>();
            var json = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(json, JsonOptions);
            return exercises ?? new List<Exercise>();
        }
        catch
        {
            return new List<Exercise>();
        }
    }

    public async Task<List<Exercise>> GetExercisesByTargetAsync(string target)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/target/{Uri.EscapeDataString(target)}");
            if (!response.IsSuccessStatusCode) return new List<Exercise>();
            var json = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(json, JsonOptions);
            return exercises ?? new List<Exercise>();
        }
        catch
        {
            return new List<Exercise>();
        }
    }

    public async Task<List<Exercise>> GetExercisesByEquipmentAsync(string equipment)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/equipment/{Uri.EscapeDataString(equipment)}");
            if (!response.IsSuccessStatusCode) return new List<Exercise>();
            var json = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializer.Deserialize<List<Exercise>>(json, JsonOptions);
            return exercises ?? new List<Exercise>();
        }
        catch
        {
            return new List<Exercise>();
        }
    }

    public async Task<Exercise?> GetExerciseByIdAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/exercise/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var exercise = JsonSerializer.Deserialize<Exercise>(json, JsonOptions);
            return exercise;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<string>> GetBodyPartListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/bodyPartList");
            if (!response.IsSuccessStatusCode) return new List<string>();
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<string>>(json, JsonOptions);
            return list ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    public async Task<List<string>> GetTargetListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/targetList");
            if (!response.IsSuccessStatusCode) return new List<string>();
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<string>>(json, JsonOptions);
            return list ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    public async Task<List<string>> GetEquipmentListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/exercises/equipmentList");
            if (!response.IsSuccessStatusCode) return new List<string>();
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<string>>(json, JsonOptions);
            return list ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
