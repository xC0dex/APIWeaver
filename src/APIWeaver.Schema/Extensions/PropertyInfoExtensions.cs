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
}