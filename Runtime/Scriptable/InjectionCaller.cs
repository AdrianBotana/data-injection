using TNRD;
using UnityEngine;
using System.Collections.Generic;

namespace Adruian.CodeInjection
{
    public class InjectionCaller<T> : MonoBehaviour
    {
        [SerializeField] private InjectionScope<T> scriptable;
        [SerializeField] private SerializableInterface<IDataCaller<T>> caller;

        private void Awake()
        {
            if (caller.TryGetValue(out IDataCaller<T> callerValue))
                scriptable.SetCaller(callerValue);
        }

        private void OnDestroy()
        {
            if (caller.TryGetValue(out IDataCaller<T> callerValue))
                scriptable.SetCaller(null);
        }
    }
}