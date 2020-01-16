using Ax2012WCFService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.ConsoleApp
{
    class Program
    {

        Ax2012WCFService.Service.UploadCustomer uploadCustomers;
        Ax2012WCFService.Service.UploadAddress uploadAddress;
        Ax2012WCFService.Service.UploadSalesInvoice uploadSalesInvoice;
        Ax2012WCFService.Service.UploadAOMPurchaseDeliveries uploadAOMPurchaseDeliveries;
        Ax2012WCFService.Service.UploadAOMInventTable uploadAOMInventTable;
        int exitCode = 0;

        static void Main(string[] args)
        {
            bool runJobsAndClose = false;
            
            string companyID = "";
            bool validCompany = false;
            string iInterface = "";
            bool validSelection = false;

            foreach (string arg in args)
            {
                switch (arg.ToUpper().PadRight(3, ' ').Substring(0,3))
                {
                    case "-CO":
                        companyID = arg.ToUpper().PadRight(6, ' ').Substring(3, 3);
                        if (Constants.validCompanies.Contains(companyID))
                            validCompany = true;
                        break;
                    case "PRD":
                    case "CUS":
                    case "APD":
                    case "ADD": 
                    case "SLJ":
                        iInterface = arg;
                        if (Constants.validInterfacesSC.Contains(iInterface))
                            validSelection = true;
                        break;

                    case "-RA":
                        runJobsAndClose = true;
                        break;
                }
            }

            if (runJobsAndClose != true)
            {
                new Program().DoSomething();
                Console.ReadLine();
            }
            else if (runJobsAndClose == true && validCompany == true && validSelection == true)
            {
                new Program().DoSomething(companyID, validCompany, iInterface, validSelection);
                Environment.Exit(0);
            }
            
        }

        private void DoSomething(string companyID = "", bool validCompany = false, string iInterface = "", bool validSelection = false)
        {
            string lastErrorMessage = "";

            while (validCompany == false)
            {
                Console.WriteLine("Please enter Company - e.g. 008");
                companyID = Console.ReadLine();
                if (Constants.validCompanies.Contains(companyID))
                    validCompany = true;
            }

            while (validSelection == false)
            {
                Console.WriteLine("Please select an interface");
                foreach (string oInterface in Constants.validInterfacesSC)
                {
                    Console.Write(string.Format("{0}:", oInterface));
                }
                iInterface = Console.ReadLine();
                if (Constants.validInterfacesSC.Contains(iInterface))
                    validSelection = true;
            }

            switch(iInterface)
            {
                case "APD":
                    uploadAOMPurchaseDeliveries = new Ax2012WCFService.Service.UploadAOMPurchaseDeliveries();
                    uploadAOMPurchaseDeliveries.Changed += new EventHandler(ListChanged);
                    uploadAOMPurchaseDeliveries.Upload(companyID, out lastErrorMessage, false);
                    uploadAOMPurchaseDeliveries.Dispose();
                    break;
                case "CUS":
                    uploadCustomers = new Ax2012WCFService.Service.UploadCustomer();
                    uploadCustomers.Changed += new EventHandler(ListChanged);
                    uploadCustomers.Upload(companyID, out lastErrorMessage);
                    uploadCustomers.Dispose();
                    break;
                case "SLJ":
                    uploadSalesInvoice = new Ax2012WCFService.Service.UploadSalesInvoice();
                    uploadSalesInvoice.Changed += new EventHandler(ListChanged);
                    uploadSalesInvoice.Upload(companyID, out lastErrorMessage);
                    uploadSalesInvoice.Dispose();
                    break;
                case "ADD":
                    uploadAddress = new Ax2012WCFService.Service.UploadAddress();
                    uploadAddress.Changed += new EventHandler(ListChanged);
                    uploadAddress.Upload(companyID, out lastErrorMessage);
                    uploadAddress.Dispose();
                    break;
                case "PRD":                    
                    uploadAOMInventTable = new Ax2012WCFService.Service.UploadAOMInventTable();
                    uploadAOMInventTable.Changed += new EventHandler(ListChanged);
                    uploadAOMInventTable.Upload(companyID, out lastErrorMessage);
                    uploadAOMInventTable.Dispose();
                    break;

            }
            

        }
        
        // This will be called whenever the list changes:
        private void ListChanged(object sender, EventArgs e)
        {
            string message = "";
            if (sender.GetType().FullName == "Ax2012WCFService.Service.UploadCustomer")
            {
                message = uploadCustomers.eventMessage;
            }
            if (sender.GetType().FullName == "Ax2012WCFService.Service.UploadAddress")
            {
                message = uploadAddress.eventMessage;
            }
            if (sender.GetType().FullName == "Ax2012WCFService.Service.UploadSalesInvoice")
            {
                message = uploadSalesInvoice.eventMessage;
            }
            if (sender.GetType().FullName == "Ax2012WCFService.Service.UploadAOMPurchaseDeliveries")
            {
                message = uploadAOMPurchaseDeliveries.eventMessage;
            }
            Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString(), message));
        }
    }
}
