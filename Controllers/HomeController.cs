using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using BankAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private BankAccountContext dbContext;
        public HomeController(BankAccountContext context)
        {
            dbContext = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult Register(User newUSer)
        {
            if(dbContext.Users.Any(u=>u.Email == newUSer.Email))
            {
                ModelState.AddModelError("Email", "Too slow! taken!");
            }

            if(ModelState.IsValid)
            {
                PasswordHasher<User> HAsher = new PasswordHasher<User>();
                newUSer.Password = HAsher.HashPassword(newUSer, newUSer.Password);
                HttpContext.Session.SetInt32("ID", newUSer.ID);
                dbContext.Add(newUSer);
                dbContext.SaveChanges();

                return Redirect($"account/{newUSer.ID}");
            }
            return View("Index");
        }
        [HttpGet("login")]
        public IActionResult LogView()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LogView logUser)
        {
            if(ModelState.IsValid)
            {
                User dbUser = dbContext.Users.FirstOrDefault(u=>u.Email == logUser.Email);
                if(dbUser != null)
                {
                    var hasher = new PasswordHasher<LogView>();
                    var result = hasher.VerifyHashedPassword(logUser, dbUser.Password, logUser.Password);
                    if(result == 0)
                    {
                        ModelState.AddModelError("Email", "Email or password incorrect");
                        return View("LogView");
                    }
                    HttpContext.Session.SetInt32("ID", dbUser.ID);
                    System.Console.WriteLine("########################################3");
                    System.Console.WriteLine(HttpContext.Session.GetInt32("ID"));
                    return Redirect($"account/{dbUser.ID}");
                }
                ModelState.AddModelError("Email", "Can't find that email Address");
                
            }
            return View("LogView");
        }

        [HttpGet("account/{userID}")]
        public IActionResult Account(int userID)
        {
            if(HttpContext.Session.GetInt32("ID") != userID)
            {
                return RedirectToAction("Index");
            }
            string UserName = dbContext.Users.FirstOrDefault(u=>u.ID == userID).FirstName;
            HttpContext.Session.SetString("UserName", UserName);
            List<Transaction> transactions = dbContext.Transactions
            .Where(t=>t.UserID == userID).ToList();
            ViewBag.Transactions = transactions;
            User SessionUser = dbContext.Users.FirstOrDefault(u=>u.ID == userID);
            double Balance = SessionUser.Balance;
            Balance = Math.Round(Balance, 2);
            HttpContext.Session.SetInt32("Balance", (int)Balance);
            return View();
        }

        [HttpPost("balance")]
        public IActionResult DepositWithdraw(Transaction newTrans)
        {
            int? sessionID = HttpContext.Session.GetInt32("ID");
            sessionID = (int)sessionID;
            User SessionUser = dbContext.Users.FirstOrDefault(u=>u.ID == sessionID);
            
            List<Transaction> transactions = dbContext.Transactions
            .Where(t=>t.UserID == sessionID).OrderByDescending(t=>t.CreatedAt).ToList();
            double Balance = SessionUser.Balance;
            Balance = Math.Round(Balance, 2);
            if(Balance + newTrans.Amount < 0 ){
                ModelState.AddModelError("Amount", "Insufficient Balance");
                TempData["AlertMessage"] = "Insufficient Balance";
            }
            
            if(ModelState.IsValid)
            {
                newTrans.Amount = Math.Round(newTrans.Amount, 2);
                SessionUser.Balance += newTrans.Amount;
                newTrans.UserID = (int) HttpContext.Session.GetInt32("ID");
                newTrans.CreatedAt = DateTime.Now;
                newTrans.User = dbContext.Users.FirstOrDefault(u=>u.ID == (int) HttpContext.Session.GetInt32("ID"));
                dbContext.Add(newTrans);
                dbContext.SaveChanges();
                return Redirect($"account/{sessionID}");

            }
            return Redirect($"account/{sessionID}");
        }
    }
}
