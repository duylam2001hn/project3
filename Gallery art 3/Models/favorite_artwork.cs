namespace Gallery_art_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class favorite_artwork
    {
        public int Id { get; set; }

        public int? Cus_id { get; set; }

        public int? Artwork_id { get; set; }

        public virtual artwork artwork { get; set; }

        public virtual customer customer { get; set; }
    }
}
