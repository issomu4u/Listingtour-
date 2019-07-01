using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Listningtour.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        

        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public string Password { set; get; }


        [Required(ErrorMessage = "Enter Confirm Password")]

        [Display(Name = "Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Email Verified")]
        public bool IsEmailVerified { get; set; }

        public string ActivationKey { get; set; }
    }


    public class Enterintotable
    {
        public void InsertUser(UserModel Model)
        {
            DataClasses1DataContext dba = new DataClasses1DataContext();

            User usr = new User();
            usr.Id = Model.Id;
           
            usr.Email = Model.Email;
            usr.Password = Model.Password;
            usr.IsEmailVerified = Model.IsEmailVerified;
            usr.ActivationKey = Model.ActivationKey;
            dba.Users.InsertOnSubmit(usr);
            dba.SubmitChanges();

        }
    }

    public static class Searchuser
    {

        public static User GetAuthorizedUser(UserModel Model)
        {

            using (var dba = new DataClasses1DataContext())
            {

                return dba.Users.FirstOrDefault(x => x.Email == Model.Email && x.Password == Model.Password);
            }
        }

        public static bool IsProfileExists(int userId)
        {
            using (var dba = new DataClasses1DataContext())
            {
                return dba.Userinfos.Any(x => x.UserId == userId);
            }
        }

    }



    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }
    }




}

