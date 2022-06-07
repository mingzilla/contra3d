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

        [SerializeField] public GameObject musicManager, sfxManager;

        // Player
        [SerializeField] public GameObject playerPrefab;
        [SerializeField] public GameObject infoScreenPrefab;
        [SerializeField] public GameObject pauseMenuPrefab;

        // Player bullet prefabs
        [SerializeField] private GameObject bulletBasicPrefab,
            bulletFPrefab,
            bulletSPrefab;

        // Power Up prefabs
        [SerializeField] public GameObject powerUpFPrefab,
            powerUpSPrefab,
            powerUpLPrefab;

        // Enemy prefabs
        [SerializeField] public GameObject enemyWalkerPrefab;

        // Enemy bullet prefabs
        [SerializeField] private GameObject enemyBasicBulletPrefab,
            enemyFollowerBulletPrefab,
            enemyGrenadePrefab,
            enemyPierceBulletPrefab,
            enemyBlastBulletPrefab;

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
        }

        public Material GetSkin(int id)
        {
            return FnVal.SafeGet(skins[0], () => skins[id]);
        }

        public int GetSkinCount()
        {
            return skins.Length;
        }

        public GameObject GetBulletHitEffect(WeaponType weaponType)
        {
            if (weaponType == WeaponType.BASIC) return playerBulletHitEffect;
            if (weaponType == WeaponType.BLAST) return playerExplosiveShotExplosion;
            if (weaponType == WeaponType.WIDE) return playerBulletHitEffect;
            return null;
        }

        public GameObject GetEnemyBulletPrefab(EnemyBulletType enemyBulletType)
        {
            if (enemyBulletType == EnemyBulletType.BASIC) return enemyBasicBulletPrefab;
            if (enemyBulletType == EnemyBulletType.FOLLOW) return enemyFollowerBulletPrefab;
            if (enemyBulletType == EnemyBulletType.GRENADE) return enemyGrenadePrefab;
            if (enemyBulletType == EnemyBulletType.PIERCE) return enemyPierceBulletPrefab;
            if (enemyBulletType == EnemyBulletType.BLAST) return enemyBlastBulletPrefab;
            return enemyBasicBulletPrefab;
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

        public GameObject GetBulletPrefab(WeaponType weaponType)
        {
            if (weaponType == WeaponType.BLAST) return bulletFPrefab;
            if (weaponType == WeaponType.WIDE) return bulletSPrefab;
            return bulletBasicPrefab;
        }
    }
}