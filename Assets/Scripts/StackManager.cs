using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public Transform stackPoint;          
    public float verticalOffset = 0.5f;  
    public float verticalSpacing = 1f;
    Vector3 velocity = Vector3.zero;

    public float followSpeed = 5f;
    private List<Transform> stackedEnemies = new();

    public void AddToStack(Transform enemy)
    {
        foreach (var rb in enemy.GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = true;

        foreach (var col in enemy.GetComponentsInChildren<Collider>())
            col.enabled = false;

        /*if (enemy.TryGetComponent<Animator>(out var anim))
            anim.enabled = false;*/

        if (enemy.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
            agent.enabled = false;

        enemy.SetParent(stackPoint);

        Vector3 pos = Vector3.up * stackedEnemies.Count * verticalOffset;
        enemy.localPosition = pos;
        enemy.localRotation = Quaternion.identity;

        stackedEnemies.Add(enemy);
    }
    private void Update()
    {
        UpdateStackedEnemiesInertia();
    }
    void UpdateStackedEnemiesInertia()
    {
        for (int i = 0; i < stackedEnemies.Count; i++)
        {
            Transform current = stackedEnemies[i];
            Transform target = (i == 0) ? stackPoint : stackedEnemies[i - 1];
            Vector3 targetPos = target.position + Vector3.up * verticalSpacing;

            current.position = Vector3.SmoothDamp(current.position, targetPos, ref velocity, 0.1f);
        }
    }

    public int DropAllInZone(Transform dropZone)
    {
        int count = stackedEnemies.Count;
        foreach (var enemy in stackedEnemies)
        {
            enemy.SetParent(null);
            enemy.position = dropZone.position + Vector3.up;
            GameObject.Destroy(enemy.gameObject);
        }
        
        stackedEnemies.Clear();

        
        return count;
    }
}