using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


public class Monitor
{
    private readonly Dictionary<Type, object> waitObjects = new Dictionary<Type, object>();

    public WaitObject<T> Wait<T>() where T : struct
    {
        WaitObject<T> o = new WaitObject<T>();
        waitObjects.Add(typeof(T), o);
        return o;
    }

    public void SetResult<T>(T result) where T : struct
    {
        Type type = typeof(T);
        if (!waitObjects.TryGetValue(type, out object o))
            return;

        waitObjects.Remove(type);
        ((WaitObject<T>)o).SetResult(result);
    }

    public class WaitObject<T> : INotifyCompletion where T : struct
    {
        public bool IsCompleted { get; private set; }
        public T Result { get; private set; }

        private Action callback;

        public void SetResult(T result)
        {
            Result = result;
            IsCompleted = true;

            Action c = callback;
            callback = null;
            c?.Invoke();
        }

        public WaitObject<T> GetAwaiter()
        {
            return this;
        }

        public void OnCompleted(Action callback)
        {
            this.callback = callback;
        }

        public T GetResult()
        {
            return Result;
        }
    }
}

