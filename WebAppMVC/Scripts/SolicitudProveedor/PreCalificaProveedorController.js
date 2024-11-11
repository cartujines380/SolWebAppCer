'use strict';

app.controller('PreCalificaSolicitudProveedorController', ['$scope', '$location', '$http', 'SolicitudProveedor', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'localStorageService', 'authService', '$timeout', '$window', function ($scope, $location, $http, SolicitudProveedor, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, localStorageService, authService, $timeout, $window) {

    $scope.OrigenActualizacion = 0;//Variable para sensar carga inicial Value=0, definición.

    $scope.tipoidentificacionList = [];
    $scope.ListTipoIdentificacion = [];
    $scope.ListSectorComercial = [];
    $scope.ListTipoProveedor = [];
    $scope.ListMotivoRechazoProveedor = [];
    $scope.ListSociedad = [];
    $scope.Sociedad = [];
    $scope.ListLinea = [];
    $scope.Linea = [];
    $scope.ListGrupoTesoreria = [];
    $scope.ListCuentaAsociada = [];
    $scope.ListFuncionContactoT = [];
    $scope.GrupoCompraFilt = [];
    $scope.GrupoCompra = [];
    $scope.ListRegion = [];
    $scope.ListRegionTemp = [];
    $scope.ListCiudad = [];
    $scope.ListCiudadTemp = [];
    $scope.ListIdioma = [];
    $scope.GrupoArticuloDS = [];
    $scope.ListClaseImpuesto = [];
    $scope.ListTratamiento = [];
    $scope.ListMaxCantAdjProveedor = [];
    $scope.ListMaxMegaProveedor = [];
    $scope.SolDocAdjunto = [];
    $scope.ListDocumentoAdjunto = [];
    $scope.ListDocumentoAdjunto.codigo = "";
    $scope.ListDocumentoAdjunto.descAlterno = "";
    $scope.ListDocumentoAdjunto.detalle = "";
    $scope.ListDocumentoAdjunto.error = "";
    $scope.ListDocumentoAdjunto.estado = "";
    $scope.ListDocumentoAdjunto.tabla = "";
    $scope.ListDocumentoAdjunto.ver = true;
    $scope.bloqueCodPostal = false;
    $scope.SolProveedor = [];
    $scope.SolProvDireccion = [];
    $scope.SolProvContacto = [];
    $scope.SolProvBanco = [];
    $scope.SolProvHistEstado = [];
    $scope.ListFuncionContacto = [];
    $scope.ListDepartaContacto = [];
    $scope.MinCantContactoProve = 0
    $scope.MaxCantCBanExtrPrv = 0;
    $scope.salir = 0;
    $scope.bandera = "0";
    $scope.codigo = "";
    $scope.detalle = "";
    $scope.index = 0;
    $scope.pasoValidacion = 0;
    $scope.pestanaActual = "D";
    var target = "";

    var re = /[+]/g;
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        target = $(e.target).attr("href") // activated tab
    });

    //integracion
    $scope.ListTipoActividad = [];
    $scope.ListTipoServicio = [];
    $scope.ListContacEstadoCivil = [];
    $scope.datosConyuge = false;
    $scope.ListNacionalidad = [];
    $scope.ListPaisResidencia = [];
    $scope.datosRegimenMatrimonial = false;
    $scope.ListContacRegimenMatrimonial = [];
    $scope.ListRelacionLaboral = [];
    $scope.ListTipoIngreso = [];
    $scope.ListContacTipoParticipante = [];

    $scope.Ingreso = {
        tipoidentificacion: "",
        SectorComercial: "",
        TipoProveedor: "",
        MotivoRechazoProveedor: "",
        Sociedad: "",
        Linea: "",
        GrupoTesoreria: "",
        CuentaAsociada: "",
        GrupoCompra: "",
        Pais: "",
        Region: "",
        Ciudad: "",
        Idioma: "",
        ClaseImpuesto: "",
        Contactipoidentificacion: "",
        Tratamiento: "",
        DocumentoAdjunto: "",
        MaxCantAdjProveedor: "",
        MaxMegaProveedor: "",
        LineaNegocio: "",
        FuncionContacto: "",
        DepartaContacto: "",
        tipoActividad: "",
        tipoServicio: '',
        ContacEstadocivil: '',
        Conyugetipoidentificacion: '',
        ContacNacionalidad: '',
        ContacResidencia: '',
        ContacRegimenMatrimonial: '',
        ContacRelacionLaboral: '',
        ContacTipoIngreso: '',
        ConyugeNacionalidad: '',
        ContacTipoParticipante: '',
    };

    $scope.ListaLineaNegocio = [];
    $scope.LineasNegocios = {
        codigo: "",
        detalle: "",
        chekeado: "",
        principal: ""
    };

    $scope.rutaDirectorio;
    $scope.codigoPre = 0;

    $scope.identificacion = '';
    $scope.tipoidentificacion = '';
    $scope.IdSolicitud = '';
    $scope.Estado = '';
    $scope.tiposolicitud = '';
    $scope.indpage = '';
    $scope.maxidentificacion = 0;
    $scope.maxcontacto = 0;
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    $scope.isDisabledApr = false;
    $scope.mensajelogin = '';

    $scope.bloquearSegundo = false;


    if (localStorageService.get('SolPreca')) {
        $scope.identificacion = localStorageService.get('SolPreca').identificacion;
        $scope.tipoidentificacion = localStorageService.get('SolPreca').tipoidentificacion;
        $scope.IdSolicitud = localStorageService.get('SolPreca').IdSolicitud;
        $scope.Estado = localStorageService.get('SolPreca').Estado;
        $scope.tiposolicitud = localStorageService.get('SolPreca').tiposolicitud;
        $scope.indpage = localStorageService.get('SolPreca').indpage;
        $scope.mensajelogin = localStorageService.get('SolPreca').MenjError;
    }

    if ($scope.mensajelogin != '' && $scope.indpage != 'APR') {

        $scope.MenjError = $scope.mensajelogin;
        $scope.salir = 2;

        if ($scope.indpage == 'ING')
            $scope.salir = 0;
    }

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=prueba';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    $scope.cambiarxTipoId = function () {
        $scope.bloquearSegundo = false;
        if ($scope.Ingreso.Contactipoidentificacion != undefined) {
            if ($scope.Ingreso.Contactipoidentificacion.codigo == 'RC' || $scope.Ingreso.Contactipoidentificacion.codigo == 'RE') {
                $scope.bloquearSegundo = true;
            }
        }

    }


    

    $scope.cambiarTipoParticipante = function () {

        if ($scope.Ingreso.ContacTipoParticipante != undefined) {
            if ($scope.p_SolProvContacto.RepLegal && $scope.Ingreso.ContacTipoParticipante.codigo != '357') {
                $scope.Ingreso.ContacTipoParticipante = '';
                $scope.MenjError = 'Tipo participante no permitido';
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if ($scope.p_SolProvContacto.RepLegal == false && $scope.Ingreso.ContacTipoParticipante.codigo == '357') {
                $scope.Ingreso.ContacTipoParticipante = '';
                $scope.MenjError = 'Tipo participante no permitido, no es representante legal';
                $('#idMensajeInformativo').modal('show');

            }
        }

        return;
    }

    $scope.cambiarEstadoCivil = function () {
        $scope.datosConyuge = false;
        $scope.datosRegimenMatrimonial = false;
        $scope.Ingreso.Conyugetipoidentificacion = '';
        $scope.Ingreso.ConyugeNacionalidad = '';
        $scope.p_SolProvContacto.Conyugetipoidentificacion = '';
        $scope.p_SolProvContacto.DesConyugetipoidentificacion = '';
        $scope.p_SolProvContacto.ConyugeIdentificacion = '';
        $scope.p_SolProvContacto.ConyugeNombres = '';
        $scope.p_SolProvContacto.ConyugeApellidos = '';
        $scope.p_SolProvContacto.ConyugeFechaNac = '';
        $scope.p_SolProvContacto.ConyugeNacionalidad = '';
        $scope.p_SolProvContacto.ContacRegimenMatrimonial = '';
        
        if ($scope.Ingreso.ContacEstadoCivil != undefined) {
            if ($scope.Ingreso.ContacEstadoCivil.codigo == '2452' || $scope.Ingreso.ContacEstadoCivil.codigo == '2456') {
                $scope.datosConyuge = true;
            }
        }

        if ($scope.Ingreso.ContacEstadoCivil != undefined) {
            if ($scope.Ingreso.ContacEstadoCivil.codigo == '2452') {
                $scope.datosRegimenMatrimonial = true;
            }
        }
        return;
    }

    $scope.eliminacontacto = function (registro) {
        $scope.accion = 998;
        $scope.registro = registro;
        $scope.MenjConfirmacion = '¿Está seguro de eliminar contacto?';
        $('#idMensajeConfirmacionEliminar').modal('show');
    }

    $scope.Eliminar = function () {

        //Eliminar contacto
        if ($scope.accion == 998) {
            for (var i = 0, len = $scope.SolProvContacto.length; i < len; i++) {
                if ($scope.SolProvContacto[i].Identificacion === $scope.registro.Identificacion
                ) {
                    break;

                }
            }

            $scope.SolProvContacto.splice(i, 1);
        }
    }

    $scope.confirmaenviar = function (val, formulario) {

        //Confirmar todos los datos a enviar


        if (formulario.seletipoidentificacionProveedor.$invalid) {
            $scope.MenjError = 'Tipo de Identificacion de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.txtIdentificacion.$invalid) {
            $scope.MenjError = 'La identificacion de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtNombreSri.$invalid) {
            $scope.MenjError = 'Nombre SRI de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtNombreSri.$invalid) {
            $scope.MenjError = 'Nombre SRI de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtNombreComercial.$invalid) {
            $scope.MenjError = 'Nombre Comercial de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.seletipoactividadProveedor.$invalid) {
            $scope.MenjError = 'Tipo de Actividad de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.selectpaisprov.$invalid) {
            $scope.MenjError = 'Seleccione un País de la pestaña Datos del proveedor...';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.selectprovinprov.$invalid) {
            $scope.MenjError = 'Seleccione una Provincia de la pestaña Datos del proveedor...';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtCallePrincipalNumero.$invalid) {
            $scope.MenjError = 'La Calle Principal de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.TxtCelular.$invalid) {
            $scope.MenjError = 'El Teléfono celular de la pestaña Datos del proveedor  es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtMailCorpo.$invalid) {
            $scope.MenjError = 'El  Correo Corporativo de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.selectclaseproveedor.$invalid) {
            $scope.MenjError = 'Seleccione una Clase de contribuyente de la pestaña Datos del proveedor.';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.selectgrupoproveedor.$invalid) {
            $scope.MenjError = 'Seleccione Grupo de Compras de la pestaña Datos del proveedor';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.selectSectorComercial.$invalid) {
            $scope.MenjError = 'Seleccione un  Sector de la pestaña Datos del proveedor.';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.selectIdioma.$invalid) {
            $scope.MenjError = 'Seleccione un Idioma de la pestaña Datos del proveedor.';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.seletipoactividadProveedor.$invalid) {
            $scope.MenjError = 'Seleccione un Tipo de Servicio de la pestaña Datos del proveedor.';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.fechaCreacion.$invalid) {
            $scope.MenjError = 'Fecha de constitución de la empresa de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }
        if (formulario.txtIdentificacionR.$invalid) {
            $scope.MenjError = 'La identificacion de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtNombresCompletos.$invalid) {
            $scope.MenjError = 'Nombres Completos de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }

        if (formulario.txtAreaR.$invalid) {
            $scope.MenjError = 'Área que labora de la pestaña Datos del proveedor es requerido';
            $('#idMensajeError').modal('show');
            return;
        }
        if (!val) {
            $scope.MenjError = 'Complete todos los datos obligatorios';
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.indpage == 'ING') {
            $scope.MenjConfirmacion = '¿Está seguro de enviar la información?';
        }

        if ($scope.indpage == 'APR') {
            $scope.MenjConfirmacion = '¿Está seguro de rechazar la información?';
        }
        $('#idMensajeConfirmacion').modal('show');
    }

    GeneralService.getCatalogo('tbl_FuncionContacto').then(function (results) {
        $scope.ListFuncionContactoT = results.data;
    }, function (error) {
    });

    $scope.$watch('Ingreso.DepartaContacto', function () {
        if ($scope.Ingreso.DepartaContacto == null) { $scope.ListFuncionContacto = []; return; };
        if ($scope.Ingreso.DepartaContacto.length == 0) { $scope.ListFuncionContacto = []; return; };
        $scope.ListFuncionContacto = [];
        $scope.ListFuncionContacto = $filter('filter')($scope.ListFuncionContactoT, { descAlterno: $scope.Ingreso.DepartaContacto.codigo }, true);

    });

    GeneralService.getCatalogo('tbl_DepartaContacto').then(function (results) {
        $scope.ListDepartaContacto = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_ClaseImpuesto').then(function (results) {
        $scope.ListClaseImpuesto = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_Tratamiento').then(function (results) {
        $scope.ListTratamiento = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoIdentificacion').then(function (results) {
        $scope.ListTipoIdentificacion = results.data;

        var index = 0;
        var _tipoide_codigo = 'RC';

        for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
            if ($scope.ListTipoIdentificacion[index].codigo === _tipoide_codigo) {
                $scope.Ingreso.tipoidentificacion = $scope.ListTipoIdentificacion[index];

                break;
            }
        }
    }, function (error) {
    });

    //Inicio Configuracion para archivos
    GeneralService.getCatalogo('tbl_MaxCantAdjProveedor').then(function (results) {
        $scope.ListMaxCantAdjProveedor = results.data;
        if ($scope.ListMaxCantAdjProveedor != "") {
            $scope.Ingreso.MaxCantAdjProveedor = $scope.ListMaxCantAdjProveedor[0].codigo;
        }
    }, function (error) {
        $scope.Ingreso.MaxCantAdjProveedor = 5;
    });

    GeneralService.getCatalogo('tbl_MaxMegaProveedor').then(function (results) {
        $scope.ListMaxMegaProveedor = results.data;
        if ($scope.ListMaxMegaProveedor != "") {
            $scope.Ingreso.MaxMegaProveedor = $scope.ListMaxMegaProveedor[0].codigo;
        }

    }, function (error) {
        $scope.Ingreso.MaxMegaProveedor = 2;
    });

    GeneralService.getCatalogo('tbl_ProvGrupoCompras').then(function (results) {
        $scope.GrupoCompra = results.data;

    }, function (error) {
        $scope.Ingreso.MaxCantAdjProveedor = 5;
    });

    GeneralService.getCatalogo('tbl_DocumentoAdjunto').then(function (results) {
        for (var i = 0; i < results.data.length; i++) {
            var grid = {};
            grid.codigo = results.data[i].codigo;
            grid.descAlterno = results.data[i].descAlterno;
            grid.detalle = results.data[i].detalle;
            grid.error = results.data[i].error;
            grid.estado = results.data[i].estado;
            grid.tabla = results.data[i].tabla;
            grid.ver = true;
            $scope.ListDocumentoAdjunto.push(grid);
        }

    }, function (error) {
    });

    $scope.$watch('Ingreso.Pais', function () {

        $scope.ListRegionTemp = [];
        if ($scope.Ingreso.Pais != '' && angular.isUndefined($scope.Ingreso.Pais) != true) {
            $scope.ListRegionTemp = $filter('filter')($scope.ListRegion,
                { descAlterno: $scope.Ingreso.Pais.codigo });

            if ($scope.Ingreso.Pais.codigo == "EC") {
                $scope.bloqueCodPostal = true;
            }
            else { $scope.bloqueCodPostal = false; }
        }
    });

    $scope.$watch('Ingreso.Region', function () {
        $scope.ListCiudadTemp = [];
        if ($scope.Ingreso.Region != null) {
            if ($scope.Ingreso.Region != '' && angular.isUndefined($scope.Ingreso.Region) != true) {
                $scope.ListCiudadTemp = $filter('filter')($scope.ListCiudad,
                    { descAlterno: $scope.Ingreso.Region.codigo });
            }
        }

    });

    GeneralService.getCatalogo('tbl_Idioma').then(function (results) {
        $scope.ListIdioma = results.data;
        var a = $scope.ListIdioma;
        for (var index = 0; index < a.length; ++index) {
            if (a[index].codigo === "ES")
                $scope.Ingreso.Idioma = a[index];
        }
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_MinCantContactoProve').then(function (results) {

        if (results.data != "") {
            $scope.MinCantContactoProve = results.data[0].codigo;
        }
    }, function (error) {
        $scope.MinCantContactoProve = 2;
    });

    GeneralService.getCatalogo('tbl_MaxCantCBanExtrPrv').then(function (results) {

        if (results.data != "") {
            $scope.MaxCantCBanExtrPrv = results.data[0].codigo;
        }
    }, function (error) {
        $scope.MaxCantCBanExtrPrv = 2;
    });


    GeneralService.getCatalogo('tbl_SectorComercial').then(function (results) {
        $scope.ListSectorComercial = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_TipoProveedor').then(function (results) {
        $scope.ListTipoProveedor = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_MotivoRechazoProveedor').then(function (results) {
        $scope.ListMotivoRechazoProveedor = results.data;
    }, function (error) {
    });
    GeneralService.getCatalogo('tbl_Sociedad').then(function (results) {
        $scope.ListSociedad = results.data;
    }, function (error) {

    });
    GeneralService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.ListLinea = results.data;
        $scope.ListaLineaNegocio = [];

        for (var i = 0; i < $scope.ListLinea.length; i++) {
            $scope.LineasNegocios = {};
            $scope.LineasNegocios.codigo = $scope.ListLinea[i].codigo;
            $scope.LineasNegocios.detalle = $scope.ListLinea[i].detalle;
            $scope.LineasNegocios.chekeado = false;
            $scope.LineasNegocios.principal = false;
            $scope.ListaLineaNegocio.push($scope.LineasNegocios);
        }
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_GrupoTesoreria').then(function (results) {
        $scope.ListGrupoTesoreria = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_CuentaAsociada').then(function (results) {
        $scope.ListCuentaAsociada = results.data;
    }, function (error) {
    });

    //fin Configuracion para archivos
    //integracion
    GeneralService.getCatalogo('tbl_tipoActividad').then(function (results) {
        $scope.ListTipoActividad = results.data

    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoServicio_neo').then(function (results) {
        $scope.ListTipoServicio = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_estadoCivil_neo').then(function (results) {
        $scope.ListContacEstadoCivil = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_nacionalidad_neo').then(function (results) {
        $scope.ListNacionalidad = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_nacionalidad_neo').then(function (results) {
        $scope.ListPaisResidencia = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_regimenMatrimonial_neo').then(function (results) {
        $scope.ListContacRegimenMatrimonial = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_relacionDepenLaboral_neo').then(function (results) {
        $scope.ListRelacionLaboral = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoIngreso_neo').then(function (results) {
        $scope.ListTipoIngreso = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoParticipante_neo').then(function (results) {
        $scope.ListContacTipoParticipante = results.data;
    }, function (error) {
    });

    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });


    //Proceso de carga archivos
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

    $scope.seleccionarLinea = function (registro) {

        if (!registro.chekeado) { registro.principal = false; $scope.GrupoCompraFilt = []; }
    }

    $scope.validarLineaPrincipal = function (registro) {

        var LineaPrincipales = $filter('filter')($scope.ListaLineaNegocio,
            { principal: true });

        if (LineaPrincipales.length > 1) {
            $scope.MenjError = 'Ya selecciono una línea de negocio como principal.';
            $('#idMensajeInformativo').modal('show');
            registro.principal = false;
            return;
        }
        $scope.GrupoCompraFilt = [];
        if (registro.principal) {
            $scope.GrupoCompraFilt = $filter('filter')($scope.GrupoCompra,
                { descAlterno: registro.codigo }, true);
        }

        $scope.Ingreso.LineaNegocio = {};
        $scope.Ingreso.LineaNegocio.codigo = registro.codigo;
    }

    $scope.Limpiasecuenciaarchivo = function () {

        $scope.codigoPre = 0;
        var superior = 100000;
        var inferior = 1;
        var resAleatorio = Math.floor((Math.random() * (superior - inferior + 1)) + inferior);
        var today = new Date();
        var dateString = today.format("ddmmyyyyHHMMss");

        $scope.rutaDir = serviceBase + 'api/Upload/D/?path=';

        var direc = "SolPreCa_" + dateString + resAleatorio;
        $scope.rutaDirectorio = direc;
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
        $scope.uploader2.url = Ruta;

    }, function (error) {
    };

    $scope.Limpiasecuenciaarchivo();

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {

            var filename = item.name.replace(re, '_');
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "rtf" || extension == "txt") {
                item.name = filename;
                return true;
            }
            else {
                $scope.MenjError = "Debe seleccionar archivos con formato pdf/doc/docs or rtf.";
                $scope.p_SolDocAdjunto.Archivo = "";
                $('#idMensajeError').modal('show');
                $('#adjuntoarchivo').val('').clone(true);
                return false;
            }
        }
    });

    uploader2.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {

            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= $scope.Ingreso.MaxMegaProveedor) {
                item.name = item.name.replace(re, '_');
                $("#" + $scope.index.toString()).val(item.name);
                return true;
            }
            else {
                $scope.Ingreso.MaxCantAdjProveedor

                $scope.MenjError = "El  archivo es inválido, execede del limite de " + $scope.Ingreso.MaxMegaProveedor + ' MB.';
                $scope.p_SolDocAdjunto.Archivo = "";
                $('#idMensajeError').modal('show');
                $('#adjuntoarchivo').val('').clone(true);
                return false;
            }
        }
    });

    uploader2.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {

            if (this.queue.length < $scope.Ingreso.MaxCantAdjProveedor) {
                item.name = item.name.replace(re, '_');
                return true;
            }
            else {
                $scope.MenjError = "Has sobrepasado la cantidad de archivos permitidos... ";
                $('#idMensajeError').modal('show');

                $('#adjuntoarchivo').val('');
                return false;
            }
        }
    });

    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        item.name = item.name.replace(re, '_');
        console.info('onWhenAddingFileFailed', item, filter, options);
    };

    uploader2.onAfterAddingFile = function (fileItem) {
        fileItem.file.name = fileItem.file.name.replace(re, '_');
        $scope.p_SolDocAdjunto.NomArchivo = fileItem.file.name;
        $scope.p_SolDocAdjunto.Archivo = $scope.rutaDirectorio;
        $scope.p_SolDocAdjunto.FechaCarga = new Date();
    };

    uploader2.onSuccessItem = function (fileItem, response, status, headers) {
        if ($scope.uploader2.progress == 100) {
            $('#adjuntoarchivo').val('');
            $scope.uploader2.progress = 0;
            fileItem.file.name = fileItem.file.name.replace(re, '_');
            $scope.uploader2.queue = [];
        }
    };

    uploader2.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };

    uploader2.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
    };

    uploader2.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };

    uploader2.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };

    uploader2.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };

    uploader2.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };

    uploader2.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };

    uploader2.onCompleteAll = function () {
        console.info('onCompleteAll');
    };

    $scope.uploadArchivo = function () {

        $timeout(function () {
            var tamanio = uploader2.queue.length - 1;
            if (tamanio >= 0) $scope.adicionaadjunto();
        }, 100);
    };

    $scope.file = function (index, content) {
        $scope.index = index;
        $scope.codigo = content.codigo;
        $scope.detalle = content.detalle;
        angular.element('#' + content.codigo).trigger('click');
    };

    console.info('uploader', uploader2);
    //FIN DE CARGA DE ARCHIVOS
    ///Fin de Proceso de Carga

    if ($scope.tiposolicitud == 'PR' && $scope.indpage == 'ING') {
        $scope.Grabar = 'Grabar';
        $scope.Cancelar = 'Enviar';
        $scope.isDisabledApr = false;
    }

    if ($scope.tiposolicitud == 'PR' && $scope.indpage == 'APR') {
        $scope.Grabar = 'Aprobar';
        $scope.Cancelar = 'Rechazar';
        $scope.isDisabledApr = false;
    }

    $scope.p_SolProveedor = {
        IdEmpresa: '',
        IdSolicitud: $scope.IdSolicitud, TipoSolicitud: $scope.tiposolicitud, DescTipoSolicitud: '', TipoProveedor: '',
        DescProveedor: '', CodSapProveedor: '', TipoIdentificacion: $scope.tipoidentificacion, DEscTipoIndentificacion: '',
        Identificacion: $scope.identificacion, NomComercial: '', RazonSocial: '', FechaSRI: '',
        SectorComercial: '', DescSectorComercial: '', Idioma: '', DescIdioma: '',
        CodGrupoProveedor: '', GenDocElec: true, FechaSolicitud: '', Estado: '',
        DescEstado: '', GrupoTesoreria: '', DescGrupoTesoreria: '', CuentaAsociada: '',
        DescCuentaAsociada: '', Autorizacion: '', TransfArticuProvAnterior: '', DepSolicitando: '',
        Responsable: '', Aprobacion: '', Comentario: '', TelfFijo: '',
        TelfFijoEXT: '', TelfMovil: '', TelfFax: '', TelfFaxEXT: '',
        EMAILCorp: '', EMAILSRI: '', ClaseContribuyente: '', DescClaseContribuyente: '',
        princliente: '', totalventas: 0, AnioConsti: '', LineaNegocio: '', DescLineaNegocio: '',
        PlazoEntrega: '', DespachaProvincia: '', DescDespachaProvincia: '', GrupoCuenta: '',
        DescGrupoCuenta: '', RetencionIva: '', DescRetencionIva: '', RetencionIva2: '',
        DescRetencionIva2: '', RetencionFuente: '', DescRetencionFuente: '',
        RetencionFuente2: '', DescRetencionFuente2: '', ViaPago: '', DescViaPago: '',
        CondicionPago: '', DescCondicionPago: '',
        TipoActividad: '', FechaCreacion: '', TipoServicio: '', Relacion: false,
        IdentificacionR: '', NomCompletosR: '', AreaLaboraR: '',
        FecTermCalificacion: '',
    }

    $scope.p_SolProvDireccion = {
        IdDireccion: '',
        IdSolicitud: $scope.IdSolicitud, GrupoCompra: '', Pais: '', DescPais: '', Provincia: '',
        DescRegion: '', Ciudad: '', CallePrincipal: '', CalleSecundaria: '',
        PisoEdificio: '', CodPostal: '', Solar: '', Estado: '',
    }

    $scope.p_SolProvContacto = {

        IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
        Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
        PreFijo: '', DepCliente: '', Departamento: '', Funcion: '', RepLegal: true, NotElectronica: false, NotTransBancaria: false,
        Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
        DescDepartamento: '', DescFuncion: '', id: 0, Estadocivil: '', DesEstadocivil: '',
        Conyugetipoidentificacion: '', DesConyugetipoidentificacion: '', ConyugeIdentificacion: '',
        ConyugeNombres: '', ConyugeApellidos: '', ConyugeFechaNac: '', ConyugeNacionalidad: '',
        FechaNacimiento: '', Nacionalidad: '', DesNacionalidad: '', Residencia: '', DesResidencia: '',
        RelacionDependencia: '', DesRelacionLaboral: '', AntiguedadLaboral: '', TipoIngreso: '', DesTipoIngreso: '',
        IngresoMensual: '', TipoParticipante: '',
    }

    $scope.p_SolProvBanco = {

        IdSolBanco: '', IdSolicitud: '', Extrangera: true, CodSapBanco: '',
        NomBanco: '', GrupoCompra: '', Pais: '', DescPAis: '', TipoCuenta: '',
        DesCuenta: '', NumeroCuenta: '', TitularCuenta: '', ReprCuenta: '',
        CodSwift: '', CodBENINT: '', CodABA: '', Principal: true,
        Estado: true,
    }

    $scope.p_SolDocAdjunto = {
        IdSolicitud: '', IdSolDocAdjunto: '', CodDocumento: '', DescDocumento: '',
        NomArchivo: '', Archivo: '', FechaCarga: '', Estado: true, id: 0,
    }

    $scope.p_SolProvHistEstado = {
        IdObservacion: '', IdSolicitud: '', Motivo: '', DesMotivo: '',
        Observacion: '', Usuario: '', Fecha: '', EstadoSolicitud: '',
    }

    $scope.cargasolicitud = function (IdSolicitud) {
        $scope.MenjError = "";

        if ($scope.IdSolicitud != '') {
            $scope.myPromise = null;
            SolicitudProveedor.getSolProveedorList($scope.IdSolicitud).then(function (response) {

                if (response.data != null && response.data.length > 0) {

                    $scope.p_SolProveedor.IdEmpresa = response.data[0].idEmpresa;
                    $scope.p_SolProveedor.IdSolicitud = response.data[0].idSolicitud;
                    $scope.p_SolProveedor.TipoSolicitud = response.data[0].tipoSolicitud;
                    $scope.p_SolProveedor.DescTipoSolicitud = response.data[0].descTipoSolicitud;
                    $scope.p_SolProveedor.TipoProveedor = response.data[0].tipoProveedor;
                    $scope.p_SolProveedor.DescProveedor = response.data[0].descProveedor;
                    $scope.p_SolProveedor.CodSapProveedor = response.data[0].codSapProveedor;
                    $scope.p_SolProveedor.TipoIdentificacion = response.data[0].tipoIdentificacion;
                    $scope.p_SolProveedor.DEscTipoIndentificacion = response.data[0].dEscTipoIndentificacion;
                    $scope.p_SolProveedor.Identificacion = response.data[0].identificacion;
                    $scope.p_SolProveedor.NomComercial = response.data[0].nomComercial;
                    $scope.p_SolProveedor.RazonSocial = response.data[0].razonSocial;
                    $scope.p_SolProveedor.FechaSRI = new Date(response.data[0].fechaSRI);
                    $scope.p_SolProveedor.SectorComercial = response.data[0].sectorComercial;
                    $scope.p_SolProveedor.DescSectorComercial = response.data[0].descSectorComercial;
                    $scope.p_SolProveedor.Idioma = response.data[0].idioma;
                    $scope.p_SolProveedor.DescIdioma = response.data[0].descIdioma;
                    $scope.p_SolProveedor.CodGrupoProveedor = response.data[0].codGrupoProveedor;
                    $scope.p_SolProveedor.ClaseContribuyente = response.data[0].claseContribuyente;
                    $scope.p_SolProveedor.princliente = response.data[0].princliente;
                    $scope.p_SolProveedor.totalventas = response.data[0].totalventas;
                    $scope.p_SolProveedor.AnioConsti = response.data[0].anioConsti;
                    $scope.p_SolProveedor.LineaNegocio = response.data[0].lineaNegocio;

                    if (response.data[0].genDocElec == "true") {
                        $scope.p_SolProveedor.GenDocElec = true;
                    }
                    else {
                        $scope.p_SolProveedor.GenDocElec = false;
                    }

                    $scope.p_SolProveedor.FechaSolicitud = new Date(response.data[0].fechaSolicitud);
                    $scope.p_SolProveedor.Estado = response.data[0].estado;
                    $scope.p_SolProveedor.DescEstado = response.data[0].descEstado;
                    $scope.p_SolProveedor.GrupoTesoreria = response.data[0].grupoTesoreria;
                    $scope.p_SolProveedor.DescGrupoTesoreria = response.data[0].descGrupoTesoreria;
                    $scope.p_SolProveedor.CuentaAsociada = response.data[0].cuentaAsociada;
                    $scope.p_SolProveedor.DescCuentaAsociada = response.data[0].descCuentaAsociada;
                    $scope.p_SolProveedor.Autorizacion = response.data[0].autorizacion;
                    $scope.p_SolProveedor.TransfArticuProvAnterior = response.data[0].transfArticuProvAnterior;
                    $scope.p_SolProveedor.DepSolicitando = response.data[0].depSolicitando;
                    $scope.p_SolProveedor.Responsable = response.data[0].responsable;
                    $scope.p_SolProveedor.Aprobacion = response.data[0].aprobacion;
                    $scope.p_SolProveedor.Comentario = response.data[0].comentario;
                    $scope.p_SolProveedor.TelfFijo = response.data[0].telfFijo;
                    $scope.p_SolProveedor.TelfFijoEXT = response.data[0].telfFijoEXT;
                    $scope.p_SolProveedor.TelfMovil = response.data[0].telfMovil;
                    $scope.p_SolProveedor.TelfFax = response.data[0].telfFax;
                    $scope.p_SolProveedor.TelfFaxEXT = response.data[0].telfFaxEXT;
                    $scope.p_SolProveedor.EMAILCorp = response.data[0].emailCorp;
                    $scope.p_SolProveedor.EMAILSRI = response.data[0].emailsri;
                    $scope.p_SolProveedor.TipoIdentificacion = response.data[0].tipoIdentificacion;
                    $scope.p_SolProveedor.GrupoCompra = response.data[0].grupoCompra;
                    $scope.Estado = $scope.p_SolProveedor.Estado;

                    $scope.p_SolProveedor.FechaCreacion = new Date(response.data[0].fechaCreacion);
                    $scope.p_SolProveedor.TipoActividad = response.data[0].tipoActividad;
                    $scope.p_SolProveedor.TipoServicio = response.data[0].tipoServicio;
                    $scope.p_SolProveedor.Relacion = response.data[0].relacion;
                    $scope.p_SolProveedor.IdentificacionR = response.data[0].identificacionR;
                    $scope.p_SolProveedor.NomCompletosR = response.data[0].nomCompletosR;
                    $scope.p_SolProveedor.AreaLaboraR = response.data[0].areaLaboraR;


                    var index = 0;
                    if ($scope.p_SolProveedor.GrupoCompra != '') {
                        for (index = 0; index < $scope.GrupoCompra.length; index++) {
                            if ($scope.GrupoCompra[index].codigo == $scope.p_SolProveedor.GrupoCompra) {
                                $scope.Ingreso.GrupoCompra = $scope.GrupoCompra[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.TipoProveedor != '') {
                        for (index = 0; index < $scope.ListTipoProveedor.length; index++) {
                            if ($scope.ListTipoProveedor[index].codigo == $scope.p_SolProveedor.TipoProveedor) {
                                $scope.Ingreso.TipoProveedor = $scope.ListTipoProveedor[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.LineaNegocio != '') {
                        for (index = 0; index < $scope.ListLinea.length; index++) {
                            if ($scope.ListLinea[index].codigo == $scope.p_SolProveedor.LineaNegocio) {

                                $scope.Ingreso.LineaNegocio = $scope.ListLinea[index];
                                $scope.GrupoCompraFilt = $filter('filter')($scope.GrupoCompra,
                                    { descAlterno: $scope.Ingreso.LineaNegocio.codigo }, true);
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.TipoIdentificacion != '') {
                        for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                            if ($scope.ListTipoIdentificacion[index].codigo === $scope.p_SolProveedor.TipoIdentificacion) {
                                $scope.Ingreso.tipoidentificacion = $scope.ListTipoIdentificacion[index];

                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.TipoActividad != '') {
                        for (index = 0; index < $scope.ListTipoActividad.length; index++) {
                            if ($scope.ListTipoActividad[index].codigo === $scope.p_SolProveedor.TipoActividad) {
                                $scope.Ingreso.tipoActividad = $scope.ListTipoActividad[index];
                                break;
                            }
                        }

                    }

                    if ($scope.p_SolProvDireccion.TipoServicio != '') {

                        for (index = 0; index < $scope.ListTipoServicio.length; index++) {
                            if ($scope.ListTipoServicio[index].codigo == $scope.p_SolProveedor.TipoServicio) {
                                $scope.Ingreso.tipoServicio = $scope.ListTipoServicio[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.ClaseContribuyente != '') {
                        for (index = 0; index < $scope.ListClaseImpuesto.length; index++) {
                            if ($scope.ListClaseImpuesto[index].codigo === $scope.p_SolProveedor.ClaseContribuyente) {
                                $scope.Ingreso.ClaseImpuesto = $scope.ListClaseImpuesto[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProvDireccion.SectorComercial != '') {

                        for (index = 0; index < $scope.ListSectorComercial.length; index++) {
                            if ($scope.ListSectorComercial[index].codigo == $scope.p_SolProveedor.SectorComercial) {
                                $scope.Ingreso.SectorComercial = $scope.ListSectorComercial[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.Idioma != '') {
                        for (index = 0; index < $scope.ListIdioma.length; index++) {
                            if ($scope.ListIdioma[index].codigo == $scope.p_SolProveedor.Idioma) {
                                $scope.Ingreso.Idioma = $scope.ListIdioma[index];
                                break;
                            }
                        }
                    }
                }
                setTimeout(function () {
                    $("#ddlTipoActividad").trigger('change');
                }, 1000);
            },
                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista Direccion
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolProvDireccionList($scope.IdSolicitud).then(function (response) {

                if (response.data != null && response.data.length > 0) {
                    $scope.p_SolProvDireccion.IdDireccion = response.data[0].idDireccion;
                    $scope.p_SolProvDireccion.IdSolicitud = response.data[0].idSolicitud;
                    $scope.p_SolProvDireccion.Pais = response.data[0].pais;
                    $scope.p_SolProvDireccion.DescPais = response.data[0].descPais;
                    $scope.p_SolProvDireccion.Provincia = response.data[0].provincia;
                    $scope.p_SolProvDireccion.DescRegion = response.data[0].descRegion;
                    $scope.p_SolProvDireccion.Ciudad = response.data[0].ciudad;
                    $scope.p_SolProvDireccion.CallePrincipal = response.data[0].callePrincipal;
                    $scope.p_SolProvDireccion.CalleSecundaria = response.data[0].calleSecundaria;
                    $scope.p_SolProvDireccion.PisoEdificio = response.data[0].pisoEdificio;
                    $scope.p_SolProvDireccion.CodPostal = response.data[0].codPostal;
                    $scope.p_SolProvDireccion.Solar = response.data[0].solar;
                    $scope.p_SolProvDireccion.Estado = response.data[0].estado;

                    var index = 0;
                    if ($scope.p_SolProvDireccion.Pais != '') {
                        for (index = 0; index < $scope.ListPais.length; index++) {
                            if ($scope.ListPais[index].codigo == $scope.p_SolProvDireccion.Pais) {
                                $scope.Ingreso.Pais = $scope.ListPais[index];
                                break;
                            }
                        }
                    }

                    //Cargar catalogo de regiones filtrados por país
                    $scope.ListRegionTemp = $filter('filter')($scope.ListRegion,
                        { descAlterno: $scope.Ingreso.Pais.codigo });

                    if ($scope.p_SolProvDireccion.Provincia != '') {
                        for (index = 0; index < $scope.ListRegionTemp.length; index++) {
                            if ($scope.ListRegionTemp[index].codigo == $scope.p_SolProvDireccion.Provincia) {

                                $scope.Ingreso.Region = $scope.ListRegionTemp[index];
                                break;
                            }
                        }
                    }

                    //Cargar catalogos de ciudades filtrados por region
                    $scope.ListCiudadTemp = $filter('filter')($scope.ListCiudad,
                        { descAlterno: $scope.Ingreso.Region.codigo });

                    if ($scope.p_SolProvDireccion.Ciudad != '') {
                        for (index = 0; index < $scope.ListCiudadTemp.length; index++) {
                            if ($scope.ListCiudadTemp[index].codigo == $scope.p_SolProvDireccion.Ciudad) {
                                $scope.Ingreso.Ciudad = $scope.ListCiudadTemp[index];
                                break;
                            }
                        }
                    }
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista Contacto
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolProvContactoList($scope.IdSolicitud).then(function (response) {

                if (response.data != null && response.data.length > 0) {

                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiacontacto();

                        $scope.p_SolProvContacto.IdSolContacto = response.data[index].idSolContacto;
                        $scope.p_SolProvContacto.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolProvContacto.TipoIdentificacion = response.data[index].tipoIdentificacion;
                        $scope.p_SolProvContacto.DescTipoIdentificacion = response.data[index].descTipoIdentificacion;
                        $scope.p_SolProvContacto.Identificacion = response.data[index].identificacion;
                        $scope.p_SolProvContacto.Nombre2 = response.data[index].nombre2;
                        $scope.p_SolProvContacto.Nombre1 = response.data[index].nombre1;
                        $scope.p_SolProvContacto.Apellido2 = response.data[index].apellido2;
                        $scope.p_SolProvContacto.Apellido1 = response.data[index].apellido1;
                        $scope.p_SolProvContacto.CodSapContacto = response.data[index].codSapContacto;
                        $scope.p_SolProvContacto.PreFijo = response.data[index].preFijo;
                        $scope.p_SolProvContacto.DepCliente = response.data[index].depCliente;
                        $scope.p_SolProvContacto.Departamento = response.data[index].departamento;
                        $scope.p_SolProvContacto.Funcion = response.data[index].funcion;
                        $scope.p_SolProvContacto.RepLegal = response.data[index].repLegal;

                        $scope.p_SolProvContacto.NotElectronica = response.data[index].notElectronica;
                        $scope.p_SolProvContacto.NotTransBancaria = response.data[index].notTransBancaria;

                        $scope.p_SolProvContacto.Estado = response.data[index].estado;
                        $scope.p_SolProvContacto.TelfFijo = response.data[index].telfFijo;
                        $scope.p_SolProvContacto.TelfFijoEXT = response.data[index].telfFijoEXT;
                        $scope.p_SolProvContacto.TelfMovil = response.data[index].telfMovil;
                        $scope.p_SolProvContacto.EMAIL = response.data[index].email;
                        $scope.p_SolProvContacto.DescFuncion = response.data[index].descFuncion;
                        $scope.p_SolProvContacto.DescDepartamento = response.data[index].descDepartamento;

                        $scope.p_SolProvContacto.Estadocivil = response.data[index].estadoCivil;
                        $scope.p_SolProvContacto.Conyugetipoidentificacion = response.data[index].conyugeTipoIdentificacion;
                        $scope.p_SolProvContacto.ConyugeIdentificacion = response.data[index].conyugeIdentificacion;
                        $scope.p_SolProvContacto.ConyugeNombres = response.data[index].conyugeNombres;
                        $scope.p_SolProvContacto.FechaNacimiento = new Date(response.data[index].fechaNacimiento);
                        $scope.p_SolProvContacto.Nacionalidad = response.data[index].nacionalidad;
                        $scope.p_SolProvContacto.Residencia = response.data[index].residencia;
                        $scope.p_SolProvContacto.ConyugeApellidos = response.data[index].conyugeApellidos;
                        $scope.p_SolProvContacto.ConyugeNacionalidad = response.data[index].conyugeNacionalidad;
                        $scope.p_SolProvContacto.ConyugeFechaNac = new Date(response.data[index].conyugeFechaNac);

                        $scope.p_SolProvContacto.RegimenMatrimonial = response.data[index].regimenMatrimonial;
                        $scope.p_SolProvContacto.RelacionDependencia = response.data[index].relacionDependencia;
                        $scope.p_SolProvContacto.AntiguedadLaboral = response.data[index].antiguedadLaboral;
                        $scope.p_SolProvContacto.TipoIngreso = response.data[index].tipoIngreso;
                        $scope.p_SolProvContacto.IngresoMensual = response.data[index].ingresoMensual;
                        $scope.p_SolProvContacto.TipoParticipante = response.data[index].tipoParticipante;

                        $scope.maxcontacto = index + 1;
                        $scope.p_SolProvContacto.id = $scope.maxcontacto;
                        $scope.SolProvContacto.push($scope.p_SolProvContacto);
                    }
                }

            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            $scope.myPromise = SolicitudProveedor.getSolDocAdjuntoList($scope.IdSolicitud).then(function (response) {
                if (response.data != null && response.data.length > 0) {

                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiaadjunto();
                        $scope.p_SolDocAdjunto.id = index;
                        $scope.p_SolDocAdjunto.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolDocAdjunto.IdSolDocAdjunto = response.data[index].idSolDocAdjunto;
                        $scope.p_SolDocAdjunto.CodDocumento = response.data[index].codDocumento;
                        $scope.p_SolDocAdjunto.DescDocumento = response.data[index].descDocumento;
                        $scope.p_SolDocAdjunto.Archivo = response.data[index].archivo;
                        $scope.p_SolDocAdjunto.FechaCarga = response.data[index].fechaCarga;
                        $scope.p_SolDocAdjunto.Estado = response.data[index].estado;
                        $scope.p_SolDocAdjunto.NomArchivo = response.data[index].nomArchivo;
                        $scope.SolDocAdjunto.push($scope.p_SolDocAdjunto);
                        $scope.maxidentificacion = index;
                    }
                    $scope.limpiaadjunto();
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //Carga lista de lineas de negocios
            $scope.myPromise = SolicitudProveedor.getSolLienasNeg($scope.IdSolicitud).then(function (response) {

                if (response.data != null && response.data.length > 0) {
                    var listaSelc = response.data;
                    for (var idx = 0; idx < listaSelc.length; idx++) {
                        var regLinea = $filter('filter')($scope.ListaLineaNegocio, { codigo: listaSelc[idx].codigo }, true);
                        if (regLinea.length > 0) {
                            regLinea[0].chekeado = true;
                            regLinea[0].principal = listaSelc[idx].principal;
                        }
                    }
                }
            },
                function (err) {
                    $scope.MenjError = err.error_description;
                });
        }


    },
        function (err) {
            $scope.MenjError = err.error_description;
        };

    //valida ruc losefocus
    $scope.validorCedulaguradr = function (txtIdentificacion) {

        var campos = txtIdentificacion;

        if (campos.length >= 10) {
            var numero = campos;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            /* Verifico que el campo no contenga letras */
            var ok = 1;

            /* Aqui almacenamos los digitos de la cedula en variables. */
            var d1 = numero.substr(0, 1);
            var d2 = numero.substr(1, 1);
            var d3 = numero.substr(2, 1);
            var d4 = numero.substr(3, 1);
            var d5 = numero.substr(4, 1);
            var d6 = numero.substr(5, 1);
            var d7 = numero.substr(6, 1);
            var d8 = numero.substr(7, 1);
            var d9 = numero.substr(8, 1);
            var d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                $scope.MenjError = "El tercer dígito ingresado es inválido. ";
                $('#idMensajeError').modal('show');
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                var p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                var p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                var p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                var p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                var p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                var p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                var p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                var p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                var p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

            /* Solo para sociedades publicas (modulo 11) */
            /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

            /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            var suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            var residuo = suma % modulo;


            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {

                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001";
                    $('#idMensajeError').modal('show');


                    return false;
                }
            }
            else if (pri == true) {
                
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001";
                    $('#idMensajeError').modal('show');

                    return false;
                }
            }

            else if (nat == true) {
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001";
                    $('#idMensajeError').modal('show');

                    return false;
                }
            }
            return true;
        }
    }

    ///optiene ruta de descarga Adjunto
    $scope.adjuntorutadowloand = function (content) {

        var solpath = content.Archivo;

        if (content.IdSolicitud != '') {
            solpath = content.Archivo;
        }

        SolicitudProveedor.getrutaarchivos(solpath, content.NomArchivo).then(function (response) {

            if (response.data != "") {
                var file = new Blob([response.data], { type: 'application/pdf' });
                var fileURL = window.URL.createObjectURL(file);
                $window.open(fileURL, '_blank', '');
            }
            else {
                $scope.MenjError = "No se pudo consultar el PDF.";
                $('#idMensajeError').modal('show');
            }
        },

            function (err) {
                $scope.MenjError = err.error_description;
            });

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///limpia Adjunto
    $scope.limpiaadjunto = function () {

        $scope.p_SolDocAdjunto = {
            id: 0,
            IdSolicitud: '', IdSolDocAdjunto: '', CodDocumento: '', DescDocumento: '',
            NomArchivo: '', Archivo: '', FechaCarga: '', Estado: true,
        }

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adiona adjunto
    $scope.adicionaadjunto = function () {

        if ($scope.p_SolDocAdjunto != "") {
            if ($scope.p_SolDocAdjunto.Archivo == "") {
                $scope.MenjError = "Seleccione el Archivo... ";
                $('#' + $scope.codigo).val("");
                $('#idMensajeError').modal('show');

                return;
            }

            if ($scope.SolDocAdjunto != "" && $scope.SolDocAdjunto.length > $scope.Ingreso.MaxCantAdjProveedor) {

                $scope.MenjError = ".. "
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.p_SolDocAdjunto.CodDocumento = $scope.codigo;
            $scope.p_SolDocAdjunto.DescDocumento = $scope.detalle;

            var today = new Date();
            var dateString = today.format("dd/mm/yyyy");

            $scope.p_SolDocAdjunto.FechaCarga = dateString;

            if ($scope.isDisabledadjunto == true) {//edita
                var index = 0
                for (index = 0; index < $scope.SolDocAdjunto.length; index++) {
                    if ($scope.SolDocAdjunto[index].id == $scope.p_SolDocAdjunto.id) {

                        $scope.SolDocAdjunto[index].IdSolicitud = $scope.p_SolDocAdjunto.IdSolicitud;
                        $scope.SolDocAdjunto[index].IdSolDocAdjunto = $scope.p_SolDocAdjunto.IdSolDocAdjunto;
                        $scope.SolDocAdjunto[index].CodDocumento = $scope.p_SolDocAdjunto.CodDocumento;
                        $scope.SolDocAdjunto[index].DescDocumento = $scope.p_SolDocAdjunto.DescDocumento;
                        $scope.SolDocAdjunto[index].NomArchivo = $scope.p_SolDocAdjunto.NomArchivo;
                        $scope.SolDocAdjunto[index].Archivo = $scope.p_SolDocAdjunto.Archivo;
                        $scope.SolDocAdjunto[index].FechaCarga = $scope.p_SolDocAdjunto.FechaCarga;
                        $scope.SolDocAdjunto[index].Estado = $scope.p_SolDocAdjunto.Estado;

                        break;
                    }
                }
            }
            else {

                $scope.p_SolDocAdjunto.id = $scope.maxidentificacion + 1;
                $scope.maxidentificacion = $scope.maxidentificacion + 1;
                $scope.SolDocAdjunto.push($scope.p_SolDocAdjunto);

                uploader2.uploadAll();
            }

            $scope.MenjError = "Adjunto ingresado correctamente "
            $('#idMensajeOk').modal('show');
            $('#modal-form-adjunto').modal('hide');
            $scope.limpiaadjunto();
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    $scope.quitarAdjunto = function (nomArchivo) {

        var listaArchivos = $scope.uploader2.queue;
        for (var i = 0; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader2.queue[i]._file.name;
            if (nomArchivo.NomArchivo == nomArchivo2) {
                $scope.uploader2.queue[i].remove();
            }
        }

        for (var i = 0; i < $scope.SolDocAdjunto.length; i++) {
            if ($scope.SolDocAdjunto[i] == nomArchivo) break;
        }

        $scope.SolDocAdjunto.splice(i, 1);
    };

    ///habilita  adjunto
    $scope.disableClickadjunto = function (ban, content) {
        $("#form-field-select-33").val('');
        if (ban == 1) {
            $scope.isDisabledadjunto = true;
        }
        else {
            $scope.isDisabledadjunto = false;
        }
        if (content != "") {
            $scope.limpiaadjunto();

            $scope.p_SolDocAdjunto.IdSolicitud = content.IdSolicitud;
            $scope.p_SolDocAdjunto.IdSolDocAdjunto = content.IdSolDocAdjunto;
            $scope.p_SolDocAdjunto.CodDocumento = content.CodDocumento;
            $scope.p_SolDocAdjunto.DescDocumento = content.DescDocumento;
            $scope.p_SolDocAdjunto.NomArchivo = content.NomArchivo;
            $scope.p_SolDocAdjunto.Archivo = content.Archivo;
            $scope.p_SolDocAdjunto.FechaCarga = content.FechaCarga;
            $scope.p_SolDocAdjunto.Estado = content.Estado;
            $scope.p_SolDocAdjunto.id = content.id;

            var index = 0;

            for (index = 0; index < $scope.ListDocumentoAdjunto.length; index++) {
                if ($scope.ListDocumentoAdjunto[index].codigo === content.CodDocumento) {
                    $scope.Ingreso.DocumentoAdjunto = $scope.ListDocumentoAdjunto[index];

                    break;
                }
            }

            $scope.Grabaradjunto = "Modificar";
        }
        else {
            $scope.limpiaadjunto();
            $scope.Grabaradjunto = "Addicionar";
        }

        $('#modal-form-adjunto').modal('show');
        return false;
    }

    ///limpia conacto
    $scope.limpiacontacto = function () {
        $scope.datosConyuge = false;
        $scope.datosRegimenMatrimonial = false;
        $scope.p_SolProvContacto = {

            IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
            Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
            PreFijo: '', DepCliente: '', Departamento: '', Funcion: '', RepLegal: true, NotElectronica: false, NotTransBancaria: false,
            Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '', id: 0,
            Estadocivil: '', DesEstadocivil: '', Conyugetipoidentificacion: '', DesConyugetipoidentificacion: '', ConyugeIdentificacion: '',
            ConyugeNombres: '', FechaNacimiento: '', DesNacionalidad: '', Nacionalidad: '', DesResidencia: '', Residencia: '',
            RegimenMatrimonial: '', DesRegimenMatrimonial: '', RelacionDependencia: '', DesRelacionLaboral: '', AntiguedadLaboral: '',
            TipoIngreso: '', DesTipoIngreso: '', IngresoMensual: '', ConyugeApellidos: '', ConyugeFechaNac: '', ConyugeNacionalidad: '',
            TipoParticipante: '',
        }

        $scope.Ingreso.Tratamiento = '';
        $scope.Ingreso.Contactipoidentificacion = '';
        $scope.Ingreso.ContacEstadoCivil = '';
        $scope.Ingreso.Conyugetipoidentificacion = '';
        $scope.Ingreso.ContacNacionalidad = '';
        $scope.Ingreso.ContacResidencia = '';
        $scope.Ingreso.ContacRelacionLaboral = '';
        $scope.Ingreso.ContacRegimenMatrimonial = '';
        $scope.Ingreso.ContacRelacionLaboral = '';
        $scope.Ingreso.ContacTipoIngreso = '';
        $scope.Ingreso.ConyugeNacionalidad = '';
        $scope.Ingreso.ContacTipoParticipante = '';
        $scope.Ingreso.DepartaContacto = '';
        $scope.Ingreso.FuncionContacto = '';




    }
        , function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
            $('#idMensajeError').modal('show');
        };

    ///adiona conacto

    $scope.adicionacontacto = function () {

        $scope.p_SolProvContacto.Nombre1 = $scope.p_SolProvContacto.Nombre1 == undefined ? "" : $scope.p_SolProvContacto.Nombre1.toUpperCase();
        $scope.p_SolProvContacto.Nombre2 = $scope.p_SolProvContacto.Nombre2 == undefined ? "" : $scope.p_SolProvContacto.Nombre2.toUpperCase();
        $scope.p_SolProvContacto.Apellido1 = $scope.p_SolProvContacto.Apellido1 == undefined ? "" : $scope.p_SolProvContacto.Apellido1.toUpperCase();
        $scope.p_SolProvContacto.Apellido2 = $scope.p_SolProvContacto.Apellido2 == undefined ? "" : $scope.p_SolProvContacto.Apellido2.toUpperCase();
        $scope.p_SolProvContacto.EMAIL = $scope.p_SolProvContacto.EMAIL == undefined ? "" : $scope.p_SolProvContacto.EMAIL.toUpperCase();

        if ($scope.isDisabledApr) {
            $scope.MenjError = "No Puede Modificar el Contacto... ";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.Ingreso.Contactipoidentificacion.codigo == 'CD' || $scope.Ingreso.Contactipoidentificacion.codigo == 'RC') {
            if ($scope.Ingreso.Contactipoidentificacion.codigo == 'RC') {
                if ($scope.p_SolProvContacto.Identificacion.length < 13) {
                    $scope.MenjError = "El RUC no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.Ingreso.Contactipoidentificacion.codigo == 'CD') {
                if ($scope.p_SolProvContacto.Identificacion.length < 10) {
                    $scope.MenjError = "La cedula no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }
                if ($scope.p_SolProvContacto.Identificacion.length > 10) {
                    $scope.MenjError = "La cedula no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($.isNumeric(($scope.p_SolProvContacto.Identificacion)) == true) {
                if ($scope.validorCedulaguradr($scope.p_SolProvContacto.Identificacion) == false) {
                    return;
                }
            }
            else {
                $scope.MenjError = "Solo se puede ingresar datos numerico en el campo Identificacion.";
                $('#idMensajeError').modal('show');
                return;
            }
        }



        if ($scope.Ingreso.Conyugetipoidentificacion.codigo == 'CD' || $scope.Ingreso.Conyugetipoidentificacion.codigo == 'RC') {
            if ($scope.p_SolProvContacto.Identificacion == $scope.p_SolProvContacto.ConyugeIdentificacion) {
                $scope.MenjError = "La identificacion del conyuge no puede ser la misma que del contacto.";
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.Ingreso.Conyugetipoidentificacion.codigo == 'RC') {
                if ($scope.p_SolProvContacto.ConyugeIdentificacion.length < 13) {
                    $scope.MenjError = "El RUC del conyuge no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
            if ($scope.Ingreso.Conyugetipoidentificacion.codigo == 'CD') {
                if ($scope.p_SolProvContacto.ConyugeIdentificacion.length < 10) {
                    $scope.MenjError = "La cedula del conyuge no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }
                if ($scope.p_SolProvContacto.ConyugeIdentificacion.length > 10) {
                    $scope.MenjError = "La cedula del conyuge no cumple con la longitud necesaria.";
                    $('#idMensajeError').modal('show');
                    return;
                }

            }
            if ($.isNumeric(($scope.p_SolProvContacto.ConyugeIdentificacion)) == true) {
                if ($scope.validorCedulaguradr($scope.p_SolProvContacto.ConyugeIdentificacion) == false) {
                    return;
                }
            }
            else {
                $scope.MenjError = "Solo se puede ingresar datos numerico en el campo Identificacion de conyuge.";
                $('#idMensajeError').modal('show');
                return;
            }
        }

        if ($scope.p_SolProvContacto != "") {

            if ($scope.Ingreso.Contactipoidentificacion != '') {
                $scope.p_SolProvContacto.TipoIdentificacion = $scope.Ingreso.Contactipoidentificacion.codigo;
            }

            if ($scope.Ingreso.Tratamiento != '') {
                $scope.p_SolProvContacto.PreFijo = $scope.Ingreso.Tratamiento.codigo;
                $scope.p_SolProvContacto.DepCliente = $scope.Ingreso.Tratamiento.detalle;
            }

            if ($scope.Ingreso.DepartaContacto != '') {
                $scope.p_SolProvContacto.Departamento = $scope.Ingreso.DepartaContacto.codigo;
                $scope.p_SolProvContacto.DescDepartamento = $scope.Ingreso.DepartaContacto.detalle;
            }

            if ($scope.Ingreso.FuncionContacto != '') {
                $scope.p_SolProvContacto.Funcion = $scope.Ingreso.FuncionContacto.codigo;
                $scope.p_SolProvContacto.DescFuncion = $scope.Ingreso.FuncionContacto.detalle;
            }

            if ($scope.Ingreso.ContacEstadoCivil != '') {
                $scope.p_SolProvContacto.Estadocivil = $scope.Ingreso.ContacEstadoCivil.codigo;
                $scope.p_SolProvContacto.DesEstadocivil = $scope.Ingreso.ContacEstadoCivil.detalle;
            }

            if ($scope.Ingreso.Conyugetipoidentificacion != '') {
                $scope.p_SolProvContacto.Conyugetipoidentificacion = $scope.Ingreso.Conyugetipoidentificacion.codigo;
                $scope.p_SolProvContacto.DesConyugetipoidentificacion = $scope.Ingreso.Conyugetipoidentificacion.detalle;
            }

            if ($scope.Ingreso.ContacNacionalidad != '') {
                $scope.p_SolProvContacto.Nacionalidad = $scope.Ingreso.ContacNacionalidad.codigo;
                $scope.p_SolProvContacto.DesNacionalidad = $scope.Ingreso.ContacNacionalidad.detalle;
            }

            if ($scope.Ingreso.ContacResidencia != '') {
                $scope.p_SolProvContacto.Residencia = $scope.Ingreso.ContacResidencia.codigo;
                $scope.p_SolProvContacto.DesResidencia = $scope.Ingreso.ContacResidencia.detalle;
            }

            if ($scope.Ingreso.ContacRegimenMatrimonial != '' && $scope.Ingreso.ContacRegimenMatrimonial != null) {
                $scope.p_SolProvContacto.RegimenMatrimonial = $scope.Ingreso.ContacRegimenMatrimonial.codigo;
                $scope.p_SolProvContacto.DesRegimenMatrimonial = $scope.Ingreso.ContacRegimenMatrimonial.detalle;
            }

            if ($scope.Ingreso.ContacRelacionLaboral != '') {
                $scope.p_SolProvContacto.RelacionDependencia = $scope.Ingreso.ContacRelacionLaboral.codigo;
                $scope.p_SolProvContacto.DesRelacionLaboral = $scope.Ingreso.ContacRelacionLaboral.detalle;
            }

            if ($scope.Ingreso.ContacTipoIngreso != '') {
                $scope.p_SolProvContacto.TipoIngreso = $scope.Ingreso.ContacTipoIngreso.codigo;
                $scope.p_SolProvContacto.DesTipoIngreso = $scope.Ingreso.ContacTipoIngreso.detalle;
            }

            if ($scope.Ingreso.ConyugeNacionalidad != '') {
                $scope.p_SolProvContacto.ConyugeNacionalidad = $scope.Ingreso.ConyugeNacionalidad.codigo;
            }

            if ($scope.Ingreso.ContacTipoParticipante != '') {
                $scope.p_SolProvContacto.TipoParticipante = $scope.Ingreso.ContacTipoParticipante.codigo;
            }

            if (!$scope.p_SolProvContacto.Nombre1) {
                const input = document.getElementById("form-field-nombre1");
                input.setCustomValidity('El Primer Nombre es  requerido');
                input.reportValidity();
                return;
            }

            if (!$scope.p_SolProvContacto.Apellido1) {
                const input = document.getElementById("form-field-apellido1");
                input.setCustomValidity('El Primer Apellido es requerido');
                input.reportValidity();
                return;
            }

            if (!$scope.p_SolProvContacto.EMAIL) {
                const input = document.getElementById("form-field-email");
                input.setCustomValidity('El Correo es requerido');
                input.reportValidity();
                return;
            }

            if ($scope.datosConyuge && !$scope.p_SolProvContacto.ConyugeNombres) {
                const input = document.getElementById("form-field-conyugeNombres");
                input.setCustomValidity('Los Nombre son requeridos');
                input.reportValidity();
                return;
            }

            if ($scope.datosConyuge && !$scope.p_SolProvContacto.ConyugeApellidos) {
                const input = document.getElementById("form-field-conyugeApellidos");
                input.setCustomValidity('Los Apellidos son requeridos');
                input.reportValidity();
                return;
            }

            if ($scope.p_SolProvContacto.TelfFijo && !/^\d+$/.test($scope.p_SolProvContacto.TelfFijo)) {
                const input = document.getElementById("form-field-telfFijo");
                input.setCustomValidity('Solo se permiten números');
                input.reportValidity();
                input.setCustomValidity('');
                return;
            }

            if (!/^\d+$/.test($scope.p_SolProvContacto.TelfMovil)) {
                const input = document.getElementById("form-field-telfMovil");
                input.setCustomValidity('El Celular es requerido, solo números');
                input.reportValidity();
                return;
            }

            if ($scope.isDisabledContact == true) {//edita
                var index = 0
                for (index = 0; index < $scope.SolProvContacto.length; index++) {

                    if ($scope.SolProvContacto[index].Identificacion == $scope.p_SolProvContacto.Identificacion) {
                        $scope.SolProvContacto[index].IdSolContacto = $scope.p_SolProvContacto.IdSolContacto;
                        $scope.SolProvContacto[index].IdSolicitud = $scope.p_SolProvContacto.IdSolicitud;
                        $scope.SolProvContacto[index].DescTipoIdentificacion = $scope.p_SolProvContacto.DescTipoIdentificacion;
                        $scope.SolProvContacto[index].Nombre2 = $scope.p_SolProvContacto.Nombre2;
                        $scope.SolProvContacto[index].Nombre1 = $scope.p_SolProvContacto.Nombre1;
                        $scope.SolProvContacto[index].Apellido2 = $scope.p_SolProvContacto.Apellido2;
                        $scope.SolProvContacto[index].Apellido1 = $scope.p_SolProvContacto.Apellido1;
                        $scope.SolProvContacto[index].CodSapContacto = $scope.p_SolProvContacto.CodSapContacto;
                        $scope.SolProvContacto[index].PreFijo = $scope.p_SolProvContacto.PreFijo;
                        $scope.SolProvContacto[index].DepCliente = $scope.p_SolProvContacto.DepCliente;
                        $scope.SolProvContacto[index].Departamento = $scope.p_SolProvContacto.Departamento;
                        $scope.SolProvContacto[index].DescDepartamento = $scope.p_SolProvContacto.DescDepartamento;
                        $scope.SolProvContacto[index].Funcion = $scope.p_SolProvContacto.Funcion;
                        $scope.SolProvContacto[index].DescFuncion = $scope.p_SolProvContacto.DescFuncion;
                        $scope.SolProvContacto[index].RepLegal = $scope.p_SolProvContacto.RepLegal;

                        $scope.SolProvContacto[index].NotElectronica = $scope.p_SolProvContacto.NotElectronica;
                        $scope.SolProvContacto[index].NotTransBancaria = $scope.p_SolProvContacto.NotTransBancaria;

                        $scope.SolProvContacto[index].Estado = $scope.p_SolProvContacto.Estado;
                        $scope.SolProvContacto[index].TelfFijo = $scope.p_SolProvContacto.TelfFijo;
                        $scope.SolProvContacto[index].TelfFijoEXT = $scope.p_SolProvContacto.TelfFijoEXT;
                        $scope.SolProvContacto[index].TelfMovil = $scope.p_SolProvContacto.TelfMovil;
                        $scope.SolProvContacto[index].EMAIL = $scope.p_SolProvContacto.EMAIL;

                        $scope.SolProvContacto[index].Estadocivil = $scope.p_SolProvContacto.Estadocivil;
                        $scope.SolProvContacto[index].Conyugetipoidentificacion = $scope.p_SolProvContacto.Conyugetipoidentificacion
                        $scope.SolProvContacto[index].ConyugeIdentificacion = $scope.p_SolProvContacto.ConyugeIdentificacion;
                        $scope.SolProvContacto[index].ConyugeNombres = $scope.p_SolProvContacto.ConyugeNombres;
                        $scope.SolProvContacto[index].ConyugeApellidos = $scope.p_SolProvContacto.ConyugeApellidos;

                        if ($scope.datosConyuge) {
                            $scope.SolProvContacto[index].ConyugeFechaNac = new Date($scope.p_SolProvContacto.ConyugeFechaNac);
                        } else {
                            const tiempoTranscurrido = Date.now();
                            $scope.SolProvContacto[index].ConyugeFechaNac = new Date(tiempoTranscurrido);
                        }
                        $scope.SolProvContacto[index].ConyugeNacionalidad = $scope.p_SolProvContacto.ConyugeNacionalidad;
                        $scope.SolProvContacto[index].FechaNacimiento = new Date($scope.p_SolProvContacto.FechaNacimiento);
                        $scope.SolProvContacto[index].Nacionalidad = $scope.p_SolProvContacto.Nacionalidad;
                        $scope.SolProvContacto[index].Residencia = $scope.p_SolProvContacto.Residencia;

                        $scope.SolProvContacto[index].RegimenMatrimonial = $scope.p_SolProvContacto.RegimenMatrimonial;
                        $scope.SolProvContacto[index].RelacionDependencia = $scope.p_SolProvContacto.RelacionDependencia;
                        $scope.SolProvContacto[index].AntiguedadLaboral = $scope.p_SolProvContacto.AntiguedadLaboral;
                        $scope.SolProvContacto[index].TipoIngreso = $scope.p_SolProvContacto.TipoIngreso;
                        $scope.SolProvContacto[index].IngresoMensual = $scope.p_SolProvContacto.IngresoMensual;
                        $scope.SolProvContacto[index].TipoParticipante = $scope.p_SolProvContacto.TipoParticipante;

                        break;
                    }
                }
            }
            else {//agrega
                for (index = 0; index < $scope.SolProvContacto.length; index++) {
                    if ($scope.datosConyuge) {
                        $scope.SolProvContacto[index].ConyugeFechaNac = new Date($scope.p_SolProvContacto.ConyugeFechaNac);
                    } else {
                        const tiempoTranscurrido = Date.now();
                        $scope.SolProvContacto[index].ConyugeFechaNac = new Date(tiempoTranscurrido);
                    }
                    if (
                        $scope.SolProvContacto[index].Identificacion == $scope.p_SolProvContacto.Identificacion) {

                        $scope.MenjError = "Ya existe un contacto con esta identificación... "
                        $('#idMensajeError').modal('show');
                        return;

                    }
                }

                $scope.maxcontacto = $scope.maxcontacto + 1;
                $scope.p_SolProvContacto.id = $scope.maxcontacto;

                if ($scope.datosConyuge) {

                } else {
                    const tiempoTranscurrido = Date.now();
                    $scope.p_SolProvContacto.ConyugeFechaNac = new Date(tiempoTranscurrido);
                }

                if ($scope.SolProvContacto != "") {
                    $scope.SolProvContacto.push($scope.p_SolProvContacto);
                }
                else {

                    $scope.SolProvContacto = [$scope.p_SolProvContacto];
                }
            }

            $scope.limpiacontacto();
            $scope.MenjError = "Contacto ingresado correctamente "
            $('#idMensajeOk').modal('show');
            $('#modal-form').modal('hide');
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    $("#idMensajeOk").click(function () {
        $scope.Aceptar();
    });

    $scope.CalculaAntiguedad = function (fechaCreacion) {
        var hoy = new Date();
        var cumpleanos = new Date(fechaCreacion);
        var edad = hoy.getFullYear() - cumpleanos.getFullYear();
        var m = hoy.getMonth() - cumpleanos.getMonth();
        if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
            edad--;
        }
        return edad;
    }

    $scope.Aceptar = function () {

        if ($scope.salir == 1) {
            if ($scope.indpage == 'ING') {
                if ($scope.bandera == '2') {
                    localStorageService.remove('SolProv');
                    window.location = "../Home/Index";
                    return;
                }
                if ($scope.bandera == '0') {

                }
                else {
                    $scope.salir = 1;
                    localStorageService.remove('SolProv');

                    var someSessionObj = {

                        'tipoide': '',
                        'identificacion': authService.authentication.ruc,
                        'tipoidentificacion': 'R',
                        'IdSolicitud': $scope.IdSolicitud,
                        'Estado': 'IN',
                        'tiposolicitud': 'NU',
                        'indpage': 'ING',
                        'MenjError': $scope.MenjError



                    };

                    localStorageService.set('SolProv', someSessionObj);
                    window.location = "../Proveedor/frmSolictud";
                }
            }
            if ($scope.indpage == 'APR') {

            }
        }
    },
        function (error) {

            $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
            $('#idMensajeError').modal('show');
        };

    ///habilita identificacion contacto
    $scope.disableClickcontacto = function (ban, content) {

        if (ban == 1) {
            $scope.isDisabledContact = true;
        }
        else {
            $scope.isDisabledContact = false;
        }
        if (content != "") {
            $scope.limpiacontacto();
            $scope.p_SolProvContacto.IdSolContacto = content.IdSolContacto;
            $scope.p_SolProvContacto.IdSolicitud = content.IdSolicitud;
            $scope.p_SolProvContacto.TipoIdentificacion = content.TipoIdentificacion;
            $scope.p_SolProvContacto.DescTipoIdentificacion = content.DescTipoIdentificacion;
            $scope.p_SolProvContacto.Identificacion = content.Identificacion;
            $scope.p_SolProvContacto.Nombre2 = content.Nombre2;
            $scope.p_SolProvContacto.Nombre1 = content.Nombre1;
            $scope.p_SolProvContacto.Apellido2 = content.Apellido2;
            $scope.p_SolProvContacto.Apellido1 = content.Apellido1;
            $scope.p_SolProvContacto.CodSapContacto = content.CodSapContacto;
            $scope.p_SolProvContacto.PreFijo = content.PreFijo;
            $scope.p_SolProvContacto.DepCliente = content.DepCliente;
            $scope.p_SolProvContacto.Departamento = content.Departamento;
            $scope.p_SolProvContacto.Funcion = content.Funcion;
            $scope.p_SolProvContacto.RepLegal = content.RepLegal;

            $scope.p_SolProvContacto.NotElectronica = content.NotElectronica;
            $scope.p_SolProvContacto.NotTransBancaria = content.NotTransBancaria;

            $scope.p_SolProvContacto.Estado = content.Estado;
            $scope.p_SolProvContacto.TelfFijo = content.TelfFijo;
            $scope.p_SolProvContacto.TelfFijoEXT = content.TelfFijoEXT;
            $scope.p_SolProvContacto.TelfMovil = content.TelfMovil;
            $scope.p_SolProvContacto.EMAIL = content.EMAIL;
            $scope.p_SolProvContacto.id = content.id;

            $scope.p_SolProvContacto.ConyugeIdentificacion = content.ConyugeIdentificacion;
            $scope.p_SolProvContacto.ConyugeNombres = content.ConyugeNombres;
            $scope.p_SolProvContacto.ConyugeApellidos = content.ConyugeApellidos;
            $scope.p_SolProvContacto.ConyugeFechaNac = new Date(content.ConyugeFechaNac);
            $scope.p_SolProvContacto.FechaNacimiento = new Date(content.FechaNacimiento);
            $scope.p_SolProvContacto.AntiguedadLaboral = content.AntiguedadLaboral;
            $scope.p_SolProvContacto.IngresoMensual = content.IngresoMensual;

            var index = 0;

            for (index = 0; index < $scope.ListContacEstadoCivil.length; index++) {
                if ($scope.ListContacEstadoCivil[index].codigo === content.Estadocivil) {
                    $scope.Ingreso.ContacEstadoCivil = $scope.ListContacEstadoCivil[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListContacRegimenMatrimonial.length; index++) {
                if ($scope.ListContacRegimenMatrimonial[index].codigo === content.RegimenMatrimonial) {
                    $scope.Ingreso.ContacRegimenMatrimonial = $scope.ListContacRegimenMatrimonial[index];

                    break;
                }
            }

            $scope.datosConyuge = false;
            $scope.datosRegimenMatrimonial = false;
            if (content.Estadocivil == '2452' || content.Estadocivil == '2456') {
                $scope.datosConyuge = true;
            }
            if (content.Estadocivil == '2452') {
                $scope.datosRegimenMatrimonial = true;
            }

            for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                if ($scope.ListTipoIdentificacion[index].codigo === content.TipoIdentificacion) {
                    $scope.Ingreso.Contactipoidentificacion = $scope.ListTipoIdentificacion[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                if ($scope.ListTipoIdentificacion[index].codigo === content.Conyugetipoidentificacion) {
                    $scope.Ingreso.Conyugetipoidentificacion = $scope.ListTipoIdentificacion[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListNacionalidad.length; index++) {
                if ($scope.ListNacionalidad[index].codigo === content.Nacionalidad) {
                    $scope.Ingreso.ContacNacionalidad = $scope.ListNacionalidad[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListPaisResidencia.length; index++) {
                if ($scope.ListPaisResidencia[index].codigo === content.Residencia) {
                    $scope.Ingreso.ContacResidencia = $scope.ListPaisResidencia[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListDepartaContacto.length; index++) {
                if ($scope.ListDepartaContacto[index].codigo === content.Departamento) {
                    $scope.Ingreso.DepartaContacto = $scope.ListDepartaContacto[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListFuncionContactoT.length; index++) {
                if ($scope.ListFuncionContactoT[index].codigo === content.Funcion) {
                    $scope.Ingreso.FuncionContacto = $scope.ListFuncionContactoT[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListTratamiento.length; index++) {
                if ($scope.ListTratamiento[index].codigo === content.PreFijo) {
                    $scope.Ingreso.Tratamiento = $scope.ListTratamiento[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListContacRegimenMatrimonial.length; index++) {
                if ($scope.ListContacRegimenMatrimonial[index].codigo === content.RegimenMatrimonial) {
                    $scope.Ingreso.ContacRegimenMatrimonial = $scope.ListContacRegimenMatrimonial[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListRelacionLaboral.length; index++) {
                if ($scope.ListRelacionLaboral[index].codigo === content.RelacionDependencia) {
                    $scope.Ingreso.ContacRelacionLaboral = $scope.ListRelacionLaboral[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListTipoIngreso.length; index++) {
                if ($scope.ListTipoIngreso[index].codigo === content.TipoIngreso) {
                    $scope.Ingreso.ContacTipoIngreso = $scope.ListTipoIngreso[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListNacionalidad.length; index++) {
                if ($scope.ListNacionalidad[index].codigo === content.ConyugeNacionalidad) {
                    $scope.Ingreso.ConyugeNacionalidad = $scope.ListNacionalidad[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListContacTipoParticipante.length; index++) {
                if ($scope.ListContacTipoParticipante[index].codigo === content.TipoParticipante) {
                    $scope.Ingreso.ContacTipoParticipante = $scope.ListContacTipoParticipante[index];
                    break;
                }
            }

            $scope.Grabarcontacto = "Modificar";
        }
        else {
            $scope.limpiacontacto();

            $scope.Grabarcontacto = "Adicionar";
        }

        $('#modal-form').modal('show');
        return false;
    }

    $scope.validaGrabado = function (pestana) {
        $scope.SaveProveedorList('0', 0);

        if ($scope.pasoValidacion == 0) {
            if ($scope.pestanaActual == "D") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#datosproveedor"]').tab('show');
                    return;
                }, 300);
            }
            else if ($scope.pestanaActual == "C") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#contactoproveedor"]').tab('show');
                    return;
                }, 300);
            }
        }
        $scope.pestanaActual = pestana;
        $scope.pasoValidacion = 0;
    }

    ///grabar solicitud proveedor
    $scope.SaveProveedorList = function (bandera, conMensaje) {
        $scope.salir = 0;


        var exisPrincipal = $filter('filter')($scope.SolProvContacto, { RepLegal: true }, true);
        $scope.SolProveedor = [];
        $scope.SolProvDireccion = [];

        if ($scope.Ingreso.Pais != '' && $scope.Ingreso.Pais != null) {
            $scope.p_SolProvDireccion.Pais = $scope.Ingreso.Pais.codigo;
        }
        else {
            $scope.p_SolProveedor.Pais = "";
        }

        if ($scope.Ingreso.Region != '' && $scope.Ingreso.Region != null) {
            $scope.p_SolProvDireccion.Provincia = $scope.Ingreso.Region.codigo;
        }
        else {
            $scope.p_SolProveedor.Provincia = "";
        }

        if ($scope.Ingreso.Ciudad != '' && $scope.Ingreso.Ciudad != null) {
            $scope.p_SolProvDireccion.Ciudad = $scope.Ingreso.Ciudad.codigo;
        }
        else {
            $scope.p_SolProveedor.Ciudad = "";
        }

        if ($scope.Ingreso.SectorComercial != undefined && $scope.Ingreso.Ciudad != null && $scope.Ingreso.SectorComercial != '') {
            $scope.p_SolProveedor.SectorComercial = $scope.Ingreso.SectorComercial.codigo;
        }
        else {
            $scope.p_SolProveedor.SectorComercial = "";
        }

        if ($scope.Ingreso.TipoProveedor != '' && $scope.Ingreso.TipoProveedor != null) {
            $scope.p_SolProveedor.TipoProveedor = $scope.Ingreso.TipoProveedor.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoProveedor = "";
        }

        if ($scope.Ingreso.Idioma != '' && $scope.Ingreso.Idioma != null) {
            $scope.p_SolProveedor.Idioma = $scope.Ingreso.Idioma.codigo;
        }
        else {
            $scope.p_SolProveedor.Idioma = "";
        }

        if ($scope.Ingreso.GrupoCompra != '' && $scope.Ingreso.GrupoCompra != null) {
            $scope.p_SolProveedor.GrupoCompra = $scope.Ingreso.GrupoCompra.codigo;
        }
        else {
            $scope.p_SolProveedor.GrupoCompra = "";
        }

        if ($scope.Ingreso.TipoProveedor != '' && $scope.Ingreso.TipoProveedor != null) {
            $scope.p_SolProveedor.TipoProveedor = $scope.Ingreso.TipoProveedor.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoProveedor = "";
        }

        if ($scope.Ingreso.ClaseImpuesto != '' && $scope.Ingreso.ClaseImpuesto != null) {
            $scope.p_SolProveedor.ClaseContribuyente = $scope.Ingreso.ClaseImpuesto.codigo;
        }
        else {
            $scope.p_SolProveedor.ClaseContribuyente = "";
        }

        if ($scope.Ingreso.LineaNegocio != '' && $scope.Ingreso.LineaNegocio != null) {
            $scope.p_SolProveedor.LineaNegocio = $scope.Ingreso.LineaNegocio.codigo;
        }
        else {
            $scope.p_SolProveedor.LineaNegocio = "";
        }

        if ($scope.Ingreso.tipoActividad != '' && $scope.Ingreso.tipoActividad != undefined && $scope.Ingreso.tipoActividad != null) {
            $scope.p_SolProveedor.TipoActividad = $scope.Ingreso.tipoActividad.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoActividad = "";
        }

        if ($scope.Ingreso.tipoServicio != '' && $scope.Ingreso.tipoServicio != null) {
            $scope.p_SolProveedor.TipoServicio = $scope.Ingreso.tipoServicio.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoServicio = "";
        }

        if ($scope.tiposolicitud == 'PR' && $scope.indpage == 'ING') {



            if (bandera == '0') {
                if ($scope.IdSolicitud != "") {
                    $scope.p_SolProveedor.Estado = $scope.p_SolProveedor.Estado; //'IP';
                }
                else {
                    $scope.p_SolProveedor.Estado = 'IP';
                }
            }
            else {
                if (exisPrincipal.length == 0) {
                    $scope.MenjError = "Indique al menos un contacto como representante legal de la pestaña Contactos";
                    $('#idMensajeInformativo').modal('show');
                    return;
                }

                if (exisPrincipal.length > 1) {
                    $scope.MenjError = "Solo puede indicar un contacto como representante legal de la pestaña Contactos";
                    $('#idMensajeInformativo').modal('show');
                    return;
                }
                if ($scope.SolDocAdjunto != "") {
                }
                else {
                }
                if ($scope.SolProvContacto != "") {
                    if ($scope.SolProvContacto.length < $scope.MinCantContactoProve) {
                        $scope.MenjError = "Ingrese mínimo   " + $scope.MinCantContactoProve + " contactos";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                }
                else {
                    $scope.MenjError = "Ingrese mínimo   " + $scope.MinCantContactoProve + " contactos";
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.p_SolProvDireccion.Pais != "EC") {

                    if ($scope.SolProvBanco != "" && $scope.SolProvBanco.length > $scope.MaxCantCBanExtrPrv) {
                        $scope.MenjError = "Ingrese al hasta   " + $scope.MaxCantCBanExtrPrv + " cuentas bancarias de la pestaña Banco";
                        $('#idMensajeError').modal('show');

                        return;
                    }
                }
            }
        }
        if ($scope.tiposolicitud == 'PR' && $scope.indpage == 'APR') {

            if (($scope.Estado != '') && ($scope.Estado == "PA" || $scope.Estado == "PR") && $scope.Estado != "IP") {
                $scope.MenjError = "Esta solicitud ha sido atendida ";
                $('#idMensajeError').modal('show');
                return;
            }

            if (bandera == '0') {
                $scope.p_SolProveedor.Estado = 'PA';
            }
            else {
                $scope.p_SolProveedor.Estado = 'PR';
            }
        } else {
            if (bandera == '0') {
                if ($scope.IdSolicitud != "") {
                    $scope.p_SolProveedor.Estado = $scope.p_SolProveedor.Estado; //'IP';
                }
                else {
                    $scope.p_SolProveedor.Estado = 'IP';
                }
            }
            if (bandera == '1') {

                $scope.p_SolProveedor.Estado = 'PA';

            }
        }
        //Validar que seleccione al menos una linea de negocio como principal
        var valLinNegocioP = $filter('filter')($scope.ListaLineaNegocio, { principal: true }, true);
        if (valLinNegocioP.length == 0) {
            $scope.MenjError = "Seleccione al menos una línea de negocio como principal de la pestaña Datos del Proveedor";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        const tiempoTranscurrido = Date.now();
        $scope.p_SolProveedor.FecTermCalificacion = new Date(tiempoTranscurrido);
        $scope.SolProveedor.push($scope.p_SolProveedor);
        $scope.SolProvDireccion.push($scope.p_SolProvDireccion);
        $scope.salir = 1;

        $scope.myPromise = null;
        //Filtrar solo los seleccionados
        var listaLinNegocio = $filter('filter')($scope.ListaLineaNegocio, { chekeado: true }, true);

        $scope.pasoValidacion = 1;

        if (bandera == '1') {

            console.log("antes de getValidaActividadEconomica");
            $scope.myPromise = SolicitudProveedor.getValidaActividadEconomica($scope.p_SolProveedor.Identificacion, $scope.p_SolProveedor.TipoActividad).then(function (response) {
                if (bandera == '1') {
                    console.log("respuesta de getValidaActividadEconomica");
                    if (response.data.split("|")[0] == "False") {
                        console.log("False getValidaActividadEconomica");
                        $scope.p_SolProveedor.Estado = 'PR';
                        $scope.p_SolProvHistEstado.IdSolicitud = $scope.IdSolicitud;
                        $scope.p_SolProvHistEstado.Motivo = "11";
                        $scope.p_SolProvHistEstado.DesMotivo = "Actividad Economica";
                        $scope.p_SolProvHistEstado.Observacion = response.data.split("|")[1];

                        $scope.p_SolProvHistEstado.EstadoSolicitud = $scope.p_SolProveedor.Estado
                        $scope.SolProvHistEstado.push($scope.p_SolProvHistEstado);
                        return false;
                    }
                } else {
                    console.log("no valida getValidaActividadEconomica");
                }
                return true;
            })
                .then(function (response2) {
                    console.log("antes de getPostValidacionPoliticas")
                    if (response2) {
                        $scope.myPromise = SolicitudProveedor.getPostValidacionPoliticas($scope.SolProveedor, $scope.SolProvContacto).then(function (response) {
                            if (bandera == '1') {
                                console.log("respuesta de getPostValidacionPoliticas")
                                if (response.data.split("|")[0] == "False") {
                                    console.log("False getPostValidacionPoliticas")
                                    $scope.MenjError = 'Proveedor no cumple con las politicas establecidas';
                                    $scope.p_SolProveedor.Estado = 'PR';
                                    $scope.p_SolProvHistEstado.IdSolicitud = $scope.IdSolicitud;
                                    $scope.p_SolProvHistEstado.Motivo = "12";
                                    $scope.p_SolProvHistEstado.DesMotivo = "Políticas internas";
                                    $scope.p_SolProvHistEstado.Observacion = response.data.split("|")[1];

                                    $scope.p_SolProvHistEstado.EstadoSolicitud = $scope.p_SolProveedor.Estado
                                    $scope.SolProvHistEstado.push($scope.p_SolProvHistEstado);
                                }
                            }
                            console.log("antes de getPostSolicitudList")
                            $scope.myPromise = SolicitudProveedor.getPostSolicitudList($scope.SolProveedor, $scope.SolProvContacto, '', $scope.SolProvDireccion, $scope.SolDocAdjunto, '', '', '', $scope.SolProvHistEstado, listaLinNegocio).then(function (response) {
                                console.log("respuesta de getPostSolicitudList")
                                $scope.p_SolProveedor.IdSolicitud = response.data;
                                $scope.IdSolicitud = response.data;
                                console.log('2.tiposolicitud: ' + $scope.tiposolicitud);
                                if ($scope.tiposolicitud == 'PR') {
                                    if (bandera == '0') {
                                        if ($scope.indpage == 'ING') {
                                            $scope.MenjError = 'Registro guardado correctamente';
                                        }
                                        if ($scope.indpage == 'APR') {
                                            $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                                        }
                                    }
                                    else {
                                        if ($scope.indpage == 'ING') {
                                            $scope.p_SolProveedor.Estado = 'PA'; //forzar la aprobacion (frank miranda)
                                            if ($scope.p_SolProveedor.Estado == 'PR') {
                                                $scope.MenjError = 'La solicitud ha sido rechazada';
                                                bandera = 0;
                                            } else {
                                                $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                                            }
                                        }
                                        if ($scope.indpage == 'APR') {
                                            $scope.MenjError = 'La solicitud ha sido rechazada';
                                        }
                                    }
                                    if ($scope.p_SolProveedor.Estado == 'PR') {
                                        $scope.bandera = 2;
                                    } else {
                                        $scope.bandera = bandera;
                                    }
                                    if (conMensaje == 1)
                                        $('#idMensajeOk').modal('show');
                                }
                            },
                                function (err) {

                                    $scope.MenjError = "Se ha producido el siguiente error: " + err.data.exceptionMessage;
                                    $('#idMensajeError').modal('show');
                                });

                        });
                    } else {
                        console.log("antes de getPostSolicitudList2")
                        $scope.myPromise = SolicitudProveedor.getPostSolicitudList($scope.SolProveedor, $scope.SolProvContacto, '', $scope.SolProvDireccion, $scope.SolDocAdjunto, '', '', '', $scope.SolProvHistEstado, listaLinNegocio).then(function (response) {
                            console.log("respuesta de getPostSolicitudList2")
                            $scope.p_SolProveedor.IdSolicitud = response.data;
                            $scope.IdSolicitud = response.data;
                            console.log('3.tiposolicitud: ' + $scope.tiposolicitud);
                            if ($scope.tiposolicitud == 'PR') {
                                if (bandera == '0') {
                                    if ($scope.indpage == 'ING') {
                                        $scope.MenjError = 'Registro guardado correctamente';
                                    }
                                    if ($scope.indpage == 'APR') {
                                        $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                                    }
                                }
                                else {
                                    if ($scope.indpage == 'ING') {

                                        if ($scope.p_SolProveedor.Estado == 'PR') {
                                            $scope.MenjError = 'La solicitud ha sido rechazada';
                                            bandera = 0;
                                        } else {
                                            $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                                        }
                                    }
                                    if ($scope.indpage == 'APR') {
                                        $scope.MenjError = 'La solicitud ha sido rechazada';
                                    }
                                }
                                if ($scope.p_SolProveedor.Estado == 'PR') {
                                    $scope.bandera = 2;
                                } else {
                                    $scope.bandera = bandera;
                                }
                                if (conMensaje == 1)
                                    $('#idMensajeOk').modal('show');
                            }
                        },
                            function (err) {

                                $scope.MenjError = "Se ha producido el siguiente error: " + err.data.exceptionMessage;
                                $('#idMensajeError').modal('show');
                            });
                    }
                })
                .then(function (response3) {
                    console.log("valida PM 2")
                    if (!response3) {
                    }

                });

        } else {
            $scope.myPromise = SolicitudProveedor.getPostSolicitudList($scope.SolProveedor, $scope.SolProvContacto, '', $scope.SolProvDireccion, $scope.SolDocAdjunto, '', '', '', $scope.SolProvHistEstado, listaLinNegocio).then(function (response) {
                console.log("respuesta de getPostSolicitudList2")
                $scope.p_SolProveedor.IdSolicitud = response.data;
                $scope.IdSolicitud = response.data;
                console.log('4.tiposolicitud: ' + $scope.tiposolicitud );
                if ($scope.tiposolicitud == 'PR') {
                    if (bandera == '0') {
                        if ($scope.indpage == 'ING') {
                            $scope.MenjError = 'Registro guardado correctamente';
                        }
                        if ($scope.indpage == 'APR') {
                            $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                        }
                    }
                    else {
                        if ($scope.indpage == 'ING') {

                            if ($scope.p_SolProveedor.Estado == 'PR') {
                                $scope.MenjError = 'La solicitud ha sido rechazada';
                                bandera = 0;
                            } else {
                                $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para continuar con el proceso';
                            }
                        }
                        if ($scope.indpage == 'APR') {
                            $scope.MenjError = 'La solicitud ha sido rechazada';
                        }
                    }
                    if ($scope.p_SolProveedor.Estado == 'PR' && $scope.bandera!= "0" ) {             
                        $scope.bandera = 2;
                    } else {
                        $scope.bandera = bandera;
                    }
                    if (conMensaje == 1)
                        $('#idMensajeOk').modal('show');
                }
            },
                function (err) {

                    $scope.MenjError = "Se ha producido el siguiente error: " + err.data.exceptionMessage;
                    $('#idMensajeError').modal('show');

                });
        }
    }, function (error) {

        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');

    }


    $scope.cambionuevo = function () {
        window.location = "../Proveedor/frmSolictud";
    }

    $scope.cargaInicialDocumento = function () {
        //Carga de catalogos de País-Region(provincia)-ciudad
        GeneralService.getCatalogo('tbl_Pais').then(function (results) {

            $scope.ListPais = results.data;

            GeneralService.getCatalogo('tbl_Region').then(function (results) {
                $scope.ListRegion = results.data;
                var a = $scope.ListPais;
                for (var index = 0; index < a.length; ++index) {
                    if (a[index].codigo === "EC")
                        $scope.Ingreso.Pais = a[index];
                }

                GeneralService.getCatalogo('tbl_Ciudad').then(function (results) {
                    $scope.ListCiudad = results.data;

                    if ($scope.IdSolicitud != '') {
                        $scope.cargasolicitud($scope.IdSolicitud);
                        if ($scope.Estado == "PA" || $scope.tiposolicitud == "NU") {
                            $scope.MenjErrorpre = "Diríjase a la opción Calificación para continuar con el proceso"

                            $('#idMensajeInformativocalipre').modal('show');
                        }
                    }
                    else {
                        $scope.p_SolProveedor.EMAILCorp = authService.authentication.userName;
                    }

                }, function (error) {
                });

            }, function (error) {
            });

        }, function (error) {

        });

    }

}]);


