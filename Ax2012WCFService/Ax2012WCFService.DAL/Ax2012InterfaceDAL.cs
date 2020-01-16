using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ax2012WCFService.Entities;
using Ax2012WCFService.Common;
using AutoMapper;


namespace Ax2012WCFService.DAL
{
    public class Ax2012InterfaceDAL
    {
        public Ax2012InterfaceDAL()
        {
            AutoMapperConfigurator.Configure();
        }
        /// <summary>
        /// Gets the customer changes for the customer
        /// </summary>
        /// <param name="companyID">Company ID on Ax</param>
        /// <returns></returns>
        public List<CustChangeEntity> GetCustomerChanges(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<CustChange> customerChanges = context.CustChanges.Where(l => l.CompanyID == companyID && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<CustChange>, List<CustChangeEntity>>(customerChanges);
            }

        }

        /// <summary>
        /// Get Address Changes
        /// </summary>
        /// <param name="companyID">Ax Company ID</param>
        /// <param name="batchSize">No of records to return</param>
        /// <returns></returns>
        public List<AddressChangeEntity> GetAddressChanges(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<AddressChange> addressChanges = context.AddressChanges.Where(l => l.CompanyId == companyID && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<AddressChange>, List<AddressChangeEntity>>(addressChanges);
            }

        }

        /// <summary>
        /// Get Vendor Changes
        /// </summary>
        /// <param name="companyID">Ax Company ID</param>
        /// <param name="batchSize">No of records to return</param>
        /// <returns></returns>
        public List<VendorChangeEntity> GetVendorChanges(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<VendorChange> vendorChanges = context.VendorChanges.Where(l => l.CompanyId == companyID && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<VendorChange>, List<VendorChangeEntity>>(vendorChanges);
            }

        }

        /// <summary>
        /// Get the Purchase Invoice Headers for company
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public List<PurchaseInvoiceHeaderEntity> GetPurchaseInvoices(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<PurchaseInvoiceHeader> purchaseInvoices = context.PurchaseInvoiceHeaders.Where(l => l.CompanyID == companyID && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<PurchaseInvoiceHeader>, List<PurchaseInvoiceHeaderEntity>>(purchaseInvoices);
            }
        }

