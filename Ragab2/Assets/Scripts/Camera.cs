using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {


    [SerializeField]
    Transform focusTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FocusOnTarget();
	}

    private void FocusOnTarget()
    {
        this.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, this.transform.position.z);
    }
}
