using UnityEngine;
using System.Collections.Generic;
using System;

namespace Adruian.CodeInjection
{
    public class InjectionScope<T> : ScriptableObject
    {
        private List<IDataCaller<T>> callers = new List<IDataCaller<T>>();
        private List<IDataListener<T>> listeners = new List<IDataListener<T>>();

        public void InjectData(T data)
        {
            foreach (IDataListener<T> listener in listeners)
                listener.VariableChanged(data);
        }

        public void AddListener(IDataListener<T> listener) => EnsureAddListener(listener);
        public void RemoveListener(IDataListener<T> listener) => EnsureRemoveListener(listener);

        public void AddCaller(IDataCaller<T> caller)
        {
            if (caller == null) return;
            callers.Add(caller);
            foreach (IDataListener<T> listener in listeners)
                caller.OnVariableChanged += listener.VariableChanged;
        }

        public void RemoveCaller(IDataCaller<T> caller)
        {
            if (!callers.Contains(caller)) return;
            callers.Remove(caller);
            foreach (IDataListener<T> listener in listeners)
                caller.OnVariableChanged -= listener.VariableChanged;
        }

        private void EnsureAddListener(IDataListener<T> listener)
        {
            listeners.Add(listener);
            foreach (IDataCaller<T> caller in callers)
                caller.OnVariableChanged += listener.VariableChanged;
        }

        private void EnsureRemoveListener(IDataListener<T> listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
                foreach (IDataCaller<T> caller in callers)
                    caller.OnVariableChanged -= listener.VariableChanged;
            }
        }
    }
}