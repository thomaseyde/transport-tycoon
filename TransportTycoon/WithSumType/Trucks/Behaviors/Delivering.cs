using TransportTycoon.Time;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Delivering : Truck
    {
        public Container Container { get; }
        public Moment DeliveryTime => Container.DeliveryTime;

        public Truck Move()
        {
            return new Unloading(Container, port, factory);
        }

        public Delivering(Container container, Factory factory, Port port)
        {
            Container = container;
            this.port = port;
            this.factory = factory;
        }

        readonly Port port;
        readonly Factory factory;
    }
}