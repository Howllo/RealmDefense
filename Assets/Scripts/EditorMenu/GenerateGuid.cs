using System;
using UnityEditor;
using Random = UnityEngine.Random;

namespace EditorMenu
{
    // https://docs.unity3d.com/ScriptReference/SerializedObject.html
    // This prevents so much copy & paste. Just place object in the 
    // scene and click the button to generate a GUID to create unique
    // ID for each objects. This is used for the save system to
    // find them and decided which object to be disable or enabled.
    // Ripped from Assignment 2.
    public class GenerateGuid : Editor
    {
        [MenuItem("Tool/Generate Guid/Generate Node GUID")]
        private static void GenerateCollectGuid()
        {
            Node[] allGo = FindObjectsOfType<Node>();
            foreach (Node go in allGo)
            {
                Node item = go.GetComponent<Node>();
                if (string.IsNullOrEmpty(item.objectGuid))
                {
                    string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                    var so = new SerializedObject(item);
                    so.FindProperty("objectGuid").stringValue = GUID;
                    so.ApplyModifiedProperties();
                }
            }
        }
    }
}