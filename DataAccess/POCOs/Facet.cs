using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.POCOs
{
    public class Facet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Facet_id { get; set; }
        //[Index("IX_UniqueFacetNameAndTaxonomy_id", 1, IsUnique = true)]
        public int Taxonomy_id { get; set; }

        // Todo: Make taxonomy_id and Name unique
        //[Index("IX_UniqueFacetNameAndTaxonomy_id", 2, IsUnique = true)]
        //[Column(TypeName = "NVARCHAR")]
        //[StringLength(500)]
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
