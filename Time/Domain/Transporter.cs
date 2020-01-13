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

        protected Transporter(Location origin)
        {
            _origin = origin;
            _location = origin;
            _container = Option.None;
            _transportationTime = new Time(0);
        }

        public void Move()
        {
            _travelTime = _travelTime.Advance();

            _location = _transportationTime
                        .Filter(time => time == _travelTime)
                        .FlatMap(_ => _container)
                        .Map(container => container.Destination)
                        .Map(destination => destination.Location)
                        .Reduce(_location);
        }

        protected void Load(Option<Container> container)
        {
            if (_location == _origin)
            {
                _transportationTime = TransportationTimeOf(container);
                _travelTime = new Time(0);
                _container = container;
            }
        }

        private Option<Time> TransportationTimeOf(Option<Container> container)
        {
            return container
                   .Map(c => c.Destination)
                   .Map(destination => Time.Between(_origin, destination));
        }

        protected void Unload(Action<Container> handler)
        {
            _container
                .Map(container => container.With(_transportationTime))
                .Filter(container => container.Destination.Location == _location)
                .MatchSome(container =>
                {
                    handler(container);
                    _container = Option.None;
                });
        }

        public Option<Container> Container => _container;
    }
}