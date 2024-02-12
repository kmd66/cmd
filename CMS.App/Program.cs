using CMS.Dal.DataSource;
using Microsoft.Extensions.WebEncoders;
using Sample.Model.Data;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

SetProperty(builder);
RegisterLibrary(builder.Services);
CMS.App.Container.Init(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
        context.Response.Redirect("/error/404");
});
app.UseSession();

app.Run();
async void RegisterLibrary(IServiceCollection services)
{
    services.Configure<WebEncoderOptions>(options =>
    {
        options.TextEncoderSettings = new TextEncoderSettings(
            UnicodeRanges.BasicLatin,
            UnicodeRanges.All);
    });

    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
