using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    public Rigidbody[] limbs;
    public Animator animator;

    void Awake()
    {
        foreach (var rb in limbs)
        {
            rb.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;
        foreach (var rb in limbs)
        {
            rb.isKinematic = false;
        }
    }
}
