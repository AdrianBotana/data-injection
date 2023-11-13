using TNRD;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Adruian.CodeInjection
{
    public class InjectionCaller<T> : MonoBehaviour, IDataInjector
    {
        public string Type => string.Empty;
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

        private void OnValidate()
        {
            if (callers.Count < 1)
                FindAllListeners();
        }

        public void FindAllListeners()
        {
            callers.Clear();
            var dataCallers = FindObjectsOfType<MonoBehaviour>().OfType<IDataCaller<T>>();
            foreach (IDataCaller<T> dataCaller in dataCallers)
            {
                SerializableInterface<IDataCaller<T>> serializableDataCaller = new SerializableInterface<IDataCaller<T>>();
                serializableDataCaller.Value = dataCaller;
                callers.Add(serializableDataCaller);
            }
        }
    }
}