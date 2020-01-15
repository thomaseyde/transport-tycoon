namespace TransportTycoon.Time
{
    public class Clock
    {
        public Moment Now { get; private set; } = Moment.Zero;

        public void Tick()
        {
            Now = Now.Add(Duration.One);
        }
    }
}