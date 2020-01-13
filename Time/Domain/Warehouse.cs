using System.Collections.Generic;

namespace TransportTycoon.Domain
{
    public class Warehouse : Storage
    {
        private readonly ContainerList _delivered;

        public Warehouse(Location location, ContainerList delivered) : base(location)
        {
            _delivered = delivered;
        }

        protected override void OnStocked(Container container)
        {
            _delivered.Add(container);
        }
    }
}