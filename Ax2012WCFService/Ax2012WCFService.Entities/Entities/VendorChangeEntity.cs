using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class VendorChangeEntity
    {
        public int VendorChangeId { get; set; }
        public string AccountNum { get; set; }
        public decimal CreditMax { get; set; }
        public string Currency { get; set; }
        public string CompanyId { get; set; }
        public string InventSiteId { get; set; }
        public string InvoiceAccount { get; set; }
        public string Name { get; set; }
        public string NameAlias { get; set; }
        public string PaymMode { get; set; }
        public string PaymModeInbound { get; set; }
        public string StagingId { get; set; }
        public string TaxGroup { get; set; }
        public string TaxGroupInbound { get; set; }
        public string VatNum { get; set; }
        public string CreditRating { get; set; }
        public string VendGroup { get; set; }
        public string PaymSpecInbound { get; set; }
        public string PaymSpec { get; set; }
        public string PaymSchedInbound { get; set; }
        public string PaymSched { get; set; }
        public string BankAccount { get; set; }
        public Nullable<int> Blocked { get; set; }
        public string YourAccountNum { get; set; }
        public string LineOfBusinessId { get; set; }
        public string SegmentId { get; set; }
        public string SubsegmentId { get; set; }
        public string PaymdayId { get; set; }
        public string CompanyChainId { get; set; }
        public string Memo { get; set; }
        public Nullable<int> AllowSplitPack { get; set; }
        public string Message01 { get; set; }
        public string Message02 { get; set; }
        public string Message03 { get; set; }
        public string Message04 { get; set; }
        public string Message05 { get; set; }
        public Nullable<int> MinorderValue { get; set; }
        public Nullable<int> SupplierLeadTime { get; set; }
        public string TheirCode { get; set; }
        public Nullable<int> Internal { get; set; }
        public Nullable<int> PrintSupplier { get; set; }
        public Nullable<int> ExtractStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExtractedDate { get; set; }
        public int InsUpdDelete { get; set; }
    }
}
