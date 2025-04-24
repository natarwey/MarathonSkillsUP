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
    public partial class CoordinatorMenuPage : Page
    {
        public CoordinatorMenuPage()
        {
            InitializeComponent();
        }

        private void RunnersButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму управления бегунами
            App.ShowMessageBox("Функция управления бегунами будет добавлена позже.", "Информация");
        }

        private void SponsorsButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы переход на форму управления спонсорами
            App.ShowMessageBox("Функция управления спонсорами будет добавлена позже.", "Информация");
        }
    }
}
