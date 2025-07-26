using _project.Scripts.LeoECS.Mono;
using UnityEngine;

namespace _project.Scripts.Configs
{
    [CreateAssetMenu(
        fileName = "BulletConfig",
        menuName = "Config/BulletConfig",
        order = 0)]
    public class BulletConfig : ScriptableObject
    {
        [field: SerializeField] public EcsMonoObject Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
    }
}