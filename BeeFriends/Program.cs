using BeeFriends.Context;
using BeeFriends.Hubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In memory databse

builder.Services.AddDbContext<AppDBContext>(options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("BeeFriendsDB")));

// In memory dictionary thingy

builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => new Dictionary<string, UserConnection>());

// Singal R

builder.Services.AddSignalR(e => {
    e.MaximumReceiveMessageSize = 102400000;
});

// Cors
// Cors Options

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(cors =>
    {
        cors.WithOrigins(builder.Configuration.GetSection("AllowedOrigin").Value)
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

app.UseRouting();
//Cors
app.UseCors();

//Signal R
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
