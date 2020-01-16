using System.Collections.Generic;

namespace TransportTycoon.WithSumType.Stores
{
    class Port
    {
        readonly Queue<Container>
            inventory = new Queue<Container>();

        public bool Holds(Container container)
        {
            return inventory.Contains(container);
        }

        public void Replenish(Container container)
        {
            inventory.Enqueue(container);
        }
    }
}