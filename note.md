
# Unity 學習筆記

## 物件與組件（Component）
- Unity 裡的東西都是由**物件**組成。
- 每個物件包含了不同的**組件**，組件可以想成是把物件內的 **member** 和 **method** 做一些分類。
- 組件（Component）也有自己的 **method** 和 **member** 可以呼叫。

## Transform 組件
- 每個物件都有 **Transform 組件**，就算是空物件也一定會附加上 Transform。
- 有 Transform 才能夠改變物件的位置、旋轉和縮放。
- 常用範例：
  ```csharp
  transform.position = new Vector3(0, 0, 0); // 設定位置
  transform.rotation = Quaternion.Euler(0, 90, 0); // 旋轉90度
  transform.localScale = new Vector3(2, 2, 2); // 物件放大2倍
  ```

## Script 的基本架構
- 把 Script 拖到物件上，這個 Script 就會變成該物件的一個組件。
- Script 中 **public** 的變數可以在 Unity UI 裡直接修改，方便設計時操作。

### 基本範例
```csharp
using UnityEngine;

public class xxxx : MonoBehaviour // 繼承 MonoBehaviour
{
    void Start() // 定義起始狀態
    {
        
    }

    void Update() // 定義每一幀更新
    {
        
    }
}
```

---



### 繼承 MonoBehaviour
- MonoBehaviour 包含了 Unity 的一些內建函式和 script（像是 Start()、Update() ），加上 MonoBehaviour 類似是讓他套上 Unity 有的一些基本操作。
- 如果沒有繼承他就只是一個普通的 C# file ， Unity 不會主動呼叫它的 Start 或 Update，也不能被加到物件上當作組件使用。

### Time.deltaTime
- `Time.deltaTime` 是 "從上一幀到這一幀花的時間"。
- 不同電腦、不同時間下，遊戲每秒的幀數（FPS）可能會不同。
- 為了讓移動速度不受 FPS 影響，可以把移動量去乘上 `Time.deltaTime`，這樣不管幀數高低，物件移動速度都是一樣的。
- ex：
  ```csharp
  transform.position += new Vector3(1, 0, 0) * 5f * Time.deltaTime;
  ```
  就是每秒移動 5，而不是每幀移動 5。

---

# PlayerMovement.cs

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // 左右鍵盤輸入 (-1, 1)
        transform.position += new Vector3(moveX, 0, 0) * 5f * Time.deltaTime; // 移動角色
    }
}
```

- 這裡的 `transform.position` 是一個 **Vector3**。
- 建立新的 Vector3 要記得加 `new`。

( 這裡只是簡單做個物件動看看 後面沒用到player )

---




# Enemy.cs
```csharp
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f; // enemy speed
    public Transform[] path; // 儲存路徑轉彎點

    private int targetIndex = 0; // 當前目標點

    void Update()
    {
        if (path.Length == 0) return; // 沒有設定路徑直接跳過

        // 往目標點移動
        transform.position = Vector3.MoveTowards(transform.position, path[targetIndex].position, speed * Time.deltaTime);

        // 到達目標點就更新下一個目標
        if (transform.position == path[targetIndex].position)
        {
            ++targetIndex;
            if (targetIndex >= path.Length)
            {
                Destroy(gameObject); // 到達終點，銷毀自己
            }
        }
    }
}
```

### 說明
- 用一個 `Transform[] path` 陣列來儲存整條路徑的轉彎點。
- `MoveTowards` 是 `Vector3` 的內建方法：
  ```csharp
  public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);
  ```
  - `current`: 目前位置。
  - `target`: 目標位置。
  - `maxDistanceDelta`: 每次可以移動的最大距離。

- `Destroy(gameObject);`
  - `gameObject` 是指自己這個物件的 reference。
  - 有點像 C++ 的 `this`，但 C# 沒有指標，只有 reference，所以直接用 `.` 呼叫。

---

## EnemySpawner.cs
```csharp
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy; // 要生成的敵人
    public Transform spawnPoint; // 出生位置
    public float spawnTime = 4f; // 生成間隔時間
    private float timer; // 計時器

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
```

### 說明
- 透過一個計時器累積時間。
- timer 每到 `spawnTime` 秒，就呼叫 `SpawnEnemy()` 產生一個新的敵人。
- `Instantiate()` 可以 clone Prefab 或現有的 object 。

---


## Prefab 概念
- **Prefab** 可以想成是 C# 的 **Class**，從這個 Class 可以隨時生成新的 **Instance**。
- Prefab 好處：
  - 適合用來生成大量一樣的物件。
  - 避免場景中原本的物件被 Destroy 後無法再生成的問題。
  - 像這裡的 spawner 如果是用 UI 把 scene 上的 enemy 拉進來而不是用 prefeb 的，那個 enemy 被 Destroy 後 enemyspawner 就會找不到 enemy。
  - 方便管理和修改，改一個 Prefab，scene 上面的物件都會跟著更新。


## Instantiate
```csharp
public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
```
- `original`: 要複製的 Object 或 Prefab。
- `position`: 新物件的位置。
- `rotation`: 新物件的旋轉角度。

（簡單來說，就是 Clone 一個新的，但原本的 Prefab 不會被影響）

---





### 在 EnemyMovement 中加入血量與死亡系統

```csharp
public int initial_health;
[SerializeField] private int health;

