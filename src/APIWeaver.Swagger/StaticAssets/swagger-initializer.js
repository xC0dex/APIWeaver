fetch('./configuration.json')
    .then(response => response.json())
    .then(({title, uiOptions, additionalUiOptions, oAuth2Options}) => {
        document.title = title;
        if (additionalUiOptions.darkMode) {
            appendHeaderContent('<link rel="stylesheet" type="text/css" href="./dark-mode.css" />');
        }
        uiOptions.dom_id = '#swagger-ui';
        uiOptions.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
        uiOptions.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
        uiOptions.layout = 'StandaloneLayout';
        window.ui = SwaggerUIBundle(uiOptions);
        oAuth2Options && window.ui.initOAuth(oAuth2Options);
    });

function appendHeaderContent(content) {
    document.getElementsByTagName('head')[0].insertAdjacentHTML('beforeend', content);
}