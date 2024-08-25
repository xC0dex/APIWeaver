fetch('./configuration.json')
    .then(response => response.json())
    .then(({title, uiOptions, additionalUiOptions, oAuth2Options}) => {
        document.title = title;
        if (additionalUiOptions.darkMode) {
            appendHeaderContent('<link rel="stylesheet" type="text/css" href="./dark-mode.css" />');
        }

        additionalUiOptions.stylesheets.forEach((href) => appendHeaderContent(`<link rel="stylesheet" type="text/css" href="${href}" />`));
        additionalUiOptions.scripts.forEach((src) => document.body.insertAdjacentHTML('beforeend', `<script src="${src}"></script>`));
        uiOptions.dom_id = '#swagger-ui';
        uiOptions.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
        uiOptions.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
        uiOptions.layout = 'StandaloneLayout';
        window.ui = SwaggerUIBundle(uiOptions);
        oAuth2Options && window.ui.initOAuth(oAuth2Options);
    });

function appendHeaderContent(content) {
    document.head.insertAdjacentHTML('beforeend', content);
}