using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class PurchaseInvoiceLineEntity
    {
        public long PurchaseInvoiceLinesID { get; set; }
        public string EXTERNAL_ORD_ID { get; set; }
        public string ItemID { get; set; }
        public string Name { get; set; }
        public int InvoiceLineNum { get; set; }
        public string EXTERNAL_DEL_ID { get; set; }
        public Nullable<decimal> PurchPrice { get; set; }
        public int PurchQty { get; set; }
        public int LineNum { get; set; }
        public Nullable<decimal> LINE_TOTAL_EX_VAT { get; set; }
        public Nullable<decimal> PurchReceivedNow { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string EXTERNAL_INV_ID { get; set; }
        public short PriceUnit { get; set; }
        public System.DateTime CREATEDDATE { get; set; }
        public string VendAccount { get; set; }
        public System.Guid InvoiceGUID { get; set; }
        public System.Guid BatchIdentifier { get; set; }
        public string CompanyID { get; set; }
    }
}
