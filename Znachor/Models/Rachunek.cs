namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rachunek")]
    public partial class Rachunek
    {
        [Key]
        public int id_rachunku { get; set; }

        [Column(TypeName = "money")]
        public decimal towary_netto { get; set; }

        [Column(TypeName = "money")]
        public decimal przesylka_netto { get; set; }

        [Column(TypeName = "money")]
        public decimal suma_vat { get; set; }

        [Column(TypeName = "money")]
        public decimal rabat { get; set; }

        [Column(TypeName = "money")]
        public decimal suma_brutto { get; set; }

        [StringLength(1)]
        public string czy_zaplacony { get; set; }
    }
}
