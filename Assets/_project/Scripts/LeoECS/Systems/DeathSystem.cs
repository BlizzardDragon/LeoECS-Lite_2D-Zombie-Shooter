using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class DeathSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthComponent, EcsMonoObjectComponent>> _healthPool;

        private readonly EcsPoolInject<DestroyEvent> _destroyPool;

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

                    _destroyPool.Value.Add(entity).GameObject =
                        _healthPool.Pools.Inc2.Get(entity).EcsMonoObject.gameObject;
                }
            }
        }
    }
}