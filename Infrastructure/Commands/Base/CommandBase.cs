using System;
using System.Windows.Input;

namespace MVVM_PLayer.Infrastructure.Commands.Base
{
    internal abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            //добавляем обработчик события, который кто-то пытался сюда добавить
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        //Если функция возвращает ложь то команду выполнить нельзя
        //тогда элемент, к которому привязана команда, отключается автоматически
        public abstract bool CanExecute(object parameter);
        //То что должно быть выполнено самой командой 
        public abstract void Execute(object parameter);
        //Реализацией этих двух методов займутся наследники
    }
}
