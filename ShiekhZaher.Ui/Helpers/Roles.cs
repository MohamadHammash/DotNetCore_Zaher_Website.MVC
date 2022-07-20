using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zaher.Ui.Helpers
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Editor = "Editor";
        public const string Moderator = "Moderator";
        public const string AllRoles = "SuperAdmin,Admin,Editor,Moderator";
        public const string SuperAdminAdminEditor = "SuperAdmin,Admin,Editor";
        public const string SuperAdminAdmin = "SuperAdmin,Admin";
        public const string SuperAdminEditor = "SuperAdmin,Editor";
        public const string SuperAdminModerator = "SuperAdmin,Moderator";
        public const string AdminEditorModerator = "Admin,Editor,Moderator";
        public const string AdminEditor = "Admin,Editor";
        public const string EditorModerator = "Editor,Moderator";

    }
}
