namespace _project.Scripts.LeoECS.Services
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        float CurrentTime { get; }

        void SetDeltaTIme(float deltaTime);
        void SetCurrentTIme(float currentTime);
    }

    public class TimeService : ITimeService
    {
        public float DeltaTime { get; private set; }
        public float CurrentTime { get; private set; }

        public void SetDeltaTIme(float deltaTime) => DeltaTime = deltaTime;
        public void SetCurrentTIme(float currentTime) => CurrentTime = currentTime;
    }
}