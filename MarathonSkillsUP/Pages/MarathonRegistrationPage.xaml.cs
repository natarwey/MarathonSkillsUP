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
    public partial class MarathonRegistrationPage : Page
    {
        private decimal marathonCost = 0;
        private decimal kitCost = 0;
        private decimal donationAmount = 0;
        private decimal totalCost = 0;

        public MarathonRegistrationPage()
        {
            InitializeComponent();
            LoadCharities();
            UpdateCosts();
        }

        private void LoadCharities()
        {
            // В реальном приложении загрузка из базы данных
            CharityComboBox.Items.Add("Фонд Кошек");
            CharityComboBox.Items.Add("Фонд Собак");
            CharityComboBox.Items.Add("Фонд Детей");
            CharityComboBox.SelectedIndex = 0;
        }

        private void MarathonCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCosts();
        }

        private void MarathonCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCosts();
        }

        private void OptionRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCosts();
        }

        private void DonationAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs inputEventArgs)
        {
            // Разрешаем только цифры и точку
            inputEventArgs.Handled = !char.IsDigit(inputEventArgs.Text[0]) && inputEventArgs.Text[0] != '.';
        }

        private void DonationAmountTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (decimal.TryParse(DonationAmountTextBox.Text, out decimal amount))
            {
                donationAmount = amount;
                UpdateCosts();
            }
        }

        private void CharityInfoButton_Click(object sender, RoutedEventArgs charityInfoEventArgs)
        {
            string selectedCharity = CharityComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedCharity))
                return;

            string description = "";

            switch (selectedCharity)
            {
                case "Фонд Кошек":
                    description = "Фонд Кошек - это благотворительная организация, которая помогает бездомным кошкам. Мы предоставляем еду, кров и медицинскую помощь для кошек, которые в этом нуждаются.";
                    break;
                case "Фонд Собак":
                    description = "Фонд Собак - это благотворительная организация, которая помогает бездомным собакам. Мы предоставляем еду, кров и медицинскую помощь для собак, которые в этом нуждаются.";
                    break;
                case "Фонд Детей":
                    description = "Фонд Детей - это благотворительная организация, которая помогает детям из малообеспеченных семей. Мы предоставляем еду, одежду и образование для детей, которые в этом нуждаются.";
                    break;
            }

            // Создаем всплывающее окно
            Window popup = new Window
            {
                Title = "Информация о благотворительности",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            // Создаем содержимое окна
            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            TextBlock titleLabel = new TextBlock
            {
                Text = selectedCharity,
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock descriptionLabel = new TextBlock
            {
                Text = description,
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 20)
            };

            Button closeButton = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            closeButton.Click += (s, closeEventArgs) => popup.Close();

            // Добавляем элементы в панель
            panel.Children.Add(titleLabel);
            panel.Children.Add(descriptionLabel);
            panel.Children.Add(closeButton);

            // Устанавливаем содержимое окна
            popup.Content = panel;

            // Показываем окно
            popup.ShowDialog();
        }

        private void UpdateCosts()
        {
            // Расчет стоимости марафонов
            marathonCost = 0;
            if (FullMarathonCheckBox.IsChecked == true)
                marathonCost += 145;
            if (HalfMarathonCheckBox.IsChecked == true)
                marathonCost += 75;
            if (FiveKmRunCheckBox.IsChecked == true)
                marathonCost += 20;

            // Расчет стоимости комплекта
            kitCost = 0;
            if (OptionBRadioButton.IsChecked == true)
                kitCost = 20;
            else if (OptionCRadioButton.IsChecked == true)
                kitCost = 45;

            // Расчет общей стоимости
            totalCost = marathonCost + kitCost + donationAmount;

            // Обновление текстовых блоков
            MarathonCostTextBlock.Text = $"${marathonCost}";
            KitCostTextBlock.Text = $"${kitCost}";
            DonationTextBlock.Text = $"${donationAmount}";
            TotalCostTextBlock.Text = $"${totalCost}";
        }

        private bool ValidateForm()
        {
            // Проверка выбора хотя бы одного марафона
            if (FullMarathonCheckBox.IsChecked != true &&
                HalfMarathonCheckBox.IsChecked != true &&
                FiveKmRunCheckBox.IsChecked != true)
            {
                ShowError("Выберите хотя бы один вид марафона");
                return false;
            }

            // Проверка суммы взноса
            if (donationAmount < 0)
            {
                ShowError("Сумма взноса должна быть положительным числом");
                return false;
            }

            HideError();
            return true;
        }

        private void ShowError(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs registerEventArgs)
        {
            if (ValidateForm())
            {
                try
                {
                    // В реальном приложении здесь был бы код для сохранения данных в базу данных

                    // Для демонстрации просто переходим на страницу подтверждения регистрации
                    NavigationService.Navigate(new RunnerRegistrationConfirmationPage(totalCost));
                }
                catch (Exception ex)
                {
                    App.ShowMessageBox($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs cancelEventArgs)
        {
            NavigationService.GoBack();
        }
    }
}
