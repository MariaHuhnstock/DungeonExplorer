namespace DungeonExplorerV5.UI.Combat
{
    public enum CombatChoice
    {
        Attack,
        Potion,
        Run
    }

    public static class CombatInput
    {
        public static CombatChoice GetChoice()
        {
            Console.WriteLine("(A) Attack   (H) Potion   (R) Run");

            while (true)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.NumPad1:
                        return CombatChoice.Attack;

                    case ConsoleKey.H:
                    case ConsoleKey.NumPad2:
                        return CombatChoice.Potion;

                    case ConsoleKey.R:
                    case ConsoleKey.NumPad3:
                        return CombatChoice.Run;
                }
            }
        }
    }
}
