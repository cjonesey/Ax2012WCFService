using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class AOMProductChangeEntity
    {
        public int ProductChangeID { get; set; }
        public string ITEMID { get; set; }
        public string NAME { get; set; }
        public decimal WIDTH { get; set; }
        public decimal HEIGHT { get; set; }
        public decimal DEPTH { get; set; }
        public decimal NETWEIGHT { get; set; }
        public string INTRACODE { get; set; }
        public string PRIMARYVENDID { get; set; }
        public string ORIGCOUNTRYID { get; set; }
        public string STOCK_OWNER { get; set; }
        public string VAT_GROUP { get; set; }
        public string DESCRIPTION_5 { get; set; }
        public string DESCRIPTION_4 { get; set; }
        public string DESCRIPTION_3 { get; set; }
        public string DESCRIPTION_2 { get; set; }
        public string PRODUCT_OWNER { get; set; }
        public string MANUFACTURER { get; set; }
        public string MANUFACTURER_PRODCODE { get; set; }
        public int DELIVERYTIME { get; set; }
        public decimal STANDARD_COST { get; set; }
        public int SPECIAL { get; set; }
        public decimal SELL_PRICEUNIT { get; set; }
        public decimal SELL_PRICE { get; set; }
        public int PAIDSTOCK { get; set; }
        public string ITEMGROUP { get; set; }
        public int TYPE { get; set; }
        public string UNSPSC { get; set; }
        public int SHELFLIFE { get; set; }
        public string STATUSBLOCKED { get; set; }
        public long BOSSF_GROUP { get; set; }
        public long BOSSF_RANGE { get; set; }
        public long PRODUCT_RANGE { get; set; }
        public long PRODUCT_GROUP { get; set; }
        public string MASTERITEMID { get; set; }
        public string DATAAREAID { get; set; }
        public long PARTITION { get; set; }
        public byte ExtractStatus { get; set; }
    }
}
