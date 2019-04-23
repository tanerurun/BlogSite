namespace BlogSite.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BlogDB : DbContext
    {
        public BlogDB()
            : base("name=BlogDB")
        {
        }

        public virtual DbSet<Etiket> Etikets { get; set; }
        public virtual DbSet<Kategori> Kategoris { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<Makale> Makales { get; set; }
        public virtual DbSet<Yetkilendirme> Yetkilendirmes { get; set; }
        public virtual DbSet<Yorum> Yorums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Makales)
                .WithMany(e => e.Etikets)
                .Map(m => m.ToTable("EtiketMakale").MapLeftKey("EtiketID").MapRightKey("MakaleID"));

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Makales)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Yorums)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Makale>()
                .HasMany(e => e.Yorums)
                .WithRequired(e => e.Makale)
                .HasForeignKey(e => e.KullaniciID)
                .WillCascadeOnDelete(false);
        }
    }
}
