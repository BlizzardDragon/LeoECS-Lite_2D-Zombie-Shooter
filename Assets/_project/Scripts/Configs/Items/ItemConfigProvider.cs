using System;
using UnityEngine;

namespace _project.Scripts.Configs.Items
{
    public interface IItemConfigProvider
    {
        ItemConfig GetConfig(int ID);
    }
    
    [CreateAssetMenu(
        fileName = "ItemConfigProvider",
        menuName = "Config/Items/ItemConfigProvider",
        order = 0)]
    public class ItemConfigProvider : ScriptableObject, IItemConfigProvider
    {
        [SerializeField] private ItemConfig[] _itemConfigs;

        public ItemConfig GetConfig(int ID)
        {
            for (int i = 0; i < _itemConfigs.Length; i++)
            {
                var itemConfig = _itemConfigs[i];
                if (ID == itemConfig.ID)
                {
                    return itemConfig;
                }
            }
            
            throw new Exception($"Item with id {ID} not found!");
        }
    }
}