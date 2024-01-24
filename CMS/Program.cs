
using CMS.Dal.DataSource;
using CMS.Helper;
using CMS.Model.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.WebEncoders;
using Sample.Model.Data;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();

SetProperty(builder);
RegisterLibrary(builder.Services);
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

async void RegisterLibrary(IServiceCollection services)
{
    services.Configure<WebEncoderOptions>(options =>
    {
        options.TextEncoderSettings = new TextEncoderSettings(
            UnicodeRanges.BasicLatin,
            UnicodeRanges.All);
    });
    services.AddDbService();
    //services.AddScoped<IInfo, CMS.Tools.Info>();
    //services.AddScoped<IOptionDataSource, OptionDataSource>();
    services.AddTransient<ApiRequest>();

    new MenuDataSource().ListAsync();
    new OptionDataSource().ListAsync();
}

void SetProperty(WebApplicationBuilder b)
{
    var builder = WebApplication.CreateBuilder();
    //CMS.Model.Property.ConnectionString = b.Configuration.GetConnectionString("SqlServer");
    CMS.Model.Property.ConnectionString = b.Configuration.GetConnectionString("SqlServer");
    CMS.Model.Property.AccessTokenExpireTimeSpan = int.Parse(b.Configuration["AppSettings:AccessTokenExpireTimeSpan"]);
    CMS.Model.Property.AttachmentSize = int.Parse(b.Configuration["AppSettings:AttachmentSize"]);
    CMS.Model.Property.Upload = b.Configuration["AppSettings:Upload"];
    var env = b.Services.BuildServiceProvider().GetService<IWebHostEnvironment>();
    CMS.Model.Property.WebRootPath = env.WebRootPath;
    var t = CMS.Model.Property.UploadPath;

}

