namespace Gallery_art_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("artist")]
    public partial class artist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public artist()
        {
            artworks = new HashSet<artwork>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Certificate { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Style { get; set; }

        [StringLength(255)]
        public string Expire_date { get; set; }

        public int Cus_id { get; set; }
        
        [Required]
        [StringLength(255)]
        [RegularExpression("^[A-Za-z0-9 ]*$", ErrorMessage = "Not Input special characters please")]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Input characters in aphalbet please")]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Input characters in aphalbet please")]
        public string Country { get; set; }

        public virtual customer customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<artwork> artworks { get; set; }
    }
}
