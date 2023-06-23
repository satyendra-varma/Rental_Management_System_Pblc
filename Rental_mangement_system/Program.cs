using Rental_mangement_system.ImageUploadService;
using Rental_mangement_system.ImageDeleteService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ImageUploadService, LocalImageUploadService>();
builder.Services.AddScoped<ImageDeleteService, AWSImageDeleteService>();

var app = builder.Build();

//app.Urls.Add("http://209.23.9.19:5024"); //windows ip
//app.Urls.Add("http://localhost:5024");

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
