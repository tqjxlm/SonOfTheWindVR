using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{
	private SteamVR_TrackedObject trackedObj;

    // 1
    private GameObject collidingObject;
    // 2
    private GameObject objectInHand;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 1
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        // 2
        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        if (collidingObject.CompareTag("Fan") || collidingObject.CompareTag("GameObject"))
        {
            // Debug.Log(collidingObject.name);
            GameObject currentItem = this.gameObject.transform.GetChild(0).gameObject;
            // Debug.Log(currentItem.name);

            Vector3 localPosition = collidingObject.transform.localPosition;
            Quaternion localRotation = collidingObject.transform.localRotation;

            // Exchange transformation
            currentItem.transform.SetParent(collidingObject.transform.parent);
            currentItem.transform.localRotation = localRotation;
            currentItem.transform.localPosition = localPosition;

            collidingObject.transform.SetParent(this.gameObject.transform);
            collidingObject.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);
            collidingObject.transform.localPosition = Vector3.zero;

            if (collidingObject.CompareTag("Fan"))
            {
                currentItem.transform.position += new Vector3(0, 0.7f, 0.0f);
                currentItem.transform.localRotation = Quaternion.Euler(-90.0f, -90.0f, 0.0f);

                collidingObject.transform.localPosition = new Vector3(0.0f, 0.0f, -0.1f);
            }
            else
            {
                currentItem.transform.position -= new Vector3(0, 0.7f, 0.0f);
                currentItem.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }

            // Exchange collider
            BoxCollider newCollider = currentItem.AddComponent<BoxCollider>();
            BoxCollider oldCollider = collidingObject.GetComponent<BoxCollider>();
            BoxCollider currentCollider = GetComponent<BoxCollider>();

            newCollider.center = currentCollider.center;
            newCollider.size = currentCollider.size;

            // currentCollider.center = oldCollider.center;
            currentCollider.center = oldCollider.center;
            Vector3 size = oldCollider.size;
            size.Scale(collidingObject.transform.localScale);
            currentCollider.size = size;

            Destroy(oldCollider);

            // Exchange rigidbody
            Rigidbody rigidbody = currentItem.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            Destroy(collidingObject.GetComponent<Rigidbody>());

            collidingObject = null;
        }
        else
        {
            objectInHand = collidingObject;
            collidingObject = null;
            // 2
            var joint = AddFixedJoint();
            joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }
    }

    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        // 1
        if (GetComponent<FixedJoint>())
        {
            // 2
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // 3
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        // 4
        objectInHand = null;
    }

}
