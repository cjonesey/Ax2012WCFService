//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ax2012WCFService.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseInvoiceHeader
    {
        public long PurchaseInvoiceHeadersID { get; set; }
        public string EXTERNAL_INV_ID { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string VENDACCOUNT { get; set; }
        public string EXTERNAL_ORD_ID { get; set; }
        public string EXTERNAL_DEL_ID { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> Order_Total_Net { get; set; }
        public Nullable<decimal> Order_Total_Gross { get; set; }
        public string VendorRef { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid BatchIdentifier { get; set; }
        public Nullable<System.Guid> ExtractBatchID { get; set; }
        public System.Guid InvoiceGUID { get; set; }
        public Nullable<System.DateTime> ExtractedDate { get; set; }
        public Nullable<short> ErrorStatus { get; set; }
        public Nullable<short> ExtractStatus { get; set; }
        public Nullable<short> OrderVerfied { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string InvoiceType { get; set; }
        public string CompanyID { get; set; }
    }
}