void Start()
{
    health = initial_health;
}

void Damage(int damage)
{
    health -= damage;
}
```

- `initial_health`：設為公開變數，在 Unity Inspector 中可以修改初始血量。
- `[SerializeField]`：`private` 加上 `[SerializeField]` 可以在 Inspector 中看到和修改，但不能從外部(其他 script )修改。可以想成讓 Inspector 可以修改的同時保護變數不被其他物件動到。
- `Damage(int)`：對外提供扣血的函式。

```csharp
void Update()
{
    if (health <= 0)
    {
        Destroy(gameObject);
    }
}
```

- 如果血量歸 0 就會 destroy enemy。

---



---

## towerType1：定時範圍攻擊塔

這是一種每隔一段時間，對圓形範圍內敵人造成傷害的塔。



### towerType1.cs

```csharp
using UnityEngine;

public class towerType1 : MonoBehaviour
{
    [SerializeField] private int damage = 10; // 傷害值
    private float timer; // 計時器
    private float attack_time = 1f; // 攻擊間隔
    [SerializeField] private float range; // 攻擊範圍半徑

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attack_time)
        {
            Attack();
            timer = 0f;
        }
    }

    void Attack()
    {
        // 找出範圍內所有的碰撞體
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D e in hit)
        {
            if (e.gameObject == gameObject) continue; // 排除自己
            Debug.Log("Hit object: " + e.name + ", Tag: " + e.tag);
            if (e.CompareTag("enemy"))
            {
                Debug.Log("find enemy");
                e.GetComponent<EnemyMovement>().Damage(damage); // 扣血
            }
        }
    }
}
```




### 碰撞檢測 - OverlapCircleAll
- 用 `Physics2D.OverlapCircleAll` 來偵測範圍內所有 Collider2D。
- 語法（只先用到前兩個參數）：
  
  ```csharp
  Collider2D[] hit = Physics2D.OverlapCircleAll(Vector2 point, float radius);
  ```
  - `point`：圓心，用 `transform.position`。
  - `radius`：半徑，這裡是 `range`。
  - 回傳是一個陣列，裡面是所有碰到的 `Collider2D`。

### 排除自己
```csharp
if (e.gameObject == gameObject) continue;
```
因為 OverlapCircleAll 也會抓到自己，要記得排除自己。

### 用 tag 判斷是不是敵人
```csharp
if (e.CompareTag("enemy"))
```
- 用 `CompareTag` 判斷物件的 tag，這個 tag 可以在 Inspector 裡設。
- 如果是敵人，才去叫用他的 `EnemyMovement.Damage()`。

####  Debug 技巧
```csharp
Debug.Log("Hit object: " + e.name + ", Tag: " + e.tag);
```
- 可以把log訊息輸出到 Console 。
- 開啟 Console 的方式：
  - 點左上 `Window` → `General` → `Console`
  - 或是用快捷鍵 `Ctrl + Shift + C`

---





## towerType2：射子彈的塔 + bullet 子彈物件

這種塔的目標是**定時偵測範圍內敵人並發射子彈**。



###  bullet.cs

```csharp
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float speed;
    private float maxRange;
    private Vector3 direction;
    private Vector3 spawnPoint;
    private int damage;

    void Start()
    {
        
    }

    public void SetUp(Vector3 dir, float range, float getSpeed, int getDamage)
    {
        direction = dir.normalized; // 轉成單位向量
        maxRange = range;
        speed = getSpeed;
        spawnPoint = transform.position;
        damage = getDamage;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // 子彈移動

        if (Vector3.Distance(spawnPoint, transform.position) > maxRange) // 超出射程自爆
        {
            Destroy(gameObject);
        }
    }
}
```

---


- `SetUp()`：讓塔可以設定子彈的**方向**、**射程**、**速度**和**傷害**。
- 移動方式是：
  ```csharp
  transform.Translate(direction * speed * Time.deltaTime);
  ```
  方向是單位向量，乘上速度跟 deltaTime 讓移動更平滑。
- 如果飛太遠（超過 `maxRange`），就直接 `Destroy` 自己，避免沒射中敵人子彈物件數量過多的浪費。

---

### towerType2.cs

```csharp
using UnityEngine;

