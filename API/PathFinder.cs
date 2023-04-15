using System.Collections.Generic;
namespace Pathfinding
{
    public static class PathFinder
    {
        private static PriorityQueue<Node, uint> _frontier = new(8);
        private static Dictionary<Node, Node> _cameFrom = new(8);
        private static Dictionary<Node, uint> _cost = new(8);
        private static void ClearBuffers()
        {
            _frontier.Clear();
            _cameFrom.Clear();
            _cost.Clear();
        }
        public static bool FindPath(this Graph graph, in Node from, in Node target, in List<Node> result, TravelType travelType)
        {
            if (from.Equals(target) || !from.IsWalkable || !target.IsWalkable)
                return false;

            result.Clear();
            ClearBuffers();

            _frontier.Insert(from, 0);
            _cameFrom.Add(from, default);
            _cost.Add(from, 0);

            while (_frontier.Count > 0 )
            {
                if (!_frontier.TryPop(out var currNode, out var _))
                    return false;

                if (currNode.Equals(target))
                {
                    result.Add(currNode);
                    while (!_cameFrom[currNode].Equals(from))
                    {
                        result.Add(_cameFrom[currNode]);
                        currNode = _cameFrom[currNode];
                    }
                    result.Add(from);
                    result.Reverse();
                    return true;
                }
                
                for(int n = 0; n < (int)travelType; n++)
                {
                    GetNeighbour(n, out int column, out int row);
                    if (!graph.TryGet(currNode.X + column, currNode.Y + row, out Node nextNode) || !nextNode.IsWalkable || _cameFrom.ContainsKey(nextNode))
                        continue;

                        var cost = _cost[currNode];
                        cost += 1 + nextNode.TravelCost;

                        if(!_cost.ContainsKey(nextNode) || cost < _cost[nextNode])
                        {
                            _cost[nextNode] = cost;
                            _frontier.Insert(nextNode, cost + nextNode.Distance(currNode));
                            _cameFrom[nextNode] = currNode;
                        }
                    }
            }
            return false;
        }

        private static void GetNeighbour(in int index, out int column, out int row)
        {
            row = column = 0;
            switch (index)
            {
                case 0: // north
                    row += 1;
                    break;
                case 1: // east
                    column += 1;
                    break;
                case 2: // south
                    row -= 1;
                    break;
                case 3: // west
                    column -= 1;
                    break;
                case 4: // north east
                    row += 1;
                    column += 1;
                    break;
                case 5: // south east
                    row -= 1;
                    column += 1;
                    break;
                case 6: // south west
                    row -= 1;
                    column -= 1;
                    break;
                case 7: //north west
                    row += 1;
                    column -= 1;
                    break;
                default:
                    return;
            }
        }
    }
}