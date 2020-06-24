using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SceneSettings : MonoBehaviour {

	// Use this for initialization
	void Start () {
		XRSettings.eyeTextureResolutionScale = 2.0f; // tune this value between 1.4-2.0
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
