using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ax2012WCFService.DAL;
using Ax2012WCFService.Common;
using Ax2012WCFService.Entities;
using System.Diagnostics;
using System.Collections;
using System.ServiceModel;

namespace Ax2012WCFService.Service
{
    public class UploadAOMPurchaseDeliveries : IDisposable
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

        public UploadAOMPurchaseDeliveries()
        {
            dal = new Ax2012InterfaceDAL();

        }

        public bool Upload(string companyID, out string lastErrorMessage, Boolean deliveryRecord)
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
                returnValue = Upload(companyID, out errorMessage, out recordsUploaded, out recordCount, deliveryRecord);
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
                eventMessage = string.Format("Purchase Deliveries - Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
                OnChanged(EventArgs.Empty);

            } while (loop == true);

            //if (successCount > 0)
                //dynamicsAxDAL.UpdateBatchJob("Customer Batch Processing (13)");

            lastErrorMessage = errorMessage;
            return returnValue;
        }

        public bool Upload(string companyID, out string lastErrorMessage, out bool recordsReturned, out int recordCount, Boolean deliveryRecord)
        {
            DateTime extractedDate = System.DateTime.Now;
            Guid extractBatchID = System.Guid.NewGuid();
            List<long> aomPurchaseDeliveryIDs;

            lastErrorMessage = "";
            recordCount = 0;

            int ledger = 0;
            string orderNumber = "";
            string deliveryNumber = "";
            int aomPurchaseDeliveryHeaderID;

            recordsReturned = false;

            //Get the customer changes
            var aomPurchaseDeliveryList = dal.GetPurchaseDeliveryHeaders(companyID, Constants.customerBatchSize);


            //Create a reference to the invent Service
            UploadPurchOrderService.TEC_STG_PurchOrderServiceClient axPurchaseOrderClient = new UploadPurchOrderService.TEC_STG_PurchOrderServiceClient();

            //inject my endpoint behavior
            axPurchaseOrderClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //This is the completed invent message (1 or more items)
            UploadPurchOrderService.AxdTEC_STG_PurchOrder axPurchaseDeliveries = new UploadPurchOrderService.AxdTEC_STG_PurchOrder();

            //This is the blank call context - this is optional, however the place holder must be in the file
            UploadPurchOrderService.CallContext callContext = new UploadPurchOrderService.CallContext();

            //Set the company ID for teh call context
            callContext.Company = companyID;

            try
            {
                ArrayList axPurchaseDeliveryCollection = new ArrayList();
                aomPurchaseDeliveryIDs = new List<long>();
                foreach (AOMPurchaseDeliveryHeadersEntity sqlPurchaseDeliveryHeader in aomPurchaseDeliveryList)
                {
                    orderNumber = sqlPurchaseDeliveryHeader.EXTERNAL_ORD_ID.Trim();
                    aomPurchaseDeliveryHeaderID = sqlPurchaseDeliveryHeader.AOMPurchaseDeliveryHeaderID;
                    
                    deliveryNumber = string.Format("{0}-{1}",sqlPurchaseDeliveryHeader.EXTERNAL_DEL_ID.Trim(), sqlPurchaseDeliveryHeader.HeaderSequence.ToString());

                    UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable axPurchaseDeliveryHeader = new UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable();
                    
                    Debug.WriteLine(orderNumber);
                    recordCount++;

                    axPurchaseDeliveryHeader.OrderUpdateType = UploadPurchOrderService.AxdEnum_TEC_OrderUpdateType.Delivery;

                    axPurchaseDeliveryHeader.OrderUpdateTypeSpecified = true;
                    axPurchaseDeliveryHeader.EXTERNAL_ORD_ID = sqlPurchaseDeliveryHeader.EXTERNAL_ORD_ID;

                    //Add in the cost centre to the header - at the momenteverything is 0999
                    ArrayList axOrderHeaderDimensions = new ArrayList();
                    UploadPurchOrderService.AxdType_DimensionAttributeValue dimensionHeader = new UploadPurchOrderService.AxdType_DimensionAttributeValue();
                    dimensionHeader.Name = "CostCenter";
                    dimensionHeader.Value = "0999"; //ToDo: Add in the correct value
                    axOrderHeaderDimensions.Add(dimensionHeader);
                    //Now add the array to a dimension set
                    UploadPurchOrderService.AxdType_DimensionAttributeValueSet dimensionHeaderSet = new UploadPurchOrderService.AxdType_DimensionAttributeValueSet();
                    dimensionHeaderSet.Values = (UploadPurchOrderService.AxdType_DimensionAttributeValue[])axOrderHeaderDimensions.ToArray(typeof(UploadPurchOrderService.AxdType_DimensionAttributeValue));
                    //Finally add the dimension set the Order
                    axPurchaseDeliveryHeader.DefaultDimension = dimensionHeaderSet;

                    //axPurchaseOrderHeader.Add_Reference = sqlPurchaseOrderHeader.Inv_Add_Reference;
                    axPurchaseDeliveryHeader.Add_City = sqlPurchaseDeliveryHeader.ADD_CITY;
                    axPurchaseDeliveryHeader.Add_Country = sqlPurchaseDeliveryHeader.ADD_COUNTRY;
                    axPurchaseDeliveryHeader.Add_Line_1 = sqlPurchaseDeliveryHeader.ADD_LINE_1;
                    axPurchaseDeliveryHeader.Add_Line_2 = sqlPurchaseDeliveryHeader.ADD_LINE_2 ;
                    axPurchaseDeliveryHeader.Add_Territory = sqlPurchaseDeliveryHeader.ADD_TERRITORY;
                    axPurchaseDeliveryHeader.Add_Zipcode = sqlPurchaseDeliveryHeader.ADD_ZIPCODE;

                    //Site numbers
                    //string inventLocationID = sqlPurchaseOrderHeader.InventSiteId.ToString();
                    axPurchaseDeliveryHeader.InventLocationId = sqlPurchaseDeliveryHeader.INVENTLOCATIONID;
                    //CJ - 2016-09-01 - This is now populated in the database
                    //axPurchaseDeliveryHeader.InventSiteId = Constants.defaultSpicersSiteId;
                    axPurchaseDeliveryHeader.InventSiteId = sqlPurchaseDeliveryHeader.INVENTSITEID;

                    //axPurchaseOrderHeader.Invoice_Name = sqlPurchaseOrderHeader.Invoice_Name;
                    axPurchaseDeliveryHeader.OperatorCode = sqlPurchaseDeliveryHeader.OPERATORCODE;
                    axPurchaseDeliveryHeader.DlvCity = sqlPurchaseDeliveryHeader.DLVCITY;
                    //axPurchaseOrderHeader.Shp_Add_Reference = sqlPurchaseDeliveryHeader.Shp_Add_Reference;
                    axPurchaseDeliveryHeader.DlvCountry = sqlPurchaseDeliveryHeader.DLVCOUNTRY;
                    axPurchaseDeliveryHeader.DlvName = sqlPurchaseDeliveryHeader.DLVNAME;
                    axPurchaseDeliveryHeader.DlvAdd_Line_1 = sqlPurchaseDeliveryHeader.DLVADD_LINE_1;
                    axPurchaseDeliveryHeader.DlvAdd_Line_2 = sqlPurchaseDeliveryHeader.DLVADD_LINE_2;
                    axPurchaseDeliveryHeader.DlvTerritory = sqlPurchaseDeliveryHeader.DLVTERRITORY;
                    axPurchaseDeliveryHeader.DlvZipcode = sqlPurchaseDeliveryHeader.DLVZIPCODE;
                    
                    if (sqlPurchaseDeliveryHeader.HeaderSequence == 1)
                    {
                        axPurchaseDeliveryHeader.Type = UploadPurchOrderService.AxdExtType_TEC_Type.Insert;
                    }
                    else
                    {
                        axPurchaseDeliveryHeader.Type = UploadPurchOrderService.AxdExtType_TEC_Type.Update;
                    }

                    axPurchaseDeliveryHeader.TypeSpecified = true;

                    axPurchaseDeliveryHeader.PurchaseType = UploadPurchOrderService.AxdEnum_PurchaseType.Purch; //Purchase order type
                    axPurchaseDeliveryHeader.PurchaseTypeSpecified = true;
                    axPurchaseDeliveryHeader.VendAccount = sqlPurchaseDeliveryHeader.VENDACCOUNT;
                    //Get the purchase lines

                    var aomLineList = dal.GetPurchaseDeliveryLines(companyID, orderNumber, aomPurchaseDeliveryHeaderID);
                    if (aomLineList != null)
                    {
                        ArrayList axPurchaseOrderLines = new ArrayList();
                        foreach (var sqlPurchaseDeliveryLine in aomLineList)
                        {
                            UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine axPurchaseOrderLine = new UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine();
                            axPurchaseOrderLine.EXTERNAL_ORD_ID = sqlPurchaseDeliveryLine.EXTERNAL_ORD_ID;
                            
                            ArrayList axOrderLineDimensions = new ArrayList();
                            UploadPurchOrderService.AxdType_DimensionAttributeValue dimensionLine = new UploadPurchOrderService.AxdType_DimensionAttributeValue();
                            //Add in the cost centre dimension
                            dimensionLine.Name = "CostCenter";
                            dimensionLine.Value = "0999"; //ToDo: sqlPurchaseOrderLine.Dimension1; //dimensionValue;
                            axOrderLineDimensions.Add(dimensionLine);
                            //Now add the array to a dimension set
                            UploadPurchOrderService.AxdType_DimensionAttributeValueSet dimensionLineSet = new UploadPurchOrderService.AxdType_DimensionAttributeValueSet();
                            dimensionLineSet.Values = (UploadPurchOrderService.AxdType_DimensionAttributeValue[])axOrderLineDimensions.ToArray(typeof(UploadPurchOrderService.AxdType_DimensionAttributeValue));
                            //Finally add the dimension set the Order
                            axPurchaseOrderLine.DefaultDimension = dimensionLineSet;

                            axPurchaseOrderLine.Itemid = sqlPurchaseDeliveryLine.ITEMID;
                            axPurchaseOrderLine.Name = sqlPurchaseDeliveryLine.NAME;
                            
                            axPurchaseOrderLine.LineAmount = sqlPurchaseDeliveryLine.LINEAMOUNT;
                            axPurchaseOrderLine.LineAmountSpecified = true;
                            
                            //2016-01-21 -- we were getting rounding errors from AOM
                            axPurchaseOrderLine.PurchPrice = (sqlPurchaseDeliveryLine.LINEAMOUNT / sqlPurchaseDeliveryLine.PURCHQTY);
                            axPurchaseOrderLine.PurchPriceSpecified = true;
                            
                            axPurchaseOrderLine.LineNum = sqlPurchaseDeliveryLine.Seq;
                            axPurchaseOrderLine.LineNumSpecified = true;

                            //if (sqlPurchaseDeliveryLine.PURCHPRICE != null)
                            //{
                            //    axPurchaseOrderLine.PurchPrice = sqlPurchaseDeliveryLine.PURCHPRICE;
                            //    axPurchaseOrderLine.PurchPriceSpecified = true;
                            //}

                            axPurchaseOrderLine.PurchQty = sqlPurchaseDeliveryLine.PURCHQTY;

                            axPurchaseOrderLine.PurchReceivedNow = sqlPurchaseDeliveryLine.PURCHQTY;
                            if (deliveryRecord == true)
                            {
                                axPurchaseOrderLine.PurchReceivedNow = sqlPurchaseDeliveryLine.PURCHQTY;
                            }
                            axPurchaseOrderLine.PurchReceivedNowSpecified = true;

                            axPurchaseOrderLine.PurchQtySpecified = true;
                            axPurchaseOrderLine.EXTERNAL_DEL_ID = deliveryNumber;

                            axPurchaseOrderLine.PriceUnit = 1;// sqlPurchaseOrderLine.PriceUnit; //Need to get this added
                            axPurchaseOrderLine.PriceUnitSpecified = true;

                            axPurchaseOrderLine.Type = UploadPurchOrderService.AxdExtType_TEC_Type.Insert;

                            //Add the line to the array
                            axPurchaseOrderLines.Add(axPurchaseOrderLine);
                        }
                        //Now add the order lines to the order header.
                        axPurchaseDeliveryHeader.TEC_STG_PurchLine = (UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine[])axPurchaseOrderLines.ToArray(typeof(UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine));
                    }
                    axPurchaseDeliveryCollection.Add(axPurchaseDeliveryHeader);
                    aomPurchaseDeliveryIDs.Add(sqlPurchaseDeliveryHeader.AOMPurchaseDeliveryHeaderID);
                }
                if (recordCount != 0)
                {
                    dal.UpdateAOMPurchaseDeliveryHeaderExtractStatus(aomPurchaseDeliveryIDs, Constants.beingExtracted);
                    axPurchaseDeliveries.TEC_STG_PurchTable = (UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable[])axPurchaseDeliveryCollection.ToArray(typeof(UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable));
                    //Now process the order
                    recordsReturned = true;
                    var response = axPurchaseOrderClient.create(callContext, axPurchaseDeliveries);
                    //Save the records - this updates the status to Updated
                    dal.UpdateAOMPurchaseDeliveryHeaderExtractStatus(aomPurchaseDeliveryIDs, Constants.extracted);
                }
            }
            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling TecPurchOrderService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling TecPurchOrderService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling TecPurchOrderService" + ex.ToString());
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
