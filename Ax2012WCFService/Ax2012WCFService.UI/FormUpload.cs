using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ax2012WCFService;
using System.Diagnostics;
using System.Threading;
using Ax2012WCFService.Common;

namespace Ax2012WCFService.UI
{
    public partial class FormUpload : Form
    {
        private bool counterEnabled = false;
        private DateTime nextRun = System.DateTime.Now;
        private double standardInterval = 0.25;

        public FormUpload()
        {
            InitializeComponent();
            foreach (string validCompany in Constants.validCompanies)
            {
                checkedListBoxCompanies.Items.Add(validCompany);
                comboBoxSelectCompany.Items.Add(validCompany);
            }
            foreach (string validInterface in Constants.validInterfaces)
            {
                checkedListBoxInterfaces.Items.Add(validInterface, true);
            }

        }

        Ax2012WCFService.Service.UploadCustomer uploadCustomers;
        Ax2012WCFService.Service.UploadAddress uploadAddress;
        Ax2012WCFService.Service.UploadSalesInvoice uploadSalesInvoice;
        Ax2012WCFService.Service.UploadAOMPurchaseDeliveries uploadAOMPurchaseDeliveries;
        Ax2012WCFService.Service.UploadAOMInventTable uploadAOMInventTable;
        Ax2012WCFService.Service.UploadVendor uploadVendors;
        Ax2012WCFService.Service.UploadPurchaseInvoices uploadPurchaseInvoices;

        private void buttonVendor_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                string lastErrorMessage = "";
                UpdateVendors(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }
        }

