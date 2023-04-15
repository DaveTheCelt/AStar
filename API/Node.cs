using System;
namespace Pathfinding
{
    public readonly struct Node: IEquatable<Node>
    {
        private readonly int _x;
        private readonly int _y;
        private readonly bool _isWalkable;
        private readonly uint _travelCost;

        public int X => _x;
        public int Y => _y;
        public bool IsWalkable => _isWalkable;
        public uint TravelCost => _travelCost;

        public Node(in int x, in int y, in bool isWalkable = true, in uint travelCost = 0)
        {
            _x = x;
            _y = y;
            _isWalkable = isWalkable;
            _travelCost = travelCost;
        }
        public bool Equals(Node other) => _x == other.X && _y == other.Y;
        public override int GetHashCode() => _x + 3 * _y;
        public override bool Equals(object obj)
        {
            if (obj is Node node)
                return node.Equals(this);
            return false;
        }
        public override string ToString()
        {
            string s = "[" + X + "," + Y+"]";
            s += " IsWalkable: " + IsWalkable;
            s += " Travel Cost: " + TravelCost;
            return s;
        }
        public uint Distance(in Node node)
        {
            var dx = _x < node._x ? node._x - _x : _x - node._x;
            var dy = _y < node._y ? node._y - _y : _y - node._y;
            return (uint)(dx + dy);
        }
    }
}