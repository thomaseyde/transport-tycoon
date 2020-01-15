namespace TransportTycoon.Domain
{
    public class Container
    {
        readonly Destination destination;
        public Time TravelTime { get; set; }

        public Container(Destination destination)
        {
            this.destination = destination;
            TravelTime = Time.Zero;
        }

        public Container With(Time currentTime)
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