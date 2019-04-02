namespace SampleGameProject.Helpers.Builders
{
    public static class ProjectBuilder
    {
        public static void CreateTestProject()
        {
            BehaviorBuilder.GeneratePlayerBehavior(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
            BehaviorBuilder.GenerateEnemyBehavior(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);

            MapBuilder.GenerateTestMap(FilePathHelper.MAP_PATH);
        }
    }
}
