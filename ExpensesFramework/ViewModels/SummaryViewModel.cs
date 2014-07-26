using ExpensesFramework.Core;
using ExpensesFramework.Core.Contracts;
using ExpensesFramework.Core.Entities;
using ExpensesFramework.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace ExpensesFramework.ViewModels
{
	public class SummaryViewModel : ISummaryViewModel, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event QueryLaunchedEventHandler QueryLaunched;
		public event QueryCompletedEventHandler QueryCompleted;
		public event TotalChangedEventHandler TotalChanged;
		public delegate void QueryLaunchedEventHandler(string message);
		public delegate void QueryCompletedEventHandler(string message);
		public delegate void TotalChangedEventHandler(double total);

		private SummaryModel Model { get; set; }
		
		public ITransaction Current { get; private set; }
		
		public bool IsUpdating { get; private set; }

		private double _Total;
		public double Total
		{
			get { return _Total; }
			private set
			{
				_Total = value;
				OnTotalChanged(this, _Total);
			}
		}

		private object _transactionsLock = new Object();

		private ObservableCollection<ITransaction> _Transactions;
		public ObservableCollection<ITransaction> Transactions
		{
			get { return _Transactions; }
			set
			{
				if (_Transactions != value)
				{
					_Transactions = value;
					OnPropertyChanged();
				}
			}
		}

		public SummaryViewModel()
		{
			this.Model = new SummaryModel();
			this.Transactions = new ObservableCollection<ITransaction>();
			BindingOperations.EnableCollectionSynchronization(this.Transactions, _transactionsLock);

			this.Model.TransactionAdded += OnTransactionAdded;
			this.Model.TransactionUpdated += OnTransactionUpdated;
			this.Model.TransactionRemoved += OnTrasactionRemoved;

		}

		public ITransaction GetTransaction(Guid id)
		{
			return this.Transactions.FirstOrDefault(x => x.Id == id);
		}

		public void GetTransactions(Expression<Func<TransactionBase, bool>> expression)
		{
			OnQueryLaunched(this, String.Format("Load data: {0}", expression.Body.ToString()));

			new Task(() =>
			{
				foreach (var t in this.Model.Get(expression))
				{
					this.Transactions.Add(t);
				}

				this.Total = this.Transactions.Sum(x => x.Amount);

				OnQueryCompleted(this, "Data is loaded");
				
			}).Start();
		}

		public void Save(string remark, double amount)
		{
			if (this.IsUpdating)
				UpdateProcess(remark, amount);
			else
				InsertProcess(remark, amount);

			this.IsUpdating = false;
		}

		public void Remove(Guid id)
		{
			Delete(this.Transactions.FirstOrDefault(x => x.Id == id));
		}

		public void SetUpdate(Guid id)
		{
			this.IsUpdating = true;
			this.Current = this.Transactions.FirstOrDefault(x => x.Id == id);
		}

		private void InsertProcess(string remark, double amount)
		{
			if (amount < 0)
				Insert(TransactionFluentFactory.Init<Expense>()
																.SetDate(DateTime.UtcNow)
																.SetRemark(remark)
																.SetAmount(amount)
																.Create());
			else
				Insert(TransactionFluentFactory.Init<Income>()
																.SetDate(DateTime.UtcNow)
																.SetRemark(remark)
																.SetAmount(amount)
																.Create());
		}

		private void UpdateProcess(string remark, double amount)
		{
			this.Current.Amount = amount;
			this.Current.Remark = remark;

			Update(this.Current);
		}

		#region "Model calls"

		private void Insert(ITransaction transaction)
		{
			new Task(() =>
			{
				this.Model.SaveTransaction(transaction);
				this.Total = this.Transactions.Sum(x => x.Amount);
			}).Start();
		}

		private void Update(ITransaction transaction)
		{
			new Task(() =>
			{
				this.Model.UpdateTransaction(transaction);
				this.Total = this.Transactions.Sum(x => x.Amount);
			}).Start();
		}

		private void Delete(ITransaction transaction)
		{
			new Task(() =>
			{
				this.Model.DeleteTransaction(transaction);
				this.Total = this.Transactions.Sum(x => x.Amount);
			}).Start();
		}

		#endregion


		#region "Subscribed events"

		private void OnTransactionAdded(ITransaction transaction)
		{
			this.Transactions.Add(transaction);
		}

		//TODO
		private void OnTransactionUpdated(ITransaction transaction)
		{
			this.Transactions.Remove(this.Transactions.First(x => x.Id == transaction.Id));
			this.Transactions.Add(transaction);
		}

		private void OnTrasactionRemoved(ITransaction transaction)
		{
			this.Transactions.Remove(transaction);
		}

		#endregion

		#region "Events"

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void OnTotalChanged(object sender, double total)
		{
			if (TotalChanged != null)
				TotalChanged(total);
		}
		private void OnQueryLaunched(object sender, string message)
		{
			if (QueryLaunched != null)
				QueryLaunched(message);
		}

		private void OnQueryCompleted(object sender, string message)
		{
			if (QueryCompleted != null)
				QueryCompleted(message);
		}

		#endregion
	}
}
