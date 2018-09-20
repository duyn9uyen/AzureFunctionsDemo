﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.POCOs
{
    public class Taxonomy
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Taxonomy_id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(500)]
        public string Name { get; set; }
    }
}
