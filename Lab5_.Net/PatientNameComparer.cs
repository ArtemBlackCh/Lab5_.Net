using System.Globalization;

namespace laba5
{
    public class PatientNameComparer : IComparer<Patient>
    {
        private static readonly CultureInfo _culture = new CultureInfo("uk-UA");

        public int Compare(Patient? x, Patient? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return string.Compare(x.FullName, y.FullName, _culture, CompareOptions.IgnoreCase);
        }
    }
}
