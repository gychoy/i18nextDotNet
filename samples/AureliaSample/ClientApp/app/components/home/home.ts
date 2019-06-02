import { autoinject } from "aurelia-framework";
import { I18N } from "aurelia-i18n";
import * as moment from 'moment';
import * as numeral from 'numeral';

@autoinject
export class Home {
    public today: Date;
    public title: string;
    public someMoneyAmount: number;
    public anotherMoneyAmount: string;

    constructor(private i18n: I18N) {
        this.title = this.i18n.i18next.t("title", "Page Title fallback");
        this.today = new Date();
        this.someMoneyAmount = 10.01;
        this.anotherMoneyAmount = numeral(this.someMoneyAmount).format('($0,0.00)');
    }
}
