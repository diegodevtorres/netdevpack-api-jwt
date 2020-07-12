using Login.Lib.Converts;
using System.ComponentModel;

namespace Login.Lib.Enumerators
{
    public enum CustomClaimTypesEnum
    {
        [Description("Administrator")]
        [StringValue("administrator")]
        Administrator = 0,

        [Description("Account")]
        [StringValue("account")]
        Account = 1
    }
}
