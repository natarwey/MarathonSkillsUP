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
    public partial class SponsorshipConfirmationPage : Page
    {
        public SponsorshipConfirmationPage(string runnerName, int donationAmount)
        {
            InitializeComponent();

            RunnerLabel.Text = $"Вы спонсировали бегуна: {runnerName}";
            AmountLabel.Text = $"Сумма: ${donationAmount}";
            SponsorCountLabel.Text = $"Благодарим за поддержку бегуна в Marathon Skills 2016!";
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}
