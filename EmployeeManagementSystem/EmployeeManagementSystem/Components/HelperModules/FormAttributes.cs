namespace EmployeeManagementSystem.Components.HelperModules
{
    public abstract class FormAttributes
    {
        public static Dictionary<string, object> passwordAttribute = new Dictionary<string, object> { { "type", "password" } };
        public static Dictionary<string, object> emailAttribute = new Dictionary<string, object> { { "type", "email" } };
        public static Dictionary<string, object> numberAttribute = new Dictionary<string, object> { { "type", "number" } };
    }
}
