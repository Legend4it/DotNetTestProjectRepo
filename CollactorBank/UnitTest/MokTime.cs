using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public static class MokTime
    {
        public static IEnumerable<DateTime> GetPassTime()
        {
            DateTime firstPass = DateTime.ParseExact("2013-01-08 07:00:20", "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime secondPass = DateTime.ParseExact("2013-01-08 08:30:10", "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

            DateTime therdPass = DateTime.ParseExact("2013-01-08 15:29:20", "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

            DateTime forthPass = DateTime.ParseExact("2013-01-08 18:29:00", "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

            DateTime fithPass = DateTime.ParseExact("2013-01-08 20:20:30", "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);
            DateTime sexthPass = DateTime.ParseExact("2013-01-08 21:20:30", "yyyy-MM-dd HH:mm:ss",
                                                   System.Globalization.CultureInfo.InvariantCulture);

            return new DateTime[] { firstPass, secondPass, therdPass, forthPass, fithPass, sexthPass };
        }
    }
}
