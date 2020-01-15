using System;
using TransportTycoon.Domain;
using Value;

namespace TransportTycoon.Time
{
    public class Moment : ValueObject
    {
        public static readonly Moment Zero = new Moment(0);
        public static Moment From(int value) => new Moment(value);
        
        public static Duration Between(Location origin, Location destination)
        {
            if (origin == Location.Port)
            {
                return new Duration(4);
            }

            if (origin == Location.Factory)
            {
                if (destination == Location.Port)
                {
                    return new Duration(1);
                }

                if (destination == Location.B)
                {
                    return new Duration(5);
                }
            }

            throw new InvalidOperationException();
        }

        public int Value { get; }

        public Moment Add(Duration duration) => 
            new Moment(Value + duration.Value);

        public override string ToString() => 
            Value.ToString();

        Moment(int value) => Value = value;
    }
}