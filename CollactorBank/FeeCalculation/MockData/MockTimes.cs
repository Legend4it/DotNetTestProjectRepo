using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeCalculation.MockData
{
    public static class MockTimes
    {
        public static DateTime[] GetTimes()
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

            return new DateTime[] {firstPass,secondPass,therdPass,forthPass,firstPass };
        }
    }
}
