using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Unloading : IBehaviour
    {
        public Unloading(Container container)
        {
            Container = container;
        }

        public Container Container { get; }

        public IBehaviour TransitionFrom(Truck current)
        {
            return current.Unloading(this);
        }
    }
}