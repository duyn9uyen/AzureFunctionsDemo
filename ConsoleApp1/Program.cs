using DataAccess;
using DataAccess.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var tax = new Taxonomy()
            {
                Name = "Category",
                Taxonomy_id = 1
            };

            var facet = new Facet()
            {
                Name = "facet1",
                Taxonomy_id = 1
            };

            var nutrition = new Nutrition()
            {
                Calories = "5g",
                Totalfat = "0g"
            };

            var recipe = new Recipe();
            recipe.Author = "me";
            recipe.CreatedDate = DateTime.Now;
            recipe.Description = "debugging";
            recipe.Name = "test recipe";
            recipe.Recipe_id = 1;

            recipe.Nutrition = nutrition;
            recipe.Facets = new List<Facet>();
            recipe.Facets.Add(facet);
            recipe.Taxonomies = new List<Taxonomy>();
            recipe.Taxonomies.Add(tax);

            using (var ctx = new RecipeContext())
            {
                ctx.Recipe.Add(recipe);
                ctx.SaveChanges();
            }
        }
    }
}
