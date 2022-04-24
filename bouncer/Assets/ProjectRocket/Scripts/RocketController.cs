using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Util3D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rocket
{
    public class RocketController : MonoBehaviour
    {
        private UserInput userInput = UserInput.Create(0);
        private Rigidbody rb;
        private AudioSource audioSource;

        public float rotateSpeed = 200f;
        public float boostSpeed = 1000f;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            if (userInput.horizontal != 0)
            {
                float speed = 0 - (rotateSpeed * userInput.horizontal * deltaTime);
                PlayerActionHandler3D.Rotate(rb, transform, Vector3.forward * speed);
            }
            if (userInput.fire1)
            {
                rb.AddRelativeForce(Vector3.up * boostSpeed * deltaTime);
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
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