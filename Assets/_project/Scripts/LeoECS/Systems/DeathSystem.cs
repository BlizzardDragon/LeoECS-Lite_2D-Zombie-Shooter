using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class DeathSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthComponent, EcsMonoObjectComponent>> _healthPool;

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
                    if (_destroyPool.Value.Has(entity)) return;

                    var gameObject = _healthPool.Pools.Inc2.Get(entity).EcsMonoObject.gameObject;
                    _destroyPool.Value.Add(entity).GameObject = gameObject;

                    PlayAudio(entity);
                }
            }
        }

        private void PlayAudio(int entity)
        {
            if (!_deathAudioPool.Value.Has(entity)) return;

            var audioEntity = _world.Value.NewEntity();
            _audioPool.Value.Add(audioEntity).Clip = _deathAudioPool.Value.Get(entity).Clip;
        }
    }
}