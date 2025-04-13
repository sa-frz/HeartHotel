using Microsoft.EntityFrameworkCore;

namespace Firoozi.Helper
{
    public class Helper
    {
        public static int GenerateCode()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(100000, 999999); // creates a 6 digit random no.
            return myRandomNo;
        }

        public static int getUserId(HttpContext httpContext)
        {
            int UserId = 0;
            try
            {
                UserId = Convert.ToInt32(httpContext.Session.GetString("userid"));
            }
            catch { }

            if (UserId <= 0 || UserId > 10)
            {
                UserId = 0;
            }

            return UserId;
        }
    }
}