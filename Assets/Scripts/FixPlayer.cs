using UnityEngine;
using UnityEngine.InputSystem;

public class InputFix : MonoBehaviour
{
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions?.Disable();
        }
    }
}