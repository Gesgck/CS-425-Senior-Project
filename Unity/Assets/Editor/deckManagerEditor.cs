using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(deckManager))]
public class deckManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        deckManager deckManager = (deckManager)target;
        if (GUILayout.Button("Draw Next Card")){
            handManager handManager = FindObjectOfType<handManager>();
            if (handManager != null){
                deckManager.drawCard(handManager);
            }
        }
    }
}
#endif