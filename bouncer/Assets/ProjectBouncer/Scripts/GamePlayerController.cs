using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Util3D;
using Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bouncer
{
    public class GamePlayerController : MonoBehaviour
    {
        private UserInput userInput = UserInput.Create(0);
        public float playerSpeed = 2;

        public static int hitCount = 0;

        void Update()
        {
            float deltaTime = Time.deltaTime;
            PlayerActionHandler3D.MoveXZ(transform, userInput, playerSpeed, deltaTime);
        }

        public void Move(InputAction.CallbackContext context)
        {
            userInput = UserInput.Move(userInput, context);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag(GameTag.OBSTACLE.name)) hitCount++;
        }
    }
}