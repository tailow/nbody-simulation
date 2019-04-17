using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRenderer : MonoBehaviour
{
    public Material material;

    Matrix4x4[][] transformList;

    Mesh mesh;

    int nBodyCount;

    const int instance_max = 1023;

    const float star_size = 0.01f;

    void Start()
    {
        nBodyCount = GetComponent<GameManagement>().nBodyCount;

        transformList = new Matrix4x4[nBodyCount / instance_max][];

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-star_size, -star_size, 0);
        vertices[1] = new Vector3(star_size, -star_size, 0);
        vertices[2] = new Vector3(-star_size, star_size, 0);
        vertices[3] = new Vector3(star_size, star_size, 0);

        mesh.vertices = vertices;

        int[] tri = new int[6];

        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;

        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;

        mesh.triangles = tri;

        for (int set = 0; set < nBodyCount / instance_max; set++)
        {
            int instances = instance_max;
            if (set == (nBodyCount / instance_max) - 1)
            {
                instances = nBodyCount % instance_max;
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
        for (int set = 0; set < nBodyCount / instance_max; set++)
        {
            int instances = instance_max;

            if (set == (nBodyCount / instance_max) - 1)
            {
                instances = nBodyCount % instance_max;
            }

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();

            mpb.SetInt("offset", set * instance_max);

            mpb.SetColor("color", new Color(1, 1, 1, 1));

            Graphics.DrawMeshInstanced(mesh, 0, material, transformList[set], instances, mpb);
        }
    }
}
