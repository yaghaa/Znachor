namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RodzajPlatnosci")]
    public partial class RodzajPlatnosci
    {
        [Key]
        [StringLength(30)]
        public string rodzaj { get; set; }

        [Required]
        [StringLength(255)]
        public string szczegoly { get; set; }

        [Required]
        [StringLength(26)]
        public string nr_konta { get; set; }
    }
}
