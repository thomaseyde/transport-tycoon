using TransportTycoon.Time;

namespace TransportTycoon.Domain
{
    public abstract class Transporter
    {
        State state;

        protected Transporter(Location origin)
        {
            state = new Loading(origin);
        }

        public bool Carries(Container container) => state.Carries(container);
        public void Move(Moment currentTime) => state = state.Move(currentTime);
        protected void Load(Storage storage, Moment currentTime) => state = state.Load(storage, currentTime);
        protected void Unload(Storage storage, Moment currentTime) => state = state.Unload(storage, currentTime);

        abstract class State
        {
            public virtual State Move(Moment currentTime) => this;
            public virtual State Unload(Storage storage, Moment currentTime) => this;
            public virtual bool Carries(Container other) => false;
            public virtual State Load(Storage storage, Moment currentTime) => this;
        }

        class Loading : State
        {
            readonly Location origin;

            public Loading(Location origin)
            {
                this.origin = origin;
            }

            public override State Load(Storage storage, Moment currentTime)
            {
                State next = this;

                storage.Deplete(container =>
                {
                    var destination = container.LocationAfter(origin);
                    var duration = Moment.Between(origin, destination);
                    var deliveryTime = currentTime.Add(duration);
                    
                    next = new Delivering(container, origin, duration, deliveryTime);
                });

                return next;
            }
        }

        class Delivering : State
        {
            readonly Container container;
            readonly Duration duration;
            readonly Location origin;
            readonly Moment arrivalTime;

            public Delivering(
                Container container,
                Location origin,
                Duration duration,
                Moment arrivalTime)
            {
                this.container = container;
                this.duration = duration;
                this.origin = origin;
                this.arrivalTime = arrivalTime;
            }

            public override State Move(Moment currentTime)
            {
                if (currentTime == arrivalTime)
                {
                    return new Unloading(container, duration, origin);
                }

                return this;
            }

            public override bool Carries(Container other) => container == other;
        }

        class Unloading : State
        {
            readonly Container container;
            readonly Duration duration;
            readonly Location origin;

            public Unloading(
                Container container,
                Duration duration,
                Location origin)
            {
                this.container = container;
                this.duration = duration;
                this.origin = origin;
            }

            public override State Unload(Storage storage, Moment currentTime)
            {
                var destination = container.LocationAfter(origin);

                if (destination != storage.Location) return this;

                storage.Replenish(container.With(currentTime));
                var arrivalTime = currentTime.Add(duration);
                return new Returning(arrivalTime, origin);

            }
        }

        class Returning : State
        {
            readonly Moment arrivalTime;
            readonly Location origin;

            public Returning(Moment arrivalTime, Location origin)
            {
                this.arrivalTime = arrivalTime;
                this.origin = origin;
            }

            public override State Move(Moment currentTime)
            {
                if (arrivalTime == currentTime)
                {
                    return new Loading(origin);
                }

                return this;
            }
        }
    }
}