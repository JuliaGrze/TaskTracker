using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddControllersWithViews();

//Add services into IoC container
builder.Services.AddScoped<ITasksService, TasksService>();

var app = builder.Build();


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
