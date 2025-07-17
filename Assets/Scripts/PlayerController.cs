using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 JoystickSize = new Vector2(200, 200);
    public Joystick Joystick;
    public NavMeshAgent playerNavMesh;
    private Finger MovementFinger;
    public Vector2 MoveAmnt;
    public Animator playerAnim;
    public UpgradeManager upgradeManager;

    private void Start()
    {
        playerNavMesh = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<Animator>();
        upgradeManager = GetComponent<UpgradeManager>();
    }

    private void Update()
    {
        Vector3 scaledMove = playerNavMesh.speed * Time.deltaTime * new Vector3(MoveAmnt.x, 0, MoveAmnt.y);

        playerNavMesh.Move(scaledMove);

        playerNavMesh.transform.LookAt(playerNavMesh.transform.position + scaledMove, Vector3.up);


        playerAnim.SetFloat("moveX", MoveAmnt.x);
        playerAnim.SetFloat("moveZ", MoveAmnt.y);
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        EnhancedTouchSupport.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("achei o inimigo");
            Enemy ragdoll = other.GetComponent<Enemy>();
            //playerAnim.SetTrigger("");
            if (ragdoll.isDead == false)
            {
                if (ragdoll != null)
                {
                    //Debug.Log("ani");
                    playerAnim.SetFloat("moveX", 0f);
                    playerAnim.SetFloat("moveZ", 0f);
                    playerAnim.SetTrigger("Punch");
                    ragdoll.ActivateRagdoll();
                }
                /**/
            }
           /* else
            {
                ragdoll.DeactivateRagdoll();
            }*/
           if(ragdoll.activePickUp == true && upgradeManager.currentStack < upgradeManager.stackCount)
            {
                Debug.Log("passei no pickup");
                upgradeManager.currentStack++;
                var playerStack = GetComponentInParent<StackManager>();
                if (playerStack != null)
                {
                    playerStack.AddToStack(other.transform);
                }
            }
        }
        if (other.CompareTag("Stack"))
        {
            StackManager stack = gameObject.GetComponent<StackManager>();//other.GetComponent<StackManager>();
            
            if (stack != null)
            {
                int entregues = stack.DropAllInZone(transform);
                upgradeManager.AddMoney(entregues);
                upgradeManager.currentStack = 0;
                Debug.Log("DropZone recebeu " + entregues + " inimigos!");
                
            }
        }
    }

    private void HandleFingerMove(Finger move)
    {
        if (move == MovementFinger)
        {
            Vector2 knobPos;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = move.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, Joystick.joystickObj.anchoredPosition) > maxMovement)
            {
                knobPos = (currentTouch.screenPosition - Joystick.joystickObj.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPos = currentTouch.screenPosition - Joystick.joystickObj.anchoredPosition;
            }
            Joystick.Knob.anchoredPosition = knobPos;
            MoveAmnt = knobPos / maxMovement;
        }
    }

    private void HandleFingerDown(Finger move)
    {
        if (MovementFinger == null && move.screenPosition.x <= Screen.width)
        {
            MovementFinger = move;
            MoveAmnt = Vector2.zero;
            Joystick.gameObject.SetActive(true);
            Joystick.joystickObj.sizeDelta = JoystickSize;
            Joystick.joystickObj.anchoredPosition = ClampStartPosition(move.screenPosition);
        }
    }

    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == MovementFinger)
        {
            MovementFinger = null;
            Joystick.Knob.anchoredPosition = Vector2.zero;
            Joystick.gameObject.SetActive(false);
            MoveAmnt = Vector2.zero;
        }
    }

    private Vector2 ClampStartPosition(Vector2 screenPosition)
    {
        if (screenPosition.x < JoystickSize.x / 2)
        {
            screenPosition.x = JoystickSize.x / 2;
        }
        if (screenPosition.y < JoystickSize.y / 2)
        {
            screenPosition.y = JoystickSize.y / 2;
        }
        else if (screenPosition.y > Screen.height - JoystickSize.y / 2)
        {
            screenPosition.y = Screen.height - JoystickSize.y / 2;
        }
        return screenPosition;
    }
}
