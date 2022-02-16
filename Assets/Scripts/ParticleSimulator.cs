using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSimulator : MonoBehaviour {

    [SerializeField] private GameObject objectTemplate;

    private List<Particle> particleList = new List<Particle>();

    [SerializeField] private int objectToSimulate;
    [SerializeField] private float objectSpeed = 5f;

    void Start() {
        for(int i = 0; i < objectToSimulate; i++) {
            Particle particle = Instantiate(objectTemplate).GetComponent<Particle>();
            particle.Position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            particle.Velocity = Random.insideUnitCircle * objectSpeed;
            particleList.Add(particle);
        }
    }

    void Update() {
        for(int i = 0; i < particleList.Count; i++) {
            particleList[i].Position = particleList[i].Position + particleList[i].Velocity * Time.deltaTime;
            if(particleList[i].Position.x + particleList[i].Radius > 5f && particleList[i].Velocity.x > 0) {
                particleList[i].Velocity.x = -particleList[i].Velocity.x;
            }
            if(particleList[i].Position.x - particleList[i].Radius < -5f && particleList[i].Velocity.x < 0) {
                particleList[i].Velocity.x = -particleList[i].Velocity.x;
            }
            if (particleList[i].Position.y + particleList[i].Radius > 5f && particleList[i].Velocity.y > 0) {
                particleList[i].Velocity.y = -particleList[i].Velocity.y;
            }
            if(particleList[i].Position.y - particleList[i].Radius < -5f && particleList[i].Velocity.y < 0) {
                particleList[i].Velocity.y = -particleList[i].Velocity.y;
            }
        }
    }
}
