using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeeCalculation
{
    public static class Fee
    {

        public static int GetFee(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 9;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 16;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 22;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 16;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 9;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 16;
            else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 22;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 16;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 9;
            else return 0;

        }

    }
}
