using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatFire : MonoBehaviour {

    public GameObject fireObject;
    public float timeInterval;
    private float timeIncrement;
    public bool isFiring = true;

	
	// Update is called once per frame
	void Update () {
        if (isFiring)
        {
            timeIncrement += Time.deltaTime;
            if(timeIncrement >= timeInterval)
            {
                Instantiate(fireObject, transform.position, transform.rotation);
                timeIncrement = 0f;
            }
        }
	}
}
