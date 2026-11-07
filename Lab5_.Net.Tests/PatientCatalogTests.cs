using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace laba5.Tests
{
    [TestClass]
    public class PatientCatalogTests
    {
        private string _testFilePath = string.Empty;
        private PatientCatalog _catalog = null!;
        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
            _catalog = new PatientCatalog(new JsonFileWriter(), new JsonFileReader(), _testFilePath);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
        
        private void FillCatalog()
        {
            _catalog.AddPatient(new Patient("Іваненко І.І.", "Грип", DateTime.Today.AddDays(-10), 5));
            _catalog.AddPatient(new Patient("Бондаренко Б.Б.", "Ангіна", DateTime.Today.AddDays(-30), 8));
            _catalog.AddPatient(new Patient("Андрієнко А.А.", "Бронхіт", DateTime.Today.AddDays(-7), 3));
            _catalog.AddPatient(new Patient("Коваленко К.К.", "Застуда", DateTime.Today.AddDays(-2), 15));
        }
        [TestMethod]
        public void AddPatient_AddsPatientToVisibleCollection()
        {
            _catalog.AddPatient(new Patient("Іваненко І.І.", "Грип", DateTime.Today, 5));
            Assert.AreEqual(1, _catalog.Patients.Count);
        }
        [TestMethod]
        public void DeletePatient_RemovesPatientFromCollection()
        {
            var patient = new Patient("Іваненко І.І.", "Грип", DateTime.Today, 5);
            _catalog.AddPatient(patient);
            _catalog.DeletePatient(patient);
            Assert.AreEqual(0, _catalog.Patients.Count);
            
            _catalog.ResetFilter();
            Assert.AreEqual(0, _catalog.Patients.Count);
        }
        [TestMethod]
        public void FilterByDays_ReturnsOnlyPatientsTreatedMoreThanGivenDays()
        {
            FillCatalog();
            _catalog.FilterByDays(7);
            Assert.AreEqual(2, _catalog.Patients.Count);
            Assert.IsTrue(_catalog.Patients.All(p => p.DaysInTreatment > 7));
        }
        [TestMethod]
        public void ResetFilter_RestoresAllPatients()
        {
            FillCatalog();
            _catalog.FilterByDays(7);
            _catalog.ResetFilter();
            Assert.AreEqual(4, _catalog.Patients.Count);
        }
        [TestMethod]
        public void SortByName_OrdersPatientsByFullName()
        {
            FillCatalog();
            _catalog.SortByName();
            var names = _catalog.Patients.Select(p => p.FullName).ToList();
            CollectionAssert.AreEqual(
                new List<string> { "Андрієнко А.А.", "Бондаренко Б.Б.", "Іваненко І.І.", "Коваленко К.К." },
                names);
        }
        
        [TestMethod]
        public void FilterThenSort_ReturnsLongTreatedPatientsSortedByName()
        {
            FillCatalog();
            _catalog.FilterByDays(7);
            _catalog.SortByName();
            var names = _catalog.Patients.Select(p => p.FullName).ToList();
            CollectionAssert.AreEqual(new List<string> { "Бондаренко Б.Б.", "Іваненко І.І." }, names);
        }
        [TestMethod]
        public async Task SaveAsync_ThenLoadAsync_RestoresPatients()
        {
            FillCatalog();
            await _catalog.SaveAsync();
            var newCatalog = new PatientCatalog(new JsonFileWriter(), new JsonFileReader(), _testFilePath);
            await newCatalog.LoadAsync();
            Assert.AreEqual(4, newCatalog.Patients.Count);
        }
    }
}
