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
    public partial class HowLongMarathonPage : Page
    {
        private class ComparisonItem
        {
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public double Value { get; set; }
            public bool IsSpeed { get; set; }

            public string GetComparisonText()
            {
                if (IsSpeed)
                {
                    // Расчет времени для преодоления марафона
                    double timeInHours = 42.2 / Value;
                    int hours = (int)timeInHours;
                    int minutes = (int)((timeInHours - hours) * 60);

                    return $"Максимальная скорость {Name} {Value} км/ч. Это займет {hours} ч {minutes} мин чтобы завершить 42 км марафон.";
                }
                else
                {
                    // Расчет количества элементов для покрытия марафона
                    double count = 42200 / Value; // 42.2 км = 42200 м

                    return $"Длина {Name} {Value} м. Потребуется {count:F0} из них, чтобы покрыть расстояние в 42 км марафона.";
                }
            }
        }

        private List<ComparisonItem> speedItems;
        private List<ComparisonItem> distanceItems;

        public HowLongMarathonPage()
        {
            InitializeComponent();
            InitializeComparisonItems();
            LoadItems();
        }

        private void InitializeComparisonItems()
        {
            // Инициализация элементов для сравнения скорости
            speedItems = new List<ComparisonItem>
            {
                new ComparisonItem { Name = "Улитка", ImagePath = "/Images/HowLong/snail.jpg", Value = 0.03, IsSpeed = true },
                new ComparisonItem { Name = "Человек (ходьба)", ImagePath = "/Images/HowLong/walking.jpg", Value = 5.0, IsSpeed = true },
                new ComparisonItem { Name = "Бегун (марафон)", ImagePath = "/Images/HowLong/runner.jpg", Value = 12.0, IsSpeed = true },
                new ComparisonItem { Name = "Велосипедист", ImagePath = "/Images/HowLong/cyclist.jpg", Value = 30.0, IsSpeed = true },
                new ComparisonItem { Name = "Автомобиль (город)", ImagePath = "/Images/HowLong/car.jpg", Value = 60.0, IsSpeed = true },
                new ComparisonItem { Name = "Гепард", ImagePath = "/Images/HowLong/cheetah.jpg", Value = 100.0, IsSpeed = true },
                new ComparisonItem { Name = "Самолет", ImagePath = "/Images/HowLong/airplane.jpg", Value = 800.0, IsSpeed = true }
            };

            // Инициализация элементов для сравнения расстояния
            distanceItems = new List<ComparisonItem>
            {
                new ComparisonItem { Name = "Автобус", ImagePath = "/Images/HowLong/bus.jpg", Value = 12.0, IsSpeed = false },
                new ComparisonItem { Name = "Футбольное поле", ImagePath = "/Images/HowLong/football_field.jpg", Value = 105.0, IsSpeed = false },
                new ComparisonItem { Name = "Эйфелева башня", ImagePath = "/Images/HowLong/eiffel_tower.jpg", Value = 324.0, IsSpeed = false },
                new ComparisonItem { Name = "Эмпайр Стейт Билдинг", ImagePath = "/Images/HowLong/empire_state.jpg", Value = 443.0, IsSpeed = false },
                new ComparisonItem { Name = "Бурдж Халифа", ImagePath = "/Images/HowLong/burj_khalifa.jpg", Value = 828.0, IsSpeed = false },
                new ComparisonItem { Name = "Аэробус A380", ImagePath = "/Images/HowLong/airbus.jpg", Value = 73.0, IsSpeed = false }
            };
        }

        private void LoadItems()
        {
            ItemsListBox.Items.Clear();

            List<ComparisonItem> items = SpeedRadioButton.IsChecked == true ? speedItems : distanceItems;

            foreach (var item in items)
            {
                ItemsListBox.Items.Add(item.Name);
            }

            if (ItemsListBox.Items.Count > 0)
                ItemsListBox.SelectedIndex = 0;
        }

        private void CategoryRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadItems();
        }

        private void ItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsListBox.SelectedIndex == -1)
                return;

            List<ComparisonItem> items = SpeedRadioButton.IsChecked == true ? speedItems : distanceItems;
            ComparisonItem selectedItem = items[ItemsListBox.SelectedIndex];

            ItemNameTextBlock.Text = selectedItem.Name;

            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedItem.ImagePath, UriKind.Relative);
                bitmap.EndInit();
                ItemImage.Source = bitmap;
            }
            catch (Exception ex)
            {
                App.ShowMessageBox($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ComparisonTextBlock.Text = selectedItem.GetComparisonText();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
