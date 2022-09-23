using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_PLayer.ViewModel
{
    //INotifyPropertyChanged - этот интерфейс способен уведомлять нас о том,
    //что у объекта изменилось какое-то свойство
    //abstract - Озночает что класс создан только как базовый для других классов
    //Создавать экземпляры абстрактного класса нельзя
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>Событие для извещения об изменения свойства</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //этот метод будет вызываться только из view-моделей(это я сам выдумал)
        //использование атрибута [CallerMemberName] позволяет компилятору 
        //передавать в параметр имя вызывающего метода
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Его задача обновлять значение свойства, для которого опеределено поле в котором это свойсво хранит данные
        //Разрешить кольцевые изменения свойств (когда меняется 1 св-во система может автоматически менять другое, а то третье)
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            //Если значение поля, которое мы хотим обновить уже соответствует значению которое мы передали
            //то мы возвращаем ложь
            if (Equals(field, value)) return false;
            //Иначе мы меняем значение и возвращаем истину
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
