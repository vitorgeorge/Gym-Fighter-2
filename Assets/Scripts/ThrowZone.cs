using UnityEngine;

public class ThrowZone : MonoBehaviour
{
    public StackManager stackManager;
    public UpgradeManager upgradeManager;

    private void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("Player"))
        {
            int dropped = stackManager.DropAll();
            upgradeManager.AddMoney(dropped * 10); // Ex: 10 por inimigo
        }*/
    }
}
