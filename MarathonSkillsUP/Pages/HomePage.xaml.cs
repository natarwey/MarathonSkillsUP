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
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void BecomeRunnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RunnerCheckPage());
        }

        private void SponsorRunnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SponsorRunnerPage());
        }

        private void LearnMoreButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DetailedInfoPage());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
