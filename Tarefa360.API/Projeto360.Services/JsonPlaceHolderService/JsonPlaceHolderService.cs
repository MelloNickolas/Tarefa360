// using Projeto360.Services.Interfaces;
// using Projeto360.Domain.Entities;
// using Newtonsoft.Json;
// using Projeto360.Services.JsonPlaceHolderService.Models;

// public class JsonPlaceHolderService : IJsonPlaceHolderService
// {
//     private readonly HttpClient _httpClient;
//     public JsonPlaceHolderService()
//     {
//         _httpClient = new HttpClient
//         {
//             BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
//         };
//     }

//     public async Task<List<Tarefa>> ListarTarefas()
//     {
//         HttpResponseMessage response = await _httpClient.GetAsync("todos");
//         response.EnsureSuccessStatusCode();

//         string responseBody = await response.Content.ReadAsStringAsync();
//         var todos = JsonConvert.DeserializeObject<List<Todo>>(responseBody);

//         var tarefas = todos.Select(t => new Tarefa
//         {
//             ID = t.Id,
//             Titulo = t.Title,
//             Completa = t.Completed
//         }).ToList();

//         return tarefas;
//     }
// }