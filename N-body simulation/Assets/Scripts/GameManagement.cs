using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public int nBodyCount;
    public float timeStep;

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

        computeShader.SetInt("nBodyCount", nBodyCount);
        computeShader.SetFloat("timeStep", timeStep);

        Vector3[] posData = new Vector3[nBodyCount];
        Vector3[] velData = new Vector3[nBodyCount];

        for (int i = 0; i < nBodyCount; i++)
        {
            float randX = (0.5f - (i % Mathf.Sqrt(nBodyCount)) / Mathf.Sqrt(nBodyCount)) * 16.0f;
            float randY = (0.5f - (i / Mathf.Sqrt(nBodyCount)) / Mathf.Sqrt(nBodyCount)) * 16.0f;

            posData[i] = new Vector3(randX, randY, 0);

            velData[i] = new Vector3(randY, -randX, 0) * 0.003f;
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