public class towerType2 : MonoBehaviour
{
    [SerializeField] private int range = 5;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private GameObject bullet;
    private float timer;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D e in hit)
        {
            if (e.gameObject == gameObject) continue; // 排除自己

            if (e.CompareTag("enemy"))
            {
                Vector3 direction = (e.transform.position - transform.position).normalized;
                GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
                bullet1.GetComponent<bullet>().SetUp(direction, range, bulletSpeed, damage);
                break; // 只射一個敵人
            }
        }
    }
}
```

---



###  為什麼這裡要有 `[SerializeField] private GameObject bullet;`？
- 因為塔需要知道是哪個 bullet prefab，才能呼叫 `.SetUp()`。
- 不像前面的 `enemySpawner` 那樣只是單純生成敵人，這邊塔生成 bullet 後還要設定方向、速度、傷害，所以要特別 serialize。

### 子彈方向的算法
```csharp
Vector3 direction = (e.transform.position - transform.position);
```
- 從自己指向敵人的向量，然後 `.normalized` 轉成單位向量，這樣移動速度才會正確，不會因為距離改變。

###  一次只射一個敵人
- 加 `break;`，找到第一個敵人就停，比較省計算。

> （如果想一次射多個敵人，可以自己加個 `count`，控制生成幾顆子彈。）

---



---

# 子彈與敵人互動

## 一開始的作法

在子彈的 `Update()` 中，每一幀都檢查周圍是否有敵人：

```csharp
Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);

foreach (Collider2D e in hit)
{
    if (e.gameObject == gameObject) continue; // 略過自己
    if (e.CompareTag("enemy"))
    {
        Debug.Log("hit enemy");
        e.GetComponent<EnemyMovement>().Damage(damage);
        Destroy(gameObject);
    }
}
```

製作上遇到的問題：
- 因為子彈數量可能很多，每一顆子彈每一幀都檢查，導致效能很差，電腦容易變燙。
- 即使加入 `timer` 限制檢查間隔（checkInterval），效能問題仍未完全解決。

---

## 改進：使用 Trigger 觸發碰撞

將子彈改成 **由 Unity 的物理引擎自動處理碰撞觸發**，只在真正發生碰撞時才做處理。

### 步驟

1. **子彈的 Collider2D 打勾 `IsTrigger`**。
2. **子彈加上 Rigidbody2D**
   - `Body Type` 設為 `Kinematic`（靜態，不受重力影響）。
3. **修改子彈的腳本**
   - 取消原本定時檢查的邏輯。
   - 改用 `OnTriggerEnter2D()`，只有碰到敵人時才做傷害判定。

```csharp
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("enemy"))
    {
        Debug.Log("bullet hit enemy");
        collision.GetComponent<EnemyMovement>().Damage(damage);
        Destroy(gameObject);
    }
}
```

### 說明
- Rigidbody2D 的觸發模式只會在從「未碰撞」變成「碰撞」那一刻呼叫 `OnTriggerEnter2D`，而不是每一幀都呼叫，因此不會有大量效能浪費。
- `collision` 參數就是撞到的物件。
- 可以想成原本是每一幀都自己檢查是不是有撞到人，變成了撞到了會有人來叫我們，效能會好很多。

---

## 修改 towerType2，配合新碰撞方式

再來想要試試看讓塔也做類似的優化，改成使用 Trigger 並控制射擊間隔。

```csharp
using UnityEngine;

public class towerType2 : MonoBehaviour
{
    [SerializeField] private int range = 5;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private GameObject bullet;
    private float timer;
    private float interval = 1f;
    private float interval_timer;

