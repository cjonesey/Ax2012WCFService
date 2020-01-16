using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class AOMPurchaseDeliveryHeadersEntity
    {
        public int AOMPurchaseDeliveryHeaderID { get; set; }
        public string EXTERNAL_ORD_ID { get; set; }
        public string EXTERNAL_DEL_ID { get; set; }
        public string VENDACCOUNT { get; set; }
        public string ADD_REFERENCE { get; set; }
        public string ADD_LINE_1 { get; set; }
        public string ADD_LINE_2 { get; set; }
        public string ADD_CITY { get; set; }
        public string ADD_TERRITORY { get; set; }
        public string ADD_COUNTRY { get; set; }
        public string ADD_ZIPCODE { get; set; }
        public string VENDORREF { get; set; }
        public Nullable<System.DateTime> DELIVERYDATE { get; set; }
        public string INVENTLOCATIONID { get; set; }
        public Nullable<int> TYPE { get; set; }
        public string SHIP_NAME { get; set; }
        public Nullable<int> COSTOFSALES { get; set; }
        public string DLVNAME { get; set; }
        public string DLVADD_LINE_1 { get; set; }
        public string DLVADD_LINE_2 { get; set; }
        public string DLVCITY { get; set; }
        public string DLVTERRITORY { get; set; }
        public string DLVCOUNTRY { get; set; }
        public string DLVZIPCODE { get; set; }
        public string EXTPURCHTYPE { get; set; }
        public string OPERATORCODE { get; set; }
        public Nullable<int> ORDERUPDATETYPE { get; set; }
        public Nullable<long> DIMENSION { get; set; }
        public string INVENTSITEID { get; set; }
        public Nullable<int> PURCHASETYPE { get; set; }
        public string DLVADD_REFERENCE { get; set; }
        public Nullable<decimal> VATAMOUNT { get; set; }
        public string INVOICEID { get; set; }
        public Nullable<decimal> ORDER_TOTAL_NET { get; set; }
        public Nullable<decimal> ORDER_TOTAL_GROSS { get; set; }
        public Nullable<System.DateTime> MODIFIEDDATETIME { get; set; }
        public Nullable<System.DateTime> CREATEDDATETIME { get; set; }
        public string DATAAREAID { get; set; }
        public string INVENTLOCATIONID1 { get; set; }
        public Nullable<System.DateTime> INVOICEDATE { get; set; }
        public Nullable<byte> ExtractStatus { get; set; }
        public Nullable<byte> HeaderProcessed { get; set; }
        public Nullable<short> HeaderSequence { get; set; }
    }
}
