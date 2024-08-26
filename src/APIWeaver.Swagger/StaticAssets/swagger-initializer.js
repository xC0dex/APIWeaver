fetch('./configuration.json')
    .then(response => response.json())
    .then(options => {
        document.title = options.title;
        if (options.darkMode) {
            document.head.insertAdjacentHTML('beforeend', '<link rel="stylesheet" type="text/css" href="./dark-mode.css" />');
        }

        options.stylesheets.forEach((href) => document.head.insertAdjacentHTML('beforeend', `<link rel="stylesheet" type="text/css" href="${href}" />`));
        options.scripts.forEach((src) => document.body.insertAdjacentHTML('beforeend', `<script src="${src}"></script>`));
        
        options.dom_id = '#swagger-ui';
        options.presets = [SwaggerUIBundle.presets.apis, SwaggerUIStandalonePreset];
        options.plugins = [SwaggerUIBundle.plugins.DownloadUrl];
        options.layout = 'StandaloneLayout';
        window.ui = SwaggerUIBundle(options);
        if (options.oAuth2Options) {
            window.ui.initOAuth(options.oAuth2Options);
        }
    });