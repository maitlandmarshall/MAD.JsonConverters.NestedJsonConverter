using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests
{
    public class SingleObjectAsNestedPropertyModel
    {
        public List<Invoice> Invoices { get; set; }
    }

    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDescription { get; set; }
    }
}
