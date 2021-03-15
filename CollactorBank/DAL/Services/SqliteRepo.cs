using System;

namespace DAL.Services
{
    public class SqliteRepo : IMainRepository
    {
        public void Insert(object passTimeObject) {
            //using (var db = new PassTimeContext())
            //{
            //    db.Add(new PassTimeModel() { RegNr = "xyz123", PassTime = DateTime.Now.ToShortTimeString() });
            //    db.SaveChanges();
            //}
        }
        public void Update(object passTimeObject) { throw new NotImplementedException(); }
        public PassTimeModel GetTime(string regNr) { throw new NotImplementedException(); }
        public void Delete(string regNr) { throw new NotImplementedException(); }
    }
}
