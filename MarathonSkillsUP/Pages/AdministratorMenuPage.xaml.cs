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
    public partial class AdministratorMenuPage : Page
    {
        public AdministratorMenuPage()
        {
            InitializeComponent();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму управления пользователями
            App.ShowMessageBox("Функция управления пользователями будет добавлена позже.", "Информация");
        }

        private void VolunteersButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму управления волонтерами
            App.ShowMessageBox("Функция управления волонтерами будет добавлена позже.", "Информация");
        }

        private void CharitiesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CharitiesListPage());
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму управления инвентарем
            App.ShowMessageBox("Функция управления инвентарем будет добавлена позже.", "Информация");
        }
    }
}
