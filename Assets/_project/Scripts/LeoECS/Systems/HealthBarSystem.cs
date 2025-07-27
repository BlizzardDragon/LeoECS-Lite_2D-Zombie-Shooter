using System;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.View;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems
{
    public class HealthBarSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthBarViewComponent, HealthComponent>> _healthBarViewFilter;

        public void Run(IEcsSystems systems)
        {
            var entities = _healthBarViewFilter.Value.GetRawEntities();
            var count = _healthBarViewFilter.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];

                ref var healthComponent = ref _healthBarViewFilter.Pools.Inc2.Get(entity);
                var fillAmount = (float) healthComponent.CurrentHealth / healthComponent.MaxHealth;

                ref var healthBarViewComponent = ref _healthBarViewFilter.Pools.Inc1.Get(entity);

                if (Math.Abs(healthBarViewComponent.View.Value.FillAmount - fillAmount) > 0.001f)
                {
                    healthBarViewComponent.View.Value.RenderFillAmount(fillAmount);
                }
            }
        }
    }
}