namespace BlogSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Makales = new HashSet<Makale>();
            Yorums = new HashSet<Yorum>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(10)]
        public string KullaniciAdi { get; set; }

         
        [Required]
        [StringLength(10)]
        public string Sifre { get; set; }

        [StringLength(50)]
        public string isim { get; set; }

        [StringLength(50)]
        public string Soyisim { get; set; }

        public int? YetkiId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? KayitTarihi { get; set; }

        [StringLength(150)]
        [RegularExpression(" /^[a-zA-Z][.-]*[a-zA-Z0-9]@[a-zA-Z0-9][.-]*[a-zA-Z0-9].[a-zA-Z][a-zA-Z.]*[a-zA-Z]+$/")]
        public string Email { get; set; }

        public virtual Yetkilendirme Yetkilendirme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Makale> Makales { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
