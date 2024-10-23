using System;
using System.IO;
using Newtonsoft.Json;
using Firebase;
using Firebase.Extensions;

public class Config
{
    public string GoogleApiKey { get; set; }
    public string AppId { get; set; }
    public string ProjectId { get; set; }
}

public class Program
{
    static void Main()
    {
        // Lee config.json para obtener las claves
        string configJson = File.ReadAllText("config.json");
        Config config = JsonConvert.DeserializeObject<Config>(configJson);

        string apiKey = config.GoogleApiKey;
        string appId = config.AppId;
        string projectId = config.ProjectId;

        // Inicializa Firebase con las opciones leídas
        InitializeFirebase(apiKey, appId, projectId);
    }

    static void InitializeFirebase(string apiKey, string appId, string projectId)
    {
        FirebaseApp.Create(new AppOptions()
        {
            ApiKey = apiKey,
            AppId = appId,
            ProjectId = projectId
        });

        Console.WriteLine("Firebase App Initialized");
    }
}