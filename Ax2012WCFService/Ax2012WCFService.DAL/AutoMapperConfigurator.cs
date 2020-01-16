using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ax2012WCFService.Entities;

namespace Ax2012WCFService.DAL
{
    public class AutoMapperConfigurator
    {
        public static void Configure()
        {
            Mapper.Initialize(x => x.AddProfile<MappingProfile>());
        }

        public class MappingProfile : Profile
        {
            protected override void Configure()
            {
                base.Configure();
                CreateMaps();
            }

            /// <summary>
            /// Method to generate profile on the basis of UserMapping.
            /// </summary>
            protected void CreateMaps()
            {
                CreateMap<CustChange, CustChangeEntity>();
                CreateMap<CustChangeEntity, CustChange>(); 
                CreateMap<AddressChange, AddressChangeEntity>();
                CreateMap<AddressChangeEntity, AddressChange>();
                CreateMap<VendorChange, VendorChangeEntity>();
                CreateMap<VendorChangeEntity, VendorChange>();
                CreateMap<SalesInvoiceJournal, SalesInvoiceJournalEntity>();
                CreateMap<SalesInvoiceJournalEntity, SalesInvoiceJournal>();
                CreateMap<spExtractAxPaymentTerms_ResultEntity, spExtractAxPaymentTerms_Result>();
                CreateMap<spExtractAxPaymentTerms_Result, spExtractAxPaymentTerms_ResultEntity>();
                CreateMap<AOMPurchaseDeliveryHeader, AOMPurchaseDeliveryHeadersEntity>();
                CreateMap<AOMPurchaseDeliveryHeadersEntity, AOMPurchaseDeliveryHeader>();
                CreateMap<AOMPurchaseDeliveryLine, AOMPurchaseDeliveryLinesEntity>();
                CreateMap<AOMPurchaseDeliveryLinesEntity, AOMPurchaseDeliveryLine>();
                CreateMap<AOMProductChange, AOMProductChangeEntity>();
                CreateMap<AOMProductChangeEntity, AOMProductChange>();
                CreateMap<PurchaseInvoiceHeader, PurchaseInvoiceHeaderEntity>();
                CreateMap<PurchaseInvoiceLine,PurchaseInvoiceLineEntity>();
            }
        }

    }
}
