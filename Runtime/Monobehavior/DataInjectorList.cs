using System.Collections.Generic;
using UnityEngine;
using TNRD;
using System;
using System.Linq;

namespace Adruian.CodeInjection
{
    public class DataInjectorList<T> : MonoBehaviour, IDataInjector
    {
        [SerializeField] private List<SerializableInterface<IDataCaller<T>>> callers;
        [SerializeField] private List<SerializableInterface<IDataListener<T>>> listeners;

        void Awake()
        {
            foreach (SerializableInterface<IDataListener<T>> listener in listeners)
            {
                foreach (SerializableInterface<IDataCaller<T>> caller in callers)
                {
                    caller.Value.OnVariableChanged += UpdateListener;
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

        public void AddInjectListener(IDataListener<T> listener)
        {
            foreach (SerializableInterface<IDataCaller<T>> caller in callers)
            {
                caller.Value.OnVariableChanged += UpdateListener;
                void UpdateListener(T obj)
                {
                    // listener.Value = obj;
                    listener.VariableChanged(obj);
                }
            }
        }

        public void AddListener(IDataListener<T> listener)
        {
            foreach (SerializableInterface<IDataCaller<T>> caller in callers)
            {
                caller.Value.OnVariableChanged += listener.VariableChanged;
            }
        }

        public void AddListener(Action<T> listener)
        {
            foreach (SerializableInterface<IDataCaller<T>> caller in callers)
            {
                caller.Value.OnVariableChanged += listener;
            }
        }

        public void RemoveListener(IDataListener<T> listener)
        {
            foreach (SerializableInterface<IDataCaller<T>> caller in callers)
            {
                caller.Value.OnVariableChanged -= listener.VariableChanged;
            }
        }

        public void RemoveListener(Action<T> listener)
        {
            foreach (SerializableInterface<IDataCaller<T>> caller in callers)
            {
                caller.Value.OnVariableChanged -= listener;
            }
        }

        void OnValidate()
        {
            string[] parts = typeof(T).ToString().Split('.');
            gameObject.name = "[" + parts[parts.Length - 1] + "]";
        }

        [Button(ButtonMode.DisabledInPlayMode)]
        public void FindAllListeners()
        {
            listeners.Clear();
            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IDataListener<T>>();
            foreach (IDataListener<T> s in ss)
            {
                SerializableInterface<IDataListener<T>> ser = new SerializableInterface<IDataListener<T>>();
                ser.Value = s;
                listeners.Add(ser);
            }
        }
    }
}