using UnityEditor;
using UnityEngine;
using System.IO;

namespace Adruian.Injection
{
    public class InjectorCreator
    {
        public static string className = string.Empty;
        public static string folderPath = string.Empty;

        [MenuItem("Injector/Create Injector Scripts")]
        static void CreateInjectorScripts()
        {
            StringModalWindow window = EditorWindow.CreateInstance<StringModalWindow>();
            window.titleContent = new GUIContent("Create Injector Scripts");
            window.ShowModal();

            if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(folderPath)) return;

            string injectionCaller = @"using UnityEngine;

namespace Adruian.CodeInjection
{
    public class " + className + @"InjectionCaller : InjectionCaller<" + className + @"> { }
}";

            File.WriteAllText(Path.Combine(folderPath, className + "InjectionCaller.cs"), injectionCaller);

            string injectionListeners = @"using UnityEngine;

namespace Adruian.CodeInjection
{
    public class " + className + @"InjectionListeners : InjectionListeners<" + className + @"> { }
}";

            File.WriteAllText(Path.Combine(folderPath, className + "InjectionListeners.cs"), injectionListeners);

            string injectionScope = @"using UnityEngine;

namespace Adruian.CodeInjection
{
    [CreateAssetMenu(fileName = " + '\u0022' + className + "InjectionScope" + '\u0022' + ", menuName = " + '\u0022' + "Injection/" + className + "InjectionScope" + '\u0022' + @", order = 0)]
    public class " + className + "InjectionScope : InjectionScope<" + className + @"> { }
}";

            File.WriteAllText(Path.Combine(folderPath, className + "InjectionScope.cs"), injectionScope);
        }

        private class StringModalWindow : EditorWindow
        {
            private void OnGUI()
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Class name", GUILayout.Width(100));
                className = GUILayout.TextField(className, 50);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Destination path", GUILayout.Width(100));
                folderPath = GUILayout.TextField(folderPath, 100);
                if (GUILayout.Button("Find", GUILayout.Width(50)))
                    folderPath = Path.GetRelativePath(Directory.GetParent(Application.dataPath).FullName, EditorUtility.SaveFolderPanel("Create scripts paths", Application.dataPath, string.Empty));
                GUILayout.EndHorizontal();

                if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(folderPath)) return;
                GUILayout.Space(10);
                if (GUILayout.Button("Submit")) Close();
            }
        }
    }
}