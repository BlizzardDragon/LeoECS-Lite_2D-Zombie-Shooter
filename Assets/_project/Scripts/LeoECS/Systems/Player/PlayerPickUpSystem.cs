using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerPickUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TriggerEnterEntityEvent>> _triggerEnterFilter;

        private readonly EcsPoolInject<AmmoComponent> _ammoPool;
        private readonly EcsPoolInject<PlayerTag> _playerTagPool;
        private readonly EcsPoolInject<AmmoChangedEvent> _ammoChangedPool;
        private readonly EcsPoolInject<PickUpItemComponent> _pickUpItemPool;
        private readonly EcsPoolInject<DestroyEvent> _destroyPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;

        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _triggerEnterFilter.Value.GetRawEntities();
            var count = _triggerEnterFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];
                ref var triggerEnterEvent = ref _triggerEnterFilter.Pools.Inc1.Get(entity);

                var (firstEntity, secondEntity) = EntitiesUnpackUtility.Unpack(
                    triggerEnterEvent.FirstPackedEntity, triggerEnterEvent.SecondPackedEntity, _world.Value);

                if (!_ammoPool.Value.Has(firstEntity) || !_playerTagPool.Value.Has(firstEntity)) return;
                if (!_pickUpItemPool.Value.Has(secondEntity)) return;

                ref var itemComponent = ref _pickUpItemPool.Value.Get(secondEntity);
                var itemCount = itemComponent.Count;

                if (itemCount == 0) return;

                var playerAmmoItemID = _shared.Value.PlayerConfig.AmmoItemID;

                if (itemComponent.ID == playerAmmoItemID)
                {
                    ref var ammoComponent = ref _ammoPool.Value.Get(firstEntity);
                    ammoComponent.AmmoCount += itemCount;
                    _ammoChangedPool.Value.Add(firstEntity);

                    if (_ecsMonoObjectPool.Value.Has(secondEntity))
                    {
                        var itemGameObject = _ecsMonoObjectPool.Value.Get(secondEntity).EcsMonoObject.gameObject;
                        _destroyPool.Value.Add(secondEntity).GameObject = itemGameObject;
                    }
                }
            }
        }
    }
}