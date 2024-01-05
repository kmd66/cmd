
using CMS.Helper;
using CMS.Model.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Sample.Model.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();

RegisterLibrary(builder.Services);
SetProperty(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

CMS.Tools.Container.Init(builder.Services);

app.MapControllers();
app.Run();

void RegisterLibrary(IServiceCollection services)
{
    services.AddDbService();
    services.AddScoped<IInfo, CMS.Tools.Info>();
    services.AddTransient<ApiRequest>();
}

void SetProperty(WebApplicationBuilder b)
{
    var builder = WebApplication.CreateBuilder();
    CMS.Model.Property.ConnectionString = b.Configuration.GetConnectionString("SqlServer");
    CMS.Model.Property.AccessTokenExpireTimeSpan = int.Parse(b.Configuration["AppSettings:AccessTokenExpireTimeSpan"]);
    CMS.Model.Property.AttachmentSize = int.Parse(b.Configuration["AppSettings:AttachmentSize"]);
    CMS.Model.Property.Upload = b.Configuration["AppSettings:Upload"];
    var env = b.Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
    CMS.Model.Property.WebRootPath = env.WebRootPath;
    var t = CMS.Model.Property.UploadPath;

}

