using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Components.Destroy;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerPickUpSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TriggerEnterEntityEvent>> _triggerEnterFilter;

        private readonly EcsPoolInject<AmmoComponent> _ammoPool;
        private readonly EcsPoolInject<PlayerTag> _playerTagPool;
        private readonly EcsPoolInject<AmmoChangedEvent> _ammoChangedPool;
        private readonly EcsPoolInject<PickUpItemComponent> _pickUpItemPool;
        private readonly EcsPoolInject<DespawnEvent> _despawnPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<PickUpAudioComponent> _pickUpAudioPool;
        private readonly EcsPoolInject<AudioEvent> _audioPool;

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

                var (collectEntity, itemEntity) = EntitiesUnpackUtility.Unpack(
                    triggerEnterEvent.FirstPackedEntity, triggerEnterEvent.SecondPackedEntity, _world.Value);

                if (!_ammoPool.Value.Has(collectEntity) || !_playerTagPool.Value.Has(collectEntity)) return;
                if (!_pickUpItemPool.Value.Has(itemEntity)) return;

                ref var itemComponent = ref _pickUpItemPool.Value.Get(itemEntity);
                var itemCount = itemComponent.Count;

                if (itemCount == 0) return;

                var playerAmmoItemID = _shared.Value.PlayerConfig.AmmoItemID;

                if (itemComponent.ID != playerAmmoItemID) continue;

                AddAmmo(collectEntity, itemCount);
                PlayAudio(itemEntity);
                DestroyItem(itemEntity);
            }
        }

        private void AddAmmo(int collectEntity, int itemCount)
        {
            ref var ammoComponent = ref _ammoPool.Value.Get(collectEntity);
            ammoComponent.AmmoCount += itemCount;
            _ammoChangedPool.Value.Add(collectEntity);
        }

        private void PlayAudio(int itemEntity)
        {
            if (!_pickUpAudioPool.Value.Has(itemEntity)) return;

            _audioPool.Value.Add(itemEntity).Clip = _pickUpAudioPool.Value.Get(itemEntity).Clip;
        }

        private void DestroyItem(int itemEntity)
        {
            if (!_ecsMonoObjectPool.Value.Has(itemEntity)) return;

            var itemGO = _ecsMonoObjectPool.Value.Get(itemEntity).EcsMonoObject.gameObject;
            _despawnPool.Value.Add(itemEntity).GameObject = itemGO;
        }
    }
}