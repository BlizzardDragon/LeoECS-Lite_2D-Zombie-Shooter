namespace _project.Scripts.LeoECS.Components
{
    public enum TeamType
    {
        PlayerTeam,
        EnemyTeam,
    }

    public struct TeamComponent
    {
        public TeamType Team;
    }
}