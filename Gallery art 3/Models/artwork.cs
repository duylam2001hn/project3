namespace Gallery_art_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("artwork")]
    public partial class artwork
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public artwork()
        {
            bids = new HashSet<bid>();
            favorite_artwork = new HashSet<favorite_artwork>();
            order_detail = new HashSet<order_detail>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Vui lòng nh?p l?i giá")]
        public double? Price { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Year { get; set; }

        
        [StringLength(100)]
        public string img_path { get; set; }

        public int artist_id { get; set; }

        public int cate_id { get; set; }

        public int status { get; set; }

        public virtual artist artist { get; set; }

        public virtual category category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bid> bids { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<favorite_artwork> favorite_artwork { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
    }
}
