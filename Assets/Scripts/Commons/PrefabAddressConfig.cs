using UnityEngine;

[CreateAssetMenu(fileName = "AddressablePrefabConfig", menuName = "Configs/PrefabAddressConfig")]
public class PrefabAddressConfig : ScriptableObject
{
    public string InGamePlanet = "InGame/Planet";
    public string InGameEnemy  = "InGame/Enemy";
    public string InGameBullet = "InGame/Bullet";
}
