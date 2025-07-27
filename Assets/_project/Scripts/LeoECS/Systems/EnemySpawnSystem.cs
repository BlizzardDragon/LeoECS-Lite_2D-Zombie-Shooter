using _project.Scripts.Configs;
using _project.Scripts.Configs.Enemies;
using _project.Scripts.LeoECS.Components;
using _project.Scripts.LeoECS.Components.Audio;
using _project.Scripts.LeoECS.Components.Tags;
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

        private readonly EcsPoolInject<TransformComponent> _transformPool;
        private readonly EcsPoolInject<MoveDirectionComponent> _moveDirectionPool;
        private readonly EcsPoolInject<HealthComponent> _healthPool;
        private readonly EcsPoolInject<DamageComponent> _damagePool;
        private readonly EcsPoolInject<TargetFollowComponent> _targetFollowPool;
        private readonly EcsPoolInject<TeamComponent> _teamPool;
        private readonly EcsPoolInject<EcsMonoObjectComponent> _ecsMonoObjectPool;
        private readonly EcsPoolInject<DropComponent> _dropPool;
        private readonly EcsPoolInject<HitAudioComponent> _hitAudioPool;
        private readonly EcsPoolInject<AttackAudioComponent> _attackAudioPool;
        private readonly EcsPoolInject<DeathAudioComponent> _deathAudioPool;

        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsSharedInject<SharedData> _shared;
        private readonly EcsWorldInject _world;

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
                var enemyConfig = _shared.Value.EnemyConfigProvider.GetRandomConfig();

                var enemy = Spawn(playerEntity, enemyConfig);
                Init(enemy, enemyConfig, playerEntity);
            }
        }

        private EcsMonoObject Spawn(int playerEntity, EnemyConfig enemyConfig)
        {
            ref var moveDirectionComponent = ref _playerFilter.Pools.Inc2.Get(playerEntity);

            var xPositionCamera = _shared.Value.MainCamera.transform.position.x;
            var xPosition = xPositionCamera + _spawnConfig.SpawnDistance * moveDirectionComponent.LastMoveDirection;
            var spawnPosition = new Vector3(xPosition, 0, 0);
            var enemyContainer = _shared.Value.EnemyContainer;

            var enemy =
                Object.Instantiate(enemyConfig.Prefab, spawnPosition, Quaternion.identity, enemyContainer);
            return enemy;
        }

        private void Init(EcsMonoObject enemy, EnemyConfig enemyConfig, int playerEntity)
        {
            var enemyEntity = _world.Value.NewEntity();
            enemy.Init(enemyEntity, _world.Value);

            _teamPool.Value.Add(enemyEntity).Team = TeamType.EnemyTeam;
            _ecsMonoObjectPool.Value.Add(enemyEntity).EcsMonoObject = enemy;
            _transformPool.Value.Add(enemyEntity).Transform = enemy.transform;
            _moveDirectionPool.Value.Add(enemyEntity).MoveSpeed = enemyConfig.MoveSpeed;
            _damagePool.Value.Add(enemyEntity).Damage = enemyConfig.Damage;
            _targetFollowPool.Value.Add(enemyEntity).Target = _playerFilter.Pools.Inc3.Get(playerEntity).Transform;
            _hitAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipHit;
            _attackAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipAttack;
            _deathAudioPool.Value.Add(enemyEntity).Clip = enemyConfig.AudioClipDeath;

            ref var healthComponent = ref _healthPool.Value.Add(enemyEntity);
            healthComponent.MaxHealth = enemyConfig.Health;
            healthComponent.CurrentHealth = enemyConfig.Health;

            ref var dropComponent = ref _dropPool.Value.Add(enemyEntity);
            dropComponent.ID = enemyConfig.ItemDrop.ID;
            dropComponent.Count = Random.Range(enemyConfig.MinDrop, enemyConfig.MaxDrop + 1);

            if (enemy.TryGetComponent<ComponentsContainer>(out var componentsContainer))
            {
                var converters = componentsContainer.Converters;

                for (int i = 0; i < converters.Length; i++)
                {
                    var converter = converters[i];
                    converter.Convert(_world.Value.PackEntityWithWorld(enemyEntity));
                }
            }
        }
    }
}