using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRenderer : MonoBehaviour
{
    public Material material;

    Matrix4x4[][] transformList;

    Mesh mesh;

    int nBodyCount;

    const int instanceMax = 1023;

    public float starSize;

    void Start()
    {
        nBodyCount = GetComponent<GameManagement>().nBodyCount;

        transformList = new Matrix4x4[nBodyCount / instanceMax][];

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-starSize, -starSize, 0);
        vertices[1] = new Vector3(starSize, -starSize, 0);
        vertices[2] = new Vector3(-starSize, starSize, 0);
        vertices[3] = new Vector3(starSize, starSize, 0);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        for (int set = 0; set < nBodyCount / instanceMax; set++)
        {
            int instances = instanceMax;
            if (set == (nBodyCount / instanceMax) - 1)
            {
                instances = nBodyCount % instanceMax;
            }

            transformList[set] = new Matrix4x4[instances];

            for (int i = 0; i < instances; i++)
            {
                Matrix4x4 matrix = new Matrix4x4();

                matrix.SetTRS(Vector3.zero, Quaternion.Euler(Vector3.zero), Vector3.one);

                transformList[set][i] = matrix;
            }
        }
    }

    void Update()
    {
        for (int set = 0; set < nBodyCount / instanceMax; set++)
        {
            int instances = instanceMax;

            if (set == (nBodyCount / instanceMax) - 1)
            {
                instances = nBodyCount % instanceMax;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            mpb.SetInt("offset", set * instanceMax);
            
            if (set >= nBodyCount / instanceMax / 2)
                mpb.SetColor("color", new Color(1, 0, 0.5f, 1));
            else
                mpb.SetColor("color", new Color(0, 0.5f, 1, 1));

            Graphics.DrawMeshInstanced(mesh, 0, material, transformList[set], instances, mpb);
        }
    }
}
