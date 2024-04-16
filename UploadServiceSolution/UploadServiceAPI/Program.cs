using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using UploadServiceAPI.Interface;
using UploadServiceAPI;
using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til containeren
builder.Services.AddControllers();
builder.Services.AddHttpClient<IWorkerClient, WorkerClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001"); // Dette skal være din Worker Service's HTTPS-port
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator // Brug kun i udviklingsmiljøer
});

var app = builder.Build();

// Konfigurer middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Aktiver HTTPS-omdirigering
    app.UseHttpsRedirection();
}



app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
