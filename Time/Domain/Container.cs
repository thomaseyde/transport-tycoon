namespace TransportTycoon.Domain
{
    public class Container
    {
        readonly Destination destination;
        public Moment TravelTime { get; set; }

        public Container(Destination destination)
        {
            this.destination = destination;
            TravelTime = Moment.Zero;
        }

        public Container With(Moment currentTime)
        {
            TravelTime = currentTime;
            return this;
        }

        public Location LocationAfter(Location origin)
        {
            return origin.NextLocationTowards(destination.Location);
        }
    }
}