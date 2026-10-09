
using Expenses.API.DTOs;
using Expenses.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Expenses.API.Data.Services
{

    public interface ITransactionsService
    {
        List<Transaction> GetAll(int userId);
        Transaction? GetById(int id);
        Transaction Add(PostTransactionDto transaction, int userId);
        Transaction? Update(int id, PutTransactionDto transaction);
        void Delete(int id);
    }

    public class TransactionsService(AppDbContext context) : ITransactionsService
    {
        public Transaction Add(PostTransactionDto transaction, int userId)
        {
            var newTransaction = new Transaction
            {
                Type = transaction.Type,
                Amount = transaction.Amount,
                Category = transaction.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };

            context.Transactions.Add(newTransaction);
            context.SaveChanges();
            return newTransaction;
        }

        public void Delete(int id)
        {
            var transaction = context.Transactions.FirstOrDefault(t => t.Id == id);
            if(transaction != null)
            {
                context.Transactions.Remove(transaction);
                context.SaveChanges();
            }
            
        }

        public List<Transaction> GetAll(int userId)
        {
            var transactions = context.Transactions.Where(item => item.UserId == userId).ToList();
            return transactions;
        }

        public Transaction? GetById(int id)
        {
            var transaction = context.Transactions.FirstOrDefault(t => t.Id == id);
            return transaction;
        }

        public Transaction? Update(int id, PutTransactionDto transaction)
        {
            var existingTransaction = context.Transactions.FirstOrDefault(t => t.Id == id);
            if(existingTransaction != null)
            {
                existingTransaction.Type = transaction.Type;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Category = transaction.Category;
                existingTransaction.UpdatedAt = DateTime.UtcNow;

                context.Transactions.Update(existingTransaction);
                context.SaveChanges();
            }
            return existingTransaction;
        }
    }
}
