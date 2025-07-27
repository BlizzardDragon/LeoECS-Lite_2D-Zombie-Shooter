using _project.Scripts.Configs;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Events;
using _project.Scripts.LeoECS.Mono;
using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class BulletSpawnSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ShootAnimationEvent, MoveDirectionComponent, TeamComponent>>
            _shootAnimationPool;

        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<DamageComponent> _damagePool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<DestroyWithDelayComponent> _destroyWithDelayPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;

        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsWorldInject _world;

        public void Run(IEcsSystems systems)
        {
            var entities = _shootAnimationPool.Value.GetRawEntities();
            var count = _shootAnimationPool.Value.GetEntitiesCount();
            var bulletConfig = _shared.Value.BulletConfig;

            for (int i = 0; i < count; i++)
            {
                var sourceEntity = entities[i];
                var lastMoveDirection = _shootAnimationPool.Pools.Inc2.Get(sourceEntity).LastMoveDirection;

                var bullet = SpawnBullet(sourceEntity, lastMoveDirection, bulletConfig);
                Init(sourceEntity, bullet, bulletConfig, lastMoveDirection);
            }
        }

        private EcsMonoObject SpawnBullet(int sourceEntity, int lastMoveDirection, BulletConfig bulletConfig)
        {
            var prefab = bulletConfig.Prefab;
            var spawnPosition = _shootAnimationPool.Pools.Inc1.Get(sourceEntity).ShootPoint.position;
            var spawnContainer = _shared.Value.BulletContainer;

            var bullet = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, spawnContainer);

            bullet.transform.localScale = new Vector3(lastMoveDirection, 1, 1);
            return bullet;
        }

        private void Init(int sourceEntity, EcsMonoObject bullet, BulletConfig bulletConfig, int lastMoveDirection)
        {
            var entity = _world.Value.NewEntity();
            bullet.Init(entity, _world.Value);

            _ecsMonoObjectPool.Value.Add(entity).EcsMonoObject = bullet;
            _teamPool.Value.Add(entity).Team = _shootAnimationPool.Pools.Inc3.Get(sourceEntity).Team;
            _damagePool.Value.Add(entity).Damage = bulletConfig.Damage;
            _transformPool.Value.Add(entity).Transform = bullet.transform;

            ref var moveDirectionComponent = ref _shootAnimationPool.Pools.Inc2.Add(entity);
            moveDirectionComponent.MoveSpeed = bulletConfig.MoveSpeed;
            moveDirectionComponent.MoveDirection = lastMoveDirection;

            ref var destroyWithDelayComponent = ref _destroyWithDelayPool.Value.Add(entity);
            destroyWithDelayComponent.Delay = bulletConfig.DestroyDelay;
            destroyWithDelayComponent.GameObject = bullet.gameObject;
            destroyWithDelayComponent.StartTime = _timeService.Value.CurrentTime;
        }
    }
}