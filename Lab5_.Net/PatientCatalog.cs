using System.Collections.ObjectModel;

namespace laba5
{
    public class PatientCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;
        public ObservableCollection<Patient> Patients { get; }
        private List<Patient> _allPatients;

        public PatientCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            Patients = new ObservableCollection<Patient>();
            _allPatients = new List<Patient>();
        }

        public void AddPatient(Patient patient)
        {
            Patients.Add(patient);
            _allPatients.Add(patient);
        }

        public void DeletePatient(Patient patient)
        {
            Patients.Remove(patient);
            _allPatients.Remove(patient);
        }
        public void SortByName()
        {
            var sortedPatients = new List<Patient>(Patients);
            sortedPatients.Sort(new PatientNameComparer());
            UpdateVisibleCollection(sortedPatients);
        }

        public void FilterByDays(int days)
        {
            var filtered = _allPatients.Where(p => p.DaysInTreatment > days).ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            UpdateVisibleCollection(_allPatients);
        }

        private void UpdateVisibleCollection(List<Patient> newList)
        {
            Patients.Clear();
            foreach (var patient in newList)
            {
                Patients.Add(patient);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allPatients);
        }

        public async Task LoadAsync()
        {
            var loadedPatients = await _fileLoader.LoadAsync<Patient>(_filePath);
            if (loadedPatients != null)
            {
                _allPatients = loadedPatients;
                UpdateVisibleCollection(_allPatients);
            }
        }
    }
}
