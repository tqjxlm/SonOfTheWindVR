using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMoving : MonoBehaviour
{
    // public Vector3 startingDirection;
    // public Vector3 moveSpeed;
    public float decayRate;
    // public float spreadRate;
    private WindZone windZone;
    private Rigidbody rigidBody;

    private float baseRadius;

    // Use this for initialization
    void Start()
    {
        windZone = GetComponent<WindZone>();
        rigidBody = GetComponent<Rigidbody>();
        baseRadius = windZone.radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rigidBody.velocity.magnitude > 0.1f)
        {
            windZone.windMain *= decayRate;
            windZone.windTurbulence *= decayRate;
            rigidBody.velocity *= decayRate;
            windZone.radius /= Mathf.Sqrt(decayRate);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
