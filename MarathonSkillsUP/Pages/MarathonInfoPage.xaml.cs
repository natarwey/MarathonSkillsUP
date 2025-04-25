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
    public partial class MarathonInfoPage : Page
    {
        public MarathonInfoPage()
        {
            InitializeComponent();
            LoadPhotos();
        }

        private void LoadPhotos()
        {
            // В реальном приложении загрузка из базы данных или файловой системы
            // Для демонстрации используем тестовые данные
            string[] photoFiles = new string[]
            {
                "/Images/Marathon/photo1.jpg",
                "/Images/Marathon/photo2.jpg",
                "/Images/Marathon/photo3.jpg",
                "/Images/Marathon/photo4.jpg",
                "/Images/Marathon/photo5.jpg",
                "/Images/Marathon/photo6.jpg"
            };

            foreach (string photoFile in photoFiles)
            {
                try
                {
                    // Создаем изображение
                    Image image = new Image
                    {
                        Width = 200,
                        Height = 150,
                        Margin = new Thickness(5),
                        Stretch = System.Windows.Media.Stretch.Uniform
                    };

                    // Загружаем изображение
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(photoFile, UriKind.Relative);
                    bitmap.EndInit();
                    image.Source = bitmap;

                    // Добавляем изображение в панель
                    PhotosPanel.Children.Add(image);
                }
                catch (Exception ex)
                {
                    App.ShowMessageBox($"Ошибка при загрузке изображения {photoFile}: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InteractiveMapButton_Click(object sender, RoutedEventArgs e)
        {
            // В реальном приложении здесь был бы код для открытия интерактивной карты
            // Для демонстрации просто показываем сообщение
            App.ShowMessageBox("Интерактивная карта будет доступна в полной версии приложения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
