using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace BankAccounts.Models
{
    public class User
    {
        [Key]
        public int ID {get;set;}
        [Required]
        [MinLength(3)]
        [Display(Name="First Name: ")]
        public string FirstName{get;set;}
        [Required]
        [MinLength(3)]
        [Display(Name="Last Name: ")]
        public string LastName{get;set;}
        [Required]
        [EmailAddress]
        [Display(Name="Email Address: ")]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}
        public double Balance {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password{get;set;}

        
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords must Match")]
        public string ConfirmPassword{get;set;}

        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        public List<Transaction> TransactionHistory{get;set;}
    }
}