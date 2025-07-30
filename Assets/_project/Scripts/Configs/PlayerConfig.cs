using _project.Scripts.Configs.Items;
using UnityEngine;

namespace _project.Scripts.Configs
{
    [CreateAssetMenu(
        fileName = "PlayerConfig",
        menuName = "Config/PlayerConfig",
        order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private ItemConfig _ammoItem;

        [field: SerializeField] public int StartHealth { get; private set; } = 1;
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int StartAmmoCount { get; private set; } = 50;
        [field: SerializeField] public float ReloadDuration { get; private set; } = 0.1f;

        public int AmmoItemID => _ammoItem.ID;
    }
}