using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TestManager.Lib.Services
{
    internal class ViewService : IViewService
    {
        private Dictionary<Type, Type> _viewDictionary;
        private Dictionary<string, Type> _keyViewDictionary;

        public ViewService()
        {
            this._viewDictionary = new Dictionary<Type, Type>();
            this._keyViewDictionary = new Dictionary<string, Type>();
        }

        public FrameworkElement GetViewbyKey(string key, ViewModelBase vm = null)
        {
            if(string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key should not be empty");

            if (!_keyViewDictionary.ContainsKey(key))
                throw new ArgumentException(string.Format("No view associated to the key {0}.", key));

            FrameworkElement view = _GetView(_keyViewDictionary[key]);
            if (vm != null)
                view.DataContext = vm;

            return view;
        }

        public FrameworkElement GetViewFor(ViewModelBase vm)
        {
            FrameworkElement view = _GetView(vm.GetType());
            view.DataContext = vm;

            return view;
        }

        public void RegisterViewFor<T>(Type viewType, string key = "") where T : ViewModelBase
        {
            Type vmType = typeof(T);

            _SetView(vmType, viewType);
            _SetKeyView(vmType, key);
        }

        private void _SetView(Type vmType, Type viewType)
        {
            if (!_viewDictionary.ContainsKey(vmType))
            {
                _viewDictionary.Add(vmType, viewType);
            }
            else
                _viewDictionary[vmType] = viewType;
        }

        private void _SetKeyView(Type vmType, string key = "")
        {
            if(string.IsNullOrWhiteSpace(key))
                key = vmType.Name;

            if (!_keyViewDictionary.ContainsKey(key))
            {
                _keyViewDictionary.Add(key, vmType);
            }
            else
                _keyViewDictionary[key] = vmType;
        }

        private FrameworkElement _GetView(Type vmType)
        {
            if (!_viewDictionary.ContainsKey(vmType))
                throw new ArgumentException(string.Format("No view associated to the Type {0}.", vmType.Name));

            var view = Activator.CreateInstance(_viewDictionary[vmType]) as FrameworkElement;
            if (view == null)
                throw new ArgumentException(string.Format("No control associated to the Type {0}.", vmType.Name));

            return view;
        }
    }
}
