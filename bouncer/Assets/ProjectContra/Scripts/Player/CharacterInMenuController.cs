using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInMenuController : MonoBehaviour
    {
        public void Init()
        {
            gameObject.tag = GameTag.CHARACTER_IN_MENU.name;
        }

        public void HandleUpdate(UserInput userInput)
        {
            
        }
    }
}