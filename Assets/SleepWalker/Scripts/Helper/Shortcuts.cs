using UnityEditor;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    [MenuItem("Shortcuts/Unit/Flip Selected Units %#&f")]
    public static void FlipSelectedUnits()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        int targets = 0;
        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject.TryGetComponent(out Orientation orientation))
            {
                Debug.Log("Flip");
                orientation.EditorFlip();
                EditorUtility.SetDirty(orientation);
                targets++;
            }

            Aiming aiming = selectedObject.GetComponentInChildren<Aiming>();
            if (aiming)
            {
                aiming.EditorFlip();
                EditorUtility.SetDirty(aiming);
                targets++;
            }
        }

        if (targets > 0)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    [MenuItem("Shortcuts/Toggle Maximise Window %#&l")]
    public static void ToggleMaximiseWindow()
    {
        EditorWindow window = EditorWindow.focusedWindow;
        if (window == null)
            return;
        window.maximized = !window.maximized;
    }
}