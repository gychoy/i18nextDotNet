import 'isomorphic-fetch';
import { Aurelia, PLATFORM } from 'aurelia-framework';
import { HttpClient } from 'aurelia-fetch-client';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';
import { I18N, TCustomAttribute } from 'aurelia-i18n';
import Backend from 'i18next-xhr-backend';
import * as moment from 'moment';
declare const IS_DEV_BUILD: boolean; // The value is supplied by Webpack during the build

export function configure(aurelia: Aurelia) {
    aurelia.use.standardConfiguration();

    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    }


    aurelia.use.plugin(PLATFORM.moduleName('aurelia-i18n'), (instance: any) => {        

        // add aliases for 't' attribute
        var aliases = ['t', 'i18n'];
        TCustomAttribute.configureAliases(aliases);

        // register backend plugin
        instance.i18next.use(Backend);

        // adapt options to your needs (see http://i18next.com/docs/options/)
        // make sure to return the promise of the setup method, in order to guarantee proper loading
        return instance.setup({
            backend: {                                  // <-- configure backend settings
                crossDomain: true,
                customHeaders: {
                    'Accept' : '*/*'
                },
                loadPath: '/locales/{{lng}}/{{ns}}.json', // <-- XHR settings for where to get the files from
            },
            interpolation: {
                format: function (value:any, format:any, lng:any) {
                    if (value instanceof Date)
                        return moment(value).format(format);

                    return value;
                }
            },
            attributes: aliases,
            lng: 'en',
            fallbackLng: 'en',
            debug: true
        });
    });

    new HttpClient().configure(config => {
        const baseUrl = document.getElementsByTagName('base')[0].href;
        config.withBaseUrl(baseUrl);
    });

    aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app/components/app/app')));
}
