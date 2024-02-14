namespace APIWeaver.Schema.Extensions;

internal static class PropertyInfoExtensions
{
    public static bool IsNullable(this PropertyInfo propertyInfo, bool checkNullableAnnotationForReferenceTypes)
    {
        if (!propertyInfo.PropertyType.IsValueType)
        {
            if (checkNullableAnnotationForReferenceTypes)
            {
                var nullabilityInfo = new NullabilityInfoContext().Create(propertyInfo);
                return nullabilityInfo.ReadState == NullabilityState.Nullable;
            }

            return true;
        }

        return Nullable.GetUnderlyingType(propertyInfo.PropertyType) is not null;
    }

    public static bool IsPublic(this PropertyInfo propertyInfo) => (propertyInfo.GetMethod is not null && propertyInfo.GetMethod.IsPublic) || (propertyInfo.SetMethod is not null && propertyInfo.SetMethod.IsPublic);

    public static bool IsReadOnly(this PropertyInfo propertyInfo) => propertyInfo is {CanRead: true, CanWrite: false};

    public static bool IsWriteOnly(this PropertyInfo propertyInfo) => propertyInfo is {CanRead: false, CanWrite: true};
}