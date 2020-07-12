using Login.Lib.Converts;
using System.ComponentModel;

namespace Login.Lib.Enumerators
{
    public enum RoleEnum
    {
        [Description("Admin")]
        [StringValue("admin")]
        Admin = 0,
        [Description("BeautySalon")]
        [StringValue("beautySalon")]
        BeautySalon = 1
    }
}
