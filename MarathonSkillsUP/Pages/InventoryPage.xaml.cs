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
    public partial class InventoryPage : Page
    {
        public class InventoryItem
        {
            public string Kit { get; set; }
            public int SelectedCount { get; set; }
            public string Number { get; set; }
            public string RfidBracelet { get; set; }
            public string Cap { get; set; }
            public string WaterBottle { get; set; }
            public string TShirt { get; set; }
            public string Booklet { get; set; }
            public int Required { get; set; }
            public int Remaining { get; set; }
        }

        public InventoryPage()
        {
            InitializeComponent();
            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            // В реальном приложении загрузка из базы данных
            // Для демонстрации используем тестовые данные
            List<InventoryItem> inventoryItems = new List<InventoryItem>
            {
                new InventoryItem
                {
                    Kit = "Тип A",
                    SelectedCount = 70,
                    Number = "70",
                    RfidBracelet = "70",
                    Cap = "✗",
                    WaterBottle = "✗",
                    TShirt = "✗",
                    Booklet = "✗",
                    Required = 70,
                    Remaining = 50
                },
                new InventoryItem
                {
                    Kit = "Тип B",
                    SelectedCount = 30,
                    Number = "30",
                    RfidBracelet = "30",
                    Cap = "30",
                    WaterBottle = "30",
                    TShirt = "✗",
                    Booklet = "✗",
                    Required = 90,
                    Remaining = 40
                },
                new InventoryItem
                {
                    Kit = "Тип C",
                    SelectedCount = 23,
                    Number = "23",
                    RfidBracelet = "23",
                    Cap = "23",
                    WaterBottle = "23",
                    TShirt = "23",
                    Booklet = "23",
                    Required = 115,
                    Remaining = 30
                }
            };

            InventoryDataGrid.ItemsSource = inventoryItems;
            TotalRunnersTextBlock.Text = "123";
        }

        private void ReportButton_Click(object sender, RoutedEventArgs reportEventArgs)
        {
            // В реальном приложении здесь был бы код для генерации отчета
            // Для демонстрации создаем простое окно с отчетом
            Window reportWindow = new Window
            {
                Title = "Отчет по инвентарю",
                Width = 600,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // Создаем содержимое окна
            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            TextBlock titleLabel = new TextBlock
            {
                Text = "Отчет по инвентарю Marathon Skills 2016",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock dateLabel = new TextBlock
            {
                Text = $"Дата: {DateTime.Now.ToShortDateString()}",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 20)
            };

            TextBlock totalRunnersLabel = new TextBlock
            {
                Text = $"Всего зарегистрировалось на марафон: {TotalRunnersTextBlock.Text}",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 20)
            };

            // Создаем таблицу для отчета
            DataGrid reportDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                GridLinesVisibility = DataGridGridLinesVisibility.All,
                BorderBrush = (System.Windows.Media.SolidColorBrush)Application.Current.FindResource("GrayBrush"),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 20)
            };

            // Добавляем колонки
            reportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Наименование", Binding = new System.Windows.Data.Binding("Name"), Width = 150 });
            reportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Необходимо", Binding = new System.Windows.Data.Binding("Required"), Width = 100 });
            reportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Остаток", Binding = new System.Windows.Data.Binding("Remaining"), Width = 100 });
            reportDataGrid.Columns.Add(new DataGridTextColumn { Header = "Необходимо заказать", Binding = new System.Windows.Data.Binding("ToOrder"), Width = 150 });

            // Создаем данные для таблицы
            List<object> reportData = new List<object>
            {
                new { Name = "Номер", Required = 123, Remaining = 50, ToOrder = 73 },
                new { Name = "RFID браслет", Required = 123, Remaining = 50, ToOrder = 73 },
                new { Name = "Бейсболка", Required = 53, Remaining = 40, ToOrder = 13 },
                new { Name = "Бутылка воды", Required = 53, Remaining = 40, ToOrder = 13 },
                new { Name = "Футболка", Required = 23, Remaining = 30, ToOrder = 0 },
                new { Name = "Сувенирный буклет", Required = 23, Remaining = 30, ToOrder = 0 }
            };

            reportDataGrid.ItemsSource = reportData;

            Button printButton = new Button
            {
                Content = "Печать",
                Width = 100,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            printButton.Click += (s, printEventArgs) => App.ShowMessageBox("Отправка на печать...", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

            // Добавляем элементы в панель
            panel.Children.Add(titleLabel);
            panel.Children.Add(dateLabel);
            panel.Children.Add(totalRunnersLabel);
            panel.Children.Add(reportDataGrid);
            panel.Children.Add(printButton);

            // Устанавливаем содержимое окна
            reportWindow.Content = panel;

            // Показываем окно
            reportWindow.ShowDialog();
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs receiptEventArgs)
        {
            NavigationService.Navigate(new InventoryReceiptPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs backEventArgs)
        {
            NavigationService.GoBack();
        }
    }
}
