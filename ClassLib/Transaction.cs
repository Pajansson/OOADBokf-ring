using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace ClassLib
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransactionName { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
