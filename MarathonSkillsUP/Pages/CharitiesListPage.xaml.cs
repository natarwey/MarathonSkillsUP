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
    public partial class CharitiesListPage : Page
    {
        public CharitiesListPage()
        {
            InitializeComponent();
            LoadCharities();
        }

        private void LoadCharities()
        {
            // Добавляем примеры благотворительных организаций
            AddCharity("Фонд Кошек", "Фонд Кошек - это благотворительная организация, которая помогает бездомным кошкам. Мы предоставляем еду, кров и медицинскую помощь для кошек, которые в этом нуждаются.");
            AddCharity("Фонд Собак", "Фонд Собак - это благотворительная организация, которая помогает бездомным собакам. Мы предоставляем еду, кров и медицинскую помощь для собак, которые в этом нуждаются.");
            AddCharity("Фонд Детей", "Фонд Детей - это благотворительная организация, которая помогает детям из малообеспеченных семей. Мы предоставляем еду, одежду и образование для детей, которые в этом нуждаются.");
        }

        private void AddCharity(string name, string description)
        {
            // Создаем панель для благотворительной организации
            Border charityBorder = new Border
            {
                BorderBrush = (SolidColorBrush)Application.Current.FindResource("GrayBrush"),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(10)
            };

            Grid charityGrid = new Grid();
            charityGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            charityGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Создаем элемент для логотипа
            Border logoBorder = new Border
            {
                Width = 70,
                Height = 70,
                BorderBrush = (SolidColorBrush)Application.Current.FindResource("GrayBrush"),
                BorderThickness = new Thickness(1),
                Background = (SolidColorBrush)Application.Current.FindResource("LightGrayBrush"),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBlock logoText = new TextBlock
            {
                Text = "Logo",
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            logoBorder.Child = logoText;

            // Создаем элементы для названия и описания
            TextBlock nameText = new TextBlock
            {
                Text = name,
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 5)
            };

            TextBlock descriptionText = new TextBlock
            {
                Text = description,
                FontFamily = (FontFamily)Application.Current.FindResource("OpenSansFont"),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            StackPanel textPanel = new StackPanel
            {
                Margin = new Thickness(10, 0, 0, 0)
            };
            textPanel.Children.Add(nameText);
            textPanel.Children.Add(descriptionText);

            // Добавляем элементы в сетку
            Grid.SetColumn(logoBorder, 0);
            Grid.SetColumn(textPanel, 1);
            charityGrid.Children.Add(logoBorder);
            charityGrid.Children.Add(textPanel);

            // Добавляем сетку в рамку
            charityBorder.Child = charityGrid;

            // Добавляем рамку в панель благотворительных организаций
            CharitiesPanel.Children.Add(charityBorder);
        }
    }
}
