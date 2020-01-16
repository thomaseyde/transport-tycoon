using TransportTycoon.Time;
using TransportTycoon.WithSumType.Maps;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Unloading : Truck
    {
        public Container Container { get; }
        public Moment DeliveryTime => Container.DeliveryTime;

        public Truck Move()
        {
            port.Replenish(Container);
            
            var arrivalTime = Timetable.ArrivalTime(
                Container.Destination, 
                factory.Location);
            
            return new Returning(arrivalTime, factory, port);
        }

        public Unloading(Container container, Port port, Factory factory)
        {
            Container = container;
            this.port = port;
            this.factory = factory;
        }

        readonly Port port;
        readonly Factory factory;
    }
}