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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Facet_id { get; set; }

        [Key]
        [Column(Order = 0)]
        public int Taxonomy_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string Name { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
