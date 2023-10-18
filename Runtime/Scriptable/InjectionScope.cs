using UnityEngine;
using System.Collections.Generic;
using System;

namespace Adruian.CodeInjection
{
    public class InjectionScope<T> : ScriptableObject
    {
        private IDataCaller<T> caller = null;
        private List<Action<T>> listeners = new List<Action<T>>();

        public void SetCaller(IDataCaller<T> caller)
        {
            if (caller != null)
            {
                foreach (Action<T> listener in listeners)
                    caller.OnVariableChanged -= listener;
            }
            this.caller = caller;
            if (caller == null) return;
            foreach (Action<T> listener in listeners)
                caller.OnVariableChanged += listener;
        }

        public void InjectData(T data)
        {
            foreach (Action<T> listener in listeners)
                listener.Invoke(data);
        }

        public void AddListener(Action<T> listener) => EnsureAddListener(listener);
        public void AddListener(IDataListener<T> listener) => EnsureAddListener(listener.VariableChanged);

        public void RemoveListener(Action<T> listener) => EnsureRemoveListener(listener);
        public void RemoveListener(IDataListener<T> listener) => EnsureRemoveListener(listener.VariableChanged);

        private void EnsureAddListener(Action<T> listener)
        {
            listeners.Add(listener);
            if (caller != null)
                caller.OnVariableChanged += listener;
        }

        private void EnsureRemoveListener(Action<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
                if (caller == null) return;
                caller.OnVariableChanged -= listener;
            }
        }
    }
}