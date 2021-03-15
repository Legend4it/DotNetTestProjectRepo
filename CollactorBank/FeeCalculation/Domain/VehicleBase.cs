using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Domain;

namespace FeeCalculation.Domain
{
    public abstract class VehicleBase : IVehicle
    {
        public string RegNr { get; protected set; }

        public virtual string GetVehicleType()
        {
            return "No Vehicle type";
        }
    }
}
