using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int ID {get;set;}
        [Required]
        [Display(Name="Transaction Amount: ")]
        [DataType(DataType.Currency)]
        public double Amount {get;set;}



        public int UserID{get;set;}
        public User User{get;set;}
        public DateTime CreatedAt{get;set;}

        
    }
}