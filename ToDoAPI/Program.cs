// Step 02. To generate the C# models:
// Scaffold-DbContext "Server=.\sqlexpress;Database=Resources;Trusted_Connection=true;MultipleActiveResultSets=true"
// Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

// Step 03. Add the using statement.
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Step 10a. Add CORS functionality to determine what websites can access the data in this application.
// CORS stands for Cross Origin Resource Sharing and by default browsers use this to block websites
// from requesting data unless that website has permission to do so. This code below determines what websites
// have access to the CORS with this API.
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("OriginPolicy", "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Step 04. Add the ToDoContext service.
// This will initialize the database connection to be used in the controllers.
builder.Services.AddDbContext<ToDoAPI.Models.ToDoContext>(
    options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ToDoDB"));
        // The string above should match the connection string name in appsetting.json.
    }
);

// Step 05. Scaffold a new API controller using Entity Framework - Scaffold Categories
// by right-clicking the controllers folder. Go to Add > Controller, choose API controller with
// Actions using Entity Framework. Then choose the Categories model, the ToDoContext for
// DataContext, and click "Add". Repeat for the ToDo controller. For next step, see ToDo
// controller.


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Step 10b. Add UseCors statement below.
app.UseCors();

app.Run();
