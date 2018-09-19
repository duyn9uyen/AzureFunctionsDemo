using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.POCOs
{
    public class Nutrition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nutrition_id { get; set; }
        public string Calories { get; set; }
        public string Totalfat { get; set; }
        public string Saturatedfat { get; set; }
        public string Cholesterol { get; set; }
        public string Sodium { get; set; }
        public string Totalcarbohydrate { get; set; }
        public string Dietaryfiber { get; set; }
        public string Protein { get; set; }
        public string Vitamina { get; set; }
        public string Vitaminc { get; set; }
        public string Calcium { get; set; }
        public string Iron { get; set; }
    }
}
