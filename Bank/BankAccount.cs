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
            
            for (int i = 0; i < 10; i++)
            {
                Bank.Add(new BankAccount($"Account #{i + 1}", 100 + i * 10 + rng.Next(100)));
            }

            foreach (BankAccount ba in Bank)
            {
                ba.Display();
                Console.WriteLine();
            }
        }
    }
}