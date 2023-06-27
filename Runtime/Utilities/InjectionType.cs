using UnityEngine;
namespace Adruian.CodeInjection
{
    public class InjectionType : MonoBehaviour, IDataInjection
    {
        [SerializeField] string type = string.Empty;
        public string Type => type;
    }
}