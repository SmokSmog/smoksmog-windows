using GalaSoft.MvvmLight.Messaging;

namespace SmokSmog.Messenger
{
    public class StationChangeMessage : GenericMessage<Model.Station>
    {
        public StationChangeMessage(Model.Station staion) : base(staion)
        {
        }
    }
}