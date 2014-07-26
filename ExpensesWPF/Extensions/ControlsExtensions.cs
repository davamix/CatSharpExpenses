using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ExpensesWPF.Extensions
{
	public static class ControlsExtensions
	{
		public static void Update<T>(this T source, Action<T> func) 
			where T:DispatcherObject
		{
			source.Dispatcher.Invoke(func, source);
		}
	}
}
