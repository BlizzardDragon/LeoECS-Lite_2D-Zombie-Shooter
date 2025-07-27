using UnityEngine;

namespace _project.Scripts.Configs.Enemies
{
    public interface IEnemyConfigProvider
    {
        EnemySpawnConfig SpawnConfig { get; }
        EnemyConfig GetRandomConfig();
    }

    [CreateAssetMenu(
        fileName = "EnemyConfigProvider",
        menuName = "Config/Enemies/EnemyConfigProvider",
        order = 0)]
    public class EnemyConfigProvider : ScriptableObject, IEnemyConfigProvider
    {
        [SerializeField] private EnemyConfig[] EnemyConfigs;

        [field: SerializeField] public EnemySpawnConfig SpawnConfig { get; private set; }

        public EnemyConfig GetRandomConfig() => EnemyConfigs[Random.Range(0, EnemyConfigs.Length)];
    }
}