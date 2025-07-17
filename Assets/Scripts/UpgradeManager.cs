using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public int money = 0;
    public int level = 1;
    public int stackCount = 2;
    public int currentStack;
    public int upgradeCost;
    public Text moneyText;
    public Text levelText;
    public Text stackText;
    public Renderer playerRenderer;

    public Material[] playerUpgradeMaterial;
    public int stackCounter;

    void Update()
    {
        moneyText.text = $"Money: {money}";
        levelText.text = $"Level: {level}";
        stackText.text = $"Stack: {stackCount}";
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void Upgrade()
    {
        if (money >= upgradeCost)
        {
            money -= upgradeCost;
            level++;
            stackCount++;
            playerRenderer.material.color = Random.ColorHSV(0f, 1f, 0.3f, 0.6f, 0.8f, 1f);
        }
    }
}
