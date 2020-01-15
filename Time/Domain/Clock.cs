namespace TransportTycoon.Domain
{
    public class Clock
    {
        public Time Now { get; private set; } = Time.Zero;

        public void Tick()
        {
            Now = Now.Add(Time.From(1));
        }
    }
}