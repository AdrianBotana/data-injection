using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Adruian.CodeInjection
{
    public class DataInjectorSearch : MonoBehaviour
    {
        public void FindAllDataInjectors()
        {
            var injectors = FindObjectsOfType<MonoBehaviour>().OfType<IDataInjector>();

            foreach (var item in injectors)
                item.FindAllListeners();
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(DataInjectorSearch))]
        public class ObjectBuilderEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                DataInjectorSearch myScript = (DataInjectorSearch)target;
                if (GUILayout.Button("FindAllDataInjectors"))
                {
                    myScript.FindAllDataInjectors();
                }
            }
        }
#endif

    }
}