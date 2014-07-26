using ExpensesFramework.Core.Contracts;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Entities
{
	public abstract class TransactionBase : ITransaction
	{
		[BsonId]
		public Guid Id { get; private set; }

		public DateTime Date { get; set; }

		public double Amount { get; set; }

		public string Remark { get; set; }

		public TransactionBase()
		{
			this.Id = Guid.NewGuid();
		}

	}
}
