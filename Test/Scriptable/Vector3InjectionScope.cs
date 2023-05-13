using UnityEngine;

namespace Adruian.CodeInjection.Test
{
    [CreateAssetMenu(fileName = "Vector3InjectorScriptable", menuName = "Injection/Vector3InjectorScriptable", order = 0)]
    public class Vector3InjectionScope : InjectionScope<Vector3> { }
}