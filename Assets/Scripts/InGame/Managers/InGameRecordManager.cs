using UnityEngine;
using System.Threading.Tasks;
public class InGameRecordManager : GameObjectSingleton<InGameRecordManager>
{
    private RecordData _recordData = new RecordData();

    public int TotalEnemiesDestroyed => _recordData.TotalEnemiesDestroyed;
    public int TotalCoinEarned => _recordData.TotalCoinEarned;
    public int TotalPointsEarned => _recordData.TotalPointsEarned;

    protected override void Awake()
    {
        base.Awake();
        InGameEventHandler.OnEnemyDestroyed += OnEnemyDestroyed;
        InGameEventHandler.OnCoinEarned += OnCoinEarned;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InGameEventHandler.OnEnemyDestroyed -= OnEnemyDestroyed;
        InGameEventHandler.OnCoinEarned -= OnCoinEarned;
    }

    public async Task Initialize()
    {
        _recordData.Initialize();
        await Task.CompletedTask;
    }

    public void ResetRecord()
    {
        _recordData.Initialize();
    }

    private void OnEnemyDestroyed(int enemyId, bool isKilled)
    {
        if (isKilled)
        {
            _recordData.RecordEnemyKilled();
            InGameEventHandler.InvokeRecordDataChanged(_recordData);
            // 적 파괴 시 코인 획득
            int coinAmount = 10; // 기본 코인 획득량
            InGameEventHandler.InvokeCoinEarned(coinAmount);
        }
    }

    private void OnCoinEarned(int coin)
    {
        _recordData.RecordCoins(coin);
        InGameEventHandler.InvokeRecordDataChanged(_recordData);
    }
}