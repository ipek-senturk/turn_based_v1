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

    private Vector3 InputMovement;

    // Start is called before the first frame update
    void Start()
    {
        state = ControllerState.Movable;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == ControllerState.Movable)
        {
            InputMovement.x = Input.GetAxisRaw("Horizontal");
            InputMovement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            InputMovement = Vector3.zero; // Reset movement when not movable
        }
    }

    public Vector3 getMovement()
    {
        return InputMovement;
    }

    public void stopPlayer()
    {
        state = ControllerState.Unmovable;
    }
}
