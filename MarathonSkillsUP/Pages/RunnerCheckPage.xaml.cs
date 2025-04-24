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
    public partial class RunnerCheckPage : Page
    {
        public RunnerCheckPage()
        {
            InitializeComponent();
        }

        private void PreviousParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private void NewParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму регистрации
            App.ShowMessageBox("Функция регистрации нового участника будет добавлена позже.", "Информация");
        }
    }
}
