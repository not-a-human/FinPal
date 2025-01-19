using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace FinPal.Utility
{
   public class UIHelper
    {

        public static string GetFormValidClass(int isValid, int display)
        {
            switch(display)
            {
                case 1 :
                    return isValid switch
                    {
                        0 => "",
                        2 => "",
                        _ => "d-block"
                    };
                default:
                    return isValid switch
                    {
                        0 => "",
                        2 => "is-valid",
                        _ => "is-invalid"
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

        public static string TruncateString(string input, int max = 20)
        {
            return input.Length > max ? input.Substring(0, max) + "..." : input;
        }

        // Method to open a URL
        public static async Task OpenExternalUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
            {
                await Launcher.OpenAsync(uriResult);
            }
        }

        public static string GetMonthString(int month)
        {
            return new DateTime(DateTime.Now.Year, month, 1).ToString("MMM");
        }

    }
}
