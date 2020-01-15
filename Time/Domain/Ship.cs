
namespace TransportTycoon.Domain
{
    public class Ship : Transporter
    {
        public Ship() : base(Location.Port)
        {
        }

        public void LoadFrom(Port port, Time currentTime) => Load(port, currentTime);

        public void UnloadTo(Warehouse warehouse) => Unload(warehouse);
    }
}