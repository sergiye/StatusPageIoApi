using System.Runtime.Serialization;

namespace SergiyE.StatusPageIoApi {
  public enum PageBranding {
    [EnumMember(Value = "basic")]
    Basic = 0,

    [EnumMember(Value = "premium")]
    Premium = 1,
  }
}