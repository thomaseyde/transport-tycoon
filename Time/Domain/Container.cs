namespace TransportTycoon.Domain
{
    public class Container
    {
        State state;

        public Container(Destination destination)
        {
            state = new Produced(destination);
        }

        public Time TravelTime => state.TravelTime;

        public Container With(Time deliveryTime)
        {
            state = state.With(deliveryTime);
            return this;
        }

        public Location LocationAfter(Location origin)
        {
            return state.LocationAfter(origin);
        }

        abstract class State
        {
            public virtual Time TravelTime => Time.Zero;

            readonly Destination destination;

            protected State(Destination destination)
            {
                this.destination = destination;
            }

            public Location LocationAfter(Location origin)
            {
                return origin.NextLocationTowards(destination.Location);
            }

            public State With(Time time)
            {
                return With(destination, time);
            }

            protected abstract State With(Destination destination, Time arrivalTime);
        }

        class Produced : State
        {
            public Produced(Destination destination) : base(destination) {
            }

            protected override State With(Destination destination, Time arrivalTime)
            {
                return new Transporting(destination, arrivalTime);
            }
        }

        class Transporting : State
        {
            readonly Time currentTime;

            public Transporting(Destination destination, Time currentTime) : base(destination)
            {
                this.currentTime = currentTime;
            }

            protected override State With(Destination destination, Time arrivalTime)
            {
                return new Transporting(destination, arrivalTime);
            }

            public override Time TravelTime => currentTime;
        }
    }
}