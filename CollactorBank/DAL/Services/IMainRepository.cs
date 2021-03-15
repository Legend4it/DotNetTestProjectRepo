
namespace DAL.Services
{
    public interface IMainRepository
    {
        void Delete(string regNr);
        PassTimeModel GetTime(string regNr);
        void Insert(object passTimeObject);
        void Update(object passTimeObject);
    }
}