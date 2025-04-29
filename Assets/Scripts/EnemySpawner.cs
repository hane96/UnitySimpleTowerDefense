using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy; 
    public Transform spawnPoint; 
    public float spawnTime = 4f; 
    private float timer; 

    private float speedIncreaseRate = 0.1f; 
    private int healthIncreaseRate = 5; 
    private float speed = 1f; 
    private int health = 10; 
    private int count; 
    private int level_up=5;

    void Start()
    {
        timer = 0f;
        count = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer>=spawnTime) //生成時間到
        {
            SpawnEnemy();
            timer=0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
        count++;
        if(count>=level_up) //每level_up次升級一次
        {
            count=0;
            health += healthIncreaseRate;
            speed += speedIncreaseRate;
        }
        newEnemy.GetComponent<EnemyMovement>().SetHealth(health);
        newEnemy.GetComponent<EnemyMovement>().SetSpeed(speed);
        
    }
}
 