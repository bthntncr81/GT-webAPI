using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Utilities.Consts
{
    public class ValidationMessages
    {
        //User Validation Not Empty Messages
        public const string Name_Not_Empty = "Name cannot be empty.";
        public const string Email_Not_Empty = "E-Mail cannot be empty.";
        public const string Username_Not_Empty = "Username cannot be empty.";
        public const string Password_Not_Empty = "Password cannot be empty.";
        public const string Phone_Not_Empty = "Phone cannot be empty.";
        public const string Address_Not_Empty = "Address cannot be empty.";
        public const string Surname_Not_Empty = "Surname cannot be empty.";
        
        
        public const string Email_Invalid = "Please enter a valid e-mail address.";
        public const string Phone_Number_Invalid = "Please enter a valid phone number.";

        public const string Not_Empty = "{PropertyName} cannot be empty";
        public const string Max_Length = "Maximum limit {MaxLength} reached. You entered: {TotalLength}";

        public const string Min_Length = "Minimum limit reached {MinLength}. You entered: {TotalLength}";

        public const string Password_Not_Match = "Passwords not match.";



        public const string PasswordConfirm_Not_Empty = "Password confirm cannot be empty.";
        public const string Unauthorized = "Unauthorized.";

        public const string Invalid = "Entered value invalid for {{PropertyName}}.";
        public const string Not_Found = "{PropertyName} not found.";
        public const string Null = "{PropertyName} cannot be null.";
        public const string Delete_Success = "{PropertyName} deleted successfully.";

        public static string Build(string propertyName, string messageTemplate)
        {
            return messageTemplate.Replace("{PropertyName}", propertyName);
        }


    }
}
