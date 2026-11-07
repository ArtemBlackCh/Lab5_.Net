using System.Text.Json.Serialization;

namespace laba5
{
    public class Patient : IComparable<Patient>
    {
        private string _fullName = string.Empty;
        private string _disease = string.Empty;
        private DateTime _admissionDate;
        private int _workExperience;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ПІБ хворого не може бути пустим.", nameof(FullName));
                }
                _fullName = value;
            }
        }

        public string Disease
        {
            get { return _disease; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Назва хвороби не може бути пустою.", nameof(Disease));
                }
                _disease = value;
            }
        }

        public DateTime AdmissionDate
        {
            get { return _admissionDate; }
            set
            {
                if (value.Date > DateTime.Today)
                {
                    throw new ArgumentException("Дата надходження не може бути в майбутньому.", nameof(AdmissionDate));
                }
                if (value.Year < 1900)
                {
                    throw new ArgumentException("Дата надходження не може бути раніше 1900 року.", nameof(AdmissionDate));
                }
                _admissionDate = value;
            }
        }

        public int WorkExperience
        {
            get { return _workExperience; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Робочий стаж не може бути від'ємним.", nameof(WorkExperience));
                }
                if (value > 60)
                {
                    throw new ArgumentException("Робочий стаж не може перевищувати 60 років.", nameof(WorkExperience));
                }
                _workExperience = value;
            }
        }

        // Кількість днів на лікуванні (обчислювана властивість, у файл не записується)
        [JsonIgnore]
        public int DaysInTreatment
        {
            get { return GetDaysInTreatment(DateTime.Today); }
        }

        public Patient(string fullName, string disease, DateTime admissionDate, int workExperience)
        {
            // Конструктор просто викликає сеттери, що містять валідацію
            FullName = fullName;
            Disease = disease;
            AdmissionDate = admissionDate;
            WorkExperience = workExperience;
        }

        // Кількість повних днів між датою надходження та заданою датою
        // (дата передається параметром, щоб метод можна було протестувати)
        public int GetDaysInTreatment(DateTime today)
        {
            return (today.Date - AdmissionDate.Date).Days;
        }

        // Реалізація інтерфейсу IComparable (порівняння за датою надходження)
        public int CompareTo(Patient? other)
        {
            if (other == null)
            {
                return 1;
            }
            return AdmissionDate.CompareTo(other.AdmissionDate);
        }

        public override string ToString()
        {
            return $"ПІБ: {FullName}, Хвороба: {Disease}, Дата надходження: {AdmissionDate:dd.MM.yyyy}, Стаж: {WorkExperience}";
        }
    }
}
