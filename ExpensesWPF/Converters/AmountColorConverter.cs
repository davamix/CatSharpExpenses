using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ExpensesWPF.Converters
{
	public class AmountColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return new SolidColorBrush(Colors.Black);

			if ((double)value < 0)
				return new SolidColorBrush(Colors.Red); 

			return new SolidColorBrush(Colors.Green);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new SolidColorBrush(Colors.Black);
		}
	}
}
