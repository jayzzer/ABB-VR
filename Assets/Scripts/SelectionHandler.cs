using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
public class SelectionHandler : MonoBehaviour
{
    public XRBaseInteractable CurrentInteractable { get; private set; } = null;

    public bool CheckForNewInteractable()
    {
        var newInteractable = GetInteractable();

        var isDifferent = IsDifferentInteractable(CurrentInteractable, newInteractable);
        CurrentInteractable = isDifferent ? newInteractable : CurrentInteractable;

        return isDifferent;
    }

    private XRBaseInteractable GetInteractable()
    {
        XRBaseInteractable newInteractable = null;
        GameObject selectedObject;

        #if UNITY_EDITOR
        selectedObject = Selection.activeGameObject;
        #endif

        if (!selectedObject) return null;
        
        if (selectedObject.TryGetComponent(out XRBaseInteractable interactable))
            newInteractable = interactable;

        return newInteractable;
    }

    private bool IsDifferentInteractable(XRBaseInteractable currentInteractable, XRBaseInteractable newInteractable)
    {
        var isDifferent = !currentInteractable;

        if (currentInteractable && newInteractable)
            isDifferent = currentInteractable != newInteractable;

        return isDifferent;
    }

    public GameObject SetObjectPose(Pose pose)
    {
        GameObject selectedObject;

        #if UNITY_EDITOR
        selectedObject = Selection.activeGameObject;
        #endif

        if (!selectedObject) return selectedObject;
        if (!selectedObject.TryGetComponent(out PoseContainer poseContainer)) return selectedObject;
        
        poseContainer.pose = pose;

        MarkActiveSceneAsDirty();

        return selectedObject;
    }

    public Pose TryGetPose(GameObject targetObject)
    {
        Pose pose = null;
        if (!targetObject) return null;
        
        if (targetObject.TryGetComponent(out PoseContainer poseContainer))
            pose = poseContainer.pose;

        return pose;
    }

    private void MarkActiveSceneAsDirty()
    {
        #if UNITY_EDITOR
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        #endif
    }
}