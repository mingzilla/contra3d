using System;
using System.Collections;
using System.Collections.Generic;
using Types;
using UnityEngine;

namespace Rocket
{
    public class RocketDestinationController : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(GameTag.PLAYER.name))
            {
                Debug.Log("Yay!");
            }
        }
    }
}