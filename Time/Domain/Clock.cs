namespace TransportTycoon.Domain
{
    public class Clock
    {
        public Time Now { get; set; } = Time.Zero;

        public void Tick()
        {
            Now = Now.Advance();
        }
    }
}