namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Adresprzesylki")]
    public partial class Adresprzesylki
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adresprzesylki()
        {
            Przesylkas = new HashSet<Przesylka>();
        }

        [Key]
        public int id_adresu { get; set; }

        [Required]
        [StringLength(40)]
        public string miejscowosc { get; set; }

        [Required]
        [StringLength(50)]
        public string ulica { get; set; }

        public int nrdomu { get; set; }

        public int? nrlokalu { get; set; }

        [Required]
        [StringLength(6)]
        public string kodpocztowy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Przesylka> Przesylkas { get; set; }
    }
}
