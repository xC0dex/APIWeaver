fetch('./configuration.json')
    .then(response => response.json())
    .then(({title, swaggerOptions, oAuth2Options}) => {
        document.title = title;
        swaggerOptions.dom_id = '#swagger-ui';
        swaggerOptions.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
        swaggerOptions.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
        swaggerOptions.layout = 'StandaloneLayout';
        window.ui = SwaggerUIBundle(swaggerOptions);
        oAuth2Options && window.ui.initOAuth(oAuth2Options);
    });