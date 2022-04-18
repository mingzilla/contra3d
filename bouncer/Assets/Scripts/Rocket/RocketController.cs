using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rocket
{
    public class RocketController : MonoBehaviour
    {
        private UserInput userInput = UserInput.Create(0);
        private Rigidbody rb;

        public float rotateSpeed = 200f;
        public float boostSpeed = 1000f;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            if (userInput.horizontal != 0)
            {
                float speed = 0 - (rotateSpeed * userInput.horizontal * deltaTime);
                rb.freezeRotation = true;
                transform.Rotate(Vector3.forward * speed);
                rb.freezeRotation = false;
            }
            if (userInput.fire1)
            {
                rb.AddRelativeForce(Vector3.up * boostSpeed * deltaTime);
            }

            UserInput.ResetTriggers(userInput);
        }

        public void Move(InputAction.CallbackContext context)
        {
            userInput = UserInput.Move(userInput, context);
        }

        public void Boost(InputAction.CallbackContext context)
        {
            if (context.started) userInput.fire1 = true;
        }
    }
}