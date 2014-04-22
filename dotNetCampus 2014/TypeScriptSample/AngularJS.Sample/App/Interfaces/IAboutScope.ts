/// <reference path='../_all.ts' />

module myControllers {
    export interface IAboutScope extends ng.IScope {
        data: myModels.Person[];
        name: string;
    }
}