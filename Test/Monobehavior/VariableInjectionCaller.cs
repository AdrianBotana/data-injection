
using System;
using UnityEngine;

namespace Adruian.CodeInjection.Test
{
    public class VariableInjectionCaller : MonoBehaviour, IDataCaller<Vector3>
    {
        public event Action<Vector3> OnVariableChanged;

        [SerializeField] bool callEvent;
        [SerializeField] Vector3 vector;

        void Start() => OnVariableChanged?.Invoke(vector);

        private void OnValidate()
        {
            if (callEvent)
            {
                callEvent = false;
                OnVariableChanged?.Invoke(vector);
            }
        }
    }
}