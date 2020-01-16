using System;
using TransportTycoon.Time;

namespace TransportTycoon.WithTypes.Stores
{
    class Factory
    {
        public Factory()
        {
            Location = Location.Factory;
        }

        public Location Location { get; }

        public T Load<T>(Func<Container, T> one, Func<T> none)
        {
            return one(new Container(Moment.Zero, Location.Port));
        }
    }
}