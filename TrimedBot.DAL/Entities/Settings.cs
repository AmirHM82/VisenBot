using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot.DAL.Entities
{
    public class Settings
    {
        public int Id { get; set; }
        public bool IsResponsingAvailable { get; set; } = true;
        #region Ads
        public decimal PerMemberAdsPrice { get; set; }
        public decimal BasicAdsPrice { get; set; }
        public byte NumberOfAdsPerDay { get; set; }
        #endregion
    }
}
