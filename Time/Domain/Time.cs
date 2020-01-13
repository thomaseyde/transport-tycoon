using System;
using Value;

namespace TransportTycoon.Domain
{
    public class Time : ValueObject
    {
        public static readonly Time Zero = new Time(0);

        public Time(int value)
        {
            Value = value;
        }

        public int Value { get; }


        public Time Advance()
        {
            return new Time(Value + 1);
        }

        public static Time Between(Location origin, Location destination)
        {
            if (origin == Location.Port)
            {
                return new Time(4);
            }

            if (origin == Location.Factory)
            {
                if (destination == Location.Port)
                {
                    return new Time(1);
                }
                if (destination == Location.B)
                {
                    return new Time(5);
                }
            }

            throw new InvalidOperationException();
        }
    }
}