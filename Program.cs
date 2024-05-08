using StudentCatalog.ContextModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace StudentCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Creăm un obiect `builder` pentru a configura și construi aplicația web ASP.NET Core.
            var builder = WebApplication.CreateBuilder(args);

            // Adăugăm serviciile la container.

            // Adăugăm suport pentru controlere cu vizualizări (MVC).
            builder.Services.AddControllersWithViews();

            // Adăugăm și configurăm contextul de date, folosind SQL Server cu o conexiune specificată în fișierul de configurare.
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectDatabase")));

            // Configurăm serviciul de autentificare folosind cookie-uri.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // Definim calea unde utilizatorii vor fi redirecționați pentru autentificare.
                    options.LoginPath = "/Authentication/Login";

                    // Definim calea unde utilizatorii vor fi redirecționați dacă accesul este refuzat (de ex. pentru pagini cu restricții de acces).
                    options.AccessDeniedPath = "/Authentication/Forbidden"; // De adăugat ca acțiune în controler și view
                });

            // Construim aplicația finală folosind configurația specificată în `builder`.
            var app = builder.Build();

            // Configurăm conducta de procesare a cererilor HTTP (HTTP request pipeline).

            // Dacă aplicația nu rulează în mediul de dezvoltare, configurăm un handler pentru erori.
            if (!app.Environment.IsDevelopment())
            {
                // Definim ruta implicită pentru gestionarea erorilor.
                app.UseExceptionHandler("/Home/Error");

                // Adăugăm suport pentru HSTS (HTTP Strict Transport Security), pentru a preveni anumite tipuri de atacuri.
                // Valoarea implicită este 30 de zile, dar poate fi modificată în producție.
                app.UseHsts();
            }

            // Redirecționăm cererile HTTP către HTTPS.
            app.UseHttpsRedirection();

            // Permitem utilizarea fișierelor statice (CSS, JS, imagini) din directoare publice.
            app.UseStaticFiles();

            // Definim pipeline-ul de rutare pentru cereri.
            app.UseRouting();

            // Activăm autentificarea și autorizarea în pipeline-ul aplicației.
            app.UseAuthorization();

            // Definim ruta implicită a aplicației, astfel încât cererile să fie direcționate către controlerul și acțiunea specificată.
            // Dacă nu se specifică un controler și o acțiune în cerere, aceasta va fi redirecționată către `AuthenticationController` și acțiunea `Login`.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            // Pornim aplicația web și începem să ascultăm cereri.
            app.Run();

        }
    }
}
