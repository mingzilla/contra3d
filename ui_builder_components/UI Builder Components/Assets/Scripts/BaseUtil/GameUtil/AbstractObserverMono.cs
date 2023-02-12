using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using System;
using System.Reflection;

namespace BaseUtil.GameUtil
{
    public abstract class AbstractObserverMono : MonoBehaviour
    {
        protected List<Observable<ObservableData>> subscriptions = new List<Observable<ObservableData>>();
        protected List<string> storeNames = new List<string>();

        private void OnDestroy()
        {
            subscriptions.ForEach((it) => it.Unsubscribe());
        }

        void SubscribeMultiple(Dictionary<string, FnRxDataStore<ObservableData>> fieldAndStore, Action updateContentFn)
        {
            List<string> keys = new List<string>(fieldAndStore.Keys);
            keys.ForEach((key) =>
            {
                subscriptions.Add(fieldAndStore[(key)].GetObservable().Subscribe((storeData) =>
                {
                    Type type = typeof(AbstractObserverMono);
                    FieldInfo fieldInfo = type.GetField(key);
                    fieldInfo.SetValue(this, key);

                    bool isObservedDataReady = keys.Count > 0 && Fn.All((k) => fieldInfo.GetValue(k) != null, keys);
                    if (isObservedDataReady)
                    {
                        updateContentFn();
                    }
                }));
            });
        }
    }
}