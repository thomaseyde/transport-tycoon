using TransportTycoon.Time;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Returning : Truck
    {
        public Moment ArrivalTime { get; }

        public Truck Move()
        {
            return new Loading(factory, port);
        }

        public Returning(Moment arrivalTime, Factory factory, Port port)
        {
            ArrivalTime = arrivalTime;
            this.factory = factory;
            this.port = port;
        }

        readonly Factory factory;
        readonly Port port;
    }
}