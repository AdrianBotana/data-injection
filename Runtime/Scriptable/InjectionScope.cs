using UnityEngine;
using System.Collections.Generic;
using System;

namespace Adruian.CodeInjection
{
    public class InjectionScope<T> : ScriptableObject
    {
        private List<IDataCaller<T>> callers = new List<IDataCaller<T>>();
        private List<Action<T>> listeners = new List<Action<T>>();

        public void InjectData(T data)
        {
            foreach (Action<T> listener in listeners)
                listener.Invoke(data);
        }

        public void AddListener(Action<T> listener) => EnsureAddListener(listener);
        public void AddListener(IDataListener<T> listener) => EnsureAddListener(listener.VariableChanged);

        public void RemoveListener(Action<T> listener) => EnsureRemoveListener(listener);
        public void RemoveListener(IDataListener<T> listener) => EnsureRemoveListener(listener.VariableChanged);

        public void AddCaller(IDataCaller<T> caller)
        {
            if (caller == null) return;
            callers.Add(caller);
            foreach (Action<T> listener in listeners)
                caller.OnVariableChanged += listener;
        }

        public void RemoveCaller(IDataCaller<T> caller)
        {
            if (!callers.Contains(caller)) return;
            callers.Remove(caller);
            foreach (Action<T> listener in listeners)
                caller.OnVariableChanged -= listener;
        }

        private void EnsureAddListener(Action<T> listener)
        {
            listeners.Add(listener);
            foreach (IDataCaller<T> caller in callers)
                caller.OnVariableChanged += listener;
        }

        private void EnsureRemoveListener(Action<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
                foreach (IDataCaller<T> caller in callers)
                    caller.OnVariableChanged -= listener;
            }
        }
    }
}