using System;
using System.Collections.Generic;

namespace CloudChasersSaveManager.Binding
{
    internal sealed class Binder
    {
        private readonly Dictionary<string, Delegate> _bindings = new Dictionary<string, Delegate>();
        
        private readonly INotifyPropertyValueChanged _npcSubject;
        private readonly Type _npcType;

        internal static Binder From<T>(T subject) where T : INotifyPropertyValueChanged
        {
            return new Binder(subject);
        }

        private Binder(INotifyPropertyValueChanged subject)
        {
            _npcSubject = subject;
            _npcSubject.PropertyValueChanged += ViewModel_PropertyValueChanged;
            _npcType = subject.GetType();
        }

        internal void Bind<TArg>(string propertyName, Action<TArg> notificationAction)
        {
            notificationAction((TArg)_npcType.GetProperty(propertyName).GetValue(_npcSubject));
            
            _bindings.Add(propertyName, notificationAction);
        }
        
        private void ViewModel_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (_bindings.ContainsKey(e.PropertyName))
            {
                _bindings[e.PropertyName].DynamicInvoke(e.PropertyNewValue);
            }
        }

        private struct BindDelegates
        {
            internal Delegate GetValue { get; private set; }
            internal Delegate Notification { get; private set; }

            public BindDelegates(Delegate getValue, Delegate notification)
            {
                GetValue = getValue;
                Notification = notification;
            }
        }
    }
}
