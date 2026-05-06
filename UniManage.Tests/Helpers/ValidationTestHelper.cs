using System.ComponentModel.DataAnnotations;

namespace UniManage.Tests.Helpers;

public static class ValidationTestHelper
{
    public static IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }
}
