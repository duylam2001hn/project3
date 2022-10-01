namespace Gallery_art_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_buy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order_buy()
        {
            order_detail = new HashSet<order_detail>();
        }

        public int Id { get; set; }

        public double Total_price { get; set; }

        public int Cus_id { get; set; }

        [StringLength(100)]
        public string Date_start { get; set; }

        public int status { get; set; }

        public int Payment_id { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Input characters in aphalbet please")]
        [StringLength(3,MinimumLength =2,ErrorMessage = "Input only 2-3 characters please! ")]
        public string Country_code { get; set; }
        [Required]
        [RegularExpression("^[0-9 ]*$", ErrorMessage ="Input only number please")]
        [DataType(DataType.PostalCode,ErrorMessage = "Input Zip code again please")]
        public string Zip_code { get; set; }
        

        [Required]
        [RegularExpression("^[0-9 ]*$", ErrorMessage = "Input only number please")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Input phone number again please")]
        public string PhoneNumber { get; set; }

        
        [Required]
        [RegularExpression("^[A-Za-z0-9 ]*$", ErrorMessage = "Not Input special characters please")]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Input characters in aphalbet please")]
        public string City { get; set; }

        [Required]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Input characters in aphalbet please")]
        public string Recipient { get; set; }


        public virtual customer customer { get; set; }

        public virtual payment_method payment_method { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
    }
}
