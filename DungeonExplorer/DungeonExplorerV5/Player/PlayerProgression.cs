namespace DungeonExplorerV5.Core
{
    public class PlayerProgression
    {
        public int Level { get; private set; } = 1;
        public int Experience { get; private set; } = 0;

        public int XPToNextLevel => 100 + (Level - 1) * 50;

        public void AddXP(int amount)
        {
            Experience += amount;

            while (Experience >= XPToNextLevel)
            {
                Experience -= XPToNextLevel;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Level++;
        }
    }
}
