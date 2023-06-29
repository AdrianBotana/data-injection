using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Adruian.CodeInjection
{
    public class InjectionListenerSearch : MonoBehaviour
    {
        public void FindAllDataInjectors()
        {
            var injectors = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataInjector>();

            foreach (var item in injectors)
                item.FindAllListeners();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(InjectionListenerSearch))]
        public class ObjectBuilderEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                InjectionListenerSearch myScript = (InjectionListenerSearch)target;
                if (GUILayout.Button("FindAllDataInjectors"))
                {
                    myScript.FindAllDataInjectors();
                }
            }
        }
#endif

    }
}