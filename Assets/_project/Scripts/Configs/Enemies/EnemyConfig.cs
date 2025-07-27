using _project.Scripts.LeoECS.Mono;
using UnityEngine;

namespace _project.Scripts.Configs.Enemies
{
    [CreateAssetMenu(
        fileName = "EnemyConfig",
        menuName = "Config/Enemies/EnemyConfig",
        order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public EcsMonoObject Prefab { get; private set; }
    }
}