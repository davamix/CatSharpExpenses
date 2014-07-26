using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Contracts
{
	public interface ITransactionFactory
	{
		ITransactionFactory SetDate(DateTime date);
		ITransactionFactory SetAmount(double amount);
		ITransactionFactory SetRemark(string remark);
		ITransaction Create();
	}
}
