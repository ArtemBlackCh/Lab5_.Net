using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace laba5.Tests
{
    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath = string.Empty;
        
        [TestInitialize]
        public void TestInitialize()
        {
            
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
        [TestMethod]
        public async Task SaveAsync_Should_WriteDataToFile()
        {
            var patients = new List<Patient>
            {
                new Patient("Іваненко Іван Іванович", "Грип", new DateTime(2026, 10, 1), 12),
                new Patient("Петренко Петро Петрович", "Ангіна", new DateTime(2026, 10, 20), 8)
            };
            var fileSaver = new JsonFileWriter();
            await fileSaver.SaveAsync(_testFilePath, patients);
            Assert.IsTrue(File.Exists(_testFilePath), "Файл не було створено.");
            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent), "Файл порожній.");
            
            var fileReader = new JsonFileReader();
            var loadedPatients = await fileReader.LoadAsync<Patient>(_testFilePath);
            Assert.IsNotNull(loadedPatients, "Десеріалізація не вдалася.");
            Assert.AreEqual(2, loadedPatients.Count, "Кількість десеріалізованих об'єктів не збігається.");
            Assert.AreEqual(new DateTime(2026, 10, 1), loadedPatients[0].AdmissionDate);
        }
        [TestMethod]
        public async Task LoadAsync_Should_ReadDataFromFile()
        {
            var jsonContent = "[{\"FullName\":\"Іваненко Іван Іванович\",\"Disease\":\"Грип\",\"AdmissionDate\":\"2026-10-01T00:00:00\",\"WorkExperience\":12}]";
            await File.WriteAllTextAsync(_testFilePath, jsonContent);
            var fileReader = new JsonFileReader();
            var loadedPatients = await fileReader.LoadAsync<Patient>(_testFilePath);
            Assert.IsNotNull(loadedPatients, "Об'єкти не були завантажені.");
            Assert.AreEqual(1, loadedPatients.Count, "Некоректна кількість об'єктів.");
            Assert.AreEqual("Іваненко Іван Іванович", loadedPatients[0].FullName);
            Assert.AreEqual("Грип", loadedPatients[0].Disease);
            Assert.AreEqual(12, loadedPatients[0].WorkExperience);
        }
        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var fileReader = new JsonFileReader();
            var loadedPatients = await fileReader.LoadAsync<Patient>(_testFilePath);
            Assert.IsNull(loadedPatients, "Повинен повертати null, якщо файл не існує.");
        }
    }
}
