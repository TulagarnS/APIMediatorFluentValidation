using Interview_Test.Validations;
using FluentValidation;
using Interview_Test.Infrastructure;
using Interview_Test.Mediator.Handler;
using Interview_Test.Mediator.Queries;
using Interview_Test.Middlewares;
using Interview_Test.Repositories;
using Interview_Test.Repositories.Interfaces;
using Interview_Test.Validations;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>(); //Register service lifetime
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly));  //MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); //Fluent Validator
var connection = "Server=.;Database=entitytest;Trusted_Connection=True;TrustServerCertificate=true;";
builder.Services.AddDbContext<InterviewTestDbContext>(options =>
    {
        options.UseSqlServer(connection,
            sqlOptions =>
            {
                sqlOptions.UseCompatibilityLevel(110);
                sqlOptions.CommandTimeout(30);
                sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
            });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseFluentValidationHandlerExtension();
app.UseMvc();
app.Run();