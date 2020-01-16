﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class CustChangeEntity
    {
        public int CustChangeID { get; set; }
        public System.Guid StagingId { get; set; }
        public string AccountNum { get; set; }
        public string Name { get; set; }
        public string NameAlias { get; set; }
        public int AccountStatement { get; set; }
        public string Currency { get; set; }
        public string CustGroup { get; set; }
        public int Blocked { get; set; }
        public Nullable<decimal> CreditMax { get; set; }
        public Nullable<int> MandatoryCreditLimit { get; set; }
        public string CreditRating { get; set; }
        public string InvoiceAccount { get; set; }
        public string VendAccount { get; set; }
        public string InventLocation { get; set; }
        public Nullable<int> InvoiceAddress { get; set; }
        public string OurAccountNum { get; set; }
        public string PaymIdType { get; set; }
        public string PaymDayId { get; set; }
        public string PaymMode { get; set; }
        public string PaymTermId { get; set; }
        public string SalesGroup { get; set; }
        public string TaxGroup { get; set; }
        public string VATNum { get; set; }
        public string MainContactWorker { get; set; }
        public string SalesDistrictId { get; set; }
        public string LineOfBusinessId { get; set; }
        public string SegmentId { get; set; }
        public string SubsegmentId { get; set; }
        public string CompanyChainId { get; set; }
        public string CustClassificationId { get; set; }
        public string INVENTSITEID { get; set; }
        public Nullable<decimal> DiscountRate { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<long> Tec_PriceBand { get; set; }
        public Nullable<long> Tec_MinOrderBreak { get; set; }
        public Nullable<int> AllowSplitPack { get; set; }
        public Nullable<int> MergeBackOrders { get; set; }
        public Nullable<int> MergeDeliveries { get; set; }
        public Nullable<int> MergeOrders { get; set; }
        public Nullable<int> DELIVERYSEQ { get; set; }
        public Nullable<int> DeliverComplete { get; set; }
        public Nullable<int> DeliveryPricing { get; set; }
        public Nullable<int> OrderNotes { get; set; }
        public Nullable<int> CUSTDNS { get; set; }
        public Nullable<int> ACKNOWLEDGEMENTS { get; set; }
        public Nullable<int> AckFormat { get; set; }
        public Nullable<int> AckPricing { get; set; }
        public Nullable<int> AckShowAllocation { get; set; }
        public Nullable<int> AllowBackorders { get; set; }
        public Nullable<int> ApplyCharge { get; set; }
        public Nullable<int> MinOrderValue { get; set; }
        public string Tec_ChargeCode { get; set; }
        public Nullable<int> ForcedSwitchSell { get; set; }
        public Nullable<int> InvoiceSequence { get; set; }
        public Nullable<int> InvoiceCompleteOrder { get; set; }
        public string Tec_InvoiceGroup { get; set; }
        public string Tec_BillingOptions { get; set; }
        public Nullable<int> FiscalYearEnd { get; set; }
        public Nullable<int> CreditCardIndicator { get; set; }
        public Nullable<int> WebEnabled { get; set; }
        public Nullable<long> Tec_WebSite { get; set; }
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string Msg3 { get; set; }
        public string Msg4 { get; set; }
        public string Msg5 { get; set; }
        public Nullable<int> ManualAddress { get; set; }
        public Nullable<int> PrintCustomer { get; set; }
        public Nullable<int> HouseAccount { get; set; }
        public Nullable<int> ExtractStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExtractedDate { get; set; }
        public int InsUpdDelete { get; set; }
        public string CompanyID { get; set; }

    }
}