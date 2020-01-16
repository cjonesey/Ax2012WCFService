﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class SalesInvoiceJournalEntity
    {
        public long SalesInvoiceJournalID { get; set; }
        public string CompanyID { get; set; }
        public string Description { get; set; }
        public int JournalType { get; set; }
        public string JournalName { get; set; }
        public string AccountType { get; set; }
        public string CostCentre { get; set; }
        public string OffsetCostCentre { get; set; }
        public string OffsetAccountType { get; set; }
        public string Txt { get; set; }
        public string Invoice { get; set; }
        public System.DateTime DocumentDate { get; set; }
        public string DocumentNum { get; set; }
        public System.DateTime Due { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchRate { get; set; }
        public string PaymMode { get; set; }
        public string PaymReference { get; set; }
        public string PostingProfile { get; set; }
        public string TaxGroup { get; set; }
        public string TaxCode { get; set; }
        public string VatNumJournal { get; set; }
        public System.DateTime TransDate { get; set; }
        public string Account { get; set; }
        public string OffsetAccount { get; set; }
        public Nullable<int> ExtractStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExtractedDate { get; set; }
        public string StagingId { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
    }
}