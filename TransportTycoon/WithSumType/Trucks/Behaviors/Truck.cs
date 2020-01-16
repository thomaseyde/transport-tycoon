using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    abstract class Truck
    {
        public static Truck Create(Factory factory, Port port)
        {
            return new Loading(factory, port);
        }
    }
}