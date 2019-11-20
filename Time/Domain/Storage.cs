using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public class Storage : IEquatable<Storage>
    {
        readonly Queue<Container> inventory = new Queue<Container>();

        readonly string name;

        public Storage(string name)
        {
            this.name = name;
        }

        public void Store(Container container)
        {
            inventory.Enqueue(container);
        }

        public void Pickup(Action<Container> loader)
        {
            if (inventory.Any())
            {
                loader(inventory.Dequeue());
            }
        }

        public void Drop(Container container)
        {
            inventory.Enqueue(container);
        }

        public override string ToString() => name;

        public bool HasInventory => inventory.Any();

        public bool Equals(Storage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Storage) obj);
        }

        public override int GetHashCode()
        {
            return name != null ? name.GetHashCode() : 0;
        }

        public static bool operator ==(Storage left, Storage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Storage left, Storage right)
        {
            return !Equals(left, right);
        }
    }
}