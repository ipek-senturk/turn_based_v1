using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum ControllerState
    {
        Movable,
        Unmovable
    }

    public ControllerState state;
    private Vector3 inputMovement;

    void Start()
    {
        state = ControllerState.Movable;
    }

    void FixedUpdate()
    {
        if (state == ControllerState.Movable)
        {
            inputMovement.x = Input.GetAxisRaw("Horizontal");
            inputMovement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            // Reset movement when not movable
            inputMovement = Vector3.zero; 
        }
    }

    public Vector3 GetMovement()
    {
        return inputMovement;
    }

    public void StopPlayer()
    {
        state = ControllerState.Unmovable;
    }
}
