using ExpensesFramework.Core.Contracts;
using ExpensesFramework.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Model
{
	public class SummaryModel
	{
		private string _connection;
		private MongoClient _client;

		private Configuration _config;

		public event TransactionAddedEventHandler TransactionAdded;
		public event TransactionUpdatedEventHandler TransactionUpdated;
		public event TransactionRemovedEventHandler TransactionRemoved;
		public delegate void TransactionAddedEventHandler(ITransaction transaction);
		public delegate void TransactionUpdatedEventHandler(ITransaction transaction);
		public delegate void TransactionRemovedEventHandler(ITransaction transaction);

		public SummaryModel()
		{
			_config = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
			_connection = _config.AppSettings.Settings["mongoConnectionString"].Value;

			_client = new MongoClient(_connection);

			BsonClassMap.RegisterClassMap<Income>();
			BsonClassMap.RegisterClassMap<Expense>();
		}

		public IEnumerable<ITransaction> Get(Expression<Func<TransactionBase, bool>> expression)
		{
			var collection = GetCollection();

			return collection.AsQueryable<TransactionBase>().Where(expression);
		}

		public void SaveTransaction(ITransaction transaction)
		{
			//Save on MongoDB
			var collection = GetCollection();

			collection.Insert(transaction);

			OnTransactionAdded(transaction);
		}

		public void UpdateTransaction(ITransaction transaction)
		{
			var collection = GetCollection();
			var update = Update.Replace<ITransaction>(transaction);
			var query = Query<TransactionBase>.EQ(x=>x.Id, transaction.Id);

			collection.Update(query, update);

			OnTransactionUpdated(transaction);
		}

		public void DeleteTransaction(ITransaction transaction)
		{
			//Remove from MongoDB
			var collection = GetCollection();

			var query = Query<TransactionBase>.EQ(x =>x.Id, transaction.Id);
			var result = collection.Remove(query);

			OnTransactionRemoved(transaction);
		}

		private MongoCollection<ITransaction> GetCollection()
		{
			return _client.GetServer()
							.GetDatabase("MongoExpenses")
							.GetCollection<ITransaction>("transactions");
		}

		private void OnTransactionAdded(ITransaction transaction)
		{
			if (TransactionAdded != null)
				TransactionAdded(transaction);
		}

		private void OnTransactionUpdated(ITransaction transaction)
		{
			if (TransactionUpdated != null)
				TransactionUpdated(transaction);
		}

		private void OnTransactionRemoved(ITransaction transaction)
		{
			if (TransactionRemoved != null)
				TransactionRemoved(transaction);
		}
	}
}
