using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.Database.Models
{
    public class Settings
    {
        public int Id { get; set; }
        #region Ads
        public decimal PerMemberAdsPrice { get; set; }
        public decimal BasicAdsPrice { get; set; }
        public byte NumberOfAdsPerDay { get; set; }
        #endregion
    }
}
