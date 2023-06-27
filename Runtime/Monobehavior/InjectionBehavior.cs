using TNRD;
using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Adruian.CodeInjection
{
    public class InjectionBehavior<T> : MonoBehaviour, IDataInjector
    {
        [SerializeField] string type = string.Empty;
        [SerializeField] bool includeType;
        [SerializeField] bool includeNoType;
        [Space]
        [SerializeField] private SerializableInterface<IDataCaller<T>> caller;
        [SerializeField] private List<SerializableInterface<IDataListener<T>>> listeners;

        public string Type => type;

        void Awake()
        {
            if (caller.TryGetValue(out IDataCaller<T> callerValue))
            {
                foreach (SerializableInterface<IDataListener<T>> listener in listeners)
                {
                    callerValue.OnVariableChanged += UpdateListener;
                    void UpdateListener(T obj)
                    {
                        // listener.Value.Value = obj;
                        listener.Value.VariableChanged(obj);
                    }
                }
            }
        }

        public void InjectData(T data)
        {
            foreach (SerializableInterface<IDataListener<T>> listener in listeners)
            {
                // listener.Value.Value = data;
                listener.Value.VariableChanged(data);
            }
        }

        public void AddListener(IDataListener<T> listener)
        {
            caller.Value.OnVariableChanged += listener.VariableChanged;
        }

        public void AddListener(Action<T> listener)
        {
            caller.Value.OnVariableChanged += listener;
        }

        public void RemoveListener(IDataListener<T> listener)
        {
            caller.Value.OnVariableChanged -= listener.VariableChanged;
        }

        public void RemoveListener(Action<T> listener)
        {
            caller.Value.OnVariableChanged -= listener;
        }

        void OnValidate()
        {
            string[] parts = typeof(T).ToString().Split('.');
            gameObject.name = "[" + parts[parts.Length - 1] + "]";

            if (listeners.Count < 1)
                FindAllListeners();
        }

        public void FindAllListeners()
        {
            listeners.Clear();
            var ss = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataListener<T>>();
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
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
#endif
        }
    }
}