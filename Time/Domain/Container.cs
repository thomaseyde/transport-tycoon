using TransportTycoon.Optional;

namespace TransportTycoon.Domain
{
    public class Container
    {
        public Destination Destination { get; }
        public Option<Time> TravelTime { get; set; }

        public Container(Destination destination)
        {
            Destination = destination;
            TravelTime = Option.None;
        }

        public Container With(Option<Time> travelTime)
        {
            TravelTime = travelTime;
            var with = this;
            return with;
        }

        public Location LocationAfter(Location origin)
        {
            return origin.NextLocationTowards(Destination.Location);
        }
    }
}