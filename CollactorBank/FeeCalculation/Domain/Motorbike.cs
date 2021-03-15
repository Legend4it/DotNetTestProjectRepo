using FeeCalculation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Domain
{
    public class Motorbike : VehicleBase
    {
        public Motorbike(string regnr)
        {
            this.RegNr = regnr;
        }
        public override string GetVehicleType()
        {
            return "Motorbike";
        }
    }
}
