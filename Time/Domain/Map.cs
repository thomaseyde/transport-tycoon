using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public class Map
    {
        readonly List<Storage[]> routes = new List<Storage[]>();
        readonly List<Leg> legs = new List<Leg>();

        public Storage NextDropPoint(Storage current, Storage final)
        {
            var route = routes.Single(locations => locations.Contains(final));

            for (var i = 0; i < route.Length - 1; i++)
            {
                if (route[i] == current)
                {
                    return route[i + 1];
                }
            }

            throw new InvalidOperationException(
                "Next destination not found for " + current);
        }

        public void Route(params Storage[] route)
        {
            routes.Add(route);
        }

        public void Time(Storage a, Storage b, int travelTime)
        {
            legs.Add(new Leg(a, b, travelTime));
        }

        public int TravelTimeBetween(Storage a, Storage b)
        {
            if (a == null) return 0;
            if (b == null) return 0;
            var leg = legs.Single(l => l.IsBetween(a, b));
            return leg.TravelTime;
        }

        class Leg
        {
            readonly Storage a;
            readonly Storage b;

            public Leg(Storage a, Storage b, in int travelTime)
            {
                this.a = a;
                this.b = b;
                TravelTime = travelTime;
            }

            public int TravelTime { get; }

            public bool IsBetween(Storage first, Storage second)
            {
                return first == a && second == b ||
                       first == b && second == a;
            }
        }

    }
}