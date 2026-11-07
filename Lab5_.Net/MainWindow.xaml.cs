using System.Windows;

namespace laba5
{
    public partial class MainWindow : Window
    {
        private readonly PatientCatalog _catalog;

        public MainWindow()
        {
            InitializeComponent();
            // Визначаємо шлях до файлу
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(documentsPath, "patients.json");
            // Ініціалізуємо клас бізнес-логіки
            _catalog = new PatientCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);
            // Прив'язуємо колекцію з PatientCatalog до DataGrid
            PatientsDataGrid.ItemsSource = _catalog.Patients;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AdmissionDatePicker.SelectedDate == null)
                {
                    throw new ArgumentException("Оберіть дату надходження.");
                }
                if (!int.TryParse(WorkExperienceTextBox.Text, out int experience))
                {
                    throw new ArgumentException("Робочий стаж має бути цілим числом.");
                }
                var newPatient = new Patient(
                    FullNameTextBox.Text,
                    DiseaseTextBox.Text,
                    AdmissionDatePicker.SelectedDate.Value,
                    experience
                );
                _catalog.AddPatient(newPatient);
                // Очищаємо поля
                FullNameTextBox.Clear();
                DiseaseTextBox.Clear();
                AdmissionDatePicker.SelectedDate = null;
                WorkExperienceTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsDataGrid.SelectedItem is Patient selectedPatient)
            {
                _catalog.DeletePatient(selectedPatient);
            }
        }

        private void SortByNameButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortByName();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterDaysTextBox.Text))
            {
                _catalog.ResetFilter();
            }
            else if (int.TryParse(FilterDaysTextBox.Text, out int days) && days >= 0)
            {
                _catalog.FilterByDays(days);
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректну кількість днів.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.SaveAsync();
            MessageBox.Show("Список успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _catalog.LoadAsync();
            MessageBox.Show("Список успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
