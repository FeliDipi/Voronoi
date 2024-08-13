using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Voronoi))]
public class VoronoiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Voronoi voronoi = (Voronoi)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate"))
        {
            voronoi.Generate();
        }
    }
}
