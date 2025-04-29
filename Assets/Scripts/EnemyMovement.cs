using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 1f; //enemy speed
    public Transform[] path; //儲存path 這裡是存轉彎點
    public int initial_health;
    private castle castleScript;
    [SerializeField] private int enemyDamage;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private int rewardMoney;

    [SerializeField] private int health=1; //血量
    private int targetIndex = 0; //目標

    void Start() 
    {
        moneyManager = FindFirstObjectByType<MoneyManager>();
        castleScript = GameObject.Find("castle").GetComponent<castle>();
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Damage(int damage)
    {
        health-=damage;
        Debug.Log("Enemy health: " + health);
    }
    
    void Update()
    {
        if(health<=0)
        {
            moneyManager.AddMoney(rewardMoney);
            Destroy(gameObject);
        }

        if (path.Length == 0) return; //還沒到終點

        //往下一個點移動
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex].position, speed * Time.deltaTime);

        if (targetIndex >= path.Length) //終點destroy enemy
        {
            castleScript.damageCastle(enemyDamage);
            Destroy(gameObject);
        }

        //到達轉彎點更新目標
        if (transform.position == path[targetIndex].position)
        {
            ++targetIndex;
            if (targetIndex >= path.Length) //終點destroy enemy
            {
                castleScript.damageCastle(enemyDamage);
                Destroy(gameObject);
            }
        }
    } 
}
