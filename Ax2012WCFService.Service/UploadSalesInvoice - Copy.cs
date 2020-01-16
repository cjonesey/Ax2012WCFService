using Ax2012WCFService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ServiceModel;
using Ax2012WCFService.Common;
using System.Collections;
using Ax2012WCFService.Entities;

namespace Ax2012WCFService.Service
{
    public class UploadSalesInvoice: IDisposable
    {
        public string eventMessage { get; set; }
        public event EventHandler Changed;

        private bool disposed = false;
        Ax2012InterfaceDAL dal;
        List<spExtractAxPaymentTerms_ResultEntity> paymentTerms = new List<spExtractAxPaymentTerms_ResultEntity>();

        // Invoke the Changed event; called whenever list changes:
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
        public UploadSalesInvoice()
        {
            dal = new Ax2012InterfaceDAL();
        }

        public bool Upload(string companyID, out string lastErrorMessage)
        {
            bool loop = true;
            bool returnValue = false;
            bool recordsUploaded = false;
            int recordCount = 0;
            int successCount = 0;
            int errorCount = 0;

            string errorMessage = "";
            paymentTerms = dal.GetAxPaymentTerms(companyID);
            //Require payment terms before starting            
            if (paymentTerms.Count() > 0)
            {
                do
                {
                    returnValue = Upload(companyID, out errorMessage, out recordsUploaded, out recordCount);
                    if (recordsUploaded == false)
                        loop = false;
                    if (returnValue == true)
                    {
                        successCount += recordCount;
                        errorMessage = "";
                    }
                    else
                    {
                        errorCount += recordCount;
                    }
                    //Create an event
                    eventMessage = string.Format("Sales Journals - Success:{1} - Error:{2} {3}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString(), errorMessage);
                    OnChanged(EventArgs.Empty);

                } while (loop == true);
            }
            lastErrorMessage = errorMessage;
            return returnValue;
        }

        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsReturned, out int recordCount)
        {
            DateTime extractedDate = System.DateTime.Now;
            Guid extractBatchID = System.Guid.NewGuid();
            List<long> salesInvoiceJournalID;
            recordsReturned = false;

            lastErrorMessage = "";
            recordCount = 0;

            Guid deliveryGUID;

            //Get the customer changes
            var salesInvoiceList = dal.GetSalesInvoiceJournal(companyID, Constants.salesInvoiceBatchSize);

            //Create a reference to the sales invoice service
            UploadSalesInvoiceService.QTQ_STG_SalesInvoiceServiceClient axSalesInvoiceClient = new UploadSalesInvoiceService.QTQ_STG_SalesInvoiceServiceClient();

            //inject my endpoint behavior
            axSalesInvoiceClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //Set the call context - this contains the company details:
            UploadSalesInvoiceService.CallContext axContext = new UploadSalesInvoiceService.CallContext();

            //Container for all of the invoice lines
            UploadSalesInvoiceService.AxdQTQ_STG_SalesInvoice axSalesInvoices = new UploadSalesInvoiceService.AxdQTQ_STG_SalesInvoice();

            try
            {
                ArrayList axSalesInvoiceCollection = new ArrayList();
                salesInvoiceJournalID = new List<long>();
                foreach (SalesInvoiceJournalEntity invoiceLine in salesInvoiceList)
                {
                    Debug.WriteLine(string.Format("Record:{0}:{1}",invoiceLine.Account,invoiceLine.Invoice));
                    salesInvoiceJournalID.Add(invoiceLine.SalesInvoiceJournalID);
                    recordCount++;

                    //Set the reference to the new address
                    UploadSalesInvoiceService.AxdEntity_STG_SINVTable salesInvoice = new UploadSalesInvoiceService.AxdEntity_STG_SINVTable();
                    salesInvoice.DestinationDataAreaId = invoiceLine.CompanyID;
                    //TODO:Reset:Done
                    salesInvoice.Description = string.Format("Focus Invoices {0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);  //invoiceLine.Description; //Journal Description not line
                    //salesInvoice.Description = "Opening Balances";
                    
                    //invoiceLine.JournalType; -- Need to have a think about this one
                    salesInvoice.JournalType = UploadSalesInvoiceService.AxdEnum_LedgerJournalType.Daily; 
                    
                    
                    salesInvoice.JournalName = invoiceLine.JournalName;
                    

                    //invoiceLine.AccountType; -- Need to add in an action for customer/supplier/nominal etc
                    //Default the account type to Ledger
                    salesInvoice.AccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Ledger;
                    if (!String.IsNullOrEmpty(invoiceLine.AccountType))
                    {
                        switch (invoiceLine.AccountType.ToString().ToUpper())
                        {
                            case "CUSTOMER":
                                salesInvoice.AccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Cust;
                                break;
                            case "SUPPLIER":
                                salesInvoice.AccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Vend;
                                break;
                        }
                    }
                    salesInvoice.AccountTypeSpecified = true;

                    salesInvoice.OffsetAccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Ledger;
                    if (!String.IsNullOrEmpty(invoiceLine.OffsetAccountType))
                    {
                        switch (invoiceLine.OffsetAccountType.ToString().ToUpper())
                        {
                            case "CUSTOMER":
                                salesInvoice.OffsetAccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Cust;
                                break;
                            case "SUPPLIER":
                                salesInvoice.OffsetAccountType = UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Vend;
                                break;
                        }
                    }
                    //Set the account and Offset account codes
                    salesInvoice.OffsetAccountTypeSpecified = true;
                    if (!String.IsNullOrEmpty(invoiceLine.Account))
                    {
                        salesInvoice.Account = invoiceLine.Account;
                    }

                    if (!String.IsNullOrEmpty(invoiceLine.OffsetAccount))
                    {
                        salesInvoice.OffsetAccount = invoiceLine.OffsetAccount;
                    }


                    //If the account type is ledger set the cost centre on the Account
                    if (salesInvoice.AccountType == UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Ledger)
                    {
                        if (!String.IsNullOrEmpty(invoiceLine.CostCentre))
                            salesInvoice.CostCentre = invoiceLine.CostCentre;                           
                    }

                    //If the offset account type is ledger set the cost centre on the Account
                    if (salesInvoice.OffsetAccountType == UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Ledger)
                    {
                        if (!String.IsNullOrEmpty(invoiceLine.OffsetCostCentre) && !String.IsNullOrEmpty(invoiceLine.OffsetAccount))
                            salesInvoice.OffsetCostCentre = invoiceLine.OffsetCostCentre;                        
                    }

                    //if the account type is vendor or customer and the offset is ledger the put the cost centre into the offset and make the account cost centre blank
                    if (salesInvoice.AccountType == UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Cust || salesInvoice.AccountType == UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Vend 
                        && salesInvoice.OffsetAccountType == UploadSalesInvoiceService.AxdEnum_LedgerJournalACType.Ledger)
                    {
                        if (!String.IsNullOrEmpty(invoiceLine.CostCentre) && !String.IsNullOrEmpty(invoiceLine.OffsetAccount))
                            salesInvoice.OffsetCostCentre = invoiceLine.CostCentre;
                    }
                                                                          
                    salesInvoice.Txt = invoiceLine.Txt;
                    salesInvoice.Invoice = invoiceLine.Invoice;
                    
                    salesInvoice.DocumentDate = invoiceLine.DocumentDate;
                    salesInvoice.DocumentDateSpecified = true;

                    

                    salesInvoice.DocumentNum = invoiceLine.DocumentNum;

                    //TODO: Remove and put the due dates back in correctly!!:Done
                    DateTime dueDate = CalculateDueDate(invoiceLine.Account, invoiceLine.DocumentDate);
                    if (dueDate == new DateTime(1900, 1, 1))
                        dueDate = invoiceLine.DocumentDate.AddDays(30);
                    salesInvoice.Due = dueDate;

                    string paymMode = GetPaymentMode(invoiceLine.Account);
                    salesInvoice.PaymMode = paymMode;

                    //salesInvoice.Due = invoiceLine.Due;
                    //2015-11-05
                    //TODO: Remove and put the due dates back in correctly!!
                    salesInvoice.DueSpecified = true;

                    //invoiceLine.salesInvoice.Amount = invoiceLine.Amount + invoiceLine.VATAmount;
                    salesInvoice.Amount = invoiceLine.Amount;
                    salesInvoice.AmountSpecified = true;
                    
                    //TODO: Add the VAT back in!:Done
                    salesInvoice.CorrectedTaxAmount = invoiceLine.VATAmount;
                    salesInvoice.CorrectedTaxAmountSpecified = true;

                    salesInvoice.StagingID = invoiceLine.StagingId.ToString();                                       

                    salesInvoice.CurrencyCode = invoiceLine.CurrencyCode;
                    //salesInvoice.ExchRate = invoiceLine.ExchRate;
                    salesInvoice.ExchRate = 0;
                    salesInvoice.ExchRateSpecified = true;



                    salesInvoice.PaymReference = invoiceLine.PaymReference;
                    salesInvoice.PostingProfile = invoiceLine.PostingProfile;

                    //ToDo - Replace this crap with the correct VAT data
                    //TODO: Add the VAT back in!:Done
                    if (invoiceLine.VATAmount != 0)
                    {
                        salesInvoice.TaxGroup = "T";
                        salesInvoice.TaxGroupInbound = "T";
                        salesInvoice.TaxCode = "A";
                        salesInvoice.TaxCodeInbound = "A";
                    }

                    salesInvoice.VATNumJournal = invoiceLine.VatNumJournal;
                    
                    salesInvoice.TransDate = invoiceLine.TransDate;
                    salesInvoice.TransDateSpecified = true;

                    axSalesInvoiceCollection.Add(salesInvoice);
                }
                dal.UpdateSalesInvoiceJournalExtractStatus(salesInvoiceJournalID, Constants.beingExtracted);
                if (recordCount != 0)
                {
                    axSalesInvoices.STG_SINVTable = (UploadSalesInvoiceService.AxdEntity_STG_SINVTable[])axSalesInvoiceCollection.ToArray(typeof(UploadSalesInvoiceService.AxdEntity_STG_SINVTable));
                    //Now process the record
                    var response = axSalesInvoiceClient.create(axContext, axSalesInvoices);
                    //Save the records - and mark as complete
                    dal.UpdateSalesInvoiceJournalExtractStatus(salesInvoiceJournalID, Constants.extracted);
                    recordsReturned = true;
                }
            }

            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling QTQSalesInvoiceService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling QTQSalesInvoiceService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling QTQSalesInvoiceService" + ex.ToString());
                lastErrorMessage = ex.ToString();
                return false;
            }
            lastErrorMessage = "OK";
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docDate"></param>
        /// <returns></returns>
        private DateTime CalculateDueDate(string account, DateTime docDate)
        {

            DateTime dueDate = docDate;

            var accountTerms = paymentTerms.Where(o => o.AccountNum == account).FirstOrDefault();
            try
            {
                if (paymentTerms.Count() == 0)
                    throw new Exception("Payment Terms empty");

                if (accountTerms != null && string.IsNullOrEmpty(accountTerms.AccountNum) != true)
                {
                    if (accountTerms.PAYMMETHOD == 1)
                    {
                        dueDate = docDate.AddMonths((int)accountTerms.NUMOFMONTHS + 1);
                        dueDate = dueDate.AddDays(-dueDate.Day);
                        dueDate = dueDate.AddDays((int)accountTerms.NUMOFDAYS);
                    }
                    else
                    {
                        dueDate = docDate.AddDays((int)accountTerms.NUMOFDAYS);
                        dueDate = docDate.AddMonths((int)accountTerms.NUMOFMONTHS);
                    }
                }
            }
            catch(Exception ex)
            {
                dueDate = new DateTime(1900, 1, 1);
                Debug.WriteLine(ex.ToString());
            }
            return dueDate;

        }

        private string GetPaymentMode(string account)
        {
            string paymMode = "";
            var accountTerms = paymentTerms.Where(o => o.AccountNum == account).FirstOrDefault();
            try
            {
                if (paymentTerms.Count() == 0)
                    throw new Exception("Payment Terms empty");

                if (accountTerms != null && string.IsNullOrEmpty(accountTerms.AccountNum) != true)
                {
                    paymMode = accountTerms.PAYMMODE;
                }
            }
            catch (Exception ex)
            {
                paymMode = "";
                Debug.WriteLine(ex.ToString());
            }
            if (paymMode == "")
                paymMode = "REC_BACS";
            return paymMode;
        }

        #region Disposing
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dal = null;
                    // Dispose managed resources.
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }
            disposed = true;

            // If it is available, make the call to the
            // base class's Dispose(Boolean) method
            //base. Dispose(disposing);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }
        #endregion

    }
}
