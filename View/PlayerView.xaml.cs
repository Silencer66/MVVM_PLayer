using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MVVM_PLayer.View
{
    /// <summary>
    /// Логика взаимодействия для PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private double panelWidth;
        public PlayerView()
        {
            InitializeComponent();
            panelWidth = sidePanel.Width;
            sidePanel.Width = 48;
        }

        //для анимации
        private void SlideButton_Click(object sender, RoutedEventArgs e)
        {
            //тогда мы закрываем панель
            if (sidePanel.Width == panelWidth)
            {
                DoubleAnimation sidePanelAnimation = new DoubleAnimation();
                sidePanelAnimation.From = sidePanel.ActualWidth;
                sidePanelAnimation.To = 48;
                sidePanelAnimation.Duration = TimeSpan.FromSeconds(0.3);
                sidePanel.BeginAnimation(StackPanel.WidthProperty, sidePanelAnimation);

                //Включаем textblock для поиска
                SearchTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                DoubleAnimation sidePanelAnimation = new DoubleAnimation();
                sidePanelAnimation.From = sidePanel.ActualWidth;
                sidePanelAnimation.To = panelWidth;
                sidePanelAnimation.Duration = TimeSpan.FromSeconds(0.3);
                sidePanel.BeginAnimation(StackPanel.WidthProperty, sidePanelAnimation);
                
                //Скрываем textblock для поиска
                SearchTextBlock.Visibility = Visibility.Hidden;                
            }
        }

        private void Slider_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtSearch.Text))
            {
                SearchSoundText.Text = "";
            }
            else
            {
                SearchSoundText.Text = "Введите название трека";
            }
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            popUp.IsOpen = true;
        }

        
    }
}
