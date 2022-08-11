
namespace CarWashShopAPI.Helpers
{
    public static class CustomFunctions 
    {
        public static string GetUserName(this string id, string userName)
        {
            string text = id.Substring(0, userName.Length);
            return text.ToUpper(); 
        }

    }
}
