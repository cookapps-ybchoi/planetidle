using UnityEngine;
using System.Threading.Tasks;
using Game.ObjectPool;
using System.Collections;

public class InGameManager : GameObjectSingleton<InGameManager>
{
    private InGamePlanet _planet;

    public InGamePlanet Planet => _planet;

    public bool IsPlaying => _currentState == InGameState.GamePlay;

    private InGameState _currentState = InGameState.None;

    private async void Start()
    {
        // DataManager 인스턴스 대기
        await WaitForInstance(DataManager.Instance);

        // 데이터 초기화
        await DataManager.Instance.Initialize();

        // ObjectPoolManager 인스턴스 대기
        await WaitForInstance(ObjectPoolManager.Instance);

        // ObjectPoolManager 초기화
        await ObjectPoolManager.Instance.Initialize();

        // AddressableManager 인스턴스 대기
        await WaitForInstance(AddressableManager.Instance);

        // AddressableManager 초기화
        await AddressableManager.Instance.Initialize();

        // InGameWaveManager 인스턴스 대기
        await WaitForInstance(InGameWaveManager.Instance);

        // InGameWaveManager 초기화
        await InGameWaveManager.Instance.Initialize();

        // InGameEventManager 인스턴스 대기
        await WaitForInstance(InGameEventManager.Instance);

        // InGameEventManager 초기화
        await InGameEventManager.Instance.Initialize();

        // 미리 로드
        await Preload();
    }

    public void StartGame()
    {
        if (_currentState == InGameState.GamePlay) return;

        // 행성 생성
        StartCoroutine(CreatePlanetAndStartGame());
    }

    private IEnumerator CreatePlanetAndStartGame()
    {
        // 행성 생성 대기
        var task = AddressableManager.Instance.GetPlanet(DataManager.Instance.PlanetData.PlanetId, Vector3.zero, transform);
        yield return new WaitUntil(() => task.IsCompleted);
        _planet = task.Result;

        // 행성이 생성되면 게임 시작
        _currentState = InGameState.GamePlay;
        InGameEventManager.Instance.InvokeGameStateChanged(_currentState);

        InGameWaveManager.Instance.StartWave();
    }

    public void PauseGame()
    {
        if (_currentState != InGameState.GamePlay) return;

        Time.timeScale = 0;

        _currentState = InGameState.GamePause;
        InGameEventManager.Instance.InvokeGameStateChanged(_currentState);
    }

    public void ResumeGame()
    {
        if (_currentState != InGameState.GamePause) return;

        Time.timeScale = 1;

        _currentState = InGameState.GamePlay;
        InGameEventManager.Instance.InvokeGameStateChanged(_currentState);
    }

    public void GameOver()
    {
        _currentState = InGameState.GameOver;
        InGameEventManager.Instance.InvokeGameStateChanged(_currentState);

        InGameWaveManager.Instance.StopWave();
        _planet.Finish();
        _planet = null;

        Debug.Log("GameOver");
    }

    public double GetPlanetAttackPower()
    {
        return DataManager.Instance.PlanetData.GetStatValue(PlanetStatType.AttackPower);
    }

    private async Task Preload()
    {
        InGameEnemy enemy = await AddressableManager.Instance.GetEnemy(1, Vector3.zero, transform);
        enemy.gameObject.SetActive(false);

        InGameBullet bullet = await AddressableManager.Instance.GetBullet(1, Vector3.zero, transform);
        bullet.gameObject.SetActive(false);

        InGameExplosion explosion = await AddressableManager.Instance.GetExplosion(1, Vector3.zero, transform);
        explosion.gameObject.SetActive(false);

        InGameDamage damage = await AddressableManager.Instance.GetDamage(Vector3.zero, transform);
        damage.gameObject.SetActive(false);

        InGamePoint point = await AddressableManager.Instance.GetPoint(Vector3.zero, transform);
        point.gameObject.SetActive(false);
    }

    private async Task WaitForInstance<T>(T instance) where T : class
    {
        while (instance == null) await Task.Yield();
    }
}

