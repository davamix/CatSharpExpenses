using ExpensesFramework.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Contracts
{
	public interface ISummaryModel
	{
		IEnumerable<ITransaction> GetTransaction(Expression<Func<TransactionBase, bool>> expression);
		void SaveTransaction(ITransaction transaction);
		ITransaction UpdateTransaction(ITransaction transaction);
		void DeleteTransaction(ITransaction transaction);
	}
}
