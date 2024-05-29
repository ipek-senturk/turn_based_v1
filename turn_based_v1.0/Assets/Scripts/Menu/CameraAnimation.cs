using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    public float speed = 5f; // Camera speed x axis

    private enum CameraState
    {
        MovingRight,
        MovingUp,
        MovingLeft,
        MovingDown
    }

    private CameraState currentState = CameraState.MovingRight;

    void Update()
    {
        switch (currentState)
        {
            case CameraState.MovingRight:
                MoveRight();
                break;
            case CameraState.MovingUp:
                MoveUp();
                break;
            case CameraState.MovingLeft:
                MoveLeft();
                break;
            case CameraState.MovingDown:
                MoveDown();
                break;
        }
    }

    private void MoveRight()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);

        if (transform.position.x >= 35f)
        {
            currentState = CameraState.MovingUp;
        }
    }

    private void MoveUp()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        if (transform.position.y >= 20f)
        {
            currentState = CameraState.MovingLeft;
        }
    }

    private void MoveLeft()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);

        if (transform.position.x <= 0f)
        {
            currentState = CameraState.MovingDown;
        }
    }

    private void MoveDown()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

        if (transform.position.y <= -25f)
        {
            currentState = CameraState.MovingRight;
        }
    }
}
