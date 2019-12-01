var app = angular.module('app', [
    'datatables',
    'datatables.colreorder',
    'datatables.buttons',
    'datatables.columnfilter',
    'datatables.light-columnfilter',
    'datatables.fixedcolumns',
    'ngAnimate',
    'ngResource',
    'ui.select2',
    'ui.bootstrap',
    'ngSanitize',
    'blockUI',
    'angucomplete-alt',
    'ngPrint'
]);

app.config(function (blockUIConfig) {
    blockUIConfig.message = '';
});

app.filter('startFrom', function () {
    return function (input, start) {
        var outPut = [];
        if (input) {
            start = +start;
            outPut = input.slice(start);
            return outPut;
        }
        return outPut;
    };
});

app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind('keydown keypress', function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
});

app.run(function ($rootScope, $sce, $filter, DTOptionsBuilder, DTDefaultOptions) {
    
    $rootScope.pager = {
        objects: [],
        currentPage: 1,
        pageSize: 20,
        pageDivide: 6,
        numberPage: 0,
        pages: []
    };

    $rootScope.basic = {};
    $rootScope.filterPager = {};

    $rootScope.datesingles = function () {
        $rootScope.basic.date.months = [{ 'id': '1', 'name': 'Enero' }, { 'id': '2', 'name': 'Febrero' }, { 'id': '3', 'name': 'Marzo' }, { 'id': '4', 'name': 'Abril' }, { 'id': '5', 'name': 'Mayo' }, { 'id': '6', 'name': 'Junio' }, { 'id': '7', 'name': 'Julio' },
                                        { 'id': '8', 'name': 'Agosto' }, { 'id': '9', 'name': 'Septiembre' }, { 'id': '10', 'name': 'Octubre' }, { 'id': '11', 'name': 'Noviembre' }, { 'id': '12', 'name': 'Diciembre' }];
        $rootScope.basic.date.years = $rootScope.setNumbers(1900, new Date().getFullYear());
    };

    $rootScope.setDateDays = function (year, month) {
        if (year != undefined && month != undefined) {
            var n = 0;
            if (['1', '3', '5', '7', '8', '10', '12'].indexOf(month) >= 0)
                n = 31;
            else if (['4', '6', '9', '11'].includes(month))
                n = 30;
            else {
                if (month == 2)
                    n = (((year % 4) == 0 && (year % 100) != 0) || ((year % 400) == 0)) ? 29 : 28;
            }
            $rootScope.basic.date.days = $rootScope.setNumbers(1, n);
        } else {
            $rootScope.basic.date.days = [];
        }
    };

    //$rootScope.disabledTag = function(target,state=true){
    //    state?$(target).addClass('disabled'):$(target).removeClass('disabled');
    //};

    $rootScope.messageSuccess = function (message) {
        $.notify({ title: '<strong>Mensaje:</strong>', message: message }, { type: 'success' });
    }
    $rootScope.messageError = function (message) {
        $.notify({ title: '<strong>Mensaje:</strong>', message: message }, { type: 'danger' });
    }
    $rootScope.messageWarning = function (message) {
        $.notify({ title: '<strong>Mensaje:</strong>', message: message }, { type: 'warning' });
    }
    
    $rootScope.setNumbers = function (start, end, type) {
        var array = [];
        for (let i = start != undefined ? start : 0, n = end; i <= n; i++) {
            array.push(i);
        }
        return array;
    };

    $rootScope.changePage = function(number) {
        $rootScope.pager.currentPage = (number - 1);
    };

    $rootScope.numberOfPages = function(sizeList) {
        if (sizeList > 0) {
            $rootScope.pager.numberPage = Math.ceil(sizeList / $rootScope.pager.pageSize);
            for (var i = 0; i < $rootScope.pager.numberPage; i++) {
                $rootScope.pager.pages.push((i + 1));
            }
        }
    };

    $rootScope.searchFilter = function(dataList, inputSearch) {
        var out = $filter('filter')(dataList, inputSearch);
        $rootScope.pager.pages = [];
        $rootScope.numberOfPages(out.length);
        $rootScope.pager.objects = out;
        return out;
    };

    $rootScope.allowHtml = function(text) {
        if (text !== undefined && text !== null) {
            text = text.replace(/\n/g, "<br />");
            return $sce.trustAsHtml(text);
        }
        return null;
    };

    $rootScope.today = function(language) {
        var date;
        var date_today = new Date();
        var dd = date_today.getDate();
        var mm = (date_today.getMonth() + 1);
        if (dd < 10) { dd = '0' + dd; }
        if (mm < 10) { mm = '0' + mm; }
        switch (language) {
            case 'es':
                date = dd + '/' + mm + '/' + date_today.getFullYear();
                break;
            case 'en':
                date = date_today.getFullYear() + "-" + mm + '-' + dd;
                break;
            default:
                date = dd + '/' + mm + '/' + date_today.getFullYear();
        }
        return date;
    };

    $rootScope.date = function(input) {
        var custom_date = input.split('-');
        return new Date(custom_date[0], navigator.language=='en-US'?(parseInt(custom_date[1])-1):custom_date[1],custom_date[2]);
    };

    $rootScope.formatDate = function(dateinput, language, type) {
        var response = '';
        if (dateinput !== undefined) {
            var dateInput = new Date(dateinput);
            if (type === undefined)
                var date = new Date(dateInput.getFullYear(), (dateInput.getMonth() + 1), (dateInput.getDate() + 1));
            else
                var date = new Date(dateInput.getFullYear(), (dateInput.getMonth() + 1), (dateInput.getDate()));
            var dd = date.getDate();
            var mm = date.getMonth();
            if (dd < 10) { dd = '0' + dd; }
            if (mm < 10) { mm = '0' + mm; }
            switch (language) {
                case 'es':
                    response = dd + "/" + mm + "/" + date.getFullYear();
                    break;
            }
        }
        return response;
    };

    $rootScope.capitalize = function(string) {
        string = string.toLowerCase();
        var pieces = string.split(" ");
        if (pieces.length > 0) {
            for (var i = 0; i < pieces.length; i++) {
                var j = pieces[i].charAt(0).toUpperCase();
                pieces[i] = j + pieces[i].substr(1);
            }
        }
        // return string[0].toUpperCase() + string.slice(1);
        return pieces.join(" ");
    };

    $rootScope.zeroPad = function(num, places) {
        var zero = places - num.toString().length + 1;
        return Array(+(zero > 0 && zero)).join("0") + num;
    };

    $rootScope.soloNumeros = function(e) {
        var key = window.Event ? e.which : e.keyCode
        return (key >= 48 && key <= 57)
    };

    $rootScope.operDate = function(date, days) {
        date.setDate(date.getDate() + days);
        return date;
    };

    $rootScope.listUbigeo = function(cod_dep, cod_pro, cod_dis) {
        var params = {
            cod_dep: (cod_dep != undefined) ? cod_dep : 0,
            cod_pro: (cod_pro != undefined) ? cod_pro : 0,
            cod_dis: (cod_dis != undefined) ? cod_dis : 0
        };
        if (params.cod_dep === 0 && params.cod_pro === 0 && params.cod_dis === 0) {
            CommonService.getUbigeo(params).then(function(data) {
                if (data.data) {
                    if (params.cod_dep === 0) {
                        $rootScope.basic.department = data.data;
                    }
                }
            });
        }
        if (params.cod_dep > 0 && params.cod_pro === 0 && params.cod_dis === 0) {
            CommonService.getUbigeo(params).then(function(data) {
                if (data.data) {
                    if (params.cod_pro === 0) {
                        $rootScope.basic.province = data.data;
                    }
                }
            });
        }
        if (params.cod_dep > 0 && params.cod_pro > 0 && params.cod_dis === 0) {
            CommonService.getUbigeo(params).then(function(data) {
                if (data.data) {
                    if (params.cod_dis === 0) {
                        $rootScope.basic.district = data.data;
                    }
                }
            });
        }
    };

    $rootScope.dtOptions = DTOptionsBuilder.newOptions().withOption('aoColumnDefs', [{
        "bSearchable": false, "aTargets": ["not-search"]
    }]).withPaginationType('full_numbers').withLanguage({
        "sEmptyTable": "No hay Datos Disponibles",
        "sInfo": "Mostrando _START_ hasta _END_ de _TOTAL_ Filas",
        "sInfoEmpty": "Viendo 0 hasta 0 de 0 filas",
        "sInfoFiltered": "(filtrado de _MAX_ Filas)",
        "sInfoPostFix": "",
        "sInfoThousands": ",",
        "sLengthMenu": "Ver _MENU_ Filas",
        "sLoadingRecords": "Cargando...",
        "sProcessing": "Procesando...",
        "sSearch": "Buscar:",
        "sZeroRecords": "No se encontraron registros",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Ultimo",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": activado para ordenar columna ascendente",
            "sSortDescending": ": activado para ordenar columna descendente"
        }
    }).withColReorder();
});

