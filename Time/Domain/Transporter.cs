using Value;

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
        public void Move(Time currentTime) => state = state.Move(currentTime);
        protected void Load(Storage storage, Time currentTime) => state = state.Load(storage, currentTime);
        protected void Unload(Storage storage) => state = state.Unload(storage);

        public bool AtOrigin()
        {
            return state.AtOrigin();
        }

        abstract class State
        {
            public virtual State Move(Time currentTime) => this;
            public virtual State Unload(Storage storage) => this;
            public virtual bool Carries(Container other) => false;
            public virtual State Load(Storage storage, Time currentTime) => this;
            public virtual bool AtOrigin() => false;
        }

        class Loading : State
        {
            readonly Location origin;

            public Loading(Location origin)
            {
                this.origin = origin;
            }

            public override State Load(Storage storage, Time currentTime)
            {
                State next = this;

                storage.LoadContainer(container =>
                {
                    var destination = container.LocationAfter(origin);
                    var transportationTime = Time.Between(origin, destination);
                    var deliveryTime = currentTime.Add(transportationTime);
                    
                    next = new Delivering(container, deliveryTime, origin);
                });

                return next;
            }

            public override bool AtOrigin()
            {
                return true;
            }
        }

        class Delivering : State
        {
            readonly Container container;
            readonly Time deliveryTime;
            readonly Location origin;

            public Delivering(
                Container container,
                Time deliveryTime,
                Location origin)
            {
                this.container = container;
                this.deliveryTime = deliveryTime;
                this.origin = origin;
            }

            public override State Move(Time currentTime)
            {
                if (currentTime == deliveryTime)
                {
                    return new Unloading(container, deliveryTime, origin);
                }

                return this;
            }

            public override bool Carries(Container other) => container == other;
        }

        class Unloading : State
        {
            readonly Container container;
            readonly Time deliveryTime;
            readonly Location origin;

            public Unloading(
                Container container,
                Time deliveryTime,
                Location origin)
            {
                this.container = container;
                this.deliveryTime = deliveryTime;
                this.origin = origin;
            }

            public override State Unload(Storage storage)
            {
                var destination = container.LocationAfter(origin);

                if (destination == storage.Location)
                {
                    storage.Stock(container.With(deliveryTime));
                    return new Returning(deliveryTime, origin);
                }

                return this;
            }
        }

        class Returning : State
        {
            readonly Time transportationTime;
            readonly Location origin;
            Time travelTime;

            public Returning(Time transportationTime, Location origin)
            {
                this.transportationTime = transportationTime;
                this.origin = origin;
                travelTime = Time.Zero;
            }

            public override State Move(Time currentTime)
            {
                travelTime = travelTime.Advance();

                if (travelTime == transportationTime)
                {
                    return new Loading(origin);
                }

                return this;
            }
        }
    }
}