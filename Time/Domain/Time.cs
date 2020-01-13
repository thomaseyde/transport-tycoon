using System;
using Value;

namespace TransportTycoon.Domain
{
    public class Time : ValueObject
    {
        public Time(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public Time Advance()
        {
            return new Time(Value + 1);
        }

        public static Time Between(Location origin, Destination destination)
        {
            if (origin == Location.Port)
            {
                return new Time(4);
            }

            throw new InvalidOperationException();
        }
    }
}