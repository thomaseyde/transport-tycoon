using System;

namespace TransportTycoon.Domain
{
    public abstract class Transporter
    {
        private Option<Time> _transportationTime;
        private Option<Container> _container;
        private readonly Location _origin;
        private Location _location;
        private Time _travelTime;
        private Option<Location> _destination;

        protected Transporter(Location origin)
        {
            _origin = origin;
            _location = origin;
            _container = Option.None;
            _transportationTime = Time.Zero;
        }

        protected void Load(Option<Container> container)
        {
            container
                .Filter(_ => _location == _origin)
                .MatchSome(Load);
        }

        public void Move()
        {
            _travelTime = _travelTime.Advance();

            _location = _transportationTime
                        .Filter(time => time == _travelTime)
                        .MapOptional(_ => _destination)
                        .Reduce(_location);
        }

        protected void Unload(Action<Container> handler)
        {
            var transported = TransportedContainer();

            transported.MatchSome(handler);

            _container = transported.Match(_ => Option.None, () => _container);
        }

        private Option<Container> TransportedContainer()
        {
            return _destination
                   .Filter(location => location == _location)
                   .MapOptional(_ => _container)
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

        public Option<Container> Container => _container;
    }
}