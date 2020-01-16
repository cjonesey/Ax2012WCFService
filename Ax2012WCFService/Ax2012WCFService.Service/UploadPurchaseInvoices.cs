using Ax2012WCFService.Common;
using Ax2012WCFService.DAL;
using Ax2012WCFService.Entities;
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
    public class UploadPurchaseInvoices : IDisposable
    {
        public string eventMessage { get; set; }
        public event EventHandler Changed;

        private bool disposed = false;
        Ax2012InterfaceDAL dal;

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }
        public UploadPurchaseInvoices()
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
                eventMessage = string.Format("Purchase Invoices - Success:{1} - Error:{2}", DateTime.Now.ToString(), successCount.ToString(), errorCount.ToString());
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
            List<long> purchaseInvoiceIDs;
            recordsReturned = false;
            string externalInvID = "";
            DateTime invoiceDate;
            string vendAccount = "";
            DateTime dateAdded;
            bool hasPurchaseLines = false;
            int validRecords = 0;
            lastErrorMessage = "";
            recordCount = 0;

            Guid invoiceGUID;

            //Get the purchase invoices
            var purchaseInvoiceList = dal.GetPurchaseInvoices(companyID, Constants.purchaseInvoiceBatchSize);

            //Create a reference to the purchase invoice service
            UploadPurchOrderService.TEC_STG_PurchOrderServiceClient axPurchaseInvoiceClient = new UploadPurchOrderService.TEC_STG_PurchOrderServiceClient();

            //inject my endpoint behavior
            axPurchaseInvoiceClient.Endpoint.Behaviors.Add(new SimpleEndpointBehavior());

            //Set the call context - this contains the company details:
            UploadPurchOrderService.CallContext axContext = new UploadPurchOrderService.CallContext();
            //Set the company ID for teh call context
            axContext.Company = companyID;

            //This is the completed invent message (1 or more items)
            UploadPurchOrderService.AxdTEC_STG_PurchOrder axPurchaseOrders = new UploadPurchOrderService.AxdTEC_STG_PurchOrder();

            try
            {
                ArrayList axPurchaseInvoiceCollection = new ArrayList();
                purchaseInvoiceIDs = new List<long>();
                foreach (PurchaseInvoiceHeaderEntity sqlPurchaseInvoiceHeader in purchaseInvoiceList)
                {
                    recordCount++;                      
                    //This is the uniue identifier for the invoice record
                    externalInvID = sqlPurchaseInvoiceHeader.EXTERNAL_INV_ID.Trim();
                    invoiceDate = sqlPurchaseInvoiceHeader.InvoiceDate;
                    vendAccount = sqlPurchaseInvoiceHeader.VENDACCOUNT.Trim();
                    dateAdded = sqlPurchaseInvoiceHeader.DateAdded;
                    invoiceGUID = sqlPurchaseInvoiceHeader.InvoiceGUID;

                    UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable axPurchaseOrderHeader = new UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable();
                    axPurchaseOrderHeader.OrderUpdateType = UploadPurchOrderService.AxdEnum_TEC_OrderUpdateType.Invoice;
                    axPurchaseOrderHeader.OrderUpdateTypeSpecified = true;

                    axPurchaseOrderHeader.InvoiceDate = invoiceDate;
                    axPurchaseOrderHeader.InvoiceDateSpecified = true;

                    //get the buyers order number
                    axPurchaseOrderHeader.EXTERNAL_ORD_ID = sqlPurchaseInvoiceHeader.EXTERNAL_ORD_ID.Trim();

                    ArrayList axOrderHeaderDimensions = new ArrayList();
                    UploadPurchOrderService.AxdType_DimensionAttributeValue dimensionHeader = new UploadPurchOrderService.AxdType_DimensionAttributeValue();

                    //dimensionHeader.Name = "CostCenter";
                    //dimensionHeader.Value = "0999"; //dimensionValue - will take the dimensions from the line
                    //axOrderHeaderDimensions.Add(dimensionHeader);
                    ////Now add the array to a dimension set
                    //tecPurchOrderService.AxdType_DimensionAttributeValueSet dimensionHeaderSet = new tecPurchOrderService.AxdType_DimensionAttributeValueSet();
                    //dimensionHeaderSet.Values = (tecPurchOrderService.AxdType_DimensionAttributeValue[])axOrderHeaderDimensions.ToArray(typeof(tecPurchOrderService.AxdType_DimensionAttributeValue));
                    ////Finally add the dimension set the Order
                    //axPurchaseOrderHeader.DefaultDimension = dimensionHeaderSet;

                    //axPurchaseOrderHeader.Add_Reference = sqlPurchaseOrderHeader.Inv_Add_Reference;
                    axPurchaseOrderHeader.Add_City = ""; // sqlPurchaseInvoiceHeader.SupplierCity;
                    axPurchaseOrderHeader.Add_Country = ""; // sqlPurchaseInvoiceHeader.SupplierState;
                    axPurchaseOrderHeader.Add_Line_1 = ""; // sqlPurchaseInvoiceHeader.SupplierAddressLine;
                    axPurchaseOrderHeader.Add_Line_2 = "";
                    axPurchaseOrderHeader.Add_Territory = ""; // sqlPurchaseInvoiceHeader.SupplierCountryCode;
                    axPurchaseOrderHeader.Add_Zipcode = "";  // sqlPurchaseInvoiceHeader.SupplierPostCode.Substring(1, 16);


                    //This is an odd one as we will need to get the site number from the purchase order first
                    //axPurchaseOrderHeader.InventLocationId = "0";// sqlPurchaseOrderHeader.Inventlocationid; (Do not update the site as this has already been provided)
                    axPurchaseOrderHeader.InventSiteId = Constants.defaultSpicersSiteId.ToString();

                    //axPurchaseOrderHeader.Invoice_Name = sqlPurchaseOrderHeader.Invoice_Name;
                    axPurchaseOrderHeader.OperatorCode = ""; //Not on an Invoice
                    axPurchaseOrderHeader.DlvCity = "";//sqlPurchaseInvoiceHeader.DeliveryToCity;
                    axPurchaseOrderHeader.DlvCountry = "";//sqlPurchaseInvoiceHeader.InvoiceToCountryCode;
                    axPurchaseOrderHeader.DlvName = ""; //sqlPurchaseInvoiceHeader.DeliveryToName;
                    axPurchaseOrderHeader.DlvAdd_Line_1 = "";//sqlPurchaseInvoiceHeader.DeliveryToAddressLine ;
                    axPurchaseOrderHeader.DlvAdd_Line_2 = "";
                    axPurchaseOrderHeader.DlvTerritory = "";//sqlPurchaseInvoiceHeader.DeliveryToState;
                    axPurchaseOrderHeader.DlvZipcode = "";//sqlPurchaseInvoiceHeader.DeliveryToPostCode;
                    axPurchaseOrderHeader.Type = UploadPurchOrderService.AxdExtType_TEC_Type.Update;
                    axPurchaseOrderHeader.TypeSpecified = true;
                    axPurchaseOrderHeader.PurchaseType = UploadPurchOrderService.AxdEnum_PurchaseType.Purch; //Purchase order type
                    axPurchaseOrderHeader.PurchaseTypeSpecified = true;
                    axPurchaseOrderHeader.VendAccount = vendAccount;

                    axPurchaseOrderHeader.VATAmount = sqlPurchaseInvoiceHeader.VATAmount;
                    axPurchaseOrderHeader.VATAmountSpecified = true;

                    axPurchaseOrderHeader.Order_Total_Gross = sqlPurchaseInvoiceHeader.Order_Total_Gross;
                    axPurchaseOrderHeader.Order_Total_GrossSpecified = true;

                    axPurchaseOrderHeader.Order_Total_Net = sqlPurchaseInvoiceHeader.Order_Total_Net;
                    axPurchaseOrderHeader.Order_Total_NetSpecified = true;

                    axPurchaseOrderHeader.VendorRef = externalInvID;
                    axPurchaseOrderHeader.InvoiceDate = invoiceDate;

                    axPurchaseOrderHeader.InvoiceId = externalInvID;
                    
                    //Get the purchase invoice lines
                    hasPurchaseLines = false;
                    var sqlPurchaseInvoiceLines = dal.GetPurchaseInvoiceLines(companyID, invoiceDate, externalInvID, vendAccount);
                    if (sqlPurchaseInvoiceLines != null && sqlPurchaseInvoiceLines.Count() > 0)
                    {
                        hasPurchaseLines = true;
                        ArrayList axPurchaseInvoiceLines = new ArrayList();
                        foreach (var sqlPurchaseInvoiceLine in sqlPurchaseInvoiceLines)
                        {
                            UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine axPurchaseInvoiceLine = new UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine();
                            axPurchaseInvoiceLine.EXTERNAL_ORD_ID = sqlPurchaseInvoiceLine.EXTERNAL_ORD_ID;

                            //ArrayList axOrderLineDimensions = new ArrayList();
                            //tecPurchOrderService.AxdType_DimensionAttributeValue dimensionLine = new tecPurchOrderService.AxdType_DimensionAttributeValue();
                            //dimensionLine.Name = "CostCenter";
                            //dimensionLine.Value = "0999"; //sqlPurchaseOrderLine.Dimension1; //dimensionValue;
                            //axOrderLineDimensions.Add(dimensionLine);
                            ////Now add the array to a dimension set
                            //tecPurchOrderService.AxdType_DimensionAttributeValueSet dimensionLineSet = new tecPurchOrderService.AxdType_DimensionAttributeValueSet();
                            //dimensionLineSet.Values = (tecPurchOrderService.AxdType_DimensionAttributeValue[])axOrderLineDimensions.ToArray(typeof(tecPurchOrderService.AxdType_DimensionAttributeValue));
                            ////Finally add the dimension set the Order
                            //axPurchaseInvoiceLine.DefaultDimension = dimensionLineSet;

                            //axPurchaseInvoiceLine.InventSiteId = sqlPurchaseOrderLine.InventSiteId; //Takes the site from the order header
                            axPurchaseInvoiceLine.Itemid = sqlPurchaseInvoiceLine.ItemID;
                            axPurchaseInvoiceLine.Name = sqlPurchaseInvoiceLine.Name;

                            //axPurchaseInvoiceLine.LineNum = sqlPurchaseInvoiceLine.LineNumber;
                            axPurchaseInvoiceLine.LineNum = sqlPurchaseInvoiceLine.LineNum;
                            axPurchaseInvoiceLine.LineNumSpecified = true;

                            //axPurchaseInvoiceLine.EXTERNAL_DEL_ID = sqlPurchaseInvoiceLine.DeliveryNoteNumber;
                            axPurchaseInvoiceLine.EXTERNAL_DEL_ID = sqlPurchaseInvoiceLine.EXTERNAL_DEL_ID;

                            //axPurchaseInvoiceLine.PurchPrice = sqlPurchaseInvoiceLine.UnitPrice;
                            axPurchaseInvoiceLine.PurchPrice = sqlPurchaseInvoiceLine.PurchPrice;
                            axPurchaseInvoiceLine.PurchPriceSpecified = true;

                            axPurchaseInvoiceLine.PurchQty = sqlPurchaseInvoiceLine.PurchQty; //This is the original qty
                            axPurchaseInvoiceLine.LineAmount = sqlPurchaseInvoiceLine.LINE_TOTAL_EX_VAT; //Done
                            axPurchaseInvoiceLine.PurchReceivedNow = sqlPurchaseInvoiceLine.PurchReceivedNow; //Done
                            axPurchaseInvoiceLine.VATLineAmount = sqlPurchaseInvoiceLine.VATAmount; //Done
                            axPurchaseInvoiceLine.LineAmountSpecified = true;//Done
                            axPurchaseInvoiceLine.PurchQtySpecified = true;//Done
                            axPurchaseInvoiceLine.PurchReceivedNowSpecified = true;//Done
                            axPurchaseInvoiceLine.VATLineAmountSpecified = true;//Done
                            axPurchaseInvoiceLine.EXTERNAL_INV_ID = externalInvID; //Done
                            axPurchaseInvoiceLine.PriceUnit = 1;// sqlPurchaseOrderLine.PriceUnit; //Need to get this added
                            axPurchaseInvoiceLine.PriceUnitSpecified = true;

                            //2016-07-08 New field for the Invoice Price - basically the Line Amount / Receievd Qty
                            axPurchaseInvoiceLine.InvoicePrice = (sqlPurchaseInvoiceLine.LINE_TOTAL_EX_VAT / sqlPurchaseInvoiceLine.PurchReceivedNow);
                            axPurchaseInvoiceLine.InvoicePriceSpecified = true;

                            axPurchaseInvoiceLine.Type = UploadPurchOrderService.AxdExtType_TEC_Type.Update;
                            axPurchaseInvoiceLine.TypeSpecified = true;
                            //Add the line to the array
                            axPurchaseInvoiceLines.Add(axPurchaseInvoiceLine);
                        }//End of  add lines loop
                        //Now add the order lines to the order header.
                        axPurchaseOrderHeader.TEC_STG_PurchLine = (UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine[])axPurchaseInvoiceLines.ToArray(typeof(UploadPurchOrderService.AxdEntity_TEC_STG_PurchLine));
                    }
                    if (hasPurchaseLines == true)
                    {
                        axPurchaseInvoiceCollection.Add(axPurchaseOrderHeader);
                        purchaseInvoiceIDs.Add(sqlPurchaseInvoiceHeader.PurchaseInvoiceHeadersID);
                        validRecords++;
                    }
                }
                if (recordCount != 0)
                {
                    dal.UpdateAOMPurchaseInvoiceHeaderExtractStatus(purchaseInvoiceIDs, Constants.beingExtracted);
                    recordsReturned = true;
                }
                if (validRecords > 0)
                {
                    axPurchaseOrders.TEC_STG_PurchTable = (UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable[])axPurchaseInvoiceCollection.ToArray(typeof(UploadPurchOrderService.AxdEntity_TEC_STG_PurchTable));
                    var response = axPurchaseInvoiceClient.create(axContext, axPurchaseOrders);
                    dal.UpdateAOMPurchaseInvoiceHeaderExtractStatus(purchaseInvoiceIDs, Constants.extracted);
                }
            }
            catch (FaultException exFault)
            {
                Debug.WriteLine("Error occured whilst calling tecPurchaseInvoiceOrderService" + exFault.ToString());
                lastErrorMessage = exFault.ToString();
                return false;
            }
            catch (CommunicationException exComm)
            {
                Debug.WriteLine("Error occured whilst calling tecPurchaseInvoiceOrderService" + exComm.ToString());
                lastErrorMessage = exComm.ToString();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured whilst calling tecPurchaseInvoiceOrderService" + ex.ToString());
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
