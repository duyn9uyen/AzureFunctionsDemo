using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RecipeContext : DbContext
    {
        public RecipeContext() : base("name=RecipeConnection")
        {
            Database.SetInitializer<RecipeContext>(new CreateDatabaseIfNotExists<RecipeContext>());
        }

        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Facet> Facet { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Taxonomy> Taxonomy { get; set; }
    }
}
