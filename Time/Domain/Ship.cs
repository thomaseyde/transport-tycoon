namespace TransportTycoon.Domain
{
    public class Ship : Transporter
    {
        public Ship() : base(Location.Port)
        {
        }

        public void LoadFrom(Port port) => Load(port.LoadContainer());

        public void UnloadTo(Warehouse warehouse) => Unload(warehouse.Stock);
    }
}