using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Rigidbody[] ragdollBodies;
    public UpgradeManager upgradeManager;
    public bool isDead;
    public bool activePickUp;
    void Start()
    {
        SetRagdollState(false);
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stack"))
        {
            upgradeManager.money++;
            Destroy(gameObject);
        }
    }
    public void ActivateRagdoll()
    {
        SetRagdollState(true);
    }
    public void DeactivateRagdoll()
    {
        SetRagdollState(false);
    }

    public IEnumerator ActivePickUp()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("agr sim");
        activePickUp = true;
    }
    void SetRagdollState(bool state)
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !state;
            if (rb.TryGetComponent<Collider>(out Collider col))
                col.enabled = state;
        }

        //if (TryGetComponent<Animator>(out Animator anim)) anim.enabled = !state;
        if (TryGetComponent<NavMeshAgent>(out var agent)) agent.enabled = !state;

        if (!isDead && state == true)
        {
            StartCoroutine(ActivePickUp());
            isDead = true;
        }
    }
}
