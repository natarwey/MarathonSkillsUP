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
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите email и пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Аутентификация пользователя
            if (UserManager.Instance.AuthenticateUser(email, password))
            {
                // Перенаправление в зависимости от роли
                if (UserManager.Instance.IsRunner())
                {
                    NavigationService.Navigate(new RunnerMenuPage());
                }
                else if (UserManager.Instance.IsCoordinator())
                {
                    NavigationService.Navigate(new CoordinatorMenuPage());
                }
                else if (UserManager.Instance.IsAdministrator())
                {
                    NavigationService.Navigate(new AdministratorMenuPage());
                }
                else
                {
                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    UserManager.Instance.LogoutUser();
                }
            }
            else
            {
                MessageBox.Show("Неверный email или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}
