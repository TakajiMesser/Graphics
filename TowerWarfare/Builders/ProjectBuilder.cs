using TowerWarfare.Helpers;

namespace TowerWarfare.Builders
{
    public static class ProjectBuilder
    {
        public static void CreateTestProject()
        {
            BehaviorBuilder.GenerateCameraBehavior(FilePathHelper.CAMERA_BEHAVIOR_PATH);
            //BehaviorBuilder.GeneratePlayerBehavior(FilePathHelper.PLAYER_INPUT_BEHAVIOR_PATH);
            BehaviorBuilder.GenerateEnemyBehavior(FilePathHelper.ENEMY_PATROL_BEHAVIOR_PATH);

            MapBuilder.GenerateTestMap(FilePathHelper.MAP_PATH);
        }
    }
}
