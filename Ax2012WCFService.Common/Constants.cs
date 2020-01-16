using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Common
{
    public class Constants
    {
        public const byte notExtracted = 0;
        public const byte beingExtracted = 1;
        public const byte extracted = 2;


        public const int customerBatchSize = 1;
        public const int addressBatchSize = 11;
        public const int vendorBatchSize = 11;
        public const int salesInvoiceBatchSize = 50;
        public const int inventBatchSize = 50;
        public const int purchaseInvoiceBatchSize = 1;
        public const int defaultInventSiteID = 0;

        public const int orderBatchSize = 1;
        public const int productBatchSize = 1;

        public const string defaultCompany = "008";

        public static List<string> validCompanies = new List<string> { "008","013", "015"};
        public static List<string> validInterfaces = new List<string> { "Customer", "SalesInvoice", "Address", "Vendor"};//, "AOM Purchase Delivery", "AOM Items" };
        public static List<string> validInterfacesSC = new List<string> { "CUS", "APD", "ADD", "SLJ", "PRD" };


        public const string defaultSpicersSiteId = "SPI";
        public const string defaultOfficeTeamSiteId = "OFT";

        public const int batchJobWaiting = 1;

    }
}
