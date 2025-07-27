using System;
using _project.Scripts.UI;
using _project.Scripts.View;
using TNRD;

namespace _project.Scripts.LeoECS.Components.UI
{
    [Serializable]
    public struct GameOverScreenViewComponent
    {
        public SerializableInterface<IScreenView> View;
    }
}