namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Przesylka")]
    public partial class Przesylka
    {
        [Key]
        public int id_przesylki { get; set; }

        public int id_adresu { get; set; }

        public int id_kuriera { get; set; }

        [StringLength(1)]
        public string czy_dostarczona { get; set; }

        public DateTime? data_wyslania { get; set; }

        public DateTime? data_dostarczenia { get; set; }

        [Required]
        [StringLength(30)]
        public string rodzaj { get; set; }

        public int Adresprzesylkiid_adresu { get; set; }

        public int Kurierid_kuriera { get; set; }

        [Column("dbo.RodzajPrzesylkirodzaj")]
        [Required]
        [StringLength(30)]
        public string dbo_RodzajPrzesylkirodzaj { get; set; }

        public virtual Adresprzesylki Adresprzesylki { get; set; }
    }
}
