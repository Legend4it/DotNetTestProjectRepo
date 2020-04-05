using System;
using System.Threading.Tasks;
using BotekAssignmentNet;

namespace Botek
{
    public class HardwareHandlerTask : ITaskBase
    {
        public int? TaskId => GetTaskId();
        public DataObject Result { get; set; }
        private int _pId;
        public HardwareHandlerTask()
        {

            var task = Task.Factory.StartNew(()=> { EventInvoker(); });

            _pId = task.Id;
        }
        private void EventInvoker()
        {
            HardwareHandler.OnPositionReceived += HardwareHandler_OnPositionReceived;
            HardwareHandler.OnCollectionReceived += HardwareHandler_OnCollectionReceived;
            HardwareHandler.OnDeviationReceived += HardwareHandler_OnDeviationReceived;
        }
        private void EventUnSubscriber()
        {
            HardwareHandler.OnPositionReceived -= HardwareHandler_OnPositionReceived;
            HardwareHandler.OnCollectionReceived -= HardwareHandler_OnCollectionReceived;
            HardwareHandler.OnDeviationReceived -= HardwareHandler_OnDeviationReceived;
        }
        private void HardwareHandler_OnDeviationReceived(string identifier, double? latitude, double? longitude, string deviation)
        {
            Console.WriteLine($"OnDeviationReceived:Identifier:{identifier},Iatitude:{latitude},Iongitude:{longitude},Deviation:{deviation}");
        }

        private void HardwareHandler_OnCollectionReceived(string identifier, double? latitude, double? longitude, double? weight, string errorCode)
        {
            Console.WriteLine($"OnCollectionReceived:Identifier:{identifier},Iatitude:{latitude},Iongitude:{longitude},Weight:{weight},ErrorCode:{errorCode}");
        }

        private void HardwareHandler_OnPositionReceived(double? latitude, double? longitude)
        {
            Console.WriteLine($"PositionReceived:Iatitude:{latitude},Iongitude:{longitude}");
        }

        //Get Task Id to manage TaskMAnager...!
        public int? GetTaskId()
        {
            return _pId;
        }

        public void Dispose()
        {
            //Clean Resources
            EventUnSubscriber();
        }
    }
    public class DataObject
    {
        //Map to DataObject To show data in specific form

    }

}
