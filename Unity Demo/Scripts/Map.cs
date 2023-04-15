using System;
using System.Collections.Generic;
using UnityEngine;
namespace Pathfinding.Demo
{
    [RequireComponent(typeof(Grid))]
    public class Map : MonoBehaviour
    {
        [SerializeField, Range(1, 50)]
        private int _rows;
        [SerializeField, Range(1, 50)]
        private int _columns;

        Grid _grid;
        Graph _graph;
        HashSet<Vector2Int> _obstacles = new();

        public Graph Graph => _graph;
        public int Rows => _rows;
        public int Columns => _columns;
        public HashSet<Vector2Int> Obstacles => _obstacles;

        private void Awake() => Generate();

        void Generate()
        {
            _grid = GetComponent<Grid>();
            Graph.TryCreate(_rows, _columns, out _graph);
        }

        public void GenerateObstacles(int total)
        {
            foreach (var o in _obstacles)
                _graph.ToggleWalkability(o.x, o.y, true);

            _obstacles.Clear();
            for (int i = 0; i < total; i++)
            {
                int x = UnityEngine.Random.Range(0, _columns);
                int y = UnityEngine.Random.Range(0, _rows);
                _graph.ToggleWalkability(x, y, false);
                if (_obstacles.Contains(new(x, y)))
                    i -= 1;
                else
                    _obstacles.Add(new(x, y));
            }
        }
        public Vector3 ToWorld(in int column, in int row) => new Vector3(column, 0, row);
        public Vector3 ToWorldCenter(in int column, in int row) => ToWorld(column, row) + _grid.cellSize / 2f;
        public Vector2Int ToGrid(in Vector3 worldPos)
        {
            var grid = _grid.WorldToCell(worldPos);
            return new(grid.x, grid.y);
        }
        public bool FindPath(int x1, int y1, int x2, int y2, in List<Node> path, TravelType travelType = TravelType.Cardinal)
        {
            return _graph != null && _graph.TryGet(x1, y1, out var startNode) && _graph.TryGet(x2, y2, out var targetNode) && _graph.FindPath(startNode, targetNode, path, travelType);
        }
    }
}