using System;
using _project.Scripts.View;
using TNRD;

namespace _project.Scripts.LeoECS.Components.View
{
    [Serializable]
    public struct HealthBarViewComponent
    {
        public SerializableInterface<IHealthBarView> View;
    }
}