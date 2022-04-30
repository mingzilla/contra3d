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

        // Player bullets
        [SerializeField] private GameObject basicBulletPrefab, blastBulletPrefab;
        public readonly Dictionary<WeaponType, GameObject> weaponTypeAndBulletPrefab = new Dictionary<WeaponType, GameObject>();

        // Enemy bullets
        [SerializeField] private GameObject enemyBasicBulletPrefab, enemyFollowerBulletPrefab, enemyGrenadePrefab, enemyBlastBulletPrefab;
        public readonly Dictionary<EnemyBulletType, GameObject> enemyBulletTypeAndBulletPrefab = new Dictionary<EnemyBulletType, GameObject>();

        // Explosion effects
        [SerializeField] public GameObject smallExplosionPrefab, bigExplosionPrefab, explosiveShotEffectPrefab;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            ConfigureBullets();
            ConfigureEnemyBullets();
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

        void ConfigureEnemyBullets()
        {
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.BASIC] = enemyBasicBulletPrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.FOLLOW] = enemyFollowerBulletPrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.GRENADE] = enemyGrenadePrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.BLAST] = enemyBlastBulletPrefab;
        }

        public GameObject GetEnemyBulletPrefab(EnemyBulletType enemyBulletType)
        {
            if (!enemyBulletTypeAndBulletPrefab.ContainsKey(enemyBulletType)) return enemyBulletTypeAndBulletPrefab[EnemyBulletType.BASIC];
            return enemyBulletTypeAndBulletPrefab[enemyBulletType];
        }
    }
}