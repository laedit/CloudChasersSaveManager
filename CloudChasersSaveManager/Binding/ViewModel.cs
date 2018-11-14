using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CloudChasersSaveManager.Binding
{
    internal abstract class ViewModel : INotifyPropertyValueChanged //INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyValueChangedEventHandler PropertyValueChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void OnPropertyValueChanged(string propertyName, object newValue)
        {
            PropertyValueChanged?.Invoke(this, new PropertyValueChangedEventArgs(propertyName, newValue));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            //OnPropertyChanged(propertyName);
            OnPropertyValueChanged(propertyName, value);
            return true;
        }
    }
}
