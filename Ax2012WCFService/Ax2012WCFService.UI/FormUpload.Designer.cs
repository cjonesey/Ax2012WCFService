namespace Ax2012WCFService.UI
{
    partial class FormUpload
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonAddress = new System.Windows.Forms.Button();
            this.buttonVendor = new System.Windows.Forms.Button();
            this.buttonSalesJournal = new System.Windows.Forms.Button();
            this.buttonCustomer = new System.Windows.Forms.Button();
            this.listViewFeedback = new System.Windows.Forms.ListView();
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxFeedback = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.buttonEnable = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelCounter = new System.Windows.Forms.Label();
            this.buttonAOMPurchaseDeliveries = new System.Windows.Forms.Button();
            this.buttonAOMProducts = new System.Windows.Forms.Button();
            this.groupBoxManual = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSelectCompany = new System.Windows.Forms.ComboBox();
            this.groupBox2Automatic = new System.Windows.Forms.GroupBox();
            this.labelInterfaces = new System.Windows.Forms.Label();
            this.checkedListBoxInterfaces = new System.Windows.Forms.CheckedListBox();
            this.labelCompanies = new System.Windows.Forms.Label();
            this.checkedListBoxCompanies = new System.Windows.Forms.CheckedListBox();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonPurchaseInvoice = new System.Windows.Forms.Button();
            this.groupBoxFeedback.SuspendLayout();
            this.groupBoxManual.SuspendLayout();
            this.groupBox2Automatic.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddress
            // 
            this.buttonAddress.Location = new System.Drawing.Point(109, 76);
            this.buttonAddress.Name = "buttonAddress";
            this.buttonAddress.Size = new System.Drawing.Size(91, 23);
            this.buttonAddress.TabIndex = 1;
            this.buttonAddress.Text = "Addresses";
            this.buttonAddress.UseVisualStyleBackColor = true;
            this.buttonAddress.Click += new System.EventHandler(this.buttonAddress_Click);
            // 
            // buttonVendor
            // 
            this.buttonVendor.Location = new System.Drawing.Point(108, 19);
            this.buttonVendor.Name = "buttonVendor";
            this.buttonVendor.Size = new System.Drawing.Size(90, 24);
            this.buttonVendor.TabIndex = 2;
            this.buttonVendor.Text = "Vendor";
            this.buttonVendor.UseVisualStyleBackColor = true;
            this.buttonVendor.Click += new System.EventHandler(this.buttonVendor_Click);
            // 
            // buttonSalesJournal
            // 
            this.buttonSalesJournal.Location = new System.Drawing.Point(12, 77);
            this.buttonSalesJournal.Name = "buttonSalesJournal";
            this.buttonSalesJournal.Size = new System.Drawing.Size(91, 23);
            this.buttonSalesJournal.TabIndex = 3;
            this.buttonSalesJournal.Text = "Sales Journal";
            this.buttonSalesJournal.UseVisualStyleBackColor = true;
            this.buttonSalesJournal.Click += new System.EventHandler(this.buttonSalesJournal_Click);
            // 
            // buttonCustomer
            // 
            this.buttonCustomer.Location = new System.Drawing.Point(108, 47);
            this.buttonCustomer.Name = "buttonCustomer";
            this.buttonCustomer.Size = new System.Drawing.Size(91, 23);
            this.buttonCustomer.TabIndex = 4;
            this.buttonCustomer.Text = "Customer";
            this.buttonCustomer.UseVisualStyleBackColor = true;
            this.buttonCustomer.Click += new System.EventHandler(this.buttonCustomer_Click);
            // 
            // listViewFeedback
            // 
            this.listViewFeedback.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewFeedback.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Date,
            this.Message});
            this.listViewFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFeedback.GridLines = true;
            this.listViewFeedback.Location = new System.Drawing.Point(3, 16);
            this.listViewFeedback.Name = "listViewFeedback";
            this.listViewFeedback.Size = new System.Drawing.Size(573, 179);
            this.listViewFeedback.TabIndex = 5;
            this.listViewFeedback.UseCompatibleStateImageBehavior = false;
            this.listViewFeedback.View = System.Windows.Forms.View.Details;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 172;
            // 
            // Message
            // 
            this.Message.Text = "Message";
            this.Message.Width = 398;
            // 
            // groupBoxFeedback
            // 
            this.groupBoxFeedback.Controls.Add(this.listViewFeedback);
            this.groupBoxFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFeedback.Location = new System.Drawing.Point(0, 236);
            this.groupBoxFeedback.Name = "groupBoxFeedback";
            this.groupBoxFeedback.Size = new System.Drawing.Size(579, 198);
            this.groupBoxFeedback.TabIndex = 6;
            this.groupBoxFeedback.TabStop = false;
            this.groupBoxFeedback.Text = "Messages";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 434);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(579, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // buttonEnable
            // 
            this.buttonEnable.Location = new System.Drawing.Point(461, 74);
            this.buttonEnable.Name = "buttonEnable";
            this.buttonEnable.Size = new System.Drawing.Size(93, 23);
            this.buttonEnable.TabIndex = 8;
            this.buttonEnable.Text = "Disabled";
            this.buttonEnable.UseVisualStyleBackColor = true;
            this.buttonEnable.Click += new System.EventHandler(this.buttonEnable_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelCounter
            // 
            this.labelCounter.AutoSize = true;
            this.labelCounter.Location = new System.Drawing.Point(510, 16);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(34, 13);
            this.labelCounter.TabIndex = 9;
            this.labelCounter.Text = "00:00";
            // 
            // buttonAOMPurchaseDeliveries
            // 
            this.buttonAOMPurchaseDeliveries.Location = new System.Drawing.Point(12, 47);
            this.buttonAOMPurchaseDeliveries.Name = "buttonAOMPurchaseDeliveries";
            this.buttonAOMPurchaseDeliveries.Size = new System.Drawing.Size(90, 24);
            this.buttonAOMPurchaseDeliveries.TabIndex = 10;
            this.buttonAOMPurchaseDeliveries.Text = "AOM Dely";
            this.buttonAOMPurchaseDeliveries.UseVisualStyleBackColor = true;
            this.buttonAOMPurchaseDeliveries.Click += new System.EventHandler(this.buttonAOMPurchaseDeliveries_Click);
            // 
            // buttonAOMProducts
            // 
            this.buttonAOMProducts.Location = new System.Drawing.Point(12, 19);
            this.buttonAOMProducts.Name = "buttonAOMProducts";
            this.buttonAOMProducts.Size = new System.Drawing.Size(90, 24);
            this.buttonAOMProducts.TabIndex = 11;
            this.buttonAOMProducts.Text = "AOM Prod";
            this.buttonAOMProducts.UseVisualStyleBackColor = true;
            this.buttonAOMProducts.Click += new System.EventHandler(this.buttonAOMProducts_Click);
            // 
            // groupBoxManual
            // 
            this.groupBoxManual.Controls.Add(this.buttonPurchaseInvoice);
            this.groupBoxManual.Controls.Add(this.label1);
            this.groupBoxManual.Controls.Add(this.comboBoxSelectCompany);
            this.groupBoxManual.Controls.Add(this.buttonAOMProducts);
            this.groupBoxManual.Controls.Add(this.buttonAOMPurchaseDeliveries);
            this.groupBoxManual.Controls.Add(this.buttonCustomer);
            this.groupBoxManual.Controls.Add(this.buttonAddress);
            this.groupBoxManual.Controls.Add(this.buttonSalesJournal);
            this.groupBoxManual.Controls.Add(this.buttonVendor);
            this.groupBoxManual.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxManual.Location = new System.Drawing.Point(0, 0);
            this.groupBoxManual.Name = "groupBoxManual";
            this.groupBoxManual.Size = new System.Drawing.Size(579, 109);
            this.groupBoxManual.TabIndex = 14;
            this.groupBoxManual.TabStop = false;
            this.groupBoxManual.Text = "Manual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(221, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Select Company";
            // 
            // comboBoxSelectCompany
            // 
            this.comboBoxSelectCompany.FormattingEnabled = true;
            this.comboBoxSelectCompany.Location = new System.Drawing.Point(311, 19);
            this.comboBoxSelectCompany.Name = "comboBoxSelectCompany";
            this.comboBoxSelectCompany.Size = new System.Drawing.Size(159, 21);
            this.comboBoxSelectCompany.TabIndex = 12;
            // 
            // groupBox2Automatic
            // 
            this.groupBox2Automatic.Controls.Add(this.labelInterfaces);
            this.groupBox2Automatic.Controls.Add(this.checkedListBoxInterfaces);
            this.groupBox2Automatic.Controls.Add(this.labelCompanies);
            this.groupBox2Automatic.Controls.Add(this.checkedListBoxCompanies);
            this.groupBox2Automatic.Controls.Add(this.buttonSelectAll);
            this.groupBox2Automatic.Controls.Add(this.labelCounter);
            this.groupBox2Automatic.Controls.Add(this.buttonEnable);
            this.groupBox2Automatic.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2Automatic.Location = new System.Drawing.Point(0, 109);
            this.groupBox2Automatic.Name = "groupBox2Automatic";
            this.groupBox2Automatic.Size = new System.Drawing.Size(579, 127);
            this.groupBox2Automatic.TabIndex = 15;
            this.groupBox2Automatic.TabStop = false;
            this.groupBox2Automatic.Text = "Automatic";
            // 
            // labelInterfaces
            // 
            this.labelInterfaces.AutoSize = true;
            this.labelInterfaces.Location = new System.Drawing.Point(197, 16);
            this.labelInterfaces.Name = "labelInterfaces";
            this.labelInterfaces.Size = new System.Drawing.Size(54, 13);
            this.labelInterfaces.TabIndex = 17;
            this.labelInterfaces.Text = "Interfaces";
            // 
            // checkedListBoxInterfaces
            // 
            this.checkedListBoxInterfaces.FormattingEnabled = true;
            this.checkedListBoxInterfaces.Location = new System.Drawing.Point(200, 34);
            this.checkedListBoxInterfaces.Name = "checkedListBoxInterfaces";
            this.checkedListBoxInterfaces.Size = new System.Drawing.Size(238, 79);
            this.checkedListBoxInterfaces.TabIndex = 16;
            // 
            // labelCompanies
            // 
            this.labelCompanies.AutoSize = true;
            this.labelCompanies.Location = new System.Drawing.Point(12, 16);
            this.labelCompanies.Name = "labelCompanies";
            this.labelCompanies.Size = new System.Drawing.Size(59, 13);
            this.labelCompanies.TabIndex = 15;
            this.labelCompanies.Text = "Companies";
            // 
            // checkedListBoxCompanies
            // 
            this.checkedListBoxCompanies.FormattingEnabled = true;
            this.checkedListBoxCompanies.Location = new System.Drawing.Point(12, 34);
            this.checkedListBoxCompanies.Name = "checkedListBoxCompanies";
            this.checkedListBoxCompanies.Size = new System.Drawing.Size(167, 79);
            this.checkedListBoxCompanies.TabIndex = 14;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(461, 45);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(93, 23);
            this.buttonSelectAll.TabIndex = 10;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            // 
            // buttonPurchaseInvoice
            // 
            this.buttonPurchaseInvoice.Location = new System.Drawing.Point(206, 75);
            this.buttonPurchaseInvoice.Name = "buttonPurchaseInvoice";
            this.buttonPurchaseInvoice.Size = new System.Drawing.Size(90, 24);
            this.buttonPurchaseInvoice.TabIndex = 14;
            this.buttonPurchaseInvoice.Text = "Purchase Inv";
            this.buttonPurchaseInvoice.UseVisualStyleBackColor = true;
            this.buttonPurchaseInvoice.Click += new System.EventHandler(this.buttonPurchaseInvoice_Click);
            // 
            // FormUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 456);
            this.Controls.Add(this.groupBoxFeedback);
            this.Controls.Add(this.groupBox2Automatic);
            this.Controls.Add(this.groupBoxManual);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FormUpload";
            this.Text = "Form1";
            this.groupBoxFeedback.ResumeLayout(false);
            this.groupBoxManual.ResumeLayout(false);
            this.groupBoxManual.PerformLayout();
            this.groupBox2Automatic.ResumeLayout(false);
            this.groupBox2Automatic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddress;
        private System.Windows.Forms.Button buttonVendor;
        private System.Windows.Forms.Button buttonSalesJournal;
        private System.Windows.Forms.Button buttonCustomer;
        private System.Windows.Forms.ListView listViewFeedback;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Message;
        private System.Windows.Forms.GroupBox groupBoxFeedback;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button buttonEnable;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelCounter;
        private System.Windows.Forms.Button buttonAOMPurchaseDeliveries;
        private System.Windows.Forms.Button buttonAOMProducts;
        private System.Windows.Forms.GroupBox groupBoxManual;
        private System.Windows.Forms.GroupBox groupBox2Automatic;
        private System.Windows.Forms.Label labelInterfaces;
        private System.Windows.Forms.CheckedListBox checkedListBoxInterfaces;
        private System.Windows.Forms.Label labelCompanies;
        private System.Windows.Forms.CheckedListBox checkedListBoxCompanies;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSelectCompany;
        private System.Windows.Forms.Button buttonPurchaseInvoice;
    }
}

