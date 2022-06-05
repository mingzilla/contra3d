using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInstance : MonoBehaviour {

    public float deadTime;

	// Use this for initialization
	void Start () {
        Invoke("DestroyObject", deadTime);
	}

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
