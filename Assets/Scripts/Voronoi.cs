using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private int _numberOfPoints = 10;
    [SerializeField] private Vector2Int _size = new Vector2Int(10, 10);

    private List<Vector2> _points = new List<Vector2>();
    private SpriteRenderer _spr;

    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Generate();
    }

    private void Generate()
    {
        _spr.sprite = null;
        _points.Clear();

        GeneratePoints();
        GenerateVoronoi();
    }

    private void GeneratePoints()
    {
        _points = new List<Vector2>();
        for (int i = 0; i < _numberOfPoints; i++)
        {
            Vector2 newPoint = new Vector2(Random.Range(0, _size.x), Random.Range(0, _size.y));
            _points.Add(newPoint);
        }
    }

    private void GenerateVoronoi()
    {
        Texture2D texture = new Texture2D(_size.x, _size.y);
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Vector2 closestPoint = _points[0];
                float closestDistance = Vector2.Distance(new Vector2(x, y), _points[0]);

                foreach (Vector2 point in _points)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), point);
                    if (distance < closestDistance)
                    {
                        closestPoint = point;
                        closestDistance = distance;
                    }
                }

                texture.SetPixel(x, y, ColorFromPoint(closestPoint));
            }
        }

        texture.Apply();
        _spr.sprite = Sprite.Create(texture, new Rect(0, 0, _size.x, _size.y), new Vector2(0.5f, 0.5f));
    }

    private Color ColorFromPoint(Vector2 point)
    {
        float hue = point.x / _size.x;
        float saturation = point.y / _size.y;
        return Color.HSVToRGB(hue, saturation, 1.0f);
    }
}

