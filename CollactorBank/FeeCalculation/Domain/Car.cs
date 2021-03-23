using FeeCalculation.Domain;
using System;

namespace TollFeeCalculator.Domain
{
    public class Car : VehicleBase
    {

        public Car(string regNr)
        {
            RegNr = regNr;
        }

        public override String GetVehicleType()
        {
            return "Car";
        }
    }
}