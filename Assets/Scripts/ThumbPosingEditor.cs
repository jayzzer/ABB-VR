using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandPoseController))]
public class ThumbPosingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var thumbPosing = (HandPoseController) target;

        if (GUILayout.Button("Start squeeze"))
        {
            thumbPosing.StartSqueeze();
        }

        if (GUILayout.Button("Stop squeeze"))
        {
            thumbPosing.StopSqueeze();
        }

        if (GUILayout.Button("Reset hand"))
        {
            thumbPosing.ResetHand();
        }
    }
}