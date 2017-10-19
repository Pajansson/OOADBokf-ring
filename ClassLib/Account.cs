using System;
using System.Collections.Generic;

namespace ClassLib
{
    public class Account
    {
        public string AccountCode { get; set; }
        public int AccountId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public bool IsSummary { get; set; }
        public List<Account> Children { get; set; }

    }
}
