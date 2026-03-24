using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => account.Debit(debitAmount));
        }

        [TestMethod]
        [TestCategory("Logic")]
        public void Debit_WhenAmountExceedsBalance_ShouldThrowArgumentOutOfRange()
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
            catch (System.ArgumentOutOfRangeException e)
            {
                // Assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [TestMethod]
        [TestCategory("Data Validation")]
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
    }
}
