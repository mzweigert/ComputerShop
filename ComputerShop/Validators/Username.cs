using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ComputerShop.Validators
{
    public class Username : ValidationAttribute
    {
        public Username() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorMessage;
            string username;
            Regex RgxUrl = new Regex("[^A-Za-z0-9_]");

     
            if (value == null)
                return ValidationResult.Success;

            if (validationContext.DisplayName == null)
                errorMessage = "Username is incorrect";
            else
                errorMessage = FormatErrorMessage(validationContext.DisplayName);

            if (value is string)
                username = value.ToString();
            else
                return new ValidationResult("Type of username field is not a string type");


            if(!RgxUrl.IsMatch(username))
                return ValidationResult.Success;
            else
                return new ValidationResult("Username can't contains special chars and whitespace");


        }
    }
}