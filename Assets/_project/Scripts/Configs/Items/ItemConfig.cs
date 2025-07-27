using _project.Scripts.LeoECS.Mono;
using UnityEngine;

namespace _project.Scripts.Configs.Items
{
    [CreateAssetMenu(
        fileName = "ItemConfig",
        menuName = "Config/Items/ItemConfig",
        order = 0)]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int ID { get; private set; }
        [field: SerializeField] public EcsMonoObject Prefab { get; private set; }


        private void OnValidate()
        {
            ID = Name.GetHashCode();
        }
    }
}