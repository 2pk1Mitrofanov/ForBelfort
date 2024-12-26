using System;
using System.Linq;
using System.Windows;

namespace ForBelfort
{
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string employeeName = EmployeeNameTextBox.Text.Trim();
                string description = DescriptionTextBox.Text.Trim();

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new ForBELFORTEntities1())
                {
                    var user = context.Users.FirstOrDefault(u => u.Name == employeeName);
                    if (user == null)
                    {
                        MessageBox.Show("Сотрудник с таким именем не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var newApplication = new Applications
                    {
                        UserID = user.UserID,
                        StatusID = context.Statuses.FirstOrDefault(s => s.StatusName == "На рассмотрении")?.StatusID ?? 1,
                        Description = description,
                        SubmissionDate = DateTime.Now
                    };

                    context.Applications.Add(newApplication);
                    context.SaveChanges();
                    MessageBox.Show("Заявка успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заявки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
