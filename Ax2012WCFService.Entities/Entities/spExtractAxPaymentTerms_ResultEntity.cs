using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ax2012WCFService.Entities
{
    public partial class spExtractAxPaymentTerms_ResultEntity
    {
        public string AccountNum { get; set; }
        public string PaymTermID { get; set; }
        public Nullable<int> PAYMMETHOD { get; set; }
        public Nullable<int> NUMOFMONTHS { get; set; }
        public Nullable<int> NUMOFDAYS { get; set; }
        public string PAYMMODE { get; set; }
    }
}
