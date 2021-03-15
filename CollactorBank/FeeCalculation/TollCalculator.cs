using FeeCalculation;
using Nager.Date;
using System;
using System.Globalization;
using TollFeeCalculator;
using TollFeeCalculator.Domain;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            TimeSpan diffInMillies = date.Subtract(intervalStart);
            double minutes = Math.Round(diffInMillies.TotalMinutes);
            totalFee += nextFee;
        }

        if (totalFee > 60) totalFee = 60;

        return totalFee;
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.GetVehicleType());

    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        return Fee.GetFee(date);
    }

    private bool IsTollFreeDate(DateTime date)
    {

        if (IsWeekEnd(date)) return true;

        if (IsFeeFreeDay(date)) return true;

        return false;
    }

    private bool IsWeekEnd(DateTime date)
    {
        return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
    }

    private bool IsFeeFreeDay(DateTime date)
    {
        
        if (DateSystem.IsPublicHoliday(date, CountryCode.SE)) { return true; }

        return false;

    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}