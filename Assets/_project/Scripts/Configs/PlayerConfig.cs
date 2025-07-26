using UnityEngine;

namespace _project.Scripts.Configs
{
    [CreateAssetMenu(
        fileName = "PlayerConfig",
        menuName = "Config/PlayerConfig",
        order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
    }
}