using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Components.Destroy;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.Tags;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class DeathSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthComponent, EcsMonoObjectComponent>> _healthPool;

        private readonly EcsPoolInject<PoolObjectTag> _poolObjectTagPool;
        private readonly EcsPoolInject<DespawnEvent> _despawnPool;
        private readonly EcsPoolInject<DestroyEvent> _destroyPool;
        private readonly EcsPoolInject<DeathAudioComponent> _deathAudioPool;
        private readonly EcsPoolInject<AudioEvent> _audioPool;

        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _healthPool.Value.GetRawEntities();
            var count = _healthPool.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                ref var healthComponent = ref _healthPool.Pools.Inc1.Get(entity);

                if (healthComponent.CurrentHealth <= 0)
                {
                    var gameObject = _healthPool.Pools.Inc2.Get(entity).EcsMonoObject.gameObject;

                    if (_poolObjectTagPool.Value.Has(entity))
                    {
                        if (_despawnPool.Value.Has(entity)) return;

                        _despawnPool.Value.Add(entity).GameObject = gameObject;
                    }
                    else
                    {
                        if (_destroyPool.Value.Has(entity)) return;

                        _destroyPool.Value.Add(entity).GameObject = gameObject;
                    }

                    PlayDeathAudio(entity);
                }
            }
        }

        private void PlayDeathAudio(int entity)
        {
            if (!_deathAudioPool.Value.Has(entity)) return;

            var audioEntity = _world.Value.NewEntity();
            _audioPool.Value.Add(audioEntity).Clip = _deathAudioPool.Value.Get(entity).Clip;
        }
    }
}