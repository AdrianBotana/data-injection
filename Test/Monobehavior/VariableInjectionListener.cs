using UnityEngine;

namespace Adruian.CodeInjection.Test
{
    public class VariableInjectionListener : MonoBehaviour, IDataListener<Vector3>
    {
        public void VariableChanged(Vector3 value) => Debug.Log(value);
    }
}