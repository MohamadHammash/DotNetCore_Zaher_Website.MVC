using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zaher.Ui.Helpers
{
    public static class RolesInArabic
    {
        public static string Admin => "مشرف";
        public static string Editor => "محرر";
        public static string Moderator => "مراقب";
        public static string SuperAdmin => "المشرف العام";


        public static string ConvertRoleIntoArabic(string role)
        {
            string roleArabic = role switch
            {
                "Admin" => Admin,
                "Editor" => Editor,
                "Moderator" => Moderator,
                "SuperAdmin" => SuperAdmin,
                _ => "غير مصنف",
            };
            return roleArabic;
        }
    }
}
