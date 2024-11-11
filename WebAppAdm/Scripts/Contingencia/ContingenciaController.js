app.controller('ConsEnvPedController', ['$scope', 'ngAuthSettings', 'ConsEnvPedService', 'FileUploader', '$sce', 'authService', '$http', '$filter', 'SeguridadService', function ($scope, ngAuthSettings, ConsEnvPedService, FileUploader, $sce, authService, $http, $filter, SeguridadService) {
    var dateFormat = function () {
        var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
            timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
            timezoneClip = /[^-+\dA-Z]/g,
            pad = function (val, len) {
                val = String(val);
                len = len || 2;
                while (val.length < len) val = "0" + val;
                return val;
            };

        return function (date, mask, utc) {
            var dF = dateFormat;


            if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
                mask = date;
                date = undefined;
            }


            date = date ? new Date(date) : new Date;
            if (isNaN(date)) throw SyntaxError("invalid date");

            mask = String(dF.masks[mask] || mask || dF.masks["default"]);


            if (mask.slice(0, 4) == "UTC:") {
                mask = mask.slice(4);
                utc = true;
            }

            var _ = utc ? "getUTC" : "get",
                d = date[_ + "Date"](),
                D = date[_ + "Day"](),
                m = date[_ + "Month"](),
                y = date[_ + "FullYear"](),
                H = date[_ + "Hours"](),
                M = date[_ + "Minutes"](),
                s = date[_ + "Seconds"](),
                L = date[_ + "Milliseconds"](),
                o = utc ? 0 : date.getTimezoneOffset(),
                flags = {
                    d: d,
                    dd: pad(d),
                    ddd: dF.i18n.dayNames[D],
                    dddd: dF.i18n.dayNames[D + 7],
                    m: m + 1,
                    mm: pad(m + 1),
                    mmm: dF.i18n.monthNames[m],
                    mmmm: dF.i18n.monthNames[m + 12],
                    yy: String(y).slice(2),
                    yyyy: y,
                    h: H % 12 || 12,
                    hh: pad(H % 12 || 12),
                    H: H,
                    HH: pad(H),
                    M: M,
                    MM: pad(M),
                    s: s,
                    ss: pad(s),
                    l: pad(L, 3),
                    L: pad(L > 99 ? Math.round(L / 10) : L),
                    t: H < 12 ? "a" : "p",
                    tt: H < 12 ? "am" : "pm",
                    T: H < 12 ? "A" : "P",
                    TT: H < 12 ? "AM" : "PM",
                    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
                };

            return mask.replace(token, function ($0) {
                return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
            });
        };
    }();


    dateFormat.masks = {
        "default": "ddd mmm dd yyyy HH:MM:ss",
        shortDate: "m/d/yy",
        mediumDate: "mmm d, yyyy",
        longDate: "mmmm d, yyyy",
        fullDate: "dddd, mmmm d, yyyy",
        shortTime: "h:MM TT",
        mediumTime: "h:MM:ss TT",
        longTime: "h:MM:ss TT Z",
        isoDate: "yyyy-mm-dd",
        isoTime: "HH:MM:ss",
        isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
        isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
    };


    dateFormat.i18n = {
        dayNames: [
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        ],
        monthNames: [
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        ]
    };


    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };
    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Contador Grid
    $scope.ContGrPedido = "";

    //Grids
    $scope.grPedido = [];
    var _grPedido = [];
    $scope.pagesPed = [];
    $scope.pageContentPed = [];
    $scope.pOpcGrp2 = "T";
    var dateString  = new Date();
    var dateString1 = new Date();
    var d1 = dateString1.format("dd/mm/yyyy");
    dateString.setDate(dateString.getDate() - 15);
    //d.setDate(d.getDate() - 15);
    var d = dateString.format("dd/mm/yyyy");
    $scope.txtFechaDesde = d;
    $scope.txtFechaHasta = d1
    $scope.txtRuc = "";
    $scope.rdbProv = "1";
    $scope.rdbPedid = "0";
    $scope.txtCodigo = "";
    $scope.txtNumPedido = "";
    $scope.accion = 0;
    $scope.valor = "";
    $scope.listaAlmacenes = [];
    $scope.selecAlmacenes = [];
    $scope.SettingAlmacenes = {  displayProp: 'pNomAlmacen', idProp: 'pCodAlmacen', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };
    $scope.chkEstadosList = [];
    $scope.chkEstadosSelModel = [];
    $scope.chkEstadosSettings = { displayProp: 'detalle', idProp: 'codigo', enableSearch: false, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };
    $scope.rucEnvia = "";
    $scope.provEnvia = "";

    $scope.myPromise = ConsEnvPedService.getCatalogo('tbl_EstadosPedidos').then(function (results) {
        $scope.chkEstadosList = results.data;
       
        //LANZO CONSULTA AUTOMATICA AL CARGAR
        var list = $scope.chkEstadosList;
        for (var i = 0; i < list.length; i++) {
           
                $scope.chkEstadosSelModel.push({ id: list[i].codigo });
           
        }

    }, function (error) {
    });

    $scope.myPromise = ConsEnvPedService.getConsAlmacenes("4").then(function (results) {
        if (results.data.success) {
          
            var listAlmacen = results.data.root[0];
            
            $scope.listaAlmacenes = listAlmacen;
            for (var i = 0; i < listAlmacen.length; i++) {

                $scope.selecAlmacenes.push({ id: listAlmacen[i].pCodAlmacen });

            }
        

        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });



    $scope.ConfirmarEnviar = function () {

        $scope.allChecks = true;
        var sRuc = $scope.rucEnvia; 
       
        var sNombre = '';
        var listaUsuarios = [];
        var listaConstactos = [];
        $scope.resDgUsrsEnviar = [];
        $scope.allChecks = true;

        //Consultar Usuarios 
        $scope.myPromise = SeguridadService.getConsDatosLegAsociados(sRuc, sNombre).then(function (results) {
            if (results.data.success) {

                //listaUsuarios = results.data.root[0];
                listaUsuarios = $filter('filter')(results.data.root[0],
                                             { prolComercial: true }, true);

                if (listaUsuarios == undefined)
                    listaUsuarios = [];

            }
            //Consulta contactos
            debugger;  
            $scope.myPromise = SeguridadService.getContactoList($scope.provEnvia).then(function (response) {

                if (response.data.success) {
                   
                    //listaConstactos = response.data.root[0];
                    listaConstactos = $filter('filter')(response.data.root[0],
                                                { departamento: "0003" }, true);
                    if (listaConstactos == undefined)
                        listaConstactos = [];

                    for (var id = 0; id < listaUsuarios.length; id++) {
                        $scope.usrEnviar = {}
                        $scope.usrEnviar.id = $scope.resDgUsrsEnviar.length + 1;
                        $scope.usrEnviar.nombres = listaUsuarios[id].pApellidos + ' ' + listaUsuarios[id].pNombres;
                        $scope.usrEnviar.correoEnviar = listaUsuarios[id].pCorreo;
                        $scope.usrEnviar.isCheck = true;
                        $scope.resDgUsrsEnviar.push($scope.usrEnviar);
                    }

                    for (var id = 0; id < listaConstactos.length; id++) {

                        $scope.usrEnviar = {}
                        $scope.usrEnviar.id = $scope.resDgUsrsEnviar.length + 1;
                        $scope.usrEnviar.nombres = listaConstactos[id].apellido1 + ' ' + listaConstactos[id].apellido2 +
                                                   listaConstactos[id].nombre1 + ' ' + listaConstactos[id].nombre2;
                        $scope.usrEnviar.correoEnviar = listaConstactos[id].email;
                        $scope.usrEnviar.isCheck = true;
                        $scope.resDgUsrsEnviar.push($scope.usrEnviar);
                    }
                    $('#usuariosEnvia').modal('show');


                }

            },
            function (err) {
                $scope.MenjError = "Error en comunicación: SeguridadService.getContactoList()";
                $('#idMensajeError').modal('show');
            });


        }, function (error) {
            $scope.showMessage('E', "Error en comunicación: SeguridadService.getConsDatosLegAsociados()");
        });







    }

    $scope.marcaChecks = function (valor) {


        for (var idx = 0 ; idx < $scope.resDgUsrsEnviar.length; idx++) {
            var update = $scope.resDgUsrsEnviar[idx];
            update.isCheck = valor;
        }
    }
    $("#btnMensajeOK").click(function () {
        window.location = 'frmConsEnvPedidos';
    });

    $scope.CargaConsulta = function (tipo) {
       
        $scope.accion = 0;
        if ($scope.rdbProv == "0" ) {
            $scope.txtRuc = "";
            $scope.txtCodigo = "";
            $scope.txtRazonSocial = "";
        }
        if ($scope.rdbProv == "1") {
            
            $scope.txtCodigo = "";
            $scope.txtRazonSocial = "";
        }
        if ($scope.rdbProv == "2") {
            $scope.txtRuc = "";
            $scope.txtRazonSocial = "";
           
        }
        if ($scope.rdbProv == "3") {
            $scope.txtRuc = "";
            $scope.txtCodigo = "";

        }
        if ($scope.rdbPedid == "0") {
            $scope.txtNumPedido = "";           
        }
        
        
        //Valida Rango de fecha
        if ($scope.txtFechaDesde == null || $scope.txtFechaDesde == "") {
            $scope.showMessage('I', 'Seleccione la fecha desde del rango a consultar.');
            return;
        }
        if ($scope.txtFechaHasta == null || $scope.txtFechaHasta == "") {
            $scope.showMessage('I', 'Seleccione la fecha hasta del rango a consultar.');
            return;
        }
        var strDate1 = $scope.txtFechaDesde.split("/");
        var strDate2 = $scope.txtFechaHasta.split("/");
        var date1 = new Date(strDate1[2], strDate1[1]-1, strDate1[0]);
        var date2 = new Date(strDate2[2], strDate2[1] - 1, strDate2[0]);
        var dif = date2 - date1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (date1 > date2) {
            $scope.showMessage('I', 'La fecha hasta debe ser mayor a la fecha inicial a consultar.');
            return;
        }

        //Valida que no sean mas de 15 días
        var undia = 24 * 60 * 60 * 1000; // un dia en horas*minutos*segundos*milisegundos
        var diferencia = Math.round(Math.abs(($scope.txtFechaDesde - $scope.txtFechaHasta) / (undia)));
        if (dias > 15) {
            $scope.showMessage('I', 'El rango de fecha no debe superar los 15 días.');
            return;
        }
        //Validar a ingresa ruc  
        if ($scope.rdbProv == "1" && $scope.txtRuc == "") {
            $scope.showMessage('I', 'Ingresar RUC del proveedor.');
            return;
        }
        //Validar a ingresa codProveedor  
        if ($scope.rdbProv == "2" && $scope.txtCodigo == "") {
            $scope.showMessage('I', 'Ingresar código del proveedor.');
            return;
        }
        //Validar a ingresa razon social
        if ($scope.rdbProv == "3" && $scope.txtRazonSocial == "") {
            $scope.showMessage('I', 'Ingresar razón social del proveedor.');
            return;
        }
        //Validar a ingresa pedido  
        if ($scope.rdbPedid == "1" && $scope.txtNumPedido == "") {
            $scope.showMessage('I', 'Ingresar número de pedido.');
            return;
        }
        //var d_desde = "";
        //if ($scope.txtFechaDesde != "")
        //{ d_desde = $filter('date')($scope.txtFechaDesde, 'dd/MM/yyyy'); }
        //var d_hasta = "";
        //if ($scope.txtFechaHasta != "")
        //{ d_hasta = $filter('date')($scope.txtFechaHasta, 'dd/MM/yyyy'); }

        //Validar que seleccione al menos un almacén
        if ($scope.selecAlmacenes.length == 0)
        {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
            return;
        }

        if ($scope.chkEstadosSelModel.length == 0) {
            $scope.showMessage('I', 'Seleccione al menos un estado.');
            return;
        }

        //Enviar string concatenado los datos de almacen y estados
        var listaEnviarAlm = "";
        for (var ix = 0 ; ix < $scope.selecAlmacenes.length; ix++)
        {
            listaEnviarAlm = listaEnviarAlm + "|" + $scope.selecAlmacenes[ix].id;
        }
        var listaEnviarEst = "";
        for (var ix = 0 ; ix < $scope.chkEstadosSelModel.length; ix++) {
            listaEnviarEst = listaEnviarEst + "|" + $scope.chkEstadosSelModel[ix].id;
        }


        var a1 = "";
        var a2 = "";

        if ($scope.txtFechaDesde != "") {
            var parts1 = $scope.txtFechaDesde.split('/');

            a1 = new Date(parts1[2], parts1[1] - 1, parts1[0]);

        }

        if ($scope.txtFechaHasta != "") {
            var parts2 = $scope.txtFechaHasta.split('/');

            a2 = new Date(parts2[2], parts2[1] - 1, parts2[0]);

        }

        var d_desde = $filter('date')(a1, 'dd/MM/yyyy');
        var d_hasta = $filter('date')(a2, 'dd/MM/yyyy');
      
        if ($scope.txtCodigo == undefined) $scope.txtCodigo = "";
        if ($scope.txtNumPedido == undefined) $scope.txtNumPedido = "";

        //Se agrega parametro accion para nuevos filtros - FCASTRO 06-05-2016
        // Valores:   accion = 1 por Ruc ;  accion = 2  por codigo proveedor; accion = 3 por razon social
        if ($scope.rdbProv == "1") { $scope.accion = 1; $scope.valor = $scope.txtRuc; }
        if ($scope.rdbProv == "2") { $scope.accion = 2; $scope.valor = $scope.txtCodigo;}
        if ($scope.rdbProv == "3") { $scope.accion = 3; $scope.valor = $scope.txtRazonSocial; }
        if ($scope.rdbPedid == "0" ) {
            $scope.txtNumPedido = "";
        }
           
        //Armar correo de destinarios
        if (tipo == 2)
        {
           
            if ($scope.resDgUsrsEnviar.length == 0)
            {
                $scope.showMessage('I', 'No existen usuarios para enviar.');
                return;

            }

            var lisValCorreos = $filter('filter')($scope.resDgUsrsEnviar, { isCheck: true }, true);
            if (lisValCorreos.length == 0) {
                $scope.showMessage('I', 'No existen usuarios para enviar.');
                return;
            }
            var correoDestinarios = "";
            for (var ix = 0 ; ix < lisValCorreos.length; ix++)
            {
                correoDestinarios = correoDestinarios + lisValCorreos[ix].correoEnviar + ";";
            }
            if (correoDestinarios != "")
            {
                
                correoDestinarios = correoDestinarios.substring(0,correoDestinarios.length - 1);
                
                
            }
        }
       
        listaEnviarEst = $scope.pOpcGrp2;
        
        $scope.myPromise = ConsEnvPedService.getConsEnvPedidos(tipo, d_desde, d_hasta, $scope.txtRuc, true, true, true, $scope.accion, $scope.valor, $scope.txtNumPedido,
                                                               listaEnviarEst, listaEnviarAlm, correoDestinarios).then(function (results) {
        
            if (results.data.success) {
                //Consulta
                if (tipo === 1) {
                    if (results.data.root[0][0].length == 0) {
                        $scope.showMessage('I', 'No se encontraron registros con los valores especificados.');
                        $scope.grPedido = [];
                        $scope.ContGrPedido = $scope.grPedido.length.toString();
                        $scope.rucEnvia = "";
                        $scope.provEnvia = "";
                        return;
                    } else {
                        $scope.grPedido = results.data.root[0][0];
                        $scope.ContGrPedido = $scope.grPedido.length.toString();
                        $scope.rucEnvia = $scope.grPedido[0].pRuc;
                        $scope.provEnvia = $scope.grPedido[0].pCodProveedor;
                    }
                }
                //Envio
                if (tipo === 2) {
                    $scope.showMessage('S', 'Se han enviado las órdenes.');
                    return;
                }
            }
            else {
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }
            setTimeout(function () { $('#btnConsultau').focus(); }, 100);
            setTimeout(function () { $('#rbtTodos').focus(); }, 150);
        },
         function (error) {
           
             $scope.showMessage('E', "Error en comunicación: ConsEnvPedService.getConsEnvPedidos()" );
         });
    };


    $scope.showMessage = function (tipo, mensaje) {
        //E=Error, I=Informativo, M/S/G=MensajeOK(grabar,procesar,satisfactorio,etc.)
        $scope.MenjError = mensaje;
        if (tipo == 'I') {
            $('#idMensajeInformativo').modal('show');
        }
        else if (tipo == 'E') {
            $('#idMensajeError').modal('show');
        }
        else if (tipo == 'M' || tipo == 'S' || tipo == 'G') {
            $('#idMensajeOk').modal('show');
        }
    }

    $scope.grabar = function () {
        $scope.CargaConsulta(2);
    }
}
]);