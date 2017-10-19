using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClassLib;
using Microsoft.AspNetCore.Mvc;
using TransAmBank.Data;
using TransAmBank.Models;

namespace TransAmBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var accountCost = new Account { Name = "Kostnader", IsSummary = true, AccountCode = "K" };
            var accountDebt = new Account { Name = "Skulder", IsSummary = true, AccountCode = "S" };
            var accountIncome = new Account { Name = "Intäkter", IsSummary = true, AccountCode = "I" };
            var accountAsset = new Account { Name = "Tillgångar", IsSummary = true, AccountCode = "T" };

            _context.Accounts.AddRange(accountCost, accountAsset, accountDebt, accountIncome);
            _context.SaveChanges();

            using (var s = new StreamReader(@"C:\OOAD\MATTIAS0_SIE4 2015-09-01 - 2016-08-31.SE.txt"))
            {

                var unit = new Unit { Name = "SEK" };

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
                            var newAccount = new Account
                            {
                                AccountCode = match.Groups[1].Value,
                                Name = match.Groups[2].Value,
                                ParentId = accountAsset.AccountId,
                                IsSummary = false
                            };
                            _context.Accounts.Add(newAccount);
                            _context.SaveChanges();
                        }
                        if (match2.Groups[2].Value == "I")
                        {
                            var newAccount = new Account
                            {
                                AccountCode = match.Groups[1].Value,
                                Name = match.Groups[2].Value,
                                ParentId = accountIncome.AccountId,
                                IsSummary = false
                            };
                            _context.Accounts.Add(newAccount);
                            _context.SaveChanges();
                        }
                        if (match2.Groups[2].Value == "K")
                        {
                            var newAccount = new Account
                            {
                                AccountCode = match.Groups[1].Value,
                                Name = match.Groups[2].Value,
                                ParentId = accountCost.AccountId,
                                IsSummary = false
                            };
                            _context.Accounts.Add(newAccount);
                            _context.SaveChanges();
                        }
                        if (match2.Groups[2].Value == "S")
                        {
                            var newAccount = new Account
                            {
                                AccountCode = match.Groups[1].Value,
                                Name = match.Groups[2].Value,
                                ParentId = accountDebt.AccountId,
                                IsSummary = false
                            };
                            _context.Accounts.Add(newAccount);
                            _context.SaveChanges();
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



                                var quantity = new Quantity { Amount = decimal.Parse(value, CultureInfo.InvariantCulture), Unit = unit };
                                var entry = new Entry { Quantity = quantity, AccountId = Int32.Parse(hit.Groups[1].Value) };
                                trans.Entries.Add(entry);

                            }
                        }
                        _context.Add(trans);
                        _context.SaveChanges();
                    }

                }
            }



            


            return View(_context.Accounts.ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
