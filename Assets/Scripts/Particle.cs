using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    public Vector2 Velocity;

    public float Radius;

}
