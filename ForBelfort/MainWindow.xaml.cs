using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ForBelfort
{
    public partial class MainWindow : Window
    {
        private Random _random;
        public MainWindow()
        {
            InitializeComponent();
            LoadApplications();
            _random = new Random();
        }

        private void LoadApplications()
        {
            try
            {
                using (var context = new ForBELFORTEntities1())
                {
                    var applications = from app in context.Applications
                                       join user in context.Users on app.UserID equals user.UserID
                                       join status in context.Statuses on app.StatusID equals status.StatusID
                                       select new
                                       {
                                           ApplicationID = app.ApplicationID,
                                           UserName = user.Name,
                                           StatusName = status.StatusName,
                                           SubmissionDate = app.SubmissionDate,
                                           Description = app.Description
                                       };

                    ApplicationsDataGrid.ItemsSource = applications.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem != null)
            {
                dynamic selectedApplication = ApplicationsDataGrid.SelectedItem;
                int applicationId = selectedApplication.ApplicationID;

                try
                {
                    using (var context = new ForBELFORTEntities1())
                    {
                        var application = context.Applications.FirstOrDefault(app => app.ApplicationID == applicationId);
                        if (application != null)
                        {
                            var approvedStatus = context.Statuses.FirstOrDefault(s => s.StatusName == "Одобрено");
                            if (approvedStatus != null)
                            {
                                application.StatusID = approvedStatus.StatusID;
                                context.SaveChanges();
                                LoadApplications(); // Перезагружаем данные
                                MessageBox.Show("Статус заявки успешно изменён на 'Одобрено'.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при одобрении заявки: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для одобрения.");
            }
        }


        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem != null)
            {
                dynamic selectedApplication = ApplicationsDataGrid.SelectedItem;
                int applicationId = selectedApplication.ApplicationID;

                try
                {
                    using (var context = new ForBELFORTEntities1())
                    {
                        var application = context.Applications.FirstOrDefault(app => app.ApplicationID == applicationId);
                        if (application != null)
                        {
                            var rejectedStatus = context.Statuses.FirstOrDefault(s => s.StatusName == "Отклонено");
                            if (rejectedStatus != null)
                            {
                                application.StatusID = rejectedStatus.StatusID;
                                context.SaveChanges();
                                LoadApplications(); // Перезагружаем данные
                                MessageBox.Show("Статус заявки успешно изменён на 'Отклонено'.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отклонении заявки: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для отклонения.");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ChangeBackground_Click(object sender, RoutedEventArgs e)
        {
            Color randomColor = GetRandomColor();

            this.Background = new SolidColorBrush(randomColor);
        }

        private void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Контактная информация:telegram: @tw0se");
        }

        private void OpenRickroll_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.youtube.com/watch?v=dQw4w9WgXcQ") { UseShellExecute = true });
        }
        private Color GetRandomColor()
        {
            byte[] rgb = new byte[3];
            _random.NextBytes(rgb); 
            return Color.FromRgb(rgb[0], rgb[1], rgb[2]);
        }

        private void AddApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationWindow appWindow = new ApplicationWindow();
            appWindow.ShowDialog();

            LoadApplications();
        }



    }
}
