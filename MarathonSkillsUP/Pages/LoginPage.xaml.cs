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

namespace MarathonSkillsUP.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Для тестирования используем радиокнопки
            if (RunnerRadioButton.IsChecked == true)
            {
                UserManager.TestLogin(UserManager.UserRole.Runner);
                NavigationService.Navigate(new RunnerMenuPage());
            }
            else if (CoordinatorRadioButton.IsChecked == true)
            {
                UserManager.TestLogin(UserManager.UserRole.Coordinator);
                NavigationService.Navigate(new CoordinatorMenuPage());
            }
            else if (AdministratorRadioButton.IsChecked == true)
            {
                UserManager.TestLogin(UserManager.UserRole.Administrator);
                NavigationService.Navigate(new AdministratorMenuPage());
            }
            else
            {
                // Обычный процесс входа
                string email = EmailTextBox.Text;
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    App.ShowMessageBox("Пожалуйста, введите email и пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (UserManager.Login(email, password))
                {
                    switch (UserManager.CurrentUserRole)
                    {
                        case UserManager.UserRole.Runner:
                            NavigationService.Navigate(new RunnerMenuPage());
                            break;
                        case UserManager.UserRole.Coordinator:
                            NavigationService.Navigate(new CoordinatorMenuPage());
                            break;
                        case UserManager.UserRole.Administrator:
                            NavigationService.Navigate(new AdministratorMenuPage());
                            break;
                    }
                }
                else
                {
                    App.ShowMessageBox("Неверный email или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}
