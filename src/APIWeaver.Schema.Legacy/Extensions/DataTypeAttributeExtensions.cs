using System.ComponentModel.DataAnnotations;

namespace APIWeaver.Schema.Extensions;

internal static class DataTypeAttributeExtensions
{
    public static string? ToSchemaFormat(this DataTypeAttribute attribute)
    {
        return attribute.DataType switch
        {
            DataType.Custom => attribute.CustomDataType,
            DataType.Date => "date",
            DataType.Time => "time",
            DataType.DateTime => "date-time",
            DataType.Duration => "duration",
            DataType.PhoneNumber => "tel",
            DataType.Currency => "currency",
            DataType.Text => "text",
            DataType.Html => "html",
            DataType.MultilineText => "multiline",
            DataType.EmailAddress => "email",
            DataType.Password => "password",
            DataType.Url => "uri",
            DataType.ImageUrl => "uri",
            DataType.CreditCard => "credit-card",
            DataType.PostalCode => "postal-code",
            DataType.Upload => "binary",
            _ => null
        };
    }
}