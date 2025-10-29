using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace TaskTimerWidget.Helpers
{
    /// <summary>
    /// Converts task active state to background color (for card backgrounds).
    /// </summary>
    public class TaskColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is bool isActive && isActive)
            {
                return new SolidColorBrush(Microsoft.UI.Colors.Gold);
            }
            return new SolidColorBrush(Microsoft.UI.Colors.LightGray);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts IsActive state to text color (Black if active, White if inactive).
    /// </summary>
    public class RunningTextColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is bool isActive && isActive)
            {
                return new SolidColorBrush(Microsoft.UI.Colors.Black);
            }
            return new SolidColorBrush(Microsoft.UI.Colors.White);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts IsRunning state to readable string.
    /// </summary>
    public class IsRunningConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is bool isRunning)
            {
                return isRunning ? "⏱️ Running..." : "⏸️ Paused";
            }
            return "⏸️ Paused";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts empty task list to visibility.
    /// </summary>
    public class EmptyStateConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is int count && count == 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts error message presence to visibility.
    /// </summary>
    public class ErrorVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is string message && !string.IsNullOrEmpty(message))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Inverse of ErrorVisibilityConverter for success state.
    /// </summary>
    public class SuccessVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is string message && string.IsNullOrEmpty(message))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts task count to formatted string.
    /// </summary>
    public class TaskCountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is int count)
            {
                return $"{count} task{(count != 1 ? "s" : "")}";
            }
            return "0 tasks";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts boolean to Visibility (true=Visible, false=Collapsed).
    /// Supports ConverterParameter for inversion (any value inverts the logic).
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is bool boolValue)
            {
                // If parameter is provided, invert the logic
                bool shouldBeVisible = parameter != null ? !boolValue : boolValue;
                return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
