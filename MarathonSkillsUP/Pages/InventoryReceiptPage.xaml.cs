using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class InventoryReceiptPage : Page
    {
        private int currentRemaining = 0;

        public InventoryReceiptPage()
        {
            InitializeComponent();
            InventoryTypeListBox.SelectedIndex = 0;
        }

        private void InventoryTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InventoryTypeListBox.SelectedItem == null)
                return;

            string selectedType = ((ListBoxItem)InventoryTypeListBox.SelectedItem).Content.ToString();
            SelectedInventoryTypeTextBlock.Text = selectedType;

            // В реальном приложении загрузка из базы данных
            // Для демонстрации используем тестовые данные
            switch (selectedType)
            {
                case "Номер":
                    currentRemaining = 50;
                    break;
                case "RFID браслет":
                    currentRemaining = 50;
                    break;
                case "Бейсболка":
                    currentRemaining = 40;
                    break;
                case "Бутылка воды":
                    currentRemaining = 40;
                    break;
                case "Футболка":
                    currentRemaining = 30;
                    break;
                case "Сувенирный буклет":
                    currentRemaining = 30;
                    break;
                default:
                    currentRemaining = 0;
                    break;
            }

            CurrentRemainingTextBlock.Text = currentRemaining.ToString();
            UpdateNewRemaining();
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и знак минус в начале
            Regex regex = new Regex(@"^-?\d*$");
            e.Handled = !regex.IsMatch(QuantityTextBox.Text + e.Text);

            if (!e.Handled)
            {
                // Обновляем новый остаток после изменения количества
                UpdateNewRemaining();
            }
        }

        private void UpdateNewRemaining()
        {
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                int newRemaining = currentRemaining + quantity;
                NewRemainingTextBlock.Text = newRemaining.ToString();
            }
            else
            {
                NewRemainingTextBlock.Text = currentRemaining.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryTypeListBox.SelectedItem == null)
            {
                App.ShowMessageBox("Выберите тип инвентаря", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                App.ShowMessageBox("Введите корректное количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedType = ((ListBoxItem)InventoryTypeListBox.SelectedItem).Content.ToString();
            string comment = CommentTextBox.Text;

            // В реальном приложении здесь был бы код для сохранения данных в базу данных

            App.ShowMessageBox($"Поступление инвентаря '{selectedType}' в количестве {quantity} успешно сохранено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
