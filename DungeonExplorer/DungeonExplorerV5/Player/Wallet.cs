namespace DungeonExplorerV5.Core
{
    public class Wallet
    {
        public int Coins { get; private set; } = 0;

        public void AddCoins(int amount)
        {
            if (amount > 0)
                Coins += amount;
        }

        public bool SpendCoins(int amount)
        {
            if (amount <= 0)
                return false;

            if (Coins < amount)
                return false;

            Coins -= amount;
            return true;
        }
    }
}
