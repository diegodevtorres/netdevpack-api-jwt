using Login.Lib.Converts;
using System.ComponentModel;

namespace Login.Lib.Enumerators
{
    public enum PermissionEnum
    {
        [Description("View")]
        [StringValue("view")]
        View = 0,
        [Description("Create")]
        [StringValue("create")]
        Create = 1,
        [Description("Update")]
        [StringValue("update")]
        Update = 2,
        [Description("Manager")]
        [StringValue("manager")]
        Manager = 3
    }
}
