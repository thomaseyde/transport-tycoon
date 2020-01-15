namespace TransportTycoon.Domain
{
    public class Factory : Storage
    {
        public Factory() : base(Location.Factory) { }
        public void Produce(Container container) => Replenish(container);
    }
}