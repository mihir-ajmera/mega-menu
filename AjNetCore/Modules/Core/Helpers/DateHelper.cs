using System;

namespace AjNetCore.Modules.Core.Helpers
{
    public class DateHelper
    {
        public static int GetAge(DateTime? birthDate)
        {
            if (birthDate == null)
            {
                return 0;
            }

            var today = DateTime.Now;
            var age = today.Year - birthDate?.Year;

            if (today.Month < birthDate?.Month || (today.Month == birthDate?.Month && today.Day < birthDate?.Day))
            { age--; }

            return age ?? 0;
        }
    }
}