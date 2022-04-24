using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bouncer
{
    public class SpinnerController : MonoBehaviour
    {
        public float rotationSpeed = 1f;

        void Update()
        {
            float speed = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, speed, 0);
        }
    }
}