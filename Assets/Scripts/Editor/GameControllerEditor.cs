using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button($"Remove Save File", GUILayout.Height(30)))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}