        /// <summary>
        /// Get the purchase invoice lines from the header
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="externalInvID"></param>
        /// <param name="vendAccount"></param>
        /// <returns></returns>
        public List<PurchaseInvoiceLineEntity> GetPurchaseInvoiceLines(string companyID, DateTime invoiceDate, string externalInvID, string vendAccount)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<PurchaseInvoiceLine> purchaseInvoiceLines = context.PurchaseInvoiceLines.Where(i => i.EXTERNAL_INV_ID == externalInvID && i.VendAccount == vendAccount && i.InvoiceDate == invoiceDate).ToList();
                return Mapper.Map<List<PurchaseInvoiceLine>, List<PurchaseInvoiceLineEntity>>(purchaseInvoiceLines);
            }
        }


        
        public List<SalesInvoiceJournalEntity> GetSalesInvoiceJournal(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<SalesInvoiceJournal> salesInvoices = context.SalesInvoiceJournals.Where(l => l.CompanyID == companyID && l.Amount != 0 && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<SalesInvoiceJournal>, List<SalesInvoiceJournalEntity>>(salesInvoices);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public List<AOMPurchaseDeliveryHeadersEntity> GetPurchaseDeliveryHeaders(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                //ToDo: Add in the company ID!! List<AOMPurchaseDeliveryHeadersEntity> deliveryHeaders = context.AOMPurchaseDeliveryHeaders.Where(l => l.CompanyId == companyID && (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null)).Take(batchSize).ToList();
                List<AOMPurchaseDeliveryHeader> deliveryHeaders = context.AOMPurchaseDeliveryHeaders.Where(l => (l.ExtractStatus == Constants.notExtracted || l.ExtractStatus == null) && l.HeaderProcessed == 1 && l.DATAAREAID == companyID).Take(batchSize).ToList();
                return Mapper.Map<List<AOMPurchaseDeliveryHeader>, List<AOMPurchaseDeliveryHeadersEntity>>(deliveryHeaders);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="orderNumber"></param>
        /// <param name="deliveryNumber"></param>
        /// <param name="aomPurchaseDeliveryHeaderID"></param>
        /// <returns></returns>
        public List<AOMPurchaseDeliveryLinesEntity> GetPurchaseDeliveryLines(string companyID, string orderNumber, int aomPurchaseDeliveryHeaderID)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<AOMPurchaseDeliveryLine> deliveryLines = context.AOMPurchaseDeliveryLines.Where(s => s.EXTERNAL_ORD_ID == orderNumber && s.AOMPurchaseDeliveryHeaderID == aomPurchaseDeliveryHeaderID).ToList();
                return Mapper.Map<List<AOMPurchaseDeliveryLine>, List<AOMPurchaseDeliveryLinesEntity>>(deliveryLines);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public List<AOMProductChangeEntity> GetAOMProductChanges(string companyID, int batchSize)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<AOMProductChange> productChanges = context.AOMProductChanges.Where(s => s.DATAAREAID == companyID && (s.ExtractStatus == Constants.notExtracted || s.ExtractStatus == null)).Take(batchSize).ToList();
                return Mapper.Map<List<AOMProductChange>, List<AOMProductChangeEntity>>(productChanges);
            }
        }


        public List<spExtractAxPaymentTerms_ResultEntity> GetAxPaymentTerms (string companyID)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                List<spExtractAxPaymentTerms_Result> paymentTerms = context.spExtractAxPaymentTerms(companyID).ToList();
                return Mapper.Map<List<spExtractAxPaymentTerms_Result>, List<spExtractAxPaymentTerms_ResultEntity>>(paymentTerms);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="custChangeID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateCustomerExtractStatus(List<int> custChangeID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in custChangeID)
                {
                    context.CustChanges.Find(id).ExtractStatus = extractStatus;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productChangeID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateAOMProductExtractStatus(List<int> productChangeID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in productChangeID)
                {
                    context.AOMProductChanges.Find(id).ExtractStatus = (byte)extractStatus;
                }
                context.SaveChanges();
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressChangeID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateAddressExtractStatus(List<int> addressChangeID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in addressChangeID)
                {
                    context.AddressChanges.Find(id).ExtractStatus = extractStatus;
                }
                context.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorChangeID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateVendorExtractStatus(List<int> vendorChangeID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in vendorChangeID)
                {
                    context.VendorChanges.Find(id).ExtractStatus = extractStatus;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update Sales Invoice Journal Lines with extract status
        /// </summary>
        /// <param name="custChangeID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateSalesInvoiceJournalExtractStatus(List<long> custChangeID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in custChangeID)
                {
                    context.SalesInvoiceJournals.Find(id).ExtractStatus = extractStatus;
                }
                context.SaveChanges();
            }
        }

        public void UpdateAOMPurchaseDeliveryHeaderExtractStatus(List<long> aomPurchaseDeliveryHeaderID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in aomPurchaseDeliveryHeaderID)
                {
                    context.AOMPurchaseDeliveryHeaders.Find(id).ExtractStatus = (byte)extractStatus;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update the status on the prurchase invoice ID's
        /// </summary>
        /// <param name="purchaseInvoiceHeaderID"></param>
        /// <param name="extractStatus"></param>
        public void UpdateAOMPurchaseInvoiceHeaderExtractStatus(List<long> purchaseInvoiceHeaderID, int extractStatus)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                foreach (int id in purchaseInvoiceHeaderID)
                {
                    context.PurchaseInvoiceHeaders.Find(id).ExtractStatus = (byte)extractStatus;
                }
                context.SaveChanges();
            }
        }

        public void UpdateBatchStatus(string batchJob, string companyID)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                context.spUpdateBatchStatus("", "");
            }

        }

        public void UpdateAccountsOnHoldJournal(string companyID)
        {
            using (Ax2012InterfaceEntities context = new Ax2012InterfaceEntities())
            {
                context.spUpdateAccountsOnHoldJournal(companyID);
            }
        }

    }
}
