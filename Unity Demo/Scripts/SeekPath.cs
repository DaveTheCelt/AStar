using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Demo
{
    public class SeekPath : MonoBehaviour
    {
        List<Node> _path = new();
        Map _map;
        Vector2Int _target;
        Vector2Int _start;

        float _delta;
        [SerializeField, Range(0.5f, 10), Header("Recalculate Path")]
        float _rate = 4;

        [SerializeField, Range(0, 100)]
        private int _totalObstacles;
        [SerializeField]
        private bool _regenerateObstacles;
        private bool _foundPath;

        public Vector3 StartPoint => _map.ToWorldCenter(_start.x, _start.y);
        public Vector3 TargetPoint => _map.ToWorldCenter(_target.x, _target.y);
        public Action OnRefresh { get; set; }
        public IReadOnlyList<Node> Path => _path;
        public bool FoundPath => _foundPath;
        public float RecalculateRate => _rate;

        void Awake() => _map = GetComponent<Map>();

        private void Start()
        {
            Refresh();
        }

        private void Update()
        {
            if (_regenerateObstacles)
            {
                _map.GenerateObstacles(_totalObstacles);
                _regenerateObstacles = false;
                Refresh();
            }

            _delta += Time.deltaTime;
            if (_delta >= _rate)
                Refresh();
        }

        void Refresh()
        {
            _delta = 0;
            GetNextTarget();
            FindPath();
            OnRefresh?.Invoke();

        }
        void FindPath() => _foundPath = _map.FindPath(_start.x, _start.y, _target.x, _target.y, _path);

        void GetNextTarget()
        {
            _start.x = _target.x;
            _start.y = _target.y;

            int x = UnityEngine.Random.Range(0, _map.Columns);
            int y = UnityEngine.Random.Range(0, _map.Rows);
            if (x == _target.x && y == _target.y)
                GetNextTarget();

            _target.x = x;
            _target.y = y;

        }
    }
}