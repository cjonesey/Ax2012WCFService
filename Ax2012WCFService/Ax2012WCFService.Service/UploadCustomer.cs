using Ax2012WCFService.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ax2012WCFService.DAL;
using Ax2012WCFService.Common;
using Ax2012WCFService.Entities;
using System.Collections;
using System.Diagnostics;

using System.ServiceModel;

namespace Ax2012WCFService.Service
{
    //This is the class that will updat the customer details
    public delegate void UpdateEventHandler(object sender, EventArgs e);

    public class UploadCustomer : IDisposable
    {
        public string eventMessage { get; set; }
        public event EventHandler Changed;

        private bool disposed = false;
        Ax2012InterfaceDAL dal;

        // Invoke the Changed event; called whenever list changes:
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        public UploadCustomer()
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
            do
            {
                returnValue = Upload(companyID, out errorMessage, out recordsUploaded, out recordCount);
                if (returnValue == true)
                {
                    successCount += recordCount;
                }
                else
                {
                    errorCount += recordCount;
                }
                if (recordsUploaded == false)
                    loop = false;
                //Create an event
                eventMessage = string.Format("Customers - Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
                OnChanged(EventArgs.Empty);

            } while (loop == true);

            //if (successCount > 0)
                //dynamicsAxDAL.UpdateBatchJob("Customer Batch Processing (13)");

            lastErrorMessage = errorMessage;
            return returnValue;
        }

        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsReturned, out int recordCount)
        {
            DateTime extractedDate = System.DateTime.Now;
            Guid extractBatchID = System.Guid.NewGuid();
            List<int> custChangeID;

            lastErrorMessage = "";
            recordCount = 0;

            int ledger = 0;
            string orderNumber = "";
            string deliveryNumber = "";
            Guid deliveryGUID;
            
            recordsReturned = false;


            //Get the customer changes
            var custList = dal.GetCustomerChanges(companyID, Constants.customerBatchSize);

            //Create a reference to the customer server.
            Ax2012WCFService.Service.UploadCustomerService.QTQ_STG_CustomerServiceClient axUpdateCustomerClient = new Service.UploadCustomerService.QTQ_STG_CustomerServiceClient();

            //inject my endpoint behavior
            axUpdateCustomerClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //This is the completed customer message
            //Ax2012WCFService.Service.UpdateCustomerService.AxdQTQ_STG_Customer axCustomer = new Service.UpdateCustomerService.AxdQTQ_STG_Customer();

            //Set the call context - this contains the company details:
            Ax2012WCFService.Service.UploadCustomerService.CallContext axContext = new Service.UploadCustomerService.CallContext();

            Ax2012WCFService.Service.UploadCustomerService.AxdQTQ_STG_Customer axCustomers = new Service.UploadCustomerService.AxdQTQ_STG_Customer();
            try
            {
                ArrayList axCustomerCollection = new ArrayList();
                custChangeID = new List<int>();                    
                foreach (CustChangeEntity cust in custList)
                {
                    Debug.WriteLine(cust.AccountNum);
                    recordCount++;
                    axContext.Company = cust.CompanyID;
                    //Set the reference to the new customer
                    Ax2012WCFService.Service.UploadCustomerService.AxdEntity_STG_CustTable axCustomer = new Ax2012WCFService.Service.UploadCustomerService.AxdEntity_STG_CustTable();
                    axCustomer.ActionType = UploadCustomerService.AxdEnum_QTQ_STG_ActionType.Update;
                    axCustomer.ActionTypeSpecified = true;
                    axCustomer.AccountNum = cust.AccountNum;
                    axCustomer.CreditMax = cust.CreditMax;
                    axCustomer.CreditMaxSpecified = true;
                    axCustomer.Currency = cust.Currency;
                    axCustomer.CustClassificationId = cust.CustClassificationId;
                    axCustomer.CustGroup = cust.CustGroup;
                    //axCustomer.DealerGroupOrIndependent = cust.DealerGroupOrIndependent;
                    axCustomer.DestinationDataAreaId = cust.CompanyID;
                    axCustomer.InventLocation = cust.InventLocation;
                    axCustomer.InventLocationInbound = cust.InventLocation;
                    axCustomer.InventSiteId = cust.INVENTSITEID;
                    
                    //Only set the invoice account if is not the same as the main account number
                    if (!string.IsNullOrEmpty(cust.InvoiceAccount) && cust.InvoiceAccount != cust.AccountNum && cust.InvoiceAccount != "0")
                        axCustomer.InvoiceAccount = cust.InvoiceAccount;
                    
                    //axCustomer.MainContactWorker = "5637278167";
                    axCustomer.MainContactWorkerInbound = cust.MainContactWorker;
                    //axCustomer.MainContactWorker = cust.MainContactWorker;


                    axCustomer.MandatoryCreditLimit = Ax2012WCFService.Service.UploadCustomerService.AxdExtType_MandatoryCreditLimit.Yes;

                    axCustomer.Msg1 = cust.Msg1;
                    axCustomer.Msg2 = cust.Msg2;
                    axCustomer.Msg3 = cust.Msg3;
                    axCustomer.Msg4 = cust.Msg4;
                    axCustomer.Msg5 = cust.Msg5;

                    axCustomer.Name = cust.Name;
                    axCustomer.NameAlias = cust.NameAlias;

                    axCustomer.PaymMode = cust.PaymMode;
                    axCustomer.PaymModeInbound = cust.PaymMode;
                    
                    axCustomer.PaymTermId = cust.PaymTermId;
                    axCustomer.PaymTermIdInbound = cust.PaymTermId;
                    
                    axCustomer.SalesDistrictId = cust.SalesDistrictId;
                    axCustomer.StagingID = cust.StagingId.ToString();

                    axCustomer.TaxGroup = "T"; //cust.TaxGroup;
                    axCustomer.TaxGroupInbound = "T"; //cust.TaxGroup;

                    axCustomer.TEC_BillingOptions = cust.Tec_BillingOptions;
                    axCustomer.TEC_ChargeCode = cust.Tec_ChargeCode;
                    axCustomer.TEC_InvoiceGroup = cust.Tec_InvoiceGroup;
                    axCustomer.TEC_MinOrderBreak = cust.Tec_MinOrderBreak.ToString(); //Default as there is no data
                    axCustomer.TEC_PriceBand = cust.Tec_PriceBand.ToString(); //Default as there is no data
                    axCustomer.TEC_WebSite = cust.Tec_WebSite.ToString();

                    axCustomer.VATNum = cust.VATNum;

                    axCustomerCollection.Add(axCustomer);
                    custChangeID.Add(cust.CustChangeID);
                }
                dal.UpdateCustomerExtractStatus(custChangeID, Constants.beingExtracted);
                if (recordCount != 0)
                {
                    axCustomers.STG_CustTable = (Ax2012WCFService.Service.UploadCustomerService.AxdEntity_STG_CustTable[])axCustomerCollection.ToArray(typeof(Ax2012WCFService.Service.UploadCustomerService.AxdEntity_STG_CustTable));
                    //Now process the customer
                    var response = axUpdateCustomerClient.create(axContext, axCustomers);
                    //Save the records - this updates the status to 4
                    dal.UpdateCustomerExtractStatus(custChangeID, Constants.extracted);
                    recordsReturned = true;
                }
            }

            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling QTQCustomerService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling QTQCustomerService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling QTQCustomerService" + ex.ToString());
                lastErrorMessage = ex.ToString();
                return false;
            }
            lastErrorMessage = "OK";
            return true;

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
