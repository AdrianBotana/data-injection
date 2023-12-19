using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Adruian.CodeInjection
{
    public class InjectionListenerSearch : MonoBehaviour
    {
        [SerializeField] bool autoFindInjectors = true;

        private void OnValidate()
        {
            if (autoFindInjectors)
                FindAllDataInjectors();
        }

        private void FindAllDataInjectors()
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
                if (myScript.autoFindInjectors) return;

                if (GUILayout.Button("FindAllDataInjectors"))
                    myScript.FindAllDataInjectors();
            }
        }
#endif

    }
}