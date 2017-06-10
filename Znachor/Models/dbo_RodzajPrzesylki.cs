namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[dbo.RodzajPrzesylki]")]
    public partial class dbo_RodzajPrzesylki
    {
        [Key]
        [StringLength(30)]
        public string rodzaj { get; set; }

        [Column(TypeName = "money")]
        public decimal koszt_przesylki { get; set; }
    }
}
