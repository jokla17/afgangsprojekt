using API.Repositories;
using API.Services;
using API.Hubs;
using API.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("myPolicy", builder => {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSingleton<IconRepository>();
builder.Services.AddSingleton<IconService>();
builder.Services.AddSingleton<CompumatRepository>();
builder.Services.AddSingleton<CompumatService>();
builder.Services.AddSingleton<MapRepository>();
builder.Services.AddSingleton<MapService>();
builder.Services.AddSingleton<CommunicationService>();
builder.Services.AddSingleton<RabbitService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("myPolicy");

app.UseAuthorization();

app.MapControllers();
app.MapHub<CompumatHub>("/compumatHub");

app.Run();
