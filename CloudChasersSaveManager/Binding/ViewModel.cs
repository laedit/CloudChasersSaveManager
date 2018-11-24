using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CloudChasersSaveManager.Binding
{
    internal abstract class ViewModel : INotifyPropertyValueChanged
    {
        public event PropertyValueChangedEventHandler PropertyValueChanged;
        
        protected virtual void OnPropertyValueChanged(string propertyName, object newValue)
        {
            PropertyValueChanged?.Invoke(this, new PropertyValueChangedEventArgs(propertyName, newValue));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyValueChanged(propertyName, value);
            return true;
        }
    }
}
