using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSimulatorGPU : MonoBehaviour {

    [SerializeField] private GameObject objectTemplate;

    struct S_Particle {
        public Vector2 position;
        public Vector2 velocity;
        public float radius;
    }

    private const int STRUCT_SIZE = sizeof(float) * 5;

    private List<Particle> particleList = new List<Particle>();
    private S_Particle[] particleArray;
    private ComputeBuffer particleBuffer;

    public ComputeShader simulatorShader;
    private int kernelId;
    private int deltaTimeId;

    [SerializeField] private int objectToSimulate;
    [SerializeField] private float objectSpeed = 5f;

    void Start() {
        particleBuffer = new ComputeBuffer(objectToSimulate, STRUCT_SIZE);
        particleArray = new S_Particle[objectToSimulate];
        kernelId = simulatorShader.FindKernel("Simulate");
        simulatorShader.SetBuffer(kernelId, "particles", particleBuffer);
        simulatorShader.SetInt("particleCount", objectToSimulate);

        deltaTimeId = Shader.PropertyToID("deltaTime");

        for (int i = 0; i < objectToSimulate; i++) {
            Particle particle = Instantiate(objectTemplate).GetComponent<Particle>();
            particle.Position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            particle.Velocity = Random.insideUnitCircle * objectSpeed;
            particleList.Add(particle);
        }
    }

    void Update() {
        for(int i = 0; i < objectToSimulate; i++) {
            particleArray[i].position = particleList[i].Position;
            particleArray[i].velocity = particleList[i].Velocity;
            particleArray[i].radius = particleList[i].Radius;
        }

        particleBuffer.SetData(particleArray);
        simulatorShader.SetFloat(deltaTimeId, Time.deltaTime);

        simulatorShader.Dispatch(kernelId, Mathf.CeilToInt(objectToSimulate / 64f), 1, 1);

        particleBuffer.GetData(particleArray);

        for (int i = 0; i < objectToSimulate; i++) {
            particleList[i].Position = particleArray[i].position;
            particleList[i].Velocity = particleArray[i].velocity;
        }

    }
}
