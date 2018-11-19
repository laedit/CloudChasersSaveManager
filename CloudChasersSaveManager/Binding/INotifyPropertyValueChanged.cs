using System;

namespace CloudChasersSaveManager.Binding
{
    internal interface INotifyPropertyValueChanged
    {
        event PropertyValueChangedEventHandler PropertyValueChanged;
    }

    internal delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);

    public class PropertyValueChangedEventArgs : EventArgs
    {
        public PropertyValueChangedEventArgs(string propertyName, object newValue)
        {
            PropertyName = propertyName;
            PropertyNewValue = newValue;
        }

        public virtual string PropertyName { get; }

        public object PropertyNewValue { get; }
    }
}
