using System.Windows;
using System.Windows.Input;

namespace MVVM_PLayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //При клике правой кнопкой мыши крашилось приложение с исключением
        //    if(e.RightButton == MouseButtonState.Pressed)
        //        return;
        //    DragMove();
        //}
    }
}
