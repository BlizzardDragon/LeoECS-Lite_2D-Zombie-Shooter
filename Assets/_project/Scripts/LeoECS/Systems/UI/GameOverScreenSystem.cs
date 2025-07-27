using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace _project.Scripts.LeoECS.Systems.UI
{
    public class GameOverScreenSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<GameOverEvent>> _gameOverFilter;
        private readonly EcsFilterInject<Inc<GameOverScreenViewComponent>> _viewFilter;
        
        public void Run(IEcsSystems systems)
        {
            if (_gameOverFilter.Value.GetEntitiesCount() > 0)
            {
                var entities = _viewFilter.Value.GetRawEntities();
                var count = _viewFilter.Value.GetEntitiesCount();

                for (int i = 0; i < count; i++)
                {
                    var entity = entities[i];
                    
                    _viewFilter.Pools.Inc1.Get(entity).View.Value.Enable(true);
                }
            }
        }
    }
}