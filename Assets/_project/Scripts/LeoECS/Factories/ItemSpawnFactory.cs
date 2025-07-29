using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Mono;
using EasyPoolKit;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Factories
{
    public interface IItemSpawnFactory
    {
        void Spawn(int id, int count, Vector3 position);
    }

    public class ItemSpawnFactory : IItemSpawnFactory, IEcsSystem
    {
        private readonly EcsPoolInject<PickUpItemComponent> _pickUpItemPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<PickUpAudioComponent> _pickUpAudioPool;
        private readonly EcsPoolInject<PoolObjectTag> _poolObjectTagPool;

        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

        public void Spawn(int id, int count, Vector3 position)
        {
            var itemConfig = _shared.Value.ItemConfigProvider.GetConfig(id);

            var itemGO = SimpleGOPoolKit.Instance.SimpleSpawn(itemConfig.Prefab.gameObject);
            itemGO.transform.SetParent(_shared.Value.ItemsContainer);
            itemGO.transform.position = position;

            if (!itemGO.TryGetComponent<EcsMonoObject>(out var ecsMonoObject))
            {
                Debug.LogError($"Component EcsMonoObject not found!");
                return;
            }

            var itemEntity = _world.Value.NewEntity();
            ecsMonoObject.Init(itemEntity, _world.Value);

            _ecsMonoObjectPool.Value.Add(itemEntity).EcsMonoObject = ecsMonoObject;
            _pickUpAudioPool.Value.Add(itemEntity).Clip = itemConfig.AudioClipPickUp;
            _poolObjectTagPool.Value.Add(itemEntity);

            ref var pickUpItemComponent = ref _pickUpItemPool.Value.Add(itemEntity);
            pickUpItemComponent.ID = id;
            pickUpItemComponent.Count = count;
        }
    }
}