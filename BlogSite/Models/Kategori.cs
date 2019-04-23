namespace BlogSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kategori")]
    public partial class Kategori
    {
        public int KategoriID { get; set; }

        [StringLength(50)]
        public string KategoriAdi { get; set; }
    }
}
