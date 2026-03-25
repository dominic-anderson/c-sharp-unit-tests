using BankAccountNS;

namespace BankTests
{
    [TestClass]
    public sealed class BankAccountTests
    {
        [TestMethod]
        [TestCategory("Logic")]
        public void Debit_WithValidAmount_UpdatesBalance()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 4.55;
            double expected = 7.44;
            BankAccount account = new BankAccount("Hugh Mann", beginningBalance);

            // Act
            account.Debit(debitAmount);

            // Assert
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account not debited correctly");
        }

        [TestMethod]
        [TestCategory("Data Validation")]
        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.00;
            BankAccount account = new BankAccount("Real Name", beginningBalance);

            // Act and assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => account.Debit(debitAmount));
        }

        [TestMethod]
        [TestCategory("Logic")]
        public void Debit_WhenAmountExceedsBalance_ShouldAllowDebit()
        {
            // Arrange
            double beginningBalance = 11.99;
            double debitAmount = 20.0;
            BankAccount account = new BankAccount("Qwerty U. Iop", beginningBalance);

            // Act
            try
            {
                account.Debit(debitAmount);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // Assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        [TestCategory("Data Validation")]
        [TestCategory("Miscellaneous")]
        public void CheckIfAccountHasName()
        {
            // Arrange
            double beginningBalance = 11.99;
            BankAccount account1 = new BankAccount("Has A. Name", beginningBalance);
            BankAccount account2 = new BankAccount("", beginningBalance);

            // Act and assert
            Assert.IsFalse(string.IsNullOrEmpty(account1.CustomerName) && string.IsNullOrWhiteSpace(account1.CustomerName));
            Assert.IsTrue(string.IsNullOrEmpty(account2.CustomerName) || string.IsNullOrWhiteSpace(account2.CustomerName));
        }

        [TestMethod]
        [TestCategory("Logic")]
        public void Transfer_WhenOriginAccountHasSufficientFunds_ShouldTransferAmount()
        {
            // Arrange
            BankAccount origin = new BankAccount("Origin Account", 100.00);
            BankAccount destination = new BankAccount("Destination Account", 50.00);
            double transferAmount = 30.00;
            double expectedOriginBalance = 70.00;
            double expectedDestinationBalance = 80.00;
            // Act
            BankAccount.Transfer(origin, destination, transferAmount, "Test transfer");
            // Assert
            Assert.AreEqual(expectedOriginBalance, origin.Balance, 0.001, "Origin account balance not updated correctly");
            Assert.AreEqual(expectedDestinationBalance, destination.Balance, 0.001, "Destination account balance not updated correctly");
        }

        [TestMethod]
        [TestCategory("Data Validation")]
        public void Transfer_WhenOriginAccountHasInsufficientFunds_ShouldThrowInvalidOperationException()
        {
            // Arrange
            BankAccount origin = new BankAccount("Origin Account", 20.00);
            BankAccount destination = new BankAccount("Destination Account", 50.00);
            double transferAmount = 30.00;
            // Act and assert
            Assert.ThrowsException<InvalidOperationException>(() => BankAccount.Transfer(origin, destination, transferAmount, "Test transfer"));
        }

        [TestMethod]
        [TestCategory("Miscellaneous")]
        public void Display_ShouldOutputCustomerNameAndBalance()
        {
            // Arrange
            BankAccount account1 = new BankAccount("Test Account", 10.00);
            BankAccount account2 = new BankAccount("Empty Account", 0.00);
            BankAccount account3 = new BankAccount("1 Decimal Account", 20.10);
            BankAccount account4 = new BankAccount("2 Decimal Account", 20.11);
            BankAccount account5 = new BankAccount("Negative Account", -10.00);

            // Act and assert
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                account1.Display();
                string expected1 = $"Customer: {account1.CustomerName}\nBalance: {account1.Balance:C2}";
                Assert.AreEqual(expected1, sw.ToString().Trim());
                sw.GetStringBuilder().Clear();
                account2.Display();
                string expected2 = $"Customer: {account2.CustomerName}\nBalance: {account2.Balance:C2}";
                Assert.AreEqual(expected2, sw.ToString().Trim());
                sw.GetStringBuilder().Clear();
                account3.Display();
                string expected3 = $"Customer: {account3.CustomerName}\nBalance: {account3.Balance:C2}";
                Assert.AreEqual(expected3, sw.ToString().Trim());
                sw.GetStringBuilder().Clear();
                account4.Display();
                string expected4 = $"Customer: {account4.CustomerName}\nBalance: {account4.Balance:C2}";
                Assert.AreEqual(expected4, sw.ToString().Trim());
                sw.GetStringBuilder().Clear();
                account5.Display();
                string expected5 = $"Customer: {account5.CustomerName}\nBalance: {account5.Balance:C2}";
                Assert.AreEqual(expected5, sw.ToString().Trim());
            }
        }

        [TestMethod]
        [TestCategory("Logic")]
        public void Debit_WhenAmountIsExactlyBalance_ShouldSetBalanceToZero()
        {
            // Arrange
            double beginningBalance = 50.00;
            double debitAmount = 50.00;
            BankAccount account = new BankAccount("Exact Balance Account", beginningBalance);
            double expected = 0.00;
            // Act
            account.Debit(debitAmount);
            // Assert
            double actual = account.Balance;
            Assert.AreEqual(expected, actual, 0.001, "Account balance should be zero after debiting the exact amount");
        }
    }
}
