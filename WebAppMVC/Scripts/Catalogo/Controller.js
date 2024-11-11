/// <reference path=""~/angular.js" />  
/// <reference path=""~/angular.min.js" />   
/// <reference path="Modules.js" />   
/// <reference path="Services.js" />   


'use strict';
app.controller('GeneralController', ['$scope', 'GeneralService', function ($scope, GeneralService) {

    $scope.TipoSolicitudArt = [];
    $scope.EstadoSolicitudArt = [];
    //$scope.ids = [];

    $scope.Articulos = [];
    $scope.gridapi = {};
    $scope.selectedUserIds = [];

    $scope.selectedEstadoSolicitudArtCodigos = [];


    $scope.example2model = [];
    $scope.example2data = [{ id: 1, label: "David" }, { id: 2, label: "Jhon" }, { id: 3, label: "Danny" }];
    $scope.example2settings = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };



    $scope.example1model = [];
    $scope.example1data = [{ id: 1, label: "David" }, { id: 2, label: "Jhon" }, { id: 3, label: "Danny" }];


    GeneralService.getCatalogo('tbl_TipoSolArticulo').then(function (results) {

        $scope.TipoSolicitudArt = results.data;

    }, function (error) {

    });

    GeneralService.getCatalogo('tbl_EstadoSolicitud_Art').then(function (results) {

        $scope.EstadoSolicitudArt = results.data;


    }, function (error) {

    });



    $scope.config = {
        datatype: "local",
        height: 'auto',
        width: 'auto',
        colNames: ['IdSolicitud', 'TipoSolArticulo', 'CodSapProveedor', 'CodSapContacto', 'CantidadArticulos', 'Fecha', 'EstadoSolicitud', 'Usuario'],
        colModel: [
                            { name: 'idSolicitud', index: 'idSolArticulo', editable: true, width: 150 },
                            { name: 'tipoSolArticulo', index: 'tipoSolArticulo', editable: true, width: 150 },
                            { name: 'codSapProveedor', index: 'codSapProveedor', width: 150 },
                            { name: 'codSapContacto', index: 'codSapContacto', editable: true, width: 150 },
                            { name: 'cantidadArticulos', index: 'cantidadArticulos', editable: true, width: 150 },
                            { name: 'fecha', index: 'fecha', editable: true, width: 150 },
                            { name: 'estadoSolicitud', index: 'estadoSolicitud', editable: true, width: 150 },
                            { name: 'usuario', index: 'usuario', editable: true, width: 150 }
        ],
        multiselect: true,
        caption: "Articulos",
        loadComplete: function () {
            var table = this;
            setTimeout(function () {
                styleCheckbox(table);

                updateActionIcons(table);
                updatePagerIcons(table);

            }, 0);
        },
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30],
        autowidth: true,
        formatter: 'actions',
        formatoptions: {
            keys: true,
            editformbutton: true
        }
    }


    function getAllSelectOptions() {
        var states = {
            '1': 'Alabama', '2': 'California', '3': 'Florida',
            '4': 'Hawaii', '5': 'London', '6': 'Oxford'
        };

        return states;

    }

    function updatePagerIcons(table) {
        var replacement =
        {
            'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
            'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
            'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
            'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'
        };
        $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })
    }
    function styleCheckbox(table) {

        $(table).find('input:checkbox').addClass('ace')
        .wrap('<label />')
        .after('<span class="lbl align-top" />')


        $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
        .find('input.cbox[type=checkbox]').addClass('ace')
        .wrap('<label />').after('<span class="lbl align-top" />');

    }
    function updateActionIcons(table) {

        var replacement =
        {
            'ui-ace-icon fa fa-pencil': 'ace-icon fa fa-pencil blue',
            'ui-ace-icon fa fa-trash-o': 'ace-icon fa fa-trash-o red',
            'ui-icon-disk': 'ace-icon fa fa-check green',
            'ui-icon-cancel': 'ace-icon fa fa-times red'
        };
        $(table).find('.ui-pg-div span.ui-icon').each(function () {
            var icon = $(this);
            var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
            if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
        })




    }

    $scope.loadrecord = function () {

      
        GeneralService.getArticulos().then(function (results) {
            $scope.Articulos = results.data;

            $scope.gridapi.insert($scope.Articulos);

        }, function (error) {

        });



    }

    $scope.verseleccion = function () {

        var cadena = '';

        var index;
        var a = $scope.example2model;
        for (index = 0; index < a.length; ++index) {
            cadena = cadena +' - '+ a[index].id;
        }


        alert(cadena);



    }


    



}
]);

