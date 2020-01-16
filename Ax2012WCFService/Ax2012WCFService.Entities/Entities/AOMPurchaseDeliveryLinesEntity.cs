using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class AOMPurchaseDeliveryLinesEntity
    {
        public int AOMPurchaseDeliveryLineID { get; set; }
        public Nullable<int> PurchaseHeaderID { get; set; }
        public string EXTERNAL_ORD_ID { get; set; }
        public string EXTERNAL_DEL_ID { get; set; }
        public System.DateTime DELIVERYDATE { get; set; }
        public string ITEMID { get; set; }
        public decimal LINEAMOUNT { get; set; }
        public decimal PURCHPRICE { get; set; }
        public decimal PURCHQTY { get; set; }
        public int TYPE { get; set; }
        public string NAME { get; set; }
        public string PROGRESSITEMID { get; set; }
        public decimal PRICEUNIT { get; set; }
        public decimal PURCHRECEIVEDNOW { get; set; }
        public long DEFAULTDIMENSION { get; set; }
        public decimal LINENUM { get; set; }
        public string INVENTTRANSID { get; set; }
        public string EXTERNAL_INV_ID { get; set; }
        public decimal VATLINEAMOUNT { get; set; }
        public string PURCHID { get; set; }
        public System.DateTime MODIFIEDDATETIME { get; set; }
        public System.DateTime CREATEDDATETIME { get; set; }
        public string DATAAREAID { get; set; }
        public Nullable<int> Seq { get; set; }	
    }
}
