using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.Pool;

//총 플레이 시간: 15분 (900초)
//시간 경과에 따라 적의 속도, 체력, 수, 출현 빈도가 점진적으로 증가
//웨이브는 총 15단계 존재 누적 시간이 1분이 지날때마다 웨이브 상승
//1~2분 간격으로 엘리트 몬스터 등장
//보스는 매 5분마다 등장 (5:00 / 10:00 / 15:00)
//보스가 등장하면 웨이브 시간은 대기
//보스가 처리되면 웨이브 시간 다시 증가

public class InGameWaveManager : GameObjectSingleton<InGameWaveManager>
{
    private const float ELITE_MONSTER_INTERVAL_INIT = 60f;     // 초기 기본 대기시간 (1분)
    private const float ELITE_MONSTER_INTERVAL_MIN = 30f;       // 랜덤 최소 대기시간 (0.5분)
    private const float ELITE_MONSTER_INTERVAL_MAX = 60f;      // 랜덤 최대 대기시간 (1분)

    private List<InGameEnemy> _enemies = new();
    private int _currentWaveLevel = 1;
    private int _currentSpawnCount = 0;
    private int _totalSpawnCount = 0;
    private Coroutine _currentSpawnCoroutine;   // 현재 실행 중인 SpawnEnemies 코루틴
    private int _enemySpawnId = 0;              //게임이 시작되는 생성되는 적의 고유 아이디
    private float _totalPlayTime = 0f;
    private float _nextEliteMonsterTime = 0f;
    private int _lastBossWaveTime = 0; // 마지막 보스 웨이브 시간 (분 단위)
    private bool _isBossWaveActive = false; // 보스 웨이브 진행 중 여부

    private int[] _bossWaveTimeMinutes = new int[] { 2, 3};

    public List<InGameEnemy> Enemies => _enemies;

    protected override void Awake()
    {
        base.Awake();
        InGameEventHandler.OnLevelChanged += OnLevelChanged;
        InGameEventHandler.OnBossWaveStarted += OnBossWaveFinished;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InGameEventHandler.OnLevelChanged -= OnLevelChanged;
        InGameEventHandler.OnBossWaveStarted -= OnBossWaveFinished;
    }

    public async Task Initialize()
    {
        _currentWaveLevel = 1;
        _enemySpawnId = 0;
        _totalPlayTime = 0f;
        _isBossWaveActive = false;
        _nextEliteMonsterTime = ELITE_MONSTER_INTERVAL_INIT + Random.Range(ELITE_MONSTER_INTERVAL_MIN, ELITE_MONSTER_INTERVAL_MAX);

        await Task.CompletedTask;
    }

    private void OnLevelChanged(int level)
    {
        _currentWaveLevel = level;
        StartSpawn();
    }

    private void OnBossWaveFinished(EnemyData enemyData)
    {
        _isBossWaveActive = false;
        //마지막 보스가 처리되면 게임 종료
        if (_lastBossWaveTime == _bossWaveTimeMinutes.Last())
        {
            InGameManager.Instance.GameOver();
        }
    }

    private void StartSpawn()
    {
        if (!InGameManager.Instance.IsPlaying) return;

        var waveDatas = DataManager.Instance.WaveDataList.Where(data => data.WaveLevel == _currentWaveLevel).ToList();
        if (waveDatas != null && waveDatas.Any())
        {
            if (_currentSpawnCoroutine != null)
            {
                StopCoroutine(_currentSpawnCoroutine);
            }
            _currentSpawnCoroutine = StartCoroutine(SpawnEnemies(waveDatas));
        }
    }

    private void Update()
    {
        if (!InGameManager.Instance.IsPlaying) return;

        // 보스 웨이브가 아닐 때만 웨이브 시간 증가
        if (!_isBossWaveActive)
        {
            _totalPlayTime += Time.deltaTime;
            _nextEliteMonsterTime -= Time.deltaTime;
            //_totalPlayTime 1초 증가할 때마다 현재 게임 시간 호출
            if (Time.frameCount % 60 == 0)
            {
                InGameEventHandler.InvokeTimeChanged(_totalPlayTime);
            }
        }

        // 엘리트 몬스터 등장 체크 (보스 웨이브 중에는 스킵)
        if (!_isBossWaveActive && _nextEliteMonsterTime <= 0f)
        {
            _nextEliteMonsterTime = Random.Range(ELITE_MONSTER_INTERVAL_MIN, ELITE_MONSTER_INTERVAL_MAX);
            SpawnEliteMonster();
        }

        // 보스 웨이브 체크
        int currentMinute = Mathf.FloorToInt(_totalPlayTime / 60f);
        if (_bossWaveTimeMinutes.Contains(currentMinute) && currentMinute != _lastBossWaveTime)
        {
            _lastBossWaveTime = currentMinute;
            _isBossWaveActive = true;
            SpawnBossWave();
        }
    }

