using System;

namespace DAL
{
    public class Car : VehicleBase
    {
        public override String GetVehicleType()
        {
            return "Car";
        }
    }
}