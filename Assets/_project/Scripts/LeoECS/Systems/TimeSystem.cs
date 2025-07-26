using _project.Scripts.LeoECS.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace _project.Scripts.LeoECS.Systems
{
    public class TimeSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            _timeService.Value.SetDeltaTIme(Time.deltaTime);
        }
    }
}