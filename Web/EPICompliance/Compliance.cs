using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.Profile;

namespace EPICompliance
{
    public class Compliance : IDisposable
    {
        public enum OperationType
        {
            PageView,
            DataAdd,
            DataDelete,
            DataUpdate
        }

        public enum enumPasswordValidation 
        { 
            PasswordLength,
            NoUpperCase,
            NoLowerCase,
            NoNumber,
            NoSpecialCharacter,
            OK
        }

        public static Boolean unlockAccount(string username)
        {
            MembershipUser mu = null;

            mu = Membership.GetUser(username);
            mu.UnlockUser();

            mu.LastActivityDate = DateTime.UtcNow.AddMinutes(-(Membership.UserIsOnlineTimeWindow + 1));
            Membership.UpdateUser(mu);

            return true;

        }

        public static Boolean logout(string username)
        {
            MembershipUser user = null;


            ProfileManager.DeleteProfile(username);
            HttpContext.Current.Profile.Save();
            user = Membership.GetUser(username, false);


            System.Web.Security.FormsAuthentication.SignOut();

            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();

            user.LastActivityDate = DateTime.UtcNow.AddMinutes(-(Membership.UserIsOnlineTimeWindow + 1));
            Membership.UpdateUser(user);


            return true;
        }

        public static bool passwordExpired(string username)
        {
            MembershipUser User = null;
            try
            {
                User = Membership.GetUser(username, true);
                if (User.LastPasswordChangedDate < DateTime.Now.AddDays(ConfigurationHelper.PasswordExpiryPeriod))
                    return true;

                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (User != null)
                    User = null;
            }
        }

        public static enumPasswordValidation validatePassword(string sPassword)
        { 
            char[] SpecialChars = @"!`~@#$%^&*()_+\\|{}[]:;'?/>.<,".ToCharArray();

            //1- Check if 
            if (sPassword.Trim().Length < 8)
            {
                return enumPasswordValidation.PasswordLength;
            }

            // 2- Check if it has upper case
            if(!sPassword.Any(c => char.IsUpper(c)))
            {
                return enumPasswordValidation.NoUpperCase;
            }

            // 3- Check if it has lower case
            if (!sPassword.Any(c => char.IsLower(c)))
            {
                return enumPasswordValidation.NoLowerCase;
            }

            // 4- Check if it has lower case
            if (!sPassword.Any(c => char.IsNumber(c)))
            {
                return enumPasswordValidation.NoNumber;
            }

            // 5- Check if it has upper case
            int indexOf = sPassword.IndexOfAny(SpecialChars);
            if (indexOf == -1)
            {
                return enumPasswordValidation.NoSpecialCharacter;
            }

            return enumPasswordValidation.OK;
        }

        public static int noDaysToPasswordExpiry(string username)
        {
            MembershipUser User = null;

            User = Membership.GetUser(username, true);
            return (User.LastPasswordChangedDate.Subtract(DateTime.Now.AddDays(ConfigurationHelper.PasswordExpiryPeriod))).Days;

        }

        public static void insertAuditTrail(string sUserName, string sPage, OperationType sTransaction, string sMessage, string sIPAddress)
        {
            using (DBHelper db = new DBHelper(sUserName))
            {
                db.insertAuditTrail(sUserName, sPage, sTransaction.ToString(), sMessage, sIPAddress);
            }
        }

        public static void insertLoginHistory(string sUserName, string sType, string sIPAddress)
        {
            using (DBHelper db = new DBHelper(sUserName))
            {
                db.insertLoginHistory(sUserName, sType, sIPAddress);
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
