using _project.Scripts.LeoECS.Components.Input;
using _project.Scripts.LeoECS.Services;
using _project.Scripts.LeoECS.Systems;
using _project.Scripts.LeoECS.Systems.Input;
using _project.Scripts.LeoECS.Systems.Player;
using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using UnityEngine;

namespace _project.Scripts.LeoECS
{
    class Startup : MonoBehaviour
    {
        [SerializeField] private SharedData _sharedData;

        EcsWorld _world;
        IEcsSystems _systems;

        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new TimeSystem())
                // ===== Init    
                .Add(new InputInitSystem())
                .Add(new PlayerInitSystem())
                // ===== Input    
                .Add(new InputMouseSystem())
                .Add(new PlayerMoveInputSystem())
                // ===== Move    
                .Add(new MoveDirectionSystem())
                // ===== Animations    
                .Add(new MoveFlipAnimationSystem())
                .Add(new AnimationSystem())

                .ConvertScene()
                .Inject((ITimeService) new TimeService())
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}