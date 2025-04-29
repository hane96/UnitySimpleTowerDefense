using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 100;  //初始money
    [SerializeField] private TextMeshProUGUI moneyText;  

    void Update()
    {
        // 更新金錢顯示
        moneyText.text = "Money: " + currentMoney.ToString();
    }

    public bool SpendMoney(int amount) //花錢
    {
        if (currentMoney >= amount) //檢查錢夠不夠
        {
            currentMoney -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount) //加錢
    {
        currentMoney += amount;
    }
}
