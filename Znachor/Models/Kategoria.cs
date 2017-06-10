namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kategoria")]
    public partial class Kategoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategoria()
        {
            Kategoria1 = new HashSet<Kategoria>();
            PosiadaKategories = new HashSet<PosiadaKategorie>();
        }

        [Key]
        public int id_kategorii { get; set; }

        public int? Kat_id_kategorii { get; set; }

        public int? Kategoriaid_kategorii { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kategoria> Kategoria1 { get; set; }

        public virtual Kategoria Kategoria2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PosiadaKategorie> PosiadaKategories { get; set; }
    }
}
