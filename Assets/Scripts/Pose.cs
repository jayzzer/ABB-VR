using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "NewPose")]
public class Pose : ScriptableObject
{
    public HandInfo leftHandInfo = HandInfo.Empty;
    public HandInfo rightHandInfo = HandInfo.Empty;

    public HandInfo GetHandInfo(HandType handType)
    {
        switch (handType)
        {
            case HandType.Left:
                return leftHandInfo;
            case HandType.Right:
                return rightHandInfo;
        }
        
        return HandInfo.Empty;
    }
}