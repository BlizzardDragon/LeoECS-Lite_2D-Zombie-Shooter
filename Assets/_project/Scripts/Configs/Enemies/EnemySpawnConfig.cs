using UnityEngine;

namespace _project.Scripts.Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "EnemySpawnConfig",
        menuName = "Config/Enemies/EnemySpawnConfig",
        order = 0)]
    public class EnemySpawnConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnPeriod { get; private set; }
        [field: SerializeField] public float SpawnDistance { get; private set; }
    }
}