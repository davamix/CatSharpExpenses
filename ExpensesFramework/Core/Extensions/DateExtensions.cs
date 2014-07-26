using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesFramework.Core.Extensions
{
	public static class DateExtensions
	{
		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		public static DateTime LastDayOfMonth(this DateTime date)
		{
			var nextMonth = new DateTime(date.Year, date.Month, 1);
			
			return nextMonth.AddMonths(1).AddDays(-1);
		}
	}
}
