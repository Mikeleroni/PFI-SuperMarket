using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GenerateMik : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        Mesh mesh = new();
        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUV();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private Vector2[] GenerateUV()
    {
        return new Vector2[]
        {
            new Vector2(0,0), //0
            new Vector2(1,0), //1
            new Vector2(0,1), //2
            new Vector2(1,1), //3
        };
    }
    private static int[] GenerateTriangles()
    {
        int[] triangles = new int[6]
        {
            2,1,0,
            2,3,1,

        };
        return triangles;
    }

    private static Vector3[] GenerateVertices()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0,0, 0),
            new Vector3(1,0, 0),
            new Vector3(0,1, 0),
            new Vector3(1,1, 0),
        };
        return vertices;
    }


}
