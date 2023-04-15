using System;

namespace Pathfinding
{
    public class Graph
    {
        int _rows;
        int _columns;
        Node[] _graph;

        public int Rows => _rows;
        public int Columns => _columns;
        private Graph(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _graph = new Node[rows * columns];
            Initialize();
        }

        private void Initialize()
        {
            for (int q = 0; q < _columns; q++)
            {
                for (int r = 0; r < _rows; r++)
                {
                    int index = GetIndex(q, r);
                    _graph[index] = new(q, r);
                }
            }
        }

        public void ToggleWalkability(int column, int row, bool isWalkable)
        {
            if (!TryGet(column, row, out Node node, out int index))
                return;
            _graph[index] = new(column, row, isWalkable, node.TravelCost);
        }
        public void SetTravelCost(int column, int row, uint travelCost)
        {
            if (!TryGet(column, row, out Node node, out int index))
                return;
            _graph[index] = new(column, row, node.IsWalkable, travelCost);
        }

        public bool TryGet(int column, int row, out Node node, out int index)
        {
            index = GetIndex(column, row);
            if (column < 0 || row < 0 || column > Columns - 1 || row > Rows - 1 || index < 0 || index > _graph.Length - 1)
            {
                node = default;
                return false;
            }

            node = _graph[index];
            return true;
        }
        public bool TryGet(int column, int row, out Node node) => TryGet(column, row, out node, out _);
        public int GetIndex(int column, int row) => row * _columns + column;
        public void GetColumnRow(int index, out int column, out int row)
        {
            if (index < 0 || index > _graph.Length - 1)
                throw new Exception("Index " + index + " is out of range for 0.." + _graph.Length);
            row = index / _columns;
            column = index % _columns;
        }

        public static bool TryCreate(int rows, int columns, out Graph graph)
        {
            graph = null;
            if (rows < 1 || columns < 1)
                return false;

            graph = new Graph(rows, columns);
            return true;
        }

    }
}