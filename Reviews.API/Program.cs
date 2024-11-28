using Reviews.API.Kafka;
using Reviews.API.Services;
using Reviews.Domain.Entities;
using Reviews.Domain.Entities.Factories;
using Reviews.Infrastructure.Kafka;
using Reviews.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the producer service as singletons:
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
// Add the kafka consumer service as a hosted service (background service that runs for the lifetime of the application):
builder.Services.AddHostedService<KafkaConsumer>();
builder.Services.AddSingleton<IReviewService, ReviewService>();

// Add mongoDB
builder.Services.Configure<MongoDBConnection>(
    builder.Configuration.GetSection("MongoDbConnection"));
builder.Services.AddSingleton<IReviewRepository, MongoDBRepository>();

// Add reviewFactories by them selves:
builder.Services.AddSingleton<RestaurantReviewFactory>();
builder.Services.AddSingleton<DeliveryAgentReviewFactory>();

// Use the correct review factory based on type of review:
builder.Services.AddSingleton<Func<string, IReviewFactory>>(serviceProvider =>
{
    var factories = new Dictionary<string, IReviewFactory>
    {
        { "restaurant", serviceProvider.GetRequiredService<RestaurantReviewFactory>() },
        { "deliveryAgent", serviceProvider.GetRequiredService<DeliveryAgentReviewFactory>() }
    };

    return key => factories[key];
});

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
