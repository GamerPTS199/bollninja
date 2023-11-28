using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject FruitPrefab1;
    public GameObject FruitPrefab2;
    public GameObject FruitPrefab3;
    public GameObject FruitPrefab4;
    public GameObject FruitPrefab5;

    public float MinDelay = 0.5f;
    public float MaxDelay = 1.5f;
    public float AngleRangeZ = 20;
    public float LifeTime = 7f;
    public float MinForce = 10f;
    public float MaxForce = 20f;

    private float _currentDelay = 0;

    private Collider _spawnZone;

    public GameObject BombPrefab;
    public float FruitWeight = 1f;
    public float MinBombWeight = 0.1f;
    public float MaxBombWeight = 0.25f;
    public float HeartWeight = 0.02f;
    public float SandClocksWeight = 0.04f;

    private bool _isActive = true;
    public DifficultyChanger DifficultyChanger;
    public GameObject HeartPrefab;
    public GameObject SandClocksPrefab;

    private void Start()
    {
        FillComponents();
        SetNewDelay();
    }

    private void FillComponents()
    {
        _spawnZone = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        MoveDelay();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 pos;
        pos.x = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
        pos.y = Random.Range(_spawnZone.bounds.min.y, _spawnZone.bounds.max.y);
        pos.z = Random.Range(_spawnZone.bounds.min.z, _spawnZone.bounds.max.z);
        return pos;
    }
    public void Restart()
    {
        // Перезапускаем появление фруктов и бомб
        _isActive = true;
        SetNewDelay();
    }

    private void SpawnFruit()
    {
        GameObject fruitPrefab = GetRandomFruitPrefab();
        SpawnObject(fruitPrefab);
    }

    private void SpawnBomb()
    {
        SpawnObject(BombPrefab);
    }

    private void SpawnObject(GameObject prefab)
    {
        Vector3 startPosition = GetRandomSpawnPosition();
        Quaternion startRotation = Quaternion.Euler(0f, 0f, Random.Range(-AngleRangeZ, AngleRangeZ));

        GameObject newObject = Instantiate(prefab, startPosition, startRotation);
        Destroy(newObject, LifeTime);
        AddForce(newObject);
    }

    private GameObject GetRandomFruitPrefab()
    {
        int r = Random.Range(1, 6);

        switch (r)
        {
            case 1:
                return FruitPrefab1;
            case 2:
                return FruitPrefab2;
            case 3:
                return FruitPrefab3;
            case 4:
                return FruitPrefab4;
            default:
                return FruitPrefab5;
        }
    }

    private void AddForce(GameObject obj)
    {
        float force = Random.Range(MinForce, MaxForce);
        obj.GetComponent<Rigidbody>().AddForce(obj.transform.up * force, ForceMode.Impulse);
    }

    public void Stop()
    {
        _isActive = false;
    }
    private void SetNewDelay()
    {
        // НОВОЕ: рассчитываем задержку с помощью метода CalculateRandomSpawnDelay() из скрипта DifficultyChanger
        _currentDelay = DifficultyChanger.CalculateRandomSpawnDelay(MinDelay, MaxDelay);
    }

    private void MoveDelay()
    {
        _currentDelay -= Time.deltaTime;

        if (_currentDelay < 0)
        {
            // НОВОЕ: получаем префаб объекта на основе его веса
            GameObject prefab = GetPrefabByWeights();

            // НОВОЕ: подбрасываем этот префаб
            SpawnObject(prefab);

            SetNewDelay();
        }
    }
    private GameObject GetPrefabByWeights()
    {
        // Рассчитываем вес бомбы на основе минимального и максимального веса с помощью метода CalculateBombWeight() из скрипта DifficultyChanger. 
        //У вас этот метод может называться CalculateBombChance().
        float bombWeight = DifficultyChanger.CalculateBombWeight(MinBombWeight, MaxBombWeight);

        // Суммируем веса фруктов, бомб, сердец, часов
        float totalWeight = FruitWeight + bombWeight + HeartWeight + SandClocksWeight;

        // Создаём случайное число в диапазоне от 0 до суммарного веса всех объектов
        float random = Random.Range(0, totalWeight);

        // Если случайное число меньше или равно весу бомбы, возвращаем префаб бомбы
        if (random <= bombWeight)
        {
            return BombPrefab;
        }

        // Уменьшаем случайное число на вес бомбы, чтобы проверить следующие условия
        random -= bombWeight;

        // Если случайное число меньше или равно весу сердца, возвращаем префаб сердца
        if (random <= HeartWeight)
        {
            return HeartPrefab;
        }

        // Уменьшаем случайное число на вес сердца, чтобы проверить следующие условия    
        random -= HeartWeight;

        // Если случайное число меньше или равно весу часов, возвращаем префаб часов
        if (random <= SandClocksWeight)
        {
            return SandClocksPrefab;
        }

        // Уменьшаем случайное число на вес часов, чтобы проверить следующие условия     
        random -= SandClocksWeight;

        // Если ни одно из условий не выполнено, возвращаем случайный префаб фрукта
        return GetRandomFruitPrefab();
    }
}