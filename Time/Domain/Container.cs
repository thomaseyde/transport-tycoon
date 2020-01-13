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

        private Container(Destination destination, Time travelTime)
        {
            Destination = destination;
            TravelTime = travelTime;
        }

        public Container With(Option<Time> travelTime)
        {
            var current = TravelTime.Reduce(Time.Zero);
            var added = travelTime.Reduce(Time.Zero);

            return new Container(Destination, current.Add(added));
        }

        public Location LocationAfter(Location origin)
        {
            return origin.NextLocationTowards(Destination.Location);
        }
    }
}