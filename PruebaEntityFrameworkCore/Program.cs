using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PruebaEntityFrameworkCore;
using PruebaEntityFrameworkCore.Entidades;

var builder = WebApplication.CreateBuilder(args);

//Politica de autentificación.
var polityUserAuthentifition = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllersWithViews(
    opc => opc.Filters.Add(new AuthorizeFilter(polityUserAuthentifition))
);

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<ApplicationDbContext>(opciones
//  => opciones.UseSqlServer("name=MyConnectionTrust"));

 builder.Services.AddDbContext<ApplicationDbContext>(opciones 
 => opciones.UseNpgsql("name=PostgresConnection"));

 AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

 builder.Services.AddAuthentication();

 //Utilizar los servicios de Identity
 builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    opc =>  { opc.SignIn.RequireConfirmedAccount = false; }
).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<MensajesDeErrorIdentity>();

builder.Services.PostConfigure<CookieAuthenticationOptions>(
    IdentityConstants.ApplicationScheme, opc => 
    {
        opc.LoginPath = "/user/login";
        opc.AccessDeniedPath = "/user/login";
    }
);

// CONSTRUCCION DE LA APLICACIONES:
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//EJECUTAR LA APLICACION.
app.Run();
