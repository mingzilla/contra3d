using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    public int rotationSpeed = 5;
    float currentY = 0;

    void Update()
    {
        Transform currentTransform = transform;
        currentY = (rotationSpeed * Time.time);
        currentTransform.rotation = new Quaternion(0f, currentY, 0f, 0f);
    }
}