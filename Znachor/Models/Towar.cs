namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Towar")]
    public partial class Towar
    {
        [Key]
        public int id_towaru { get; set; }

        [Required]
        [StringLength(50)]
        public string nazwa { get; set; }

        [Column(TypeName = "money")]
        public decimal cena_netto { get; set; }

        public int ilosc_w_magazynie { get; set; }

        [Required]
        [StringLength(40)]
        public string producent { get; set; }

        [Required]
        [StringLength(25)]
        public string forma { get; set; }

        [Required]
        [StringLength(255)]
        public string sklad { get; set; }

        [Required]
        [StringLength(255)]
        public string szczegoly { get; set; }
    }
}
