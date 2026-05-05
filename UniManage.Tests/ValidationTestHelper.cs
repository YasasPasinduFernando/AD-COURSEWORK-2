using System.ComponentModel.DataAnnotations;

namespace UniManage.Tests;

internal static class ValidationTestHelper
{
    public static IList<ValidationResult> ValidateAll(object instance)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(instance);
        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);
        return results;
    }
}
