using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.Tags;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.Player
{
    public class PlayerAmmoGameOverSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, AmmoComponent>> _playerFilter;

        private readonly EcsPoolInject<GameOverEvent> _gameOverPool;

        public void Run(IEcsSystems systems)
        {
            if (!_playerFilter.TryGetSingleEntity(out var entity)) return;
            if (_gameOverPool.Value.Has(entity)) return;

            if (_playerFilter.Pools.Inc2.Get(entity).AmmoCount <= 0)
            {
                _gameOverPool.Value.Add(entity);
            }
        }
    }
}