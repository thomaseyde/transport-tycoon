using TransportTycoon.Time;
using TransportTycoon.WithSumType.Stores;

namespace TransportTycoon.WithSumType.Maps
{
    class Timetable
    {
        public static Moment ArrivalTime(
            Location origin,
            Location destination)
        {
            //todo - need a clock

            if (origin == Location.Factory)
            {
                return Moment.From(1);
            }

            return Moment.From(2);
        }
    }
}