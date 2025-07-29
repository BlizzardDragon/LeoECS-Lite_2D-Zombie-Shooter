using _project.Scripts.Configs;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Despawn;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Components.Tags;
using _project.Scripts.LeoECS.Factories;
using _project.Scripts.LeoECS.Mono;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class BulletSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<SpawnBulletEvent, MoveDirectionComponent, TeamComponent>> _spawnBulletPool;

        private readonly EcsCustomInject<IBulletSpawnFactory> _bulletSpawnFactory;

        public void Run(IEcsSystems systems)
        {
            var entities = _spawnBulletPool.Value.GetRawEntities();
            var count = _spawnBulletPool.Value.GetEntitiesCount();

            for (int i = 0; i < count; i++)
            {
                var sourceEntity = entities[i];
                var lastMoveDirection = _spawnBulletPool.Pools.Inc2.Get(sourceEntity).LastMoveDirection;
                var spawnPosition = _spawnBulletPool.Pools.Inc1.Get(sourceEntity).ShootPointSource.position;
                var team = _spawnBulletPool.Pools.Inc3.Get(sourceEntity).Team;

                _bulletSpawnFactory.Value.Spawn(team, lastMoveDirection, spawnPosition);
            }
        }
    }
}