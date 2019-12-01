'use strict';

var app = angular.module('app', [
    'blockUI'
]);

app.config(function(blockUIConfig){
    blockUIConfig.message = 'Cargando';
});

app.directive('ngEnter', function(){
    return function(scope,element,attrs){
        element.bind('keydown keypress',function(event){
            if(event.which === 13){
                scope.$apply(function(){
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
});

app.run(function($rootScope,$sce, $filter) 
{    
    $rootScope.today=function(language)
    {
        var date;
        var date_today= new Date();
        var dd=date_today.getDate();
        var mm=(date_today.getMonth()+1);
        if(dd<10){dd='0'+dd;} 
        if(mm<10){mm='0'+mm;} 
        switch(language){
            case 'es':
                date=dd+'/'+mm+'/'+date_today.getFullYear();
                break;
            case 'en':
                date=date_today.getFullYear()+"-"+mm+'-'+dd  ;
                break;
            default:date=dd+'/'+mm+'/'+date_today.getFullYear();
        }
        return date;
    };

    $rootScope.formatDate=function(dateinput, language, type)
    {
        var response='';
        if(dateinput !== undefined )
        {
            var dateInput=new Date(dateinput);
            if(type===undefined)
                var date = new Date(dateInput.getFullYear(),(dateInput.getMonth()+1),(dateInput.getDate()+1));
            else
                var date = new Date(dateInput.getFullYear(),(dateInput.getMonth()+1),(dateInput.getDate()));
            var dd=date.getDate();
            var mm=date.getMonth();
            if(dd<10){dd='0'+dd;} 
            if(mm<10){mm='0'+mm;} 
            switch(language){
                case 'es':
                    response=dd+"/"+mm+"/"+date.getFullYear();
                    break;
            }
        }        
        return response;
    };

    $rootScope.soloNumeros = function(e)
    {
        var key = window.Event ? e.which : e.keyCode
        return (key >= 48 && key <= 57)
    };
});