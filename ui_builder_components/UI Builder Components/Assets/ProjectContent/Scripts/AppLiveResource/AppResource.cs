using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContent.Scripts.Data;
using UnityEngine;

namespace ProjectContent.Scripts.AppLiveResource
{
    public class AppResource : MonoBehaviour
    {
        public static AppResource instance;
        public readonly GameStoreData storeData = new GameStoreData();
        private readonly BehaviorSubject<bool> intervalResetSubject = new BehaviorSubject<bool>();

        [SerializeField] public GameObject musicManager, sfxManager;
        [SerializeField] public GameObject pauseMenuEventSystem;

        private void Awake()
        {
            // https://docs.unity3d.com/Manual/class-MonoManager.html
            // So that it runs before e.g. current scene manager
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        public Observable<bool> GetIntervalResetObservable()
        {
            return intervalResetSubject.AsObservable();
        }

        /// <summary>
        /// UnityFn.RunWithInterval(AppResource.instance, intervalState) won't reset intervalState when stopping all coroutines.
        /// So anything using AppResource.instance to register RunWithInterval() need to subscribe to intervalResetSubject, so that intervalState can be reset.
        /// </summary>
        public void StopCoroutines()
        {
            StopAllCoroutines();
            intervalResetSubject.Next(true);
        }

    }
}