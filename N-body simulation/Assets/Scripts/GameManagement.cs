using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public int nBodyCount;

    public ComputeShader computeShader;

    public ComputeBuffer posBuffer;
    public ComputeBuffer velBuffer;

    Vector3[] positionData;
    Vector3[] velocityData;

    void Start()
    {
        posBuffer = new ComputeBuffer(nBodyCount, sizeof(float) * 3);
        velBuffer = new ComputeBuffer(nBodyCount, sizeof(float) * 3);

        Shader.SetGlobalBuffer(Shader.PropertyToID("position"), posBuffer);
        Shader.SetGlobalBuffer(Shader.PropertyToID("velocity"), velBuffer);

        computeShader.SetInt("bodyCount", nBodyCount);

        Vector3[] posData = new Vector3[nBodyCount];
        Vector3[] velData = new Vector3[nBodyCount];

        for (int i = 0; i < nBodyCount; i++)
        {
            float rand_x = (0.5f - (i % 256) / 256.0f) * 16.0f;
            float rand_y = (0.5f - (i / 256) / 256.0f) * 16.0f;

            posData[i].x = rand_x;
            posData[i].y = rand_y;

            velData[i].x = rand_y * 0.001f;
            velData[i].y = -rand_x * 0.001f;
        }

        posBuffer.SetData(posData);
        velBuffer.SetData(velData);
    }

    void Update()
    {
        computeShader.Dispatch(computeShader.FindKernel("CSMain"), 256, 1, 1);
    }

    void OnDestroy()
    {
        posBuffer.Dispose();
        velBuffer.Dispose();
    }
}
