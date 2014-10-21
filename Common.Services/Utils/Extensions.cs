using System.ServiceModel.Description;

namespace Common.Services.Utils
{
    static class Extensions
    {
        public static string GetHeaderType(this MessageHeaderDescription header)
        {
            return (string)ReflectionUtils.GetValue(header, "BaseType");
        }
    }
}
