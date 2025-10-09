using System.Runtime.Serialization;

namespace SergiyE.StatusPageIoApi {
  public enum SortDirection {
    [EnumMember(Value = @"asc")]
    Asc = 0,

    [EnumMember(Value = @"desc")]
    Desc = 1,
  }
}