using _project.Scripts.Configs;
using _project.Scripts.Configs.Enemies;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Factories;
using _project.Scripts.LeoECS.Mono;
using _project.Scripts.LeoECS.Services;
using AB_Utility.FromSceneToEntityConverter;
using LeoECS.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerTag, MoveDirectionComponent, TransformComponent>> _playerFilter;

        private readonly EcsCustomInject<IEnemySpawnFactory> _enemySpawnFactory;
        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsSharedInject<SharedData> _shared;

        private EnemySpawnConfig _spawnConfig;
        private float _timer;

        public void Init(IEcsSystems systems)
        {
            _spawnConfig = _shared.Value.EnemyConfigProvider.SpawnConfig;
        }

        public void Run(IEcsSystems systems)
        {
            _timer += _timeService.Value.DeltaTime;

            if (_timer >= _spawnConfig.SpawnPeriod)
            {
                if (!_playerFilter.TryGetSingleEntity(out var playerEntity)) return;

                _timer = 0;

                var target = _playerFilter.Pools.Inc3.Get(playerEntity).Transform;
                
                ref var moveDirectionComponent = ref _playerFilter.Pools.Inc2.Get(playerEntity);
                var xPositionCamera = _shared.Value.MainCamera.transform.position.x;
                var xPosition = xPositionCamera + _spawnConfig.SpawnDistance * moveDirectionComponent.LastMoveDirection;
                var spawnPosition = new Vector3(xPosition, 0, 0);

                _enemySpawnFactory.Value.Spawn(target, spawnPosition);
            }
        }
    }
}