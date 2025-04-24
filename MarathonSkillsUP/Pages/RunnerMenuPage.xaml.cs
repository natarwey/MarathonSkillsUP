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
    public partial class RunnerMenuPage : Page
    {
        public RunnerMenuPage()
        {
            InitializeComponent();
        }

        private void RegisterForMarathonButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму регистрации на марафон
            App.ShowMessageBox("Функция регистрации на марафон будет добавлена позже.", "Информация");
        }

        private void MyResultsButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму результатов
            App.ShowMessageBox("Функция просмотра результатов будет добавлена позже.", "Информация");
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму редактирования профиля
            App.ShowMessageBox("Функция редактирования профиля будет добавлена позже.", "Информация");
        }

        private void MySponsorButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму информации о спонсоре
            App.ShowMessageBox("Функция просмотра информации о спонсоре будет добавлена позже.", "Информация");
        }

        private void ContactsButton_Click(object sender, RoutedEventArgs e)
        {
            App.ShowContactsPopup();
        }
    }
}
