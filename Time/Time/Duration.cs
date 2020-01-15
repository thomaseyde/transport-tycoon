namespace TransportTycoon.Time {
    public class Duration
    {
        public int Value { get; }

        public override string ToString() =>
            Value.ToString();

        public Duration(int value) => 
            Value = value;

        public static readonly Duration One = new Duration(1);
    }
}