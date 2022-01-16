using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IInputManager
{ 
    float GetXRotation { get; }
    float GetYRotation { get; }
    bool GetRightMouseButton { get; }
    bool GetShootButton(bool isHandGun);
    bool GetScrollWheel { get; }
    bool GetReloadButton { get; }
}

public class InputManager : MonoBehaviour//, IInputManager
{
    [SerializeField] private string VerticalAxis;
    [SerializeField] private string HorizontalAxis;
    [SerializeField] private string EscapeButton;

    [HideInInspector] public static InputManager Instance;
    
    private void Awake()
    {
        Instance = this;       
    }
    public bool GetUpMovement
    {
        get { return Input.GetAxisRaw(VerticalAxis) > 0; }
    }

    public bool GetDownMovement
    {
        get { return Input.GetAxisRaw(VerticalAxis) < 0; }
    }

    public bool GetRightMovement
    {
        get { return Input.GetAxisRaw(HorizontalAxis) > 0; }
    }

    public bool GetLeftMovement
    {
        get { return Input.GetAxisRaw(HorizontalAxis) < 0; }
    }

    public bool GetPauseButton
    {
        get { return Input.GetButtonDown(EscapeButton); }
    }

}
