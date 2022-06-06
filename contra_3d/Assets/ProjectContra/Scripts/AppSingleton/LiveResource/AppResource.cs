using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppResource : MonoBehaviour
    {
        public static AppResource instance;
        public readonly GameStoreData storeData = new GameStoreData();

        [SerializeField] public GameObject playerPrefab;
        [SerializeField] public GameObject infoScreenPrefab;
        [SerializeField] public GameObject pauseMenuPrefab;

        // Player bullet prefabs
        [SerializeField] private GameObject basicBulletPrefab, blastBulletPrefab;
        public readonly Dictionary<WeaponType, GameObject> weaponTypeAndBulletPrefab = new Dictionary<WeaponType, GameObject>();

        // Power Up prefabs
        [SerializeField] public GameObject powerUpFPrefab,
            powerUpSPrefab,
            powerUpLPrefab;

        // Enemy prefabs
        [SerializeField] public GameObject enemyWalkerPrefab;

        // Enemy bullet prefabs
        [SerializeField] private GameObject enemyBasicBulletPrefab, enemyFollowerBulletPrefab, enemyGrenadePrefab, enemyPierceBulletPrefab, enemyBlastBulletPrefab;
        public readonly Dictionary<EnemyBulletType, GameObject> enemyBulletTypeAndBulletPrefab = new Dictionary<EnemyBulletType, GameObject>();

        // Skin material
        [SerializeField] private Material[] skins;

        // Explosion effects prefabs
        [SerializeField] public GameObject enemyBulletHitEffect,
            enemyDestroyedBigExplosion,
            enemyDestroyedSmallExplosion,
            enemyGrenadeSmallExplosion,
            playerBulletHitEffect,
            playerDestroyedEffect,
            playerExplosiveShotExplosion,
            powerUpDestroyedSmallExplosion;

        // SceneInitData
        [SerializeField] private SceneInitData sceneInitDataLv1,
            sceneInitDataLv2,
            sceneInitDataLv3,
            sceneInitDataLv4,
            sceneInitDataLv5;

        // EnemyAttribute
        [SerializeField] public EnemyAttribute enemyAttributeGroundCannon,
            enemyAttributeBossLv1Gun,
            enemyAttributeBossLv1WeakPoint;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            // currentSceneManager = Object.FindObjectOfType<CurrentSceneManagerController>();
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

        public GameObject GetBulletHitEffect(WeaponType weaponType)
        {
            if (weaponType == WeaponType.BASIC) return playerBulletHitEffect;
            if (weaponType == WeaponType.BLAST) return playerExplosiveShotExplosion;
            return null;
        }

        void ConfigureEnemyBullets()
        {
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.BASIC] = enemyBasicBulletPrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.FOLLOW] = enemyFollowerBulletPrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.GRENADE] = enemyGrenadePrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.PIERCE] = enemyPierceBulletPrefab;
            enemyBulletTypeAndBulletPrefab[EnemyBulletType.BLAST] = enemyBlastBulletPrefab;
        }

        public GameObject GetEnemyBulletPrefab(EnemyBulletType enemyBulletType)
        {
            if (!enemyBulletTypeAndBulletPrefab.ContainsKey(enemyBulletType)) return enemyBulletTypeAndBulletPrefab[EnemyBulletType.BASIC];
            return enemyBulletTypeAndBulletPrefab[enemyBulletType];
        }

        public Material GetSkin(int id)
        {
            return FnVal.SafeGet(skins[0], () => skins[id]);
        }

        public int GetSkinCount()
        {
            return skins.Length;
        }

        public SceneInitData GetSceneInitData(GameScene gameScene)
        {
            if (gameScene == GameScene.LEVEL_1) return sceneInitDataLv1;
            if (gameScene == GameScene.LEVEL_2) return sceneInitDataLv2;
            if (gameScene == GameScene.LEVEL_3) return sceneInitDataLv3;
            if (gameScene == GameScene.LEVEL_4) return sceneInitDataLv4;
            if (gameScene == GameScene.LEVEL_5) return sceneInitDataLv5;
            return null;
        }

        public GameObject GetPowerUpPrefab(WeaponType weaponType)
        {
            if (weaponType == WeaponType.BLAST) return powerUpFPrefab;
            if (weaponType == WeaponType.WIDE) return powerUpSPrefab;
            if (weaponType == WeaponType.LASER) return powerUpLPrefab;
            return null;
        }
    }
}