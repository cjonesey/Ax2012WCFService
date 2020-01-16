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
    public class UploadAddress: IDisposable
    {
        public delegate void UpdateEventHandler(object sender, EventArgs e);

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
        
        public UploadAddress()
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
                eventMessage = string.Format("Addresses- Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
                OnChanged(EventArgs.Empty);

            } while (loop == true);
            
            //if (successCount > 0)
            //    dynamicsAxDAL.UpdateBatchJob("Address Batch Processing (13)");

            lastErrorMessage = errorMessage;
            return returnValue;
        }
        
        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsReturned, out int recordCount)
        {
            DateTime extractedDate = System.DateTime.Now;
            Guid extractBatchID = System.Guid.NewGuid();
            List<int> addressChangeID;

            lastErrorMessage = "";
            recordCount = 0;
            recordsReturned = false;

            Guid deliveryGUID;

            //Get the customer changes
            var addressList = dal.GetAddressChanges(companyID, Constants.addressBatchSize);

            //Create a reference to the address server.
            UploadAddressService.QTQ_STG_AddressServiceClient axUpdateAddressClient = new UploadAddressService.QTQ_STG_AddressServiceClient();

            //inject my endpoint behavior
            axUpdateAddressClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //Set the call context - this contains the company details:
            UploadAddressService.CallContext axContext = new UploadAddressService.CallContext();

            //Container for all addresses
            UploadAddressService.AxdQTQ_STG_Address axAddresses = new UploadAddressService.AxdQTQ_STG_Address();

            try
            {
                ArrayList axAddressCollection = new ArrayList();
                addressChangeID = new List<int>();
                foreach (AddressChangeEntity address in addressList)
                {
                    Debug.WriteLine(string.Format("Record:{0}:{1}",address.AccountNum,address.AddressRef));
                    addressChangeID.Add(address.AddressChangeId);
                    recordCount++;
                    axContext.Company = address.CompanyId;
                    //Set the reference to the new address
                    UploadAddressService.AxdEntity_STG_AddrTable axAddress = new UploadAddressService.AxdEntity_STG_AddrTable();
                    axAddress.ActionType = UploadAddressService.AxdEnum_QTQ_STG_ActionType.Create;
                    
                    axAddress.StagingID = address.StagingId.ToString();
                    axAddress.AccountNum = address.AccountNum;
                    //axAddress.Status = address.Status;
                    axAddress.DestinationDataAreaId = address.CompanyId;
                    axAddress.AddressRef = address.AddressRef;
                    axAddress.Description = address.Description;
                    axAddress.LocationRole = address.LocationRole;
                    axAddress.BuildingCompliment = address.BuildingCompliment;
                    axAddress.City = address.City;
                    axAddress.County = address.County;
                    axAddress.Street = address.Street;
                    axAddress.State = address.State;
                    axAddress.ZipCode = address.ZipCode;
                    axAddress.CountryRegionId = address.CountryRegionId;
                    axAddress.Comment1 = address.Comment1;
                    axAddress.Comment2 = address.Comment2;
                    axAddress.DxReference = address.DxReference;
                    axAddress.InvoiceAddressRef = address.InvoiceAddressRef;
                    axAddress.InventLocation = address.InventLocation;
                    axAddress.InventSite = address.InventSite;
                    axAddress.InventLocationInbound = address.InventLocation;

                    //axAddress.ServiceType = address.ServiceType;

                    axAddress.TheirCode = address.TheirCode;
                    axAddress.Rep1 = address.Rep1;
                    axAddress.TaxGroup = address.TaxGroup;

                    if (address.AccountType == 2)
                    {
                        axAddress.AccountType = UploadAddressService.AxdEnum_QTQ_STG_AccountType.Vendor;
                    }
                    else
                    {
                        axAddress.AccountType = UploadAddressService.AxdEnum_QTQ_STG_AccountType.Customer;
                    }
                    axAddress.AccountTypeSpecified = true;

                    axAddress.TaxGroupInbound = address.TaxGroup;
                    axAddress.ServiceLevel = address.ServiceLevel;
                    axAddress.VanRoute = address.VanRoute;
                    axAddress.DistrictName = address.DistrictName;
                    axAddressCollection.Add(axAddress);
                }
                dal.UpdateAddressExtractStatus(addressChangeID, Constants.beingExtracted);
                if (recordCount != 0)
                {
                    axAddresses.STG_AddrTable = (UploadAddressService.AxdEntity_STG_AddrTable[])axAddressCollection.ToArray(typeof(UploadAddressService.AxdEntity_STG_AddrTable));                    
                    //Now process the customer
                    var response = axUpdateAddressClient.create(axContext, axAddresses);
                    //Save the records - this updates the status to 4
                    dal.UpdateAddressExtractStatus(addressChangeID, Constants.extracted);
                    recordsReturned = true;
                }
            }

            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling QTQAddressService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling QTQAddressService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling QTQAddressService" + ex.ToString());
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
