using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery_art_3.Models
{
    public class compareList : IEqualityComparer<Update_bidding>
    {
        public bool Equals(Update_bidding x, Update_bidding y)
        {
           return x.Cus_id == y.Cus_id && x.Bid_id == y.Bid_id;
        }

        public int GetHashCode(Update_bidding obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}