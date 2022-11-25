using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    static class CommonFunctions
    {

        public static List<string> DivideOnSubstring(string s, int chunkSize)
        {
            if (s.Length < chunkSize)
            {
                List<string> ans = new List<string>();
                ans.Add(s);
                return ans;
            }
            var result = (from Match m in Regex.Matches(s, @".{1," + chunkSize + "}")
                          select m.Value).ToList();


            return result;
        }

     
    }
}
