using Sim.Faciem.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class DemoEditorWindow : FaciemEditorWindow
    {
        [MenuItem("Sim/Faciem/Demo Window")]
        public static void ShowDemoWindow()
        {
            // This method is called when the user selects the menu item in the Editor.
            EditorWindow wnd = GetWindow<DemoEditorWindow>();
            wnd.titleContent = new GUIContent("Faciem Demo Window");

            // Limit size of the window.
            wnd.minSize = new Vector2(450, 200);
            wnd.maxSize = new Vector2(1920, 720);
        }
    }
}