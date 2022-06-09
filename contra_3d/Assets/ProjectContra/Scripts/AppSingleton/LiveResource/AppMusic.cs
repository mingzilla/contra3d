using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class AppMusic : MonoBehaviour
    {
        public static AppMusic instance;

        [SerializeField] private AudioSource lv1Music,
            lv1BossMusic,
            lv2Music,
            lv2BossMusic,
            lv3Music;

        private List<AudioSource> AllMusic()
        {
            return new List<AudioSource>()
            {
                lv1Music,
                lv1BossMusic,
                lv2Music,
                lv2BossMusic,
                lv3Music,
            };
        }

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        public void PlayByScene(GameScene scene)
        {
            StopAll();
            if(scene == GameScene.LEVEL_1) lv1Music.Play();
            if(scene == GameScene.LEVEL_2) lv2Music.Play();
            if(scene == GameScene.LEVEL_3) lv3Music.Play();
        }

        public void PlayLv1BossMusic()
        {
            StopAll();
            lv1BossMusic.Play();
        }

        public void PlayLv2BossMusic()
        {
            StopAll();
            lv2BossMusic.Play();
        }

        public void StopAll()
        {
            AllMusic().ForEach(it => it.Stop());
        }
    }
}