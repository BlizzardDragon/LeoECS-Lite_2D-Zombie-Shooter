using _project.Scripts.Configs.Items;
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
        [field: SerializeField] public int MaxDrop { get; private set; }
        [field: SerializeField] public int MinDrop { get; private set; }
        [field: SerializeField] public AudioClip AudioClipAttack { get; private set; }
        [field: SerializeField] public AudioClip AudioClipHit { get; private set; }
        [field: SerializeField] public AudioClip AudioClipDeath { get; private set; }
        [field: SerializeField] public EcsMonoObject Prefab { get; private set; }
        [field: SerializeField] public ItemConfig ItemDrop { get; private set; }
    }
}