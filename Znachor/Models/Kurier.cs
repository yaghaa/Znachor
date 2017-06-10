namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kurier")]
    public partial class Kurier
    {
        [Key]
        public int id_kuriera { get; set; }

        [Required]
        [StringLength(50)]
        public string nazwa { get; set; }

        [Column(TypeName = "money")]
        public decimal koszt { get; set; }

        public int max_czas_dostawy { get; set; }
    }
}
