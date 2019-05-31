import { autoinject } from "aurelia-framework";
import { I18N } from "aurelia-i18n";
import * as moment from 'moment';

@autoinject
export class Home {
    public today: Date;
    public title: string;

    constructor(private i18n: I18N) {
        this.title = this.i18n.i18next.t("page.title1", "Page Title");
        this.today = new Date();
    }
}
