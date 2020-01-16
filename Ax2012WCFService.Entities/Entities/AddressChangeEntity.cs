﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class AddressChangeEntity
    {
        public int AddressChangeId { get; set; }
        public System.Guid StagingId { get; set; }
        public string AccountNum { get; set; }
        public string CompanyId { get; set; }
        public string AddressRef { get; set; }
        public string Description { get; set; }
        public string LocationRole { get; set; }
        public string BuildingCompliment { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryRegionId { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string DxReference { get; set; }
        public string InvoiceAddressRef { get; set; }
        public string InventLocation { get; set; }
        public string InventSite { get; set; }
        public Nullable<int> ServiceType { get; set; }
        public string TheirCode { get; set; }
        public string Rep1 { get; set; }
        public string TaxGroup { get; set; }
        public Nullable<int> AccountType { get; set; }
        public string TaxgroupInbound { get; set; }
        public string InventLocationInbound { get; set; }
        public string ServiceLevel { get; set; }
        public string VanRoute { get; set; }
        public string DistrictName { get; set; }
        public Nullable<int> ExtractStatus { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExtractedDate { get; set; }
        public int InsUpdDel { get; set; }
    }
}
