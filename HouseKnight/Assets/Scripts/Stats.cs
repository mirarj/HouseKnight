using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private Vector3[] newVertices;
    private int[] newTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
    public void Start()
    {
        Mesh mesh = new Mesh();
        Vector3 V1 = new Vector3(0, 0, 0);
        Vector3 V2 = new Vector3(0, 100, 0);
        Vector3 V3 = new Vector3(100, 100, 0);
        Vector3 V4 = new Vector3(100, 0, 0);
        newVertices = new Vector3[] { V1, V2, V3, V4 };
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        CanvasRenderer rend = GetComponent<CanvasRenderer>();
        rend.SetMesh(mesh);
    }
}
