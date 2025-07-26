namespace _project.Scripts.LeoECS.Services
{
    public interface ITimeService
    {
        float DeltaTime { get; }

        void SetDeltaTIme(float deltaTime);
    }

    public class TimeService : ITimeService
    {
        public float DeltaTime { get; private set; }

        public void SetDeltaTIme(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}