    void Start()
    {
        timer = 0f;
        interval_timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        interval_timer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (interval_timer >= interval) // 每隔一段時間才檢查一次
        {
            interval_timer = 0f;
            if (timer >= attackSpeed)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
                {
                    Vector3 direction = (collision.transform.position - transform.position).normalized;

                    GameObject bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
                    bullet1.GetComponent<bullet>().SetUp(direction, range, bulletSpeed, damage);

                    timer = 0f;
                }
            }
        }
    }
}
```

---



### Layer vs Tag
  - Layer 的判斷是在碰撞系統底層就過濾掉，效能更好。
  - Tag 判斷是碰到之後才進行比較，負擔比較大。
  - 可以想成用Layer是根本撞不到，用Tag則是撞到以後再去判斷是誰。

### 設定 Layer Collision Matrix 
  - 要設定哪些 Layer 可以彼此碰撞，可以在以下路徑設定：
  
    ```
    左上角 Edit → Project Settings → Physics 2D → Layer Collision Matrix
    ```

  - 透過取消不必要的 Layer 碰撞，可以減少碰撞運算來提高效能。

### interval_timer
  - `OnTriggerStay2D` 是在碰撞期間每一幀都呼叫的。
  - 如果不加 interval，會導致攻擊行為無限次呼叫。
  - 加上 interval_timer 可以控制每段時間才進行一次攻擊判定，大幅降低負擔。

---


## 塔放置

### TowerManager.cs

```csharp
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject towerType1Prefab;
    public GameObject towerType2Prefab;
    private GameObject currentTowerPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            currentTowerPrefab = towerType1Prefab;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))  
        {
            currentTowerPrefab = towerType2Prefab;
        }

        if (currentTowerPrefab != null && Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;  // 確認 z 座標為 0
            Instantiate(currentTowerPrefab, worldPosition, Quaternion.identity);
        }
    }
}
```

- 用鍵盤數字鍵 **1** 或 **2** 選擇要放置的塔。
- 用滑鼠左鍵選擇位置放置。
- `Input.GetKeyDown(按鍵)`：偵測按下特定鍵盤按鍵的瞬間。
- `Input.GetMouseButtonDown(0)`：偵測滑鼠左鍵按下的瞬間。（0是左鍵、1是中鍵、2是右鍵）

---

## enemy spawner隨時間強化

再來想做到enemy會隨時間增加，改成由EnemySpawner決定生成enemy的血量和速度。

### EnemyMovement 改動

```csharp
public void SetHealth(int newHealth)
{
    health = newHealth;
}

public void SetSpeed(float newSpeed)
{
    speed = newSpeed;
}
```
- 提供給 `EnemySpawner` ，調整敵人的血量和速度。

---

### EnemySpawner.cs

```csharp
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
    private int level_up = 5;

    void Start()
    {
        timer = 0f;
        count = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
        count++;
        if (count >= level_up)
        {
            count = 0;
            health += healthIncreaseRate;
            speed += speedIncreaseRate;
        }
        newEnemy.GetComponent<EnemyMovement>().SetHealth(health);
        newEnemy.GetComponent<EnemyMovement>().SetSpeed(speed);
    }
}
```

- `count`：計算已經生成幾個敵人。
- 每當生成 `level_up` 數量的敵人後，血量 (`health`) 和速度 (`speed`) 都會提升。
- 透過呼叫 `SetHealth()` 和 `SetSpeed()` 設定新敵人的強度。

---






# 金錢系統和購買

## MoneyManager.cs

```csharp
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 100;  // 初始金額
    [SerializeField] private TextMeshProUGUI moneyText; 

    void Update()
    {
        moneyText.text = "Money: " + currentMoney.ToString();
    }

    public bool SpendMoney(int amount) // 花錢
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount) // 加錢
    {
        currentMoney += amount;
    }
}
```

- 顯示金錢 UI 使用 `TextMeshProUGUI`。
- `SpendMoney()`：如果錢夠就扣錢並回傳 `true`。
- `AddMoney()`：可讓敵人死亡後掉錢。

---

## 修改 TowerManager：放塔時會花錢

```csharp
[SerializeField] private MoneyManager moneyManager;
public int towerCost = 50;

if (currentTowerPrefab != null && Input.GetMouseButtonDown(0))
{
    if (moneyManager.SpendMoney(towerCost)) // 檢查錢夠不夠
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        Instantiate(currentTowerPrefab, worldPosition, Quaternion.identity);
    }
    else
    {
        Debug.Log("Not enough money!");
    }
}
```


## 小發現：Prefab 不能直接拉入場景物件

- Unity 的 `SerializeField` 和 `public` 都可以讓我們在 Inspector 拉其他物件進來。
- 但 prefab 是「還沒出現在場景上的模板」，**不能從 Inspector 拖場景物件進去**。
- 解法是：在script裡面使用 `FindObjectOfType<>()`等函式去找場景上已經存在的物件。

```csharp
private MoneyManager moneyManager;
void Start()
{
    moneyManager = FindObjectOfType<MoneyManager>();
}
```

- 這裡不一定需要改，也可以從inspector直接拉物件是因為moneyManager遊戲中只有一個，不一定需要用prefeb來做。

---

## 敵人死亡掉錢

### EnemyMovement 新增

```csharp
[SerializeField] private int rewardMoney = 10;
private MoneyManager moneyManager;

void Start()
{
    moneyManager = FindFirstObjectByType<MoneyManager>();
}
```

### 在扣血後判斷死亡並加錢

```csharp
if (health <= 0)
{
    moneyManager.AddMoney(rewardMoney);
    Destroy(gameObject);
}
```



### `FindFirstObjectByType<T>()` vs `GameObject.Find()`

- `FindFirstObjectByType<MoneyManager>()`：  
  根據 `script` 找場上的物件，大部分情況用這種比較好。

- `GameObject.Find("名字").GetComponent<>()`：  
  根據場景`物件名稱`找物件，如果物件名稱變動就會錯。

---
