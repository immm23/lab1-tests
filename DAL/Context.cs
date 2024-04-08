using LabOOP2.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LabOOP2.DAL
{
    public class Context : DbContext
    {
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Passport> Passports { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(customer =>
            {
                customer.HasKey(p => p.Id);
                customer.OwnsOne(p => p.Passport, passport =>
                {
                    passport.HasKey(p => p.Id);
                    passport.HasIndex(p => p.SerialNumber).IsUnique();
                });
                customer.OwnsOne(p => p.BankAccount, bankAccount =>
                {
                    bankAccount.HasKey(p => p.Id);
                    bankAccount.HasIndex(p => p.CardNumber).IsUnique();
                });
                customer.OwnsMany(p => p.Transactions, transaction =>
                {
                    transaction.HasKey(p => p.Id);
                });
                customer.OwnsMany(p => p.Loans, loan =>
                {
                    loan.HasKey(p => p.Id);
                });
            });
        }
    }
}