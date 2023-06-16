using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.CommonHelper
{
    public class RandomIdGenerator
    {
        private static readonly Random _randomObj = new Random();

        public static string GenerateOrderNumberId()
        {
            var loginID = "";

            // for uppercase letters
            for (int i = 0; i < 2; i++)
            {
                loginID += (char)_randomObj.Next(65, 90);
            }

            // for one lowercase letter
            for (int i = 0; i < 1; i++)
            {
                loginID += (char)('a' + _randomObj.Next(0, 26));
            }

            loginID = "#" + loginID + "-" + _randomObj.Next(1111, 9999) + DateTime.Today.Second + DateTime.Now.Millisecond;
            return loginID;
        }
    }
}
