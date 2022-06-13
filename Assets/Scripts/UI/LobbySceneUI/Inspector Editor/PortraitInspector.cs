//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor (typeof(MinionsPortrait))]
//public class PortraitInspector : Editor
//{
//    public string stringToEdit = "modify me.";

//    string nameKey;
//    public MinionsPortrait current
//    {
//        get
//        {
//            return (MinionsPortrait) target;
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();

//        DrawDefaultInspector();

//        try
//        {
//            GUILayout.Label("Key_Name");
//            nameKey = GUILayout.TextField(nameKey, GUILayout.Width(100f));
//            if (GUILayout.Button("Add Dictionary"))
//            {
//                if (nameKey == null)
//                    throw new System.Exception("Key_name is null");
//                current.addDictionary(nameKey);
//            }
//            if (GUILayout.Button("Remove Dictionary"))
//            {
//                if (nameKey == null)
//                    throw new System.Exception("Key_name is null");
//                current.RemoveDictionary(nameKey);
//            }
//        }
//        catch(System.Exception e)
//        {
//            Debug.Log(e.Message);
//        }

//        //stringToEdit = GUI.TextField(new Rect(10, 10, 200, 20), stringToEdit, 25);

//        //if (GUI.changed)
//        //{
//        //    Debug.Log("TextField has changed.");
//        //    //current.UpdateMarker();
//        //}
//    }
//}
