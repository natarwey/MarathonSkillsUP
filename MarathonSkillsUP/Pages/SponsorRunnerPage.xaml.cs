using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MarathonSkillsUP.Pages
{
    public partial class SponsorRunnerPage : Page
    {
        private int donationAmount = 50;

        public SponsorRunnerPage()
        {
            InitializeComponent();
        }

        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            // Форматирование номера карты с пробелами после каждых 4 цифр
            string text = new string(CardNumberTextBox.Text.Where(char.IsDigit).ToArray());
            string formattedText = "";

            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    formattedText += " ";
                }
                formattedText += text[i];
            }

            if (CardNumberTextBox.Text != formattedText)
            {
                int selectionStart = CardNumberTextBox.SelectionStart;
                int spacesBeforeCursor = formattedText.Substring(0, Math.Min(selectionStart, formattedText.Length)).Count(c => c == ' ');
                int spacesInOriginal = CardNumberTextBox.Text.Substring(0, Math.Min(selectionStart, CardNumberTextBox.Text.Length)).Count(c => c == ' ');
                int newSelectionStart = selectionStart + (spacesBeforeCursor - spacesInOriginal);

                CardNumberTextBox.Text = formattedText;
                CardNumberTextBox.SelectionStart = Math.Max(0, Math.Min(newSelectionStart, formattedText.Length));
            }
        }

        private void CvcTextBox_PreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            // Разрешаем только цифры
            textCompositionEventArgs.Handled = !char.IsDigit(textCompositionEventArgs.Text[0]);
        }

        private void DonationAmountTextBox_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (int.TryParse(DonationAmountTextBox.Text, out int amount))
            {
                donationAmount = amount;
            }
        }

        private void DonationAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            // Разрешаем только цифры
            textCompositionEventArgs.Handled = !char.IsDigit(textCompositionEventArgs.Text[0]);
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            if (donationAmount >= 10)
            {
                donationAmount -= 10;
                DonationAmountTextBox.Text = donationAmount.ToString();
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            donationAmount += 10;
            DonationAmountTextBox.Text = donationAmount.ToString();
        }

        private void CharityInfoButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
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
                Text = "Фонд Кошек",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock descriptionLabel = new TextBlock
            {
                Text = "Фонд Кошек - это благотворительная организация, которая помогает бездомным кошкам. Мы предоставляем еду, кров и медицинскую помощь для кошек, которые в этом нуждаются.",
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
            closeButton.Click += (s, e) => popup.Close();

            // Добавляем элементы в панель
            panel.Children.Add(titleLabel);
            panel.Children.Add(descriptionLabel);
            panel.Children.Add(closeButton);

            // Устанавливаем содержимое окна
            popup.Content = panel;

            // Показываем окно
            popup.ShowDialog();
        }

        private void PayButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            // Проверяем все поля
            if (string.IsNullOrEmpty(SponsorNameTextBox.Text))
            {
                App.ShowMessageBox("Пожалуйста, введите ваше имя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(CardHolderTextBox.Text))
            {
                App.ShowMessageBox("Пожалуйста, введите имя владельца карты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string cardNumber = new string(CardNumberTextBox.Text.Where(char.IsDigit).ToArray());
            if (cardNumber.Length != 16)
            {
                App.ShowMessageBox("Номер карты должен содержать 16 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int month = int.Parse(((ComboBoxItem)MonthComboBox.SelectedItem).Content.ToString());
            int year = int.Parse(((ComboBoxItem)YearComboBox.SelectedItem).Content.ToString());
            DateTime expiryDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            if (expiryDate < DateTime.Now)
            {
                App.ShowMessageBox("Срок действия карты истек.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CvcTextBox.Text.Length != 3 || !CvcTextBox.Text.All(char.IsDigit))
            {
                App.ShowMessageBox("CVC должен содержать 3 цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (donationAmount <= 0)
            {
                App.ShowMessageBox("Сумма пожертвования должна быть больше 0.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Если все проверки пройдены, переходим на экран подтверждения
            string runnerName = ((ComboBoxItem)RunnerComboBox.SelectedItem).Content.ToString();
            NavigationService.Navigate(new SponsorshipConfirmationPage(runnerName, donationAmount));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            NavigationService.Navigate(new HomePage());
        }
    }
}