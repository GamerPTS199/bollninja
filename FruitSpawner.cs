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
        // ������������� ��������� ������� � ����
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
        // �����: ������������ �������� � ������� ������ CalculateRandomSpawnDelay() �� ������� DifficultyChanger
        _currentDelay = DifficultyChanger.CalculateRandomSpawnDelay(MinDelay, MaxDelay);
    }

    private void MoveDelay()
    {
        _currentDelay -= Time.deltaTime;

        if (_currentDelay < 0)
        {
            // �����: �������� ������ ������� �� ������ ��� ����
            GameObject prefab = GetPrefabByWeights();

            // �����: ������������ ���� ������
            SpawnObject(prefab);

            SetNewDelay();
        }
    }
    private GameObject GetPrefabByWeights()
    {
        // ������������ ��� ����� �� ������ ������������ � ������������� ���� � ������� ������ CalculateBombWeight() �� ������� DifficultyChanger. 
        //� ��� ���� ����� ����� ���������� CalculateBombChance().
        float bombWeight = DifficultyChanger.CalculateBombWeight(MinBombWeight, MaxBombWeight);

        // ��������� ���� �������, ����, ������, �����
        float totalWeight = FruitWeight + bombWeight + HeartWeight + SandClocksWeight;

        // ������ ��������� ����� � ��������� �� 0 �� ���������� ���� ���� ��������
        float random = Random.Range(0, totalWeight);

        // ���� ��������� ����� ������ ��� ����� ���� �����, ���������� ������ �����
        if (random <= bombWeight)
        {
            return BombPrefab;
        }

        // ��������� ��������� ����� �� ��� �����, ����� ��������� ��������� �������
        random -= bombWeight;

        // ���� ��������� ����� ������ ��� ����� ���� ������, ���������� ������ ������
        if (random <= HeartWeight)
        {
            return HeartPrefab;
        }

        // ��������� ��������� ����� �� ��� ������, ����� ��������� ��������� �������    
        random -= HeartWeight;

        // ���� ��������� ����� ������ ��� ����� ���� �����, ���������� ������ �����
        if (random <= SandClocksWeight)
        {
            return SandClocksPrefab;
        }

        // ��������� ��������� ����� �� ��� �����, ����� ��������� ��������� �������     
        random -= SandClocksWeight;

        // ���� �� ���� �� ������� �� ���������, ���������� ��������� ������ ������
        return GetRandomFruitPrefab();
    }
}