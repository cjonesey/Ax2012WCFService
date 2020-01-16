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
    //This is the class that will update the vendor details    
    public class UploadVendor: IDisposable
    {
        public string eventMessage { get; set; }
        public event EventHandler Changed;

        private bool disposed = false;
        Ax2012InterfaceDAL dal;

        public UploadVendor()
        {
            dal = new Ax2012InterfaceDAL();
        }

        // Invoke the Changed event; called whenever list changes:
        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
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
                eventMessage = string.Format("Vendors - Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
                OnChanged(EventArgs.Empty);

            } while (loop == true);

            //if (successCount > 0)
                //dynamicsAxDAL.UpdateBatchJob("Customer Batch Processing (13)");

            lastErrorMessage = errorMessage;
            return returnValue;
        }

        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsUploaded, out int recordCount)
        {
            DateTime extractedDate = System.DateTime.Now;
            Guid extractBatchID = System.Guid.NewGuid();
            List<int> vendorChangeID;

            lastErrorMessage = "";
            recordCount = 0;
            recordsUploaded = false;


            //Get the customer changes
            var vendorList = dal.GetVendorChanges(companyID, Constants.vendorBatchSize);

            //Create a reference to the address server.
            UploadVendorService.QTQ_STG_VendorServiceClient axUpdateVendorClient = new UploadVendorService.QTQ_STG_VendorServiceClient();

            //inject my endpoint behavior
            axUpdateVendorClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //Set the call context - this contains the company details:
            UploadVendorService.CallContext axContext = new UploadVendorService.CallContext();

            //Container for all addresses
            UploadVendorService.AxdQTQ_STG_Vendor axVendors = new UploadVendorService.AxdQTQ_STG_Vendor();

            try
            {
                ArrayList axVendorCollection = new ArrayList();
                vendorChangeID = new List<int>();
                foreach (VendorChangeEntity vendor in vendorList)
                {
                    Debug.WriteLine(string.Format("Record:{0}",vendor.AccountNum));
                    recordCount++;
                    vendorChangeID.Add(vendor.VendorChangeId);
                    //Set the reference to the new address
                    UploadVendorService.AxdEntity_STG_VendTable axVendor = new UploadVendorService.AxdEntity_STG_VendTable();
                    axContext.Company = vendor.CompanyId;

                    axVendor.ActionType = UploadVendorService.AxdEnum_QTQ_STG_ActionType.Create;
                    axVendor.ActionTypeSpecified = true;

                    axVendor.AccountNum = vendor.AccountNum;
                    
                    if (vendor.AllowSplitPack != null && vendor.AllowSplitPack == 0)
                    {
                        axVendor.AllowSplitPack = UploadVendorService.AxdEnum_TEC_NoYes.No;
                    }
                    else
                    {
                        axVendor.AllowSplitPack = UploadVendorService.AxdEnum_TEC_NoYes.Yes;
                    }
                    axVendor.AllowSplitPackSpecified = true;                        

                    axVendor.BankAccount = vendor.BankAccount;

                    if (vendor.Blocked != null && vendor.Blocked == 0)
                    {
                        axVendor.Blocked = UploadVendorService.AxdExtType_VendBlocked.No;
                    }
                    else
                    {
                        axVendor.Blocked = UploadVendorService.AxdExtType_VendBlocked.All;
                    }
                    axVendor.BlockedSpecified = true;

                    axVendor.CompanyChainId = vendor.CompanyChainId;
                    axVendor.CreditMax = vendor.CreditMax;
                    axVendor.CreditRating = vendor.CreditRating;
                    axVendor.Currency = vendor.Currency;
                    axVendor.DestinationDataAreaId = vendor.CompanyId;
                    if (vendor.Internal != null && vendor.Internal == 1)
                    {
                        axVendor.Internal = UploadVendorService.AxdEnum_TEC_NoYes.Yes;
                    }
                    else
                    {
                        axVendor.Internal = UploadVendorService.AxdEnum_TEC_NoYes.No;
                    }
                    axVendor.InternalSpecified = true;

                    axVendor.InventSiteId = vendor.InventSiteId;

                    axVendor.InvoiceAccount = vendor.InvoiceAccount;
                    axVendor.LineOfBusinessId = vendor.LineOfBusinessId;
                    axVendor.Memo = vendor.Memo;
                    axVendor.Message01 = vendor.Message01;
                    axVendor.Message02 = vendor.Message02;
                    axVendor.Message03 = vendor.Message03;
                    axVendor.Message04 = vendor.Message04;
                    axVendor.Message05 = vendor.Message05;
                    axVendor.MinOrderValue = vendor.MinorderValue;
                    axVendor.Name = vendor.Name;
                    axVendor.NameAlias = vendor.NameAlias;
                    axVendor.PaymDayId = vendor.PaymdayId;
                    axVendor.PaymMode = vendor.PaymMode;
                    axVendor.PaymModeInbound = vendor.PaymModeInbound;
                    axVendor.PaymSched = vendor.PaymSched;
                    axVendor.PaymSchedInbound = vendor.PaymSchedInbound;
                    axVendor.PaymSpec = vendor.PaymSpec;
                    axVendor.PaymSpecInbound = vendor.PaymSpecInbound;
                    if(vendor.PrintSupplier != null && vendor.PrintSupplier == 1)
                    {
                        axVendor.PrintSupplier = UploadVendorService.AxdEnum_TEC_NoYes.Yes;
                    }
                    else
                    {
                        axVendor.PrintSupplier = UploadVendorService.AxdEnum_TEC_NoYes.No;
                    }

                    axVendor.SegmentId = vendor.SegmentId;
                    axVendor.StagingID = vendor.StagingId.ToString();
                    axVendor.SubSegmentId = vendor.SubsegmentId;
                    axVendor.SupplierLeadTime = vendor.SupplierLeadTime;
                    axVendor.TaxGroup = vendor.TaxGroup;
                    axVendor.TaxGroupInbound = vendor.TaxGroup;
                    axVendor.TheirCode = vendor.TheirCode;
                    axVendor.VATNum = vendor.VatNum;
                    axVendor.VendGroup = vendor.VendGroup;
                    axVendor.YourAccountNum = vendor.YourAccountNum;
                    axVendorCollection.Add(axVendor);

                }
                dal.UpdateVendorExtractStatus(vendorChangeID, Constants.beingExtracted);
                if (recordCount != 0)
                {
                    axVendors.STG_VendTable = (UploadVendorService.AxdEntity_STG_VendTable[])axVendorCollection.ToArray(typeof(UploadVendorService.AxdEntity_STG_VendTable));
                    //Now process the customer
                    var response = axUpdateVendorClient.create(axContext, axVendors);
                    //Save the records - this updates the status to 4
                    dal.UpdateVendorExtractStatus(vendorChangeID, Constants.extracted);
                    recordsUploaded = true;
                }
            }

            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling QTQVendorService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling QTQVendorService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling QTQVendorService" + ex.ToString());
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
