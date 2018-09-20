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
            //method1();
            method2();
        }

        static void method1()
        {
            var course1 = new Course()
            {
                Department_id = 100,
                Name = "English"
            };

            var course2 = new Course()
            {
                Department_id = 100,
                Name = "Spanish"
            };

            var course3 = new Course()
            {
                Department_id = 100,
                Name = "English"
            };

            var student1 = new Student();
            student1.Name = "james";
            student1.Courses = new List<Course>();
            student1.Courses.Add(course1);
            student1.Courses.Add(course2);
            student1.Courses.Add(course3);

            var student2 = new Student();
            student2.Name = "John";
            student2.Courses = new List<Course>();
            student2.Courses.Add(course1);
            student2.Courses.Add(course1);

            using (var ctx = new RecipeContext())
            {
                try
                {
                    ctx.Student.Add(student1);
                    ctx.Student.Add(student2);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }
        }

        static void method2()
        {
            var facet1 = new Facet()
            { 
                Name = "Lunch",
                Taxonomy_id = 1
            };

            var facet2 = new Facet()
            {
                Name = "Dinner",
                Taxonomy_id = 1
            };

            var facet3 = new Facet()
            {
                Name = "American",
                Taxonomy_id = 2
            };

            var recipe1 = new Recipe()
            {
                CreatedDate = DateTime.Now,
                Name = "Recipe 1"
            };
            recipe1.Facets = new List<Facet>();
            recipe1.Facets.Add(facet1);

            var recipe2 = new Recipe()
            {
                CreatedDate = DateTime.Now,
                Name = "Recipe 2"
            };
            recipe2.Facets = new List<Facet>();
            recipe2.Facets.Add(facet1);

            using (var ctx = new RecipeContext())
            {
                try
                {
                    ctx.Recipe.Add(recipe1);
                    ctx.Recipe.Add(recipe2);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
