using System;
using _project.Scripts.Configs;
using _project.Scripts.Configs.Enemies;
using _project.Scripts.Configs.Items;
using TNRD;
using UnityEngine;

namespace _project.Scripts.LeoECS
{
    [Serializable]
    public class SharedData
    {
        public static string GameplayGroupName = "Gameplay";
        
        [SerializeField] private SerializableInterface<IEnemyConfigProvider> _enemyConfigProvider;
        [SerializeField] private SerializableInterface<IItemConfigProvider> _itemConfigProvider;

        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Transform BulletContainer { get; private set; }
        [field: SerializeField] public Transform EnemyContainer { get; private set; }
        [field: SerializeField] public Transform ItemsContainer { get; private set; }
        [field: SerializeField] public PlayerConfig PlayerConfig { get; private set; }
        [field: SerializeField] public BulletConfig BulletConfig { get; private set; }

        public IEnemyConfigProvider EnemyConfigProvider => _enemyConfigProvider.Value;
        public IItemConfigProvider ItemConfigProvider => _itemConfigProvider.Value;
    }
}