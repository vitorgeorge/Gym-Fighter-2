using Unity.VisualScripting;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public RectTransform joystickObj;
    public RectTransform Knob;
    private void Awake()
    {
        joystickObj = GetComponent<RectTransform>();
    }
}
