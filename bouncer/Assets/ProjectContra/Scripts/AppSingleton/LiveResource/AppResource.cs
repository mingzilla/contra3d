using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppResource : MonoBehaviour
    {
        public static AppResource instance;
        public readonly GameStoreData storeData = new GameStoreData();

        [SerializeField] private GameObject basicBulletPrefab, blastBulletPrefab;
        public readonly Dictionary<WeaponType, GameObject> weaponTypeAndBulletPrefab = new Dictionary<WeaponType, GameObject>();

        [SerializeField] public GameObject smallExplosionPrefab, bigExplosionPrefab, explosiveShotEffectPrefab;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            ConfigureBullets();
        }

        void ConfigureBullets()
        {
            weaponTypeAndBulletPrefab[WeaponType.BASIC] = basicBulletPrefab;
            weaponTypeAndBulletPrefab[WeaponType.BLAST] = blastBulletPrefab;
        }

        public GameObject GetBulletPrefab(WeaponType weaponType)
        {
            if (!weaponTypeAndBulletPrefab.ContainsKey(weaponType)) return weaponTypeAndBulletPrefab[WeaponType.BASIC];
            return weaponTypeAndBulletPrefab[weaponType];
        }
    }
}