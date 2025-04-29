using UnityEngine;

public class castle : MonoBehaviour
{
    [SerializeField] private int health;

    void Start()
    {

    }

    void Update()
    {
        if(health<=0) Destroy(gameObject);

    }

    public int GetHealth()
    {
        return health;
    }

    public void damageCastle(int damage)
    {
        health -= damage;
    }

}
