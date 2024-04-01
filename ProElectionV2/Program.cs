using FluentValidation;
using ProElectionV2.Entities;
using ProElectionV2.Entities.Validations;
using ProElectionV2.Persistence;
using ProElectionV2.Repositories;
using ProElectionV2.Repositories.Interfaces;
using ProElectionV2.Services;
using ProElectionV2.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<ProElectionV2DbContext>();

builder.Services
    .AddScoped<IElectionCodeRepository, ElectionCodeRepository>()
    .AddScoped<IElectionRepository, ElectionRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IVoteRepository, VoteRepository>();

builder.Services
    .AddScoped<IElectionService, ElectionService>()
    .AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<INotifyService, NotifyService>();

builder.Services.AddScoped<IValidator<Election>, ElectionValidator>();
builder.Services.AddScoped<IValidator<ElectionCode>, ElectionCodeValidator>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Vote>, VoteValidator>();

var app = builder.Build();

string[] supportedCultures = ["en-GB", "de-DE"];
RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

app.Run();