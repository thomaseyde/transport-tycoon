using System;
using TransportTycoon.Optional;

namespace TransportTycoon.Domain
{
    public abstract class Transporter
    {
        private readonly Location _origin;

        private Option<Time> _transportationTime;
        private Option<Location> _destination;
        private Option<Container> _container;

        private Location _location;
        private Time _travelTime;

        protected Transporter(Location origin)
        {
            _origin = origin;
            _location = origin;
            _destination = Option.None;
            _transportationTime = Option.None;
            _container = Option.None;
        }

        public bool Carries(Container container)
        {
            return _container.Match(c => c == container, () => false);
        }

        protected void Load(Option<Container> container)
        {
            container
                .Filter(_ => _location == _origin)
                .MatchSome(Load);
        }

        public void Move()
        {
            _container.MatchSome(
                _ =>
                {
                    _travelTime = _travelTime.Advance();

                    _location = _transportationTime
                                .Filter(time => time == _travelTime)
                                .Map(_ => _destination)
                                .Reduce(_location);

                });
        }

        protected void Unload(Action<Container> handler)
        {
            var delivered = DeliveredContainer();

            delivered.MatchSome(
                container =>
                {
                    handler(container);
                });

            _container = delivered.Match(_ => Option.None, () => _container);
        }

        private Option<Container> DeliveredContainer()
        {
            return _destination
                   .Filter(location => location == _location)
                   .Map(_ => _container)
                   .Map(container => container.With(_transportationTime));
        }

        private void Load(Container container)
        {
            var destination = container.LocationAfter(_origin);
            _transportationTime = Time.Between(_origin, destination);
            _destination = destination;
            _travelTime = Time.Zero;
            _container = container;
        }
    }
}