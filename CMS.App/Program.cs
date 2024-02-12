using CMS.Dal.DataSource;
using Microsoft.Extensions.WebEncoders;
using Sample.Model.Data;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

CMS.App.Container.Init(builder.Services);
SetProperty(builder);
RegisterLibrary(builder.Services);

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

app.Run();
async void RegisterLibrary(IServiceCollection services)
{
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
