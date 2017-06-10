namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zamowienie")]
    public partial class Zamowienie
    {
        [Key]
        public int id_zamowienia { get; set; }

        [Required]
        [StringLength(128)]
        public string nr_rachunku { get; set; }

        public int id_przesylki { get; set; }

        [Required]
        [StringLength(128)]
        public string Klientid_klienta { get; set; }

        [Required]
        [StringLength(30)]
        public string Platnoscrodzaj { get; set; }

        public int Przesylkaid_przesylki { get; set; }
    }
}
