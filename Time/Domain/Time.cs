using System;
using Value;

namespace TransportTycoon.Domain
{
    public class Time : ValueObject
    {
        public static readonly Time Zero = new Time(0);

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

        public int Value { get; }

        public Time Add(Time added) => new Time(Value + added.Value);

        public override string ToString() => Value.ToString();
        
        Time(int value) => Value = value;

        public static Time From(int value) => new Time(value);
    }
}