        private void buttonCustomer_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdateCustomers(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }
        }

        private void buttonAddress_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdateAddresses(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }
        }

        private void buttonSalesJournal_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdateSalesJournal(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }
        }

        private void buttonAOMPurchaseDeliveries_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdateAOMPurchaseDeliveries(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }

        }

        private void buttonAOMProducts_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdateAOMProducts(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }
        }


        private void buttonEnable_Click(object sender, EventArgs e)
        {
            if (counterEnabled == false)
            {
                timer1.Interval = 1000; //Set the timeout to be 2 minutes
                nextRun = System.DateTime.Now.AddMinutes(standardInterval);

                buttonEnable.Text = "Disable";
                counterEnabled = true;
                Thread.Sleep(3000); //Sleep for 30 Seconds before doing anything
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                buttonEnable.Text = "Enable";
                counterEnabled = false;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelCounter.Text = string.Format("{0}:{1}:{2}", ((int)(nextRun - System.DateTime.Now).TotalHours).ToString().PadLeft(2, '0'), ((int)(nextRun - System.DateTime.Now).Minutes).ToString().PadLeft(2, '0'), ((int)(nextRun - System.DateTime.Now).Seconds).ToString().PadLeft(2, '0'));

            if (System.DateTime.Now >= nextRun)
            {
                timer1.Enabled = false;
                RunUpdates();
                timer1.Enabled = true;
            }

        }

        private void RunUpdates()
        {
            ListViewItem lvi;
            string message = "";
            foreach (var companyChecked in checkedListBoxCompanies.CheckedItems)
            {
                string selectedCompany = companyChecked.ToString();
                lvi = new ListViewItem();
                lvi.Text = DateTime.Now.ToString();
                lvi.SubItems.Add(String.Format("Running company {0}", selectedCompany));
                listViewFeedback.Items.Add(lvi);
                
                foreach (var interfaceChecked in checkedListBoxInterfaces.CheckedItems)
                {
                    switch (interfaceChecked.ToString())                       
                    {
                        case "Customer":
                            message = "Calling Customers";
                            lvi = new ListViewItem();
                            lvi.Text = DateTime.Now.ToString();
                            lvi.SubItems.Add(message);
                            listViewFeedback.Items.Add(lvi);
                            Thread.Sleep(200);
                            //UpdateCustomers(selectedCompany);
                            break;
                        case "Vendor":
                            break;
                        case "SalesInvoice":
                            message = "Calling Sales Journals";
                            lvi = new ListViewItem();
                            lvi.Text = DateTime.Now.ToString();
                            lvi.SubItems.Add(message);
                            listViewFeedback.Items.Add(lvi);
                            nextRun = System.DateTime.Now.AddMinutes(standardInterval);
                            Thread.Sleep(200);
                            //UpdateSalesJournal(selectedCompany);
                            break;
                        case "AOM Purchase Delivery":
                            break;
                        case "AOM Items":
                            break;
                        case "Address":
                            message = "Calling Addresses";
                            lvi = new ListViewItem();
                            lvi.Text = DateTime.Now.ToString();
                            lvi.SubItems.Add(message);
                            listViewFeedback.Items.Add(lvi);
                            Thread.Sleep(200);
                            //UpdateAddresses(selectedCompany);
                            break;
                        default:
                            break;

                    }                
                }
            }

        }

        private void UpdateAOMPurchaseDeliveries(string companyID)
        {
            string lastErrorMessage = "";
            uploadAOMPurchaseDeliveries = new Ax2012WCFService.Service.UploadAOMPurchaseDeliveries();
            uploadAOMPurchaseDeliveries.Changed += new EventHandler(ListChanged);
            uploadAOMPurchaseDeliveries.Upload(companyID, out lastErrorMessage, false);
        }

        private void UpdatePurchaseInvoices(string companyID)
        {
            string lastErrorMessage = "";
            uploadPurchaseInvoices = new Ax2012WCFService.Service.UploadPurchaseInvoices();
            uploadPurchaseInvoices.Changed += new EventHandler(ListChanged);
            uploadPurchaseInvoices.Upload(companyID, out lastErrorMessage);
        }

        private void UpdateAOMProducts(string companyID)
        {
            string lastErrorMessage = "";
            uploadAOMInventTable = new Ax2012WCFService.Service.UploadAOMInventTable();
            uploadAOMInventTable.Changed += new EventHandler(ListChanged);
            uploadAOMInventTable.Upload(companyID, out lastErrorMessage);
        }

        public void UpdateSalesJournal(string companyID)
        {
            string lastErrorMessage = "";
            uploadSalesInvoice = new Ax2012WCFService.Service.UploadSalesInvoice();
            uploadSalesInvoice.Changed += new EventHandler(ListChanged);
            uploadSalesInvoice.Upload(companyID, out lastErrorMessage);
        }

        public void UpdateCustomers(string companyID)
        {
            string lastErrorMessage = "";
            uploadCustomers = new Ax2012WCFService.Service.UploadCustomer();
            uploadCustomers.Changed += new EventHandler(ListChanged);
            uploadCustomers.Upload(companyID, out lastErrorMessage);
            uploadCustomers.Dispose();
        }

        public void UpdateVendors(string companyID)
        {
            string lastErrorMessage = "";
            uploadVendors = new Ax2012WCFService.Service.UploadVendor();
            uploadVendors.Changed += new EventHandler(ListChanged);
            uploadVendors.Upload(companyID, out lastErrorMessage);
            uploadVendors.Dispose();
        }

        public void UpdateAddresses(string companyID)
        {
            string lastErrorMessage = "";
            uploadAddress = new Ax2012WCFService.Service.UploadAddress();
            uploadAddress.Changed += new EventHandler(ListChanged);
            uploadAddress.Upload(companyID, out lastErrorMessage);
            uploadAddress.Dispose();
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
            ListViewItem lvi = new ListViewItem();
            lvi.Text = DateTime.Now.ToString();
            lvi.SubItems.Add(message);
            listViewFeedback.Items.Add(lvi);
        }

        private void buttonPurchaseInvoice_Click(object sender, EventArgs e)
        {
            if (comboBoxSelectCompany.SelectedItem != null)
            {
                string selectedCompany = comboBoxSelectCompany.SelectedItem.ToString();
                UpdatePurchaseInvoices(selectedCompany);
            }
            else
            {
                MessageBox.Show("Please select a company before proceeding");
            }


        }


    }
}
