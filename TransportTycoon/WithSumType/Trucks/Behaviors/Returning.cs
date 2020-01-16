using TransportTycoon.Time;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Returning : IBehaviour
    {
        public Returning(Moment arrivalTime)
        {
            ArrivalTime = arrivalTime;
        }

        public Location Origin { get; set; }
        public Moment ArrivalTime { get; }

        public IBehaviour TransitionFrom(Truck current)
        {
            return current.Returning(this);
        }
    }
}