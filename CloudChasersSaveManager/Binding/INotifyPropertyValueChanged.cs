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

// TODO 
// RestoreButton => add only if backup file exists, or use StateButton
// Binding of button => nor required unless StateButton is used, _button.Clicked = cm.ButtonClicked; is enough
// In the contrary, use ICommand based object ? { CanExecute ; CanExecuteChanged ; Execute }
// with binding for CanExecuteChanged part and update of Disabled of StateButton.