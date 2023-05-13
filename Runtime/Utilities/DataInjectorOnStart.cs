using System;
using UnityEngine;
namespace Adruian.CodeInjection
{
    public class DataInjectorOnStart<T> : MonoBehaviour, IDataCaller<T>
    {
        [SerializeField] T data;
        public event Action<T> OnVariableChanged;
        private void Start() => OnVariableChanged?.Invoke(data);
    }
}