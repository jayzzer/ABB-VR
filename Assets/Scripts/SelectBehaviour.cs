using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectBehaviour : MonoBehaviour
{
    #region Settings

    [SerializeField] private InputActionProperty triggerSelectAction;

    #endregion

    #region References

    private XRRayInteractor _interactor;

    #endregion
}