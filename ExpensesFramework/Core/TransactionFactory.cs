using ExpensesFramework.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core
{
	public class TransactionFactory : ITransactionFactory
	{
		private ITransaction _transaction;

		public TransactionFactory(ITransaction transaction)
		{
			_transaction = transaction;
		}

		public ITransactionFactory SetDate(DateTime date)
		{
			_transaction.Date = date;
			return this;
		}

		public ITransactionFactory SetAmount(double amount)
		{
			_transaction.Amount = amount;
			return this;
		}

		public ITransactionFactory SetRemark(string remark)
		{
			_transaction.Remark = remark;
			return this;
		}

		public ITransaction Create()
		{
			return _transaction;
		}
	}
}
