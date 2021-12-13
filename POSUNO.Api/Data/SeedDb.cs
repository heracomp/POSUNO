using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;
using POSUNO.Api.Enums;
using POSUNO.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSUNO.Api.Data
{
    public class SeedDb
    {
        //Procedimiento para borrar la base de datos y regenerarla de nuevo
        //1.- Primero desactivar todos los proyectos excepto el POSUNO.Api en (Configuration Managr...)
        //2.- Recompilar(Rebuild) el proyecto POSUNO.Api
        //3.- En Package Manager Console ejecutar PM> drop-database
        //4.- En Package Manager Console ejecutar PM> add-migration AddCustomers
        //5.- En Package Manager Console ejecutar PM> update-database

        //Procedimiento para borrar la base de datos y regenrarla de nuevo despues de modificar 
        //el DataContext al agregar la interface IdentityDbContext<User>
        //1.- Removemos todos las migraciones        PM> remove-migration
        //2.- Eliminamos la base de datos actual     PM> drop-database
        //3.- Removemos la ultima de las migraciones PM> remove-migration
        //4.- Agregamos una nueva migración          PM> add-migration AddUserTables
        //5.- Actualizamos la base de datos          PM> update-database

        //Despues de publicar el proyecto en el servidor, es necesario agregar en el web.config
        // Para que el servidor acepte el metodo PUT.
        // <system.webServer>
        //	  <modules>
        //      <remove name = "WebDAVModule"/>
        //    </ modules >
        // </system.webServer>

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //Si la base de datos no existe la Crea, si exite la actualiza en caso de estar desactualizada.
            await CheckRolesAsync();
            await CheckUserAsync("Juan","Zuluaga","juan@yopmail.com","322 311 4620");
            await CheckUserAsync("Abel", "Hernandez", "abel@yopmail.com", "452 562 854");
            await CheckCustomersAsync();
            await CheckProductsAsync();
        }

        private async Task CheckUserAsync(string firstName, string lasName, string email, string phone)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lasName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, UserType.Admin.ToString());
                await _userHelper.AddUserToRoleAsync(user, UserType.User.ToString());
            }
            
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        //private async Task CheckUserAsync()
        //{
        //    if (!_context.Users.Any())
        //    {
        //        _context.Users.Add(new Entities.User { Email = "abel@yopmail.com", FirstName = "Abel", LastName = "Hernandez", Password = "123456" });
        //        _context.Users.Add(new Entities.User { Email = "juan@yopmail.com", FirstName = "Juan", LastName = "Zuluaga", Password = "123456" });
        //        await _context.SaveChangesAsync();
        //    }
        //}

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();
                for(int i=1; i<=200; i++)
                {
                    _context.Products.Add(new Product
                    { 
                        Name=$"Producto-{i}",
                        Description=$"Descripción-{i}",
                        Price=random.Next(5,1000),
                        stock=random.Next(0,200),
                        IsActive=true,
                        User=user
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCustomersAsync()
        {
            if (!_context.Customers.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 1; i <= 200; i++)
                {
                    _context.Customers.Add(new Customer
                    {
                        FirstName = $"Cliente-{i}",
                        LasttName = $"Apellido-{i}",
                        Email=$"cliente{i}@yopmai.com",
                        Phonenumber = "999 999 9999",
                        Address = $"Call-{i}, Colonia-{i}",
                        IsActive=true,
                        User = user
                    });
                }
                await _context.SaveChangesAsync();
            }
        }

    }
}
