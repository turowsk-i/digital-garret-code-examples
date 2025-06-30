using System.Collections.Generic;

namespace Common
{
    public interface IInvoiceParser
    {
        IEnumerable<string> ParseInvoice(IEnumerable<string> input);
    }
}