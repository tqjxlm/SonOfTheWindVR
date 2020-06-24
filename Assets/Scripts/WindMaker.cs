using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMaker : MonoBehaviour {

    public GameObject windZonePrefab;
    public GameObject tornadoPrefab;

    public Transform headTransform;

    public Terrain mainStage;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private bool isCasting = false;
	private float maxVelocity = 0.0f;
	private Vector3 startingDirection;

	// Use this for initialization
	void Start () {
	}

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
	
	// Update is called once per frame
	void Update () {

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            isCasting = true;

            startingDirection = Controller.transform.pos - headTransform.localPosition;

            maxVelocity = 0.0f;
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
			Vector3 endDirection = Controller.transform.pos - headTransform.localPosition;
            Debug.Log("Weeping angle: " + Vector3.Angle(startingDirection, endDirection));

			if (Vector3.Angle(startingDirection, endDirection) > 60) {
				GameObject windZoneInstance = Instantiate(windZonePrefab, headTransform.position, Quaternion.identity);
				windZoneInstance.GetComponent<Rigidbody>().velocity = headTransform.forward * maxVelocity * 2;

                mainStage.GetComponent<WindyTerrain>().trigger(0.9f, 0.9f, 0.9f);

                Debug.Log("Max velocity: " + maxVelocity);
				// windZoneInstance.GetComponent<Rigidbody>().angularVelocity = headTransform.forward;
			}

            isCasting = false;
        }

        if (isCasting)
        {
            if (Controller.velocity.magnitude > maxVelocity)
            {
                maxVelocity = Controller.velocity.magnitude;
            }
        }
	}
}
