using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib
{
    public class Quantity
    {
       public int QuantityId { get; set; }
       public Unit Unit { get; set; }
       public decimal Amount { get; set; }
    }
}
