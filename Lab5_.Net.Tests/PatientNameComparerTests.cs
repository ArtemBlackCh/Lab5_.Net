using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace laba5.Tests
{
    [TestClass]
    public class PatientNameComparerTests
    {
        
        [TestMethod]
        public void Compare_WhenNamesAreDifferent_ReturnsCorrectOrder()
        {
            var patient1 = new Patient("Андрієнко А.А.", "Грип", DateTime.Today, 5);
            var patient2 = new Patient("Бондаренко Б.Б.", "Грип", DateTime.Today, 5);
            var comparer = new PatientNameComparer();
            Assert.IsTrue(comparer.Compare(patient1, patient2) < 0, "'Андрієнко' має бути перед 'Бондаренко'.");
            Assert.IsTrue(comparer.Compare(patient2, patient1) > 0, "'Бондаренко' має бути після 'Андрієнко'.");
        }
        [TestMethod]
        public void Compare_WhenNamesAreIdentical_ReturnsZero()
        {
            var patient1 = new Patient("Коваленко К.К.", "Грип", DateTime.Today, 5);
            var patient2 = new Patient("Коваленко К.К.", "Ангіна", DateTime.Today, 7);
            var comparer = new PatientNameComparer();
            Assert.AreEqual(0, comparer.Compare(patient1, patient2), "Хворі з однаковим ПІБ мають бути рівними.");
        }
        [TestMethod]
        public void Compare_WhenNamesHaveDifferentCase_ReturnsZero()
        {
            var patient1 = new Patient("коваленко к.к.", "Грип", DateTime.Today, 5);
            var patient2 = new Patient("Коваленко К.К.", "Грип", DateTime.Today, 5);
            var comparer = new PatientNameComparer();
            Assert.AreEqual(0, comparer.Compare(patient1, patient2), "Порівняння має бути нечутливим до регістру.");
        }
        
        [TestMethod]
        public void Compare_UkrainianLetterI_IsOrderedByAlphabet()
        {
            var patient1 = new Patient("Бондаренко Б.Б.", "Грип", DateTime.Today, 5);
            var patient2 = new Patient("Іваненко І.І.", "Грип", DateTime.Today, 5);
            var comparer = new PatientNameComparer();
            Assert.IsTrue(comparer.Compare(patient1, patient2) < 0, "'Бондаренко' має бути перед 'Іваненко'.");
        }
        
        [TestMethod]
        public void Compare_WhenFirstPatientIsNull_ReturnsZero()
        {
            var patient2 = new Patient("Петренко П.П.", "Грип", DateTime.Today, 5);
            var comparer = new PatientNameComparer();
            Assert.AreEqual(0, comparer.Compare(null, patient2), "Порівняння null-об'єкта з іншим має повертати 0.");
        }
        [TestMethod]
        public void Compare_WhenSecondPatientIsNull_ReturnsZero()
        {
            var patient1 = new Patient("Петренко П.П.", "Грип", DateTime.Today, 5);
            var comparer = new PatientNameComparer();
            Assert.AreEqual(0, comparer.Compare(patient1, null), "Порівняння об'єкта з null-об'єктом має повертати 0.");
        }
        [TestMethod]
        public void Compare_WhenBothPatientsAreNull_ReturnsZero()
        {
            var comparer = new PatientNameComparer();
            Assert.AreEqual(0, comparer.Compare(null, null), "Порівняння двох null-об'єктів має повертати 0.");
        }
    }
}
