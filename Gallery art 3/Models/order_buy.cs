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

        public virtual customer customer { get; set; }

        public virtual payment_method payment_method { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }
    }
}
