using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Contracts
{
	public interface ITransaction
	{
		[BsonId]
		Guid Id { get; }
		DateTime Date { get; set; }
		double Amount { get; set; }
		string Remark { get; set; }

	}
}
