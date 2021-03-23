
namespace DAL
{
    public abstract class VehicleBase : IVehicle
    {
        public string RegNr { get; set; }

        public virtual string GetVehicleType()
        {
            return "No Vehicle type";
        }
    }
}
