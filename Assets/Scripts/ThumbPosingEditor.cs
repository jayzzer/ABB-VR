using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsHandAnimation))]
public class ThumbPosingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var handAnimation = (PhysicsHandAnimation) target;

        if (GUILayout.Button("Start squeeze"))
        {
            handAnimation.StartSqueeze();
        }

        if (GUILayout.Button("Stop squeeze"))
        {
            handAnimation.StopSqueeze();
        }

        if (GUILayout.Button("Reset hand"))
        {
            handAnimation.ResetHand();
        }
    }
}