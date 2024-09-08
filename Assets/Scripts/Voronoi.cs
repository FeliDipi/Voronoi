using System.Collections.Generic;
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
    private List<Vector2>[] _regionPixels;    

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        GeneratePoints();
        GenerateVoronoiTexture();
        GenerateOverlay();
        GenerateSprites();
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
        _regionPixels = new List<Vector2>[_pointCount];
        
        for (int i = 0; i < _pointCount; i++)
        {
            _regionPixels[i] = new List<Vector2>();
        }

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

                _regionPixels[closestPointIndex].Add(pixelPos);
                _voronoiTexture.SetPixel(x, y, _colors[closestPointIndex]);
            }
        }

        _voronoiTexture.Apply();
    }

    private Mesh CreateMeshForRegion(List<Vector2> regionPixels)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[regionPixels.Count];
        int[] triangles = new int[(regionPixels.Count - 2) * 3];

        for (int i = 0; i < regionPixels.Count; i++)
        {
            vertices[i] = new Vector3(regionPixels[i].x, regionPixels[i].y, 0);
        }

        // Simple triangulation for the region
        for (int i = 0; i < regionPixels.Count - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void GenerateSprites()
    {
        for (int i = 0; i < _regionPixels.Length; i++)
        {
            Mesh mesh = CreateMeshForRegion(_regionPixels[i]);

            GameObject regionObject = new GameObject($"Region_{i}");
            MeshFilter meshFilter = regionObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = regionObject.AddComponent<MeshRenderer>();

            meshFilter.mesh = mesh;
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshRenderer.material.color = _colors[i];
        }
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
