namespace Znachor.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetUserRole
    {
        [Key]
        [Column(Order = 0)]
        public string AspNetRolesid { get; set; }

        [Key]
        [Column(Order = 1)]
        public string AspNetUsersid { get; set; }
    }
}
