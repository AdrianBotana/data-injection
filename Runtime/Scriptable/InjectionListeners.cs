using TNRD;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Adruian.CodeInjection
{
    public class InjectionListeners<T> : MonoBehaviour, IDataInjector
    {
        [SerializeField] string type = string.Empty;
        [SerializeField] bool includeType;
        [SerializeField] bool includeNoType;
        [Space]
        [SerializeField] private InjectionScope<T> scriptable;
        [SerializeField] private List<SerializableInterface<IDataListener<T>>> listeners = new List<SerializableInterface<IDataListener<T>>>();

        public string Type => type;

        private void Awake()
        {
            foreach (SerializableInterface<IDataListener<T>> listener in listeners)
            {
                if (listener.TryGetValue(out IDataListener<T> listenerValue))
                    scriptable.AddListener(listenerValue);
            }
        }

        private void OnDestroy()
        {
            foreach (SerializableInterface<IDataListener<T>> listener in listeners)
            {
                if (listener.TryGetValue(out IDataListener<T> listenerValue))
                    scriptable.RemoveListener(listenerValue);
            }
        }

        private void OnValidate()
        {
            if (listeners.Count < 1)
                FindAllListeners();
        }

        public void FindAllListeners()
        {
            listeners.Clear();
            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IDataListener<T>>();
            foreach (IDataListener<T> s in ss)
            {
                if ((s as MonoBehaviour).TryGetComponent<InjectionType>(out InjectionType injection))
                {
                    // if(includeType && injection.Type != Type) return;
                    // if(!includeType && injection.Type == Type) return;

                    if (includeType == (injection.Type != Type)) continue;
                }
                else if (!includeNoType) continue;

                SerializableInterface<IDataListener<T>> ser = new SerializableInterface<IDataListener<T>>();
                ser.Value = s;
                listeners.Add(ser);
            }
        }
    }
}