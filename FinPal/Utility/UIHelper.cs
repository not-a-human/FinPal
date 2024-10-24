using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Utility
{
   public static class UIHelper
    {

        public static string GetFormValidClass(int isValid, int display)
        {
            switch(display)
            {
                case 1 :
                    return isValid switch
                    {
                        1 => "d-block",
                        _ => ""
                    };
                default:
                    return isValid switch
                    {
                        1 => "is-invalid",
                        2 => "is-valid",
                        _ => ""
                    };
            };
            
        }

        // 0 = none, 1 = not valid, 2 valid
        public static int IsZeroEmpty(int isValid)
        {
            if (isValid == 0)
                return 1;
            else
                return 2;
        }
        public static int IsZeroEmpty(decimal isValid)
        {
            if (isValid == 0)
                return 1;
            else
                return 2;
        }

        public static int IsZeroEmpty(string isValid)
        {
            if (string.IsNullOrEmpty(isValid))
                return 1;
            else
                return 2;
        }
    }
}
