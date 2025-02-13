using System;
using UnityEngine;



[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    public int xSize;
    public int zSize;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh(); // Create Mesh
        GetComponent<MeshFilter>().mesh = mesh; // Assign Mesh to MeshFilter Component

        CreateMesh();
        UpdateMesh();
    }
    private void Update()
    {

    }

    void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)]; // Create empty vertice array. eg if you want 10x10 squares, you will have 11x11 vertices
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++; // Iterates over every vertice 
            }
        }

        triangles = new int[6 * xSize * zSize]; // There are xSize x zSize squares, and each square is made up of two triangles, 6 vertices
        int vertice = 0; // which vertice we are looking at
        int square = 0; // which square we are looking at


        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                // for each square, there reason for xSize addition is the vertices are on another "layer", or just xSize indices higher
                triangles[square] = vertice + 0;
                triangles[square + 1] = vertice + 1 + xSize;
                triangles[square + 2] = vertice + 1;
                triangles[square + 3] = vertice + 1;
                triangles[square + 4] = vertice + 1 + xSize;
                triangles[square + 5] = vertice + 2 + xSize;

                vertice++;
                square += 6;
            }
            vertice++; // Moves to the next layer of vertices (skips the creation of a triangle from left most vertice to right most +1 higher vertice)
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {   
        if (vertices == null) { return; }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
