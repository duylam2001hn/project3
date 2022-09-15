namespace Gallery_art_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_detail
    {
        public int Id { get; set; }

        public int Order_id { get; set; }

        public int Art_id { get; set; }

        public int Quantity { get; set; }

        public virtual artwork artwork { get; set; }

        public virtual order_buy order_buy { get; set; }
    }
}
