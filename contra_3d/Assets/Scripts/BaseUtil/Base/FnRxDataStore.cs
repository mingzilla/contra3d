using System.Collections;
using System.Collections.Generic;

namespace BaseUtil.Base
{
    public class FnRxDataStore<T>
    {
        protected BehaviorSubject<T> subject = new BehaviorSubject<T>();
        protected T storeData;

        public Observable<T> GetObservable()
        {
            return this.subject.AsObservable();
        }

        public T GetCurrentStoreData()
        {
            return this.storeData;
        }

        protected void UpdateStoreData(T storeData)
        {
            this.storeData = storeData;
            Refresh();
        }

        public void Refresh()
        {
            subject.Next(storeData);
        }
    }
}