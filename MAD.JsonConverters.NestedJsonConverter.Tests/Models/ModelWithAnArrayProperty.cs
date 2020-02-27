using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.JsonConverters.NestedJsonConverterNS.Tests.Models
{
    public class ModelWithAnArrayProperty
    {
        public List<Invoice> Invoices { get; set; }
    }

    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public string InvoiceDescription { get; set; }
    }
}
