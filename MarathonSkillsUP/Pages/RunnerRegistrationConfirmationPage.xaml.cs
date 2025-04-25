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
    public partial class RunnerRegistrationConfirmationPage : Page
    {
        public RunnerRegistrationConfirmationPage(decimal totalCost)
        {
            InitializeComponent();

            RegistrationInfoTextBlock.Text = $"Вы были зарегистрированы для Marathon Skills 2016.";
            PaymentInfoTextBlock.Text = $"Спасибо за вашу регистрацию в качестве бегуна и оплату ${totalCost}.";
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся на главную страницу
            while (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }

            // Переходим на страницу меню бегуна
            NavigationService.Navigate(new RunnerMenuPage());
        }
    }
}
