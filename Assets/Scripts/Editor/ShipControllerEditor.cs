using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(ShipController)), CanEditMultipleObjects]
public class ShipControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ShipController myScript = (ShipController)target;
        /*if (GUILayout.Button("Set Point"))
        {
            myScript.SetPoint();
        }*/
        if (GUILayout.Button("test"))
        {
            myScript.TestCircle();
        }
    }
}
