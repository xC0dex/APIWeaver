namespace APIWeaver;

/// <summary>
/// Extension methods for <see cref="SwaggerUiOptions" />.
/// </summary>
public static class SwaggerUiOptionsExtensions
{
    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.DeepLinking" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="deepLinking">The DeepLinking value to set.</param>
    public static SwaggerOptions WithDeepLinking(this SwaggerOptions options, bool deepLinking)
    {
        options.DeepLinking = deepLinking;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.DisplayOperationId" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="displayOperationId">The DisplayOperationId value to set.</param>
    public static SwaggerOptions WithDisplayOperationId(this SwaggerOptions options, bool displayOperationId)
    {
        options.DisplayOperationId = displayOperationId;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.DefaultModelsExpandDepth" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="defaultModelsExpandDepth">The DefaultModelsExpandDepth value to set.</param>
    public static SwaggerOptions WithDefaultModelsExpandDepth(this SwaggerOptions options, int defaultModelsExpandDepth)
    {
        options.DefaultModelsExpandDepth = defaultModelsExpandDepth;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.DefaultModelExpandDepth" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="defaultModelExpandDepth">The DefaultModelExpandDepth value to set.</param>
    public static SwaggerOptions WithDefaultModelExpandDepth(this SwaggerOptions options, int defaultModelExpandDepth)
    {
        options.DefaultModelExpandDepth = defaultModelExpandDepth;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.DisplayRequestDuration" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="displayRequestDuration">The DisplayRequestDuration value to set.</param>
    public static SwaggerOptions WithDisplayRequestDuration(this SwaggerOptions options, bool displayRequestDuration)
    {
        options.DisplayRequestDuration = displayRequestDuration;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.MaxDisplayedTags" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="maxDisplayedTags">The MaxDisplayedTags value to set.</param>
    public static SwaggerOptions WithMaxDisplayedTags(this SwaggerOptions options, int? maxDisplayedTags)
    {
        options.MaxDisplayedTags = maxDisplayedTags;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.ShowExtensions" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="showExtensions">The ShowExtensions value to set.</param>
    public static SwaggerOptions WithShowExtensions(this SwaggerOptions options, bool showExtensions)
    {
        options.ShowExtensions = showExtensions;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.ShowCommonExtensions" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="showCommonExtensions">The ShowCommonExtensions value to set.</param>
    public static SwaggerOptions WithShowCommonExtensions(this SwaggerOptions options, bool showCommonExtensions)
    {
        options.ShowCommonExtensions = showCommonExtensions;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.TryItOutEnabled" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="tryItOutEnabled">The TryItOutEnabled value to set.</param>
    public static SwaggerOptions WithTryItOutEnabled(this SwaggerOptions options, bool tryItOutEnabled)
    {
        options.TryItOutEnabled = tryItOutEnabled;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.RequestSnippetsEnabled" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="requestSnippetsEnabled">The RequestSnippetsEnabled value to set.</param>
    public static SwaggerOptions WithRequestSnippetsEnabled(this SwaggerOptions options, bool requestSnippetsEnabled)
    {
        options.RequestSnippetsEnabled = requestSnippetsEnabled;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.OAuth2RedirectUrl" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="oAuth2RedirectUrl">The OAuth2RedirectUrl value to set.</param>
    public static SwaggerOptions WithOAuth2RedirectUrl(this SwaggerOptions options, string oAuth2RedirectUrl)
    {
        options.OAuth2RedirectUrl = oAuth2RedirectUrl;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerUiOptions.ValidatorUrl" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerUiOptions" />.</param>
    /// <param name="validatorUrl">The ValidatorUrl value to set.</param>
    public static SwaggerOptions WithValidatorUrl(this SwaggerOptions options, string? validatorUrl)
    {
        options.ValidatorUrl = validatorUrl;
        return options;
    }
}