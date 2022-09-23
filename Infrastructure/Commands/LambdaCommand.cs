using MVVM_PLayer.Infrastructure.Commands.Base;
using System;

namespace MVVM_PLayer.Infrastructure.Commands
{
    internal class LambdaCommand : CommandBase
    {
        //Если поле помечено атрибутом readonly, то оно будет работать быстрее
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            //Проверка если сюда не передали ссылку на делегат
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = CanExecute;
        }
        //Если в _canExecute null, тогда всегда true возвращаем
        public override bool CanExecute(object parameter) => _canExecute?.Invoke(nameof(parameter)) ?? true;
        public override void Execute(object parameter) => _execute(parameter);
    }
}
