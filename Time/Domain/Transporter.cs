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

        abstract class State 
        {
            public virtual State Move() => this;
            public virtual State Unload(Storage storage) => this;
            public virtual bool Carries(Container other) => false;
            public virtual State Load(Storage storage) => this;
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

                    next = new Delivering(container, transportationTime);
                });

                return next;
            }
        }

        class Delivering : State
        {
            readonly Container container;
            readonly Time transportationTime;
            Time travelTime;

            public Delivering(Container container, Time transportationTime)
            {
                this.container = container;
                this.transportationTime = transportationTime;
                travelTime = Time.Zero;
            }

            public override State Move()
            {
                travelTime = travelTime.Advance();

                if (travelTime == transportationTime)
                {
                    return new Unloading(container.With(transportationTime));
                }

                return this;
            }
         
            public override bool Carries(Container other) => container == other;
        }

        class Unloading : State
        {
            readonly Container container;

            public Unloading(Container container)
            {
                this.container = container;
            }

            public override State Unload(Storage storage)
            {
                storage.Stock(container);
                return new Returning();
            }
        }

        class Returning : State
        {
        }
    }
}