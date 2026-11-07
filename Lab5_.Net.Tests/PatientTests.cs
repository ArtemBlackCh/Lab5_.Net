using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace laba5.Tests
{
    [TestClass]
    public sealed class PatientTests
    {
        
        [TestMethod]
        public void Constructor_WithValidArguments_CreatesPatientObject()
        {
            
            string fullName = "Іваненко Іван Іванович";
            string disease = "Грип";
            DateTime date = new DateTime(2026, 10, 1);
            int experience = 12;
            var patient = new Patient(fullName, disease, date, experience);
            Assert.AreEqual(fullName, patient.FullName);
            Assert.AreEqual(disease, patient.Disease);
            Assert.AreEqual(date, patient.AdmissionDate);
            Assert.AreEqual(experience, patient.WorkExperience);
        }
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Constructor_WithInvalidFullName_ThrowsArgumentException(string invalidName)
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient(invalidName, "Грип", DateTime.Today, 5));
        }
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void Constructor_WithInvalidDisease_ThrowsArgumentException(string invalidDisease)
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient("Петренко П.П.", invalidDisease, DateTime.Today, 5));
        }
        [TestMethod]
        public void Constructor_WithFutureDate_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient("Петренко П.П.", "Грип", DateTime.Today.AddDays(1), 5));
        }
        [TestMethod]
        public void Constructor_WithEarlyDate_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient("Петренко П.П.", "Грип", new DateTime(1899, 12, 31), 5));
        }
        [TestMethod]
        public void Constructor_WithNegativeExperience_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient("Петренко П.П.", "Грип", DateTime.Today, -1));
        }
        [TestMethod]
        public void Constructor_WithTooBigExperience_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Patient("Петренко П.П.", "Грип", DateTime.Today, 61));
        }
        
        [TestMethod]
        public void CompareTo_WithOtherPatient_ReturnsCorrectValue()
        {
            var patient1 = new Patient("Пацієнт А", "Грип", new DateTime(2026, 9, 1), 5);
            var patient2 = new Patient("Пацієнт Б", "Ангіна", new DateTime(2026, 10, 1), 6);
            var patient3 = new Patient("Пацієнт В", "Бронхіт", new DateTime(2026, 9, 1), 7); 
            Assert.IsTrue(patient1.CompareTo(patient2) < 0, "Patient1 надійшов раніше за Patient2.");
            Assert.IsTrue(patient2.CompareTo(patient1) > 0, "Patient2 надійшов пізніше за Patient1.");
            Assert.AreEqual(0, patient1.CompareTo(patient3), "Patient1 і Patient3 надійшли в один день.");
        }
        [TestMethod]
        public void CompareTo_WithNull_ReturnsPositiveValue()
        {
            var patient = new Patient("Пацієнт А", "Грип", DateTime.Today, 5);
            Assert.IsTrue(patient.CompareTo(null) > 0, "Comparing to a null object should return a positive value.");
        }
        
        [TestMethod]
        public void GetDaysInTreatment_ReturnsCorrectNumberOfDays()
        {
            var patient = new Patient("Пацієнт А", "Грип", new DateTime(2026, 10, 1), 5);
            Assert.AreEqual(10, patient.GetDaysInTreatment(new DateTime(2026, 10, 11)));
            Assert.AreEqual(0, patient.GetDaysInTreatment(new DateTime(2026, 10, 1)));
        }
        [TestMethod]
        public void DaysInTreatment_ForPatientAdmittedTenDaysAgo_ReturnsTen()
        {
            var patient = new Patient("Пацієнт А", "Грип", DateTime.Today.AddDays(-10), 5);
            Assert.AreEqual(10, patient.DaysInTreatment);
        }
        
        [TestMethod]
        public void ToString_ReturnsCorrectlyFormattedString()
        {
            var patient = new Patient("Іваненко Іван Іванович", "Грип", new DateTime(2026, 10, 1), 12);
            string expectedString = "ПІБ: Іваненко Іван Іванович, Хвороба: Грип, Дата надходження: 01.10.2026, Стаж: 12";
            string actualString = patient.ToString();
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
