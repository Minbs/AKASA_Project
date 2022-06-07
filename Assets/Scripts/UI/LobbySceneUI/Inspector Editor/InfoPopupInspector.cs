//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;


//[CustomEditor(typeof(InfoPopup))]
//public class InfoPopupInspector : Editor
//{
//    public string stringToEdit = "modify me.";

//    string nameKey;
//    public InfoPopup current
//    {
//        get
//        {
//            return (InfoPopup)target;
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
//        catch (System.Exception e)
//        {
//            Debug.Log(e.Message);
//        }
//    }
//}
