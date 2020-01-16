using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Delivering : IBehaviour
    {
        public Delivering(Container container)
        {
            Container = container;
        }

        public Container Container { get; }

        public IBehaviour TransitionFrom(Truck current)
        {
            return current.Delivering(this);
        }
    }
}