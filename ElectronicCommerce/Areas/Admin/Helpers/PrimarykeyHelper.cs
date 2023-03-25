using System;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Helpers
{
    public class PrimarykeyHelper
    {
        public PrimarykeyHelper()
        {
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