    public int CurrentWave => _currentWaveLevel;

    //현재 웨이브의 진행률 리턴
    public float GetCurrentWaveProgress()
    {
        if (_totalSpawnCount <= 0)
        {
            return 0f;
        }
        return (float)_currentSpawnCount / (float)_totalSpawnCount;
    }

    public void StopWave()
    {
        //전체 적 제거
        foreach (var enemy in _enemies)
        {
            enemy.Stop();
        }
        _enemies.Clear();
        _totalPlayTime = 0f;

        if (_currentSpawnCoroutine != null)
        {
            StopCoroutine(_currentSpawnCoroutine);
            _currentSpawnCoroutine = null;
        }
    }

    private IEnumerator SpawnEnemies(List<WaveMetaData> waveDatas)
    {
        float spawnInterval = waveDatas.First().SpawnInterval;

        while (true)
        {
            WaveMetaData selectedWaveData = GetRandomWaveData(waveDatas);
            int spawnCount = selectedWaveData.SpawnCount;

            for (int i = 0; i < spawnCount; i++)
            {
                int randomEnemyId = selectedWaveData.SpawnId;
                StartCoroutine(SpawnEnemyCoroutine(randomEnemyId));
                _currentSpawnCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnEnemyCoroutine(int enemyId)
    {
        float distance = Constants.ENEMY_SPAWN_DISTANCE;
        float randomAngleRadians = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 spawnPosition = InGameManager.Instance.Planet.transform.position +
            new Vector3(Mathf.Cos(randomAngleRadians), Mathf.Sin(randomAngleRadians), 0) * distance;

        var enemyTask = AddressableManager.Instance.GetEnemy(enemyId, spawnPosition, transform);
        yield return new WaitUntil(() => enemyTask.IsCompleted);
        var enemy = enemyTask.Result;

        EnemyData enemyData = new EnemyData(DataManager.Instance.EnemyDataList.Find(data => data.EnemyId == enemyId), _currentWaveLevel);
        enemy.Initialize(enemyData, _enemySpawnId);
        _enemies.Add(enemy);
        _enemySpawnId++;

        if (enemyData.MetaData.EnemyType == EnemyType.Boss)
        {
            InGameEventHandler.InvokeBossWaveStarted(enemyData);
        }
    }

    private WaveMetaData GetRandomWaveData(List<WaveMetaData> waveDatas)
    {
        float totalRate = waveDatas.Sum(data => data.SpawnRate);
        float randomValue = UnityEngine.Random.Range(0f, totalRate);
        float currentSum = 0f;

        foreach (var waveData in waveDatas)
        {
            currentSum += waveData.SpawnRate;
            if (randomValue <= currentSum)
            {
                return waveData;
            }
        }

        return null;
    }

    public void RemoveEnemy(InGameEnemy enemy)
    {
        _enemies.Remove(enemy);
    }

    private void SpawnEliteMonster()
    {
        // 엘리트 몬스터 ID는 일반 몬스터 ID보다 큰 값으로 설정
        int eliteMonsterId = DataManager.Instance.EnemyDataList
            .Where(data => data.EnemyType == EnemyType.Elite)
            .OrderBy(x => Random.value)
            .FirstOrDefault()?.EnemyId ?? 0;

        if (eliteMonsterId > 0)
        {
            StartCoroutine(SpawnEnemyCoroutine(eliteMonsterId));
            InGameEventHandler.InvokeEliteMonsterSpawned(_currentWaveLevel);
        }
    }

    private void SpawnBossWave()
    {
        int bossMonsterId = DataManager.Instance.EnemyDataList
            .Where(data => data.EnemyType == EnemyType.Boss)
            .OrderBy(x => Random.value)
            .FirstOrDefault()?.EnemyId ?? 0;

        if (bossMonsterId > 0)
        {
            StartCoroutine(SpawnEnemyCoroutine(bossMonsterId));
        }
    }
}
