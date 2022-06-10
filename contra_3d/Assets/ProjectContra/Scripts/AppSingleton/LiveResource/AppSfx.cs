using System.Linq;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppSfx : MonoBehaviour
    {
        public static AppSfx instance;

        [SerializeField] public AudioSource bulletB,
            bulletM,
            bulletS,
            bulletF,
            bulletFExplode,
            powerUpExplode,
            powerUpCollected,
            playerDeath,
            grenadeExploded,
            enemyDeath,
            bigEnemyDamaged,
            bigEnemyDeath,
            bossDeath,
            pause,
            levelClear;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        public static void Play(AudioSource source)
        {
            source.pitch = 1f;
            source.Stop();
            source.Play();
        }

        public static void PlayAdjusted(AudioSource source)
        {
            source.pitch = Random.Range(0.8f, 1.2f);
            source.Stop();
            source.Play();
        }

        public static void PlayRepeatedly(AudioSource source, int times)
        {
            foreach (int i in Enumerable.Range(0, times))
            {
                UnityFn.SetTimeout(instance, i, () =>
                {
                    source.pitch = Random.Range(0.8f, 1.2f);
                    source.Play();
                });
            }
        }

        public static void PlayBulletSound(WeaponType weaponType)
        {
            AudioSource source = instance.bulletB;
            if (weaponType == WeaponType.M) source = instance.bulletM;
            if (weaponType == WeaponType.WIDE) source = instance.bulletS;
            if (weaponType == WeaponType.BLAST) source = instance.bulletF;
            PlayAdjusted(source);
        }
    }
}