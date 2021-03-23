using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteInEditMode]
public class HandManager : MonoBehaviour
{
    [SerializeField] private bool hideHands = true;
    [SerializeField] private GameObject leftHandPrefab = null;
    [SerializeField] private GameObject rightHandPrefab = null;

    public PreviewHand LeftHand { get; private set; } = null;
    public PreviewHand RightHand { get; private set; } = null;

    public bool HandsExist => LeftHand && RightHand;

    private void OnEnable()
    {
        CreateHandPreviews();
    }

    private void OnDisable()
    {
        DestroyHandPreviews();
    }

    private void CreateHandPreviews()
    {
        LeftHand = CreateHand(leftHandPrefab);
        RightHand = CreateHand(rightHandPrefab);
    }

    private PreviewHand CreateHand(GameObject prefab)
    {
        var handObject = Instantiate(prefab, transform);
        if (hideHands)
            handObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;

        return handObject.GetComponent<PreviewHand>();
    }

    private void DestroyHandPreviews()
    {
#if UNITY_EDITOR
        DestroyImmediate(LeftHand.gameObject);
        DestroyImmediate(RightHand.gameObject);
#endif
    }

    public void UpdateHands(Pose pose, Transform parentTransform)
    {
        LeftHand.transform.parent = parentTransform;
        RightHand.transform.parent = parentTransform;

        LeftHand.ApplyPose(pose);
        RightHand.ApplyPose(pose);
    }

    public void SavePose(Pose pose)
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(pose);
#endif

        pose.leftHandInfo.Save(LeftHand);
        pose.rightHandInfo.Save(RightHand);
    }
}