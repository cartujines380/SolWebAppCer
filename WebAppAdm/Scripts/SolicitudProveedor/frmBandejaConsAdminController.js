
//'use strict';
app.controller('frmBandejaConsAdminController', ['$scope', 'SeguridadService', 'localStorageService', '$filter', function ($scope, SeguridadService, localStorageService, $filter) {
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.resDgUsrsAdmin = [];
    $scope.resDgUsrsEnviar = [];
    $scope.usrEnviar = {
        nombres: '',
        correoEnviar: '',
        isCheck: true,
    }
   
    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];
    
    $scope.pgcDgUsrsAdmin = [];
    $scope.inicio = false;
    $scope.EsBloqCambioClave = true;
    $scope.rbtOpciones = 'U';
    $scope.chkSinUsuario = true;
    $scope.chkConUsuario = true;
    $scope.txtFiltraRuc = "";
    $scope.txtFiltraNombre = "";
    $scope.txtFiltraCodSap = "";
    $scope.cboFiltraEstado = [];
    $scope.cboFiltraEstadoSelItem = "";
    $scope.usrAdm = {};
    $scope.usrAdm.pRuc = "";
    $scope.usrAdm.pNombre = "";
    $scope.usrAdm.pUsuario = "";
    $scope.usrAdm.pClave = "";
    $scope.usrAdm.pCorreoE = "";
    $scope.usrAdm.pTelefono = "";
    $scope.usrAdm.pCelular = "";
    $scope.usrAdm.pCodSap = "";
    $scope.usrAdm.pIdParticipante = parseInt("-1", 10);
    $scope.usrAdm.pEstado = "";
    $scope.usrAdm.pIdRepresentante = "";
    $scope.usrAdmCtr = {};
    //$scope.usrAdmCtr.ifOpenModal = false;
    $scope.usrAdmCtr.cboEstado = [];
    $scope.usrAdmCtr.cboEstadoSelItem = "";
    $scope.codSAP = "0";
    
    $scope.etiTotRegistros = "";
    $scope.selRowUsrsAdmin = function (registro) {
        localStorageService.set('CodProveedor', registro.codSAP);
        window.location = 'frmConsultaProveedorSol';
        //window.location = '../Proveedor/frmConsultaProveedorSol';
    }
    
    $scope.btnConsultaClick = function () {
       
        var sCodSap = '';
        var sRuc = '';
        var sNombre = '';
        var sConUsuario = '';
        var sEstado = '';
        if ($scope.rbtOpciones == 'U') {
            if ($scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'T';
            else if ($scope.chkSinUsuario && !$scope.chkConUsuario)
                sConUsuario = 'N';
            else if (!$scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'S';
            else {
                $scope.showMessage('I', 'Seleccione una opción válida.');
                return;
            }
        }
        else if ($scope.rbtOpciones == 'R') {
            if ($scope.txtFiltraRuc == '' || $scope.txtFiltraRuc.length < 13) {
                if ($scope.inicio)
                    $scope.showMessage('I', 'Ingrese  los 13 dígitos del RUC.');
                $scope.inicio = true;
                return;
            }
            sRuc = $scope.txtFiltraRuc;
        }
        else if ($scope.rbtOpciones == 'N') {
            if ($scope.txtFiltraNombre == '') {
                $scope.showMessage('I', 'Ingrese la Razón Social.');
                return;
            }
            sNombre = $scope.txtFiltraNombre;
        }
        else if ($scope.rbtOpciones == 'S') {
            if ($scope.txtFiltraCodSap == '') {
                $scope.showMessage('I', 'Ingrese el  Código de Proveedor.');
                return;
            }
            sCodSap = $scope.txtFiltraCodSap;
        }
        else if ($scope.rbtOpciones == 'E') {
            if ($scope.cboFiltraEstadoSelItem == null || $scope.cboFiltraEstadoSelItem == '') {
                $scope.showMessage('I', 'Seleccione el estado.');
                return;
            }
            sEstado = $scope.cboFiltraEstadoSelItem.codigo;
        }
        $scope.etiTotRegistros = "";
        $scope.myPromise = SeguridadService.getConsBandjUsrsAdmin(sCodSap, sRuc, sNombre, "T", sEstado,"").then(function (results) {
            $scope.codSAP = sCodSap;
            if (results.data.success) {
                $scope.resDgUsrsAdmin = results.data.root[0];
                if ($scope.resDgUsrsAdmin.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                }
                else
                {
                    $scope.etiTotRegistros = $scope.resDgUsrsAdmin.length.toString();
                }
             
            }
            else {
                $scope.resDgUsrsAdmin = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }

            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorRuc').focus(); }, 150);


       
        }, function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
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

    setTimeout(function () { $('#btnConsulta').focus(); }, 100);
    setTimeout(function () { angular.element('#btnConsulta').trigger('click'); }, 150);

}
]);


//'use strict';
app.controller('frmConsultaActasAdmController', ['$scope', 'SeguridadService', 'localStorageService', '$filter', function ($scope, SeguridadService, localStorageService, $filter) {
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



    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.accion = 0;
    $scope.resDgUsrsAdmin = [];
    $scope.resDgUsrsEnviar = [];
    $scope.usrEnviar = {
        id: '',
        nombres: '',
        correoEnviar: '',
        isContacto: false,
        isUser: false,
        isCheck: true,
        identificacion: '',
    }
    $scope.lisActas = {
        anio: '',
        mes: '',
        dia : '',
        nombreArchivo: '',
    }

    var _resDgUsrsAdmin = [];
    $scope.pagesUsrsAdmin = [];

    $scope.pgcDgUsrsAdmin = [];
    $scope.inicio = false;
    $scope.EsBloqCambioClave = true;
    $scope.rbtOpciones = 'R';
    $scope.chkSinUsuario = true;
    $scope.chkConUsuario = true;
    $scope.txtFiltraRuc = "";
    $scope.txtFiltraNombre = "";
    $scope.txtFiltraCodSap = "";
    $scope.cboFiltraEstado = [];
    $scope.cboFiltraEstadoSelItem = "";
    $scope.usrAdm = {};
    $scope.usrAdm.pRuc = "";
    $scope.usrAdm.pNombre = "";
    $scope.usrAdm.pUsuario = "";
    $scope.usrAdm.pClave = "";
    $scope.usrAdm.pCorreoE = "";
    $scope.usrAdm.pTelefono = "";
    $scope.usrAdm.pCelular = "";
    $scope.usrAdm.pCodSap = "";
    $scope.usrAdm.pIdParticipante = parseInt("-1", 10);
    $scope.usrAdm.pEstado = "";
    $scope.usrAdm.pIdRepresentante = "";
    $scope.usrAdmCtr = {};
    //$scope.usrAdmCtr.ifOpenModal = false;
    $scope.usrAdmCtr.cboEstado = [];
    $scope.usrAdmCtr.cboEstadoSelItem = "";
    $scope.codSAP = "0";
    $scope.etiTotRegistros = "";
    $scope.rdbProv = "2";
    $scope.selRowUsrsAdmin = function (registro) {
        localStorageService.set('CodProveedor', registro.codSAP);
        window.location = 'frmConsultaProveedor';
    }

    //Variable de Busquedas
    $scope.txtorden = "";
    $scope.txtfactura = "";

    $scope.pOpcGrp1 = "P";
    var dateString1 = new Date();
    var dateString = new Date();

    var d1 = dateString1.format("dd/mm/yyyy");

    dateString.setDate(dateString.getDate() - 15);
    var d2 = dateString.format("dd/mm/yyyy");


    //$scope.pFechaIni = "";
    //$scope.pFechaFin = "";


    $scope.pFechaIni = d2;
    //$scope.pFechaFin = d1;
    $scope.pFechaFin = "";

    $scope.etiTotRegistros = "";
    //Combos
    $scope.EstadoSolicitudGrip = [];
    $scope.selectedItemGrip = "";
    //Fin Combos

    $scope.GridConsolidacion = [];
    var _GridConsolidacion = [];
    $scope.pagesCo = [];
    $scope.pageContentCo = [];

    $scope.resDgConsulta = [];
    $scope.sortTypeCon = 'idconsolidacion';

    $scope.pageContentCon = [];

    $scope.codid = "";

    $scope.txtCodigo = "";
    $scope.pCodSAP = "";
    $scope.txtRuc = "";
    $scope.razonSocial = "";

    $scope.traCho = {};
    $scope.traCho.Tipo = "1";
    $scope.traCho.txtfechasolicitud = "";
    $scope.traCho.horainicial = "";
    $scope.traCho.horafinal = "";
    $scope.txtorden = "" ;
    $scope.txtfactura = "";

    $scope.sortType = 'nombres'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchFish = '';
    $scope.rutaDirectorio = "";

    $scope.cboAlmacenSelItem=[]
    $scope.cboAlmacenList = [];
    //fca
    $scope.cmbestado = [];
    $scope.cmbestadoActas = [];

    $scope.myPromise = SeguridadService.getCatalogo('tbl_estadoActas').then(function (results) {
        $scope.cmbestadoActas = results.data;
        $scope.cmbestadoActas.splice(0, 0, { codigo: "T", detalle: "Todos", descalterno: "" });
        $scope.cmbestado = results.data[0];
    }, function (error) {
    });

    function validate_fechaMayorQue(fechaInicial, fechaFinal) {

        valuesStart = fechaInicial.split("/");

        valuesEnd = fechaFinal.split("/");



        // Verificamos que la fecha no sea posterior a la actual

        var dateStart = new Date(valuesStart[2], (valuesStart[1] - 1), valuesStart[0]);

        var dateEnd = new Date(valuesEnd[2], (valuesEnd[1] - 1), valuesEnd[0]);

        if (dateStart >= dateEnd) {

            return 1;

        }

        return 0;

    }

    $scope.eliminarClickUsuario = function (reg, estado) {
        debugger;
        $scope.myPromise = SeguridadService.getActualizaEstadoActaRecepcion(reg.iD_Secuencial, estado).then(function (results) {
           
            if (results.data.success) {
                if (estado == 'A') {
                    $scope.showMessage('G', 'Acta activada correctamente.');
                    reg.estado = "A";
                    reg.desEstado = "Activa";
                }
                if (estado == 'X') {
                    $scope.showMessage('G', 'Acta anulada correctamente.');
                    reg.estado = "X";
                    reg.desEstado = "Anulada";
                }
                $scope.accion = 1;
               
                //    $scope.Consultar();

            }
            else {
                
                $scope.showMessage('E', 'Error al actualizar estado');
            }

            



        }, function (error) {

            $scope.showMessage('E', "Error en comunicación: SeguridadService.getConsBandjUsrsAdmin()");
        });
    }
    

    //Se borra despues
    $scope.btnConsultaClick = function () {

        var sCodSap = '';
        var sRuc = '';
        var sNombre = '';
        var sConUsuario = '';
        var sEstado = '';

       

        if ($scope.rbtOpciones == 'U') {
            if ($scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'T';
            else if ($scope.chkSinUsuario && !$scope.chkConUsuario)
                sConUsuario = 'N';
            else if (!$scope.chkSinUsuario && $scope.chkConUsuario)
                sConUsuario = 'S';
            else {
                $scope.showMessage('I', 'Seleccione una opción válida.');
                return;
            }
        }
        else if ($scope.rbtOpciones == 'R') {
            if ($scope.txtFiltraRuc == '' || $scope.txtFiltraRuc.length < 13) {
                if ($scope.inicio)
                    $scope.showMessage('I', 'Ingrese  los 13 dígitos del RUC.');
                $scope.inicio = true;
                return;
            }
            sRuc = $scope.txtFiltraRuc;
        }
        else if ($scope.rbtOpciones == 'N') {
            if ($scope.txtFiltraNombre == '') {
                $scope.showMessage('I', 'Ingrese la Razón Social.');
                return;
            }
            sNombre = $scope.txtFiltraNombre;
        }
        else if ($scope.rbtOpciones == 'S') {
            if ($scope.txtFiltraCodSap == '') {
                $scope.showMessage('I', 'Ingrese el  Código de Proveedor.');
                return;
            }
            sCodSap = $scope.txtFiltraCodSap;
        }
        else if ($scope.rbtOpciones == 'E') {
            if ($scope.cboFiltraEstadoSelItem == null || $scope.cboFiltraEstadoSelItem == '') {
                $scope.showMessage('I', 'Seleccione el estado.');
                return;
            }
            sEstado = $scope.cboFiltraEstadoSelItem.codigo;
        }
        $scope.etiTotRegistros = "";
        $scope.myPromise = SeguridadService.getConsBandjUsrsAdmin(sCodSap, sRuc, sNombre, "T", sEstado, "").then(function (results) {
            $scope.codSAP = sCodSap;
            if (results.data.success) {
                $scope.resDgUsrsAdmin = results.data.root[0];
                if ($scope.resDgUsrsAdmin.length < 1) {
                    $scope.showMessage('I', 'No existen datos para mostrar.');
                }
                else {
                    $scope.etiTotRegistros = $scope.resDgUsrsAdmin.length.toString();
                }

            }
            else {
                $scope.resDgUsrsAdmin = [];
                $scope.showMessage('E', 'Error al consultar: ' + results.data.msgError);
            }

            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorRuc').focus(); }, 150);



        }, function (error) {
            
            $scope.showMessage('E', "Error en comunicación: SeguridadService.getConsBandjUsrsAdmin()" );
        });
    };

    $scope.myPromise = SeguridadService.getConsAlmacenes("4").then(function (results) {
        if (results.data.success) {
            var listAlmacen = results.data.root[0];
            listAlmacen.splice(0, 0, { pCodAlmacen: "-999", pNomAlmacen: "Todos los Almacenes" });
            $scope.cboAlmacenList = listAlmacen;
            $scope.cboAlmacenSelItem = listAlmacen[0];

        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });

    $scope.Consultar = function () {
       
        var Fecha1 = ""; var Fecha2 = "";
        //Valida Rango de fecha
        if ($scope.pFechaIni == null || $scope.pFechaIni == "") {
            $scope.showMessage('I', 'Seleccione la fecha desde del rango a consultar.');
            return;
        }
        //if ($scope.pFechaFin == null || $scope.pFechaFin == "") {
        //    $scope.showMessage('I', 'Seleccione la fecha hasta del rango a consultar.');
        //    return;
        //}
        var strDate1 = $scope.pFechaIni.split("/");
        var strDate2 = $scope.pFechaFin.split("/");
        var date1 = new Date(strDate1[2], strDate1[1] - 1, strDate1[0]);
        var date2 = new Date(strDate2[2], strDate2[1] - 1, strDate2[0]);
        var dif = date2 - date1;
        var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
        if (date1 > date2) {
            $scope.showMessage('I', 'La fecha hasta debe ser mayor a la fecha desde a consultar.');
            return;
        }

        //Valida que no sean mas de 15 días
        var undia = 24 * 60 * 60 * 1000; // un dia en horas*minutos*segundos*milisegundos
        var diferencia = Math.round(Math.abs(($scope.pFechaIni - $scope.pFechaFin) / (undia)));
        if (dias > 15) {
            $scope.showMessage('I', 'El rango de fecha no debe superar los 15 días.');
            return;
        }

        Fecha1 = $filter('date')($scope.pFechaIni, 'dd-MM-yyyy');
        Fecha2 = $filter('date')($scope.pFechaFin, 'dd-MM-yyyy');

       
        //if ($scope.txtCodigo == "") {
        //    $scope.showMessage('I', 'Ingrese código de proveedor.');
        //    return;
        //}

        $scope.txtRuc = "";
        $scope.pCodSAP = "";
        $scope.razonSocial = "";
        $scope.myPromise = null;
        $scope.etiTotRegistros = "";
        if ($scope.txtorden != "" || $scope.txtfactura != "") { Fecha1 = ""; Fecha2 = ""; }
        $scope.myPromise = SeguridadService.getConsulaGridActaRecepcion("2", $scope.txtorden, $scope.txtfactura, Fecha1, Fecha2, $scope.txtCodigo, $scope.cboAlmacenSelItem.pCodAlmacen).then(function (results) {
            if (results.data.success) {
                $scope.pCodSAP = $scope.txtCodigo;
                
                $scope.resDgConsulta = results.data.root[0];
                //Filtro de estado fca
                debugger;
                if ($scope.cmbestado.codigo == 'A')
                    $scope.resDgConsulta = $filter('filter')($scope.resDgConsulta, { estado: "A" }, true);
                if ($scope.cmbestado.codigo == 'X')
                    $scope.resDgConsulta = $filter('filter')($scope.resDgConsulta, { estado: "X" }, true);
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();


                if ($scope.etiTotRegistros == 0) {
                    $scope.resDgConsulta = [];
                    $scope.showMessage('I', 'No exiten resultado para su consulta.');
                    return;
                }



                $scope.txtRuc = $scope.resDgConsulta[0].ruc;
                $scope.razonSocial = $scope.resDgConsulta[0].nomComercial;
                $scope.etiTotRegistros = $scope.resDgConsulta.length.toString();
            }
            else {
                $scope.resDgConsulta = [];
                $scope.etiTotRegistros = "0";
                $scope.showMessage('I', 'No exiten resultado para su consulta.');
                return;
            }
            setTimeout(function () { $('#btnConsulta1').focus(); }, 100);
            setTimeout(function () { $('#txtsap').focus(); }, 150);

        }, function (error) {
        });
    }


    $scope.descargaArchivo = function (content) {
        $scope.myPromise = null;
        debugger;
        $scope.myPromise = SeguridadService.getArchivoActaRecepcion(content.archivo, content.anio, content.mes, content.dia).then(function (results) {
            if (results.data != "") {
                var file = new Blob([results.data], { type: 'application/pdf' });
                saveAs(file, content.archivo);
            }
        }, function (error) {
        });
        setTimeout(function () { $('#consultagrid').focus(); }, 10);
    }
    //Consulta de contactos y usuarios para envio de actas entrega-recepcion
    $scope.ConfirmarEnviar = function () {

        $scope.allChecks = false;
        var sRuc = $scope.txtRuc;
        var sNombre = '';
        var listaUsuarios = [];
        var listaConstactos = [];
        $scope.resDgUsrsEnviar = [];
        
        var exisAct = $filter('filter')($scope.resDgConsulta,
                                             { estado: "A" }, true);

        if (exisAct < 1)
        {
            $scope.showMessage('I', "No se puede enviar actas en estado Anulada.");
            return;
        }


        //Consultar Usuarios 
        $scope.myPromise = SeguridadService.getConsDatosLegAsociados(sRuc, sNombre).then(function (results) {
            if (results.data.success) {

                debugger;
                listaUsuarios = $filter('filter')(results.data.root[0],
                                             { prolLogistico: true }, true);

            }
            //Consulta contactos
            $scope.myPromise = SeguridadService.getContactoList($scope.pCodSAP).then(function (response) {

                if (response.data.success) {
                    debugger;

                    listaConstactos = $filter('filter')(response.data.root[0],
                                                { departamento: "0006", funcion: "06", pRecActas : true }, true);

                    

                    for (var id = 0; id < listaUsuarios.length; id++) {
                        $scope.usrEnviar = {}
                        $scope.usrEnviar.id = $scope.resDgUsrsEnviar.length + 1;
                        debugger;
                        $scope.usrEnviar.identificacion = listaUsuarios[id].pCedula;
                        $scope.usrEnviar.nombres = listaUsuarios[id].pApellidos + ' ' + listaUsuarios[id].pNombres;
                        $scope.usrEnviar.correoEnviar = listaUsuarios[id].pCorreo;
                        $scope.usrEnviar.isContacto= false;
                        $scope.usrEnviar.isUser = true;
                        $scope.usrEnviar.isCheck = false;
                        $scope.resDgUsrsEnviar.push($scope.usrEnviar);
                    }

                    for (var id = 0; id < listaConstactos.length; id++) {

                        var existeUsr = $filter('filter')(listaUsuarios, { pCedula: listaConstactos[id].identificacion }, true);
                        if (existeUsr.length < 1) {
                            $scope.usrEnviar = {}
                            $scope.usrEnviar.id = $scope.resDgUsrsEnviar.length + 1;
                            $scope.usrEnviar.identificacion = listaConstactos[id].identificacion;
                            $scope.usrEnviar.nombres = listaConstactos[id].apellido1 + ' ' + listaConstactos[id].apellido2 +
                                                       listaConstactos[id].nombre1 + ' ' + listaConstactos[id].nombre2;
                            $scope.usrEnviar.correoEnviar = listaConstactos[id].email;
                            $scope.usrEnviar.isContacto = true;
                            $scope.usrEnviar.isUser = false;
                            $scope.usrEnviar.isCheck = false;
                            $scope.resDgUsrsEnviar.push($scope.usrEnviar);
                        }
                        else {
                            //Actualizar como contacto
                            var update = $filter('filter')($scope.resDgUsrsEnviar, { identificacion: listaConstactos[id].identificacion }, true);
                            update[0].isContacto = true;
                        }
                    }
                    $("#selUsr").prop("checked", "");
                    $('#usuariosEnvia').modal('show');


                }

            },
            function (err) {
                $scope.MenjError = "Error en comunicación: ModificacionProveedor.getContactoList()";
                $('#idMensajeError').modal('show');
            });


        }, function (error) {
            $scope.showMessage('E', "Error en comunicación: SeguridadService.getConsDatosLegAsociados()");
        });







    }

    $scope.marcaChecks = function (valor) {

       
        for (var idx = 0 ; idx < $scope.resDgUsrsEnviar.length;idx++)
        {
            var update = $scope.resDgUsrsEnviar[idx];
            update.isCheck = valor;
        }
    }

    $scope.enviarActas = function () {

       

        var listaActas = [];
        //Llenar actas
        for (var idx = 0 ; idx < $scope.resDgConsulta.length; idx++)
        {
            $scope.lisActas = {};
            $scope.lisActas.anio = $scope.resDgConsulta[idx].anio;
            $scope.lisActas.mes = $scope.resDgConsulta[idx].mes;
            $scope.lisActas.dia = $scope.resDgConsulta[idx].dia;
            $scope.lisActas.nombreArchivo = $scope.resDgConsulta[idx].archivo;
            if ($scope.resDgConsulta[idx].estado == 'A')
                listaActas.push($scope.lisActas);
        }


        ////Simular llenado de actas
        //$scope.lisActas = {};
        //$scope.lisActas.anio = "2016"
        //$scope.lisActas.mes = "05"
        //$scope.lisActas.dia = "10"
        //$scope.lisActas.nombreArchivo = '0000101156_0100_4600235654_001001-000034534_20160423_052045.PDF'
        //listaActas.push($scope.lisActas);
        //$scope.lisActas = {};
        //$scope.lisActas.anio = "2016"
        //$scope.lisActas.mes = "05"
        //$scope.lisActas.dia = "11"
        //$scope.lisActas.nombreArchivo = '0000101156_0100_4600235654_001001-000034534_20160523_052045.PDF'
        //listaActas.push($scope.lisActas);
        //Validar que exista registros a enviar
        if (listaActas.length == 0)
        {
            $scope.showMessage('I', 'No existen actas para enviar.');
            return;
        }
  
        var lisValCorreos = $filter('filter')($scope.resDgUsrsEnviar, { isCheck: true }, true);
        if (lisValCorreos.length == 0) {
            $scope.showMessage('I', 'No existen usuarios para enviar.');
            return;
        }

        //Enviar
        $scope.myPromise = SeguridadService.getListUsrActas($scope.resDgUsrsEnviar, listaActas, $scope.razonSocial, $scope.txtRuc, $scope.pFechaIni, $scope.pFechaFin).then(function (results) {
            debugger;
            if (results.data.success) {
                $scope.showMessage('M', 'Actas enviadas correctamente.');
                
            }
            else {
              
                $scope.showMessage('E', 'Error al enviar actas: ' + results.data.mensaje);
            }

            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtPorRuc').focus(); }, 150);



        }, function (error) {

            $scope.showMessage('E', "Error en comunicación: SeguridadService.getListUsrActas()");
        });

    }

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


    $("#btnMensajeOK").click(function () {
        if ($scope.accion == 0)
            window.location = 'frmConsActasAdmin';
    });    
}
]);
