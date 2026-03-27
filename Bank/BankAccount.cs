namespace BankAccountNS
{
    /// <summary>
    /// Bank account demo class.
    /// </summary>
    
    public class BankAccount
    {
        public const string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public const string DebitAmountLessThanZeroMessage = "Debit amount is less than zero";
        private readonly string m_customerName;
        private double m_balance;

        private BankAccount() { }

        public BankAccount(string customerName, double balance)
        {
            m_customerName = customerName;
            m_balance = balance;
        }

        public string CustomerName
        {
            get { return m_customerName; }
        }

        public double Balance
        {
            get { return m_balance; }
        }

        public void Withdraw(double amount)
        {
            if (amount > m_balance)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountExceedsBalanceMessage);
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }

            m_balance = Math.Round(m_balance - amount, 2);
        }

        public void Deposit(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }

            m_balance = Math.Round(m_balance + amount, 2);
        }

        public void Display()
        {
            Console.WriteLine($"Customer: {m_customerName}\nBalance: {m_balance:C2}");
        }

        public static void Transfer(BankAccount origin, BankAccount destination, double amount, string? note)
        {
            if (origin == null)
            {
                throw new ArgumentNullException(nameof(origin));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            try
            {
                origin.Withdraw(amount);
                destination.Deposit(amount);
                Console.WriteLine($"Transferred {amount:C2} from {origin.CustomerName} to {destination.CustomerName}. Note: {note}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException("Transfer failed. See inner exception for details.", ex);
            }
        }

        public static void Main()
        {
            Random rng = new Random();
            List<BankAccount> Bank = new List<BankAccount>();
            double maxBalance = 0;
            double maxTransferBalance = 0;
            double minBalance = 9223372036854775807;
            double minTransferBalance = 9223372036854775807;
            double nextBalance = 0;
            
            for (int i = 0; i < 1000000; i++)
            {
                nextBalance = 100 + rng.NextInt64(i + 1) * rng.NextInt64(i + 1) + ((double)rng.Next(100) / 100);
                Bank.Add(new BankAccount($"Account #{i + 1}", nextBalance));
            }

            foreach (BankAccount ba in Bank)
            {
                ba.Display();
                Console.WriteLine();
            }

            foreach (BankAccount ba in Bank)
            {
                if (ba.Balance > maxBalance)
                {
                    maxBalance = ba.Balance;
                }

                if (ba.Balance < minBalance)
                {
                    minBalance = ba.Balance;
                }
            }

            for (int i = 0; i < Bank.Count; i++)
            {
                BankAccount destination = Bank[rng.Next(Bank.Count - 1)];
                Transfer(Bank[i], destination, (double)rng.NextInt64((long)Bank[i].Balance - 1) + ((double)rng.Next(100) / 100), "Transfer successful!\n");
                Bank[i].Display();
                destination.Display();
                Console.WriteLine();
            }

            foreach (BankAccount ba in Bank)
            {
                if (ba.Balance > maxTransferBalance)
                {
                    maxTransferBalance = ba.Balance;
                }

                if (ba.Balance < minTransferBalance)
                {
                    minTransferBalance = ba.Balance;
                }
            }

            Console.WriteLine($"Max Balance: {maxBalance:C2}");
            Console.WriteLine($"Min Balance: {minBalance:C2}");
            Console.WriteLine($"Max Transfer Balance: {maxTransferBalance:C2}");
            Console.WriteLine($"Min Transfer Balance: {minTransferBalance:C2}");

            //foreach (BankAccount ba in Bank)
            //{
            //    ba.Display();
            //    Console.WriteLine();
            //}
        }
    }
}