fetch('./configuration.json')
    .then(response => response.json())
    .then(({title, uiOptions, oAuth2Options}) => {
        document.title = title;
        uiOptions.dom_id = '#swagger-ui';
        uiOptions.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
        uiOptions.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
        uiOptions.layout = 'StandaloneLayout';
        window.ui = SwaggerUIBundle(uiOptions);
        oAuth2Options && window.ui.initOAuth(oAuth2Options);
    });