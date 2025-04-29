using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject towerType1Prefab;
    public GameObject towerType2Prefab;
    private GameObject currentTowerPrefab;
    [SerializeField] private MoneyManager moneyManager;
    public int towerCost = 50;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))  // 按 1 選擇塔 1
        {
            currentTowerPrefab = towerType1Prefab;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))  // 按 2 選擇塔 2
        {
            currentTowerPrefab = towerType2Prefab;
        }

        
        if (currentTowerPrefab != null && Input.GetMouseButtonDown(0))
        {
            
            if (moneyManager.SpendMoney(towerCost)) //檢查錢夠不夠
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;  // 確保z=0
                Instantiate(currentTowerPrefab, worldPosition, Quaternion.identity);
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}
