using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Wpf.Wrapper
{
    public class BaseWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void propertyChanged([CallerMemberName] string? PropertyName = null)
        {
            if (PropertyName != null)
            {
                PropertyChanged?.Invoke(this, new(PropertyName));
            }
        }
    }
}
