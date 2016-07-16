using Prism.Mvvm;
using System;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CalculationApplication.Commons
{
    public class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        private ErrorsContainer<string> errorsContainer;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ViewModelBase()
        {
            errorsContainer = new ErrorsContainer<string>(propertyName => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName)));
        }

        public bool HasErrors => errorsContainer.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return errorsContainer.GetErrors(propertyName);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(!base.SetProperty<T>(ref storage, value, propertyName)) return false;

            var context = new ValidationContext(this) { MemberName = propertyName };
            var errors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, errors))
            {
                errorsContainer.SetErrors(propertyName, errors.Select(error => error.ErrorMessage).ToArray());
            }
            else
            {
                errorsContainer.ClearErrors(propertyName);
            }
            return true;
        }
    }
}
