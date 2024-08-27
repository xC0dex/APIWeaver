namespace APIWeaver.Schema.Extensions;

internal static class FieldInfoExtensions
{
    public static bool IsNullable(this FieldInfo fieldInfo, bool checkNullableAnnotationForReferenceTypes)
    {
        if (!fieldInfo.FieldType.IsValueType)
        {
            if (checkNullableAnnotationForReferenceTypes)
            {
                var nullabilityInfo = new NullabilityInfoContext().Create(fieldInfo);
                return nullabilityInfo.ReadState == NullabilityState.Nullable;
            }

            return true;
        }

        return Nullable.GetUnderlyingType(fieldInfo.FieldType) is not null;
    }
}