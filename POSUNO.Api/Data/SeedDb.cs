using Microsoft.EntityFrameworkCore;
using POSUNO.Api.Data.Entities;
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
        private readonly DataContext _context;
       public SeedDb(DataContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //Si la base de datos no existe la Crea, si exite la actualiza en caso de estar desactualizada.
            await CheckUserAsync();
            await CheckCustomersAsync();
            await CheckProductsAsync();
        }
        private async Task CheckUserAsync()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new Entities.User { Email = "abel@yopmail.com", FirstName = "Abel", LastName = "Hernandez", Password = "123456" });
                _context.Users.Add(new Entities.User { Email = "juan@yopmail.com", FirstName = "Juan", LastName = "Zuluaga", Password = "123456" });
                await _context.SaveChangesAsync();
            }
        }

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
