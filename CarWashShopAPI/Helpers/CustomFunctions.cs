
namespace CarWashShopAPI.Helpers
{
    public static class CustomFunctions 
    {
        public static string GetUserName(this string id)
        {
            return id.Substring(36, id.Length - 36);
        }
    }
}
