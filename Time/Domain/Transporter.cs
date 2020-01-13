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
        public void Move() => state = state.Move();
        protected void Load(Storage storage) => state = state.Load(storage);
        protected void Unload(Storage storage) => state = state.Unload(storage);

        public bool AtOrigin()
        {
            return state.AtOrigin();
        }

        abstract class State
        {
            public virtual State Move() => this;
            public virtual State Unload(Storage storage) => this;
            public virtual bool Carries(Container other) => false;
            public virtual State Load(Storage storage) => this;
            public virtual bool AtOrigin() => false;
        }

        class Loading : State
        {
            readonly Location origin;

            public Loading(Location origin)
            {
                this.origin = origin;
            }

            public override State Load(Storage storage)
            {
                State next = this;

                storage.LoadContainer(container =>
                {
                    var destination = container.LocationAfter(origin);
                    var transportationTime = Time.Between(origin, destination);

                    next = new Delivering(container, transportationTime, origin);
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
            readonly Time transportationTime;
            Time travelTime;
            readonly Location origin;

            public Delivering(
                Container container,
                Time transportationTime,
                Location origin)
            {
                this.container = container;
                this.transportationTime = transportationTime;
                this.origin = origin;
                travelTime = Time.Zero;
            }

            public override State Move()
            {
                travelTime = travelTime.Advance();

                if (travelTime == transportationTime)
                {
                    return new Unloading(container, transportationTime, origin);
                }

                return this;
            }

            public override bool Carries(Container other) => container == other;
        }

        class Unloading : State
        {
            readonly Container container;
            readonly Time transportationTime;
            readonly Location origin;

            public Unloading(
                Container container,
                Time transportationTime,
                Location origin)
            {
                this.container = container;
                this.transportationTime = transportationTime;
                this.origin = origin;
            }

            public override State Unload(Storage storage)
            {
                var destination = container.LocationAfter(origin);

                if (destination == storage.Location)
                {
                    storage.Stock(container.With(transportationTime));
                    return new Returning(transportationTime, origin);
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

            public override State Move()
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