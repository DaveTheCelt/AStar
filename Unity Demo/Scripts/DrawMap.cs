using UnityEngine;
namespace Pathfinding.Demo
{
    public sealed class DrawMap : MonoBehaviour
    {
        static Material lineMaterial;

        Map _map;
        SeekPath _seeker;

        [SerializeField]
        Color _obstacleColor;
        float _delta;


        private void Awake()
        {
            _map = GetComponentInParent<Map>();
            _seeker = GetComponent<SeekPath>();
            _seeker.OnRefresh += Reset;
        }

        private void Reset() => _delta = 0;

        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        // Will be called after all regular rendering is done
        public void OnRenderObject()
        {
            CreateLineMaterial();
            lineMaterial.SetPass(0);

            GL.MultMatrix(transform.localToWorldMatrix);

            DrawGrid();
            DrawObstacles();

            _delta += Time.deltaTime;
            if (_delta > _seeker.RecalculateRate / 4f + .2f)
                DrawPath();
        }

        private void DrawPath()
        {

            if (!_seeker.FoundPath)
            {
                GL.Begin(GL.LINES);
                GL.Color(Color.red);

                GL.Vertex(_seeker.StartPoint);
                GL.Vertex(_seeker.TargetPoint);

                GL.End();
                return;
            }

            GL.Begin(GL.LINES);
            GL.Color(Color.yellow);
            for (int i = 0; i < _seeker.Path.Count - 1; i++)
            {
                Node n = _seeker.Path[i];
                Node n2 = _seeker.Path[i + 1];

                var p1 = _map.ToWorldCenter(n.X, n.Y);
                var p2 = _map.ToWorldCenter(n2.X, n2.Y);

                GL.Vertex(p1);
                GL.Vertex(p2);
            }

            GL.End();
        }

        private void DrawObstacles()
        {
            GL.Begin(GL.TRIANGLES);
            GL.Color(_obstacleColor);
            foreach (var obstacle in _map.Obstacles)
            {
                GL.Vertex3(obstacle.x, 0, obstacle.y);
                GL.Vertex3(obstacle.x, 0, obstacle.y + 1);
                GL.Vertex3(obstacle.x + 1, 0, obstacle.y + 1);
                GL.Vertex3(obstacle.x, 0, obstacle.y);
                GL.Vertex3(obstacle.x + 1, 0, obstacle.y + 1);
                GL.Vertex3(obstacle.x + 1, 0, obstacle.y);
            }

            GL.End();
        }

        private void DrawGrid()
        {
            int rows = _map.Rows;
            int columns = _map.Columns;
            GL.Begin(GL.LINES);
            GL.Color(Color.green);

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, rows);

            GL.Vertex3(0, 0, rows);
            GL.Vertex3(columns, 0, rows);

            GL.Vertex3(columns, 0, rows);
            GL.Vertex3(columns, 0, 0);

            GL.Vertex3(columns, 0, 0);
            GL.Vertex3(0, 0, 0);


            GL.End();
        }
    }
}