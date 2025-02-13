using System;
using UnityEngine;



[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] Vector2Int Size;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMesh();
    }

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh = new Mesh(); // Create Mesh
        mesh.vertices = CreateMeshVertices();
        mesh.triangles = CreateMeshTriangles();

        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh; // Assign Mesh to MeshFilter Component
    }


    private Vector3[] CreateMeshVertices()
    {
        vertices = new Vector3[(Size.x + 1) * (Size.y + 1)]; // Create empty vertice array. eg if you want 10x10 squares, you will have 11x11 vertices
        for (int i = 0, z = 0; z <= Size.y; z++)
        {
            for (int x = 0; x <= Size.x; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++; // Iterates over every vertice 
            }
        }

        return vertices;
    }

    private int[] CreateMeshTriangles()
    {
        triangles = new int[6 * Size.x * Size.y]; // There are Size.x x Size.y squares, and each square is made up of two triangles, 6 vertices
        int vertice = 0; // which vertice we are looking at
        int square = 0; // which square we are looking at


        for (int z = 0; z < Size.y; z++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                // for each square, there reason for Size.x addition is the vertices are on another "layer", or just Size.x indices higher
                triangles[square] = vertice + 0;
                triangles[square + 1] = vertice + 1 + Size.x;
                triangles[square + 2] = vertice + 1;
                triangles[square + 3] = vertice + 1;
                triangles[square + 4] = vertice + 1 + Size.x;
                triangles[square + 5] = vertice + 2 + Size.x;

                vertice++;
                square += 6;
            }
            vertice++; // Moves to the next layer of vertices (skips the creation of a triangle from left most vertice to right most +1 higher vertice)
        }

        return triangles;
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
