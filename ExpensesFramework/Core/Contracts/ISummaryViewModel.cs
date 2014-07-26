using ExpensesFramework.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Contracts
{
	public interface ISummaryViewModel
	{
		ObservableCollection<ITransaction> Transactions { get; set; }
		ITransaction Current { get; }

		ITransaction GetTransaction(Guid id);
		void GetTransactions(Expression<Func<TransactionBase, bool>> expression);
		void Save(string remark, double amount);
		void Remove(Guid id);
		void SetUpdate(Guid id);
	}
}
