using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionV1
{
    [XmlRoot(ElementName = "RecipeSet")]
    public class RecipeSet
    {
        public RecipeSet()
        {
            Recipes = new Recipes();
        }
        [XmlElement(ElementName = "TaxonomyTypes")]
        public TaxonomyTypes TaxonomyTypes { get; set; }
        [XmlElement(ElementName = "Recipes")]
        public Recipes Recipes { get; set; }
    }

    [XmlRoot(ElementName = "Taxonomy")]
    public class Taxonomy
    {
        [XmlAttribute(AttributeName = "Taxonomy_id")]
        public string Taxonomy_id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "TaxonomyTypes")]
    public class TaxonomyTypes
    {
        public TaxonomyTypes()
        {
            Taxonomy = new List<Taxonomy>();
        }

        [XmlElement(ElementName = "Taxonomy")]
        public List<Taxonomy> Taxonomy { get; set; }
    }

    [XmlRoot(ElementName = "Nutrition")]
    public class Nutrition
    {
        [XmlElement(ElementName = "calories")]
        public string Calories { get; set; }
        [XmlElement(ElementName = "totalfat")]
        public string Totalfat { get; set; }
        [XmlElement(ElementName = "saturatedfat")]
        public string Saturatedfat { get; set; }
        [XmlElement(ElementName = "cholesterol")]
        public string Cholesterol { get; set; }
        [XmlElement(ElementName = "sodium")]
        public string Sodium { get; set; }
        [XmlElement(ElementName = "totalcarbohydrate")]
        public string Totalcarbohydrate { get; set; }
        [XmlElement(ElementName = "dietaryfiber")]
        public string Dietaryfiber { get; set; }
        [XmlElement(ElementName = "protein")]
        public string Protein { get; set; }
        [XmlElement(ElementName = "vitamina")]
        public string Vitamina { get; set; }
        [XmlElement(ElementName = "vitaminc")]
        public string Vitaminc { get; set; }
        [XmlElement(ElementName = "calcium")]
        public string Calcium { get; set; }
        [XmlElement(ElementName = "iron")]
        public string Iron { get; set; }
    }

    [XmlRoot(ElementName = "Facet")]
    public class Facet
    {
        [XmlAttribute(AttributeName = "Taxonomy_id")]
        public string Taxonomy_id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "Recipe")]
    public class Recipe
    {
        [XmlElement(ElementName = "Nutrition")]
        public Nutrition Nutrition { get; set; }
        [XmlElement(ElementName = "Facet")]
        public List<Facet> Facet { get; set; }
        [XmlAttribute(AttributeName = "author")]
        public string Author { get; set; }
        [XmlAttribute(AttributeName = "createdDate")]
        public string CreatedDate { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "recipe_id")]
        public string Recipe_id { get; set; }
    }

    [XmlRoot(ElementName = "Recipes")]
    public class Recipes
    {
        public Recipes()
        {
            Recipe = new List<Recipe>();
        }

        [XmlElement(ElementName = "Recipe")]
        public List<Recipe> Recipe { get; set; }
    }

    
}
