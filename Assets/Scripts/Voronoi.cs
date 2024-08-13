using UnityEngine;
using UnityEngine.UI;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private int _textureWidth = 256;
    [SerializeField] private int _textureHeight = 256;
    [SerializeField] private int _pointCount = 10;
    [SerializeField] private RawImage _uiImage;

    private Texture2D _voronoiTexture;
    private Vector2[] _points;
    private Color[] _colors;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        GeneratePoints();
        GenerateVoronoiTexture();
        GenerateOverlay();
        ApplyTextureToUI();
    }


    private void GeneratePoints()
    {
        _points = new Vector2[_pointCount];
        _colors = new Color[_pointCount];

        for (int i = 0; i < _pointCount; i++)
        {
            _points[i] = new Vector2(Random.Range(0, _textureWidth), Random.Range(0, _textureHeight));
            _colors[i] = new Color(Random.value, Random.value, Random.value);
        }
    }

    private void GenerateVoronoiTexture()
    {
        _voronoiTexture = new Texture2D(_textureWidth, _textureHeight);

        for (int y = 0; y < _textureHeight; y++)
        {
            for (int x = 0; x < _textureWidth; x++)
            {
                Vector2 pixelPos = new Vector2(x, y);
                float closestDistance = float.MaxValue;
                int closestPointIndex = 0;

                for (int i = 0; i < _pointCount; i++)
                {
                    float distance = Vector2.Distance(pixelPos, _points[i]);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPointIndex = i;
                    }
                }

                _voronoiTexture.SetPixel(x, y, _colors[closestPointIndex]);
            }
        }

        _voronoiTexture.Apply();
    }

    private void GenerateOverlay()
    {
        foreach (Vector2 point in _points) 
        {
            _voronoiTexture.SetPixel((int)point.x, (int)point.y, Color.red);
        }

        _voronoiTexture.Apply();
    }

    private void ApplyTextureToUI()
    {
        if (_uiImage != null)
        {
            _uiImage.texture = _voronoiTexture;
        }
    }
}
