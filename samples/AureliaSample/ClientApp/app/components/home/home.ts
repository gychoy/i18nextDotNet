import { autoinject } from "aurelia-framework";
import { I18N } from "aurelia-i18n";
import * as moment from 'moment';

@autoinject
export class Home {
    public today: Date;
    public title: string;

    constructor(private i18n: I18N) {
        this.title = i18n.tr("page.title");
        this.today = new Date();
    }
}
