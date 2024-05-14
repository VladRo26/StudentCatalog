using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentCatalog.Logic
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> FilteredUserTypes()
        {
            var values = Enum.GetValues(typeof(UserType))
                             .Cast<UserType>()
                             .Where(e => e != UserType.UtilizatorNelogat && e != UserType.None)
                             .Select(e => new SelectListItem
                             {
                                 Text = e.ToString(),
                                 Value = ((int)e).ToString()
                             });

            return values;
        }
    }
}
