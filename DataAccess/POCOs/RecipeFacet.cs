//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataAccess.POCOs
//{
//    public class RecipeFacet
//    {
//        public int RecipeFacetId { get; set; }

//        //EF Core does not support creating a composite key using the Key attribute
//        [Key] 
//        [Column(Order = 1)]
//        public int Recipe_id { get; set; }

//        [Key]
//        [Column(Order = 2)]
//        public int Facet_id { get; set; }

//        public virtual ICollection<Recipe> Recipes { get; set; }
//        public virtual ICollection<Facet> Facets { get; set; }
//    }
//}
