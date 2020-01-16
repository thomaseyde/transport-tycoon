using TransportTycoon.Time;

namespace TransportTycoon.WithSumType.Stores
{
    class Container
    {
        public Container(Moment deliveryTime, Location destination)
        {
            DeliveryTime = deliveryTime;
            Destination = destination;
        }

        public Moment DeliveryTime { get; }
        public Location Destination { get; }

        public Container With(Moment deliveryTime)
        {
            return new Container(deliveryTime, Destination);
        }
    }
}