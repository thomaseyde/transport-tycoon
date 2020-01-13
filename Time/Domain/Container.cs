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

        public Container With(Time added)
        {
            state = state.With(added);
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
                return With(time, destination);
            }

            protected abstract State With(Time time, Destination destination);
        }

        class Produced : State
        {
            public Produced(Destination destination) : base(destination) {
            }

            protected override State With(Time time, Destination destination)
            {
                return new Transporting(destination, time);
            }
        }

        class Transporting : State
        {
            readonly Time currentTime;

            public Transporting(Destination destination, Time currentTime) : base(destination)
            {
                this.currentTime = currentTime;
            }

            protected override State With(Time time, Destination destination)
            {
                return new Transporting(destination, currentTime.Add(time));
            }

            public override Time TravelTime => currentTime;
        }
    }
}