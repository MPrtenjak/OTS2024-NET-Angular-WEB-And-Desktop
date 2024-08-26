using GoodDayWebApp;
using GoodDayWebApp.Auth;
using GoodDayWebApp.Auth.Interface;
using GoodDayWebApp.Database;
using GoodDayWebApp.Database.Interface;
using GoodDayWebApp.Database.Repositories;
using GoodDayWebApp.EndPoints;
using GoodDayWebApp.Environments;
using GoodDayWebApp.Localization;
using GoodDayWebApp.Logging;
using GoodDayWebApp.Middleware;
using GoodDayWebApp.Resources;
using GoodDayWebApp.Resources.Interfaces;
using GoodDayWebApp.Services;
using GoodDayWebApp.Services.Interfaces;

// 
//  It is important to use the correct  folder as the root of our client application
//
var options = new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory };
WebApplicationBuilder builder = WebApplication.CreateBuilder(options);

//
//  Logging, Authentication, Authorization, Localization
//
builder.AddCustomSerilog();

builder.Services.AddCustomAuthenticationAndAddAuthorization(builder.Configuration);

builder.Services.AddCustomLocalization();

//
//  Environment
//
var runningEnvironment = EnvironmentDetector.Detect();
builder.Services.AddSingleton<ISupportedEnvironment>(runningEnvironment);

var thisApplication = new ThisApplication();
builder.Services.AddSingleton<IThisApplication>(thisApplication);

//
//  Swagger, CORS, Dapper, Repository, UserIdentifierService
//
builder.Services.AddSwaggerSupport(thisApplication);

builder.Services.AddScoped<IConfigurationStringBuilder, SQLiteConfigurationStringBuilder>();
builder.Services.AddScoped<IJWTElements, JWTElements>();
builder.Services.AddScoped<IJWT, JWT>();

builder.Services.AddScoped<IAppResourceManager, AppResourceManager>();
builder.Services.AddScoped<IDapperContext, DapperContext>();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGratitudeRepository, GratitudeRepository>();

builder.Services.AddScoped<IUserIdentifierService>((_) => UserIdentifierServiceDetector.Detect());

builder.Services.AddCorsSupport(thisApplication);

//
//  Middleware for services
//
builder.Host.ApplyDeamons(runningEnvironment);

WebApplication app = builder.Build();

//
//  Localization
//
app.UseCustomRequestLocalization();

//
//  Swagger
//
app.UseSwaggerIfSupported(thisApplication);

//
//  HTTPS, Static Files, CORS, Authentication, Authorization
//
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCorsIfSupported(thisApplication);

app.UseAuthentication();
app.UseAuthorization();

//
// Error Handling, Startup
//
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<StartupMiddleware>();

//
//  Endpoints
//
app.MapEndPoints();

app.MapFallbackToFile("index.html");

//
//  Run
//
if (!thisApplication.RunningInDebugMode)
  thisApplication.OpenBrowser(app.Configuration, runningEnvironment);

app.Run();
