using MarathonSkillsUP.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarathonSkillsUP
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer countdownTimer;

        public MainWindow()
        {
            InitializeComponent();
            SetupCountdownTimer();

            // Регистрируем обработчик события навигации
            MainFrame.Navigated += MainFrame_Navigated;

            // Загружаем стартовую страницу
            MainFrame.Navigate(new HomePage());
        }

        private void SetupCountdownTimer()
        {
            // Обновляем текст таймера
            UpdateCountdownLabel();

            // Создаем и запускаем таймер
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
        }

        private void UpdateCountdownLabel()
        {
            CountdownLabel.Text = App.GetRemainingTimeString();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            UpdateCountdownLabel();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Обновляем видимость кнопок в зависимости от текущей страницы
            if (e.Content is HomePage)
            {
                BackButton.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackButton.Visibility = Visibility.Visible;

                // Показываем кнопку Logout только для страниц, требующих авторизации
                if (e.Content is RunnerMenuPage || e.Content is CoordinatorMenuPage || e.Content is AdministratorMenuPage)
                {
                    LogoutButton.Visibility = Visibility.Visible;
                }
                else
                {
                    LogoutButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из системы и возврат на главную страницу
            UserManager.Logout();
            MainFrame.Navigate(new HomePage());
        }
    }
}
