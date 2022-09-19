using HaberEcommerceSite.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace HaberEcommerceSite.Data
{
    public class HaberSeeder
    {
        private readonly HaberContext databaseContext;

        private readonly UserManager<User> userManager;

        public HaberSeeder(HaberContext context, UserManager<User> userManager)
        {
            this.databaseContext = context;

            this.userManager = userManager;
        }

        public async Task Seed()
        {
            await databaseContext.Database.EnsureCreatedAsync();

            // Comprueba si hay algun usuario creado antes de poblar la tabla Usuarios.
           if (userManager.Users.Any())
            {
                return;
            }

             var result = await userManager.CreateAsync(new User() { UserName = "Prueba" }, "Prueba123%");
            


            // Comprueba si hay alguna categoria creada antes de poblar la tabla Categorias.
            if (databaseContext.Categories.Any())
            {
                return;
            }
    
            await databaseContext.Categories.AddAsync(new Category { Description = "Sin Categoria" });

            await databaseContext.Subcategories.AddAsync(new Subcategory { Description = "Sin Subcategoria" });

            await databaseContext.Authors.AddAsync(new Author { Name = "Sin Autor" });

            await databaseContext.Bookbindings.AddAsync(new Bookbinding { Description = "Sin Encuadernación" });

            await databaseContext.BookCollections.AddAsync(new BookCollection { Description = "Sin Colección" });

            await databaseContext.Editorials.AddAsync(new Editorial { Description = "Sin Editorial" });

            await databaseContext.SaveChangesAsync();          
        }
    }
}
