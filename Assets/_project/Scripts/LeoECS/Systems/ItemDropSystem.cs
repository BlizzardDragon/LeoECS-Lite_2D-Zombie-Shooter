using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class ItemDropSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<DropComponent, DestroyEvent>> _dropFilter;

        private readonly EcsPoolInject<PickUpItemComponent> _pickUpItemPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;

        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _dropFilter.Value.GetRawEntities();
            var count = _dropFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var dropSourceEntity = entities[i];

                var dropComponent = _dropFilter.Pools.Inc1.Get(dropSourceEntity);
                var prefab = _shared.Value.ItemConfigProvider.GetConfig(dropComponent.ID).Prefab;
                var spawnPosition = _dropFilter.Pools.Inc2.Get(dropSourceEntity).GameObject.transform.position;

                var item =
                    Object.Instantiate(prefab, spawnPosition, Quaternion.identity, _shared.Value.ItemsContainer);

                var itemEntity = _world.Value.NewEntity();
                item.Init(itemEntity, _world.Value);
                
                _ecsMonoObjectPool.Value.Add(itemEntity).EcsMonoObject = item;
                
                ref var pickUpItemComponent = ref _pickUpItemPool.Value.Add(itemEntity);
                pickUpItemComponent.ID = dropComponent.ID;
                pickUpItemComponent.Count = dropComponent.Count;
            }
        }
    }
}