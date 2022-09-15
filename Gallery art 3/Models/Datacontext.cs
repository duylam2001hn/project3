using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Gallery_art_3.Models
{
    public partial class Datacontext : DbContext
    {
        public Datacontext()
            : base("name=Datacontext")
        {
        }

        public virtual DbSet<artist> artists { get; set; }
        public virtual DbSet<artwork> artworks { get; set; }
        public virtual DbSet<bid> bids { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<favorite_artwork> favorite_artwork { get; set; }

        public virtual DbSet<Update_bidding> update_bidding { get; set; }
        public virtual DbSet<order_buy> order_buy { get; set; }
        public virtual DbSet<order_detail> order_detail { get; set; }
        public virtual DbSet<payment_method> payment_method { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<artist>()
                .Property(e => e.Certificate)
                .IsUnicode(false);

            modelBuilder.Entity<artist>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<artist>()
                .Property(e => e.Style)
                .IsUnicode(false);

            modelBuilder.Entity<artist>()
                .Property(e => e.Expire_date)
                .IsUnicode(false);

            modelBuilder.Entity<artist>()
                .HasMany(e => e.artworks)
                .WithRequired(e => e.artist)
                .HasForeignKey(e => e.artist_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<artwork>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<artwork>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<artwork>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<artwork>()
                .Property(e => e.img_path)
                .IsUnicode(false);

            modelBuilder.Entity<artwork>()
                .HasMany(e => e.bids)
                .WithRequired(e => e.artwork)
                .HasForeignKey(e => e.Art_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<artwork>()
                .HasMany(e => e.favorite_artwork)
                .WithOptional(e => e.artwork)
                .HasForeignKey(e => e.Artwork_id);

            modelBuilder.Entity<artwork>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.artwork)
                .HasForeignKey(e => e.Art_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<bid>()
                .Property(e => e.Date_start)
                .IsUnicode(false);

            modelBuilder.Entity<bid>()
                .Property(e => e.Date_end)
                .IsUnicode(false);

            modelBuilder.Entity<bid>()
                .HasMany(e => e.update_bidding)
                .WithOptional(e => e.bid)
                .HasForeignKey(e => e.Bid_id);

            modelBuilder.Entity<category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.artworks)
                .WithRequired(e => e.category)
                .HasForeignKey(e => e.cate_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.artists)
                .WithRequired(e => e.customer)
                .HasForeignKey(e => e.Cus_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.favorite_artwork)
                .WithOptional(e => e.customer)
                .HasForeignKey(e => e.Cus_id);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.order_buy)
                .WithRequired(e => e.customer)
                .HasForeignKey(e => e.Cus_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.update_bidding)
                .WithOptional(e => e.customer)
                .HasForeignKey(e => e.Cus_id);

            modelBuilder.Entity<order_buy>()
                .Property(e => e.Date_start)
                .IsUnicode(false);

            modelBuilder.Entity<order_buy>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.order_buy)
                .HasForeignKey(e => e.Order_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<payment_method>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<payment_method>()
                .HasMany(e => e.order_buy)
                .WithRequired(e => e.payment_method)
                .HasForeignKey(e => e.Payment_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