app.directive('angucomplete', function ($parse, $http, $sce, $timeout) {
    return {
        restrict: 'EA',
        scope: {
            "id": "@id",
            "placeholder": "@placeholder",
            "selectedObject": "=selectedobject",
            "accion": "&accion",
            "modal": "@modal",
            "url": "@url",
            "dataField": "@datafield",
            "titleField": "@titlefield",
            "descriptionField": "@descriptionfield",
            "imageField": "@imagefield",
            "imageUri": "@imageuri",
            "inputClass": "@inputclass",
            "userPause": "@pause",
            "localData": "=localdata",
            "searchFields": "@searchfields",
            "minLengthUser": "@minlength",
            "matchClass": "@matchclass"
        },
        template: '<div class="angucomplete-holder row">' +
                        '<div class="col-md-10"><input id="{{id}}_value" ng-model="searchStr" type="text" placeholder="{{placeholder}}" class="{{inputClass}}" onmouseup="this.select();" ng-focus="resetHideResults()" ng-blur="hideResults()" /></div>' +
                        '<a ng-show="addElement" data-toggle="modal" data-target="#{{modal}}" class="btn btn-success btn-sm" ng-click="agregar()"><span class="glyphicon glyphicon-plus"></span></a>' +
                        '<div id="{{id}}_dropdown" class="angucomplete-dropdown" ng-if="showDropdown">' +
                            '<div class="angucomplete-searching" ng-show="searching">Buscando...</div>' +
                            /*'<div class="angucomplete-searching" ng-show="!searching && (!results || results.length == 0)">No se han encontrado resultados</div>' +*/
                            '<div class="angucomplete-row" ng-repeat="result in results" ng-mousedown="selectResult(result)" ng-mouseover="hoverRow()" ng-class="{\'angucomplete-selected-row\': $index == currentIndex}">' +
                                '<div ng-if="imageField" class="angucomplete-image-holder">' +
                                    '<img ng-if="result.image && result.image != \'\'" ng-src="{{result.image}}" class="angucomplete-image"/>' +
                                    '<div ng-if="!result.image && result.image != \'\'" class="angucomplete-image-default"></div>' +
                                '</div>' +
                                '<div class="angucomplete-title" ng-if="matchClass" ng-bind-html="result.title"></div>' +
                                '<div class="angucomplete-title" ng-if="!matchClass">{{ result.title }}</div>' +
                                '<div ng-if="result.description && result.description != \'\'" class="angucomplete-description">{{result.description}}</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>',

        link: function ($scope, elem, attrs) {
            $scope.lastSearchTerm = null;
            $scope.currentIndex = null;
            $scope.justChanged = false;
            $scope.searchTimer = null;
            $scope.hideTimer = null;
            $scope.searching = false;
            $scope.addElement = false;
            $scope.pause = 500;
            $scope.minLength = 3;
            $scope.searchStr = null;

            if ($scope.minLengthUser && $scope.minLengthUser != "") {
                $scope.minLength = $scope.minLengthUser;
            }

            if ($scope.userPause) {
                $scope.pause = $scope.userPause;
            }

            isNewSearchNeeded = function (newTerm, oldTerm) {
                return newTerm.length >= $scope.minLength && newTerm != oldTerm
            }

            $scope.processResults = function (responseData, str) {
                if (responseData && responseData.length > 0) {
                    $scope.results = [];
                    $scope.addElement = false;

                    var titleFields = [];
                    if ($scope.titleField && $scope.titleField != "") {
                        titleFields = $scope.titleField.split(",");
                    }

                    for (var i = 0; i < responseData.length; i++) {
                        // Get title variables
                        var titleCode = [];

                        for (var t = 0; t < titleFields.length; t++) {
                            titleCode.push(responseData[i][titleFields[t]]);
                        }

                        var description = "";
                        if ($scope.descriptionField) {
                            description = responseData[i][$scope.descriptionField];
                        }

                        var imageUri = "";
                        if ($scope.imageUri) {
                            imageUri = $scope.imageUri;
                        }

                        var image = "";
                        if ($scope.imageField) {
                            image = imageUri + responseData[i][$scope.imageField];
                        }

                        var text = titleCode.join(' ');
                        if ($scope.matchClass) {
                            var re = new RegExp(str, 'i');
                            var strPart = text.match(re)[0];
                            text = $sce.trustAsHtml(text.replace(re, '<span class="' + $scope.matchClass + '">' + strPart + '</span>'));
                        }

                        var resultRow = {
                            title: text,
                            description: description,
                            image: image,
                            originalObject: responseData[i]
                        }

                        $scope.results[$scope.results.length] = resultRow;
                    }


                } else {
                    $scope.results = [];
                    $scope.showDropdown = false;
                    $scope.addElement = true;

                }
            }

            $scope.searchTimerComplete = function (str) {
                // Begin the search

                if (str.length >= $scope.minLength) {
                    if ($scope.localData) {
                        var searchFields = $scope.searchFields.split(",");

                        var matches = [];

                        for (var i = 0; i < $scope.localData.length; i++) {
                            var match = false;

                            for (var s = 0; s < searchFields.length; s++) {
                                match = match || (typeof $scope.localData[i][searchFields[s]] === 'string' && typeof str === 'string' && $scope.localData[i][searchFields[s]].toLowerCase().indexOf(str.toLowerCase()) >= 0);
                            }

                            if (match) {
                                matches[matches.length] = $scope.localData[i];
                            }
                        }

                        $scope.searching = false;
                        $scope.processResults(matches, str);

                    } else {
                        $http.get($scope.url + str, {}).
                            success(function (responseData, status, headers, config) {
                                $scope.searching = false;
                                $scope.processResults((($scope.dataField) ? responseData[$scope.dataField] : responseData), str);
                            }).
                            error(function (data, status, headers, config) {
                                console.log("error");
                            });
                    }
                }
            }

            $scope.hideResults = function () {
                $scope.hideTimer = $timeout(function () {
                    $scope.showDropdown = false;
                }, $scope.pause);
            };

            $scope.resetHideResults = function () {
                if ($scope.hideTimer) {
                    $timeout.cancel($scope.hideTimer);
                };
            };

            $scope.hoverRow = function (index) {
                $scope.currentIndex = index;
            }

            $scope.keyPressed = function (event) {
                if (!(event.which == 38 || event.which == 40 || event.which == 13)) {
                    if (!$scope.searchStr || $scope.searchStr == "") {
                        $scope.showDropdown = false;
                        $scope.lastSearchTerm = null
                    } else if (isNewSearchNeeded($scope.searchStr, $scope.lastSearchTerm)) {
                        $scope.lastSearchTerm = $scope.searchStr
                        $scope.showDropdown = true;
                        $scope.currentIndex = 0;
                        $scope.results = [];

                        if ($scope.searchTimer) {
                            $timeout.cancel($scope.searchTimer);
                        }

                        $scope.searching = true;

                        $scope.searchTimer = $timeout(function () {
                            $scope.searchTimerComplete($scope.searchStr);
                        }, $scope.pause);
                    }
                } else {
                    event.preventDefault();
                }
            }

            $scope.selectResult = function (result) {
                if ($scope.matchClass) {
                    result.title = result.title.toString().replace(/(<([^>]+)>)/ig, '');
                }
                $scope.searchStr = $scope.lastSearchTerm = result.title;
                $scope.selectedObject = result.originalObject;
                $scope.showDropdown = false;
                $scope.results = [];
                //$scope.$apply();
            }

            var inputField = elem.find('input');

            inputField.on('keyup', $scope.keyPressed);

            elem.on("keyup", function (event) {
                if (event.which === 40) {
                    if ($scope.results && ($scope.currentIndex + 1) < $scope.results.length) {
                        $scope.currentIndex++;
                        $scope.$apply();
                        event.preventDefault;
                        event.stopPropagation();
                    }

                    $scope.$apply();
                } else if (event.which == 38) {
                    if ($scope.currentIndex >= 1) {
                        $scope.currentIndex--;
                        $scope.$apply();
                        event.preventDefault;
                        event.stopPropagation();
                    }

                } else if (event.which == 13) {
                    if ($scope.results && $scope.currentIndex >= 0 && $scope.currentIndex < $scope.results.length) {
                        $scope.selectResult($scope.results[$scope.currentIndex]);
                        $scope.$apply();
                        event.preventDefault;
                        event.stopPropagation();
                    } else {
                        $scope.results = [];
                        $scope.showDropdown = false;
                        $scope.$apply();
                        event.preventDefault;
                        event.stopPropagation();
                    }

                } else if (event.which == 27) {
                    $scope.results = [];
                    $scope.showDropdown = false;
                    $scope.selectedObject = null;
                    $scope.$apply();
                } else if (event.which == 8) {
                    $scope.selectedObject = null;
                    $scope.$apply();
                } else if (event.which == 9) {
                    $scope.results = [];
                    $scope.$apply();
                }
            });
            $scope.agregar = function () {
                $scope.accion({ nombre: $scope.searchStr });
            }
        }
    };
});