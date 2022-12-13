using API.Repositories;
using API.Services;
using API.Hubs;
using API.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Diagnostics;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IconRepository>();
builder.Services.AddSingleton<IconService>();
builder.Services.AddSingleton<CompumatRepository>();
builder.Services.AddSingleton<CompumatService>();
builder.Services.AddSingleton<CompumatHub>();
builder.Services.AddSingleton<MapRepository>();
builder.Services.AddSingleton<MapService>();
builder.Services.AddSingleton<CommunicationService>();
builder.Services.AddSingleton<RabbitService>();

builder.Services.AddSignalR();
builder.Services.AddCors(o => o.AddPolicy("myPolicy", builder => {
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

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

string[] bindingKeys = { "#" }; // Receive all log topics
var factory = new ConnectionFactory() { HostName = "localhost" };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
var queueName = channel.QueueDeclare().QueueName;
var consumer = new EventingBasicConsumer(channel);
channel.QueueBind(queue: queueName, exchange: "topic_logs", routingKey: bindingKeys[0]);
using (var service = app.Services.CreateScope()) {
    var services = service.ServiceProvider;
    var dependency = services.GetRequiredService<RabbitService>();
    consumer.Received += (model, eventArgs) => {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = eventArgs.RoutingKey;

        dependency.OnMessageReceived(routingKey, message);
    };
}
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

app.Run();
