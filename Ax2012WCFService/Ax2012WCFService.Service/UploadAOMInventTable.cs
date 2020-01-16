using Ax2012WCFService.Common;
using Ax2012WCFService.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Service
{
    public class UploadAOMInventTable: IDisposable
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

        public UploadAOMInventTable()
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
                eventMessage = string.Format("Products - Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
                OnChanged(EventArgs.Empty);

            } while (loop == true);

            //if (successCount > 0)
                //dynamicsAxDAL.UpdateBatchJob("Customer Batch Processing (13)");

            lastErrorMessage = errorMessage;
            return returnValue;
        }

        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsReturned, out int recordCount)
        {
            List<int> productChangeIDs;
            int productChangeID;
            recordsReturned = false;
            recordCount = 0;
            lastErrorMessage = "";


            //Create a reference to the invent Service
            UploadInventService.TEC_STG_InventServiceClient axInventServiceClient = new UploadInventService.TEC_STG_InventServiceClient();

            //inject my endpoint behavior
            axInventServiceClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //This is the completed invent message (1 or more items)
            UploadInventService.AxdTEC_STG_Invent axInventItems = new UploadInventService.AxdTEC_STG_Invent();

            //This is the blank call context - this is optional, however the place holder must be in the file
            UploadInventService.CallContext callContext = new UploadInventService.CallContext();
            
            //Get the product changes
            var aomProductChanges =  dal.GetAOMProductChanges(companyID, Constants.inventBatchSize);

            callContext.Company = companyID;
            try
            {
                productChangeIDs = new List<int>();
                //Array to hold the products
                ArrayList axProductChanges = new ArrayList();
                foreach (var aomProductChange in aomProductChanges)
                {
                    recordCount++;
                    Debug.WriteLine(aomProductChange.ITEMID);

                    //This is the item that we will create (Individual)
                    UploadInventService.AxdEntity_TEC_STG_InventTable axInventItem = new UploadInventService.AxdEntity_TEC_STG_InventTable();

                    productChangeID = aomProductChange.ProductChangeID;

                    axInventItem.ItemId = aomProductChange.ITEMID;

                    //We are not currently interfacing BOSS Groups - and there is no facility to pass the group - you have to do a lookup
                    //axInventItem.Bossf_Group.GroupID = sqlProductChange.Bossf_Group;
                    //axInventItem.Bossf_Range.GroupID = sqlProductChange.Bossf_Range;

                    axInventItem.DeliveryTime = aomProductChange.DELIVERYTIME;
                    axInventItem.Depth = aomProductChange.DEPTH;
                    axInventItem.Description_2 = aomProductChange.DESCRIPTION_2;
                    axInventItem.Description_3 = aomProductChange.DESCRIPTION_3;
                    axInventItem.Description_4 = aomProductChange.DESCRIPTION_4;
                    axInventItem.Description_5 = aomProductChange.DESCRIPTION_5;
                    axInventItem.Height = aomProductChange.HEIGHT;
                    axInventItem.HeightSpecified = true;
                    axInventItem.Intracode = aomProductChange.INTRACODE;
                    if (aomProductChange.ITEMGROUP.Length < 5 )
                    {
                        axInventItem.ItemGroup = "04160";
                    }
                    else
                    {
                        axInventItem.ItemGroup = aomProductChange.ITEMGROUP;
                    }
                    axInventItem.Manufacturer = aomProductChange.MANUFACTURER;
                    axInventItem.Manufacturer_Prodcode = aomProductChange.MANUFACTURER_PRODCODE;
                    axInventItem.Name = aomProductChange.NAME;
                    axInventItem.NetWeight = aomProductChange.NETWEIGHT;
                    axInventItem.OrigCountryId = aomProductChange.ORIGCOUNTRYID;
                    //axInventItem.PaidStock = sqlProductChange.PaidStock != null && sqlProductChange.PaidStock != true ?  tecInventService.AxdExtType_TEC_PAIDSTOCK.No : tecInventService.AxdExtType_TEC_PAIDSTOCK.Yes;
                    axInventItem.Primaryvendid = aomProductChange.PRIMARYVENDID;
                    axInventItem.Product_Owner = aomProductChange.PRODUCT_OWNER;
                    //We are not currently interfacing the product range & Group - and there is no facility to pass the group - you have to do a lookup

                    axInventItem.Sell_Price = aomProductChange.SELL_PRICE;
                    axInventItem.Sell_PriceSpecified = true;

                    axInventItem.Sell_PriceUnit = aomProductChange.SELL_PRICEUNIT;
                    axInventItem.Sell_PriceUnitSpecified = true;
                    axInventItem.SHELFLIFE = aomProductChange.SHELFLIFE;
                    //axInventItem.Special = sqlProductChange.Special != null && sqlProductChange.Special != true ? tecInventService.AxdExtType_TEC_SPECIAL.No : tecInventService.AxdExtType_TEC_SPECIAL.Yes;
                    
                    axInventItem.Standard_Cost = aomProductChange.STANDARD_COST;
                    axInventItem.Standard_CostSpecified = true;

                    axInventItem.StatusBlocked = aomProductChange.STATUSBLOCKED != null && aomProductChange.STATUSBLOCKED == "Y" ? "Y" : "N";

                    axInventItem.Stock_Owner = aomProductChange.STOCK_OWNER;
                    axInventItem.Type = aomProductChange.TYPE != null && aomProductChange.TYPE != 0 ? UploadInventService.AxdExtType_TEC_Type.Update : UploadInventService.AxdExtType_TEC_Type.Update;
                    axInventItem.TypeSpecified = true;

                    axInventItem.UNSPSC = aomProductChange.UNSPSC;

                    axInventItem.Vat_Group = aomProductChange.VAT_GROUP;

                    axInventItem.Width = aomProductChange.WIDTH;
                    axInventItem.WidthSpecified = true;
                    //Add the order to the order headers array
                    //Array to hold the products
                    axProductChanges.Add(axInventItem);

                    productChangeIDs.Add(productChangeID);
                }
                if (recordCount != 0)
                {
                    dal.UpdateAOMProductExtractStatus(productChangeIDs, Constants.beingExtracted);
                    axInventItems.TEC_STG_InventTable = (UploadInventService.AxdEntity_TEC_STG_InventTable[])axProductChanges.ToArray(typeof(UploadInventService.AxdEntity_TEC_STG_InventTable));
                    //Now process the order
                    var response = axInventServiceClient.create(callContext, axInventItems);
                    dal.UpdateAOMProductExtractStatus(productChangeIDs, Constants.extracted);
                    recordsReturned = true;
                }
                else
                {
                    recordsReturned = false;
                }
            }

            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling TecInventService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling TecInventService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling TecInventService" + ex.ToString());
                lastErrorMessage = ex.ToString();
                return false;
            }
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
