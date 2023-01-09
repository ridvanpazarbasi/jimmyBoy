using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(LeaderbordControl))]
public class LeaderboardEditor : Editor {

    public override void OnInspectorGUI()
    {
        LeaderbordControl myScript = (LeaderbordControl)target;

        myScript.inGame = EditorGUILayout.Toggle("In Game", myScript.inGame);

        if (!myScript.inGame)
        {
            myScript.rankText = (Text)EditorGUILayout.ObjectField("Rank Text",myScript.rankText, (typeof(Text)), true);

            myScript.topScore = (Text)EditorGUILayout.ObjectField("TopScore Text", myScript.topScore, (typeof(Text)), true);

            myScript.bageDisplay = (Image) EditorGUILayout.ObjectField("Bage Text", myScript.bageDisplay, (typeof(Image)), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("stars"), true);

            serializedObject.ApplyModifiedProperties();

            myScript.store = (GameObject)EditorGUILayout.ObjectField("Player Store", myScript.store, (typeof(GameObject)), true);

            //myScript.leaderbordInfo = (GameObject)EditorGUILayout.ObjectField("TopScore Text", myScript.leaderbordInfo, (typeof(GameObject)), true);
        }
        else
        {
            myScript.scoreDisplay = (Text)EditorGUILayout.ObjectField("Bage Text", myScript.scoreDisplay, (typeof(Text)), true);
        }
    }
}
