using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Screens.PlayerHp
{
    public class BossHpGridController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> bossHpItems;
        private List<BossHpUiController> bossHpCtrls;

        private void Start()
        {
            // note GetComponent() relies on gameObject to be active, so don't set prefab to inactive
            bossHpCtrls = Fn.Map(x => x.GetComponent<BossHpUiController>(), bossHpItems);
        }

        private void Update()
        {
            bossHpCtrls.ForEach(bossHpCtrl =>
            {
                GameObject bossHp = bossHpCtrl.gameObject;
                bool isVisible = !bossHpCtrl.bossCtrl.isBroken && bossHpCtrl.bossCtrl.gameObject.activeSelf;
                UnityFn.FastSetActive(bossHp, isVisible);
            });
        }
    }
}