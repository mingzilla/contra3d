using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        public float xForce = 3f;
        public float yForce = 9f;
        public WeaponType weaponType;
        private Rigidbody rb;

        public void Init(WeaponType type)
        {
            gameObject.layer = GameLayer.POWER_UP.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            weaponType = type;
            UnityFn.AddCollider<BoxCollider>(gameObject, true);
            UnityFn.Throw(rb, xForce, yForce, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            if (GameLayer.Matches(obj.layer, GameLayer.GROUND)) rb.velocity = Vector3.zero; // stop movement if touching ground
            if (GameLayer.Matches(obj.layer, GameLayer.PLAYER))
            {
                CharacterInGameController character = obj.GetComponentInParent<CharacterInGameController>();
                if (character != null) character.PowerUp(weaponType);
                Destroy(gameObject);
            }
        }
    }
}