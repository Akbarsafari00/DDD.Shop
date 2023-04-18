using DDD.Shop.Application.Consumers;
using DDD.Shop.Application.Jobs;
using DDD.Shop.Common.BackgroundServices;
using DDD.Shop.Infrastructure.DataAccess.Mongo;
using DDD.Shop.Infrastructure.DataAccess.Redis;
using DDD.Shop.Infrastructure.Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMongoDB();
builder.Services.AddRedis();
builder.Services.AddRabbitMQ();

builder.Services.AddHostedService<OrderJobs>();

builder.Services.AddHostedService<AcceptedOrderMessageConsumer>();
builder.Services.AddHostedService<PendingOrderMessageConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();