using TransportTycoon.WithSumType.Maps;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Loading : Truck
    {
        readonly Factory factory;
        readonly Port port;

        public Loading(Factory factory, Port port)
        {
            this.factory = factory;
            this.port = port;
        }

        public Truck Move()
        {
            return factory.Load(Any, None);
        }

        Truck Any(Container container)
        {
            var arrivalTime = Timetable.ArrivalTime(
                factory.Location,
                container.Destination);

            return new Delivering(
                container.With(arrivalTime),
                factory,
                port);
        }

        Loading None() => this;
    }
}