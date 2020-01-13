using Optional;

namespace TransportTycoon.Domain
{
    public class Container
    {
        readonly Destination destination;

        public Option<Time> TravelTime { get; }

        public Container(Destination destination)
        {
            this.destination = destination;
            TravelTime = Option.None<Time>();
        }

        Container(Destination destination, Time travelTime)
        {
            this.destination = destination;
            TravelTime = travelTime.SomeNotNull();
        }

        public Container With(Time added)
        {
            var travelTime = TravelTime.Match(
                time => time.Add(added), 
                () => added);

            return new Container(destination, travelTime);
        }

        public Location LocationAfter(Location origin)
        {
            return origin.NextLocationTowards(destination.Location);
        }
    }
}