﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Ax2012InterfaceEntities : DbContext
    {
        public Ax2012InterfaceEntities()
            : base("name=Ax2012InterfaceEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddressChange> AddressChanges { get; set; }
        public virtual DbSet<CustChange> CustChanges { get; set; }
        public virtual DbSet<SalesInvoiceJournal> SalesInvoiceJournals { get; set; }
        public virtual DbSet<VendorChange> VendorChanges { get; set; }
        public virtual DbSet<AOMProductChange> AOMProductChanges { get; set; }
        public virtual DbSet<AOMPurchaseDeliveryHeader> AOMPurchaseDeliveryHeaders { get; set; }
        public virtual DbSet<AOMPurchaseDeliveryLine> AOMPurchaseDeliveryLines { get; set; }
        public virtual DbSet<PurchaseInvoiceHeader> PurchaseInvoiceHeaders { get; set; }
        public virtual DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
    
        public virtual ObjectResult<spExtractAxPaymentTerms_Result> spExtractAxPaymentTerms(string dataAreaId)
        {
            var dataAreaIdParameter = dataAreaId != null ?
                new ObjectParameter("DataAreaId", dataAreaId) :
                new ObjectParameter("DataAreaId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spExtractAxPaymentTerms_Result>("spExtractAxPaymentTerms", dataAreaIdParameter);
        }
    
        public virtual int spUpdateBatchStatus(string batchJob, string companyID)
        {
            var batchJobParameter = batchJob != null ?
                new ObjectParameter("BatchJob", batchJob) :
                new ObjectParameter("BatchJob", typeof(string));
    
            var companyIDParameter = companyID != null ?
                new ObjectParameter("CompanyID", companyID) :
                new ObjectParameter("CompanyID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdateBatchStatus", batchJobParameter, companyIDParameter);
        }
    
        public virtual int spUpdateAccountsOnHoldJournal(string companyID)
        {
            var companyIDParameter = companyID != null ?
                new ObjectParameter("CompanyID", companyID) :
                new ObjectParameter("CompanyID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdateAccountsOnHoldJournal", companyIDParameter);
        }
    }
}