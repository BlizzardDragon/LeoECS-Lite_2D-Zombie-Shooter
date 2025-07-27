using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Components.UI;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.UI
{
    public class PlayerAmmoViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<AmmoComponent, PlayerTag>> _playerFilter;
        private readonly EcsFilterInject<Inc<AmmoViewComponent>> _ammoViewFilter;

        private readonly EcsPoolInject<AmmoChangedEvent> _ammoChangedPool;

        public void Init(IEcsSystems systems)
        {
            if (!_playerFilter.TryGetSingleEntity(out var playerEntity)) return;
            UpdateView(playerEntity);
        }

        public void Run(IEcsSystems systems)
        {
            if (!_playerFilter.TryGetSingleEntity(out var playerEntity)) return;
            if (!_ammoChangedPool.Value.Has(playerEntity)) return;

            UpdateView(playerEntity);
        }

        private void UpdateView(int playerEntity)
        {
            if (!_ammoViewFilter.TryGetSingleEntity(out var viewEntity)) return;

            var ammoCount = _playerFilter.Pools.Inc1.Get(playerEntity).AmmoCount;
            _ammoViewFilter.Pools.Inc1.Get(viewEntity).View.RenderCount(ammoCount.ToString());
        }
    }
}