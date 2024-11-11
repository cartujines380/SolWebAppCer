
'use strict';
app.controller('SolicitudController', ['$scope', '$location', '$http', 'SolicitudProveedor', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$timeout', '$window', 'SeguridadService', function ($scope, $location, $http, SolicitudProveedor, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $timeout, $window, SeguridadService) {
    $scope.ValidaCargaProv = "";

    $scope.ListDocumentoAdjunto = [];
    $scope.tipoidentificacionList = [];
    $scope.ListTipoIdentificacion = [];
    $scope.ListSectorComercial = [];
    $scope.ListTipoProveedor = [];
    $scope.ListMotivoRechazoProveedor = [];
    $scope.ListSociedad = [];
    $scope.Sociedad = [];
    $scope.ListLinea = [];
    $scope.Linea = [];
    $scope.GrupoCompraFilt = [];
    $scope.GrupoCompra = [];
    $scope.accion = "";
    $scope.hidecompras = true;
    $scope.hidegerentecompras = true;
    $scope.ListFuncionContactoT = [];
    $scope.hidedatosmaestro = true;
    $scope.hideproveedor = true;
    $scope.ListGrupoTesoreria = [];
    $scope.ListCuentaAsociada = [];
    $scope.SettingGrupoSociedad = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
    $scope.SettingGrupoLinea = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
    $scope.ListPais = [];
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
    $scope.SolProveedor = [];
    $scope.SolProvDireccion = [];
    $scope.SolProvContacto = [];
    $scope.SolProvBanco = [];
    $scope.SolProvHistEstado = [];
    $scope.SolLinea = [];
    $scope.ListFuncionContacto = [];
    $scope.ListDepartaContacto = [];
    $scope.ListBanco = [];
    $scope.ListRegionbancoTemp = [];
    $scope.ListTipoCuenta = [];
    $scope.SolRamo = [];
    $scope.SolZona = [];
    $scope.ListRamo = [];
    $scope.ListRetencionIva = [];
    $scope.ListRetencionIva2 = [];
    $scope.ListRetencionFuente = [];
    $scope.ListRetencionFuente2 = [];
    $scope.ListViaPago = [];
    $scope.ListCondicionPago = [];
    $scope.ListGrupoCuenta = [];
    $scope.ListDespachaProvincia = [];
    $scope.Listautorizacion = [];
    $scope.ViaPago = [];
    $scope.MinCantContactoProve = 0
    $scope.MaxCantCBanExtrPrv = 0;
    $scope.Listbeneficiariobanco = [];
    $scope.bandera = "0";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.hidelineapertenece = false;
    $scope.hidedatosmaestrocom = false;
    $scope.salir = 0;
    $scope.codigo = "";
    $scope.detalle = "";
    $scope.index = 0;
    $scope.pasoValidacion = 0;
    $scope.pestanaActual = "D";
    $scope.opcionTab = "";

    var re = /[+]/g;

    if ($scope.SolLinea.length <= 0) {
        $scope.hidelineapertenece = true;
    }

    //Creacion de nuevo usuario
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

    //integracion
    $scope.ListTipoActividad = [];
    $scope.ListTipoServicio = [];
    $scope.ListContacEstadoCivil = [];
    $scope.datosConyuge = false;
    $scope.ListNacionalidad = [];
    $scope.ListPaisResidencia = [];
    $scope.ListTipoCalificacion = [];
    $scope.ListCalificacion = [];
    $scope.archivoSgs = false;

    $scope.datosRegimenMatrimonial = false;
    $scope.ListContacRegimenMatrimonial = [];
    $scope.ListRelacionLaboral = [];
    $scope.ListTipoIngreso = [];
    $scope.ListProcesoSoporte = [];
    $scope.ListContacTipoParticipante = [];
    $scope.MostrarCritico = false;

    $scope.ListFormaPago = [];
    $scope.hidepagoCuenta = true;
    $scope.hidepagotarjeta = true;
    $scope.hidepagoCheque = true;
    $scope.ListTipoCuentaTmp = [];
    $scope.frmCuenta = false;
    $scope.frmTarjeta = false;
    $scope.bloquearSegundo = false;

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
        Banco: "",
        Paisbanco: "",
        Regionbanco: "",
        TipoCuenta: "",
        Ramo: "",
        RetencionIva: "",
        RetencionIva2: "",
        RetencionFuente: "",
        RetencionFuente2: "",
        ViaPago: "",
        CondicionPago: "",
        GrupoCuenta: "",
        DespachaProvincia: "",
        ZonaOpera: "",
        autorizacion: "",
        beneficiariobanco: "",
        tipoActividad: "",
        tipoServicio: '',
        ContacEstadocivil: '',
        Conyugetipoidentificacion: '',
        ConyugeNacionalidad: '',
        ContacNacionalidad: '',
        ContacResidencia: '',
        TipoCalificacion: '',
        Puntaje: '',
        ArchivoSgs: '',
        NomArchivoSgs: '',
        ProcesoSoporte: '',
        ContacRegimenMatrimonial: '',
        ContacRelacionLaboral: '',
        ContacTipoIngreso: '',
        ContacTipoParticipante: '',
        FormaPago: '',
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
    $scope.maxbanco = 0;
    $scope.isDisabledApr = false;
    $scope.isDisabledNomBanco = false;
    $scope.mensajelogin = '';
    $scope.maxidramo = 0;
    $scope.maxcontacto = 0;
    $scope.ocultarBotones = true;


    if (localStorageService.get('SolProv')) {
        $scope.identificacion = localStorageService.get('SolProv').identificacion;
        $scope.tipoidentificacion = localStorageService.get('SolProv').tipoidentificacion;
        $scope.IdSolicitud = localStorageService.get('SolProv').IdSolicitud;
        $scope.Estado = localStorageService.get('SolProv').Estado;

        if ($scope.Estado == "IN")
            $scope.ocultarBotones = false;
        else
            $scope.ocultarBotones = false;
        $scope.tiposolicitud = localStorageService.get('SolProv').tiposolicitud;
        $scope.indpage = localStorageService.get('SolProv').indpage;
        $scope.mensajelogin = localStorageService.get('SolProv').MenjError;

    }

    if ($scope.indpage == 'ING' || $scope.indpage == '') {
        $scope.hidedatosmaestrocom = true;
    }

    if ($scope.mensajelogin != '' && $scope.indpage == 'ING') {
        if ($scope.Estado != 'DP' && $scope.Estado != 'IN') {
            $scope.MenjError = $scope.mensajelogin;
            $('#idMensajeError').modal('show');
        }
    }

    if ($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') {

        if ($scope.indpage == 'APR') {
            $scope.hidecompras = false;
            $scope.hidegerentecompras = true;
            $scope.hidedatosmaestro = true;
            $scope.hideproveedor = true;
        }

        if ($scope.indpage == 'APG') {
            $scope.hidecompras = true;
            $scope.hidegerentecompras = false;
            $scope.hidedatosmaestro = true;
            $scope.hideproveedor = true;
            $scope.isDisabledApr = true;
        }

        if ($scope.indpage == 'APM') {
            $scope.hidecompras = true;
            $scope.hidegerentecompras = true;
            $scope.hidedatosmaestro = false;
            $scope.hideproveedor = true;
        }

        if ($scope.indpage == 'ING') {
            $scope.hidecompras = true;
            $scope.hidegerentecompras = true;
            $scope.hidedatosmaestro = true;
            $scope.hideproveedor = false;
        }

    }

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=prueba';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    $scope.quitarAdjuntoSGS = function (nomArchivo) {
        $scope.Ingreso.NomArchivoSgs = '';
        $scope.Ingreso.ArchivoSgs = '';
        $scope.archivoSgs = false;
        $scope.index = 0;

        var fileElement = angular.element('#SGS');
        angular.element(fileElement).val(null);

        var listaArchivos = $scope.uploader2.queue;
        for (var i = 0; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader2.queue[i]._file.name;
            if (nomArchivo == nomArchivo2) {
                $scope.uploader2.queue[i].remove();
            }
        }

        for (var i = 0; i < $scope.SolDocAdjunto.length; i++) {
            if ($scope.SolDocAdjunto[i].NomArchivo == nomArchivo) break;
        }

        $scope.SolDocAdjunto.splice(i, 1);
    }

    $scope.quitarAdjunto = function (nomArchivo) {

        if (nomArchivo.CodDocumento == "SGS") {
            $scope.quitarAdjuntoSGS(nomArchivo.NomArchivo);
            return;
        }

        var listaArchivos = $scope.uploader2.queue;
        for (var i = 0; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader2.queue[i]._file.name;
            if (nomArchivo.NomArchivo == nomArchivo2) {
                $scope.uploader2.queue[i].remove();
            }
        }

        for (var i = 0; i < $scope.SolDocAdjunto.length; i++) {
            if ($scope.SolDocAdjunto[i] == nomArchivo) {

                break;
            }
        }

        $scope.SolDocAdjunto.splice(i, 1);

    };

    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });

    $scope.cambioComercial = function (SectorComercial) {
        if (SectorComercial.codigo == "A") {
            for (var index = 0; index < $scope.ListDocumentoAdjunto.length; index++) {
                if ($scope.ListDocumentoAdjunto[index].codigo === "CR") {
                    $scope.ListDocumentoAdjunto[index].descAlterno = "SI";
                    break;
                }
            }
        }
        else {
            for (var index = 0; index < $scope.ListDocumentoAdjunto.length; index++) {
                if ($scope.ListDocumentoAdjunto[index].codigo === "CR") {
                    $scope.ListDocumentoAdjunto[index].descAlterno = "NO";
                    break;
                }
            }

        }


    }

    $scope.confirmaenviar = function (valido, indexform) {

        console.log('valido', valido);
        console.log('vamos enviar el formulario', indexform);

        if (indexform.txtIdentificacion.$invalid) {
            $scope.MenjError = "La identificacion de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.seletipoidentificacionProveedor.$invalid) {
            $scope.MenjError = "Tipo de Identificacion de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }
       
        if (indexform.txtNombreSri.$invalid) {
            $scope.MenjError = "Nombre SRI de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.txtNombreComercial.$invalid) {
            $scope.MenjError = "Nombre Comercial de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.seletipoactividadProveedor.$invalid) {
            $scope.MenjError = "Tipo de Actividad de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.selectpaisprov.$invalid) {
            $scope.MenjError = "Seleccione un Pais de la pestaña Datos del proveedor.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.selectprovinprov.$invalid) {
            $scope.MenjError = "Seleccione una Provincia de la pestaña Datos del proveedor.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        if (indexform.txtCallePrincipalNumero.$invalid) {
            $scope.MenjError = "La Calle Principal - Número de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        if (indexform.TxtCelular.$invalid) {
            $scope.MenjError = "El Teléfono Celular de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if (indexform.txtMailCorpo.$invalid) {
            $scope.MenjError = "El  Correo Corporativo de la pestaña Datos del proveedor es requerido";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.selectlineanegocio.$invalid) {
            $scope.MenjError = "Seleccione una Linea de negocio de la pestaña Datos del proveedor.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if (indexform.selectclaseproveedor.$invalid) {
            $scope.MenjError = "Seleccione Clase de Contribuyente de la pestaña Datos del proveedor";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if (indexform.selectgrupoproveedor.$invalid) {
            $scope.MenjError = "Seleccione Grupo de Compras de la pestaña Datos del proveedor";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.selectSectorComercial.$invalid) {
            $scope.MenjError = "Seleccione un  Sector (Indicador de Minorías) de la pestaña Datos del proveedor";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        if (indexform.selectIdioma.$invalid) {
            $scope.MenjError = "Seleccione un Idioma de la pestaña Datos del proveedor.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (indexform.fechaCreacion.$invalid) {
            $scope.MenjError = "Fecha de constitución de empresa de la pestaña Datos del proveedor es requerida";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if ($scope.p_SolProveedor.Relacion  )
        {
            if (indexform.txtIdentificacionR.$invalid) {
                $scope.MenjError = "La identificacion de la pestaña Datos del proveedor, sección Datos Adicionales es requerido";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (indexform.txtNombresCompletos.$invalid) {
                $scope.MenjError = "Nombres Completos de la pestaña Datos del proveedor es requerido";
                $('#idMensajeInformativo').modal('show');
                return;
            }

            if (indexform.txtAreaR.$invalid) {
                $scope.MenjError = "Área que labora de la pestaña Datos del proveedor es requerido";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }
      

        //Validar que se ingrese un contacto como representante legal
        var exisPrincipal = $filter('filter')($scope.SolProvContacto, { RepLegal: true }, true);
        if (exisPrincipal.length == 0) {
            $scope.MenjError = "Indique al menos un contacto como representante legal de la pestaña Contactos";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        //Validar que seleccione al menos una linea de negocio como principal
        var valLinNegocioP = $filter('filter')($scope.ListaLineaNegocio, { principal: true }, true);
        if (valLinNegocioP.length == 0) {
            $scope.MenjError = "Seleccione al menos una línea de negocio como principal de la pestaña Datos del proveedor";
            $('#idMensajeInformativo').modal('show');
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

    $scope.enviarobservacion = function (accion, valido) {

        $scope.accion = accion;


        $('#modal-form-observacion').modal('show');
    }

    GeneralService.getCatalogo('tbl_beneficiariobanco').then(function (results) {
        $scope.Listbeneficiariobanco = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_ProvGrupoCompras').then(function (results) {
        $scope.GrupoCompra = results.data;

    }, function (error) {
        $scope.Ingreso.MaxCantAdjProveedor = 5;
    });

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

    $scope.myPromise = null;
    $scope.myPromise = SolicitudProveedor.getBancoList("EC").then(function (results) {
        $scope.ListBanco = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_Ramo').then(function (results) {
        $scope.ListRamo = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_RetencionIva').then(function (results) {
        $scope.ListRetencionIva = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_RetencionIva2').then(function (results) {
        $scope.ListRetencionIva2 = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_RetencionFuente').then(function (results) {
        $scope.ListRetencionFuente = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_RetencionFuente2').then(function (results) {
        $scope.ListRetencionFuente2 = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_ViaPago').then(function (results) {
        $scope.ListViaPago = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_CondicionPago').then(function (results) {
        $scope.ListCondicionPago = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_orgcomp').then(function (results) {
        $scope.ListGrupoCuenta = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_DespachaProvincia').then(function (results) {
        $scope.ListDespachaProvincia = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_autorizacion').then(function (results) {
        $scope.Listautorizacion = results.data;
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

    GeneralService.getCatalogo('tbl_MaxMegaProveedor').then(function (results) {
        $scope.ListMaxMegaProveedor = results.data;
        if ($scope.ListMaxMegaProveedor != "") {
            $scope.Ingreso.MaxMegaProveedor = $scope.ListMaxMegaProveedor[0].codigo;
        }

    }, function (error) {
        $scope.Ingreso.MaxMegaProveedor = 2;
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
        if ($scope.Ingreso.Region != '' && angular.isUndefined($scope.Ingreso.Region) != true) {
            $scope.ListCiudadTemp = $filter('filter')($scope.ListCiudad,
                { descAlterno: $scope.Ingreso.Region.codigo });
        }
    });

    $scope.$watch('Ingreso.FormaPago', function () {
        $scope.hidepagoCuenta = true;
        $scope.hidepagotarjeta = true;
        $scope.hidepagoCheque = true;
        $scope.frmCuenta = false;
        $scope.frmTarjeta = false;
        console.log('Ingreso.FormaPago', $scope.Ingreso.FormaPago);
        if ($scope.Ingreso.FormaPago != '' && angular.isUndefined($scope.Ingreso.FormaPago) != true) {
            $scope.ListTipoCuentaTmp = $filter('filter')($scope.ListTipoCuenta,
                { descAlterno: $scope.Ingreso.FormaPago.codigo });

            if ($scope.Ingreso.FormaPago.codigo == '1') {
                $scope.hidepagoCuenta = false;
                $scope.frmCuenta = true;
            }
            if ($scope.Ingreso.FormaPago.codigo == '2') {
                $scope.hidepagotarjeta = false;
                $scope.frmTarjeta = true;
            }
            if ($scope.Ingreso.FormaPago.codigo == '3') {
                $scope.hidepagoCheque = false;
            }
        }

    });

    $scope.$watch('Ingreso.Paisbanco', function () {

        $scope.ListRegionbancoTemp = [];
        if ($scope.Ingreso.Paisbanco != null) {
            if ($scope.Ingreso.Paisbanco != '' && angular.isUndefined($scope.Ingreso.Paisbanco) != true) {
                $scope.ListRegionbancoTemp = $filter('filter')($scope.ListRegion,
                    { descAlterno: $scope.Ingreso.Paisbanco.codigo });

                if ($scope.Ingreso.Paisbanco.codigo != 'EC') {
                    $scope.isDisabledNomBanco = false;
                }
                else {
                    $scope.isDisabledNomBanco = true;
                }


            }
            else {
                $scope.isDisabledNomBanco = false;
            }
        }

    });

    $scope.$watch('Ingreso.SectorComercial', function () {

    });

    GeneralService.getCatalogo('tbl_Idioma').then(function (results) {
        $scope.ListIdioma = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_SectorComercial').then(function (results) {
        $scope.ListSectorComercial = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_GrupoCuenta').then(function (results) {
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

    GeneralService.getCatalogo('tbl_TipoCuenta').then(function (results) {
        $scope.ListTipoCuenta = results.data;
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

    GeneralService.getCatalogo('tbl_tipoCalificacionSGS').then(function (results) {
        $scope.ListTipoCalificacion = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_puntajeCalificacionSGS').then(function (results) {
        $scope.ListCalificacion = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoParticipante_neo').then(function (results) {
        $scope.ListContacTipoParticipante = results.data;
    }, function (error) {
    });
    //ListProcesoSoporte
    GeneralService.getCatalogo('tbl_procesoSoporte').then(function (results) {
        $scope.ListProcesoSoporte = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_FPagoContrato').then(function (results) {
        $scope.ListFormaPago = results.data;
    }, function (error) {
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
    }



    $scope.eliminacontacto = function (registro) {
        $scope.accion = 998;
        $scope.registro = registro;
        $scope.MenjConfirmacion = '¿Está seguro de eliminar contacto?';
        $('#idMensajeConfirmacionEliminar').modal('show');
    }

    $scope.quitarBanco = function (registro) {
        $scope.accion = 997;
        $scope.registro = registro;
        $scope.MenjConfirmacion = '¿Está seguro de eliminar banco?';
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
        //Eliminar banco

        if ($scope.accion == 997) {
            for (var i = 0, len = $scope.SolProvBanco.length; i < len; i++) {
                if ($scope.SolProvBanco[i].CodSapBanco === $scope.registro.CodSapBanco &&
                    $scope.SolProvBanco[i].NumeroCuenta == $scope.registro.NumeroCuenta
                ) {
                    break;
                }
            }

            $scope.SolProvBanco.splice(i, 1);
        }
    }

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
        var secuencia = "";
        var superior = 100000;
        var inferior = 1;
        var resAleatorio = Math.floor((Math.random() * (superior - inferior + 1)) + inferior);
        var today = new Date();
        var dateString = today.format("ddmmyyyyHHMMss");

        $scope.rutaDir = serviceBase + 'api/Upload/D/?path=';

        var direc = "SolProv_" + dateString + resAleatorio;
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
            if (extension == "pdf") {
                item.name = filename;
                return true;
            }
            else {
                $scope.MenjError = "Debe seleccionar archivos con formato pdf.";
                $scope.p_SolDocAdjunto.Archivo = "";
                $('#adjuntoarchivo').val('').clone(true);
                $('#idMensajeError').modal('show');

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
                $scope.MenjError = "has execido la cantidad de archivos permitidos ";
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

        if ($scope.ValidaCargaProv != "") {
            console.log("Ejecucion de prov.");
            //Escribe en el adm
            $scope.myPromise = SolicitudProveedor.EscribePDFAdjuntos($scope.rutaDirectorio, $scope.ValidaCargaProv).then(function (results) {
                if (results.data.lSuccess) {
                    console.log("EscribePDFAdjuntos PDF " + $scope.ValidaCargaProv + " remota exitosa");
                }
                else {
                    console.log("Error EscribePDFAdjuntos.");
                }
            }, function (error) {
                console.log("Error EscribePDFAdjuntos: " + error);
            });
            //fin escritura al adm
        }
        $scope.ValidaCargaProv = "";

    };

    uploader2.onCompleteAll = function () {
        console.info('onCompleteAll');
    };

    $scope.uploadArchivo = function () {

        $timeout(function () {
            var tamanio = uploader2.queue.length - 1;
            if (tamanio >= 0) $scope.adicionaadjunto();
        }, 200);
    };

    $scope.file = function (index, content) {
        $scope.salir = 0;
        $scope.index = index;
        $scope.codigo = content.codigo;
        $scope.detalle = content.detalle;

        var existeArchivo = false;

        for (var i = 0; i < $scope.SolDocAdjunto.length; i++) {
            if ($scope.SolDocAdjunto[i].CodDocumento == $scope.codigo) {
                existeArchivo = true;
                break;
            }

        }

        if (existeArchivo) {

            $scope.MenjError = "El documento ya se encuentra cargado, para ingresar uno nuevo elimine el documento anterior de la lista.";
            $('#idMensajeError').modal('show');

        } else {
            angular.element('#' + content.codigo).val(null);
            angular.element('#' + content.codigo).trigger('click');
        }


    };

    $scope.file2 = function (index, content) {
        $scope.index = $scope.index + 1;
        $scope.codigo = "SGS";
        $scope.detalle = "Certificado de calificación de SGS";
        angular.element('#' + content).trigger('click');
    };

    console.info('uploader', uploader2);
    //FIN DE CARGA DE ARCHIVOS
    ///Fin de Proceso de Carga

    if (($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') && $scope.indpage == 'ING') {
        $scope.Grabar = 'Grabar';
        $scope.Cancelar = 'Enviar';
        $scope.isDisabledApr = false;
    }

    if (($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') && $scope.indpage == 'APR') {
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
        DescEstado: '', GrupoTesoreria: '', DescGrupoTesoreria: '', CuentaAsociada: '', GrupoCompra: '',
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
        IdentificacionR: '', NomCompletosR: '', AreaLaboraR: '', FecTermCalificacion: '',
        TipoCalificacion: '', Calificacion: '', PersonaExpuesta: false, EleccionPopular: false,
        EsCritico: 'N', ProcesoBrindaSoporte: '', Sgs: 'SI',
    }

    $scope.p_SolProvDireccion = {
        IdDireccion: '',
        IdSolicitud: $scope.IdSolicitud, Pais: '', DescPais: '', Provincia: '',
        DescRegion: '', Ciudad: '', CallePrincipal: '', CalleSecundaria: '',
        PisoEdificio: '', CodPostal: '', Solar: '', Estado: '',
    }

    $scope.p_SolProvContacto = {

        IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
        Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
        PreFijo: '', DepCliente: '', Departamento: '', Funcion: '', RepLegal: true,
        Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
        DescDepartamento: '', DescFuncion: '', NotElectronica: false,
        NotTransBancaria: false, id: 0, Estadocivil: '', DesEstadocivil: '',
        Conyugetipoidentificacion: '', DesConyugetipoidentificacion: '', ConyugeIdentificacion: '',
        ConyugeNombres: '', ConyugeApellidos: '', ConyugeFechaNac: '', ConyugeNacionalidad: '',
        FechaNacimiento: '', Nacionalidad: '', DesNacionalidad: '', Residencia: '', DesResidencia: '',
        RelacionDependencia: '', DesRelacionLaboral: '', AntiguedadLaboral: '', TipoIngreso: '', DesTipoIngreso: '',
        IngresoMensual: '', TipoParticipante: '',
    }

    $scope.p_SolProvBanco = {
        id: 0, FormaPago: '', DesFormaPago: '',
        IdSolBanco: '', IdSolicitud: '', Extrangera: true, CodSapBanco: '',
        NomBanco: '', Pais: '', DescPAis: '', TipoCuenta: '',
        DesCuenta: '', NumeroCuenta: '', TitularCuenta: '', ReprCuenta: '',
        CodSwift: '', CodBENINT: '', CodABA: '', Principal: true,
        Estado: true, Provincia: '', DescProvincia: '', DirBancoExtranjero: '',
        BancoExtranjero: ''
    }

    $scope.p_SolDocAdjunto = {
        IdSolicitud: '', IdSolDocAdjunto: '', CodDocumento: '', DescDocumento: '',
        NomArchivo: '', Archivo: '', FechaCarga: '', Estado: true, id: 0,
    }

    $scope.p_SolProvHistEstado = {
        IdObservacion: '', IdSolicitud: '', Motivo: '', DesMotivo: '',
        Observacion: '', Usuario: '', Fecha: '', EstadoSolicitud: '', DesEstadoSolicitud: ""
    }


    $scope.p_SolRamo = {
        IdSolicitud: '', IdRamo: '', CodRamo: '', DescRamo: '',
        Estado: true, id: 0, Principal: false

    }

    $scope.p_SolZona = {
        IdSolicitud: '', CodZona: '', DescZona: '', Estado: true
    }

    $scope.p_ViaPago = {
        IdSolicitud: '', CodVia: '', DescVia: '', Estado: true, IdVia: ''
    }

    ///limpia LimpiaViapago
    $scope.limpiaViapago = function () {
        $scope.p_ViaPago = {
            IdSolicitud: '', CodVia: '', DescVia: '', Estado: true, IdVia: ''
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    $scope.limpiaRamo = function () {
        $scope.p_SolRamo = {
            IdSolicitud: '', IdRamo: '', CodRamo: '', DescRamo: '',
            Estado: true, id: 0, Principal: false

        }

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    $scope.cambiarClaseContribuyente = function () {
        if (angular.isUndefined($scope.Ingreso.ClaseImpuesto) != true && $scope.Ingreso.ClaseImpuesto != '') {
            $scope.ListDocumentoAdjunto = [];
            SolicitudProveedor.getListDocumentoAdjunto($scope.Ingreso.ClaseImpuesto.codigo).then(function (results) {
                console.log('Ingreso.ClaseImpuesto', $scope.Ingreso.ClaseImpuesto)
                console.log('p_SolProveedor.ClaseContribuyente', $scope.p_SolProveedor.ClaseContribuyente)
                console.log('doc:', results.data);
                for (var i = 0; i < results.data.length; i++) {
                    var grid = {};
                    grid.codigo = results.data[i].codigo;
                    grid.descAlterno = results.data[i].obligatorio;
                    grid.detalle = results.data[i].descripcion;
                    grid.ver = true;
                    $scope.ListDocumentoAdjunto.push(grid);

                }
                $scope.cambiarProcesoCritico();

            }, function (error) {
                $scope.MenjError = err.error_description;
            });
        }

    }

    $scope.actulizaListaDocAdjuntos = function () {
        var indexa = 0;
        for (indexa = 0; indexa < $scope.ListDocumentoAdjunto.length; indexa++) {
            if ($scope.ListDocumentoAdjunto[indexa].codigo == "CV" || $scope.ListDocumentoAdjunto[indexa].codigo == "EH") {
                $scope.ListDocumentoAdjunto.splice(indexa);
            }
        }

    }

    $scope.cambiarProcesoCritico = function () {

        $scope.actulizaListaDocAdjuntos();

        if ($scope.Ingreso.ProcesoSoporte != undefined && $scope.Ingreso.ProcesoSoporte != '') {
            if ($scope.Ingreso.ProcesoSoporte.codigo == "P3" || $scope.Ingreso.ProcesoSoporte.codigo == "P4") {
                SolicitudProveedor.getListAdjuntoCondicional($scope.Ingreso.ProcesoSoporte.codigo).then(function (results) {
                    for (var i = 0; i < results.data.length; i++) {
                        var grid = {};
                        grid.codigo = results.data[i].codigo;
                        grid.descAlterno = results.data[i].obligatorio;
                        grid.detalle = results.data[i].descripcion;
                        grid.ver = true;
                        $scope.ListDocumentoAdjunto.push(grid);
                    }

                }, function (error) {
                    $scope.MenjError = err.error_description;
                });
            }
        }




    }

    $scope.cargasolicitud = function (IdSolicitud) {
        console.log('llamar cargasolicitud');

        if (IdSolicitud != '') {
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolProveedorList($scope.IdSolicitud).then(function (response) {

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
                    $scope.p_SolProveedor.FechaSRI = response.data[0].fechaSRI;
                    $scope.p_SolProveedor.SectorComercial = response.data[0].sectorComercial;

                    $scope.p_SolProveedor.DescSectorComercial = response.data[0].descSectorComercial;
                    $scope.p_SolProveedor.Idioma = response.data[0].idioma;
                    $scope.p_SolProveedor.DescIdioma = response.data[0].descIdioma;
                    $scope.p_SolProveedor.CodGrupoProveedor = response.data[0].codGrupoProveedor;
                    $scope.p_SolProveedor.ClaseContribuyente = response.data[0].claseContribuyente;
                    $scope.p_SolProveedor.princliente = response.data[0].princliente;
                    $scope.p_SolProveedor.totalventas = response.data[0].totalventas;
                    $scope.p_SolProveedor.AnioConsti = response.data[0].anioConsti;
                    $scope.p_SolProveedor.PlazoEntrega = response.data[0].plazoEntrega;
                    $scope.p_SolProveedor.DespachaProvincia = response.data[0].despachaProvincia;
                    $scope.p_SolProveedor.DescDespachaProvincia = response.data[0].descDespachaProvincia;
                    $scope.p_SolProveedor.GrupoCuenta = response.data[0].grupoCuenta;
                    $scope.p_SolProveedor.DescGrupoCuenta = response.data[0].descGrupoCuenta;
                    $scope.p_SolProveedor.RetencionIva = response.data[0].retencionIva;
                    $scope.p_SolProveedor.DescRetencionIva = response.data[0].descRetencionIva;
                    $scope.p_SolProveedor.RetencionIva2 = response.data[0].retencionIva2;
                    $scope.p_SolProveedor.DescRetencionIva2 = response.data[0].descRetencionIva2;
                    $scope.p_SolProveedor.RetencionFuente = response.data[0].retencionFuente;
                    $scope.p_SolProveedor.DescRetencionFuente = response.data[0].descRetencionFuente;
                    $scope.p_SolProveedor.RetencionFuente2 = response.data[0].retencionFuente2;
                    $scope.p_SolProveedor.DescRetencionFuente2 = response.data[0].descRetencionFuente2;
                    $scope.p_SolProveedor.CondicionPago = response.data[0].condicionPago;
                    $scope.p_SolProveedor.DescCondicionPago = response.data[0].descCondicionPago;

                    if (response.data[0].genDocElec == "true") {
                        $scope.p_SolProveedor.GenDocElec = true;
                    }
                    else {
                        $scope.p_SolProveedor.GenDocElec = false;
                    }
                    $scope.p_SolProveedor.FechaSolicitud = response.data[0].fechaSolicitud;
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
                    $scope.Estado = $scope.p_SolProveedor.Estado;
                    $scope.p_SolProveedor.LineaNegocio = response.data[0].lineaNegocio;
                    $scope.p_SolProveedor.GrupoCompra = response.data[0].grupoCompra;

                    $scope.p_SolProveedor.FechaCreacion = new Date(response.data[0].fechaCreacion);
                    $scope.p_SolProveedor.TipoActividad = response.data[0].tipoActividad;
                    $scope.p_SolProveedor.TipoServicio = response.data[0].tipoServicio;
                    $scope.p_SolProveedor.Relacion = response.data[0].relacion;
                    $scope.p_SolProveedor.IdentificacionR = response.data[0].identificacionR;
                    $scope.p_SolProveedor.NomCompletosR = response.data[0].nomCompletosR;
                    $scope.p_SolProveedor.AreaLaboraR = response.data[0].areaLaboraR;

                    $scope.p_SolProveedor.EsCritico = response.data[0].esCritico;
                    $scope.p_SolProveedor.ProcesoBrindaSoporte = response.data[0].procesoBrindaSoporte;
                    $scope.p_SolProveedor.Sgs = response.data[0].sgs;
                    $scope.p_SolProveedor.TipoCalificacion = response.data[0].tipoCalificacion;
                    $scope.p_SolProveedor.Calificacion = response.data[0].calificacion;
                    $scope.p_SolProveedor.FecTermCalificacion = new Date(response.data[0].fecTermCalificacion);
                    $scope.p_SolProveedor.PersonaExpuesta = response.data[0].personaExpuesta
                    $scope.p_SolProveedor.EleccionPopular = response.data[0].eleccionPopular;

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

                    if ($scope.p_SolProveedor.Autorizacion != '') {
                        for (index = 0; index < $scope.Listautorizacion.length; index++) {
                            if ($scope.Listautorizacion[index].codigo == $scope.p_SolProveedor.Autorizacion) {
                                $scope.Ingreso.autorizacion = $scope.Listautorizacion[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.GrupoTesoreria != '') {
                        for (index = 0; index < $scope.ListGrupoTesoreria.length; index++) {
                            if ($scope.ListGrupoTesoreria[index].codigo == $scope.p_SolProveedor.GrupoTesoreria) {
                                $scope.Ingreso.GrupoTesoreria = $scope.ListGrupoTesoreria[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.RetencionIva != '') {
                        for (index = 0; index < $scope.ListRetencionIva.length; index++) {
                            if ($scope.ListRetencionIva[index].codigo == $scope.p_SolProveedor.RetencionIva) {
                                $scope.Ingreso.RetencionIva = $scope.ListRetencionIva[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.RetencionIva2 != '') {
                        for (index = 0; index < $scope.ListRetencionIva2.length; index++) {
                            if ($scope.ListRetencionIva2[index].codigo == $scope.p_SolProveedor.RetencionIva2) {
                                $scope.Ingreso.RetencionIva2 = $scope.ListRetencionIva2[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.RetencionFuente != '') {
                        for (index = 0; index < $scope.ListRetencionFuente.length; index++) {
                            if ($scope.ListRetencionFuente[index].codigo == $scope.p_SolProveedor.RetencionFuente) {
                                $scope.Ingreso.RetencionFuente = $scope.ListRetencionFuente[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.RetencionFuente2 != '') {
                        for (index = 0; index < $scope.ListRetencionFuente2.length; index++) {
                            if ($scope.ListRetencionFuente2[index].codigo == $scope.p_SolProveedor.RetencionFuente2) {
                                $scope.Ingreso.RetencionFuente2 = $scope.ListRetencionFuente2[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.SectorComercial == "A") {

                        for (var index = 0; index < $scope.ListDocumentoAdjunto.length; index++) {
                            if ($scope.ListDocumentoAdjunto[index].codigo === "CR") {
                                $scope.ListDocumentoAdjunto[index].descAlterno = "SI";
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.CondicionPago != '') {
                        for (index = 0; index < $scope.ListCondicionPago.length; index++) {
                            if ($scope.ListCondicionPago[index].codigo == $scope.p_SolProveedor.CondicionPago) {
                                $scope.Ingreso.CondicionPago = $scope.ListCondicionPago[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.GrupoCuenta != '') {
                        for (index = 0; index < $scope.ListGrupoCuenta.length; index++) {
                            if ($scope.ListGrupoCuenta[index].codigo == $scope.p_SolProveedor.GrupoCuenta) {
                                $scope.Ingreso.GrupoCuenta = $scope.ListGrupoCuenta[index];
                                break;

                            }
                        }
                    }

                    if ($scope.p_SolProveedor.DespachaProvincia != '') {
                        for (index = 0; index < $scope.ListDespachaProvincia.length; index++) {
                            if ($scope.ListDespachaProvincia[index].codigo == $scope.p_SolProveedor.DespachaProvincia) {
                                $scope.Ingreso.DespachaProvincia = $scope.ListDespachaProvincia[index];
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

                    if ($scope.p_SolProveedor.TipoServicio != '') {

                        for (index = 0; index < $scope.ListTipoServicio.length; index++) {
                            if ($scope.ListTipoServicio[index].codigo == $scope.p_SolProveedor.TipoServicio) {
                                $scope.Ingreso.tipoServicio = $scope.ListTipoServicio[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.TipoCalificacion != '') {

                        for (index = 0; index < $scope.ListTipoCalificacion.length; index++) {
                            if ($scope.ListTipoCalificacion[index].codigo == $scope.p_SolProveedor.TipoCalificacion) {
                                $scope.Ingreso.TipoCalificacion = $scope.ListTipoCalificacion[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.Calificacion != '') {

                        for (index = 0; index < $scope.ListCalificacion.length; index++) {
                            if ($scope.ListCalificacion[index].codigo == $scope.p_SolProveedor.Calificacion) {
                                $scope.Ingreso.Puntaje = $scope.ListCalificacion[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProveedor.ProcesoBrindaSoporte != '') {

                        for (index = 0; index < $scope.ListProcesoSoporte.length; index++) {
                            if ($scope.ListProcesoSoporte[index].codigo == $scope.p_SolProveedor.ProcesoBrindaSoporte) {
                                $scope.Ingreso.ProcesoSoporte = $scope.ListProcesoSoporte[index];
                                break;
                            }
                        }
                    }
                    if ($scope.p_SolProveedor.EsCritico == 'S') {

                    }
                    $scope.ListDocumentoAdjunto = [];
                    SolicitudProveedor.getListDocumentoAdjunto($scope.Ingreso.ClaseImpuesto.codigo).then(function (results) {
                        for (var i = 0; i < results.data.length; i++) {
                            var grid = {};
                            grid.codigo = results.data[i].codigo;
                            grid.descAlterno = results.data[i].obligatorio;
                            grid.detalle = results.data[i].descripcion;
                            grid.ver = true;
                            $scope.ListDocumentoAdjunto.push(grid);
                        }
                        $scope.cambiarProcesoCritico();
                    }, function (err) {
                        $scope.MenjError = err.error_description;
                    });


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
                        $scope.p_SolProvContacto.Estado = response.data[index].estado;
                        $scope.p_SolProvContacto.TelfFijo = response.data[index].telfFijo;
                        $scope.p_SolProvContacto.TelfFijoEXT = response.data[index].telfFijoEXT;
                        $scope.p_SolProvContacto.TelfMovil = response.data[index].telfMovil;
                        $scope.p_SolProvContacto.EMAIL = response.data[index].email;
                        $scope.p_SolProvContacto.DescFuncion = response.data[index].descFuncion;
                        $scope.p_SolProvContacto.DescDepartamento = response.data[index].descDepartamento;
                        $scope.p_SolProvContacto.NotTransBancaria = response.data[index].notTransBancaria;
                        $scope.p_SolProvContacto.NotElectronica = response.data[index].notElectronica;

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

                        $scope.p_SolProvContacto.id = index + 1;
                        $scope.SolProvContacto.push($scope.p_SolProvContacto);
                    }

                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });



            //carga lista adjunto 
            $scope.myPromise = null;
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

            //carga lista Ramo
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getRamoList($scope.IdSolicitud).then(function (response) {
                if (response.data != null && response.data.length > 0) {


                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiaRamo();
                        $scope.p_SolRamo.id = index;
                        $scope.p_SolRamo.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolRamo.IdRamo = response.data[index].idRamo;
                        $scope.p_SolRamo.CodRamo = response.data[index].codRamo;
                        $scope.p_SolRamo.DescRamo = response.data[index].descRamo;
                        $scope.p_SolRamo.Estado = response.data[index].estado;
                        $scope.p_SolRamo.Principal = response.data[index].principal;



                        $scope.SolRamo.push($scope.p_SolRamo);
                        $scope.maxidramo = index;
                    }
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista Zona
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolZonaList($scope.IdSolicitud).then(function (response) {
                if (response.data != null && response.data.length > 0) {


                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiazona();

                        $scope.p_SolZona.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolZona.IdZona = response.data[index].idZona;
                        $scope.p_SolZona.CodZona = response.data[index].codZona;
                        $scope.p_SolZona.DescZona = response.data[index].descZona;
                        $scope.p_SolZona.Estado = response.data[index].estado;
                        $scope.SolZona.push($scope.p_SolZona);
                    }
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista Via
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getViaList($scope.IdSolicitud).then(function (response) {
                if (response.data != null && response.data.length > 0) {


                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiaViapago();
                        $scope.p_ViaPago.IdVia = response.data[index].idVia;
                        $scope.p_ViaPago.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_ViaPago.CodVia = response.data[index].codVia;
                        $scope.p_ViaPago.DescVia = response.data[index].descVia;
                        $scope.p_ViaPago.Estado = response.data[index].estado;
                        $scope.ViaPago.push($scope.p_ViaPago);
                    }
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista HisEstado 
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolProvHistEstadoList($scope.IdSolicitud).then(function (response) {
                if (response.data != null && response.data.length > 0) {

                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {
                        $scope.limpiamotivo();
                        $scope.p_SolProvHistEstado.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolProvHistEstado.IdObservacion = response.data[index].idObservacion;
                        $scope.p_SolProvHistEstado.Motivo = response.data[index].motivo;
                        $scope.p_SolProvHistEstado.DesMotivo = response.data[index].desMotivo;
                        $scope.p_SolProvHistEstado.DesEstadoSolicitud = response.data[index].desEstadoSolicitud;

                        $scope.p_SolProvHistEstado.Observacion = response.data[index].observacion;
                        $scope.p_SolProvHistEstado.Usuario = response.data[index].usuario;
                        $scope.p_SolProvHistEstado.Fecha = response.data[index].fecha;
                        $scope.p_SolProvHistEstado.EstadoSolicitud = response.data[index].estadoSolicitud;
                        $scope.SolProvHistEstado.push($scope.p_SolProvHistEstado);
                        $scope.limpiamotivo();
                    }
                }
            },

                function (err) {
                    $scope.MenjError = err.error_description;
                });
            //carga lista Banco
            $scope.myPromise = null;
            $scope.myPromise = SolicitudProveedor.getSolProvBancoList($scope.IdSolicitud).then(function (response) {

                if (response.data != null && response.data.length > 0) {

                    var index = 0;
                    for (index = 0; index < response.data.length; index++) {

                        $scope.limpiabanco();

                        $scope.p_SolProvBanco.id = index;
                        $scope.p_SolProvBanco.IdSolBanco = response.data[index].idSolBanco;
                        $scope.p_SolProvBanco.IdSolicitud = response.data[index].idSolicitud;
                        $scope.p_SolProvBanco.Extrangera = response.data[index].extrangera;
                        $scope.p_SolProvBanco.CodSapBanco = response.data[index].codSapBanco;
                        $scope.p_SolProvBanco.NomBanco = response.data[index].nomBanco;
                        $scope.p_SolProvBanco.Pais = response.data[index].pais;
                        $scope.p_SolProvBanco.DescPAis = response.data[index].descPAis;
                        $scope.p_SolProvBanco.TipoCuenta = response.data[index].tipoCuenta;
                        $scope.p_SolProvBanco.DesCuenta = response.data[index].desCuenta;
                        $scope.p_SolProvBanco.NumeroCuenta = response.data[index].numeroCuenta;
                        $scope.p_SolProvBanco.TitularCuenta = response.data[index].titularCuenta;
                        $scope.p_SolProvBanco.ReprCuenta = response.data[index].reprCuenta;
                        $scope.p_SolProvBanco.CodSwift = response.data[index].codSwift;
                        $scope.p_SolProvBanco.CodBENINT = response.data[index].codBENINT;
                        $scope.p_SolProvBanco.CodABA = response.data[index].codABA;
                        $scope.p_SolProvBanco.Principal = response.data[index].principal;
                        $scope.p_SolProvBanco.Estado = response.data[index].estado;
                        $scope.p_SolProvBanco.Provincia = response.data[index].provincia;
                        $scope.p_SolProvBanco.DescProvincia = response.data[index].descProvincia;
                        $scope.p_SolProvBanco.DirBancoExtranjero = response.data[index].dirBancoExtranjero;
                        $scope.p_SolProvBanco.BancoExtranjero = response.data[index].bancoExtranjero;
                        $scope.p_SolProvBanco.FormaPago = response.data[index].formaPago;
                        $scope.p_SolProvBanco.DesFormaPago = response.data[index].desFormaPago;
                        $scope.maxbanco = index;
                        $scope.SolProvBanco.push($scope.p_SolProvBanco);
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

    ///limpia Motivo
    $scope.limpiamotivo = function () {

        $scope.p_SolProvHistEstado = {
            IdObservacion: '', IdSolicitud: '', Motivo: '', DesMotivo: '',
            Observacion: '', Usuario: '', Fecha: '', EstadoSolicitud: '', DesEstadoSolicitud: ''
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///optiene ruta de descarga Adjunto
    $scope.adjuntorutadowloand = function (content) {

        var solpath = content.Archivo;

        if (content.IdSolicitud != '') {
            solpath = content.Archivo;
        }

        $scope.myPromise = null;
        $scope.myPromise = SolicitudProveedor.getrutaarchivos(solpath, content.NomArchivo).then(function (response) {

            if (response.data != "") {
                debugger;
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

    ///adiciona Via
    $scope.adicionavia = function () {

        $scope.limpiaViapago();

        if ($scope.Ingreso.ViaPago != '') {
            $scope.p_ViaPago.CodVia = $scope.Ingreso.ViaPago.codigo;
            $scope.p_ViaPago.DescVia = $scope.Ingreso.ViaPago.detalle;
        }
        else {

            $scope.MenjError = "Seleccione la via...";
            $('#idMensajeError').modal('show');
            return
        }

        for (var index = 0; index < $scope.ViaPago.length; index++) {
            if ($scope.ViaPago[index].CodVia == $scope.p_ViaPago.CodVia) {

                $scope.MenjError = "La via de pago ingresada ya existe...";
                $('#idMensajeError').modal('show');
                return

                break;
            }
        }

        $scope.ViaPago.push($scope.p_ViaPago);

        $scope.p_ViaPago = {};

        $scope.MenjError = "Vía de pago ingresado correctamente "
        $('#idMensajeOk').modal('show');

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
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
                $scope.MenjError = "El tercer dígito ingresado es inválido ";
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

    //valida ruc losefocus
    $scope.validorCedulafocus = function (txtIdentificacion) {

        var campos = txtIdentificacion;

        if (campos.length == 13) {
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
                p_SolProveedor.CodGrupoProveedor = "";

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
                    p_SolProveedor.CodGrupoProveedor = "";


                    return false;
                }
            }
            else if (pri == true) {
                
                if (numero.substr(10, 3) != '001') {
                    p_SolProveedor.CodGrupoProveedor = "";

                    return false;
                }
            }

            else if (nat == true) {
                
                if (numero.length > 10 && numero.substr(10, 3) != '001') {

                    p_SolProveedor.CodGrupoProveedor = "";
                    return false;
                }
            }
            return true;
        }
        else {
            p_SolProveedor.CodGrupoProveedor = "";
        }
    }

    //valida Ruc
    $scope.validorCedula = function (txtIdentificacion) {

        var campos = txtIdentificacion;

        if (campos.length == 10 || campos.length == 13) {
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
                $scope.MenjError = "El tercer dígito ingresado es inválido ";
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


    ///limpia  Zona
    $scope.limpiazona = function () {

        $scope.p_SolZona = {
            IdSolicitud: '', CodZona: '', DescZona: '', Estado: true
        }

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adiciona zona
    $scope.adicionazona = function () {

        $scope.limpiazona();

        if ($scope.Ingreso.ZonaOpera != '') {
            $scope.p_SolZona.CodZona = $scope.Ingreso.ZonaOpera.codigo;
            $scope.p_SolZona.DescZona = $scope.Ingreso.ZonaOpera.detalle;
        }
        else {

            $scope.MenjError = "Se leccione la Zona...";
            $('#idMensajeError').modal('show');
            return
        }

        var index = 0;
        for (index = 0; index < $scope.SolZona.length; index++) {
            if ($scope.SolZona[index].CodZona == $scope.p_SolZona.CodZona) {

                $scope.MenjError = "La zona ingresada ya existe...";
                $('#idMensajeError').modal('show');
                return

                break;
            }
        }

        $scope.SolZona.push($scope.p_SolZona);

        $scope.p_SolZona = {};

        $scope.MenjError = "Zona ingresado correctamente "
        $('#idMensajeOk').modal('show');

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///limpia  Rama
    $scope.limpiarama = function () {

        $scope.p_SolRamo = {
            IdSolicitud: '', IdRamo: '', CodRamo: '', DescRamo: '',
            Estado: true, id: 0, Principal: false
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adiciona Rama
    $scope.adicionarama = function () {

        if ($scope.Ingreso.Ramo != '') {
            $scope.p_SolRamo.CodRamo = $scope.Ingreso.Ramo.codigo;
            $scope.p_SolRamo.DescRamo = $scope.Ingreso.Ramo.detalle;
        }
        else {

            $scope.MenjError = "Se leccione un Ramo...";
            $('#idMensajeError').modal('show');
            return
        }

        var index = 0;
        for (index = 0; index < $scope.SolRamo.length; index++) {
            if ($scope.SolRamo[index].CodRamo == $scope.p_SolRamo.CodRamo) {

                $scope.MenjError = "La rama ingresada ya existe...";
                $('#idMensajeError').modal('show');
                return
                break;
            }

            if ($scope.SolRamo[index].Principal == true && $scope.p_SolRamo.Principal == true) {

                $scope.MenjError = "YA existe una Rama Principal...";
                $('#idMensajeError').modal('show');
                return

                break;
            }
        }

        $scope.SolRamo.push($scope.p_SolRamo);
        $scope.p_SolRamo = {};
        $scope.limpiarama();
        $scope.MenjError = "Ramo ingresado correctamente "
        $('#idMensajeOk').modal('show');

    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };


    ///limpia Banco
    $scope.limpiabanco = function () {

        $scope.p_SolProvBanco = {
            id: 0, FormaPago: '', DesFormaPago: '',
            IdSolBanco: '', IdSolicitud: '', Extrangera: true, CodSapBanco: '',
            NomBanco: '', Pais: '', DescPAis: '', TipoCuenta: '',
            DesCuenta: '', NumeroCuenta: '', TitularCuenta: '', ReprCuenta: '',
            CodSwift: '', CodBENINT: '', CodABA: '', Principal: true,
            Estado: true, Provincia: '', DescProvincia: '', DirBancoExtranjero: '',
            BancoExtranjero: ''
        }

        $scope.Ingreso.FormaPago = '';
        $scope.Ingreso.TipoCuenta = '';
        $scope.Ingreso.Paisbanco = '';

        $scope.Ingreso.Banco = '';

        $scope.Ingreso.Regionbanco = '';


    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adiona Banco
    $scope.adicionabanco = function () {
	  $scope.salir = 0;

        if ($scope.p_SolProvBanco != "") {

            if ($scope.Ingreso.FormaPago != '') {
                $scope.p_SolProvBanco.FormaPago = $scope.Ingreso.FormaPago.codigo;
                $scope.p_SolProvBanco.DesFormaPago = $scope.Ingreso.FormaPago.detalle;

                if ($scope.Ingreso.FormaPago.codigo == '1') {
                    $scope.hidepagoCuenta = false;
                    $scope.frmCuenta = true;
                }
                if ($scope.Ingreso.FormaPago.codigo == '2') {
                    $scope.hidepagotarjeta = false;
                    $scope.frmTarjeta = true;
                }
                if ($scope.Ingreso.FormaPago.codigo == '3') {
                    $scope.hidepagoCheque = false;
                }
            }

            if (!$scope.hidepagoCuenta) {
                if ($scope.Ingreso.Paisbanco != '') {
                    $scope.p_SolProvBanco.Pais = $scope.Ingreso.Paisbanco.codigo;
                    $scope.p_SolProvBanco.DescPAis = $scope.Ingreso.Paisbanco.detalle;
                }
                else {

                    $scope.MenjError = "Ingrese el Pais ..";
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.Ingreso.Regionbanco != '' && $scope.Ingreso.Regionbanco != null) {
                    $scope.p_SolProvBanco.Provincia = $scope.Ingreso.Regionbanco.codigo;
                    $scope.p_SolProvBanco.DescProvincia = $scope.Ingreso.Regionbanco.detalle;
                }
                else {

                    $scope.MenjError = "Ingrese la Region ..";
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.p_SolProvBanco.Pais != 'EC' && ($scope.p_SolProvBanco.BancoExtranjero == "" || $scope.p_SolProvBanco.BancoExtranjero == null)) {

                    $scope.MenjError = "Ingrese el Banco Extranjero ..";
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.p_SolProvBanco.Pais != 'EC' && ($scope.p_SolProvBanco.DirBancoExtranjero == "" || $scope.p_SolProvBanco.DirBancoExtranjero == null)) {

                    $scope.MenjError = "Ingrese La direccion del Banco Extranjero ..";
                    $('#idMensajeError').modal('show');
                    return;
                }

                if ($scope.Ingreso.Banco != '') {
                    $scope.p_SolProvBanco.CodSapBanco = $scope.Ingreso.Banco.codBanco;
                    $scope.p_SolProvBanco.NomBanco = $scope.Ingreso.Banco.nomBanco;
                }

                if ($scope.Ingreso.beneficiariobanco != '' && $scope.Ingreso.beneficiariobanco != null) {
                    $scope.p_SolProvBanco.CodBENINT = $scope.Ingreso.beneficiariobanco.codigo;
                }
            }


            if (!$scope.hidepagoCheque) {
                $scope.p_SolProvBanco.TipoCuenta = $scope.ListTipoCuentaTmp[0].codigo;
                $scope.p_SolProvBanco.DesCuenta = $scope.ListTipoCuentaTmp[0].detalle;

            } else {
                if ($scope.Ingreso.TipoCuenta != '' && angular.isUndefined($scope.Ingreso.TipoCuenta) != true) {
                    $scope.p_SolProvBanco.TipoCuenta = $scope.Ingreso.TipoCuenta.codigo;
                    $scope.p_SolProvBanco.DesCuenta = $scope.Ingreso.TipoCuenta.detalle;
                } else {

                    $scope.MenjError = "Ingrese el tipo de cuenta..";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }







            if ($scope.isDisabledbanco == true) {//edita
                var index = 0
                debugger;
                for (index = 0; index < $scope.SolProvBanco.length; index++) {
                    if ($scope.SolProvBanco[index].CodSapBanco == $scope.p_SolProvBanco.CodSapBanco &&
                        $scope.SolProvBanco[index].NumeroCuenta == $scope.p_SolProvBanco.NumeroCuenta) {
                        $scope.SolProvBanco[index].IdSolBanco = $scope.p_SolProvBanco.IdSolBanco;
                        $scope.SolProvBanco[index].IdSolicitud = $scope.p_SolProvBanco.IdSolicitud;
                        $scope.SolProvBanco[index].Extrangera = $scope.p_SolProvBanco.Extrangera;
                        $scope.SolProvBanco[index].CodSapBanco = $scope.p_SolProvBanco.CodSapBanco;
                        $scope.SolProvBanco[index].NomBanco = $scope.p_SolProvBanco.NomBanco;
                        $scope.SolProvBanco[index].Pais = $scope.p_SolProvBanco.Pais;
                        $scope.SolProvBanco[index].DescPAis = $scope.p_SolProvBanco.DescPAis;
                        $scope.SolProvBanco[index].TipoCuenta = $scope.p_SolProvBanco.TipoCuenta;
                        $scope.SolProvBanco[index].DesCuenta = $scope.p_SolProvBanco.DesCuenta;
                        $scope.SolProvBanco[index].NumeroCuenta = $scope.p_SolProvBanco.NumeroCuenta;
                        $scope.SolProvBanco[index].TitularCuenta = $scope.p_SolProvBanco.TitularCuenta;
                        $scope.SolProvBanco[index].ReprCuenta = $scope.p_SolProvBanco.ReprCuenta;
                        $scope.SolProvBanco[index].CodSwift = $scope.p_SolProvBanco.CodSwift;
                        $scope.SolProvBanco[index].CodBENINT = $scope.p_SolProvBanco.CodBENINT;
                        $scope.SolProvBanco[index].CodABA = $scope.p_SolProvBanco.CodABA;
                        $scope.SolProvBanco[index].Principal = $scope.p_SolProvBanco.Principal;
                        $scope.SolProvBanco[index].Estado = $scope.p_SolProvBanco.Estado;
                        $scope.SolProvBanco[index].Provincia = $scope.p_SolProvBanco.Provincia;
                        $scope.SolProvBanco[index].DescProvincia = $scope.p_SolProvBanco.DescProvincia;
                        $scope.SolProvBanco[index].DirBancoExtranjero = $scope.p_SolProvBanco.DirBancoExtranjero;
                        $scope.SolProvBanco[index].BancoExtranjero = $scope.p_SolProvBanco.BancoExtranjero;
                        $scope.SolProvBanco[index].FormaPago = $scope.p_SolProvBanco.FormaPago;
                        $scope.MenjError = "Información de banco actualizada correctamente."

                        break;
                    }
                }
            }
            else {
                var index = 0
                for (index = 0; index < $scope.SolProvBanco.length; index++) {
                    if ($scope.SolProvBanco[index].CodSapBanco == $scope.p_SolProvBanco.CodSapBanco &&
                        $scope.SolProvBanco[index].NumeroCuenta == $scope.p_SolProvBanco.NumeroCuenta) {
                        $scope.MenjError = "Número de cuenta ya registrado al mismo banco. ";
                        $('#idMensajeError').modal('show');
                        $scope.limpiabanco();
                        return;
                    }
                }

                $scope.p_SolProvBanco.id = $scope.maxbanco + 1;
                $scope.maxbanco = $scope.maxbanco + 1;


                $scope.MenjError = "Banco ingresado correctamente "
                $scope.SolProvBanco.push($scope.p_SolProvBanco);
            }

            $('#idMensajeOk').modal('show');
            $scope.opcionTab = "B";
            $('#modal-form-banco').modal('hide');
            $scope.limpiabanco();
        }
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
    $scope.adicionaadjunto = function (documento) {
        var nombre = $scope.p_SolDocAdjunto.NomArchivo;
        if ($scope.p_SolDocAdjunto != "") {
            if ($scope.p_SolDocAdjunto.Archivo == "") {
                $scope.MenjError = "Seleccione el Archivo... ";
                $('#' + $scope.codigo).val("");
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
                var extension = nombre.substring(nombre.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "pdf") {
                    $scope.p_SolDocAdjunto.id = $scope.maxidentificacion + 1;
                    $scope.maxidentificacion = $scope.maxidentificacion + 1;
                    $scope.SolDocAdjunto.push($scope.p_SolDocAdjunto);


                    $scope.ValidaCargaProv = nombre;
                    uploader2.uploadAll();


                }
            }

            if ($scope.codigo == "SGS") {
                $scope.adjuntoSgs(nombre);
            }

            $scope.p_SolDocAdjunto = {};

            $scope.MenjError = "Adjunto ingresado correctamente "
            $('#idMensajeOk').modal('show');
            $('#modal-form-adjunto').modal('hide');
            $scope.limpiaadjunto();
        }
    }, function (error) {
        $scope.ValidaCargaProv = "";
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adjunto SGS
    $scope.adjuntoSgs = function (nombreArchivo) {
        $scope.archivoSgs = true;
        $scope.Ingreso.NomArchivoSgs = nombreArchivo;
    }

    ///habilita  adjunto
    $scope.disableClickadjunto = function (ban, content) {

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
            $scope.Grabaradjunto = "Adicionar";
        }

        $('#modal-form-adjunto').modal('show');
        return false;
    }

    $scope.limpiacontacto = function () {
        $scope.datosConyuge = false;
        $scope.p_SolProvContacto = {

            IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
            Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
            PreFijo: '', DepCliente: '', Departamento: '', Funcion: '', RepLegal: true,
            Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
            DescDepartamento: '', DescFuncion: '', NotElectronica: false,
            NotTransBancaria: false, id: 0,
            Estadocivil: '', DesEstadocivil: '', Conyugetipoidentificacion: '',
            DesConyugetipoidentificacion: '', ConyugeIdentificacion: '',
            ConyugeNombres: '', FechaNacimiento: '', DesNacionalidad: '',
            Nacionalidad: '', DesResidencia: '', Residencia: '',
            RegimenMatrimonial: '', DesRegimenMatrimonial: '', RelacionDependencia: '',
            DesRelacionLaboral: '', AntiguedadLaboral: '',
            TipoIngreso: '', DesTipoIngreso: '', IngresoMensual: '',
            ConyugeApellidos: '', ConyugeFechaNac: '', ConyugeNacionalidad: '',
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
	      $scope.salir = 0;
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

            if ($scope.Ingreso.ContacRegimenMatrimonial != '') {
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
                        $scope.SolProvContacto[index].Estado = $scope.p_SolProvContacto.Estado;
                        $scope.SolProvContacto[index].TelfFijo = $scope.p_SolProvContacto.TelfFijo;
                        $scope.SolProvContacto[index].TelfFijoEXT = $scope.p_SolProvContacto.TelfFijoEXT;
                        $scope.SolProvContacto[index].TelfMovil = $scope.p_SolProvContacto.TelfMovil;
                        $scope.SolProvContacto[index].EMAIL = $scope.p_SolProvContacto.EMAIL;
                        $scope.SolProvContacto[index].NotTransBancaria = $scope.p_SolProvContacto.NotTransBancaria;
                        $scope.SolProvContacto[index].NotElectronica = $scope.p_SolProvContacto.NotElectronica;

                        $scope.SolProvContacto[index].Estadocivil = $scope.p_SolProvContacto.Estadocivil
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
            $scope.opcionTab = "C";
            $scope.p_SolProvContacto = {};
            $scope.limpiacontacto();

            $scope.MenjError = "Contacto ingresado correctamente "
            $('#idMensajeOk').modal('show');

            $('#modal-form').modal('hide');

        }
    }, function (error) {
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
            $scope.p_SolProvContacto.Estado = content.Estado;
            $scope.p_SolProvContacto.TelfFijo = content.TelfFijo;
            $scope.p_SolProvContacto.TelfFijoEXT = content.TelfFijoEXT;
            $scope.p_SolProvContacto.TelfMovil = content.TelfMovil;
            $scope.p_SolProvContacto.EMAIL = content.EMAIL;
            $scope.p_SolProvContacto.NotElectronica = content.NotElectronica;
            $scope.p_SolProvContacto.NotTransBancaria = content.NotTransBancaria;
            $scope.p_SolProvContacto.id = content.id;

            $scope.p_SolProvContacto.ConyugeIdentificacion = content.ConyugeIdentificacion;
            $scope.p_SolProvContacto.ConyugeNombres = content.ConyugeNombres;
            $scope.p_SolProvContacto.ConyugeApellidos = content.ConyugeApellidos;
            $scope.p_SolProvContacto.ConyugeFechaNac = new Date(content.ConyugeFechaNac);

            $scope.p_SolProvContacto.FechaNacimiento = new Date(content.FechaNacimiento);

            $scope.p_SolProvContacto.AntiguedadLaboral = content.AntiguedadLaboral;
            $scope.p_SolProvContacto.IngresoMensual = content.IngresoMensual;

            var index = 0;

            for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                if ($scope.ListTipoIdentificacion[index].codigo === content.TipoIdentificacion) {
                    $scope.Ingreso.Contactipoidentificacion = $scope.ListTipoIdentificacion[index];

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

    $scope.cargaregion = function () {
        if ($scope.Ingreso.Paisbanco != null) {
            if ($scope.Ingreso.Paisbanco != '' && angular.isUndefined($scope.Ingreso.Paisbanco) != true) {
                $scope.ListRegionbancoTemp = $filter('filter')($scope.ListRegion,
                    { descAlterno: $scope.Ingreso.Paisbanco.codigo });

                if ($scope.Ingreso.Paisbanco.codigo != 'EC') {
                    $scope.isDisabledNomBanco = false;
                }
                else {
                    $scope.isDisabledNomBanco = true;
                }


            }
            else {
                $scope.isDisabledNomBanco = false;
            }
        }
    }

    //habilita Banco
    $scope.disableClickbanco = function (ban, content) {

        $scope.limpiabanco();
        if (ban == 1) {
            $scope.isDisabledbanco = true;
        }
        else {
            $scope.isDisabledbanco = false;
        }
        if (content != "") {

            $scope.p_SolProvBanco.id = content.id
            $scope.p_SolProvBanco.IdSolBanco = content.IdSolBanco;
            $scope.p_SolProvBanco.IdSolicitud = content.IdSolicitud;
            $scope.p_SolProvBanco.Extrangera = content.Extrangera;
            $scope.p_SolProvBanco.CodSapBanco = content.CodSapBanco;
            $scope.p_SolProvBanco.NomBanco = content.NomBanco;
            $scope.p_SolProvBanco.Pais = content.Pais;
            $scope.p_SolProvBanco.DescPAis = content.DescPAis;
            $scope.p_SolProvBanco.TipoCuenta = content.TipoCuenta;
            $scope.p_SolProvBanco.DesCuenta = content.DesCuenta;
            $scope.p_SolProvBanco.NumeroCuenta = content.NumeroCuenta;
            $scope.p_SolProvBanco.TitularCuenta = content.TitularCuenta;
            $scope.p_SolProvBanco.ReprCuenta = content.ReprCuenta;
            $scope.p_SolProvBanco.CodSwift = content.CodSwift;
            $scope.p_SolProvBanco.CodBENINT = content.CodBENINT;
            $scope.p_SolProvBanco.CodABA = content.CodABA;
            $scope.p_SolProvBanco.Principal = content.Principal;
            $scope.p_SolProvBanco.Estado = content.Estado;
            $scope.p_SolProvBanco.Provincia = content.Provincia;
            $scope.p_SolProvBanco.DescProvincia = content.DescProvincia;
            $scope.p_SolProvBanco.DirBancoExtranjero = content.DirBancoExtranjero;
            $scope.p_SolProvBanco.BancoExtranjero = content.BancoExtranjero;
            $scope.p_SolProvBanco.FormaPago = content.FormaPago;

            var index = 0;

            for (index = 0; index < $scope.ListBanco.length; index++) {
                if ($scope.ListBanco[index].codBanco === content.CodSapBanco) {
                    $scope.Ingreso.Banco = $scope.ListBanco[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListPais.length; index++) {
                if ($scope.ListPais[index].codigo === content.Pais) {
                    $scope.Ingreso.Paisbanco = $scope.ListPais[index];

                    break;
                }
            }

            for (index = 0; index < $scope.Listbeneficiariobanco.length; index++) {
                if ($scope.Listbeneficiariobanco[index].codigo === content.CodBENINT) {
                    $scope.Ingreso.beneficiariobanco = $scope.Listbeneficiariobanco[index];

                    break;
                }
            }

            $scope.ListRegionbancoTemp = $filter('filter')($scope.ListRegion,
                { descAlterno: $scope.Ingreso.Paisbanco.codigo });

            for (index = 0; index < $scope.ListRegionbancoTemp.length; index++) {
                if ($scope.ListRegionbancoTemp[index].codigo === content.Provincia) {
                    $scope.Ingreso.Regionbanco = $scope.ListRegionbancoTemp[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListTipoCuenta.length; index++) {
                if ($scope.ListTipoCuenta[index].codigo === content.TipoCuenta) {
                    $scope.Ingreso.TipoCuenta = $scope.ListTipoCuenta[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListFormaPago.length; index++) {
                if ($scope.ListFormaPago[index].codigo === content.FormaPago) {
                    $scope.Ingreso.FormaPago = $scope.ListFormaPago[index];

                    break;
                }
            }

            $scope.Grabarbanco = "Modificar";
        }
        else {
            $scope.limpiabanco();
            $scope.cargaregion();
            $scope.Grabarbanco = "Adicionar";
        }

        $('#modal-form-banco').modal('show');
        return false;
    }
    $("#idMensajeOk").click(function () {
        $scope.Aceptar();
    });

    $scope.Aceptar = function () {

        if ($scope.salir == 1) {

            if ($scope.indpage == 'ING') {
                if ($scope.bandera == '0') {

                    $scope.SolProveedor = [];
                    $scope.SolProvContacto = []; 
                    $scope.SolProvBanco = [];
                    $scope.SolProvDireccion = [];
                    $scope.SolDocAdjunto = [];
                    $scope.ViaPago = [];
                    $scope.SolRamo = [];
                    $scope.SolZona = [];
                    $scope.SolProvHistEstado = [];
               			$scope.SolProvContacto = [];
                    $scope.cargasolicitud($scope.IdSolicitud);
                }
                else {
                    window.location = "../Home/Index";
                }
            }

            if ($scope.indpage == 'APR' || $scope.indpage == 'APG' || $scope.indpage == 'APM') {
                window.location = "../Proveedor/frmBandejaSolicitud";
            }
        }

    },
        function (error) {

            $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
            $('#idMensajeError').modal('show');
        };

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
            else if ($scope.pestanaActual == "B") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#bancoproveedor"]').tab('show');
                    return;
                }, 300);
            }
            else if ($scope.pestanaActual == "A") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#adjuntoproveedor"]').tab('show');
                    return;
                }, 300);
            }
            else if ($scope.pestanaActual == "CO") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#datoscompras"]').tab('show');
                    return;
                }, 300);
            }
            else if ($scope.pestanaActual == "I") {
                setTimeout(function () {
                    $('.nav-tabs a[href="#informacionadicional"]').tab('show');
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
        var CrearNuevoUsuario = false;
        $scope.bandera = bandera;

        if (conMensaje == undefined)
            conMensaje = 1;


        $scope.SolProveedor = [];
        $scope.SolProvDireccion = [];

        if ($scope.Ingreso.NomArchivoSgs != '') {
            $scope.p_SolProveedor.Sgs = 'SI'
        }

        if ($scope.Ingreso.GrupoCompra != '' && $scope.Ingreso.GrupoCompra != null) {
            $scope.p_SolProveedor.GrupoCompra = $scope.Ingreso.GrupoCompra.codigo;
        }
        else {
            $scope.p_SolProvDireccion.GrupoCompra = "";
        }

        if ($scope.Ingreso.Pais != '' && $scope.Ingreso.Pais != null) {
            $scope.p_SolProvDireccion.Pais = $scope.Ingreso.Pais.codigo;
        }
        else {
            $scope.p_SolProvDireccion.Pais = "";
        }


        if ($scope.Ingreso.Region != '' && $scope.Ingreso.Region != null) {
            $scope.p_SolProvDireccion.Provincia = $scope.Ingreso.Region.codigo;
        }
        else {
            $scope.p_SolProvDireccion.Provincia = "";
        }

        if ($scope.Ingreso.Ciudad != '' && $scope.Ingreso.Ciudad != null) {
            $scope.p_SolProvDireccion.Ciudad = $scope.Ingreso.Ciudad.codigo;
        }
        else {
            $scope.p_SolProvDireccion.Ciudad = "";
        }

        if ($scope.Ingreso.SectorComercial != '' && $scope.Ingreso.SectorComercial != undefined && $scope.Ingreso.SectorComercial != null) {
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

        if ($scope.Ingreso.tipoActividad != '' && $scope.Ingreso.tipoActividad != null) {
            $scope.p_SolProveedor.TipoActividad = $scope.Ingreso.tipoActividad.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoActividad = "";
        }

        if ($scope.Ingreso.tipoServicio != '' && $scope.Ingreso.GrupoCompra != null) {
            $scope.p_SolProveedor.TipoServicio = $scope.Ingreso.tipoServicio.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoServicio = "";
        }

        if ($scope.Ingreso.ProcesoSoporte != '' && $scope.Ingreso.ProcesoSoporte != null) {
            $scope.p_SolProveedor.ProcesoBrindaSoporte = $scope.Ingreso.ProcesoSoporte.codigo;
            if ($scope.Ingreso.ProcesoSoporte.codigo == 'P0') {
                $scope.p_SolProveedor.EsCritico = "N";
            } else {
                $scope.p_SolProveedor.EsCritico = "S";
            }
        }
        else {
            $scope.p_SolProveedor.ProcesoBrindaSoporte = "";
            $scope.p_SolProveedor.EsCritico = "N";
        }

        if ($scope.Ingreso.TipoCalificacion != '' && $scope.Ingreso.TipoCalificacion != null) {
            $scope.p_SolProveedor.TipoCalificacion = $scope.Ingreso.TipoCalificacion.codigo;
        }
        else {
            $scope.p_SolProveedor.TipoCalificacion = "";
        }

        if ($scope.Ingreso.Puntaje != '' && $scope.Ingreso.Puntaje != null) {
            $scope.p_SolProveedor.Calificacion = $scope.Ingreso.Puntaje.codigo;
        }
        else {
            $scope.p_SolProveedor.Calificacion = "";
        }

        if ($scope.p_SolProveedor.CodGrupoProveedor != '' && $scope.p_SolProveedor.CodGrupoProveedor != null) {
            if ($scope.p_SolProveedor.CodGrupoProveedor.length < 10) {
                $scope.MenjError = "RUC Proveedor Padre es Inconrecto ";
                $('#idMensajeError').modal('show');

                return;
            }
            if ($scope.validorCedulaguradr($scope.p_SolProveedor.CodGrupoProveedor) != true) return true;
        }

        if ($scope.Ingreso.RetencionIva != '' && $scope.Ingreso.RetencionIva != null) {
            $scope.p_SolProveedor.RetencionIva = $scope.Ingreso.RetencionIva.codigo;
        }
        else {
            $scope.p_SolProveedor.RetencionIva = "";
        }

        if ($scope.Ingreso.RetencionIva2 != '' && $scope.Ingreso.RetencionIva2 != null) {
            $scope.p_SolProveedor.RetencionIva2 = $scope.Ingreso.RetencionIva2.codigo;
        }
        else {
            $scope.p_SolProveedor.RetencionIva2 = "";
        }


        if ($scope.Ingreso.RetencionFuente != '' && $scope.Ingreso.RetencionFuente != null) {
            $scope.p_SolProveedor.RetencionFuente = $scope.Ingreso.RetencionFuente.codigo;
        }
        else {
            $scope.p_SolProveedor.RetencionFuente = "";
        }

        if ($scope.Ingreso.RetencionFuente2 != '' && $scope.Ingreso.RetencionFuente2 != null) {
            $scope.p_SolProveedor.RetencionFuente2 = $scope.Ingreso.RetencionFuente2.codigo;
        }
        else {
            $scope.p_SolProveedor.RetencionFuente2 = "";
        }

        if ($scope.Ingreso.CondicionPago != '' && $scope.Ingreso.CondicionPago != null) {
            $scope.p_SolProveedor.CondicionPago = $scope.Ingreso.CondicionPago.codigo;
        }
        else {
            $scope.p_SolProveedor.CondicionPago = "";
        }

        if ($scope.Ingreso.GrupoCuenta != '' && $scope.Ingreso.GrupoCuenta != null) {
            $scope.p_SolProveedor.GrupoCuenta = $scope.Ingreso.GrupoCuenta.codigo;
        }
        else {
            $scope.p_SolProveedor.GrupoCuenta = "";
        }

        if ($scope.Ingreso.DespachaProvincia != '' && $scope.Ingreso.DespachaProvincia != null) {
            $scope.p_SolProveedor.DespachaProvincia = $scope.Ingreso.DespachaProvincia.codigo;
        }
        else {
            $scope.p_SolProveedor.DespachaProvincia = "";
        }


        if ($scope.Ingreso.autorizacion != '' && $scope.Ingreso.autorizacion != null) {
            $scope.p_SolProveedor.Autorizacion = $scope.Ingreso.autorizacion.codigo;
        }
        else {
            $scope.p_SolProveedor.Autorizacion = "";
        }

        if ($scope.Ingreso.GrupoTesoreria != '' && $scope.Ingreso.GrupoTesoreria != null) {
            $scope.p_SolProveedor.GrupoTesoreria = $scope.Ingreso.GrupoTesoreria.codigo;
        }
        else {
            $scope.p_SolProveedor.GrupoTesoreria = "";
        }

        if (bandera != "0") {
            //Validar que se ingrese la informacion de SGS
            if ($scope.Ingreso.TipoCalificacion == "" || $scope.Ingreso.TipoCalificacion == null) {
                $scope.MenjError = "Seleccione un tipo de Calificación de la pestaña Información Adicional";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if ($scope.Ingreso.Puntaje == "" || $scope.Ingreso.Puntaje == null) {
                $scope.MenjError = "Seleccione un puntaje de Calificación SGS de la pestaña Información Adicional";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            if ($scope.Ingreso.ProcesoSoporte == "" || $scope.Ingreso.ProcesoSoporte == null) {
                $scope.MenjError = "Seleccione un proceso de soporte de la pestaña Información Adicional";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }


        if (($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') && $scope.indpage == 'ING') {

            if (($scope.Estado != '' && ($scope.Estado != "AC" || $scope.Estado != "RE")) && $scope.Estado != "PA" && $scope.Estado != "IN") {

            }

            if (bandera == '0') {
                $scope.p_SolProveedor.Estado = 'IN';
            }
            else {

                if ($scope.SolProvContacto != "") {
                    if ($scope.SolProvContacto.length < $scope.MinCantContactoProve) {
                        $scope.MenjError = "Ingrese mínimo   " + $scope.MinCantContactoProve + " Contactos";
                        $('#idMensajeError').modal('show');

                        return;
                    }
                }
                else {
                    $scope.MenjError = "Ingrese mínimo   " + $scope.MinCantContactoProve + " Contactos";
                    $('#idMensajeError').modal('show');

                    return;
                }

                if ($scope.p_SolProvDireccion.Pais != "EC") {

                    if ($scope.SolProvBanco != "" && $scope.SolProvBanco.length > $scope.MaxCantCBanExtrPrv) {
                        $scope.MenjError = "Ingrese Hasta   " + $scope.MaxCantCBanExtrPrv + " Formas de Pago de la pestaña Banco";
                        $('#idMensajeError').modal('show');

                        return;
                    }
                }

                if ($scope.SolDocAdjunto != "") {

                    var index = 0;
                    var indexa = 0;
                    var band = true;
                    for (index = 0; index < $scope.ListDocumentoAdjunto.length; index++) {

                        if ($scope.ListDocumentoAdjunto[index].descAlterno === 'SI') {

                            indexa = 0;
                            band = false;
                            for (indexa = 0; indexa < $scope.SolDocAdjunto.length; indexa++) {
                                if ($scope.ListDocumentoAdjunto[index].codigo == $scope.SolDocAdjunto[indexa].CodDocumento) {
                                    band = true;
                                    break;
                                }
                            }

                            if (band != true) {
                                $scope.MenjError = "Ingrese los Documentos Adjuntos Requeridos de la pestaña Adjunto";
                                $('#idMensajeInformativo').modal('show');

                                return;
                            }
                        }

                    }
                }
                else {
                    $scope.MenjError = "Ingrese los Documentos Adjuntos Requeridos de la pestaña Adjunto";
                    $('#idMensajeInformativo').modal('show');

                    return;
                }


                if ($scope.p_SolProveedor.EsCritico == "N") {
                    $scope.p_SolProveedor.Estado = 'AP';
                } else {
                    $scope.p_SolProveedor.Estado = 'EN';
                }


                //Validación por tipo de calificación
                if ($scope.Ingreso.Puntaje.codigo == "B-" || $scope.Ingreso.Puntaje.codigo == "C" || $scope.Ingreso.Puntaje.codigo == "D") {
                    $scope.p_SolProveedor.Estado = 'DP';

                    $scope.p_SolProvHistEstado.IdSolicitud = $scope.IdSolicitud;
                    $scope.p_SolProvHistEstado.Motivo = "13";
                    $scope.p_SolProvHistEstado.DesMotivo = "Calificación SGS";
                    $scope.p_SolProvHistEstado.Observacion = "Debe mejorar la calificación";

                    $scope.p_SolProvHistEstado.EstadoSolicitud = $scope.p_SolProveedor.Estado
                    $scope.SolProvHistEstado.push($scope.p_SolProvHistEstado);



                }

            }
        }

        if ($scope.SolProvBanco != "") {
            var exisPrincipalBanco = $filter('filter')($scope.SolProvBanco, { Principal: true }, true);
            if (exisPrincipalBanco.length > 1) {
                $scope.MenjError = "Solo puede indicar una forma de pago como principal en la pestaña Banco";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }

        //Validar que se ingrese un contacto como representante legal
        var exisPrincipal = $filter('filter')($scope.SolProvContacto, { RepLegal: true }, true);
        if (exisPrincipal.length == 0) {
            $scope.MenjError = "Indique al menos un contacto como representante legal en la pestaña Contacto";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (exisPrincipal.length > 1) {
            $scope.MenjError = "Solo puede indicar un contacto como representante legal en la pestaña Contacto";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        //Validar que seleccione al menos una linea de negocio como principal
        var valLinNegocioP = $filter('filter')($scope.ListaLineaNegocio, { principal: true }, true);
        if (valLinNegocioP.length == 0) {
            $scope.MenjError = "Seleccione al menos una línea de negocio como principal en la pestaña Datos del Proveedor";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if ($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') {

            if ($scope.indpage == 'APR' || $scope.indpage == 'APG' || $scope.indpage == 'APM') {

                if ($scope.indpage == 'APR') {

                    if ($scope.p_SolProveedor.Estado != "EN" && $scope.p_SolProveedor.Estado != "RC" && $scope.p_SolProveedor.Estado != "DM") {
                        $scope.MenjError = 'Esta solicitud ha sido atendida';
                        $scope.salir = 1;
                        $('#idMensajeOk').modal('show');
                        return;
                    }
                }
                if ($scope.Ingreso.MotivoRechazoProveedor != '') {
                    $scope.p_SolProvHistEstado.Motivo = $scope.Ingreso.MotivoRechazoProveedor.codigo;
                    $scope.p_SolProvHistEstado.DesMotivo = $scope.Ingreso.MotivoRechazoProveedor.detalle;
                }
                $scope.p_SolProvHistEstado.IdSolicitud = $scope.IdSolicitud;
                $scope.p_SolProvHistEstado.EstadoSolicitud = $scope.accion;
                $scope.p_SolProveedor.Estado = $scope.accion;
                $scope.SolProvHistEstado.push($scope.p_SolProvHistEstado);
            }
        }
        $scope.p_SolProveedor.TipoSolicitud = $scope.tiposolicitud;
        $scope.SolProveedor.push($scope.p_SolProveedor);
        $scope.SolProvDireccion.push($scope.p_SolProvDireccion);
        $scope.salir = 1;
        $scope.myPromise = null;
        //Filtrar solo los seleccionados
        var listaLinNegocio = $filter('filter')($scope.ListaLineaNegocio, { chekeado: true }, true);
        var codSapNuevo = "";
        $scope.pasoValidacion = 1;
        $scope.myPromise = SolicitudProveedor.getPostSolicitudList($scope.SolProveedor, $scope.SolProvContacto, $scope.SolProvBanco, $scope.SolProvDireccion, $scope.SolDocAdjunto, $scope.ViaPago, $scope.SolRamo, $scope.SolZona, $scope.SolProvHistEstado, listaLinNegocio).then(function (response) {

            if ($scope.p_SolProveedor.Estado == "AP" || $scope.p_SolProveedor.Estado == "AC") {
                $scope.IdSolicitud = response.data.split("|")[0];
                codSapNuevo = response.data.split("|")[1];
            } else {
                $scope.IdSolicitud = response.data;
            }

            if ($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') {

                if ($scope.indpage == 'ING') {
                    if (bandera == '0') {
                        $scope.MenjError = 'Registro guardado correctamente';
                    }
                    else {
                        $scope.MenjError = 'Su solicitud ha sido enviada a revisión, el resultado se enviará por correo electrónico';
                        if ($scope.p_SolProveedor.Estado == 'DP') {
                            $scope.MenjError = 'Su solicitud ha sido enviada a revisión, revise su correo electrónico para revisar el detalle de su solicitud';

                        }
                        if ($scope.p_SolProveedor.Estado == 'AP') {
                            CrearNuevoUsuario = true;
                            $scope.MenjError = 'Su solicitud ha sido aprobada, revise su correo electrónico para ingresar al portal de proveedores';
                            CrearNuevoUsuario = true;

                        }
                    }
                }

                if ($scope.indpage == 'APR') {

                    if ($scope.accion == "RV") {
                        $scope.MenjError = 'La solicitud ha sido revisada por compras de forma exitosa';
                    }

                    if ($scope.accion == "DP") {
                        $scope.MenjError = 'La solicitud ha sido enviada al proveedor de forma exitosa';
                    }

                    if ($scope.accion == "RE") {
                        $scope.MenjError = 'La solicitud ha sido rechazado por el asistente';
                    }

                }

                if ($scope.indpage == 'APG') {

                    if ($scope.accion == "AC") {
                        $scope.MenjError = 'La solicitud ha sido aprobada';
                    }

                    if ($scope.accion == "RC") {
                        $scope.MenjError = 'La solicitud ha sido rechazada';
                    }
                }

                if ($scope.indpage == 'APM') {

                    if ($scope.accion == "DM") {
                        $scope.MenjError = 'La solicitud ha sido  devuelta';
                    }

                    if ($scope.accion == "AP") {
                        $scope.MenjError = 'La solicitud ha sido aprobada';
                    }
                }
                if (CrearNuevoUsuario) {
                    $scope.usrAdm.pRuc = $scope.p_SolProveedor.Identificacion;
                    $scope.usrAdm.pNombre = $scope.p_SolProveedor.RazonSocial;
                    $scope.usrAdm.pClave = generarPassword(8, 0, '');
                    $scope.usrAdm.pCorreoE = $scope.p_SolProveedor.EMAILCorp;
                    $scope.usrAdm.pTelefono = $scope.p_SolProveedor.TelfFijo;
                    $scope.usrAdm.pCelular = $scope.p_SolProveedor.TelfMovil;
                    $scope.usrAdm.pCodSap = codSapNuevo;
                    $scope.usrAdm.pEstado = "A";
                    $scope.myPromise = SeguridadService.getGrabaUsrAdmin($scope.usrAdm).then(function (results) {
                        if (results.data.success) {

                        } else {
                            $scope.MenjError = 'Su solicitud ha sido aprobada, pero existen problemas al crear el usuario'
                        }
                    },
                        function (error) {
                            $scope.MenjError = 'Su solicitud ha sido aprobada, pero existen problemas al crear el usuario'
                        });
                }


            }
            $scope.salir = 1;
            if (conMensaje == 1)
                $('#idMensajeOk').modal('show');
        },
            function (err) {

                $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                $('#idMensajeError').modal('show');
            });
    }, function (error) {

        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    function generarPassword(largo, numEsp, listaCarEsp) {
        var Resultado = "";
        var CanNum = false;
        var CanLet = false;
        var idx = 0;
        //Cargamos la matriz con números y letras
        var Caracter = [
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"
        ];

        while (Resultado.length < (largo - numEsp)) {
            idx = Math.floor(36 * Math.random());
            Resultado = Resultado + Caracter[idx];
            if ((idx < 10))
                CanNum = true;
            if ((idx > 9))
                CanLet = true;
        }
        if (CanNum == false) {
            Resultado = Resultado.substring(0, Resultado.length - 1);
            idx = Math.floor(10 * Math.random());
            Resultado = Resultado + Caracter[idx];
        }
        if (CanLet == false) {
            Resultado = Resultado.substring(0, Resultado.length - 1);
            idx = Math.floor(26 * Math.random()) + 10;
            Resultado = Resultado + Caracter[idx];
        }

        while (Resultado.length < largo) {
            idx = Math.floor(listaCarEsp.length * Math.random());
            Resultado = Resultado + listaCarEsp.substring(idx, idx + 1);
        }
        if (Resultado.length > 2) {
            Resultado = Resultado.substring(1, Resultado.length) + Resultado.substring(0, 1);
        }
        return Resultado;
    };


    if ($scope.IdSolicitud != '') {

        //Carga de catalogos de País-Region(provincia)-ciudad
        GeneralService.getCatalogo('tbl_Pais').then(function (results) {

            $scope.ListPais = results.data;
            var a = $scope.ListPais;
            for (var index = 0; index < a.length; ++index) {
                if (a[index].codigo === "EC")
                    $scope.Ingreso.Pais = a[index];
            }

            GeneralService.getCatalogo('tbl_Region').then(function (results) {
                $scope.ListRegion = results.data;

                GeneralService.getCatalogo('tbl_Ciudad').then(function (results) {
                    $scope.ListCiudad = results.data;
                    $scope.cargasolicitud($scope.IdSolicitud);

                }, function (error) {
                });

            }, function (error) {
            });

        }, function (error) {

        });
    }

    else {

        $scope.p_SolProveedor.EMAILCorp = authService.authentication.userName;
        if ($scope.tiposolicitud == 'AT') {
        }
    }
}]);
