namespace TransportTycoon.Domain
{
    public class Factory : Storage
    {
        public void Produce(Container container) => Stock(container);
    }
}