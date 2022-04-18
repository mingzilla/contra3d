using System;
using System.Collections.Generic;

namespace BaseUtil.Base
{
    public interface ObservableData { }

    public class Observable<T>
    {
        private BehaviorSubject<T> subject;
        private Action<T> nextFn;

        public Observable(BehaviorSubject<T> subject, Action<T> nextFn)
        {
            this.subject = subject;
            this.nextFn = nextFn;
        }

        public Observable<T> Subscribe(Action<T> next)
        {
            this.nextFn = next;
            return this;
        }

        public void Unsubscribe()
        {
            if (this.subject != null)
            {
                this.subject.RemoveObservable(this);
            }
            this.subject = null;
            this.nextFn = EmptyFn;
        }

        public void Next(T data)
        {
            this.nextFn(data);
        }

        private static void EmptyFn(T data) { }
    }

    public class BehaviorSubject<T>
    {
        private List<Observable<T>> observables = new List<Observable<T>>();
        private static void EmptyFn(T data) { }

        public Observable<T> AsObservable()
        {
            var observable = new Observable<T>(this, EmptyFn);
            this.observables.Add(observable);
            return observable;
        }

        public void RemoveObservable(Observable<T> observable)
        {
            var index = this.observables.IndexOf(observable);
            if (index > -1)
            {
                this.observables.RemoveAt(index);
            }
        }

        public void Next(T data)
        {
            this.observables.ForEach((Observable<T> observable) => { observable.Next(data); });
        }

        public void Clear()
        {
            this.observables.ForEach((Observable<T> observable) => observable.Unsubscribe());
        }
    }
}