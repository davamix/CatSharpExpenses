using ExpensesFramework.Core;
using ExpensesFramework.Core.Contracts;
using ExpensesFramework.Core.Entities;
using ExpensesFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExpensesWPF.Extensions;
using ExpensesFramework.Core.Extensions;

namespace ExpensesWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class Summary : Window
	{
		private SummaryViewModel ViewModel { get; set; }

		public Summary()
		{
			InitializeComponent();

			this.ViewModel = new SummaryViewModel();
			this.ViewModel.QueryLaunched += ViewModel_QueryLaunched;
			this.ViewModel.QueryCompleted += ViewModel_QueryCompleted;
			this.ViewModel.TotalChanged += ViewModel_TotalChanged;

			this.lstTransactions.ItemsSource = this.ViewModel.Transactions;
			this.txtTotal.DataContext = this.ViewModel.Transactions;
			

			//LoadData
			this.ViewModel.GetTransactions(x => x.Date >= DateTime.UtcNow.FirstDayOfMonth() && x.Date <= DateTime.UtcNow.LastDayOfMonth());
		}

		

		private void ResetTextBoxes()
		{
			txtAmount.Text = "Amount";
			txtAmount.Style = (Style)this.FindResource("TextBoxHint");
			
			txtRemark.Text = "Remark";
			txtRemark.Style = (Style)this.FindResource("TextBoxHint");
		}


		private void SetTextBoxNormalStyle(TextBox textBox)
		{
			if (textBox.Text.Equals(textBox.Tag))
				textBox.Clear();
			
			textBox.Style = (Style)this.FindResource("TextBoxNormal");
		}
		private void SetTextBoxHintStyle(TextBox textBox)
		{
			if (String.IsNullOrEmpty(textBox.Text))
			{
				textBox.Text = textBox.Tag.ToString();
				textBox.Style = (Style)this.FindResource("TextBoxHint");
			}
		}

		#region "Bottom status info"

		private void UpdateStatus(string message)
		{
			txtStatus.Update<TextBlock>(x => x.Text = message);
		}

		private void UpdateTotal(double total)
		{
			txtTotal.Update<TextBlock>(x => x.Text = total.ToString());
		}

		#endregion

		#region "Events"

		private void TextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			
			if (textBox == null)
				return;

			if ((bool)e.NewValue)
				SetTextBoxNormalStyle(textBox);
			else
				SetTextBoxHintStyle(textBox);
			
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (String.IsNullOrEmpty(txtRemark.Text) || String.IsNullOrEmpty(txtAmount.Text))
				return;

			double amount = 0;

			if(!Double.TryParse(txtAmount.Text, out amount))
				return;

			this.ViewModel.Save(txtRemark.Text, amount);

			ResetTextBoxes();
		}

		private void UpdateItem_Click(object sender, RoutedEventArgs e)
		{
			SetTextBoxNormalStyle(txtRemark);
			SetTextBoxNormalStyle(txtAmount);

			this.ViewModel.SetUpdate(((Guid)(sender as Button).Tag));
			
			txtRemark.Text = this.ViewModel.Current.Remark;
			txtAmount.Text = this.ViewModel.Current.Amount.ToString();
		}

		private void DeleteItem_Click(object sender, RoutedEventArgs e)
		{
			var id=(Guid)((sender as Button).Tag);
			this.ViewModel.Remove(id);
		}

		void ViewModel_QueryLaunched(string message)
		{
			UpdateStatus("Loading data...");
		}

		void ViewModel_QueryCompleted(string message)
		{
			UpdateStatus(string.Empty);
		}

		void ViewModel_TotalChanged(double total)
		{
			UpdateTotal(total);
		}

		#endregion
	}
}
