using System;

namespace AiAlgorithms.Trucks
{
    public class Box
    {
        public readonly int Index;
        public readonly double Volume;
        public readonly double Weight;

        public Box(int index, double weight, double volume)
        {
            Index = index;
            Weight = weight;
            Volume = volume;
        }

        protected bool Equals(Box other)
        {
            return Index == other.Index && Volume.Equals(other.Volume) && Weight.Equals(other.Weight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Box) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Index, Volume, Weight);
        }

        public static bool operator ==(Box left, Box right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Box left, Box right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{Index} {Weight} {Volume}";
        }
    }
}