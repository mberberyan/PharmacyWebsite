using System;
using System.Data;
using System.Web.Security;

namespace Melon.Components.CMS
{
    /// <summary>
    /// Implements CMS role provider class using ASP.NET Roles.
    /// </summary>
    public class ASPNETCMSRoleProvider : Melon.Components.CMS.Providers.RoleProvider
    {
        public override Melon.Components.CMS.RoleDataTable List()
        {
            Melon.Components.CMS.RoleDataTable roles = new Melon.Components.CMS.RoleDataTable();

            string[] roleCodes = Roles.GetAllRoles();
            foreach (string code in roleCodes)
            {
                DataRow row = roles.NewRow();
                row["Code"] = code;
                //The name of the role is the property displayed in CMS interface.
                //You could use it to display more meaningful name than the code.
                //You could use resources files to  store the names of the roles: Convert.ToString(HttpContext.GetGlobalResourceObject("Resource", code));
                row["Name"] = code;

                roles.Rows.Add(row);
            }

            return roles;
        }
    }
}
