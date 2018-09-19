using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.POCOs
{
    public class Recipe
    {
        [Key]
        public int Recipe_id { get; set; }
        public Nutrition Nutrition { get; set; }
        //public List<Facet> Facet { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Facet> Facets { get; set; }
    }
}
