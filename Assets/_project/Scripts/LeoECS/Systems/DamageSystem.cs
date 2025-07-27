using _project.Scripts.Configs;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class DamageSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<TriggerEnterEntityEvent>> _triggerEnterFilter;

        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<DamageComponent> _damagePool;
        private readonly EcsPoolInject<HealthComponent> _healthPool;
        private readonly EcsPoolInject<AttackAudioComponent> _attackAudioPool;
        private readonly EcsPoolInject<HitAudioComponent> _hitAudioPool;
        private readonly EcsPoolInject<AudioEvent> _audioPool;

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

                if (!_teamPool.Value.Has(firstEntity) || !_teamPool.Value.Has(secondEntity)) return;
                if (_teamPool.Value.Get(firstEntity).Team == _teamPool.Value.Get(secondEntity).Team) return;

                if (_damagePool.Value.Has(firstEntity))
                {
                    if (_healthPool.Value.Has(secondEntity))
                    {
                        ref var firstDamageComponent = ref _damagePool.Value.Get(firstEntity);
                        ref var secondHealthComponent = ref _healthPool.Value.Get(secondEntity);

                        secondHealthComponent.CurrentHealth -= firstDamageComponent.Damage;

                        PlayAudio(firstEntity, secondEntity);
                    }
                }
            }
        }

        private void PlayAudio(int firstEntity, int secondEntity)
        {
            var audioEntity = _world.Value.NewEntity();

            if (_attackAudioPool.Value.Has(firstEntity))
            {
                _audioPool.Value.Add(audioEntity).Clip = _attackAudioPool.Value.Get(firstEntity).Clip;
            }

            if (_hitAudioPool.Value.Has(secondEntity))
            {
                _audioPool.Value.Add(audioEntity).Clip = _hitAudioPool.Value.Get(secondEntity).Clip;
            }
        }
    }
}