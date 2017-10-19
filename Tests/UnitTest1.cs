using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ClassLib;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var s = new StreamReader(@"C:\OOAD\MATTIAS0_SIE4 2015-09-01 - 2016-08-31.SE.txt"))
            {
                var tillgangar = new Dictionary<string, string>();
                var skulder = new Dictionary<string, string>();
                var intakter = new Dictionary<string, string>();
                var kostnader = new Dictionary<string, string>();
                var transaktioner = new List<Transaction>();
                var unit = new Unit {Name = "SEK"};
                

                string line;
                while ((line = s.ReadLine()) != null)
                {
                   

                    var regEx1 = new Regex("#KONTO (\\d{4}) \"?([^\"]+)\"?");

                    var listAccounts = new List<Account>();
                    var match = regEx1.Match(line);
                    if (match.Success)
                    {
                        var regEx2 = new Regex($"#KTYP ({match.Groups[1].Value}) ([A-Z])");
                        var line2 = s.ReadLine();
                        var match2 = regEx2.Match(line2);
                        if (match2.Groups[2].Value == "T")
                        {
                            tillgangar.Add(match.Groups[1].Value, match.Groups[2].Value);
                        }
                        if (match2.Groups[2].Value == "I")
                        {
                            intakter.Add(match.Groups[1].Value, match.Groups[2].Value);
                        }
                        if (match2.Groups[2].Value == "K")
                        {
                            kostnader.Add(match.Groups[1].Value, match.Groups[2].Value);
                        }
                        if (match2.Groups[2].Value == "S")
                        {
                            skulder.Add(match.Groups[1].Value, match.Groups[2].Value);
                        }
                    }
                    var regExTrans1 = new Regex($"#VER A .*([a-zA-Z0-9]+|\".+\")");
                    if (regExTrans1.Match(line).Success)
                    {
                        var trans = new Transaction
                        {
                            TransactionName = regExTrans1.Match(line).Groups[1].Value,
                            Entries = new List<Entry>(),
                        };

                        while ((line = s.ReadLine()) != "}")
                        {
                            var entries = new List<Entry>();
                            var regExTrans2 = new Regex(@"#TRANS (\d{4}) {} (-?\d+\.\d{2})");
                            var hit = regExTrans2.Match(line);

                       

                            if (hit.Success)
                            {
                                var value = hit.Groups[2].Value.Replace("−", "-");



                                var quantity = new Quantity {Amount = decimal.Parse(value, CultureInfo.InvariantCulture), Unit = unit};
                                var entry = new Entry {Quantity = quantity, AccountId = Int32.Parse(hit.Groups[1].Value) };
                                trans.Entries.Add(entry);


                            }
                        }
                        transaktioner.Add(trans);
                    }
                    
                }
            }
        }
    }
}