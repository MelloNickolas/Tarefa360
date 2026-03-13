using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Projeto360.Dominio.Entidades;
using Projeto360.Services.Interfaces;
using Projeto360.Services.JsonPlaceHolderService.Models;
public class JsonPlaceHolderService : IJsonPlaceHolderService
{
  private readonly HttpClient _httpClient;

  public JsonPlaceHolderService()
  {
    _httpClient = new HttpClient();
    _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
  }

  public async Task<List<Tarefa>> ListarTarefas()
  {
    HttpResponseMessage responseMessage = await _httpClient.GetAsync("todos");
    responseMessage.EnsureSuccessStatusCode();

    string responseBody = await responseMessage.Content.ReadAsStringAsync();
    var todos = JsonConvert.DeserializeObject<List<Todo>>(responseBody);

    var tarefas = todos.Select(todo => new Tarefa()
    {
      ID = todo.Id,
      Nome = todo.Title,
      Completa = todo.Completed
    }).ToList();

    return tarefas;
  }
}