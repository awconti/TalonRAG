using Microsoft.AspNetCore.Diagnostics;
using TalonRAG.Application.Exceptions;
using TalonRAG.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.RegisterConversationWebApiDependencies();

var app = builder.Build();

app.UseExceptionHandler(contextBuilder =>
{
	contextBuilder.Run(async context =>
	{
		var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
		context.Response.StatusCode = exception switch
		{
			ConversationNotFoundException
				or UserNotFoundException
				or UserConversationsNotFoundException => StatusCodes.Status404NotFound,
			_ => StatusCodes.Status500InternalServerError,
		};
		await context.Response.WriteAsync(exception?.Message ?? string.Empty);
	});
});

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
