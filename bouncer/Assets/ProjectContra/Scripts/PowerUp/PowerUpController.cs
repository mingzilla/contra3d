using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        private Rigidbody rb;
        private float xForce = 1f;
        private float yForce = 4f;
        private WeaponType weaponType;

        public void Init(WeaponType type)
        {
            gameObject.layer = GameLayer.POWER_UP.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            weaponType = type;
            UnityFn.AddSphereCollider(gameObject, 1f, false);
            UnityFn.Throw(rb, xForce, yForce, 0f);
        }

        private void OnCollisionEnter(Collision other)
        {
            GameObject obj = other.gameObject;
            if (GameLayer.Matches(obj.layer, GameLayer.PLAYER))
            {
                CharacterInGameController character = obj.GetComponent<CharacterInGameController>();
                if (character != null) character.PowerUp(weaponType);
                Destroy(gameObject);
            }
        }
    }
}