using TNRD;
using UnityEngine;
using System.Collections.Generic;

namespace Adruian.CodeInjection
{
    public class InjectionCaller<T> : MonoBehaviour
    {
        [SerializeField] private InjectionScope<T> scriptable;
        [SerializeField] private List<SerializableInterface<IDataCaller<T>>> callers = new List<SerializableInterface<IDataCaller<T>>>();

        private void Awake()
        {
            foreach (SerializableInterface<IDataCaller<T>> listener in callers)
            {
                if (listener.TryGetValue(out IDataCaller<T> listenerValue))
                    scriptable.AddCaller(listenerValue);
            }
        }

        private void OnDestroy()
        {
            foreach (SerializableInterface<IDataCaller<T>> listener in callers)
            {
                if (listener.TryGetValue(out IDataCaller<T> listenerValue))
                    scriptable.RemoveCaller(listenerValue);
            }
        }
    }
}