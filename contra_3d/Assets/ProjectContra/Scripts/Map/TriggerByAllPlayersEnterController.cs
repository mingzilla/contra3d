using System.Collections.Generic;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class TriggerByAllPlayersEnterController : MonoBehaviour
    {
        private GameStoreData storeData;
        public bool isActivated = false; // used to be checked to trigger action

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActivated) return;
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                Transform t = transform;
                LayerMask layer = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
                Collider[] results = new Collider[storeData.AllPlayerIds().Count];
                int size = Physics.OverlapBoxNonAlloc(t.position, t.localScale / 2, results, Quaternion.identity, layer);
                if (size == storeData.GetVisiblePlayers().Count) isActivated = true;
            }
        }
    }
}