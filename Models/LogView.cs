using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BankAccounts.Models
{
    public class LogView
    {
        [Required]
        [Display(Name="Email: ")]
        public string Email{get;set;}

        [Required]
        [Display(Name="Password: ")]
        public string Password{get;set;}
    }
}