using CloudPlatform.Tool.Configuration;
using CloudPlatform.Tool.StorageAccount;
using CloudPlatform.Tool.WebApp;
using System;
using System.Data;

var builder = WebApplication.CreateBuilder(args);


ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await app.InitStartUp(builder.Configuration);

app.Run();


void ConfigureServices(IServiceCollection services)
{
    AppDomain.CurrentDomain.Load("CloudPlatform.Tool.Configuration");
    services.AddLocalization(options => options.ResourcesPath = "Resources");

    // Add services to the container.
    services.AddRazorPages()
            .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(Program)))
            .AddRazorPagesOptions(options =>
            {
              
            });


    services.AddBlobConfig();


    services.AddBlobStorage(builder.Configuration, options => options.ContentRootPath = builder.Environment.ContentRootPath);
}

