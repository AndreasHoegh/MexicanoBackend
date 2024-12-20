



//using Microsoft.EntityFrameworkCore;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);
//            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//            // Add services to the container.
//            builder.Services.AddDbContext<MexicanoBackend.Data.TournamentContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//            // Add CORS policy
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy(name: MyAllowSpecificOrigins,
//                                  policy =>
//                                  {
//                                      policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
//                                            .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
//                                            .AllowAnyHeader()  // Allow any headers
//                                            .AllowCredentials();
//                                  });
//            });

//            builder.Services.AddControllers()
//            .AddJsonOptions(options =>
//             {
//                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//                 options.JsonSerializerOptions.MaxDepth = 64;
//             });

//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            // Enable CORS
//            app.UseRouting(); // Ensure routing is configured before using CORS
//            app.UseCors(MyAllowSpecificOrigins); // Apply the CORS policy

//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();


using Microsoft.EntityFrameworkCore;
using MexicanoBackend.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<TournamentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Add your frontend URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

// Ensure the database is created and apply any pending migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TournamentContext>();
    //context.Database.EnsureCreated();
    //context.Database.Migrate();
}

app.Run();

