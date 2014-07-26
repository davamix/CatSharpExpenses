using ExpensesFramework.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core
{
	public static class TransactionFluentFactory
	{
		public static ITransactionFactory Init<T>() 
			where T:ITransaction, new()
			
		{
			return new TransactionFactory(new T());
		}
	}
}
