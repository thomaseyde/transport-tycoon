namespace TransportTycoon.Domain
{
    public class Truck : Transporter
    {
        public Truck(Location origin) : base(origin)
        {
        }

        public void LoadFrom(Factory factory) => Load(factory.LoadContainer());

        public void UnloadTo(Warehouse warehouse) => Unload(warehouse.Stock);

        public void UnloadTo(Port port) => Unload(port.Stock);
    }
}