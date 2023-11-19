
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Simulate;
using System.Net.Http;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHostedService<Simulator>();

        var app = builder.Build();

        app.Run();
    }
}