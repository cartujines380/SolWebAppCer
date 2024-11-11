//Pantalla de Bandeja de Solicitudes
'use strict';
app.controller('ConsSolArticuloController', ['$scope', '$location', '$http', 'ConsSolArticuloService', 'ngAuthSettings', 'localStorageService', '$filter', 'authService', function ($scope, $location, $http, ConsSolArticuloService, ngAuthSettings, localStorageService, $filter, authService) {

    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Mensaje ERROR
    $scope.MenjError = "";

    //Variable de Grid
    $scope.Articulo = []; //Para filtrar y mostrar la observacion
    $scope.GridArticulo = [];
    var _GridArticulo = [];
    $scope.pagesArt = [];
    $scope.pageContentArt = [];

    //DataSources
    $scope.EstadoSolicitudArt = [];
    $scope.TipoSolicitud = [];
    $scope.LineaNegocio = [];

    $scope.motivoRechazo = [];
    $scope.motivoRechazoArt = [];

    //Por cada control con el que interactua el controlador se debe crear una variable para get y set
    $scope.txtCodReferencia = "";
    $scope.txtCodSap = "";
    $scope.txtFechaDesde = new Date();
    $scope.txtFechaHasta = new Date();
    $scope.ddlTipoSolicitud = "";
    $scope.ddlLineaNegocio = "";
    $scope.rdbEstado = "1";
    $scope.rdbCodigo = "1";
    $scope.rdbFecha = "2";
    $scope.rdbTipoSol = "1";
    $scope.rdbLinea = "1";
    $scope.etiTotRegistros = "";
    $scope.etiTotRegistros2 = "";


    //Para multiseleccion de estados
    $scope.EstadoSolicitud = [];
    $scope.EstadosCkeck = {}
    $scope.EstadosCkeck.id = "";


    $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };

    $scope.rolSeleccionado = localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser');
    $scope.rolSeleccionadodes = "";
    if ($scope.rolSeleccionado == "1") {
        $scope.rolSeleccionadodes = "APR";
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "EN";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "EC";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "DG";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "DD";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);
    }
    if ($scope.rolSeleccionado == "2") {
        $scope.rolSeleccionadodes = "APG";
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "RC";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);

    }
    if ($scope.rolSeleccionado == "3") {
        $scope.rolSeleccionadodes = "APM";
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "AG";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);
        $scope.EstadosCkeck = {}
        $scope.EstadosCkeck.id = "ED";
        $scope.EstadoSolicitud.push($scope.EstadosCkeck);

    }

    ConsSolArticuloService.getCatalogo('tbl_EstadoSolArt').then(function (results) {

        $scope.EstadoSolicitudArt = results.data;



    }, function (error) {

    });

    ConsSolArticuloService.getCatalogo('tbl_TipoSolArticulo').then(function (results) {
        $scope.TipoSolicitud = results.data;
    });

    ConsSolArticuloService.getCatalogo('tbl_MotivoRechazo_Art').then(function (results) {
        $scope.motivoRechazo = results.data;
        $scope.motivoRechazoArt = results.data;
    });
    //ConsSolArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
    //     
    //    $scope.LineaNegocio = results.data;
    //});


    ConsSolArticuloService.getSolProvLineaAdminList($scope.rolSeleccionadodes, "", "ART", authService.authentication.userName).then(function (response) {

        if (response.data != null && response.data.length > 0) {

            $scope.ListLinea = response.data;

            for (var i = 0; i < $scope.ListLinea.length; i++) {
                var codLineaAdm = $scope.ListLinea[i].linea;
                var detLineaAdm = $scope.ListLinea[i].descLinea;
                $scope.LineaNegocio.push(
                    { codigo: codLineaAdm, detalle: detLineaAdm });
            }
        }
    },

        function (err) {
            $scope.MenjError = err.error_description;
        });


   

    $scope.CargaConsulta = function () {

        //Para armar arreglo de estados seleccionados
        var chkCodRef = ($scope.rdbCodigo === "2" ? "true" : "false");
        if (chkCodRef === "true" && $scope.txtCodReferencia === "") {
            $scope.MenjError = "Ingrese el número de solicitud.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkCodSap = ($scope.rdbCodigo === "3" ? "true" : "false");
        if (chkCodSap === "true" && $scope.txtCodSap === "") {
            $scope.MenjError = "Ingrese el código SAP.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkFecha = ($scope.rdbFecha === "2" ? "false" : "true");
        //Valida ingreso de fechas
        debugger;
        if (chkFecha === "true" && $scope.txtFechaDesde === "" && $scope.txtFechaHasta === "") {
            $scope.MenjError = "Ingrese el rango de fechas.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkFecha === "true" && $scope.txtFechaDesde == "") {
            $scope.MenjError = "Ingrese fecha desde.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkFecha === "true" && $scope.txtFechaHasta == "") {
            $scope.MenjError = "Ingrese fecha hasta.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida que la fecha desde no sea mayor a la fecha hasta
        if (chkFecha === "true" && ($scope.txtFechaDesde > $scope.txtFechaHasta)) {
            $scope.MenjError = "La fecha desde no puede ser mayor a la fecha hasta." ;
            $('#idMensajeError').modal('show');
            return;
        }
        var chkEstado = ($scope.rdbEstado === "2" ? "false" : "true");
        if (chkEstado === "true" && $scope.EstadoSolicitud.length === 0) {
            $scope.MenjError = "Debe seleccionar por lo menos un estado.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Tipo Solicitud
        var chkTipoSol = ($scope.rdbTipoSol === "1" ? "false" : "true");
        if (chkTipoSol === "true" && $scope.ddlTipoSolicitud === "") {
            $scope.MenjError = "Debe seleccionar un tipo de solicitud.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkTipoSol === "true" && $scope.ddlTipoSolicitud === null) {
            $scope.MenjError = "Debe seleccionar un tipo de solicitud.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Linea Negocio
        var chkLinea = ($scope.rdbLinea === "1" ? "false" : "true");
        if (chkLinea === "true" && $scope.ddlLineaNegocio === "") {
            $scope.MenjError = "Debe seleccionar una línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkLinea === "true" && $scope.ddlLineaNegocio === null) {
            $scope.MenjError = "Debe seleccionar una línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        var index;
        var estados = new Array();
        var a = $scope.EstadoSolicitud;
        for (index = 0; index < a.length; ++index) {
            estados[index] = a[index].id;
        }


        var d_desde = "";
        if ($scope.txtFechaDesde != "")
        { d_desde = $filter('date')($scope.txtFechaDesde, 'dd/MM/yyyy'); }
        var d_hasta = "";
        if ($scope.txtFechaHasta != "")
        { d_hasta = $filter('date')($scope.txtFechaHasta, 'dd/MM/yyyy'); }


        //tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, estado, chkTipoSol, TipoSolicitud, chkLinea, LineaNegocio
        $scope.myPromise = ConsSolArticuloService.getArticulos("1", "0", chkCodRef, $scope.txtCodReferencia, chkCodSap, $scope.txtCodSap, chkFecha, d_desde, d_hasta, chkEstado, estados, chkTipoSol, $scope.ddlTipoSolicitud.codigo, chkLinea, $scope.ddlLineaNegocio.codigo, authService.authentication.userName, $scope.rolSeleccionado, "").then(function (results) {

            if (results.data.success) {

                $scope.Articulo = results.data.root[0];
                $scope.GridArticulo = results.data.root[0];
                $scope.etiTotRegistros = $scope.GridArticulo.length;

                if ($scope.etiTotRegistros == 0) {
                    $scope.MenjError = "No existe resultado para su consulta.";
                    $('#idMensajeInformativo').modal('show');
                    $scope.GridArticulo = [];
                    return;
                }

                //$scope.selectedEstadosb = ['x'];
                //$scope.usrRolSel = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
                //if ($scope.usrRolSel == '1')
                //    $scope.selectedEstadosb = ['ENVIADO', 'EN REVISIÓN ASIST. COMPRAS', 'DEVUELTO GERNT. COMPRAS', 'DEVUELTO DATOS MAESTROS'];
                //if ($scope.usrRolSel == '2')
                //    $scope.selectedEstadosb = ['REVISADO ASIST. COMPRA'];
                //if ($scope.usrRolSel == '3')
                //    $scope.selectedEstadosb = ['APROBADO GERNT. COMPRAS', 'EN REVISIÓN DATOS MAESTROS'];
                //$scope.filterByEstados = function (content) {
                //    return ($scope.selectedEstadosb.indexOf(content.estadoSolicitud) !== -1);
                //};

            }
            else {
                $scope.MenjError = "No existe resultado para su consulta.";
                $('#idMensajeInformativo').modal('show');
                $scope.GridArticulo = [];
                return;
            }
            setTimeout(function () { $('#btnConsulta').focus(); }, 100);
            setTimeout(function () { $('#rbtArtRef').focus(); }, 150);

        }, function (error) {
        });

    }

    //******************************************************************************
    $scope.popuptipouser = function (Id) {

        $scope.IdSol = Id;
        $('#idTipoUsuario').modal('show');
    }
    $scope.verasiscompras = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 1);
        window.location = '../Articulos/frmAdminArticulo';
    }
    $scope.vergercompras = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 2);
        window.location = '../Articulos/frmAdminArticulo';
    }
    $scope.verdatosmaest = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 3);
        window.location = '../Articulos/frmAdminArticulo';
    }
    //******************************************************************************

    $scope.VerObservacion = function (Id) {
        debugger;
        var res = "";
        $scope.ObservacionSol = $filter('filter')($scope.Articulo, { idSolicitud: Id })[0].observacion;
        if ($scope.ObservacionSol != "") {
            res = $scope.ObservacionSol.split("|");
            $scope.ObservacionSol = res[0];
            $scope.MotivoSol = $filter('filter')($scope.motivoRechazo, { codigo: res[1] });
        }
        else {
            $scope.ObservacionSol = "Revisar Solicitud de Artículo";
            $scope.MotivoSol = $filter('filter')($scope.motivoRechazo, { codigo: '12' });
            //$scope.MotivoSol = "Solicitud Nuevo Artículo";
        }

        $('#idVerObservacion').modal('show');
    }

    $scope.Seleccion = function (Id, estado, codSAPpro, tipoSolicitud, idTipoSolicitud) {



        tipoSolicitud = idTipoSolicitud;
        localStorageService.set('IdSolicitud', Id);
        localStorageService.set('EstSolicitud', estado);
        localStorageService.set('CodSAPproveedor', codSAPpro);
        localStorageService.set('tipoSolicitud', tipoSolicitud);
        $scope.usrRolSel = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
        localStorageService.set('tipouser', $scope.usrRolSel);
        if (idTipoSolicitud == "5")
            window.location = '../Articulos/frmAdminArtAden';
        else
            window.location = '../Articulos/frmAdminArticulo';
    }

    $scope.SolicitudOPCION = function (opc) {

        $scope.usrRolSel = opc;
        localStorageService.set('tipouser', $scope.usrRolSel);

        window.location = '../Articulos/frmConsSolArticulo';
    }

    //$scope.CargaConsulta();
}
]);

//Pantalla de Ingreso de Articulos
'use strict';
app.controller('AdmArticuloController', ['$scope', '$location', '$http', 'AdmArticuloService', 'ngAuthSettings', '$filter', 'FileUploader', '$routeParams', 'localStorageService', 'authService', function ($scope, $location, $http, AdmArticuloService, ngAuthSettings, $filter, FileUploader, $routeParams, localStorageService, authService) {


    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Grids
    $scope.grArticulo = [];
    $scope.grArticuloDet = [];
    $scope.grArtMedida = [];
    $scope.grArtCodBarra = [];
    $scope.grArtImagen = [];
    //Administrativo
    $scope.grArtCatalogacion = [];
    $scope.grArtCentros = [];
    $scope.grArtAlmacen = [];
    $scope.grArtIndTipoAlmEnt = [];
    $scope.grArtIndTipoAlmSal = [];
    $scope.grArtIndAreaAlmacen = [];
    $scope.grArtObservacion = [];
    $scope.etiTotRegistros = "";
    //DataSource Combos
    $scope.lineaNegocio = [];
    $scope.marca = [];
    $scope.paisOrigen = [];
    $scope.regionOrigen = [];
    $scope.regionOrigenTemp = [];
    $scope.talla = [];
    $scope.gradoAlcohol = [];
    $scope.color = [];
    $scope.fragancia = [];
    $scope.tipos = [];
    $scope.sabor = [];
    $scope.modelo = [];
    $scope.coleccion = [];
    $scope.temporada = [];
    $scope.catalogoVolumen = [];
    $scope.estacion = [];
    $scope.grupoArticuloTot = [];
    $scope.grupoArticulo = [];
    //De uno a muchos
    $scope.catalogacion = [];
    $scope.centros = [];
    $scope.almacen = [];
    $scope.indTipoAlmEnt = [];
    $scope.indTipoAlmSal = [];
    $scope.indAreaAlmNew = [];
    $scope.indAreaAlmacen = [];
    $scope.canalDistibucion = [];
    $scope.canalDistibucionF = [];

    $scope.unidadMedida = [];
    $scope.tipoEANart = [];
    $scope.tipoUnidadMedida = [];
    $scope.uniMedConvers = [];
    $scope.clasificacionFiscal = [];
    $scope.tipoAlmacenBod = [];
    $scope.ddlVolumen = [];
    //SI/NO Deducible y Retención
    $scope.sn_ded = [];
    $scope.sn_ret = [];
    $scope.SINO = [];
    $scope.CaracteristicasART = [];
    $scope.sn = { codigo: 'S', detalle: 'SI' };
    $scope.SINO.push($scope.sn);
    $scope.sn = { codigo: 'N', detalle: 'NO' };
    $scope.SINO.push($scope.sn);
    //$scope.sn_ded = $scope.SINO;
    //$scope.sn_ret = $scope.SINO;
    $scope.codArticulo = "";
    $scope.idArticuloGen = "";
    //modelo Detalle
    $scope.CabPrincipal = [];
    $scope.idSeleccionado = "";
    //modelo Detalle
    $scope.Det = [];
    $scope.p_Det = {};
    $scope.p_Det.idDetalle = "";
    $scope.p_Det.codReferencia = "";
    $scope.p_Det.marca = "";
    $scope.p_Det.desMarca = "";
    $scope.ddlMarca = "";
    $scope.p_Det.paisOrigen = "";
    $scope.ddlPaisOrigen = "";
    $scope.p_Det.regionOrigen = "";
    $scope.ddlRegionOrigen = "";
    $scope.p_Det.gradoAlcohol = "";
    $scope.ddlGradoAlcohol = "";
    $scope.p_Det.talla = "";
    $scope.ddlTalla = "";
    $scope.p_Det.color = "";
    $scope.ddlColor = "";
    $scope.p_Det.fragancia = "";
    $scope.ddlFragancia = "";
    $scope.p_Det.tipos = "";
    $scope.ddlTipos = "";
    $scope.p_Det.sabor = "";
    $scope.ddlSabor = "";
    $scope.p_Det.modelo = "";
    $scope.ddlModelo = "";
    $scope.p_Det.coleccion = "";
    $scope.ddlColeccion = "";
    $scope.p_Det.temporada = "";
    $scope.ddlTemporada = "";
    $scope.p_Det.estacion = "";
    $scope.ddlEstacion = "";
    $scope.ddlCantidadPedir = "";
    $scope.p_Det.iva = "";
    $scope.ddlIva = "";
    $scope.p_Det.deducible = "";
    $scope.ddlDeducible = "";
    $scope.p_Det.retencion = "";
    $scope.ddlRetencion = "";
    $scope.p_Det.descripcion = "";
    $scope.p_Det.marcaNueva = "";
    $scope.p_Det.viewMarcaNueva = true; //Para activar o inactivar el campo
    $scope.p_Det.tamArticulo = "";
    $scope.p_Det.otroId = "";
    $scope.p_Det.contAlcohol = false;
    $scope.p_Det.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Det.accion = "";
    $scope.p_Det.precioBruto = "";
    $scope.p_Det.descuento1 = "";
    $scope.p_Det.descuento2 = "";
    $scope.p_Det.impVerde = false;
    $scope.verDetVariantes = false;
    $scope.ddlAgrupacion = "";
    //modelo Medida
    $scope.Med = [];
    $scope.p_Med = {};
    $scope.p_Med.idDetalle = "";
    $scope.p_Med.unidadMedida = "";
    $scope.p_Med.desUnidadMedida = "";
    $scope.ddlUnidadMedida = "";
    $scope.p_Med.tipoUnidadMedida = "";
    $scope.p_Med.desTipoUnidadMedida = "";
    $scope.ddlTipoUnidadMedida = "";
    $scope.p_Med.uniMedConvers = "";
    $scope.p_Med.desUniMedConvers = "";
    $scope.ddlUniMedConvers = "";
    $scope.p_Med.factorCon = "";
    $scope.p_Med.pesoNeto = "";
    $scope.p_Med.pesoBruto = "";
    $scope.p_Med.longitud = "";
    $scope.p_Med.ancho = "";
    $scope.p_Med.altura = "";
    $scope.p_Med.volumen = "";
    $scope.p_Med.precioBruto = "";
    $scope.p_Med.descuento1 = "";
    $scope.p_Med.descuento2 = "";
    $scope.p_Med.impVerde = false;
    $scope.p_Med.medBase = false;
    $scope.p_Med.medPedido = false;
    $scope.p_Med.medES = false;
    $scope.p_Med.medVenta = false;
    $scope.p_Med.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Med.accion = "";

    //modelo Codigo Barra
    $scope.CBr = [];
    $scope.p_CBr = {};
    $scope.p_CBr.idDetalle = "";
    $scope.p_CBr.unidadMedida = "";
    $scope.p_CBr.numeroEan = "";
    $scope.p_CBr.tipoEan = "";
    $scope.p_CBr.descripcionEan = "";
    $scope.p_CBr.principal = false;
    $scope.p_CBr.accion = "";

    //modelo Imagenes
    $scope.Ima = [];
    $scope.p_Ima = {};
    $scope.p_Ima.idDetalle = "";
    $scope.p_Ima.path = "";
    $scope.p_Ima.idDocAdjunto = "";
    $scope.p_Ima.nomArchivo = "";
    $scope.p_Ima.accion = "";

    //modelo Compras
    $scope.Com = [];
    $scope.p_Com = {};
    $scope.p_Com.idDetalle = "";
    $scope.p_Com.codSap = "";
    $scope.p_Com.codLegacy = "";
    $scope.p_Com.costoFOB = "";
    $scope.p_Com.validoDesde = "";
    $scope.p_Com.observaciones = "";
    $scope.p_Com.organizacionCompras = "";
    $scope.p_Com.organizacionComprasDes = "";
    $scope.p_Com.frecuenciaEntrega = "";
    $scope.p_Com.tipoMaterial = "";
    $scope.p_Com.tipoMaterialDes = "";
    $scope.p_Com.categoriaMaterial = "";
    $scope.p_Com.categoriaMaterialDes = "";
    $scope.p_Com.grupoArticulo = "";
    $scope.p_Com.seccionArticulo = "";
    $scope.p_Com.surtidoParcial = "";
    $scope.p_Com.materia = "";
    $scope.p_Com.materiaDes = "";
    $scope.p_Com.indPedido = "";
    $scope.p_Com.perfilDistribucion = "";
    $scope.p_Com.grupoCompra = "";
    $scope.p_Com.categoriaValoracion = "";
    $scope.p_Com.tipoAlamcen = "";
    $scope.p_Com.condicionAlmacen = "";
    $scope.p_Com.clListaSurtido = "";
    $scope.p_Com.estatusMaterial = "";
    $scope.p_Com.estatusVenta = "";
    $scope.p_Com.grupoBalanzas = "";
    $scope.p_Com.nacionalImportado = "";
    $scope.p_Com.coleccion = "";
    $scope.p_Com.temporada = "";
    $scope.p_Com.estacion = "";
    $scope.p_Com.cantidadPedir = "";
    //Nuevos
    $scope.p_Com.jerarquiaProd = ""; //Jerarquía de Productos
    $scope.p_Com.susceptBonifEsp = ""; //El material es susceptible de bonificación en especie
    $scope.p_Com.procedimCatalog = ""; //Procedimiento de Catalogación
    $scope.p_Com.caracterPlanNec = ""; //Característica de planificación de necesidades
    $scope.p_Com.fuenteProvision = ""; //Fuente aprovisionamiento

    $scope.p_Com.estado = "";
    $scope.p_Com.motivoRechazo = "";
    /*Pasan de uno a muchos necesitan su propio modelo
    $scope.p_Com.catalogacion = "";
    $scope.p_Com.almacen = "";
    $scope.p_Com.indalmaentrada = "";
    $scope.p_Com.indalmasalida = "";
    $scope.p_Com.indareaalmacen = "";*/
    //Combos
    $scope.ddlOrgCompras = "";
    $scope.ddlFrecEntrega = "";
    $scope.ddlTipoMaterial = "";
    $scope.ddlCatMaterial = "";
    $scope.ddlGrupoArticulo = "";
    $scope.ddlSeccArticulo = "";
    $scope.ddlSurtParcial = "";
    $scope.ddlMateria = "";
    $scope.ddlMateriaDes = "";
    $scope.ddlIndPedido = "";
    $scope.ddlPerfDistribucion = "";
    $scope.ddlGrupoCompras = "";
    $scope.ddlCatValoracion = "";
    $scope.ddlTipoAlmacen = "";
    $scope.ddlTipoAlmacenBod = "";
    $scope.ddlCondAlmacen = "";
    $scope.ddlClListSurtidos = "";
    $scope.ddlEstatusMaterial = "";
    $scope.ddlEstatusVentas = "";
    $scope.ddlGrupoBalanzas = "";
    $scope.ddlNacionalImportado = "";
    $scope.ddlColeccion = "";
    $scope.ddlTemporada = "";
    $scope.ddlEstacion = "";
    $scope.ddlCantidadPedir = "";
    //De uno a muchos
    $scope.ddlCatalogacion = "";
    $scope.ddlCentros = "";
    $scope.ddlAlmacen = "";
    $scope.ddlIndTipoAlmEnt = "";
    $scope.ddlIndTipoAlmSal = "";
    $scope.ddlIndAreaAlmNew = "";
    $scope.ddlIndAreaAlmacen = "";
    $scope.ddlCanalDistibucion = [];
    $scope.ddlCanalDistibucionF = [];

    //modelo Centros
    $scope.Cen = [];
    $scope.p_Cen = {};
    $scope.p_Cen.idDetalle = "";
    $scope.p_Cen.centros = "";
    $scope.p_Cen.desCentros = "";
    $scope.p_Cen.accion = "";
    //modelo Catalogacion
    $scope.Cat = [];
    $scope.p_Cat = {};
    $scope.p_Cat.idDetalle = "";
    $scope.p_Cat.catalogacion = "";
    $scope.p_Cat.desCatalogacion = "";
    $scope.p_Cat.accion = "";
    //modelo Almacen
    $scope.Alm = [];
    $scope.p_Alm = {};
    $scope.p_Alm.idDetalle = "";
    $scope.p_Alm.almacen = "";
    $scope.p_Alm.desAlmacen = "";
    $scope.p_Alm.accion = "";
    //modelo IndTipoAlmEnt
    $scope.Iae = [];
    $scope.p_Iae = {};
    $scope.p_Iae.idDetalle = "";
    $scope.p_Iae.indTipoAlmEnt = "";
    $scope.p_Iae.desIndTipoAlmEnt = "";
    $scope.p_Iae.accion = "";
    //modelo IndTipoAlmSal
    $scope.Ias = [];
    $scope.p_Ias = {};
    $scope.p_Ias.idDetalle = "";
    $scope.p_Ias.indTipoAlmSal = "";
    $scope.p_Ias.desIndTipoAlmSal = "";

    $scope.p_Ias.accion = "";
    //modelo IndAreaAlmacen
    $scope.Iaa = [];
    $scope.p_Iaa = {};
    $scope.p_Iaa.idDetalle = "";
    $scope.p_Iaa.indAreaAlmacen = "";
    $scope.p_Iaa.desIndAreaAlmacen = "";
    $scope.p_Iaa.accion = "";


    //Cabecera
    $scope.ddlLineaNegocio = "";
    $scope.txtIdSolicitud = "";
    $scope.txtEstSolicitud = "";
    $scope.codSapProv = "";
    $scope.txtContRegistros = "";

    //Mensaje Error
    $scope.MsjConfirmacion = "";
    $scope.accion = "";

    //path Imagenes
    $scope.Rutas = [];
    $scope.Ruta = {};
    $scope.Ruta.codReferencia = "";
    $scope.Ruta.path = "";
    //Observacion y cambio de estado
    $scope.TextoBoton = "Grabar";
    $scope.ViewBoton = false;
    $scope.observEstado = "";
    $scope.ViewAsisCompras = true;
    $scope.ViewGernCompras = true;
    $scope.ViewDatosMaestr = true;
    //A nivel de la solicitud
    $scope.ViewMotRechazo = true;
    $scope.motivoRechazo = [];
    $scope.ddlMotivoRechazo = "";
    //A nivel del articulo
    $scope.ViewMotRechazoArt = true;
    $scope.motivoRechazoArt = [];
    $scope.ddlMotivoRechazoArt = "";


    //modelo caracteristicas
    $scope.CaractArt = [];
    $scope.CaractArtTMP = [];
    $scope.p_CaractArt = {};
    $scope.p_CaractArt.idDetalle = "";
    $scope.p_CaractArt.idCaract = "";
    $scope.p_CaractArt.idValor = "";
    $scope.p_CaractArt.idAgrupacion = "";
    $scope.p_CaractArt.accion = "";

    //boolean para inactivar botones, cajas de texto y tabs
    $scope.inactivaBot = false;
    $scope.inactivaTab = true;
    $scope.bloqueaBotInicio = false;
    $scope.isReadOnly = false;
    $scope.bloqueaCodReferencia = true;
    $scope.bloqueaDetalles = false;
    $scope.bloqCamposModif = true;
    $scope.inactivaCodEAN = false;
    $scope.isNewGenerico = false;

    //Lista de elementos eliminados
    $scope.listaDetallesSolEliminados = [];
    $scope.listaMedidadSolEliminados = [];
    $scope.listaCodBarrasSolEliminados = [];
    $scope.listaImagenesSolEliminados = [];
    $scope.listaCatalogosSolEliminados = [];
    $scope.listaCentrosSolEliminados = [];
    $scope.listaAlmacenSolEliminados = [];
    $scope.listaTipoAlmacenESolEliminados = [];
    $scope.listaTipoAlmacenSSolEliminados = [];
    $scope.listaAreaAlmacenSolEliminados = [];

    $scope.validaDatos = false;
    $scope.observEstado = "";
    $scope.EnviaNotificacionSN = true;
    $scope.isAprobado = false;
    $scope.datosAdicionales = [];
    $scope.agrupacion = [];
    $scope.codAgrupacion = {};

    $scope.ingDatCompras = {
        idDetalle: '',
        codLegacyProv: '',
        costoFOB: '',
        observaciones: '',
        jerarquiaProd: '',
        susceptBonifEsp: '',
        procedimCatalog: '',
        caracterPlanNec: '',
        fuenteProvision: '',
        organizacionCompras: '',
        organizacionComprasDes: '',
        frecuenciaEntrega: '',
        tipoMaterial: '',
        tipoMaterialDes: '',
        categoriaMaterial: '',
        categoriaMaterialDes: '',
        grupoArticulo: '',
        seccionArticulo: '',
        surtidoParcial: '',
        materia: '',
        materiaDes: '',
        indPedido: '',
        perfilDistribucion: '',
        grupoCompra: '',
        categoriaValoracion: '',
        tipoAlamcen: '',
        condicionAlmacen: '',
        clListaSurtido: '',
        estatusMaterial: '',
        estatusVenta: '',
        grupoBalanzas: '',
        nacionalImportado: '',
        motivoRechazo: '',
        observacion: '',
        validoDesde: '',
        estado: '',
        accion: ''


    }

    $scope.rolSeleccionado = localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser');

    //Carga Combos

    //Carga Combos
    $scope.myPromise = AdmArticuloService.getConsCaracteristicas("999").then(function (results) {

        if (results.data.success) {
            //var descAux = "";
            $scope.listaCaracteristicas = results.data.root[0];
            for (var idx = 0; idx < $scope.listaCaracteristicas.length; idx++) {
                $scope.codAgrupacion = {};
                $scope.codAgrupacion.codigo = $scope.listaCaracteristicas[idx];
                $scope.codAgrupacion.detalle = $scope.listaCaracteristicas[idx];
                $scope.agrupacion.push($scope.codAgrupacion);
            }

        }
        else {
            $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
            $('#idMensajeError').modal('show');

        }
    }, function (error) {
        $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
        $('#idMensajeError').modal('show');

    });

    $scope.myPromise =
     AdmArticuloService.getCatalogo('tbl_MotivoRechazo_Art').then(function (results) {
         $scope.motivoRechazo = results.data;
         $scope.motivoRechazoArt = results.data;
     });
    ;




    //Marca de articulo 
    $scope.myPromise =
        AdmArticuloService.getCatalogo('tbl_MarcaArticulo').then(function (results) {
            $scope.marca = results.data;
            //Agrega el campo para "Otros"
            $scope.mar = { codigo: '-1', detalle: 'OTROS (Especifique)', descAlterno: '' };
            $scope.marca.push($scope.mar);
        });
    ;
    //Cuando la marca sea OTROS activa 

    $scope.$watch('ddlMarca', function () {
        $scope.p_Det.marcaNueva = "";
        if ($scope.ddlMarca.codigo === "-1") {
            $scope.p_Det.viewMarcaNueva = false;
        } else {
            $scope.p_Det.viewMarcaNueva = true;
        }
    });

    $scope.$watch('p_Det.precioBruto', function () {
        $scope.p_Com.costoFOB = $scope.p_Det.precioBruto
    });
    //Mostrar caracteristicas dependiendo de grupo
    $scope.$watch('ddlAgrupacion', function () {

        $scope.CaracteristicasART = [];

        if ($scope.ddlAgrupacion == null) return;

        $scope.myPromise = AdmArticuloService.getConsCaracteristicas($scope.ddlAgrupacion.codigo).then(function (results) {

            if (results.data.success) {
                $scope.CaracteristicasART = results.data.root[0];
            }
            else {
                $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
                $('#idMensajeError').modal('show');

            }
        }, function (error) {
            $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
            $('#idMensajeError').modal('show');

        });
    });

    //País
    $scope.myPromise =
       AdmArticuloService.getCatalogo('tbl_Pais').then(function (results) {
           $scope.paisOrigen = results.data;
       });
    ;

    //Region

    $scope.myPromise =
        AdmArticuloService.getCatalogo('tbl_Region').then(function (results) {
            $scope.regionOrigenTemp = results.data;
        });
    ;


    $scope.$watch('ddlPaisOrigen', function () {
        $scope.regionOrigen = [];
        if ($scope.ddlPaisOrigen != '' && angular.isUndefined($scope.ddlPaisOrigen) != true) {
            $scope.regionOrigen = $filter('filter')($scope.regionOrigenTemp, { descAlterno: $scope.ddlPaisOrigen.codigo });
        }
    });


    //Alcohol
    $scope.myPromise =
       AdmArticuloService.getCatalogo('tbl_GradoAlcohol').then(function (results) {
           $scope.gradoAlcohol = results.data;
       });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_ClasificacionFiscal').then(function (results) {
        $scope.clasificacionFiscal = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Deducible').then(function (results) {
        $scope.sn_ded = results.data;
    });
    ;
    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_volumen').then(function (results) {
        $scope.catalogoVolumen = results.data;
    });
    ;
    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_IndicadorReten').then(function (results) {
        $scope.sn_ret = results.data;
    });
    ;

    //MEdidas
    $scope.myPromise =
     AdmArticuloService.getCatalogo('tbl_UnidadMedidaArt').then(function (results) {
         $scope.unidadMedida = results.data;
     });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_TipoUnidMedArt').then(function (results) {
        $scope.tipoUnidadMedida = results.data;
    });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_UniMedConverArt').then(function (results) {
        $scope.uniMedConvers = results.data;
    });
    ;


    //EAN
    $scope.myPromise =
       AdmArticuloService.getCatalogo('tbl_TiposEAN').then(function (results) {
           $scope.tipoEANart = results.data;
       });
    ;

    //Compras
    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Orgcompras').then(function (results) {
        $scope.orgCompras = results.data;
    });
    ;

    $scope.myPromise =
   AdmArticuloService.getCatalogo('tbl_tipoMaterial_Art').then(function (results) {
       $scope.tipoMaterial = results.data;
   });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_CatMaterial').then(function (results) {
        $scope.catMaterial = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_GrupoArticulo').then(function (results) {
        
        $scope.grupoArticuloTot = results.data;
    });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Orgcompras').then(function (results) {
        $scope.orgCompras = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_FrecEntrega').then(function (results) {
        $scope.frecEntrega = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_tipoMaterial_Art').then(function (results) {
        $scope.tipoMaterial = results.data;
    });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_CatMaterial').then(function (results) {
        $scope.catMaterial = results.data;
    });
    ;

   

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_SurtParcial').then(function (results) {
        $scope.surtParcial = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Materia_Art').then(function (results) {
        $scope.materia = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_IndPedido').then(function (results) {
        $scope.indPedido = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_GrupoCompras').then(function (results) {
        $scope.grupoCompras = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_CatValoracion').then(function (results) {
        $scope.catValoracion = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Condalmacen').then(function (results) {
        $scope.condAlmacen = results.data;
    });
    ;


    $scope.myPromise =
       AdmArticuloService.getCatalogo('tbl_Cllistsurtidos').then(function (results) {
           $scope.clListSurtidos = results.data;
       });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Estatusmaterial').then(function (results) {
        $scope.estatusMaterial = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Estatusventas').then(function (results) {
        $scope.estatusVentas = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_PerfDistribucion').then(function (results) {
        $scope.perfDistribucion = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Nacional_Importado').then(function (results) {
        $scope.nacionalImportado = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_lgnum').then(function (results) {
        $scope.almacen = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Tipoalmacen').then(function (results) {
        $scope.tipoAlmacen = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_TipoAlmBod').then(function (results) {
        $scope.tipoAlmacenBod = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Indtipoalment').then(function (results) {
        $scope.indTipoAlmEnt = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Indtipoalmsal').then(function (results) {
        $scope.indTipoAlmSal = results.data;
    });
    ;
    $scope.myPromise =
   AdmArticuloService.getCatalogo('tbl_Indareaalmacen').then(function (results) {
       $scope.indAreaAlmNew = results.data;
   });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Grupobalanzas').then(function (results) {
        $scope.grupoBalanzas = results.data;
    });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Canaldistribucion').then(function (results) {
        $scope.canalDistibucion = results.data;
    });
    ;

    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_Catalogacion').then(function (results) {
        $scope.catalogacion = results.data;
    });
    ;

    $scope.myPromise =
   AdmArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
       $scope.coleccion = results.data;
   });
    ;

    $scope.myPromise =
   AdmArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
       $scope.temporada = results.data;
   });
    ;


    $scope.myPromise =
    AdmArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.lineaNegocio = results.data;
    });
    ;

    $scope.calculaVolumen = function () {

        if ($scope.p_Med.longitud == "") $scope.p_Med.longitud = 0;
        if ($scope.p_Med.ancho == "") $scope.p_Med.ancho = 0;
        if ($scope.p_Med.altura == "") $scope.p_Med.altura = 0;
        $scope.p_Med.volumen = $scope.p_Med.longitud * $scope.p_Med.ancho * $scope.p_Med.altura;

    }

    //Administrativo
    //Validar Tipo de EAN
    $scope.$watch('p_CBr.tipoEanCat', function () {
        if ($scope.p_CBr.tipoEanCat != undefined) {
            if ($scope.p_CBr.tipoEanCat.codigo == 'IE') {
                $scope.inactivaCodEAN = true;
                $scope.p_CBr.numeroEan = "9999999999999";
            }
            else {
                $scope.inactivaCodEAN = false;
                $scope.p_CBr.numeroEan = "";
            }

        }
    });

    //Ver detalle de variantes
    $scope.QuitarVariante = function (registro) {
        var resGenerico = 0;
        var varianteModicar = $filter('filter')($scope.grArticuloDet, { idDetalle: registro.idDetalle }, true)[0];
        resGenerico = varianteModicar.codGenerico;
        varianteModicar.codGenerico = 0;
        if (varianteModicar.accion != "I")
            varianteModicar.accion = "U";
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: resGenerico }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;
    }


    //Agregar variante a Generico
    $scope.agregarVariante = function (idPadre) {
        var artiGenericoSel = $filter('filter')($scope.grArticulo, { seleccionaGenerico: true });
        if (artiGenericoSel.length == 0) {
            $scope.MenjError = "Seleccione al menos un artículo.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        var valVariantes = false;
        var codRefval = "";
        for (var idx = 0 ; idx < artiGenericoSel.length; idx++) {
            if (!artiGenericoSel[idx].isVariante) {
                valVariantes = true;
                codRefval = artiGenericoSel[idx].codReferencia;
                break;
            }

        }

        if (valVariantes) {
            $scope.MenjError = "El artículo " + codRefval + " no es variante.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

   
        for (var idx = 0; idx < artiGenericoSel.length; idx++) {
            var update = artiGenericoSel[idx];
            update.codGenerico = idPadre;
            update.seleccionaGenerico = false;
            update.accion = "U";
        }

        var auxgrArticulo = $filter('filter')($scope.grArticulo, { codGenerico: "0" }, true);
        $scope.etiTotRegistros = auxgrArticulo.length;

        $scope.verDetVariantes = true;
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: idPadre }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;




    }
    //Ver detalle de variantes
    $scope.verVariantes = function (registro) {
        $scope.verDetVariantes = true;
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: registro.idDetalle }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;
        $scope.codArticulo = registro.codReferencia;
        $scope.idArticuloGen = registro.idDetalle;

    }

    //Crear artículo generico
    $scope.crearGenerico = function () {
        $scope.isNewGenerico = true;
        //var aux = $scope.grArticulo;
        var artiGenericoSel = $filter('filter')($scope.grArticulo, { seleccionaGenerico: true });
        if (artiGenericoSel.length == 0) {
            $scope.MenjError = "Seleccione al menos un artículo.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        var valVariantes = false;
        var codRefval = "";
        for (var idx = 0 ; idx < artiGenericoSel.length; idx++) {
            if (!artiGenericoSel[idx].isVariante) {
                valVariantes = true;
                codRefval = artiGenericoSel[idx].codReferencia;
                break;
            }

        }

        if (valVariantes) {
            $scope.MenjError = "El artículo " + codRefval + " no es variante.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        $scope.NewArticulo();


    }
    $scope.agregarGenerico = function (idPadre) {


        var artiGenericoSel = $filter('filter')($scope.grArticulo, { seleccionaGenerico: true });
        for (var idx = 0; idx < artiGenericoSel.length; idx++) {
            var update = artiGenericoSel[idx];
            update.codGenerico = idPadre;
            update.seleccionaGenerico = false;
            update.accion = "U";
        }

        var auxgrArticulo = $filter('filter')($scope.grArticulo, { codGenerico: "0" }, true);
        $scope.etiTotRegistros = auxgrArticulo.length;

    }

    //Validacion al realizar ingreso de medida    
    $scope.validarUnidadMedida = function () {

        //Limpia Unidad Medida
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.p_Med.desUniMedConvers = "";
        //$scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = "";
        $scope.p_Med.ancho = "";
        $scope.p_Med.altura = "";
        $scope.p_Med.volumen = "";
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.estado = "";
        $scope.p_Med.impVerde = false;
        $scope.p_Med.medBase = false;
        $scope.p_Med.medPedido = false;
        $scope.p_Med.medES = false;
        $scope.p_Med.medVenta = false;
        //Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
        if ($scope.ddlUnidadMedida != undefined)
            $scope.SeleccionMed($scope.ddlUnidadMedida.codigo);
        //alert("llego");


    }


    //Si no es consulta de una solicitud no puede acceder a la pantalla
    if (localStorageService.get('IdSolicitud') === undefined) {
        $scope.MenjError = 'Consulte una solicitud para acceder a esta pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeError').modal('show');
        //window.location = '/Articulos/frmConsSolArticulo';
    }
    if (localStorageService.get('IdSolicitud') === null) {
        $scope.MenjError = 'Consulte una solicitud para acceder a esta pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeError').modal('show');
    }

    $scope.txtIdSolicitud = (localStorageService.get('IdSolicitud') === null ? "0" : localStorageService.get('IdSolicitud'));
    $scope.codSapProv = (localStorageService.get('CodSAPproveedor') === null ? "0" : localStorageService.get('CodSAPproveedor'));
    $scope.txtEstSolicitud = (localStorageService.get('EstSolicitud') === null ? "0" : localStorageService.get('EstSolicitud'));
    $scope.txtTipoSolicitud = (localStorageService.get('tipoSolicitud') === null ? "0" : localStorageService.get('tipoSolicitud'));
    $scope.tipouser = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
    //Limpia
    localStorageService.remove('IdSolicitud');
    localStorageService.remove('EstSolicitud');
    localStorageService.remove('CodSAPproveedor');
    localStorageService.remove('tipoSolicitud');
    localStorageService.remove('tipouser');
    //Muestra u Oculta paneles de botones

    if ($scope.tipouser != "-1") {
        $scope.ViewAsisCompras = ($scope.tipouser === "1" ? false : true);
        $scope.ViewGernCompras = ($scope.tipouser === "2" ? false : true);
        $scope.ViewDatosMaestr = ($scope.tipouser === "3" ? false : true);
    }

    //Cuando entra a la pagina carga los datos de la cosulta
    $scope.CargaConsulta = function () {


        if ($scope.txtIdSolicitud != "0") {

            //tipo, codigo Solo necesita el tipo y el codigo
            $scope.myPromise = AdmArticuloService.getArticulos("2", $scope.txtIdSolicitud, "", "", "", "", "", "", "", "", "", "", "", "", "", authService.authentication.userName, $scope.rolSeleccionado, "").then(function (results) {

                
                //Linea de Negocio
                var index;
                var linea = {};
                linea = results.data.root[0];
                $scope.datosAdicionales = linea;
                $scope.txtRScocial = linea[0].razonSocial;
                //Validar estados que puede modificar por tipo de usuario
                //Asistente de compras 'EN','EC', 'DG','DD'                
                if (!$scope.ViewAsisCompras) {
                    if (linea[0].estado == 'EN' || linea[0].estado == 'EC' || linea[0].estado == 'DG' || linea[0].estado == 'DD') {
                        $scope.isReadOnly = false;
                        $scope.inactivaBot = false;
                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                }
                //Gerente de compras 'RC'             
                if (!$scope.ViewGernCompras) {
                    if (linea[0].estado == 'RC') {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;
                        $scope.ViewGernCompras = true;

                    }
                }

                //datos maestros  'AG','ED'            
                if (!$scope.ViewDatosMaestr) {
                    if (linea[0].estado == 'AG' || linea[0].estado == 'ED') {
                        $scope.isReadOnly = false;
                        $scope.inactivaBot = false;
                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                }


                $scope.Det = results.data.root[1];
               
                $scope.ListacodLegacy = results.data.root[14];
                $scope.grArticulo = $scope.Det;
                var auxgrArticulo = $filter('filter')($scope.grArticulo, { codGenerico: "0" }, true);
                $scope.etiTotRegistros = auxgrArticulo.length;
                //for (var i = 0 ; i < $scope.grArticulo.length; i++)
                //{ 
                //    if ($scope.grArticulo[i].estado == 'APD')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Aprobado' }
                //    if ($scope.grArticulo[i].estado == 'RED')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Rechazado' }

                //}


                //bloquear linea de negocio
                if ($scope.grArticulo.length > 0) {
                    $scope.bloqueaBotInicio = true;
                }
                $scope.CabPrincipal = results.data.root[0];
                $scope.Med = results.data.root[2];
                $scope.CBr = results.data.root[3];
                $scope.Ima = results.data.root[4];
                $scope.Rutas = results.data.root[5];
                $scope.Com = results.data.root[6];
                $scope.Cat = results.data.root[7];
                $scope.Alm = results.data.root[8];
                $scope.Iae = results.data.root[9];
                $scope.Ias = results.data.root[10];
                $scope.Iaa = results.data.root[11];

                $scope.grArtObservacion = results.data.root[12];
                $scope.Cen = results.data.root[13];
                $scope.CaractArt = results.data.root[15];

                $scope.lineaCargada = linea[0].lineaNegocio;
                var ln = $scope.lineaNegocio;
                for (index = 0; index < ln.length; ++index) {
                    if (ln[index].codigo === linea[0].lineaNegocio)
                    { $scope.ddlLineaNegocio = ln[index]; }
                }
                //var continuar = true
                //var maxContinuar = 900000;
                ////alert(linea[0].lineaNegocio + '-' + $scope.lineaNegocio.length);
                //while (continuar) {
                //    maxContinuar = maxContinuar -1;
                //    var ln = $scope.lineaNegocio;
                //    for (index = 0; index < ln.length; ++index) {
                //        if (ln[index].codigo === linea[0].lineaNegocio)
                //        { $scope.ddlLineaNegocio = ln[index]; continuar = false; }
                //    }
                //    if(maxContinuar == 0 )
                //        continuar = false;
                //}

                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }, function (error) {
            });

        }
    }

    //Carga la unidad de medida seleccionada en el combo para la seccion de codigo de barras
    $scope.$watch('ddlUnidadMedida', function () {

        if ($scope.ddlUnidadMedida != undefined) {
            $scope.p_CBr.unidadMedida = $scope.ddlUnidadMedida.codigo;
            $scope.desnumeroEan = $scope.ddlUnidadMedida.detalle;
        }
        else {
            $scope.p_CBr.unidadMedida = "";
            $scope.desnumeroEan = "";
        }


    });

    //Actualizar Unidades de medida desde Grid
    //BASE
    $scope.actualizaUniMedB = function (uniMed, uniMedBase) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {

            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    update3.uniMedBase = false;
                    //update.uniMedPedido = $scope.p_Med.medPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedBase = false;
            }


            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = $scope.p_Med.medPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //PEDIDO
    $scope.actualizaUniMedP = function (uniMed, uniMedPedido) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    update3.uniMedPedido = false;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedPedido = false;
            }



            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    update.uniMedPedido = uniMedPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //ES
    $scope.actualizaUniMedES = function (uniMed, uniMedES) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    //update3.uniMedPedido = false;
                    update3.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedES = false;
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = uniMedPedido;
                    update.uniMedES = uniMedES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //Venta
    $scope.actualizaUniMedV = function (uniMed, uniMedV) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {

            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    //update3.uniMedPedido = false;
                    //update3.uniMedES = $scope.p_Med.medES;
                    update3.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedVenta = false;
            }



            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = uniMedPedido;
                    //update.uniMedES = uniMedES;
                    update.uniMedVenta = uniMedV;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //Eliminar Catalogacion
    $scope.EliminaCat = function (IdCat, IdCan) {

        for (var i = 0, len = $scope.grArtCatalogacion.length; i < len; i++) {
            if ($scope.grArtCatalogacion[i].catalogacion === IdCat && $scope.grArtCatalogacion[i].canaldistribucion == IdCan) {
                break;

            }
        }


        $scope.grArtCatalogacion.splice(i, 1);
        for (var i = 0, len = $scope.Cat.length; i < len; i++) {
            if ($scope.Cat[i].catalogacion === IdCat && $scope.Cat[i].idDetalle == $scope.p_Det.idDetalle &&
                $scope.Cat[i].canaldistribucion === IdCan && $scope.Cat[i].idDetalle == $scope.p_Det.idDetalle
                ) {
                break;

            }
        }
        $scope.Cat[i].accion = 'D';
        $scope.listaCatalogosSolEliminados[$scope.listaCatalogosSolEliminados.length] = $scope.Cat[i];
        $scope.Cat.splice(i, 1);

    }

    //Eliminar CENTROS
    $scope.EliminaCen = function (Id) {

        for (var i = 0, len = $scope.grArtCentros.length; i < len; i++) {
            if ($scope.grArtCentros[i].centros === Id) {
                break;

            }
        }


        $scope.grArtCentros.splice(i, 1);
        for (var i = 0, len = $scope.Cen.length; i < len; i++) {
            if ($scope.Cen[i].centros === Id && $scope.Cen[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Cen[i].accion = 'D';
        $scope.listaCentrosSolEliminados[$scope.listaCentrosSolEliminados.length] = $scope.Cen[i];
        $scope.Cen.splice(i, 1);

    }

    //Eliminar Almacen
    $scope.EliminaAlm = function (Id) {

        for (var i = 0, len = $scope.grArtAlmacen.length; i < len; i++) {
            if ($scope.grArtAlmacen[i].almacen === Id.almacen &&
                $scope.grArtAlmacen[i].tipAlmacen === Id.tipAlmacen &&
                $scope.grArtAlmacen[i].indAlmacenE === Id.indAlmacenE &&
                $scope.grArtAlmacen[i].indAlmacenS === Id.indAlmacenS &&
                $scope.grArtAlmacen[i].indAreaAlmNew === Id.indAreaAlmNew
                ) {
                break;

            }
        }


        $scope.grArtAlmacen.splice(i, 1);
        for (var i = 0, len = $scope.Alm.length; i < len; i++) {
            if ($scope.Alm[i].almacen === Id.almacen &&
                $scope.Alm[i].tipAlmacen === Id.tipAlmacen &&
                $scope.Alm[i].indAlmacenE === Id.indAlmacenE &&
                $scope.Alm[i].indAlmacenS === Id.indAlmacenS &&
                $scope.Alm[i].indAreaAlmNew === Id.indAreaAlmNew &&
                $scope.Alm[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Alm[i].accion = 'D';
        $scope.listaAlmacenSolEliminados[$scope.listaAlmacenSolEliminados.length] = $scope.Alm[i];
        $scope.Alm.splice(i, 1);

    }

    //Eliminar tipo de Almacen entrada
    $scope.EliminaIae = function (Id) {

        for (var i = 0, len = $scope.grArtIndTipoAlmEnt.length; i < len; i++) {
            if ($scope.grArtIndTipoAlmEnt[i].indTipoAlmEnt === Id) {
                break;

            }
        }


        $scope.grArtIndTipoAlmEnt.splice(i, 1);
        for (var i = 0, len = $scope.Iae.length; i < len; i++) {
            if ($scope.Iae[i].indTipoAlmEnt === Id && $scope.Iae[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Iae[i].accion = 'D';
        $scope.listaTipoAlmacenESolEliminados[$scope.listaTipoAlmacenESolEliminados.length] = $scope.Iae[i];
        $scope.Iae.splice(i, 1);

    }

    //Eliminar tipo de Almacen salida
    $scope.EliminaIas = function (Id) {

        for (var i = 0, len = $scope.grArtIndTipoAlmSal.length; i < len; i++) {
            if ($scope.grArtIndTipoAlmSal[i].indTipoAlmSal === Id) {
                break;

            }
        }


        $scope.grArtIndTipoAlmSal.splice(i, 1);
        for (var i = 0, len = $scope.Ias.length; i < len; i++) {
            if ($scope.Ias[i].indTipoAlmSal === Id && $scope.Ias[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Ias[i].accion = 'D';
        $scope.listaTipoAlmacenSSolEliminados[$scope.listaTipoAlmacenSSolEliminados.length] = $scope.Ias[i];
        $scope.Ias.splice(i, 1);

    }

    //Eliminar tipo de Area Almacen 
    $scope.EliminaIaa = function (Id, Id2) {

        for (var i = 0, len = $scope.grArtIndAreaAlmacen.length; i < len; i++) {
            if ($scope.grArtIndAreaAlmacen[i].indAreaAlmacen === Id &&
                $scope.grArtIndAreaAlmacen[i].grupoBalanzas === Id2
                ) {
                break;

            }
        }


        $scope.grArtIndAreaAlmacen.splice(i, 1);
        for (var i = 0, len = $scope.Iaa.length; i < len; i++) {
            if ($scope.Iaa[i].indAreaAlmacen === Id &&
                $scope.Iaa[i].grupoBalanzas === Id2 &&
                $scope.Iaa[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Iaa[i].accion = 'D';
        $scope.listaAreaAlmacenSolEliminados[$scope.listaAreaAlmacenSolEliminados.length] = $scope.Iaa[i];
        $scope.Iaa.splice(i, 1);
        //Obtener Canales de ditribucion seleccionados
        $scope.CargaCanalesDistribucion();
        //Quitar catalagociones de canal de distribucion
        $scope.QuitarCanalesDistribucion(Id);

    }

    //Quitar Catalogaciones que pertenecen a canal de ditribucion
    $scope.QuitarCanalesDistribucion = function (Id) {
        if ($scope.grArtCatalogacion.length > 0) {

            var contElimCat = true;
            while (contElimCat) {
                contElimCat = false;
                for (var ict = 0 ; ict < $scope.grArtCatalogacion.length; ict++) {
                    if ($scope.grArtCatalogacion[ict].canaldistribucion == Id) {
                        $scope.EliminaCat($scope.grArtCatalogacion[ict].catalogacion, Id);
                        contElimCat = true;
                        break;
                    }


                }

            }


        }
    }

    //Quitar articulo    
    $scope.QuitarArt = function (Id) {

        var existe = false;

        if ($scope.grArticulo.length != 0) {
            for (var i = 0, len = $scope.grArticulo.length; i < len; i++) {
                if ($scope.grArticulo[i].codReferencia === Id) {
                    existe = true;
                    break;
                }
            }
            if (existe) {




                //Quitar catalogacion
                for (var j = 0, len = $scope.Cat.length; j < len; j++) {
                    if ($scope.Cat.length == 0)
                    { break; }
                    if (j < $scope.Cat.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Cat[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Cat[j].accion = 'D';
                        $scope.listaCatalogosSolEliminados[$scope.listaCatalogosSolEliminados.length] = $scope.Cat[j];
                        $scope.Cat.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar centros
                for (var j = 0, len = $scope.Cen.length; j < len; j++) {
                    if ($scope.Cen.length == 0)
                    { break; }
                    if (j < $scope.Cen.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Cen[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Cen[j].accion = 'D';
                        $scope.listaCentrosSolEliminados[$scope.listaCentrosSolEliminados.length] = $scope.Cen[j];
                        $scope.Cen.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacenes
                for (var j = 0, len = $scope.Alm.length; j < len; j++) {
                    if ($scope.Alm.length == 0)
                    { break; }
                    if (j < $scope.Alm.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Alm[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Alm[j].accion = 'D';
                        $scope.listaAlmacenSolEliminados[$scope.listaAlmacenSolEliminados.length] = $scope.Alm[j];
                        $scope.Alm.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen entrada
                for (var j = 0, len = $scope.Iae.length; j < len; j++) {
                    if ($scope.Iae.length == 0)
                    { break; }
                    if (j < $scope.Iae.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Iae[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Iae[j].accion = 'D';
                        $scope.listaTipoAlmacenESolEliminados[$scope.listaTipoAlmacenESolEliminados.length] = $scope.Iae[j];
                        $scope.Iae.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen salida
                for (var j = 0, len = $scope.Ias.length; j < len; j++) {
                    if ($scope.Ias.length == 0)
                    { break; }
                    if (j < $scope.Ias.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Ias[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Ias[j].accion = 'D';
                        $scope.listaTipoAlmacenSSolEliminados[$scope.listaTipoAlmacenSSolEliminados.length] = $scope.Ias[j];
                        $scope.Ias.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen Area
                for (var j = 0, len = $scope.Iaa.length; j < len; j++) {
                    if ($scope.Iaa.length == 0)
                    { break; }
                    if (j < $scope.Iaa.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Iaa[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Iaa[j].accion = 'D';
                        $scope.listaAreaAlmacenSolEliminados[$scope.listaAreaAlmacenSolEliminados.length] = $scope.Iaa[j];
                        $scope.Iaa.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar unidades de medidas
                for (var j = 0, len = $scope.Med.length; j < len; j++) {
                    if ($scope.Med.length == 0)
                    { break; }
                    if (j < $scope.Med.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Med[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Med[j].accion = 'D';
                        $scope.listaMedidadSolEliminados[$scope.listaMedidadSolEliminados.length] = $scope.Med[j];
                        $scope.Med.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar codigo de barras
                for (var j = 0, len = $scope.CBr.length; j < len; j++) {
                    if ($scope.CBr.length == 0)
                    { break; }
                    if (j < $scope.CBr.length)
                        var x;
                    else
                    { break; }
                    if ($scope.CBr[j].idDetalle === $scope.grArticulo[i].idDetalle) {

                        $scope.CBr[j].accion = 'D';
                        $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[j];
                        $scope.CBr.splice(j, 1);
                        j = j - 1;
                    }
                }
                //Quitar adjuntos
                for (var j = 0, len = $scope.Ima.length; j < len; j++) {
                    if ($scope.Ima.length == 0)
                    { break; }
                    if (j < $scope.Ima.length)
                        var x;
                    else
                    { break; }

                    if ($scope.Ima[j].idDetalle === $scope.grArticulo[i].idDetalle) {
                        $scope.Ima[j].accion = 'D';
                        $scope.listaImagenesSolEliminados[$scope.listaImagenesSolEliminados.length] = $scope.Ima[j];
                        $scope.Ima.splice(j, 1);
                        j = j - 1;
                    }

                }

                


                $scope.grArticulo[i].accion = 'D';
                $scope.listaDetallesSolEliminados[$scope.listaDetallesSolEliminados.length] = $scope.grArticulo[i];
                $scope.grArticulo.splice(i, 1);
                $scope.etiTotRegistros = $scope.grArticulo.length;
                $scope.Limpia();
                if ($scope.grArticulo.length > 0) {
                    $scope.bloqueaBotInicio = true;
                }
                else
                    $scope.bloqueaBotInicio = false;
                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }
        }

    }

    $scope.CargaCanalesDistribucion = function () {

        $scope.canalDistibucionF = [];
        if ($scope.grArtIndAreaAlmacen.length > 0) {

            for (var idx = 0 ; idx < $scope.grArtIndAreaAlmacen.length ; idx++) {
                var regCanalDisF = {};
                regCanalDisF.codigo = $scope.grArtIndAreaAlmacen[idx].indAreaAlmacen;
                regCanalDisF.detalle = $scope.grArtIndAreaAlmacen[idx].desIndAreaAlmacen;
                //regCanalDisF.descAlterno = $scope.grArtIndAreaAlmacen[idx].detalle;
                $scope.canalDistibucionF.push(regCanalDisF);
            }
        }
    }

    $scope.agregarCaracteristicas = function () {

        //Agregar caracteristicas

        $scope.CaractArtTMP = [];
        for (var idx = 0; idx < $scope.CaracteristicasART.length; idx++) {

            $scope.p_CaractArt = {};
            $scope.p_CaractArt.idDetalle = $scope.p_Det.idDetalle;
            var idBus = "#selectValorCaract" + $scope.CaracteristicasART[idx].id;
            var valor = $(idBus + " option:selected").html();
            $scope.p_CaractArt.idCaract = $scope.CaracteristicasART[idx].id;
            $scope.p_CaractArt.idAgrupacion = $scope.CaracteristicasART[idx].agrupacion;
            $scope.p_CaractArt.idValor = valor;
            $scope.p_CaractArt.accion = "I";

            $scope.CaractArtTMP.push($scope.p_CaractArt);

        }





        //$(document).ready(function () {
        // Así accedemos al Valor de la opción seleccionada
        //var valor = $("#selectValorCaract1").val();
        //$('#txtValorCarac1').val(valor);
        //alert(valor);
        // Si seleccionamos la opción "Texto 1"
        // nos mostrará por pantalla "1"
        //});
        //var valor2 = $(idBus + " option:selected").val();
        //var valor3 = $("#selectValorCaract1").val();
        //var valor3 = $('select[name=valorCarac]').val();
        //var valor4 = $scope.ddlCaract1;
        //// Obtenemos el valor por el id
        //var porId = document.getElementById(idBus).value;
        //var valor = $(idBus).val();


        //alert(valor);
    }

    $scope.cargarCaracteristicas = function (idDet) {

        for (var u = 0 ; u < $scope.CaracteristicasART.length; u++) {

            var idBus = "selectValorCaract" + $scope.CaracteristicasART[u].id;
            if ($scope.CaracteristicasART[u].lista) {
                var caracSelec;
                caracSelec = $filter('filter')($scope.CaractArt, { idDetalle: idDet, idCaract: $scope.CaracteristicasART[u].id });

                if (caracSelec.length > 0) {

                    var lisValores = $scope.CaracteristicasART[u].listaValor;
                    for (var x = 0; x < lisValores.length; x++) {
                        if (lisValores[x].detalle == caracSelec[0].idValor)
                            break;

                    }

                    document.getElementById(idBus).selectedIndex = x + 1;

                }

            }

        }


    }

    function pruebaTime() {
        $scope.cargarCaracteristicas($scope.idSeleccionado);
    }

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href") // activated tab
        $scope.agregarCaracteristicas();
    });

    $scope.SeleccionArt = function (Id) {

        var a = $scope.grupoArticuloTot;
        $scope.grupoArticulo = [];
        var index;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo.charAt(0) == $scope.ddlLineaNegocio.codigo) {
                //Agrega los grupos por linea de negocio
                //$scope.gra = { codigo: a[index].codigo, detalle: a[index].detalle, descAlterno: '' };
                $scope.grupoArticulo.push(a[index]);
            }
        }

        $scope.isNewGenerico = false;
        $scope.isCreadoSAP = false;
        if ($scope.ddlLineaNegocio.codigo == undefined) {
            var ln = $scope.lineaNegocio;
            for (var index = 0; index < ln.length; ++index) {
                if (ln[index].codigo === $scope.lineaCargada)
                { $scope.ddlLineaNegocio = ln[index]; }
            }
        }

        $scope.bloqueaCodReferencia = true;

        if ($scope.isReadOnly == true) {
            $scope.inactivaTab = true;
            $scope.inactivaBot = true;
        }
        else {
            $scope.inactivaTab = false;
            $scope.inactivaBot = false;

            for (var i = 0, len = $scope.Det.length; i < len; i++) {
                if ($scope.Det[i].idDetalle === Id) {
                    if ($scope.Det[i].estado == "Creado") {
                        $scope.inactivaTab = true;
                        $scope.isCreadoSAP = true;

                    }
                    else {
                        $scope.inactivaTab = false;
                        $scope.isCreadoSAP = false;

                    }

                }
            }

        }




        $scope.Limpia();
        $scope.p_Det.idDetalle = Id;
        //Caracteristicas
        $scope.idSeleccionado = Id;
        //Carga Caracteristicas
        var busCaract = false;
        for (var bus = 0 ; bus < $scope.CaractArt.length; bus++) {
            if ($scope.CaractArt[bus].idDetalle == Id) {
                busCaract = true;
                break;
            }

        }
        if (busCaract) {

            for (var i = 0; i < $scope.agrupacion.length; i++) {
                if ($scope.agrupacion[i].detalle == $scope.CaractArt[bus].idAgrupacion)
                    $scope.ddlAgrupacion = $scope.agrupacion[i];
            }

            $scope.myPromise = AdmArticuloService.getConsCaracteristicas($scope.CaractArt[bus].idAgrupacion).then(function (results) {


                if (results.data.success) {
                    $scope.CaracteristicasART = results.data.root[0];
                }
                else {
                    $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
                    $('#idMensajeError').modal('show');

                }

                setTimeout(pruebaTime, 500);

            }, function (error) {
                $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
                $('#idMensajeError').modal('show');

            });
        }

        //$scope.p_Det.codReferencia = Id;
        //Carga datos y grid filtrados
        for (var i = 0, len = $scope.Det.length; i < len; i++) {
            if ($scope.Det[i].idDetalle === Id) {
                var index;
                if ($scope.Det[i].isVariante && $scope.Det[i].codGenerico != 0)
                    $scope.verDetVariantes = true;
                else
                    $scope.verDetVariantes = false;
                //Cod Sap Articulo
                $scope.p_Com.codSap = $scope.Det[i].codSAPart;

                //marca
                var ma = $scope.marca;
                for (index = 0; index < ma.length; ++index) {
                    if (ma[index].codigo === $scope.Det[i].marca)
                    { $scope.ddlMarca = ma[index]; break; }
                }
                //Pais Origen
                var po = $scope.paisOrigen;
                for (index = 0; index < po.length; ++index) {
                    if (po[index].codigo === $scope.Det[i].paisOrigen)
                    { $scope.ddlPaisOrigen = po[index]; break; }
                }
                //Region Origen

                var ro = $scope.regionOrigenTemp;
                for (index = 0; index < ro.length; ++index) {
                    if (ro[index].codigo === $scope.Det[i].regionOrigen)
                    { $scope.ddlRegionOrigen = ro[index]; break; }
                }
                //Talla Articulo
                var ta = $scope.talla;
                for (index = 0; index < ta.length; ++index) {
                    if (ta[index].codigo === $scope.Det[i].talla)
                    { $scope.ddlTalla = ta[index]; break; }
                }
                //Grado Alcohol
                var ga = $scope.gradoAlcohol;
                for (index = 0; index < ga.length; ++index) {
                    if (ga[index].codigo === $scope.Det[i].gradoAlcohol)
                    { $scope.ddlGradoAlcohol = ga[index]; break; }
                }
                //color
                var co = $scope.color;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Det[i].color)
                    { $scope.ddlColor = co[index]; break; }
                }
                //fragancia
                var fa = $scope.fragancia;
                for (index = 0; index < fa.length; ++index) {
                    if (fa[index].codigo === $scope.Det[i].fragancia)
                    { $scope.ddlFragancia = fa[index]; break; }
                }
                //tipos
                var ti = $scope.tipos;
                for (index = 0; index < ti.length; ++index) {
                    if (ti[index].codigo === $scope.Det[i].tipos)
                    { $scope.ddlTipos = ti[index]; break; }
                }
                //sabor
                var sa = $scope.sabor;
                for (index = 0; index < sa.length; ++index) {
                    if (sa[index].codigo === $scope.Det[i].sabor)
                    { $scope.ddlSabor = sa[index]; break; }
                }
                //modelo
                $scope.ddlModelo = $scope.Det[i].modelo;
                //var mo = $scope.modelo;
                //for (index = 0; index < mo.length; ++index) {
                //    if (mo[index].codigo === $scope.Det[i].modelo)
                //        $scope.ddlModelo = mo[index];
                //}
                //iva
                var cl = $scope.clasificacionFiscal;
                for (index = 0; index < cl.length; ++index) {
                    if (cl[index].codigo === $scope.Det[i].iva)
                    { $scope.ddlIva = cl[index]; break; }
                }
                //deducible
                var te = $scope.sn_ded;
                for (index = 0; index < te.length; ++index) {
                    if (te[index].codigo === $scope.Det[i].deducible)
                    { $scope.ddlDeducible = te[index]; break; }
                }
                //retencion
                var es = $scope.sn_ret;
                for (index = 0; index < es.length; ++index) {
                    if (es[index].codigo === $scope.Det[i].retencion)
                    { $scope.ddlRetencion = es[index]; break; }
                }
                //Coleccion
                var co = $scope.coleccion;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Det[i].coleccion)
                        $scope.ddlColeccion = co[index];
                }

                //Temporada
                var tp = $scope.temporada;
                for (index = 0; index < tp.length; ++index) {
                    if (tp[index].codigo === $scope.Det[i].temporada)
                        $scope.ddlTemporada = tp[index];
                }

                $scope.p_Det.codReferencia = $scope.Det[i].codReferencia;
                $scope.p_Det.idDetalle = $scope.Det[i].idDetalle;
                //$scope.p_Det.codReferencia = Id;
                $scope.p_Det.marcaNueva = $scope.Det[i].marcaNueva;
                $scope.p_Det.descripcion = $scope.Det[i].descripcion;
                $scope.p_Det.tamArticulo = $scope.Det[i].tamArticulo;
                $scope.p_Det.otroId = $scope.Det[i].otroId;
                $scope.p_Det.contAlcohol = $scope.Det[i].contAlcohol;
                $scope.p_Det.precioBruto = $scope.Det[i].precioBruto;
                $scope.p_Det.descuento1 = $scope.Det[i].descuento1;
                $scope.p_Det.descuento2 = $scope.Det[i].descuento2;
                $scope.p_Det.impVerde = $scope.Det[i].impVerde;
                $scope.p_Com.costoFOB = $scope.p_Det.precioBruto;
                $scope.ddlEstacion = $scope.Det[i].estacion;
                $scope.ddlCantidadPedir = $scope.Det[i].cantidadPedir;
            }
        }

        //Carga el Administrativo
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            if ($scope.Com[i].idDetalle === $scope.p_Det.idDetalle) {
                var index;
                //Cod legacy prov               
                //$scope.p_Com.codLegacy = $scope.Com[i].codLegacyProv;
                var clprov = $scope.ListacodLegacy;
                for (index = 0; index < clprov.length; ++index) {
                    if (clprov[index].codigo === $scope.Com[i].codLegacyProv)
                    { $scope.p_Com.codLegacy = clprov[index]; break; }
                }

                //organizacionCompras
                var oc = $scope.orgCompras;
                for (index = 0; index < oc.length; ++index) {
                    if (oc[index].codigo === $scope.Com[i].organizacionCompras)
                    { $scope.ddlOrgCompras = oc[index]; break; }
                }
                //frecuenciaEntrega
                var fe = $scope.frecEntrega;
                for (index = 0; index < fe.length; ++index) {
                    if (fe[index].codigo === $scope.Com[i].frecuenciaEntrega)
                    { $scope.ddlFrecEntrega = fe[index]; break; }
                }
                //tipoMaterial
                var tm = $scope.tipoMaterial;
                for (index = 0; index < tm.length; ++index) {
                    if (tm[index].codigo === $scope.Com[i].tipoMaterial)
                    { $scope.ddlTipoMaterial = tm[index]; break; }
                }
                //categoriaMaterial -- para esta pantalla por defecto sera 00-material individual
                var cm = $scope.catMaterial;
                for (index = 0; index < cm.length; ++index) {
                    if (cm[index].codigo === $scope.Com[i].categoriaMaterial)
                    { $scope.ddlCatMaterial = cm[index]; break; }
                }
                //$scope.ddlCatMaterial = "00";
                //grupoArticulo
                var ga = $scope.grupoArticulo;
                for (index = 0; index < ga.length; ++index) {
                    if (ga[index].codigo === $scope.Com[i].grupoArticulo)
                    { $scope.ddlGrupoArticulo = ga[index]; break; }
                }
                //seccionArticulo
                //var sa = $scope.seccArticulo;
                //for (index = 0; index < sa.length; ++index) {
                //    if (sa[index].codigo === $scope.Com[i].seccionArticulo)
                //        $scope.ddlSeccArticulo = sa[index];
                //}
                //catalogacion
                //var ca = $scope.catalogacion;
                //for (index = 0; index < ca.length; ++index) {
                //    if (ca[index].codigo === $scope.Com[i].catalogacion)
                //        $scope.ddlCatalogacion = ca[index];
                //}
                //surtidoParcial
                var sp = $scope.surtParcial;
                for (index = 0; index < sp.length; ++index) {
                    if (sp[index].codigo === $scope.Com[i].surtidoParcial)
                    { $scope.ddlSurtParcial = sp[index]; break; }
                }
                //materia
                var ma = $scope.materia;
                for (index = 0; index < ma.length; ++index) {
                    if (ma[index].codigo === $scope.Com[i].materia) {
                        $scope.ddlMateria = ma[index];
                        $scope.ddlMateriaDes = ma[index]; break;
                    }
                }
                //indPedido
                var ip = $scope.indPedido;
                for (index = 0; index < ip.length; ++index) {
                    if (ip[index].codigo === $scope.Com[i].indPedido)
                    { $scope.ddlIndPedido = ip[index]; break; }
                }
                //perfilDistribucion
                var pd = $scope.perfDistribucion;
                for (index = 0; index < pd.length; ++index) {
                    if (pd[index].codigo === $scope.Com[i].perfilDistribucion)
                    { $scope.ddlPerfDistribucion = pd[index]; break; }
                }
                //almacen
                //var al = $scope.almacen;
                //for (index = 0; index < al.length; ++index) {
                //    if (al[index].codigo === $scope.Com[i].almacen)
                //        $scope.ddlAlmacen = al[index];
                //}
                //grupoCompra
                var gc = $scope.grupoCompras;
                for (index = 0; index < gc.length; ++index) {
                    if (gc[index].codigo === $scope.Com[i].grupoCompra)
                    { $scope.ddlGrupoCompras = gc[index]; break; }
                }
                //categoriaValoracion
                var cv = $scope.catValoracion;
                for (index = 0; index < cv.length; ++index) {
                    if (cv[index].codigo === $scope.Com[i].categoriaValoracion)
                    { $scope.ddlCatValoracion = cv[index]; break; }
                }
                //tipoAlamcen
                var ta = $scope.tipoAlmacenBod;
                for (index = 0; index < ta.length; ++index) {
                    if (ta[index].codigo === $scope.Com[i].tipoAlamcen)
                    { $scope.ddlTipoAlmacenBod = ta[index]; break; }
                }
                //indalmaentrada
                //var ae = $scope.indTipoAlmEnt;
                //for (index = 0; index < ae.length; ++index) {
                //    if (ae[index].codigo === $scope.Com[i].indalmaentrada)
                //        $scope.ddlIndTipoAlmEnt = ae[index];
                //}
                //indalmasalida
                //var as = $scope.indTipoAlmSal;
                //for (index = 0; index < as.length; ++index) {
                //    if (as[index].codigo === $scope.Com[i].indalmasalida)
                //        $scope.ddlIndTipoAlmSal = as[index];
                //}
                //indareaalmacen
                //var aa = $scope.indAreaAlmacen;
                //for (index = 0; index < aa.length; ++index) {
                //    if (aa[index].codigo === $scope.Com[i].indareaalmacen)
                //        $scope.ddlIndAreaAlmacen = aa[index];
                //}
                //condicionAlmacen
                var co = $scope.condAlmacen;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Com[i].condicionAlmacen)
                    { $scope.ddlCondAlmacen = co[index]; break; }
                }
                //clListaSurtido
                var ls = $scope.clListSurtidos;
                for (index = 0; index < ls.length; ++index) {
                    if (ls[index].codigo === $scope.Com[i].clListaSurtido)
                    { $scope.ddlClListSurtidos = ls[index]; break; }
                }
                //estatusMaterial
                var em = $scope.estatusMaterial;
                for (index = 0; index < em.length; ++index) {
                    if (em[index].codigo === $scope.Com[i].estatusMaterial)
                    { $scope.ddlEstatusMaterial = em[index]; break; }
                }
                //estatusVenta
                var ev = $scope.estatusVentas;
                for (index = 0; index < ev.length; ++index) {
                    if (ev[index].codigo === $scope.Com[i].estatusVenta)
                    { $scope.ddlEstatusVentas = ev[index]; break; }
                }
                //grupoBalanzas
                var gb = $scope.grupoBalanzas;
                for (index = 0; index < gb.length; ++index) {
                    if (gb[index].codigo === $scope.Com[i].grupoBalanzas)
                    { $scope.ddlGrupoBalanzas = gb[index]; break; }
                }
                //Nacional importado

                var ni = $scope.nacionalImportado;
                for (index = 0; index < ni.length; ++index) {
                    if (ni[index].codigo === $scope.Com[i].jerarquiaProd) {
                        { $scope.ddlNacionalImportado = ni[index]; break; }

                    }
                }
                //coleccion
                //var cl = $scope.coleccion;
                //for (index = 0; index < cl.length; ++index) {
                //    if (cl[index].codigo === $scope.Com[i].coleccion)
                //        {$scope.ddlColeccion = cl[index];break; }
                //}
                ////temporada
                //var te = $scope.temporada;
                //for (index = 0; index < te.length; ++index) {
                //    if (te[index].codigo === $scope.Com[i].temporada)
                //       { $scope.ddlTemporada = te[index];break; }
                //}
                //estacion
                //$scope.ddlEstacion = $scope.Com[i].estacion;
                //cantidad
                //$scope.ddlCantidadPedir = $scope.Com[i].cantidadPedir;

                //var et = $scope.estacion;
                //for (index = 0; index < et.length; ++index) {
                //    if (et[index].codigo === $scope.Com[i].estacion)
                //        $scope.ddlEstacion = et[index];
                //}

                $scope.p_Com.idDetalle = $scope.Com[i].idDetalle;
                if ($scope.txtTipoSolicitud != "1") {
                    $scope.bloqueaDetalles = true;
                }
                else {
                    $scope.bloqueaDetalles = false;
                }
                //$scope.p_Com.codSap = $scope.Com[i].codSap;
                //$scope.p_Com.codLegacy = $scope.Com[i].codLegacy;

                //$scope.p_Com.costoFOB = $scope.Com[i].costoFOB;

                $scope.p_Com.validoDesde = new Date($scope.Com[i].validoDesde);
                //$scope.p_Com.validoDesde.setTime($scope.p_Com.validoDesde.getTime() + 1 * 24 * 60 * 60 * 1000);
                $scope.p_Com.observaciones = $scope.Com[i].observaciones;
                //Nuevos
                //$scope.p_Com.jerarquiaProd = "0001";
                $scope.p_Com.susceptBonifEsp = $scope.Com[i].susceptBonifEsp;
                $scope.p_Com.procedimCatalog = $scope.Com[i].procedimCatalog;
                $scope.p_Com.caracterPlanNec = $scope.Com[i].caracterPlanNec;
                $scope.p_Com.fuenteProvision = $scope.Com[i].fuenteProvision;

            }
        }


        //Carga la ruta para nuevas imagenes
        if ($scope.Rutas.length > 0) {
            var direc = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path //obtiene la ruta segun el articulo
            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
            $scope.uploader2.url = Ruta;
        }


        if ($scope.Med.length > 0) {

            $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
            //for (var j = 0; j < $scope.grArtMedida.length; j++) {
            //    $scope.SeleccionMed($scope.grArtMedida[j].unidadMedida);
            //}
        }

        if ($scope.Med.length > 0) {
            $scope.grArtImagen = $filter('filter')($scope.Ima, { idDetalle: $scope.p_Det.idDetalle });
        }

        //Adminsitrativo

        $scope.grArtCatalogacion = $filter('filter')($scope.Cat, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtCentros = $filter('filter')($scope.Cen, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtAlmacen = $filter('filter')($scope.Alm, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmEnt = $filter('filter')($scope.Iae, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmSal = $filter('filter')($scope.Ias, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });
        //Obtener Canales de ditribucion seleccionados
        $scope.CargaCanalesDistribucion();
        $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
    }

    $scope.SeleccionMed = function (Id) {
        //Carga datos
        for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
            if ($scope.grArtMedida[i].unidadMedida === Id) {
                var index;
                //Unidad Medida
                var um = $scope.unidadMedida;
                for (index = 0; index < um.length; ++index) {
                    if (um[index].codigo === $scope.grArtMedida[i].unidadMedida)
                    { $scope.ddlUnidadMedida = um[index]; break; }
                }
                //Tipo Unidad Medida
                var tu = $scope.tipoUnidadMedida;
                for (index = 0; index < tu.length; ++index) {
                    if (tu[index].codigo === $scope.grArtMedida[i].tipoUnidadMedida)
                    { $scope.ddlTipoUnidadMedida = tu[index]; break; }
                }
                //Tipo Unidad Medida
                var mc = $scope.uniMedConvers;
                for (index = 0; index < mc.length; ++index) {
                    if (mc[index].codigo === $scope.grArtMedida[i].uniMedConvers)
                    { $scope.ddlUniMedConvers = mc[index]; break; }
                }
                //Unidad Medida de Volumen
                var mv = $scope.catalogoVolumen;
                for (index = 0; index < mv.length; ++index) {
                    if (mv[index].codigo === $scope.grArtMedida[i].uniMedidaVolumen)
                        $scope.ddlVolumen = mv[index];
                }

                $scope.temp_med = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
                $scope.p_Med.idDetalle = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].idDetalle;
                $scope.p_Med.codReferencia = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].codReferencia;

                $scope.p_Med.factorCon = $scope.grArtMedida[i].factorCon;
                $scope.p_Med.pesoNeto = $scope.grArtMedida[i].pesoNeto;
                $scope.p_Med.pesoBruto = $scope.grArtMedida[i].pesoBruto;
                $scope.p_Med.longitud = $scope.grArtMedida[i].longitud;
                $scope.p_Med.ancho = $scope.grArtMedida[i].ancho;
                $scope.p_Med.altura = $scope.grArtMedida[i].altura;
                $scope.p_Med.volumen = $scope.grArtMedida[i].volumen;
                $scope.p_Med.precioBruto = $scope.grArtMedida[i].precioBruto;
                $scope.p_Med.descuento1 = $scope.grArtMedida[i].descuento1;
                $scope.p_Med.descuento2 = $scope.grArtMedida[i].descuento2;
                $scope.p_Med.impVerde = $scope.grArtMedida[i].impVerde;

                $scope.p_Med.medBase = $scope.grArtMedida[i].uniMedBase;
                $scope.p_Med.medPedido = $scope.grArtMedida[i].uniMedPedido;
                $scope.p_Med.medES = $scope.grArtMedida[i].uniMedES;
                $scope.p_Med.medVenta = $scope.grArtMedida[i].uniMedVenta;
            }
        }
        //Carga grid codigo barra

        if ($scope.p_Med.idDetalle == "")
        { $scope.p_Med.idDetalle = 0; }
        $scope.temp_cbr = $filter('filter')($scope.CBr, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtCodBarra = $filter('filter')($scope.temp_cbr, { unidadMedida: Id });
        //$scope.grArtCodBarra = $scope.temp_cbr;
    }

    //Quitar Medidas
    $scope.QuitarMed = function (Id) {
        //Quitar medidas
        for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
            if ($scope.grArtMedida[i].unidadMedida === Id) {
                break;

            }
        }


        $scope.grArtMedida.splice(i, 1);
        for (var i = 0, len = $scope.Med.length; i < len; i++) {
            if ($scope.Med[i].unidadMedida === Id && $scope.Med[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Med[i].accion = 'D';
        $scope.listaMedidadSolEliminados[$scope.listaMedidadSolEliminados.length] = $scope.Med[i];
        $scope.Med.splice(i, 1);


        //QUitar codigo de barras
        for (var j = 0, len = $scope.CBr.length; j < len; j++) {
            if ($scope.CBr.length == 0)
            { break; }
            if (j < $scope.CBr.length)
                var x;
            else
            { break; }
            if ($scope.CBr[j].idDetalle === $scope.p_Det.idDetalle && $scope.CBr[j].unidadMedida == Id) {
                $scope.CBr[j].accion = 'D';
                $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[j];
                $scope.CBr.splice(j, 1);
                j = j - 1;
            }
        }

        if ($scope.temp_cbr != undefined) {
            //QUitar codigo de barras temporal
            for (var j = 0, len = $scope.temp_cbr.length; j < len; j++) {
                if ($scope.temp_cbr.length == 0)
                { break; }
                if (j < $scope.temp_cbr.length)
                    var x;
                else
                { break; }
                if ($scope.temp_cbr[j].unidadMedida === Id) {
                    $scope.temp_cbr.splice(j, 1);
                    j = j - 1;
                }
            }
        }

        //Limpia Unidad Medida
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.p_Med.desUniMedConvers = "";
        $scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = "";
        $scope.p_Med.ancho = "";
        $scope.p_Med.altura = "";
        $scope.p_Med.volumen = "";
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.estado = "";
        $scope.p_Med.impVerde = false;
        $scope.p_Med.medBase = false;
        $scope.p_Med.medPedido = false;
        $scope.p_Med.medES = false;
        $scope.p_Med.medVenta = false;
        //Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
    }

    $scope.SeleccionCbr = function (Id) {
        //Carga los datos desde el grid

        for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
            if ($scope.grArtCodBarra[i].numeroEan === Id) {
                $scope.p_CBr.numeroEan = $scope.grArtCodBarra[i].numeroEan;
                $scope.p_CBr.tipoEan = $scope.grArtCodBarra[i].tipoEan;
                $scope.p_CBr.descripcionEan = $scope.grArtCodBarra[i].descripcionEan;
                $scope.p_CBr.principal = $scope.grArtCodBarra[i].principal;
            }
        }
    }
    $scope.QuitarCbr = function (Id) {
        //Carga los datos desde el grid

        var unidadMedidaSelec = ""
        for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
            if ($scope.grArtCodBarra[i].numeroEan === Id) {
                unidadMedidaSelec = $scope.grArtCodBarra[i].unidadMedida;
                break;
            }
        }
        $scope.grArtCodBarra.splice(i, 1);

        for (var i = 0, len = $scope.CBr.length; i < len; i++) {
            if ($scope.CBr[i].numeroEan === Id && $scope.CBr[i].idDetalle == $scope.p_Det.idDetalle && $scope.CBr[i].unidadMedida == unidadMedidaSelec) {
                break;
            }
        }
        $scope.CBr[i].accion = 'D';
        $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[i];
        $scope.CBr.splice(i, 1);
        for (var i = 0, len = $scope.temp_cbr.length; i < len; i++) {
            if ($scope.temp_cbr[i].numeroEan === Id) {
                break;
            }
        }
        $scope.temp_cbr.splice(i, 1);



    }

    $scope.SeleccionIma = function (Id) {

        //Obtiene la ruta segun el articulo
        var pathima = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path;
        //Validar cuando sea consulta o cuando sea una nueva solicitud
        $scope.myPromise =
        AdmArticuloService.getBajaTempArchivo(pathima, Id).then(function (results) {
            $scope.imagenurl = results.data;
            $('#idMuestraImagen').modal('show');
        }, function (error) {
        });
        ;
    }

    $scope.QuitarIma = function (Id) {

        //Quitar adjuntos
        for (var j = 0, len = $scope.Ima.length; j < len; j++) {
            if ($scope.Ima[j].idDocAdjunto === Id && $scope.Ima[j].idDetalle == $scope.p_Det.idDetalle) {

                $scope.Ima[j].accion = 'D';
                $scope.listaImagenesSolEliminados[$scope.listaImagenesSolEliminados.length] = $scope.Ima[j];
                $scope.Ima.splice(j, 1);
                break;
            }

        }
        for (var j = 0, len = $scope.grArtImagen.length; j < len; j++) {
            if ($scope.grArtImagen[j].idDocAdjunto === Id && $scope.grArtImagen[j].idDetalle == $scope.p_Det.idDetalle) {
                $scope.grArtImagen.splice(j, 1);
                break;
            }

        }

    }

    //Asistente de Compras
    $scope.ObservGrabar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 1;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
        $scope.EnviaNotificacionSN = false;
    }

    $scope.ObservRevisado = function () {

        var exsiteAprobados = false;
        var exsitePendientes = false;
        for (var i = 0; i < $scope.grArticulo.length; i++) {
            if ($scope.grArticulo[i].estado == 'Pendiente')
            { exsitePendientes = true; break; }
            if ($scope.grArticulo[i].estado == 'Aprobado')
                exsiteAprobados = true;
        }
        if (exsitePendientes == true) {
            $scope.MenjError = "Existe artículo pendiente de  revisión. ";
            $('#idMensajeError').modal('show');
            return;
        }


        if (exsiteAprobados == true) {
            $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud? ";
            $scope.accion = 2;

            $scope.ViewMotRechazo = true;
            $scope.observEstado = "Solicitud Aprobada.";
            $scope.ddlMotivoRechazo = {};
            $scope.ddlMotivoRechazo.codigo = 0;
            $scope.ddlMotivoRechazo.detalle = "";
            $scope.ddlMotivoRechazo.codigo = 10;
            $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
            //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
            $('#idMensajeConfirmacion').modal('show');
            //$scope.grabar();
        }
        else {
            $scope.MenjError = "No hay ningún artículo aprobado para enviar la solicitud a revisión. ";
            $('#idMensajeError').modal('show');
            return;
        }

    }

    $scope.ObservDevolver = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 10;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 10;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    $scope.ObservRechazar = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 10;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea rechazar la solicitud? ";
        $scope.accion = 15;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Gerente de Compras
    $scope.ObservAprobar = function () {

        //$scope.MenjError = "¿Está seguro que desea Aprobar la Solicitud?";
        //$scope.accion = 4;
        //$scope.ViewMotRechazo = true;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 4;
        $scope.ViewMotRechazo = true;
        $scope.ddlMotivoRechazo = {};
        $scope.ddlMotivoRechazo.codigo = 0;
        $scope.ddlMotivoRechazo.detalle = "";
        $scope.ddlMotivoRechazo.codigo = 10;
        $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
        $scope.observEstado = "Solicitud Aprobada.";
        //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
        $('#idMensajeConfirmacion').modal('show');

        //$('#modal-form-observacion').modal('show');
    }

    $scope.ObservDevolverGC = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 5;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 5;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Datos Maestros
    $scope.ObservGrabarDM = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 6;
        $scope.ViewMotRechazo = true;
        $scope.EnviaNotificacionSN = false;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservAprobarDM = function () {
        //$scope.MenjError = "¿Está seguro que desea Aprobar la Solicitud?";
        //$scope.accion = 7;
        //$scope.ViewMotRechazo = true;
        //$('#idMensajeConfirmacion').modal('show');
        var exsiteAprobados = false;
        var exsitePendientes = false;
        var exsiteErrores = false;
        for (var i = 0; i < $scope.grArticulo.length; i++) {
            if ($scope.grArticulo[i].estado == 'Pendiente')
            { exsitePendientes = true; break; }
            if ($scope.grArticulo[i].estado == 'Error')
            { exsiteErrores = true; break; }
            if ($scope.grArticulo[i].estado == 'Aprobado') {
                exsiteAprobados = true;
                if ($scope.grArticulo[i].codSAPart == "") {
                    //$scope.MenjError = "Existen artículos sin código SAP ingresado. ";
                    //$('#idMensajeError').modal('show');
                    //return;

                }
            }

        }
        if (exsitePendientes == true) {
            $scope.MenjError = "Existe artículo pendiente de revisión. ";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if (exsiteErrores == true) {
            $scope.MenjError = "Existe artículo en estado Error, por favor revisar. ";
            $('#idMensajeInformativo').modal('show');
            return;
        }



        if (exsiteAprobados == true) {
            $scope.MenjConfirmacion = "¿Está seguro que desea aprobar la solicitud? ";
            $scope.accion = 7;
            $scope.ViewMotRechazo = true;
            $scope.ddlMotivoRechazo = {};
            $scope.ddlMotivoRechazo.codigo = 0;
            $scope.ddlMotivoRechazo.detalle = "";
            $scope.ddlMotivoRechazo.codigo = 10;
            $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
            $scope.observEstado = "Solicitud Aprobada.";
            //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
            $('#idMensajeConfirmacion').modal('show');
            //$('#modal-form-observacion').modal('show');
        }
        else {
            $scope.MenjError = "No hay ningún artículo aprobado para enviar la solicitud. ";
            $('#idMensajeError').modal('show');
            return;
        }



    }

    $scope.ObservDevolverDM = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 8;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 8;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Muestra el popup para confimar si desea o no grabar la solicitud
    $scope.ConfirmGrabar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 1;
        $('#idMensajeConfirmacion').modal('show');
    }
    //Muestra el popup para confimar si desea quitar o no articulo
    $scope.ConfirmQuitarArticulo = function (id) {
        $scope.MenjConfirmacion = "¿Está seguro que desea quitar articulo?";
        $scope.accion = 999;
        $scope.CodigoArticulo = id;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Muestra el popup para confimar si desea o no enviar la solicitud
    $scope.ConfirmEnviar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud?";
        $scope.accion = 2;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Evento click boton Grabar
    $scope.grabar = function () {
        debugger;
        if ($scope.accion == 998) {
            $scope.AddArticuloAdmin();
            $('#modal-form-observacion_2').modal('hide');
            return;
        }
        if ($scope.accion == 999) {
            $scope.QuitarArt($scope.CodigoArticulo);
            return;
        }
        //Valida tipo de accion
        if ($scope.accion === 3 || $scope.accion === 9) {
            window.location = '../Articulos/frmConsSolArticulo';
        } else {

            //Valida si selecciono la Linea de Negocio

            var ln = $scope.lineaNegocio;
            for (var index = 0; index < ln.length; ++index) {
                if (ln[index].codigo === $scope.lineaCargada)
                { $scope.ddlLineaNegocio = ln[index]; }
            }


            if ($scope.ddlLineaNegocio === "") {
                $scope.MenjError = "Debe seleccionar una línea de negocio.";
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.ddlLineaNegocio === null) {
                $scope.MenjError = "Debe seleccionar una línea de negocio.";
                $('#idMensajeError').modal('show');
                return;
            }

            //Valida que por lo menos haya un articulo ingresado
            if ($scope.grArticulo.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos un artículo.";
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.accion == 2 || $scope.accion == 4 || $scope.accion == 7) {
                $scope.isAprobado = true;
            }

            //Para validar el estado
            var estado = ($scope.accion === 1 || $scope.accion === 6 ? "grabado" : "enviado");
            var idsolicitud = ($scope.txtIdSolicitud === "" ? "0" : $scope.txtIdSolicitud);

            //Arma la cabecera
            $scope.Cab = [];
            $scope.p_Cab = {};
            $scope.p_Cab.codproveedor = authService.authentication.CodSAP;
            $scope.p_Cab.Usuario = authService.authentication.userName;
            $scope.p_Cab.tiposolicitud = $scope.txtTipoSolicitud;
            $scope.p_Cab.lineanegocio = $scope.ddlLineaNegocio.codigo;
            $scope.p_Cab.idsolicitud = idsolicitud;

            $scope.p_Cab.accion = "U";
            $scope.p_Cab.estado = ($scope.accion === 15 ? "RE" : ($scope.accion === 1 ? "EC" : ($scope.accion === 2 ? "RC" : ($scope.accion === 10 ? "DC" : ($scope.accion === 4 ? "AG" : ($scope.accion === 5 ? "DG" : ($scope.accion === 6 ? "ED" : ($scope.accion === 7 ? "AD" : "DD"))))))));
            $scope.p_Cab.observacion = $scope.observEstado;
            $scope.p_Cab.motivoRechazo = $scope.ddlMotivoRechazo.codigo;
            if ($scope.EnviaNotificacionSN)
                $scope.p_Cab.enviaNotificacion = "S";
            else
                $scope.p_Cab.enviaNotificacion = "N";
            if ($scope.isAprobado)
                $scope.p_Cab.enviaAprobar = "S";
            else
                $scope.p_Cab.enviaAprobar = "N";
            $scope.Cab.push($scope.p_Cab);


            //Solo para administrativo
            //$scope.Com = [];
            //$scope.Cat = [];
            //$scope.Alm = [];
            //$scope.Iae = [];
            //$scope.Ias = [];
            //$scope.Iaa = [];

            //Eliminar detalles de articulos
            for (var i = 0 ; i < $scope.listaDetallesSolEliminados.length; i++)
                $scope.Det[$scope.Det.length] = $scope.listaDetallesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaMedidadSolEliminados.length; i++)
                $scope.Med[$scope.Med.length] = $scope.listaMedidadSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCodBarrasSolEliminados.length; i++)
                $scope.CBr[$scope.CBr.length] = $scope.listaCodBarrasSolEliminados[i];
            for (var i = 0 ; i < $scope.listaImagenesSolEliminados.length; i++)
                $scope.Ima[$scope.Ima.length] = $scope.listaImagenesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCatalogosSolEliminados.length; i++)
                $scope.Cat[$scope.Cat.length] = $scope.listaCatalogosSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCentrosSolEliminados.length; i++)
                $scope.Cen[$scope.Cen.length] = $scope.listaCentrosSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAlmacenSolEliminados.length; i++)
                $scope.Alm[$scope.Alm.length] = $scope.listaAlmacenSolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenESolEliminados.length; i++)
                $scope.Iae[$scope.Iae.length] = $scope.listaTipoAlmacenESolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenSSolEliminados.length; i++)
                $scope.Ias[$scope.Ias.length] = $scope.listaTipoAlmacenSSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAreaAlmacenSolEliminados.length; i++)
                $scope.Iaa[$scope.Iaa.length] = $scope.listaAreaAlmacenSolEliminados[i];
            //Lama el metodo

            $scope.Cab[0].codproveedor = $scope.CabPrincipal[0].codProveedor;

            $scope.myPromise =
            AdmArticuloService.getGrabaSolicitud($scope.Cab, $scope.Det, $scope.Med, $scope.CBr, $scope.Ima, $scope.Com, $scope.Cat,
                                                 $scope.Alm, $scope.Iae, $scope.Ias, $scope.Iaa, $scope.Cen, $scope.CaractArt).then(function (response) {

                                                     if (response.data.success) {
                                                         if ($scope.p_Cab.estado == "AD") {

                                                             $scope.MenjError = 'Artículos grabados correctamente en SAP.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeOk').modal('show');
                                                         }

                                                         else {

                                                             $scope.MenjError = 'Se ha ' + estado + ' exitosamente la solicitud.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeOk').modal('show');
                                                         }
                                                     }
                                                     else {
                                                         if ($scope.p_Cab.estado == "AD") {
                                                             $scope.MenjError = 'Error el grabar artículos en SAP.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeError').modal('show');


                                                             return;
                                                         }
                                                         else {
                                                             $scope.MenjError = response.data.mensaje;
                                                             $('#idMensajeError').modal('show');
                                                             return;
                                                         }
                                                     }


                                                 },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
            ;
        }
    }

    //Evento click boton Reporte
    $scope.reporte = function () {


        //Valida que por lo menos haya un articulo ingresado
        if ($scope.grArticulo.length === 0) {
            $scope.MenjError = "Debe ingresar por lo menos un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Arma la cabecera
        $scope.Cab = [];
        $scope.p_Cab = {};
        $scope.p_Cab.codproveedor = authService.authentication.CodSAP;
        $scope.p_Cab.Usuario = authService.authentication.userName;
        $scope.p_Cab.usrSolicitud = $scope.datosAdicionales[0].usuario;
        $scope.p_Cab.tiposolicitud = $scope.datosAdicionales[0].tipoSolicitud;
        $scope.p_Cab.lineanegocio = $scope.ddlLineaNegocio.codigo;
        $scope.p_Cab.idsolicitud = $scope.txtIdSolicitud;
        $scope.p_Cab.ruc = $scope.datosAdicionales[0].ruc;
        $scope.p_Cab.fecSolicitud = $scope.datosAdicionales[0].fecSolicitud;
        $scope.p_Cab.celular = $scope.datosAdicionales[0].celular;
        $scope.p_Cab.correo = $scope.datosAdicionales[0].correo;
        $scope.p_Cab.correoProveedor = $scope.datosAdicionales[0].correoProveedor;
        $scope.p_Cab.personaContacto = $scope.datosAdicionales[0].personaContacto;

        $scope.p_Cab.responsable = $scope.datosAdicionales[0].responsable;
        $scope.p_Cab.aprobador = $scope.datosAdicionales[0].aprobador;
        $scope.p_Cab.departamento = $scope.datosAdicionales[0].departamento;

        $scope.p_Cab.razonSocial = $scope.datosAdicionales[0].razonSocial;
        $scope.p_Cab.telefono = $scope.datosAdicionales[0].telefono;
        $scope.p_Cab.accion = "U";
        //$scope.p_Cab.estado = ($scope.accion === 15 ? "RE" : ($scope.accion === 1 ? "EC" : ($scope.accion === 2 ? "RC" : ($scope.accion === 10 ? "DC" : ($scope.accion === 4 ? "AG" : ($scope.accion === 5 ? "DG" : ($scope.accion === 6 ? "ED" : ($scope.accion === 7 ? "AD" : "DD"))))))));
        $scope.p_Cab.observacion = $scope.observEstado;
        $scope.p_Cab.motivoRechazo = $scope.ddlMotivoRechazo.codigo;
        //if ($scope.EnviaNotificacionSN)
        //    $scope.p_Cab.enviaNotificacion = "S";
        //else
        //    $scope.p_Cab.enviaNotificacion = "N";
        //if ($scope.isAprobado)
        //    $scope.p_Cab.enviaAprobar = "S";
        //else
        //    $scope.p_Cab.enviaAprobar = "N";
        $scope.Cab.push($scope.p_Cab);

        //Eliminar detalles de articulos
        for (var i = 0 ; i < $scope.listaDetallesSolEliminados.length; i++)
            $scope.Det[$scope.Det.length] = $scope.listaDetallesSolEliminados[i];
        for (var i = 0 ; i < $scope.listaMedidadSolEliminados.length; i++)
            $scope.Med[$scope.Med.length] = $scope.listaMedidadSolEliminados[i];
        for (var i = 0 ; i < $scope.listaCodBarrasSolEliminados.length; i++)
            $scope.CBr[$scope.CBr.length] = $scope.listaCodBarrasSolEliminados[i];
        for (var i = 0 ; i < $scope.listaImagenesSolEliminados.length; i++)
            $scope.Ima[$scope.Ima.length] = $scope.listaImagenesSolEliminados[i];
        for (var i = 0 ; i < $scope.listaCatalogosSolEliminados.length; i++)
            $scope.Cat[$scope.Cat.length] = $scope.listaCatalogosSolEliminados[i];
        for (var i = 0 ; i < $scope.listaCentrosSolEliminados.length; i++)
            $scope.Cen[$scope.Cen.length] = $scope.listaCentrosSolEliminados[i];
        for (var i = 0 ; i < $scope.listaAlmacenSolEliminados.length; i++)
            $scope.Alm[$scope.Alm.length] = $scope.listaAlmacenSolEliminados[i];
        for (var i = 0 ; i < $scope.listaTipoAlmacenESolEliminados.length; i++)
            $scope.Iae[$scope.Iae.length] = $scope.listaTipoAlmacenESolEliminados[i];
        for (var i = 0 ; i < $scope.listaTipoAlmacenSSolEliminados.length; i++)
            $scope.Ias[$scope.Ias.length] = $scope.listaTipoAlmacenSSolEliminados[i];
        for (var i = 0 ; i < $scope.listaAreaAlmacenSolEliminados.length; i++)
            $scope.Iaa[$scope.Iaa.length] = $scope.listaAreaAlmacenSolEliminados[i];
        //Lama el metodo

        $scope.Cab[0].codproveedor = $scope.CabPrincipal[0].codProveedor;
        $scope.myPromise =
        AdmArticuloService.getReporteSolicitud($scope.Cab, $scope.Det, $scope.Med, $scope.CBr, $scope.Ima, $scope.Com, $scope.Cat,
                                             $scope.Alm, $scope.Iae, $scope.Ias, $scope.Iaa, $scope.Cen).then(function (response) {
                                                 var file;
                                                 if (response.data != "") {
                                                     file = new Blob([response.data], { type: 'application/xls' });
                                                     saveAs(file, 'Reporte_Solicitud_Artículos' + '.xls');
                                                     //window.open(response.data, '_blank', '');
                                                     //$scope.MenjError = 'Se ha ' + estado + ' exitosamente la Solicitud';
                                                     //$scope.accion = 3;
                                                     //localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                     //$('#idMensajeOk').modal('show');

                                                 }
                                                 else {

                                                     $scope.MenjError = "Error al generar reporte.";
                                                     $('#idMensajeError').modal('show');
                                                     return;
                                                 }


                                             },
         function (err) {
             $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
             $('#idMensajeError').modal('show');
         });
        ;

    }

    $("#btnMensajeOK").click(function () {
        if ($scope.accion == 3)
            $scope.grabar();
    });
    $("#btnMsjError").click(function () {
        if ($scope.accion == 3)
            $scope.grabar();
    });



    $("#idMensajeError").click(function () {
        if ($scope.accion == 9)
            window.location = '../Home/Index';
    });

    //A nivel de articulo
    $scope.AprobarArticulo = function () {
 
        $scope.MenjConfirmacion = "¿Está seguro que desea aprobar el artículo?";
        $scope.accionArt = 1;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.RechazarArticulo = function () {
        $scope.MsjConfirmacionArt = "¿Está seguro que desea rechazar el artículo?";
        $scope.accionArt = 2;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = false;
        $('#modal-form-observacion_2').modal('show');
    }

    $scope.AddCentros = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlCentros === "") {
            $scope.MenjError = "Debe seleccionar un centro.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCentros === undefined) {
            $scope.MenjError = "Debe seleccionar un centro.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtCentros.length != 0) {
            for (var i = 0, len = $scope.grArtCentros.length; i < len; i++) {
                if ($scope.grArtCentros[i].centros === $scope.ddlCentros.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Cen = {};
            $scope.p_Cen.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Cen.centros = $scope.ddlCentros.codigo;
            $scope.p_Cen.desCentros = $scope.ddlCentros.detalle;
            //$scope.p_Cen.perfilDistribucion = $scope.ddlPerfDistribucion.codigo;
            //$scope.p_Cen.desperfilDistribucion = $scope.ddlPerfDistribucion.detalle;
            $scope.p_Cen.accion = "I";
            $scope.Cen.push($scope.p_Cen);
        }

        //Filtra y muestra
        $scope.grArtCentros = $filter('filter')($scope.Cen, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddCatalogacion = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado canal de distribucion
        if ($scope.ddlCanalDistibucionF === null) {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCanalDistibucionF === "") {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCanalDistibucionF === undefined) {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado Catalogacion
        if ($scope.ddlCatalogacion === null) {
            $scope.MenjError = "Debe seleccionar una catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCatalogacion === "") {
            $scope.MenjError = "Debe seleccionar una catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCatalogacion === undefined) {
            $scope.MenjError = "Debe seleccionar una catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtCatalogacion.length != 0) {
            for (var i = 0, len = $scope.grArtCatalogacion.length; i < len; i++) {
                if ($scope.grArtCatalogacion[i].catalogacion === $scope.ddlCatalogacion.codigo &&
                    $scope.grArtCatalogacion[i].canaldistribucion === $scope.ddlCanalDistibucionF.codigo

                    ) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Cat = {};
            $scope.p_Cat.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Cat.catalogacion = $scope.ddlCatalogacion.codigo;
            $scope.p_Cat.desCatalogacion = $scope.ddlCatalogacion.detalle;
            $scope.p_Cat.canaldistribucion = $scope.ddlCanalDistibucionF.codigo;
            $scope.p_Cat.desCanaldistribucion = $scope.ddlCanalDistibucionF.detalle;
            $scope.p_Cat.accion = "I";
            $scope.Cat.push($scope.p_Cat);
        }

        //Filtra y muestra
        $scope.grArtCatalogacion = $filter('filter')($scope.Cat, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddAlmacen = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlAlmacen === "") {
            $scope.MenjError = "Debe seleccionar un almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlAlmacen === undefined || $scope.ddlAlmacen === null) {
            $scope.MenjError = "Debe seleccionar un almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado tipo de almacen
        if ($scope.ddlTipoAlmacen === "") {
            $scope.MenjError = "Debe seleccionar un tipo almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlTipoAlmacen === undefined || $scope.ddlTipoAlmacen === null) {
            $scope.MenjError = "Debe seleccionar un tipo almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado indicador de almacen E
        if ($scope.ddlIndTipoAlmEnt === "") {
            $scope.MenjError = "Debe seleccionar indicador de almacén de entrada.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmEnt === undefined || $scope.ddlIndTipoAlmEnt === null) {
            $scope.MenjError = "Debe seleccionar indicador de almacén de entrada.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado indicador de almacen S
        if ($scope.ddlIndTipoAlmSal === "") {
            $scope.MenjError = "Debe seleccionar indicador de almacén de salida.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmSal === undefined || $scope.ddlIndTipoAlmSal === null) {
            $scope.MenjError = "Debe seleccionar indicador de almacén de salida.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado indicador de area almacen 
        if ($scope.ddlIndAreaAlmNew === "") {
            $scope.MenjError = "Debe seleccionar indicador de área de almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndAreaAlmNew === undefined || $scope.ddlIndAreaAlmNew === null) {
            $scope.MenjError = "Debe seleccionar indicador de área de almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.grArtAlmacen == undefined)
            $scope.grArtAlmacen = [];

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtAlmacen.length != 0) {
            for (var i = 0, len = $scope.grArtAlmacen.length; i < len; i++) {
                if ($scope.grArtAlmacen[i].almacen === $scope.ddlAlmacen.codigo &&
                    $scope.grArtAlmacen[i].tipAlmacen === $scope.ddlTipoAlmacen.codigo &&
                    $scope.grArtAlmacen[i].indAlmacenE === $scope.ddlIndTipoAlmEnt.codigo &&
                    $scope.grArtAlmacen[i].indAlmacenS === $scope.ddlIndTipoAlmSal.codigo &&
                    $scope.grArtAlmacen[i].indAreaAlmNew === $scope.ddlIndAreaAlmNew.codigo
                    ) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe registro.";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Alm = {};
            $scope.p_Alm.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Alm.almacen = $scope.ddlAlmacen.codigo;
            $scope.p_Alm.desAlmacen = $scope.ddlAlmacen.detalle;
            $scope.p_Alm.tipAlmacen = $scope.ddlTipoAlmacen.codigo;
            $scope.p_Alm.destipAlmacen = $scope.ddlTipoAlmacen.detalle;
            $scope.p_Alm.indAlmacenE = $scope.ddlIndTipoAlmEnt.codigo;
            $scope.p_Alm.desindAlmacenE = $scope.ddlIndTipoAlmEnt.detalle;
            $scope.p_Alm.indAlmacenS = $scope.ddlIndTipoAlmSal.codigo;
            $scope.p_Alm.desindAlmacenS = $scope.ddlIndTipoAlmSal.detalle;
            $scope.p_Alm.indAreaAlmNew = $scope.ddlIndAreaAlmNew.codigo;
            $scope.p_Alm.desIndAreaAlmNew = $scope.ddlIndAreaAlmNew.detalle;
            $scope.p_Alm.accion = "I";
            $scope.Alm.push($scope.p_Alm);
        }

        //Filtra y muestra
        $scope.grArtAlmacen = $filter('filter')($scope.Alm, { idDetalle: $scope.p_Det.idDetalle });
        $scope.ddlAlmacen = "";
        $scope.ddlTipoAlmacen = "";
        $scope.ddlIndTipoAlmEnt = "";
        $scope.ddlIndTipoAlmSal = "";
        $scope.ddlIndAreaAlmNew = "";

    }

    $scope.AddIndTipoAlmEnt = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmEnt === "") {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén entrada.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmEnt === undefined) {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén entrada.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndTipoAlmEnt.length != 0) {
            for (var i = 0, len = $scope.grArtIndTipoAlmEnt.length; i < len; i++) {
                if ($scope.grArtIndTipoAlmEnt[i].indTipoAlmEnt === $scope.ddlIndTipoAlmEnt.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Iae = {};
            $scope.p_Iae.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Iae.indTipoAlmEnt = $scope.ddlIndTipoAlmEnt.codigo;
            $scope.p_Iae.desIndTipoAlmEnt = $scope.ddlIndTipoAlmEnt.detalle;
            $scope.p_Iae.accion = "I";
            $scope.Iae.push($scope.p_Iae);
        }

        //Filtra y muestra
        $scope.grArtIndTipoAlmEnt = $filter('filter')($scope.Iae, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddIndTipoAlmSal = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmSal === "") {
            $scope.MenjError = "Debe seleccionar un indicador Tipo almacén salida.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmSal === undefined) {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén salida.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndTipoAlmSal.length != 0) {
            for (var i = 0, len = $scope.grArtIndTipoAlmSal.length; i < len; i++) {
                if ($scope.grArtIndTipoAlmSal[i].indTipoAlmSal === $scope.ddlIndTipoAlmSal.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Ias = {};
            $scope.p_Ias.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Ias.indTipoAlmSal = $scope.ddlIndTipoAlmSal.codigo;
            $scope.p_Ias.desIndTipoAlmSal = $scope.ddlIndTipoAlmSal.detalle;
            $scope.p_Ias.accion = "I";
            $scope.Ias.push($scope.p_Ias);
        }

        //Filtra y muestra
        $scope.grArtIndTipoAlmSal = $filter('filter')($scope.Ias, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddIndAreaAlmacen = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndAreaAlmacen === "") {
            $scope.MenjError = "Debe seleccionar una indicador área almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndAreaAlmacen === undefined) {
            $scope.MenjError = "Debe seleccionar una indicador área almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndAreaAlmacen.length != 0) {
            for (var i = 0, len = $scope.grArtIndAreaAlmacen.length; i < len; i++) {
                if ($scope.grArtIndAreaAlmacen[i].indAreaAlmacen === $scope.ddlIndAreaAlmacen.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Iaa = {};
            $scope.p_Iaa.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Iaa.indAreaAlmacen = $scope.ddlIndAreaAlmacen.codigo;
            $scope.p_Iaa.desIndAreaAlmacen = $scope.ddlIndAreaAlmacen.detalle;
            $scope.p_Iaa.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
            $scope.p_Iaa.desgrupoBalanzas = $scope.ddlGrupoBalanzas.detalle;
            $scope.p_Iaa.accion = "I";
            $scope.Iaa.push($scope.p_Iaa);
        }

        //Filtra y muestra
        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddCanalDistribucion = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlGrupoBalanzas === null) {
            $scope.MenjError = "Debe seleccionar grupo balanzas.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlGrupoBalanzas === "") {
            $scope.MenjError = "Debe seleccionar grupo balanzas.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlGrupoBalanzas === undefined) {
            $scope.MenjError = "Debe seleccionar grupo balanzas.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado
        if ($scope.ddlCanalDistibucion === null) {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCanalDistibucion.codigo === undefined) {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCanalDistibucion === "") {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.ddlCanalDistibucion === undefined) {
            $scope.MenjError = "Debe seleccionar un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }



        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndAreaAlmacen.length != 0) {
            for (var i = 0, len = $scope.grArtIndAreaAlmacen.length; i < len; i++) {
                if ( //$scope.grArtIndAreaAlmacen[i].grupoBalanzas === $scope.ddlGrupoBalanzas.codigo
                    $scope.grArtIndAreaAlmacen[i].indAreaAlmacen === $scope.ddlCanalDistibucion.codigo
                    ) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Iaa = {};
            $scope.p_Iaa.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Iaa.indAreaAlmacen = $scope.ddlCanalDistibucion.codigo;
            $scope.p_Iaa.desIndAreaAlmacen = $scope.ddlCanalDistibucion.detalle;
            $scope.p_Iaa.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
            $scope.p_Iaa.desgrupoBalanzas = $scope.ddlGrupoBalanzas.detalle;
            $scope.p_Iaa.accion = "I";
            $scope.Iaa.push($scope.p_Iaa);
        }

        //Filtra y muestra
        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });
        //Obtener Canales de ditribucion seleccionados
        $scope.CargaCanalesDistribucion();
    }

    $scope.nextCompras = function () {
        $('.nav-tabs a[href="#Compras"]').tab('show');

    }

    $scope.AddArticulo = function () {
        $scope.p_Det.descripcion = $scope.p_Det.descripcion.toUpperCase();
        $scope.validaDatos = false;
        //Validar que se han seleccionado todas las unidades de medida
        var exisUMbase = false;
        var exisUMpedido = false;
        var exisUMes = false;
        var exisUMventa = false;

        for (var k = 0 ; k < $scope.grArtMedida.length ; k++) {


            if ($scope.grArtMedida[k].uniMedBase) exisUMbase = true;
            if ($scope.grArtMedida[k].uniMedPedido) exisUMpedido = true;
            if ($scope.grArtMedida[k].uniMedVenta) exisUMventa = true;
            if ($scope.grArtMedida[k].uniMedES) exisUMes = true;
        }


        if (!exisUMbase) {
            $scope.MenjError = "Debe seleccionar Unidad Base en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMpedido) {
            $scope.MenjError = "Debe seleccionar Unidad Pedido en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMes) {
            $scope.MenjError = "Debe seleccionar Unidad E/S en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMventa) {
            $scope.MenjError = "Debe seleccionar Unidad Venta en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        //Validar Canal de distribucion
        if ($scope.grArtIndAreaAlmacen.length == 0) {
            $scope.MenjError = "Debe ingresar al menos un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Validar que se ingrese valores obligatorios
        if ($scope.p_Det.codReferencia == "") {
            $scope.MenjError = "Debe ingresar código de referencia del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlMarca == "" || $scope.ddlMarca == undefined) {
            $scope.MenjError = "Debe ingresar marca del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descripcion == "") {
            $scope.MenjError = "Debe ingresar descripción del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.tamArticulo == "") {
            $scope.MenjError = "Debe ingresar tamaño o presentación del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.precioBruto == "") {
            $scope.MenjError = "Debe ingresar precio bruto del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.ddlIva == "" || $scope.ddlIva == undefined) {
            $scope.MenjError = "Debe ingresar valor de clasificación fiscal.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlDeducible == "" || $scope.ddlDeducible == undefined) {
            $scope.MenjError = "Debe ingresar Deducible.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlRetencion == "" || $scope.ddlRetencion == undefined) {
            $scope.MenjError = "Debe ingresar retención.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida si ha ingresado una medida
        if ($scope.grArtMedida.length === 0) {
            $scope.MenjError = "Debe ingresar por lo menos una unidad de medida del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        var ingresa = false;
        if ($scope.grArticulo.length != 0) {
            for (var i = 0, len = $scope.grArticulo.length; i < len; i++) {
                if ($scope.grArticulo[i].codReferencia === $scope.p_Det.codReferencia) {
                    edita = true;
                    if ($scope.grArticulo[i].accion == "I") ingresa = true;
                    break;
                }
            }
        }



        //Valida que haya ingresado una nueva marca en caso de que haya seleccionado OTROS
        if ($scope.ddlMarca.codigo === "-1" && $scope.Det.marcaNueva === "") {
            $scope.MenjError = "Ingrese la nueva marca del articulo si seleccionó <OTROS>.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Obtiene y carga los datos de los combos
        if ($scope.ddlMarca != null)
            $scope.p_Det.marca = $scope.ddlMarca.codigo;
        else
            $scope.p_Det.marca = "";
        if ($scope.ddlMarca != null)
            $scope.p_Det.desMarca = $scope.ddlMarca.detalle;
        else
            $scope.p_Det.desMarca = "";
        if ($scope.ddlPaisOrigen != null)
            $scope.p_Det.paisOrigen = $scope.ddlPaisOrigen.codigo;
        else
            $scope.p_Det.paisOrigen = "";
        if ($scope.ddlRegionOrigen != null)
            $scope.p_Det.regionOrigen = $scope.ddlRegionOrigen.codigo;
        else
            $scope.p_Det.regionOrigen = "";
        if ($scope.ddlTalla != null)
            $scope.p_Det.talla = $scope.ddlTalla.codigo;
        else
            $scope.p_Det.talla = "";
        if ($scope.ddlGradoAlcohol != null)
            $scope.p_Det.gradoAlcohol = $scope.ddlGradoAlcohol.codigo;
        else
            $scope.p_Det.gradoAlcohol = "";
        if ($scope.ddlColor != null)
            $scope.p_Det.color = $scope.ddlColor.codigo;
        else
            $scope.p_Det.color = "";
        if ($scope.ddlFragancia != null)
            $scope.p_Det.fragancia = $scope.ddlFragancia.codigo;
        else
            $scope.p_Det.fragancia = "";
        if ($scope.ddlTipos != null)
            $scope.p_Det.tipos = $scope.ddlTipos.codigo;
        else
            $scope.p_Det.tipos = "";
        if ($scope.ddlSabor != null)
            $scope.p_Det.sabor = $scope.ddlSabor.codigo;
        else
            $scope.p_Det.sabor = "";
        $scope.p_Det.modelo = $scope.ddlModelo;
        //if ($scope.ddlModelo != null)
        //    $scope.p_Det.modelo = $scope.ddlModelo.codigo;
        //else
        //    $scope.p_Det.modelo = "";
        if ($scope.ddlColeccion != null)
            $scope.p_Det.coleccion = $scope.ddlColeccion.codigo;
        else
            $scope.p_Det.coleccion = "";
        if ($scope.ddlTemporada != null)
            $scope.p_Det.temporada = $scope.ddlTemporada.codigo;
        else
            $scope.p_Det.temporada = "";
        $scope.p_Det.estacion = $scope.ddlEstacion;
        $scope.p_Det.cantidadPedir = $scope.ddlCantidadPedir;

        if ($scope.ddlIva != null)
            $scope.p_Det.iva = $scope.ddlIva.codigo;
        else
            $scope.p_Det.iva = "";
        if ($scope.ddlDeducible != null)
            $scope.p_Det.deducible = $scope.ddlDeducible.codigo;
        else
            $scope.p_Det.deducible = "";
        if ($scope.ddlRetencion != null)
            $scope.p_Det.retencion = $scope.ddlRetencion.codigo;
        else
            $scope.p_Det.retencion = "";


        $scope.p_Det.accion = "I";
        $scope.p_Det.estado = "NAR";
        $scope.p_Det.isVariante = false;
        if ($scope.isNewGenerico)
            $scope.p_Det.isGenerico = true;
        else
            $scope.p_Det.isGenerico = false;
        $scope.p_Det.codGenerico = "0";
        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.Det.length; i < len; i++) {
                var update = $scope.Det[i];
                if (update.codReferencia === $scope.p_Det.codReferencia) {
                    update.marca = $scope.p_Det.marca;
                    update.DesMarca = $scope.p_Det.desMarca;
                    update.paisOrigen = $scope.p_Det.paisOrigen;
                    update.regionOrigen = $scope.p_Det.regionOrigen;
                    update.tamArticulo = $scope.p_Det.tamArticulo;
                    update.gradoAlcohol = $scope.p_Det.gradoAlcohol;
                    update.talla = $scope.p_Det.talla;
                    update.color = $scope.p_Det.color;
                    update.fragancia = $scope.p_Det.fragancia;
                    update.tipos = $scope.p_Det.tipos;
                    update.sabor = $scope.p_Det.sabor;
                    update.modelo = $scope.p_Det.modelo;
                    update.precioBruto = $scope.p_Det.precioBruto;
                    update.descuento1 = $scope.p_Det.descuento1;
                    update.descuento2 = $scope.p_Det.descuento2;
                    update.impVerde = $scope.p_Det.impVerde;
                    update.coleccion = $scope.p_Det.coleccion;
                    update.temporada = $scope.p_Det.temporada;
                    update.estacion = $scope.p_Det.estacion;
                    update.cantidadPedir = $scope.p_Det.cantidadPedir;
                    update.iva = $scope.p_Det.iva;
                    update.deducible = $scope.p_Det.deducible;
                    update.retencion = $scope.p_Det.retencion;
                    update.descripcion = $scope.p_Det.descripcion;
                    update.codSAPart = $scope.p_Com.codSap;
                    update.otroId = $scope.p_Det.otroId;
                    update.observacion = $scope.observEstado;
                    update.contAlcohol = $scope.p_Det.contAlcohol;

                    //Validar si es material variante
                    if ($scope.ddlCatMaterial.codigo == "02") {
                        update.isVariante = true;
                    }
                    else {
                        update.isVariante = false;
                    }


                    if (ingresa)
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "I");
                    else
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Agrega nuevo articulo
            $scope.Det.push($scope.p_Det);
        }

        //Agrega el articulo y carga el grid
        $scope.grArticulo = $scope.Det;
        if ($scope.grArticulo.length > 0) {
            $scope.bloqueaBotInicio = true;
        }
        //$scope.Limpia();
        $scope.inactivaTab = true;
        $scope.validaDatos = true;

        //Eliminar caracteristicas 
        var seguir = true;
        while (seguir) {
            seguir = false;
            for (var ini = 0 ; ini < $scope.CaractArt.length; ini++) {
                if ($scope.CaractArt[ini].idDetalle == $scope.p_Det.idDetalle) {
                    seguir = true;
                    break;
                }
            }
            $scope.CaractArt.splice(ini, 1);
        }

        for (var ini = 0 ; ini < $scope.CaractArtTMP.length; ini++) {
            $scope.p_CaractArt = {};
            $scope.p_CaractArt.idDetalle = $scope.CaractArtTMP[ini].idDetalle;
            $scope.p_CaractArt.idCaract = $scope.CaractArtTMP[ini].idCaract;
            $scope.p_CaractArt.idValor = $scope.CaractArtTMP[ini].idValor;
            $scope.p_CaractArt.accion = $scope.CaractArtTMP[ini].accion;
            $scope.p_CaractArt.idAgrupacion = $scope.CaractArtTMP[ini].idAgrupacion;

            $scope.CaractArt.push($scope.p_CaractArt);
        }
        $scope.CaractArtTMP = [];

    }

    $scope.AddArticuloAdmin = function () {

        $scope.AddArticulo();
        if (!$scope.validaDatos) return;
        //Carga los datos de Compras        
        $scope.p_Com.idDetalle = $scope.p_Det.idDetalle;
        //Modifica los datos de compras
        var esIngreso = true;
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            var update = $scope.Com[i];
            if (update.idDetalle === $scope.p_Com.idDetalle) {
                esIngreso = false;
                //update.codLegacyProv = $scope.p_Com.codLegacy.codigo;
                if ($scope.p_Com.codLegacy != null)
                    update.codLegacyProv = $scope.p_Com.codLegacy.codigo;
                else
                    update.codLegacyProv = "";
                update.costoFOB = $scope.p_Com.costoFOB;
                update.observaciones = $scope.p_Com.observaciones;
                //Nuevos
                //update.jerarquiaProd = $scope.p_Com.jerarquiaProd;
                if ($scope.ddlNacionalImportado != null)
                    update.jerarquiaProd = $scope.ddlNacionalImportado.codigo;
                else
                    update.jerarquiaProd = "";
                update.susceptBonifEsp = $scope.p_Com.susceptBonifEsp;
                update.procedimCatalog = $scope.p_Com.procedimCatalog;
                update.caracterPlanNec = $scope.p_Com.caracterPlanNec;
                update.fuenteProvision = $scope.p_Com.fuenteProvision;
                if ($scope.ddlOrgCompras != null) {
                    update.organizacionCompras = $scope.ddlOrgCompras.codigo;
                    update.organizacionComprasDes = $scope.ddlOrgCompras.detalle;
                }
                else {
                    update.organizacionCompras = "";
                    update.organizacionComprasDes = "";
                }
                if ($scope.ddlFrecEntrega != null)
                    update.frecuenciaEntrega = $scope.ddlFrecEntrega.codigo;
                else
                    update.frecuenciaEntrega = "";
                if ($scope.ddlTipoMaterial != null) {
                    update.tipoMaterial = $scope.ddlTipoMaterial.codigo;
                    update.tipoMaterialDes = $scope.ddlTipoMaterial.detalle;
                }
                else { update.tipoMaterial = ""; update.tipoMaterialDes = ""; }
                if ($scope.ddlCatMaterial != null) {
                    update.categoriaMaterial = $scope.ddlCatMaterial.codigo;
                    update.categoriaMaterialDes = $scope.ddlCatMaterial.detalle;
                }
                else { update.categoriaMaterial = ""; update.categoriaMaterialDes = ""; }
                if ($scope.ddlGrupoArticulo != null)
                    update.grupoArticulo = $scope.ddlGrupoArticulo.codigo;
                else
                    update.grupoArticulo = "";
                if ($scope.ddlSeccArticulo != null)
                    update.seccionArticulo = $scope.ddlSeccArticulo.codigo;
                else
                    update.seccionArticulo = "";
                //update.catalogacion = $scope.ddlCatalogacion.codigo;
                if ($scope.ddlSurtParcial != null)
                    update.surtidoParcial = $scope.ddlSurtParcial.codigo;
                else
                    update.surtidoParcial = "";
                if ($scope.ddlMateria != null)
                { update.materia = $scope.ddlMateria.codigo; update.materiaDes = $scope.ddlMateria.detalle; }
                else
                { update.materia = ""; update.materiaDes = ""; }
                if ($scope.ddlIndPedido != null)
                    update.indPedido = $scope.ddlIndPedido.codigo;
                else
                    update.indPedido = "";
                if ($scope.ddlPerfDistribucion != null)
                    update.perfilDistribucion = $scope.ddlPerfDistribucion.codigo;
                else
                    update.perfilDistribucion = "";
                //update.almacen = $scope.ddlAlmacen.codigo;
                if ($scope.ddlGrupoCompras != null)
                    update.grupoCompra = $scope.ddlGrupoCompras.codigo;
                else
                    update.grupoCompra = "";
                if ($scope.ddlCatValoracion != null)
                    update.categoriaValoracion = $scope.ddlCatValoracion.codigo;
                else
                    update.categoriaValoracion = "";
                if ($scope.ddlTipoAlmacenBod != null)
                    update.tipoAlamcen = $scope.ddlTipoAlmacenBod.codigo;
                else
                    update.tipoAlamcen = "";

                if ($scope.ddlCondAlmacen != null)
                    update.condicionAlmacen = $scope.ddlCondAlmacen.codigo;
                else
                    update.condicionAlmacen = "";
                if ($scope.ddlClListSurtidos != null)
                    update.clListaSurtido = $scope.ddlClListSurtidos.codigo;
                else
                    update.clListaSurtido = "";
                if ($scope.ddlEstatusMaterial != null)
                    update.estatusMaterial = $scope.ddlEstatusMaterial.codigo;
                else
                    update.estatusMaterial = "";
                if ($scope.ddlEstatusVentas != null)
                    update.estatusVenta = $scope.ddlEstatusVentas.codigo;
                else
                    update.estatusVenta = "";
                if ($scope.ddlGrupoBalanzas != null)
                    update.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
                else
                    update.grupoBalanzas = "";
                if ($scope.ddlNacionalImportado != null)
                    update.nacionalImportado = $scope.ddlNacionalImportado.codigo;
                else
                    update.nacionalImportado = "";

                if ($scope.ddlMotivoRechazoArt != null)
                    update.motivoRechazo = $scope.ddlMotivoRechazoArt.codigo;
                else
                    update.motivoRechazo = "";

                update.observacion = $scope.observEstado;
                var d_fecha = "";
                if ($scope.p_Com.validoDesde != "")
                { d_fecha = $filter('date')($scope.p_Com.validoDesde, 'MM/dd/yyyy'); }
                update.validoDesde = d_fecha;
                //1: Asistente Compras //3: Datos Maestros
                update.estado = ($scope.accionArt === 1 ? ($scope.tipouser === "1" ? "APC" : "APD") : ($scope.tipouser === "1" ? "REC" : "RED"));
                for (var i = 0 ; i < $scope.grArticulo.length; i++) {
                    if ($scope.grArticulo[i].idDetalle == $scope.p_Com.idDetalle)
                        break;
                }

                if (update.estado == 'APC' || update.estado == 'APD')
                    $scope.grArticulo[i].estado = 'Aprobado';
                if (update.estado == 'REC' || update.estado == 'RED')
                    $scope.grArticulo[i].estado = 'Rechazado';
                update.accion = "U";
                break;
            }
        }

        if (esIngreso) {
            $scope.ingDatCompras = {};
            $scope.ingDatCompras.idDetalle = $scope.p_Com.idDetalle;
            if ($scope.p_Com.codLegacy != null)
                $scope.ingDatCompras.codLegacyProv = $scope.p_Com.codLegacy.codigo;
            else
                $scope.ingDatCompras.codLegacyProv = "";
            $scope.ingDatCompras.costoFOB = $scope.p_Com.costoFOB;
            $scope.ingDatCompras.observaciones = $scope.p_Com.observaciones;
            //Nuevos

            if ($scope.ddlNacionalImportado != null)
                $scope.ingDatCompras.jerarquiaProd = $scope.ddlNacionalImportado.codigo;
            else
                $scope.ingDatCompras.jerarquiaProd = "";
            $scope.ingDatCompras.susceptBonifEsp = $scope.p_Com.susceptBonifEsp;
            $scope.ingDatCompras.procedimCatalog = $scope.p_Com.procedimCatalog;
            $scope.ingDatCompras.caracterPlanNec = $scope.p_Com.caracterPlanNec;
            $scope.ingDatCompras.fuenteProvision = $scope.p_Com.fuenteProvision;
            if ($scope.ddlOrgCompras != null) {
                $scope.ingDatCompras.organizacionCompras = $scope.ddlOrgCompras.codigo;
                $scope.ingDatCompras.organizacionComprasDes = $scope.ddlOrgCompras.detalle;
            }
            else {
                $scope.ingDatCompras.organizacionCompras = "";
                $scope.ingDatCompras.organizacionComprasDes = "";
            }
            if ($scope.ddlFrecEntrega != null)
                $scope.ingDatCompras.frecuenciaEntrega = $scope.ddlFrecEntrega.codigo;
            else
                $scope.ingDatCompras.frecuenciaEntrega = "";
            if ($scope.ddlTipoMaterial != null) {
                $scope.ingDatCompras.tipoMaterial = $scope.ddlTipoMaterial.codigo;
                $scope.ingDatCompras.tipoMaterialDes = $scope.ddlTipoMaterial.detalle;
            }
            else { $scope.ingDatCompras.tipoMaterial = ""; $scope.ingDatCompras.tipoMaterialDes = ""; }
            if ($scope.ddlCatMaterial != null) {
                $scope.ingDatCompras.categoriaMaterial = $scope.ddlCatMaterial.codigo;
                $scope.ingDatCompras.categoriaMaterialDes = $scope.ddlCatMaterial.detalle;
            }
            else { $scope.ingDatCompras.categoriaMaterial = ""; $scope.ingDatCompras.categoriaMaterialDes = ""; }
            if ($scope.ddlGrupoArticulo != null)
                $scope.ingDatCompras.grupoArticulo = $scope.ddlGrupoArticulo.codigo;
            else
                $scope.ingDatCompras.grupoArticulo = "";
            if ($scope.ddlSeccArticulo != null)
                $scope.ingDatCompras.seccionArticulo = $scope.ddlSeccArticulo.codigo;
            else
                $scope.ingDatCompras.seccionArticulo = "";

            if ($scope.ddlSurtParcial != null)
                $scope.ingDatCompras.surtidoParcial = $scope.ddlSurtParcial.codigo;
            else
                $scope.ingDatCompras.surtidoParcial = "";
            if ($scope.ddlMateria != null)
            { $scope.ingDatCompras.materia = $scope.ddlMateria.codigo; $scope.ingDatCompras.materiaDes = $scope.ddlMateria.detalle; }
            else
            { $scope.ingDatCompras.materia = ""; $scope.ingDatCompras.materiaDes = ""; }
            if ($scope.ddlIndPedido != null)
                $scope.ingDatCompras.indPedido = $scope.ddlIndPedido.codigo;
            else
                $scope.ingDatCompras.indPedido = "";
            if ($scope.ddlPerfDistribucion != null)
                $scope.ingDatCompras.perfilDistribucion = $scope.ddlPerfDistribucion.codigo;
            else
                $scope.ingDatCompras.perfilDistribucion = "";

            if ($scope.ddlGrupoCompras != null)
                $scope.ingDatCompras.grupoCompra = $scope.ddlGrupoCompras.codigo;
            else
                $scope.ingDatCompras.grupoCompra = "";
            if ($scope.ddlCatValoracion != null)
                $scope.ingDatCompras.categoriaValoracion = $scope.ddlCatValoracion.codigo;
            else
                $scope.ingDatCompras.categoriaValoracion = "";
            if ($scope.ddlTipoAlmacenBod != null)
                $scope.ingDatCompras.tipoAlamcen = $scope.ddlTipoAlmacenBod.codigo;
            else
                $scope.ingDatCompras.tipoAlamcen = "";

            if ($scope.ddlCondAlmacen != null)
                $scope.ingDatCompras.condicionAlmacen = $scope.ddlCondAlmacen.codigo;
            else
                $scope.ingDatCompras.condicionAlmacen = "";
            if ($scope.ddlClListSurtidos != null)
                $scope.ingDatCompras.clListaSurtido = $scope.ddlClListSurtidos.codigo;
            else
                $scope.ingDatCompras.clListaSurtido = "";
            if ($scope.ddlEstatusMaterial != null)
                $scope.ingDatCompras.estatusMaterial = $scope.ddlEstatusMaterial.codigo;
            else
                $scope.ingDatCompras.estatusMaterial = "";
            if ($scope.ddlEstatusVentas != null)
                $scope.ingDatCompras.estatusVenta = $scope.ddlEstatusVentas.codigo;
            else
                $scope.ingDatCompras.estatusVenta = "";
            if ($scope.ddlGrupoBalanzas != null)
                $scope.ingDatCompras.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
            else
                $scope.ingDatCompras.grupoBalanzas = "";
            if ($scope.ddlNacionalImportado != null)
                $scope.ingDatCompras.nacionalImportado = $scope.ddlNacionalImportado.codigo;
            else
                $scope.ingDatCompras.nacionalImportado = "";

            if ($scope.ddlMotivoRechazoArt != null)
                $scope.ingDatCompras.motivoRechazo = $scope.ddlMotivoRechazoArt.codigo;
            else
                $scope.ingDatCompras.motivoRechazo = "";

            $scope.ingDatCompras.observacion = $scope.observEstado;
            var d_fecha = "";
            if ($scope.p_Com.validoDesde != "")
            { d_fecha = $filter('date')($scope.p_Com.validoDesde, 'MM/dd/yyyy'); }
            $scope.ingDatCompras.validoDesde = d_fecha;
            //1: Asistente Compras //3: Datos Maestros
            $scope.ingDatCompras.estado = ($scope.accionArt === 1 ? ($scope.tipouser === "1" ? "APC" : "APD") : ($scope.tipouser === "1" ? "REC" : "RED"));
            for (var i = 0 ; i < $scope.grArticulo.length; i++) {
                if ($scope.grArticulo[i].idDetalle == $scope.p_Com.idDetalle)
                    break;
            }

            if ($scope.ingDatCompras.estado == 'APC' || $scope.ingDatCompras.estado == 'APD')
                $scope.grArticulo[i].estado = 'Aprobado';
            if ($scope.ingDatCompras.estado == 'REC' || $scope.ingDatCompras.estado == 'RED')
                $scope.grArticulo[i].estado = 'Rechazado';
            $scope.ingDatCompras.accion = "I";

            $scope.Com.push($scope.ingDatCompras);
        }



        //Agrega el articulo y carga el grid
        //$scope.grArticulo = $scope.Det;

        if ($scope.isNewGenerico) {
            $scope.agregarGenerico($scope.p_Com.idDetalle);
        }
        $scope.Limpia();
        $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
    }

    $scope.AddMedida = function () {

        if (!$scope.frmDatosMedida.txtFactorCon.$valid) {
            $scope.MenjError = "Valor incorrecto en  factor de conversión. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtPesoNeto.$valid) {
            $scope.MenjError = "Valor incorrecto en peso neto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtPesoBruto.$valid) {
            $scope.MenjError = "Valor incorrecto en peso bruto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtLongitud.$valid) {
            $scope.MenjError = "Valor incorrecto en longitud. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtAncho.$valid) {
            $scope.MenjError = "Valor incorrecto en ancho. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtAltura.$valid) {
            $scope.MenjError = "Valor incorrecto en altura. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtVolumen.$valid) {
            $scope.MenjError = "Valor incorrecto en volumen. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.descuento1 > 100) {
            $scope.MenjError = "Descuento 1 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.descuento2 > 100) {
            $scope.MenjError = "Descuento 2 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }



        //if (!$scope.frmDatosMedida.txtPrecioBruto.$valid) {
        //    $scope.MenjError = "Valor incorrecto en precio bruto. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}
        //if (!$scope.frmDatosMedida.txtDescuento1.$valid) {
        //    $scope.MenjError = "Valor incorrecto en descuento 1. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}
        //if (!$scope.frmDatosMedida.txtDescuento2.$valid) {
        //    $scope.MenjError = "Valor incorrecto en descuento 2. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}


        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe generar un nuevo artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //validar ingreso solo de una unidad de medida
        if ($scope.grArtMedida.length >= 1) {
            if ($scope.grArtMedida[0].unidadMedida != $scope.ddlUnidadMedida.codigo) {

                //$scope.MenjError = "Solo se puede agregar una unidad de medida.";
                //$('#idMensajeError').modal('show');
                //return;
            }

        }

        //No validar si la unidad de medida es CAJAMASTER METRO CUBICO O MEDIO PALET
        if ($scope.ddlUnidadMedida.codigo != 'KI' &&
                                                  $scope.ddlUnidadMedida.codigo != 'M3' &&
                                                  $scope.ddlUnidadMedida.codigo != 'MPL') {
            //Valida si ha ingresado un codigo de barra
            if ($scope.grArtCodBarra.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos el código de barra principal.";
                $('#idMensajeError').modal('show');
                return;
            } else {
                //Valida si hay un código de barra ingresado como el principal
                var contador = 0;
                for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                    if ($scope.grArtCodBarra[i].principal === true) {
                        contador += 1;
                    }
                }
                if (contador === 0) {
                    $scope.MenjError = "Un código de barra debe ser el principal.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }
        }



        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == $scope.ddlUnidadMedida.codigo) {
                    edita = true;
                    break;
                }
            }
        }

        //Obtiene y carga los datos de los combos
        $scope.p_Med.unidadMedida = $scope.ddlUnidadMedida.codigo;
        $scope.p_Med.desUnidadMedida = $scope.ddlUnidadMedida.detalle;
        $scope.p_Med.uniMedidaVolumen = $scope.ddlVolumen.codigo;
        $scope.p_Med.desUniMedidaVolumen = $scope.ddlVolumen.detalle;
        //for (var i = 0; i < $scope.tipoUnidadMedida.length;i++)
        //{
        //    $scope.p_Med.tipoUnidadMedida = $scope.tipoUnidadMedida[i].codigo;
        //    $scope.p_Med.desTipoUnidadMedida = $scope.tipoUnidadMedida[i].detalle;
        //}
        $scope.p_Med.tipoUnidadMedida = $scope.ddlTipoUnidadMedida.codigo;
        $scope.p_Med.desTipoUnidadMedida = $scope.ddlTipoUnidadMedida.detalle;
        $scope.p_Med.uniMedConvers = $scope.ddlUniMedConvers.codigo;
        $scope.p_Med.desUniMedConvers = $scope.ddlUniMedConvers.detalle;
        $scope.p_Med.uniMedBase = $scope.p_Med.medBase;
        $scope.p_Med.uniMedPedido = $scope.p_Med.medPedido;
        $scope.p_Med.uniMedES = $scope.p_Med.medES;
        $scope.p_Med.uniMedVenta = $scope.p_Med.medVenta;
        $scope.p_Med.estado = "1";
        $scope.p_Med.accion = "I";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == $scope.ddlUnidadMedida.codigo) {
                    update.unidadMedida = $scope.p_Med.unidadMedida;
                    update.tipoUnidadMedida = $scope.p_Med.tipoUnidadMedida;
                    update.desTipoUnidadMedida = $scope.p_Med.desTipoUnidadMedida;
                    update.uniMedidaVolumen = $scope.p_Med.uniMedidaVolumen;
                    update.desUniMedidaVolumen = $scope.p_Med.desUniMedidaVolumen;
                    update.uniMedConvers = $scope.p_Med.uniMedConvers
                    update.desUniMedConvers = $scope.p_Med.desUniMedConvers;
                    update.factorCon = $scope.p_Med.factorCon;
                    update.pesoNeto = $scope.p_Med.pesoNeto;
                    update.pesoBruto = $scope.p_Med.pesoBruto;
                    update.longitud = $scope.p_Med.longitud;
                    update.ancho = $scope.p_Med.ancho;
                    update.altura = $scope.p_Med.altura;
                    update.volumen = $scope.p_Med.volumen;
                    update.precioBruto = $scope.p_Med.precioBruto;
                    update.descuento1 = $scope.p_Med.descuento1;
                    update.descuento2 = $scope.p_Med.descuento2;
                    update.impVerde = $scope.p_Med.impVerde;
                    update.uniMedBase = $scope.p_Med.medBase;
                    update.uniMedPedido = $scope.p_Med.medPedido;
                    update.uniMedES = $scope.p_Med.medES;
                    update.uniMedVenta = $scope.p_Med.medVenta;
                    update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Agrega nuevo codigo de barra
            $scope.p_Med.idDetalle = $scope.p_Det.idDetalle;
            $scope.Med.push($scope.p_Med);
        }

        //Filtra y muestra las medidas
        //$scope.p_Med = {};
        $scope.p_Med.idDetalle = $scope.p_Det.idDetalle;
        $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Med.idDetalle });

        //Limpia Unidad Medida
        //$scope.p_Med.unidadMedida = "";
        //$scope.p_Med.tipoUnidadMedida = "";
        //$scope.p_Med.uniMedConvers = "";
        //$scope.p_Med.desUnidadMedida = "";
        //$scope.p_Med.desTipoUnidadMedida = "";
        //$scope.p_Med.desUniMedConvers = "";
        //$scope.ddlUnidadMedida = "";
        //$scope.ddlTipoUnidadMedida = "";
        //$scope.ddlUniMedConvers = "";
        //$scope.p_Med.factorCon = "";
        //$scope.p_Med.pesoNeto = "";
        //$scope.p_Med.pesoBruto = "";
        //$scope.p_Med.longitud = "";
        //$scope.p_Med.ancho = "";
        //$scope.p_Med.altura = "";
        //$scope.p_Med.precioBruto = "";
        //$scope.p_Med.descuento1 = "";
        //$scope.p_Med.descuento2 = "";
        //$scope.p_Med.estado = "";
        //$scope.p_Med.impVerde = false;
        $scope.p_Med = {};
        $scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        ////Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
        //$scope.NextImagen();
    }

    //Consulta codigos EAN    
    $scope.buscarCodEAN = function (codEAN, tipoEAN) {

        if ($scope.p_CBr.unidadMedida === "") {
            $scope.MenjError = "Debe seleccionar la unidad de medida.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Validar si ya esta ingresado EAN
        for (var t = 0 ; $scope.grArtCodBarra.length > t; t++) {
            if ($scope.grArtCodBarra[t].numeroEan == codEAN) {
                $scope.MenjError = "Ya existe código de barra.";
                $('#idMensajeError').modal('show');
                return;
            }
        }
        //Valida que haya consultado (BAPI) el codigo de barra ingresado
        if (codEAN === "") { //Cambiar despues por el tipo o descripción - deshabiltar controles cuando este la BAPI
            $scope.MenjError = "Debe digitar el Código de Barra.";
            $('#idMensajeError').modal('show');
            return;
        }

        $scope.myPromise =
            AdmArticuloService.getConsultaEAN(codEAN, tipoEAN.codigo).then(function (response) {

                if (response.data.success || $scope.inactivaCodEAN) {

                    $scope.p_CBr.tipoEan = tipoEAN.codigo;
                    $scope.p_CBr.descripcionEan = tipoEAN.detalle;
                    $scope.p_CBr.paisEan = $scope.p_CBr.paisEanCat.codigo;
                    $scope.p_CBr.paisDesEan = $scope.p_CBr.paisEanCat.detalle;
                    $scope.AddCodBarra();
                }
                else {

                    $scope.MenjError = "Código EAN no valido o ya registrado.";
                    $('#idMensajeError').modal('show');
                    return;
                }


            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        ;



    }

    $scope.AddCodBarra = function () {

        //Valida que haya seleccionado la unidad de medida
        if ($scope.p_CBr.unidadMedida === "") {
            $scope.MenjError = "Debe seleccionar la unidad de medida.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya consultado (BAPI) el codigo de barra ingresado
        if ($scope.p_CBr.tipoEan === "" && $scope.p_CBr.descripcionEan === "") { //Cambiar despues por el tipo o descripción - deshabiltar controles cuando este la BAPI
            $scope.MenjError = "Debe digitar el código de barra y consultar el tipo y la descripción del mismo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        if ($scope.grArtCodBarra.length != 0) {
            for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                if ($scope.grArtCodBarra[i].numeroEan === $scope.p_CBr.numeroEan) {
                    edita = true;
                    break;
                }
            }
        }

        $scope.p_CBr.accion = "I";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.CBr.length; i < len; i++) {
                var update = $scope.CBr[i];
                if (update.numeroEan === $scope.p_CBr.numeroEan) {
                    update.tipoEan = $scope.p_CBr.tipoEan;
                    update.descripcionEan = $scope.p_CBr.descripcionEan;
                    update.paisEan = $scope.p_CBr.paisEan;
                    update.paisDesEan = $scope.p_CBr.paisDesEan;
                    update.principal = $scope.p_CBr.principal;
                    update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Valida si ya existe un principal
            if ($scope.grArtCodBarra.length != 0) {
                for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                    if ($scope.grArtCodBarra[i].principal === true && $scope.p_CBr.principal === true) {
                        $scope.MenjError = "Ya existe un EAN principal.";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                }
            }
            //Agrega nuevo codigo de barra
            $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
            $scope.CBr.push($scope.p_CBr);
        }

        //Filtra y muestra los codigos de barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = $scope.ddlUnidadMedida.codigo;
        $scope.temp_cbr = $filter('filter')($scope.CBr, { idDetalle: $scope.p_CBr.idDetalle });
        $scope.grArtCodBarra = $filter('filter')($scope.temp_cbr, { unidadMedida: $scope.p_CBr.unidadMedida });





        //Limpia Codigo de Barra
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
    }

    $scope.NewArticulo = function () {

        $scope.bloqueaCodReferencia = false;
        //Validar que haya seleccionado una linea de negocio
        if ($scope.ddlLineaNegocio === "") {
            $scope.MenjError = "Debe seleccionar la línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlLineaNegocio === null) {
            $scope.MenjError = "Debe seleccionar la línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }

        $scope.inactivaTab = false;

        $scope.Limpia();
        $scope.p_Com = {};
        //categoriaMaterial -- para esta pantalla por defecto sera 01-material generico
        var cm = $scope.catMaterial;
        for (var index = 0; index < cm.length; ++index) {
            if (cm[index].codigo === "01")
            { $scope.ddlCatMaterial = cm[index]; break; }
        }
        $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
        //Para setear el path temporal para las imagenes
        var secuencia = "";
        AdmArticuloService.getSecuenciaDirectorio("Notificacion").then(function (results) {
            if (results.data.success) {
                secuencia = results.data.root[0];
                var direc = "Art_" + secuencia;

                //Graba rutas por articulos
                $scope.Ruta = {};
                $scope.Ruta.idDetalle = $scope.p_Det.idDetalle;
                $scope.Ruta.path = direc;
                $scope.Rutas.push($scope.Ruta);

                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                $scope.uploader2.url = Ruta;
            }
        }, function (error) {
        });
        //Genera nuevo codigo de referencia

        if ($scope.grArticulo.length > 0) {
            var len = $scope.grArticulo.length - 1;
            $scope.p_Det.idDetalle = parseInt($scope.grArticulo[len].idDetalle) + 1;
        }
        else
            $scope.p_Det.idDetalle = $scope.grArticulo.length + 1;

    }

    $scope.Limpia = function () {
        //Detalle
        $scope.p_Det = {};
        $scope.p_Det.idDetalle = "";
        $scope.p_Det.codReferencia = "";
        $scope.p_Det.marca = "";
        $scope.p_Det.desMarca = "";
        $scope.ddlMarca = "";
        $scope.p_Det.paisOrigen = "";
        $scope.ddlPaisOrigen = "";
        $scope.p_Det.regionOrigen = "";
        $scope.ddlRegionOrigen = "";
        $scope.p_Det.gradoAlcohol = "";
        $scope.ddlGradoAlcohol = "";
        $scope.p_Det.talla = "";
        $scope.ddlTalla = "";
        $scope.p_Det.color = "";
        $scope.ddlColor = "";
        $scope.p_Det.fragancia = "";
        $scope.ddlFragancia = "";
        $scope.p_Det.tipos = "";
        $scope.ddlTipos = "";
        $scope.p_Det.sabor = "";
        $scope.ddlSabor = "";
        $scope.p_Det.modelo = "";
        $scope.ddlModelo = "";
        $scope.p_Det.coleccion = "";
        $scope.ddlColeccion = "";
        $scope.p_Det.temporada = "";
        $scope.ddlTemporada = "";
        $scope.p_Det.estacion = "";
        $scope.ddlEstacion = "";
        $scope.ddlCantidadPedir = "";
        $scope.p_Det.iva = "";
        $scope.ddlIva = "";
        $scope.p_Det.deducible = "";
        $scope.ddlDeducible = "";
        $scope.p_Det.retencion = "";
        $scope.ddlRetencion = "";
        $scope.p_Det.descripcion = "";
        $scope.p_Det.otroId = "";
        $scope.p_Det.tamArticulo = "";
        $scope.p_Det.contAlcohol = false;
        $scope.p_Det.accion = "";
        $scope.p_Det.estado = "";
        $scope.p_Det.precioBruto = "";
        $scope.p_Det.descuento1 = "";
        $scope.p_Det.descuento2 = "";
        $scope.p_Det.impVerde = "";
        //Compras
        $scope.p_Com = {};
        $scope.p_Com.idDetalle = "";
        $scope.p_Com.codSap = "";
        $scope.p_Com.codLegacy = "";
        $scope.p_Com.costoFOB = "";
        $scope.p_Com.validoDesde = "";
        $scope.p_Com.observaciones = "";
        $scope.p_Com.organizacionCompras = "";
        $scope.p_Com.organizacionComprasDes = "";
        $scope.p_Com.frecuenciaEntrega = "";
        $scope.p_Com.tipoMaterial = "";
        $scope.p_Com.tipoMaterialDes = "";
        $scope.p_Com.categoriaMaterial = "";
        $scope.p_Com.categoriaMaterialDes = "";
        $scope.p_Com.grupoArticulo = "";
        $scope.p_Com.seccionArticulo = "";
        $scope.p_Com.catalogacion = "";
        $scope.p_Com.surtidoParcial = "";
        $scope.p_Com.materia = "";
        $scope.p_Com.indPedido = "";
        $scope.p_Com.perfilDistribucion = "";
        $scope.p_Com.almacen = "";
        $scope.p_Com.grupoCompra = "";
        $scope.p_Com.categoriaValoracion = "";
        $scope.p_Com.tipoAlamcen = "";
        $scope.p_Com.indalmaentrada = "";
        $scope.p_Com.indalmasalida = "";
        $scope.p_Com.indareaalmacen = "";
        $scope.p_Com.condicionAlmacen = "";
        $scope.p_Com.clListaSurtido = "";
        $scope.p_Com.estatusMaterial = "";
        $scope.p_Com.estatusVenta = "";
        $scope.p_Com.grupoBalanzas = "";
        $scope.p_Com.nacionalImportado = "";
        $scope.p_Com.coleccion = "";
        $scope.p_Com.temporada = "";
        $scope.p_Com.estacion = "";
        $scope.p_Com.estado = "";
        $scope.p_Com.motivoRechazo = "";
        //Combos
        $scope.ddlOrgCompras = "";
        $scope.ddlFrecEntrega = "";
        $scope.ddlTipoMaterial = "";
        $scope.ddlCatMaterial = "";
        $scope.ddlGrupoArticulo = "";
        $scope.ddlSeccArticulo = "";
        $scope.ddlCatalogacion = "";
        $scope.ddlSurtParcial = "";
        $scope.ddlMateria = "";
        $scope.ddlMateriaDes = "";
        $scope.ddlIndPedido = "";
        $scope.ddlPerfDistribucion = "";
        $scope.ddlAlmacen = "";
        $scope.ddlGrupoCompras = "";
        $scope.ddlCatValoracion = "";
        $scope.ddlTipoAlmacen = "";
        $scope.ddlIndTipoAlmEnt = "";
        $scope.ddlIndTipoAlmSal = "";
        $scope.ddlIndAreaAlmacen = "";
        $scope.ddlCondAlmacen = "";
        $scope.ddlClListSurtidos = "";
        $scope.ddlEstatusMaterial = "";
        $scope.ddlEstatusVentas = "";
        $scope.ddlGrupoBalanzas = "";
        $scope.ddlNacionalImportado = "";
        $scope.ddlColeccion = "";
        $scope.ddlTemporada = "";
        $scope.ddlEstacion = "";
        $scope.ddlCantidadPedir = "";
        //Catalogacion
        $scope.p_Cat = {};
        $scope.grArtCatalogacion = [];
        $scope.p_Cat.idDetalle = "";
        $scope.p_Cat.catalogacion = "";
        $scope.p_Cat.desCatalogacion = "";
        $scope.p_Cat.accion = "";
        //Almacen

        $scope.p_Alm = {};
        $scope.grArtAlmacen = [];
        $scope.p_Alm.idDetalle = "";
        $scope.p_Alm.almacen = "";
        $scope.p_Alm.desAlmacen = "";
        $scope.p_Alm.accion = "";
        //IndTipoAlmEnt
        $scope.p_Iae = {};
        $scope.grArtIndTipoAlmEnt = [];
        $scope.p_Iae.idDetalle = "";
        $scope.p_Iae.indTipoAlmEnt = "";
        $scope.p_Iae.desIndTipoAlmEnt = "";
        $scope.p_Iae.accion = "";
        //IndTipoAlmSal
        $scope.p_Ias = {};
        $scope.grArtIndTipoAlmSal = [];
        $scope.p_Ias.idDetalle = "";
        $scope.p_Ias.indTipoAlmSal = "";
        $scope.p_Ias.desIndTipoAlmSal = "";
        $scope.p_Ias.accion = "";
        //IndAreaAlmacen
        $scope.p_Iaa = {};
        $scope.grArtIndAreaAlmacen = [];
        $scope.p_Iaa.idDetalle = "";
        $scope.p_Iaa.indAreaAlmacen = "";
        $scope.p_Iaa.desIndAreaAlmacen = "";
        $scope.p_Iaa.accion = "";
        //Medida
        $scope.p_Med = {};
        $scope.p_Med.idDetalle = "";
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.ddlUnidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUniMedConvers = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = "";
        $scope.p_Med.ancho = "";
        $scope.p_Med.altura = "";
        $scope.p_Med.volumen = "";
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.impVerde = false;
        $scope.p_Med.medBase = false;
        $scope.p_Med.medPedido = false;
        $scope.p_Med.medES = false;
        $scope.p_Med.medVenta = false;
        $scope.p_Med.accion = "";
        $scope.p_Med.estado = "";
        //Codigo Barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = "";
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.p_CBr.accion = "";
        //Imagenes
        $scope.p_Ima = {};
        $scope.p_Ima.idDetalle = "";
        $scope.p_Ima.path = "";
        $scope.p_Ima.idDocAdjunto = "";
        $scope.p_Ima.nomArchivo = "";
        $scope.p_Ima.accion = "";
        //Limpia Grids
        $scope.grArtMedida = [];
        $scope.grArtCodBarra = [];
        $scope.grArtImagen = [];

        $scope.ddlAgrupacion = "";
        $scope.CaractArtTMP = [];
    }

    $scope.NextMedida = function () {

        if (!$scope.requeridodet.txtPrecioBruto.$valid) {
            $scope.MenjError = "Valor incorrecto en precio bruto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.requeridodet.txtDescuento1.$valid) {
            $scope.MenjError = "Valor incorrecto en descuento 1. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descuento1 > 100) {
            $scope.MenjError = "Descuento 1 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.requeridodet.txtDescuento2.$valid) {
            $scope.MenjError = "Valor incorrecto en descuento 2. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descuento2 > 100) {
            $scope.MenjError = "Descuento 2 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Genere un nuevo artículo";
            $('#idMensajeError').modal('show');
            $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
        } else {
            $('.nav-tabs a[href="#Medidas"]').tab('show');
        }
    }

    $scope.NextImagen = function () {


        //Validar que se han seleccionado todas las unidades de medida
        var exisUMbase = false;
        var exisUMpedido = false;
        var exisUMes = false;
        var exisUMventa = false;

        for (var k = 0 ; k < $scope.grArtMedida.length ; k++) {


            if ($scope.grArtMedida[k].uniMedBase) exisUMbase = true;
            if ($scope.grArtMedida[k].uniMedPedido) exisUMpedido = true;
            if ($scope.grArtMedida[k].uniMedVenta) exisUMventa = true;
            if ($scope.grArtMedida[k].uniMedES) exisUMes = true;
        }


        if (!exisUMbase) {
            $scope.MenjError = "Debe seleccionar Unidad Base en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMpedido) {
            $scope.MenjError = "Debe seleccionar Unidad Pedido en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMes) {
            $scope.MenjError = "Debe seleccionar Unidad E/S en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMventa) {
            $scope.MenjError = "Debe seleccionar Unidad Venta en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }




        if ($scope.grArtMedida.length != 0) {
            $('.nav-tabs a[href="#Imagenes"]').tab('show');
        } else {
            $scope.MenjError = "Debe ingresar por lo menos una Medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
    }

    //INICIO CARGA DE IMAGENES 
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileArticulo/UploadFile/?path=';
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            // 
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "jpg" || extension == "bmp" || extension == "png" || extension == "jpeg" || extension == "gif")
                return true;
            else {
                alert('Formato de Imagen incorrecto. Por favor seleccione un archivo con los siguientes formatos jpg/bmp/png/jpeg o gif e intente de nuevo.');
                return false;
            }
        }
    });
    uploader2.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });
    uploader2.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            // 
            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });
    // CALLBACKS
    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader2.onAfterAddingFile = function (fileItem) {
        if ($scope.Ruta.path === "") {
            //Remueve las imagenes de la cola
            uploader2.clearQueue();
            $scope.MenjError = "Debe generar un nuevo código de artículo.";
            $('#idMensajeError').modal('show');
            return;
        } else {
            //Registra en el modelo
            $scope.p_Ima = {};
            $scope.p_Ima.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Ima.path = $scope.Ruta.path;
            $scope.p_Ima.idDocAdjunto = $scope.uploader2.queue.length;
            $scope.p_Ima.nomArchivo = fileItem.file.name;
            $scope.p_Ima.accion = "I";
            $scope.Ima.push($scope.p_Ima);
            //Registra en el grid
            $scope.grArtImagen.push(
                { idDocAdjunto: $scope.uploader2.queue.length, nomArchivo: fileItem.file.name });
            uploader2.uploadAll();
        }
    };
    //Se dispara cuando realiza la carga
    uploader2.onSuccessItem = function (fileItem, response, status, headers) {

        if ($scope.uploader2.progress == 100) {

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
    console.info('uploader', uploader2);
    //FIN CARGA DE IMAGENES

    $scope.nomfile = "";
    $scope.direcXls = "Art_ExcelMasiva";

    $scope.MasivaPopup = function () {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + $scope.direcXls;
        $scope.uploader3.url = Ruta;
        $('#idCargaMasiva').modal('show');
    }

    $scope.CargaMasiva = function () {

        if (uploader3.queue.length === 0) {
            $scope.MenjError = "Seleccione un archivo.";
            $('#idMensajeError').modal('show');
        } else {
            AdmArticuloService.getCargaMasiva($scope.direcXls, $scope.nomfile).then(function (results) {

                $scope.Det = results.data.root[0];

                $scope.grArticulo = $scope.Det;
                $scope.Med = results.data.root[1];
                $scope.CBr = results.data.root[2];
                $scope.accion = 0;
                $scope.MenjError = 'Se ha cargado exitosamente.';
                $('#idMensajeOk').modal('show');
            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        }
    }

    //INICIO CARGA MASIVA
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileArticulo/UploadFile/?path=';
    var uploader3 = $scope.uploader3 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader3.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            // 
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "xls" || extension == "xlsx")
                return true;
            else {
                alert('Formato de archivo incorrecto. Por favor seleccione un archivo con los siguientes formatos xls/xlsx e intente de nuevo.');
                return false;
            }
        }
    });
    uploader3.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });
    uploader3.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
            // 
            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });
    // CALLBACKS
    uploader3.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader3.onAfterAddingFile = function (fileItem) {
        $scope.nomfile = fileItem.file.name;
        uploader3.uploadAll();
    };
    //Se dispara cuando realiza la carga
    uploader3.onSuccessItem = function (fileItem, response, status, headers) {

        if ($scope.uploader3.progress == 100) {

        }
    };
    uploader3.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };
    uploader3.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
    };
    uploader3.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader3.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };
    uploader3.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader3.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };
    uploader3.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };
    uploader3.onCompleteAll = function () {
        console.info('onCompleteAll');
    };
    console.info('uploader', uploader3);
    //FIN CARGA MASIVA

    $scope.CargaConsulta();
}
]);

'use strict';
app.controller('AdmArtAdenController', ['$scope', '$location', '$http', 'AdmArticuloService', 'ngAuthSettings', '$filter', 'FileUploader', '$routeParams', 'localStorageService', 'authService', function ($scope, $location, $http, AdmArticuloService, ngAuthSettings, $filter, FileUploader, $routeParams, localStorageService, authService) {



    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Grids
    $scope.grArticulo = [];
    $scope.grArtMedida = [];
    $scope.grArtCodBarra = [];
    $scope.grArtImagen = [];
    //Administrativo
    $scope.grArtCatalogacion = [];
    $scope.grArtCentros = [];
    $scope.grArtAlmacen = [];
    $scope.grArtIndTipoAlmEnt = [];
    $scope.grArtIndTipoAlmSal = [];
    $scope.grArtIndAreaAlmacen = [];
    $scope.grArtObservacion = [];
    $scope.etiTotRegistros = "";
    //DataSource Combos
    $scope.lineaNegocio = [];
    $scope.marca = [];
    $scope.paisOrigen = [];
    $scope.regionOrigen = [];
    $scope.regionOrigenTemp = [];
    $scope.talla = [];
    $scope.gradoAlcohol = [];
    $scope.color = [];
    $scope.fragancia = [];
    $scope.tipos = [];
    $scope.sabor = [];
    $scope.modelo = [];
    $scope.coleccion = [];
    $scope.temporada = [];
    $scope.catalogoVolumen = [];
    $scope.estacion = [];

    //De uno a muchos
    $scope.catalogacion = [];
    $scope.centros = [];
    $scope.almacen = [];
    $scope.indTipoAlmEnt = [];
    $scope.indTipoAlmSal = [];
    $scope.indAreaAlmNew = [];
    $scope.indAreaAlmacen = [];
    $scope.canalDistibucion = [];
    $scope.canalDistibucionF = [];

    $scope.unidadMedida = [];
    $scope.tipoEANart = [];
    $scope.tipoUnidadMedida = [];
    $scope.uniMedConvers = [];
    $scope.clasificacionFiscal = [];
    $scope.tipoAlmacenBod = [];
    $scope.ddlVolumen = [];
    //SI/NO Deducible y Retención
    $scope.sn_ded = [];
    $scope.sn_ret = [];
    $scope.SINO = [];
    $scope.CaracteristicasART = [];
    $scope.sn = { codigo: 'S', detalle: 'SI' };
    $scope.SINO.push($scope.sn);
    $scope.sn = { codigo: 'N', detalle: 'NO' };
    $scope.SINO.push($scope.sn);
    //$scope.sn_ded = $scope.SINO;
    //$scope.sn_ret = $scope.SINO;

    //modelo Detalle
    $scope.CabPrincipal = [];

    //modelo Detalle
    $scope.Det = [];
    $scope.p_Det = {};
    $scope.p_Det.idDetalle = "";
    $scope.p_Det.codReferencia = "";
    $scope.p_Det.marca = "";
    $scope.p_Det.desMarca = "";
    $scope.ddlMarca = "";
    $scope.p_Det.paisOrigen = "";
    $scope.ddlPaisOrigen = "";
    $scope.p_Det.regionOrigen = "";
    $scope.ddlRegionOrigen = "";
    $scope.p_Det.gradoAlcohol = "";
    $scope.ddlGradoAlcohol = "";
    $scope.p_Det.talla = "";
    $scope.ddlTalla = "";
    $scope.p_Det.color = "";
    $scope.ddlColor = "";
    $scope.p_Det.fragancia = "";
    $scope.ddlFragancia = "";
    $scope.p_Det.tipos = "";
    $scope.ddlTipos = "";
    $scope.p_Det.sabor = "";
    $scope.ddlSabor = "";
    $scope.p_Det.modelo = "";
    $scope.ddlModelo = "";
    $scope.p_Det.coleccion = "";
    $scope.ddlColeccion = "";
    $scope.p_Det.temporada = "";
    $scope.ddlTemporada = "";
    $scope.p_Det.estacion = "";
    $scope.ddlEstacion = "";
    $scope.ddlCantidadPedir = "";
    $scope.p_Det.iva = "";
    $scope.ddlIva = "";
    $scope.p_Det.deducible = "";
    $scope.ddlDeducible = "";
    $scope.p_Det.retencion = "";
    $scope.ddlRetencion = "";
    $scope.p_Det.descripcion = "";
    $scope.p_Det.marcaNueva = "";
    $scope.p_Det.viewMarcaNueva = true; //Para activar o inactivar el campo
    $scope.p_Det.tamArticulo = "";
    $scope.p_Det.otroId = "";
    $scope.p_Det.contAlcohol = false;
    $scope.p_Det.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Det.accion = "";
    $scope.p_Det.precioBruto = "";
    $scope.p_Det.descuento1 = "";
    $scope.p_Det.descuento2 = "";
    $scope.p_Det.impVerde = false;
    $scope.ddlAgrupacion = "";
    //modelo Medida
    $scope.Med = [];
    $scope.p_Med = {};
    $scope.p_Med.idDetalle = "";
    $scope.p_Med.unidadMedida = "";
    $scope.p_Med.desUnidadMedida = "";
    $scope.ddlUnidadMedida = "";
    $scope.p_Med.tipoUnidadMedida = "";
    $scope.p_Med.desTipoUnidadMedida = "";
    $scope.ddlTipoUnidadMedida = "";
    $scope.p_Med.uniMedConvers = "";
    $scope.p_Med.desUniMedConvers = "";
    $scope.ddlUniMedConvers = "";
    $scope.p_Med.factorCon = "";
    $scope.p_Med.pesoNeto = "";
    $scope.p_Med.pesoBruto = "";
    $scope.p_Med.longitud = "";
    $scope.p_Med.ancho = "";
    $scope.p_Med.altura = "";
    $scope.p_Med.volumen = "";
    $scope.p_Med.precioBruto = "";
    $scope.p_Med.descuento1 = "";
    $scope.p_Med.descuento2 = "";
    $scope.p_Med.impVerde = false;
    $scope.p_Med.medBase = false;
    $scope.p_Med.medPedido = false;
    $scope.p_Med.medES = false;
    $scope.p_Med.medVenta = false;
    $scope.p_Med.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Med.accion = "";

    //modelo Codigo Barra
    $scope.CBr = [];
    $scope.p_CBr = {};
    $scope.p_CBr.idDetalle = "";
    $scope.p_CBr.unidadMedida = "";
    $scope.p_CBr.numeroEan = "";
    $scope.p_CBr.tipoEan = "";
    $scope.p_CBr.descripcionEan = "";
    $scope.p_CBr.principal = false;
    $scope.p_CBr.accion = "";

    //modelo Imagenes
    $scope.Ima = [];
    $scope.p_Ima = {};
    $scope.p_Ima.idDetalle = "";
    $scope.p_Ima.path = "";
    $scope.p_Ima.idDocAdjunto = "";
    $scope.p_Ima.nomArchivo = "";
    $scope.p_Ima.accion = "";

    //modelo Compras
    $scope.Com = [];
    $scope.p_Com = {};
    $scope.p_Com.idDetalle = "";
    $scope.p_Com.codSap = "";
    $scope.p_Com.codLegacy = "";
    $scope.p_Com.costoFOB = "";
    $scope.p_Com.validoDesde = "";
    $scope.p_Com.observaciones = "";
    $scope.p_Com.organizacionCompras = "";
    $scope.p_Com.organizacionComprasDes = "";
    $scope.p_Com.frecuenciaEntrega = "";
    $scope.p_Com.tipoMaterial = "";
    $scope.p_Com.tipoMaterialDes = "";
    $scope.p_Com.categoriaMaterial = "";
    $scope.p_Com.categoriaMaterialDes = "";
    $scope.p_Com.grupoArticulo = "";
    $scope.p_Com.seccionArticulo = "";
    $scope.p_Com.surtidoParcial = "";
    $scope.p_Com.materia = "";
    $scope.p_Com.materiaDes = "";
    $scope.p_Com.indPedido = "";
    $scope.p_Com.perfilDistribucion = "";
    $scope.p_Com.grupoCompra = "";
    $scope.p_Com.categoriaValoracion = "";
    $scope.p_Com.tipoAlamcen = "";
    $scope.p_Com.condicionAlmacen = "";
    $scope.p_Com.clListaSurtido = "";
    $scope.p_Com.estatusMaterial = "";
    $scope.p_Com.estatusVenta = "";
    $scope.p_Com.grupoBalanzas = "";
    $scope.p_Com.nacionalImportado = "";
    $scope.p_Com.coleccion = "";
    $scope.p_Com.temporada = "";
    $scope.p_Com.estacion = "";
    $scope.p_Com.cantidadPedir = "";
    //Nuevos
    $scope.p_Com.jerarquiaProd = ""; //Jerarquía de Productos
    $scope.p_Com.susceptBonifEsp = ""; //El material es susceptible de bonificación en especie
    $scope.p_Com.procedimCatalog = ""; //Procedimiento de Catalogación
    $scope.p_Com.caracterPlanNec = ""; //Característica de planificación de necesidades
    $scope.p_Com.fuenteProvision = ""; //Fuente aprovisionamiento

    $scope.p_Com.estado = "";
    $scope.p_Com.motivoRechazo = "";
    /*Pasan de uno a muchos necesitan su propio modelo
    $scope.p_Com.catalogacion = "";
    $scope.p_Com.almacen = "";
    $scope.p_Com.indalmaentrada = "";
    $scope.p_Com.indalmasalida = "";
    $scope.p_Com.indareaalmacen = "";*/
    //Combos
    $scope.ddlOrgCompras = "";
    $scope.ddlFrecEntrega = "";
    $scope.ddlTipoMaterial = "";
    $scope.ddlCatMaterial = "";
    $scope.ddlGrupoArticulo = "";
    $scope.ddlSeccArticulo = "";
    $scope.ddlSurtParcial = "";
    $scope.ddlMateria = "";
    $scope.ddlMateriaDes = "";
    $scope.ddlIndPedido = "";
    $scope.ddlPerfDistribucion = "";
    $scope.ddlGrupoCompras = "";
    $scope.ddlCatValoracion = "";
    $scope.ddlTipoAlmacen = "";
    $scope.ddlTipoAlmacenBod = "";
    $scope.ddlCondAlmacen = "";
    $scope.ddlClListSurtidos = "";
    $scope.ddlEstatusMaterial = "";
    $scope.ddlEstatusVentas = "";
    $scope.ddlGrupoBalanzas = "";
    $scope.ddlNacionalImportado = "";
    $scope.ddlColeccion = "";
    $scope.ddlTemporada = "";
    $scope.ddlEstacion = "";
    $scope.ddlCantidadPedir = "";
    //De uno a muchos
    $scope.ddlCatalogacion = "";
    $scope.ddlCentros = "";
    $scope.ddlAlmacen = "";
    $scope.ddlIndTipoAlmEnt = "";
    $scope.ddlIndTipoAlmSal = "";
    $scope.ddlIndAreaAlmNew = "";
    $scope.ddlIndAreaAlmacen = "";
    $scope.ddlCanalDistibucion = [];
    $scope.ddlCanalDistibucionF = [];

    //modelo Centros
    $scope.Cen = [];
    $scope.p_Cen = {};
    $scope.p_Cen.idDetalle = "";
    $scope.p_Cen.centros = "";
    $scope.p_Cen.desCentros = "";
    $scope.p_Cen.accion = "";
    //modelo Catalogacion
    $scope.Cat = [];
    $scope.p_Cat = {};
    $scope.p_Cat.idDetalle = "";
    $scope.p_Cat.catalogacion = "";
    $scope.p_Cat.desCatalogacion = "";
    $scope.p_Cat.accion = "";
    //modelo Almacen
    $scope.Alm = [];
    $scope.p_Alm = {};
    $scope.p_Alm.idDetalle = "";
    $scope.p_Alm.almacen = "";
    $scope.p_Alm.desAlmacen = "";
    $scope.p_Alm.accion = "";
    //modelo IndTipoAlmEnt
    $scope.Iae = [];
    $scope.p_Iae = {};
    $scope.p_Iae.idDetalle = "";
    $scope.p_Iae.indTipoAlmEnt = "";
    $scope.p_Iae.desIndTipoAlmEnt = "";
    $scope.p_Iae.accion = "";
    //modelo IndTipoAlmSal
    $scope.Ias = [];
    $scope.p_Ias = {};
    $scope.p_Ias.idDetalle = "";
    $scope.p_Ias.indTipoAlmSal = "";
    $scope.p_Ias.desIndTipoAlmSal = "";

    $scope.p_Ias.accion = "";
    //modelo IndAreaAlmacen
    $scope.Iaa = [];
    $scope.p_Iaa = {};
    $scope.p_Iaa.idDetalle = "";
    $scope.p_Iaa.indAreaAlmacen = "";
    $scope.p_Iaa.desIndAreaAlmacen = "";
    $scope.p_Iaa.accion = "";


    //Cabecera
    $scope.ddlLineaNegocio = "";
    $scope.txtIdSolicitud = "";
    $scope.txtEstSolicitud = "";
    $scope.codSapProv = "";
    $scope.txtContRegistros = "";

    //Mensaje Error
    $scope.MsjConfirmacion = "";
    $scope.accion = "";

    //path Imagenes
    $scope.Rutas = [];
    $scope.Ruta = {};
    $scope.Ruta.codReferencia = "";
    $scope.Ruta.path = "";
    //Observacion y cambio de estado
    $scope.TextoBoton = "Grabar";
    $scope.ViewBoton = false;
    $scope.observEstado = "";
    $scope.ViewAsisCompras = true;
    $scope.ViewGernCompras = true;
    $scope.ViewDatosMaestr = true;
    //A nivel de la solicitud
    $scope.ViewMotRechazo = true;
    $scope.motivoRechazo = [];
    $scope.ddlMotivoRechazo = "";
    //A nivel del articulo
    $scope.ViewMotRechazoArt = true;
    $scope.motivoRechazoArt = [];
    $scope.ddlMotivoRechazoArt = "";


    //modelo caracteristicas
    $scope.CaractArt = [];
    $scope.CaractArtTMP = [];
    $scope.p_CaractArt = {};
    $scope.p_CaractArt.idDetalle = "";
    $scope.p_CaractArt.idCaract = "";
    $scope.p_CaractArt.idValor = "";
    $scope.p_CaractArt.idAgrupacion = "";
    $scope.p_CaractArt.accion = "";

    //boolean para inactivar botones, cajas de texto y tabs
    $scope.inactivaBot = false;
    $scope.inactivaTab = true;
    $scope.bloqueaBotInicio = false;
    $scope.isReadOnly = false;
    $scope.bloqueaCodReferencia = true;
    $scope.bloqueaDetalles = false;
    $scope.bloqCamposModif = true;
    $scope.inactivaCodEAN = false;


    //Lista de elementos eliminados
    $scope.listaDetallesSolEliminados = [];
    $scope.listaMedidadSolEliminados = [];
    $scope.listaCodBarrasSolEliminados = [];
    $scope.listaImagenesSolEliminados = [];
    $scope.listaCatalogosSolEliminados = [];
    $scope.listaCentrosSolEliminados = [];
    $scope.listaAlmacenSolEliminados = [];
    $scope.listaTipoAlmacenESolEliminados = [];
    $scope.listaTipoAlmacenSSolEliminados = [];
    $scope.listaAreaAlmacenSolEliminados = [];

    $scope.validaDatos = false;
    $scope.observEstado = "";
    $scope.EnviaNotificacionSN = true;
    $scope.isAprobado = false;
    $scope.datosAdicionales = [];
    $scope.agrupacion = [];
    $scope.codAgrupacion = {};

    $scope.rolSeleccionado = localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser');

    //Carga Combos






    $scope.myPromise =
     AdmArticuloService.getCatalogo('tbl_MotivoRechazo_Art').then(function (results) {
         $scope.motivoRechazo = results.data;
         $scope.motivoRechazoArt = results.data;
     });
    ;

    $scope.myPromise =
  AdmArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
      $scope.lineaNegocio = results.data;
  });
    ;




















    //Si no es consulta de una solicitud no puede acceder a la pantalla
    if (localStorageService.get('IdSolicitud') === undefined) {
        $scope.MenjError = 'Consulte una solicitud para acceder a esta pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeError').modal('show');
        //window.location = '/Articulos/frmConsSolArticulo';
    }
    if (localStorageService.get('IdSolicitud') === null) {
        $scope.MenjError = 'Consulte una solicitud para acceder a esta pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeError').modal('show');
    }

    $scope.txtIdSolicitud = (localStorageService.get('IdSolicitud') === null ? "0" : localStorageService.get('IdSolicitud'));
    $scope.codSapProv = (localStorageService.get('CodSAPproveedor') === null ? "0" : localStorageService.get('CodSAPproveedor'));
    $scope.txtEstSolicitud = (localStorageService.get('EstSolicitud') === null ? "0" : localStorageService.get('EstSolicitud'));
    $scope.txtTipoSolicitud = (localStorageService.get('tipoSolicitud') === null ? "0" : localStorageService.get('tipoSolicitud'));
    $scope.tipouser = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
    //Limpia
    localStorageService.remove('IdSolicitud');
    localStorageService.remove('EstSolicitud');
    localStorageService.remove('CodSAPproveedor');
    localStorageService.remove('tipoSolicitud');
    localStorageService.remove('tipouser');
    //Muestra u Oculta paneles de botones

    if ($scope.tipouser != "-1") {
        $scope.ViewAsisCompras = ($scope.tipouser === "1" ? false : true);
        $scope.ViewGernCompras = ($scope.tipouser === "2" ? false : true);
        $scope.ViewDatosMaestr = ($scope.tipouser === "3" ? false : true);
    }

    //Cuando entra a la pagina carga los datos de la cosulta
    $scope.CargaConsulta = function () {


        if ($scope.txtIdSolicitud != "0") {

            //tipo, codigo Solo necesita el tipo y el codigo
            $scope.myPromise = AdmArticuloService.getArticulos("2", $scope.txtIdSolicitud, "", "", "", "", "", "", "", "", "", "", "", "", "", authService.authentication.userName, $scope.rolSeleccionado, "").then(function (results) {


                //Linea de Negocio
                var index;
                var linea = {};
                linea = results.data.root[0];
                $scope.datosAdicionales = linea;
                $scope.txtRScocial = linea[0].razonSocial;
                //Validar estados que puede modificar por tipo de usuario
                //Asistente de compras 'EN','EC', 'DG','DD'                
                if (!$scope.ViewAsisCompras) {
                    if (linea[0].estado == 'EN' || linea[0].estado == 'EC' || linea[0].estado == 'DG' || linea[0].estado == 'DD') {
                        $scope.isReadOnly = false;
                        $scope.inactivaBot = false;
                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                }
                //Gerente de compras 'RC'             
                if (!$scope.ViewGernCompras) {
                    if (linea[0].estado == 'RC') {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;
                        $scope.ViewGernCompras = true;

                    }
                }

                //datos maestros  'AG','ED'            
                if (!$scope.ViewDatosMaestr) {
                    if (linea[0].estado == 'AG' || linea[0].estado == 'ED') {
                        $scope.isReadOnly = false;
                        $scope.inactivaBot = false;
                    }
                    else {
                        $scope.isReadOnly = true;
                        $scope.inactivaBot = true;

                    }
                }


                $scope.Det = results.data.root[1];
                $scope.ListacodLegacy = results.data.root[14];
          
                $scope.grArticulo = $scope.Det;

                var auxgrArticulo = $filter('filter')($scope.grArticulo, { codGenerico: "0" }, true);
                $scope.etiTotRegistros = auxgrArticulo.length;

                //for (var i = 0 ; i < $scope.grArticulo.length; i++)
                //{ 
                //    if ($scope.grArticulo[i].estado == 'APD')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Aprobado' }
                //    if ($scope.grArticulo[i].estado == 'RED')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Rechazado' }

                //}


                //bloquear linea de negocio
                if ($scope.grArticulo.length > 0) {
                    $scope.bloqueaBotInicio = true;
                }
                $scope.CabPrincipal = results.data.root[0];
                $scope.Med = results.data.root[2];
                $scope.CBr = results.data.root[3];
                $scope.Ima = results.data.root[4];
                $scope.Rutas = results.data.root[5];
                $scope.Com = results.data.root[6];
                $scope.Cat = results.data.root[7];
                $scope.Alm = results.data.root[8];
                $scope.Iae = results.data.root[9];
                $scope.Ias = results.data.root[10];
                $scope.Iaa = results.data.root[11];

                $scope.grArtObservacion = results.data.root[12];
                $scope.Cen = results.data.root[13];
                $scope.CaractArt = results.data.root[15];

                $scope.lineaCargada = linea[0].lineaNegocio;
                var ln = $scope.lineaNegocio;
                for (index = 0; index < ln.length; ++index) {
                    if (ln[index].codigo === linea[0].lineaNegocio)
                    { $scope.ddlLineaNegocio = ln[index]; }
                }
                //var continuar = true
                //var maxContinuar = 900000;
                ////alert(linea[0].lineaNegocio + '-' + $scope.lineaNegocio.length);
                //while (continuar) {
                //    maxContinuar = maxContinuar -1;
                //    var ln = $scope.lineaNegocio;
                //    for (index = 0; index < ln.length; ++index) {
                //        if (ln[index].codigo === linea[0].lineaNegocio)
                //        { $scope.ddlLineaNegocio = ln[index]; continuar = false; }
                //    }
                //    if(maxContinuar == 0 )
                //        continuar = false;
                //}

                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }, function (error) {
            });

        }
    }







    //Asistente de Compras
    $scope.ObservGrabar = function () {
        //validar que han ingresado todos los precios
        for (var idx = 0; idx < $scope.grArticulo.length; idx++) {

            if ($scope.grArticulo[idx].precioNuevo == "") {

                $scope.MenjError = "Ingrese nuevo precio para el artículo. " + $scope.grArticulo[idx].codSAPart;
                $('#idMensajeInformativo').modal('show');
                return;
            }


            if (!$scope.requerido.txtNuevoPrecio.$valid) {
                $scope.MenjError = "Valor incorrecto en precio a negociar. 99999999.999";
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.grArticulo[idx].precioNuevo == 0) {

                $scope.MenjError = "Ingrese nuevo precio mayor a 0 para el artículo. " + $scope.grArticulo[idx].codSAPart;
                $('#idMensajeInformativo').modal('show');
                return;
            }


        }
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 1;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
        $scope.EnviaNotificacionSN = false;
    }

    $scope.ObservRevisado = function () {

        var exsiteAprobados = true;
        //validar que han ingresado todos los precios
        for (var idx = 0; idx < $scope.grArticulo.length; idx++) {

            if ($scope.grArticulo[idx].precioNuevo == "") {

                $scope.MenjError = "Ingrese nuevo precio para el artículo. " + $scope.grArticulo[idx].codSAPart;
                $('#idMensajeInformativo').modal('show');
                return;
            }


            if (!$scope.requerido.txtNuevoPrecio.$valid) {
                $scope.MenjError = "Valor incorrecto en precio a negociar. 99999999.999";
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.grArticulo[idx].precioNuevo == 0) {

                $scope.MenjError = "Ingrese nuevo precio mayor a 0 para el artículo. " + $scope.grArticulo[idx].codSAPart;
                $('#idMensajeInformativo').modal('show');
                return;
            }


        }


        if (exsiteAprobados == true) {
            $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud? ";
            $scope.accion = 2;

            $scope.ViewMotRechazo = true;
            $scope.observEstado = "Solicitud Aprobada.";
            $scope.ddlMotivoRechazo = {};
            $scope.ddlMotivoRechazo.codigo = 0;
            $scope.ddlMotivoRechazo.detalle = "";
            $scope.ddlMotivoRechazo.codigo = 10;
            $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
            //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
            $('#idMensajeConfirmacion').modal('show');
            //$scope.grabar();
        }
        else {
            $scope.MenjError = "No hay ningún artículo aprobado para enviar la solicitud a revisión. ";
            $('#idMensajeError').modal('show');
            return;
        }

    }

    $scope.ObservDevolver = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 10;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 10;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    $scope.ObservRechazar = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 10;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea rechazar la solicitud? ";
        $scope.accion = 15;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Gerente de Compras
    $scope.ObservAprobar = function () {

        //$scope.MenjError = "¿Está seguro que desea Aprobar la Solicitud?";
        //$scope.accion = 4;
        //$scope.ViewMotRechazo = true;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 4;
        $scope.ViewMotRechazo = true;
        $scope.ddlMotivoRechazo = {};
        $scope.ddlMotivoRechazo.codigo = 0;
        $scope.ddlMotivoRechazo.detalle = "";
        $scope.ddlMotivoRechazo.codigo = 10;
        $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
        $scope.observEstado = "Solicitud Aprobada.";
        //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
        $('#idMensajeConfirmacion').modal('show');

        //$('#modal-form-observacion').modal('show');
    }

    $scope.ObservDevolverGC = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 5;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 5;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Datos Maestros
    $scope.ObservGrabarDM = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 6;
        $scope.ViewMotRechazo = true;
        $scope.EnviaNotificacionSN = false;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservAprobarDM = function () {
        //$scope.MenjError = "¿Está seguro que desea Aprobar la Solicitud?";
        //$scope.accion = 7;
        //$scope.ViewMotRechazo = true;
        //$('#idMensajeConfirmacion').modal('show');
        var exsiteAprobados = false;
        var exsitePendientes = false;
        for (var i = 0; i < $scope.grArticulo.length; i++) {
            if ($scope.grArticulo[i].estado == 'Pendiente')
            { exsitePendientes = true; break; }
            if ($scope.grArticulo[i].estado == 'Aprobado') {
                exsiteAprobados = true;
                if ($scope.grArticulo[i].codSAPart == "") {
                    $scope.MenjError = "Existen artículos sin código SAP ingresado. ";
                    $('#idMensajeError').modal('show');
                    return;

                }
            }

        }
        if (exsitePendientes == true) {
            $scope.MenjError = "Existe artículo pendiente de  revisión. ";
            $('#idMensajeError').modal('show');
            return;
        }




        if (exsiteAprobados == true) {
            $scope.MenjConfirmacion = "¿Está seguro que desea aprobar la solicitud? ";
            $scope.accion = 7;
            $scope.ViewMotRechazo = true;
            $scope.ddlMotivoRechazo = {};
            $scope.ddlMotivoRechazo.codigo = 0;
            $scope.ddlMotivoRechazo.detalle = "";
            $scope.ddlMotivoRechazo.codigo = 10;
            $scope.ddlMotivoRechazo.detalle = "Solicitud revisada";
            $scope.observEstado = "Solicitud Aprobada.";
            //$('#modal-form-observacion').modal('show');idMensajeConfirmacion
            $('#idMensajeConfirmacion').modal('show');
            //$('#modal-form-observacion').modal('show');
        }
        else {
            $scope.MenjError = "No hay ningún artículo aprobado para enviar la solicitud. ";
            $('#idMensajeError').modal('show');
            return;
        }



    }

    $scope.ObservDevolverDM = function () {
        //$scope.MenjError = "Ingrese una Observación";
        //$scope.accion = 8;
        //$scope.ViewMotRechazo = false;
        //$('#idMensajeConfirmacion').modal('show');

        $scope.MenjError = "¿Está seguro que desea enviar la solicitud? ";
        $scope.accion = 8;
        $scope.ViewMotRechazo = true;
        $('#modal-form-observacion').modal('show');
    }

    //Muestra el popup para confimar si desea o no grabar la solicitud
    $scope.ConfirmGrabar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 1;
        $('#idMensajeConfirmacion').modal('show');
    }
    //Muestra el popup para confimar si desea quitar o no articulo
    $scope.ConfirmQuitarArticulo = function (id) {
        $scope.MenjConfirmacion = "¿Está seguro que desea quitar articulo?";
        $scope.accion = 999;
        $scope.CodigoArticulo = id;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Muestra el popup para confimar si desea o no enviar la solicitud
    $scope.ConfirmEnviar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud?";
        $scope.accion = 2;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Evento click boton Grabar
    $scope.grabar = function () {

        if ($scope.accion == 998) {
            $scope.AddArticuloAdmin();
            $('#modal-form-observacion_2').modal('hide');
            return;
        }
        if ($scope.accion == 999) {
            $scope.QuitarArt($scope.CodigoArticulo);
            return;
        }
        //Valida tipo de accion
        if ($scope.accion === 3 || $scope.accion === 9) {
            window.location = '../Articulos/frmConsSolArticulo';
        } else {

            //Valida si selecciono la Linea de Negocio

            var ln = $scope.lineaNegocio;
            for (var index = 0; index < ln.length; ++index) {
                if (ln[index].codigo === $scope.lineaCargada)
                { $scope.ddlLineaNegocio = ln[index]; }
            }


            if ($scope.ddlLineaNegocio === "") {
                $scope.MenjError = "Debe seleccionar una línea de negocio.";
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.ddlLineaNegocio === null) {
                $scope.MenjError = "Debe seleccionar una línea de negocio.";
                $('#idMensajeError').modal('show');
                return;
            }

            //Valida que por lo menos haya un articulo ingresado
            if ($scope.grArticulo.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos un artículo.";
                $('#idMensajeError').modal('show');
                return;
            }

            if ($scope.accion == 2 || $scope.accion == 4 || $scope.accion == 7) {
                $scope.isAprobado = true;
            }

            //Para validar el estado
            var estado = ($scope.accion === 1 ? "grabado" : "enviado");
            var idsolicitud = ($scope.txtIdSolicitud === "" ? "0" : $scope.txtIdSolicitud);

            //Arma la cabecera
            $scope.Cab = [];
            $scope.p_Cab = {};
            $scope.p_Cab.codproveedor = authService.authentication.CodSAP;
            $scope.p_Cab.Usuario = authService.authentication.userName;
            $scope.p_Cab.tiposolicitud = $scope.txtTipoSolicitud;
            $scope.p_Cab.lineanegocio = $scope.ddlLineaNegocio.codigo;
            $scope.p_Cab.idsolicitud = idsolicitud;

            $scope.p_Cab.accion = "U";
            $scope.p_Cab.estado = ($scope.accion === 15 ? "RE" : ($scope.accion === 1 ? "EC" : ($scope.accion === 2 ? "RC" : ($scope.accion === 10 ? "DC" : ($scope.accion === 4 ? "AG" : ($scope.accion === 5 ? "DG" : ($scope.accion === 6 ? "ED" : ($scope.accion === 7 ? "AD" : "DD"))))))));
            $scope.p_Cab.observacion = $scope.observEstado;
            $scope.p_Cab.motivoRechazo = $scope.ddlMotivoRechazo.codigo;
            if ($scope.EnviaNotificacionSN)
                $scope.p_Cab.enviaNotificacion = "S";
            else
                $scope.p_Cab.enviaNotificacion = "N";
            if ($scope.isAprobado)
                $scope.p_Cab.enviaAprobar = "S";
            else
                $scope.p_Cab.enviaAprobar = "N";
            $scope.Cab.push($scope.p_Cab);


            //Solo para administrativo
            //$scope.Com = [];
            //$scope.Cat = [];
            //$scope.Alm = [];
            //$scope.Iae = [];
            //$scope.Ias = [];
            //$scope.Iaa = [];

            //Eliminar detalles de articulos
            for (var i = 0 ; i < $scope.listaDetallesSolEliminados.length; i++)
                $scope.Det[$scope.Det.length] = $scope.listaDetallesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaMedidadSolEliminados.length; i++)
                $scope.Med[$scope.Med.length] = $scope.listaMedidadSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCodBarrasSolEliminados.length; i++)
                $scope.CBr[$scope.CBr.length] = $scope.listaCodBarrasSolEliminados[i];
            for (var i = 0 ; i < $scope.listaImagenesSolEliminados.length; i++)
                $scope.Ima[$scope.Ima.length] = $scope.listaImagenesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCatalogosSolEliminados.length; i++)
                $scope.Cat[$scope.Cat.length] = $scope.listaCatalogosSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCentrosSolEliminados.length; i++)
                $scope.Cen[$scope.Cen.length] = $scope.listaCentrosSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAlmacenSolEliminados.length; i++)
                $scope.Alm[$scope.Alm.length] = $scope.listaAlmacenSolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenESolEliminados.length; i++)
                $scope.Iae[$scope.Iae.length] = $scope.listaTipoAlmacenESolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenSSolEliminados.length; i++)
                $scope.Ias[$scope.Ias.length] = $scope.listaTipoAlmacenSSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAreaAlmacenSolEliminados.length; i++)
                $scope.Iaa[$scope.Iaa.length] = $scope.listaAreaAlmacenSolEliminados[i];
            //Lama el metodo

            $scope.Cab[0].codproveedor = $scope.CabPrincipal[0].codProveedor;

            $scope.myPromise =
            AdmArticuloService.getGrabaSolicitud($scope.Cab, $scope.Det, $scope.Med, $scope.CBr, $scope.Ima, $scope.Com, $scope.Cat,
                                                 $scope.Alm, $scope.Iae, $scope.Ias, $scope.Iaa, $scope.Cen, $scope.CaractArt).then(function (response) {

                                                     if (response.data.success) {
                                                         if ($scope.p_Cab.estado == "AD") {

                                                             $scope.MenjError = 'Artículos grabados correctamente en SAP.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeOk').modal('show');
                                                         }

                                                         else {

                                                             $scope.MenjError = 'Se ha ' + estado + ' exitosamente la solicitud.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeOk').modal('show');
                                                         }
                                                     }
                                                     else {
                                                         if ($scope.p_Cab.estado == "AD") {
                                                             $scope.MenjError = 'Error el grabar artículos en SAP.';
                                                             $scope.accion = 3;
                                                             localStorageService.set('tipouser', $scope.rolSeleccionado);
                                                             $('#idMensajeError').modal('show');


                                                             return;
                                                         }
                                                         else {
                                                             $scope.MenjError = response.data.mensaje;
                                                             $('#idMensajeError').modal('show');
                                                             return;
                                                         }
                                                     }


                                                 },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
            ;
        }
    }



    $("#btnMensajeOK").click(function () {
        if ($scope.accion == 3)
            $scope.grabar();
    });
    $("#btnMsjError").click(function () {
        if ($scope.accion == 3)
            $scope.grabar();
    });



    $("#idMensajeError").click(function () {
        if ($scope.accion == 9)
            window.location = '../Home/Index';
    });

    //A nivel de articulo
    $scope.AprobarArticulo = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea aprobar el artículo?";
        $scope.accionArt = 1;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.RechazarArticulo = function () {
        $scope.MsjConfirmacionArt = "¿Está seguro que desea rechazar el artículo?";
        $scope.accionArt = 2;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = false;
        $('#modal-form-observacion_2').modal('show');
    }



    $scope.AddArticulo = function () {
        $scope.p_Det.descripcion = $scope.p_Det.descripcion.toUpperCase();
        $scope.validaDatos = false;
        //Validar que se han seleccionado todas las unidades de medida
        var exisUMbase = false;
        var exisUMpedido = false;
        var exisUMes = false;
        var exisUMventa = false;

        for (var k = 0 ; k < $scope.grArtMedida.length ; k++) {


            if ($scope.grArtMedida[k].uniMedBase) exisUMbase = true;
            if ($scope.grArtMedida[k].uniMedPedido) exisUMpedido = true;
            if ($scope.grArtMedida[k].uniMedVenta) exisUMventa = true;
            if ($scope.grArtMedida[k].uniMedES) exisUMes = true;
        }


        if (!exisUMbase) {
            $scope.MenjError = "Debe seleccionar Unidad Base en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMpedido) {
            $scope.MenjError = "Debe seleccionar Unidad Pedido en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMes) {
            $scope.MenjError = "Debe seleccionar Unidad E/S en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMventa) {
            $scope.MenjError = "Debe seleccionar Unidad Venta en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        //Validar Canal de distribucion
        if ($scope.grArtIndAreaAlmacen.length == 0) {
            $scope.MenjError = "Debe ingresar al menos un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Validar que se ingrese valores obligatorios
        if ($scope.p_Det.codReferencia == "") {
            $scope.MenjError = "Debe ingresar código de referencia del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlMarca == "" || $scope.ddlMarca == undefined) {
            $scope.MenjError = "Debe ingresar marca del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descripcion == "") {
            $scope.MenjError = "Debe ingresar descripción del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.tamArticulo == "") {
            $scope.MenjError = "Debe ingresar tamaño o presentación del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.precioBruto == "") {
            $scope.MenjError = "Debe ingresar precio bruto del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.ddlIva == "" || $scope.ddlIva == undefined) {
            $scope.MenjError = "Debe ingresar valor de clasificación fiscal.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlDeducible == "" || $scope.ddlDeducible == undefined) {
            $scope.MenjError = "Debe ingresar Deducible.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlRetencion == "" || $scope.ddlRetencion == undefined) {
            $scope.MenjError = "Debe ingresar retención.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida si ha ingresado una medida
        if ($scope.grArtMedida.length === 0) {
            $scope.MenjError = "Debe ingresar por lo menos una unidad de medida del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        var ingresa = false;
        if ($scope.grArticulo.length != 0) {
            for (var i = 0, len = $scope.grArticulo.length; i < len; i++) {
                if ($scope.grArticulo[i].codReferencia === $scope.p_Det.codReferencia) {
                    edita = true;
                    if ($scope.grArticulo[i].accion == "I") ingresa = true;
                    break;
                }
            }
        }



        //Valida que haya ingresado una nueva marca en caso de que haya seleccionado OTROS
        if ($scope.ddlMarca.codigo === "-1" && $scope.Det.marcaNueva === "") {
            $scope.MenjError = "Ingrese la nueva marca del articulo si seleccionó <OTROS>.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Obtiene y carga los datos de los combos
        if ($scope.ddlMarca != null)
            $scope.p_Det.marca = $scope.ddlMarca.codigo;
        else
            $scope.p_Det.marca = "";
        if ($scope.ddlMarca != null)
            $scope.p_Det.desMarca = $scope.ddlMarca.detalle;
        else
            $scope.p_Det.desMarca = "";
        if ($scope.ddlPaisOrigen != null)
            $scope.p_Det.paisOrigen = $scope.ddlPaisOrigen.codigo;
        else
            $scope.p_Det.paisOrigen = "";
        if ($scope.ddlRegionOrigen != null)
            $scope.p_Det.regionOrigen = $scope.ddlRegionOrigen.codigo;
        else
            $scope.p_Det.regionOrigen = "";
        if ($scope.ddlTalla != null)
            $scope.p_Det.talla = $scope.ddlTalla.codigo;
        else
            $scope.p_Det.talla = "";
        if ($scope.ddlGradoAlcohol != null)
            $scope.p_Det.gradoAlcohol = $scope.ddlGradoAlcohol.codigo;
        else
            $scope.p_Det.gradoAlcohol = "";
        if ($scope.ddlColor != null)
            $scope.p_Det.color = $scope.ddlColor.codigo;
        else
            $scope.p_Det.color = "";
        if ($scope.ddlFragancia != null)
            $scope.p_Det.fragancia = $scope.ddlFragancia.codigo;
        else
            $scope.p_Det.fragancia = "";
        if ($scope.ddlTipos != null)
            $scope.p_Det.tipos = $scope.ddlTipos.codigo;
        else
            $scope.p_Det.tipos = "";
        if ($scope.ddlSabor != null)
            $scope.p_Det.sabor = $scope.ddlSabor.codigo;
        else
            $scope.p_Det.sabor = "";
        $scope.p_Det.modelo = $scope.ddlModelo;
        //if ($scope.ddlModelo != null)
        //    $scope.p_Det.modelo = $scope.ddlModelo.codigo;
        //else
        //    $scope.p_Det.modelo = "";
        if ($scope.ddlColeccion != null)
            $scope.p_Det.coleccion = $scope.ddlColeccion.codigo;
        else
            $scope.p_Det.coleccion = "";
        if ($scope.ddlTemporada != null)
            $scope.p_Det.temporada = $scope.ddlTemporada.codigo;
        else
            $scope.p_Det.temporada = "";
        $scope.p_Det.estacion = $scope.ddlEstacion;
        $scope.p_Det.cantidadPedir = $scope.ddlCantidadPedir;

        if ($scope.ddlIva != null)
            $scope.p_Det.iva = $scope.ddlIva.codigo;
        else
            $scope.p_Det.iva = "";
        if ($scope.ddlDeducible != null)
            $scope.p_Det.deducible = $scope.ddlDeducible.codigo;
        else
            $scope.p_Det.deducible = "";
        if ($scope.ddlRetencion != null)
            $scope.p_Det.retencion = $scope.ddlRetencion.codigo;
        else
            $scope.p_Det.retencion = "";


        $scope.p_Det.accion = "I";
        $scope.p_Det.estado = "NAR";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.Det.length; i < len; i++) {
                var update = $scope.Det[i];
                if (update.codReferencia === $scope.p_Det.codReferencia) {
                    update.marca = $scope.p_Det.marca;
                    update.DesMarca = $scope.p_Det.desMarca;
                    update.paisOrigen = $scope.p_Det.paisOrigen;
                    update.regionOrigen = $scope.p_Det.regionOrigen;
                    update.tamArticulo = $scope.p_Det.tamArticulo;
                    update.gradoAlcohol = $scope.p_Det.gradoAlcohol;
                    update.talla = $scope.p_Det.talla;
                    update.color = $scope.p_Det.color;
                    update.fragancia = $scope.p_Det.fragancia;
                    update.tipos = $scope.p_Det.tipos;
                    update.sabor = $scope.p_Det.sabor;
                    update.modelo = $scope.p_Det.modelo;
                    update.precioBruto = $scope.p_Det.precioBruto;
                    update.descuento1 = $scope.p_Det.descuento1;
                    update.descuento2 = $scope.p_Det.descuento2;
                    update.impVerde = $scope.p_Det.impVerde;
                    update.coleccion = $scope.p_Det.coleccion;
                    update.temporada = $scope.p_Det.temporada;
                    update.estacion = $scope.p_Det.estacion;
                    update.cantidadPedir = $scope.p_Det.cantidadPedir;
                    update.iva = $scope.p_Det.iva;
                    update.deducible = $scope.p_Det.deducible;
                    update.retencion = $scope.p_Det.retencion;
                    update.descripcion = $scope.p_Det.descripcion;
                    update.codSAPart = $scope.p_Com.codSap;
                    update.otroId = $scope.p_Det.otroId;
                    update.observacion = $scope.observEstado;
                    update.contAlcohol = $scope.p_Det.contAlcohol;



                    if (ingresa)
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "I");
                    else
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Agrega nuevo articulo
            $scope.Det.push($scope.p_Det);
        }

        //Agrega el articulo y carga el grid
        $scope.grArticulo = $scope.Det;
        if ($scope.grArticulo.length > 0) {
            $scope.bloqueaBotInicio = true;
        }
        //$scope.Limpia();
        $scope.inactivaTab = true;
        $scope.validaDatos = true;

        //Eliminar caracteristicas 
        var seguir = true;
        while (seguir) {
            seguir = false;
            for (var ini = 0 ; ini < $scope.CaractArt.length; ini++) {
                if ($scope.CaractArt[ini].idDetalle == $scope.p_Det.idDetalle) {
                    seguir = true;
                    break;
                }
            }
            $scope.CaractArt.splice(ini, 1);
        }

        for (var ini = 0 ; ini < $scope.CaractArtTMP.length; ini++) {
            $scope.p_CaractArt = {};
            $scope.p_CaractArt.idDetalle = $scope.CaractArtTMP[ini].idDetalle;
            $scope.p_CaractArt.idCaract = $scope.CaractArtTMP[ini].idCaract;
            $scope.p_CaractArt.idValor = $scope.CaractArtTMP[ini].idValor;
            $scope.p_CaractArt.accion = $scope.CaractArtTMP[ini].accion;
            $scope.p_CaractArt.idAgrupacion = $scope.CaractArtTMP[ini].idAgrupacion;

            $scope.CaractArt.push($scope.p_CaractArt);
        }
        $scope.CaractArtTMP = [];

    }

    $scope.AddArticuloAdmin = function () {

        $scope.AddArticulo();
        if (!$scope.validaDatos) return;
        //Carga los datos de Compras
     


        $scope.p_Com.idDetalle = $scope.p_Det.idDetalle;


        //Modifica los datos de compras
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            var update = $scope.Com[i];
            if (update.idDetalle === $scope.p_Com.idDetalle) {
                //update.codLegacyProv = $scope.p_Com.codLegacy.codigo;
                if ($scope.p_Com.codLegacy != null)
                    update.codLegacyProv = $scope.p_Com.codLegacy.codigo;
                else
                    update.codLegacyProv = "";

                update.costoFOB = $scope.p_Com.costoFOB;
                update.observaciones = $scope.p_Com.observaciones;
                //Nuevos
                //update.jerarquiaProd = $scope.p_Com.jerarquiaProd;
                if ($scope.ddlNacionalImportado != null)
                    update.jerarquiaProd = $scope.ddlNacionalImportado.codigo;
                else
                    update.jerarquiaProd = "";
                update.susceptBonifEsp = $scope.p_Com.susceptBonifEsp;
                update.procedimCatalog = $scope.p_Com.procedimCatalog;
                update.caracterPlanNec = $scope.p_Com.caracterPlanNec;
                update.fuenteProvision = $scope.p_Com.fuenteProvision;
                if ($scope.ddlOrgCompras != null) {
                    update.organizacionCompras = $scope.ddlOrgCompras.codigo;
                    update.organizacionComprasDes = $scope.ddlOrgCompras.detalle;
                }
                else {
                    update.organizacionCompras = "";
                    update.organizacionComprasDes = "";
                }
                if ($scope.ddlFrecEntrega != null)
                    update.frecuenciaEntrega = $scope.ddlFrecEntrega.codigo;
                else
                    update.frecuenciaEntrega = "";
                if ($scope.ddlTipoMaterial != null) {
                    update.tipoMaterial = $scope.ddlTipoMaterial.codigo;
                    update.tipoMaterialDes = $scope.ddlTipoMaterial.detalle;
                }
                else { update.tipoMaterial = ""; update.tipoMaterialDes = ""; }
                if ($scope.ddlCatMaterial != null) {
                    update.categoriaMaterial = $scope.ddlCatMaterial.codigo;
                    update.categoriaMaterialDes = $scope.ddlCatMaterial.detalle;
                }
                else { update.categoriaMaterial = ""; update.categoriaMaterialDes = ""; }
                if ($scope.ddlGrupoArticulo != null)
                    update.grupoArticulo = $scope.ddlGrupoArticulo.codigo;
                else
                    update.grupoArticulo = "";
                if ($scope.ddlSeccArticulo != null)
                    update.seccionArticulo = $scope.ddlSeccArticulo.codigo;
                else
                    update.seccionArticulo = "";
                //update.catalogacion = $scope.ddlCatalogacion.codigo;
                if ($scope.ddlSurtParcial != null)
                    update.surtidoParcial = $scope.ddlSurtParcial.codigo;
                else
                    update.surtidoParcial = "";
                if ($scope.ddlMateria != null)
                { update.materia = $scope.ddlMateria.codigo; update.materiaDes = $scope.ddlMateria.detalle; }
                else
                { update.materia = ""; update.materiaDes = ""; }
                if ($scope.ddlIndPedido != null)
                    update.indPedido = $scope.ddlIndPedido.codigo;
                else
                    update.indPedido = "";
                if ($scope.ddlPerfDistribucion != null)
                    update.perfilDistribucion = $scope.ddlPerfDistribucion.codigo;
                else
                    update.perfilDistribucion = "";
                //update.almacen = $scope.ddlAlmacen.codigo;
                if ($scope.ddlGrupoCompras != null)
                    update.grupoCompra = $scope.ddlGrupoCompras.codigo;
                else
                    update.grupoCompra = "";
                if ($scope.ddlCatValoracion != null)
                    update.categoriaValoracion = $scope.ddlCatValoracion.codigo;
                else
                    update.categoriaValoracion = "";
                if ($scope.ddlTipoAlmacenBod != null)
                    update.tipoAlamcen = $scope.ddlTipoAlmacenBod.codigo;
                else
                    update.tipoAlamcen = "";
                //update.indalmaentrada = $scope.ddlIndTipoAlmEnt.codigo;
                //update.indalmasalida = $scope.ddlIndTipoAlmSal.codigo;
                //update.indareaalmacen = $scope.ddlIndAreaAlmacen.codigo;
                if ($scope.ddlCondAlmacen != null)
                    update.condicionAlmacen = $scope.ddlCondAlmacen.codigo;
                else
                    update.condicionAlmacen = "";
                if ($scope.ddlClListSurtidos != null)
                    update.clListaSurtido = $scope.ddlClListSurtidos.codigo;
                else
                    update.clListaSurtido = "";
                if ($scope.ddlEstatusMaterial != null)
                    update.estatusMaterial = $scope.ddlEstatusMaterial.codigo;
                else
                    update.estatusMaterial = "";
                if ($scope.ddlEstatusVentas != null)
                    update.estatusVenta = $scope.ddlEstatusVentas.codigo;
                else
                    update.estatusVenta = "";
                if ($scope.ddlGrupoBalanzas != null)
                    update.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
                else
                    update.grupoBalanzas = "";
                if ($scope.ddlNacionalImportado != null)
                    update.nacionalImportado = $scope.ddlNacionalImportado.codigo;
                else
                    update.nacionalImportado = "";
                //if ($scope.ddlColeccion != null)
                //    update.coleccion = $scope.ddlColeccion.codigo;
                //else
                //    update.coleccion = "";
                //if ($scope.ddlTemporada != null)
                //    update.temporada = $scope.ddlTemporada.codigo;
                //else
                //    update.temporada = "";
                //update.estacion = $scope.ddlEstacion;
                //update.cantidadPedir = $scope.ddlCantidadPedir;
                //if ($scope.ddlEstacion != null)
                //    update.estacion = $scope.ddlEstacion.codigo;
                //else
                //    update.estacion = "";
                if ($scope.ddlMotivoRechazoArt != null)
                    update.motivoRechazo = $scope.ddlMotivoRechazoArt.codigo;
                else
                    update.motivoRechazo = "";

                update.observacion = $scope.observEstado;
                var d_fecha = "";
                if ($scope.p_Com.validoDesde != "")
                { d_fecha = $filter('date')($scope.p_Com.validoDesde, 'MM/dd/yyyy'); }
                update.validoDesde = d_fecha;
                //1: Asistente Compras //3: Datos Maestros
                update.estado = ($scope.accionArt === 1 ? ($scope.tipouser === "1" ? "APC" : "APD") : ($scope.tipouser === "1" ? "REC" : "RED"));
                for (var i = 0 ; i < $scope.grArticulo.length; i++) {
                    if ($scope.grArticulo[i].idDetalle == $scope.p_Com.idDetalle)
                        break;
                }

                if (update.estado == 'APC' || update.estado == 'APD')
                    $scope.grArticulo[i].estado = 'Aprobado';
                if (update.estado == 'REC' || update.estado == 'RED')
                    $scope.grArticulo[i].estado = 'Rechazado';
                update.accion = "U";
                break;
            }
        }



        //Agrega el articulo y carga el grid
        //$scope.grArticulo = $scope.Det;
        $scope.Limpia();
        $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
    }



    $scope.NewArticulo = function () {

        $scope.bloqueaCodReferencia = false;
        //Validar que haya seleccionado una linea de negocio
        if ($scope.ddlLineaNegocio === "") {
            $scope.MenjError = "Debe seleccionar la línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlLineaNegocio === null) {
            $scope.MenjError = "Debe seleccionar la línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }

        $scope.inactivaTab = false;

        $scope.Limpia();
        $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
        //Para setear el path temporal para las imagenes
        var secuencia = "";
        AdmArticuloService.getSecuenciaDirectorio("Notificacion").then(function (results) {
            if (results.data.success) {
                secuencia = results.data.root[0];
                var direc = "Art_" + secuencia;

                //Graba rutas por articulos
                $scope.Ruta = {};
                $scope.Ruta.idDetalle = $scope.p_Det.idDetalle;
                $scope.Ruta.path = direc;
                $scope.Rutas.push($scope.Ruta);

                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                $scope.uploader2.url = Ruta;
            }
        }, function (error) {
        });
        //Genera nuevo codigo de referencia

        if ($scope.grArticulo.length > 0) {
            var len = $scope.grArticulo.length - 1;
            $scope.p_Det.idDetalle = parseInt($scope.grArticulo[len].idDetalle) + 1;
        }
        else
            $scope.p_Det.idDetalle = $scope.grArticulo.length + 1;

    }

    $scope.Limpia = function () {
        //Detalle
        $scope.p_Det = {};
        $scope.p_Det.idDetalle = "";
        $scope.p_Det.codReferencia = "";
        $scope.p_Det.marca = "";
        $scope.p_Det.desMarca = "";
        $scope.ddlMarca = "";
        $scope.p_Det.paisOrigen = "";
        $scope.ddlPaisOrigen = "";
        $scope.p_Det.regionOrigen = "";
        $scope.ddlRegionOrigen = "";
        $scope.p_Det.gradoAlcohol = "";
        $scope.ddlGradoAlcohol = "";
        $scope.p_Det.talla = "";
        $scope.ddlTalla = "";
        $scope.p_Det.color = "";
        $scope.ddlColor = "";
        $scope.p_Det.fragancia = "";
        $scope.ddlFragancia = "";
        $scope.p_Det.tipos = "";
        $scope.ddlTipos = "";
        $scope.p_Det.sabor = "";
        $scope.ddlSabor = "";
        $scope.p_Det.modelo = "";
        $scope.ddlModelo = "";
        $scope.p_Det.coleccion = "";
        $scope.ddlColeccion = "";
        $scope.p_Det.temporada = "";
        $scope.ddlTemporada = "";
        $scope.p_Det.estacion = "";
        $scope.ddlEstacion = "";
        $scope.ddlCantidadPedir = "";
        $scope.p_Det.iva = "";
        $scope.ddlIva = "";
        $scope.p_Det.deducible = "";
        $scope.ddlDeducible = "";
        $scope.p_Det.retencion = "";
        $scope.ddlRetencion = "";
        $scope.p_Det.descripcion = "";
        $scope.p_Det.otroId = "";
        $scope.p_Det.tamArticulo = "";
        $scope.p_Det.contAlcohol = false;
        $scope.p_Det.accion = "";
        $scope.p_Det.estado = "";
        $scope.p_Det.precioBruto = "";
        $scope.p_Det.descuento1 = "";
        $scope.p_Det.descuento2 = "";
        $scope.p_Det.impVerde = "";
        //Compras
        $scope.p_Com = {};
        $scope.p_Com.idDetalle = "";
        $scope.p_Com.codSap = "";
        $scope.p_Com.codLegacy = "";
        $scope.p_Com.costoFOB = "";
        $scope.p_Com.validoDesde = "";
        $scope.p_Com.observaciones = "";
        $scope.p_Com.organizacionCompras = "";
        $scope.p_Com.organizacionComprasDes = "";
        $scope.p_Com.frecuenciaEntrega = "";
        $scope.p_Com.tipoMaterial = "";
        $scope.p_Com.tipoMaterialDes = "";
        $scope.p_Com.categoriaMaterial = "";
        $scope.p_Com.categoriaMaterialDes = "";
        $scope.p_Com.grupoArticulo = "";
        $scope.p_Com.seccionArticulo = "";
        $scope.p_Com.catalogacion = "";
        $scope.p_Com.surtidoParcial = "";
        $scope.p_Com.materia = "";
        $scope.p_Com.indPedido = "";
        $scope.p_Com.perfilDistribucion = "";
        $scope.p_Com.almacen = "";
        $scope.p_Com.grupoCompra = "";
        $scope.p_Com.categoriaValoracion = "";
        $scope.p_Com.tipoAlamcen = "";
        $scope.p_Com.indalmaentrada = "";
        $scope.p_Com.indalmasalida = "";
        $scope.p_Com.indareaalmacen = "";
        $scope.p_Com.condicionAlmacen = "";
        $scope.p_Com.clListaSurtido = "";
        $scope.p_Com.estatusMaterial = "";
        $scope.p_Com.estatusVenta = "";
        $scope.p_Com.grupoBalanzas = "";
        $scope.p_Com.nacionalImportado = "";
        $scope.p_Com.coleccion = "";
        $scope.p_Com.temporada = "";
        $scope.p_Com.estacion = "";
        $scope.p_Com.estado = "";
        $scope.p_Com.motivoRechazo = "";
        //Combos
        $scope.ddlOrgCompras = "";
        $scope.ddlFrecEntrega = "";
        $scope.ddlTipoMaterial = "";
        $scope.ddlCatMaterial = "";
        $scope.ddlGrupoArticulo = "";
        $scope.ddlSeccArticulo = "";
        $scope.ddlCatalogacion = "";
        $scope.ddlSurtParcial = "";
        $scope.ddlMateria = "";
        $scope.ddlMateriaDes = "";
        $scope.ddlIndPedido = "";
        $scope.ddlPerfDistribucion = "";
        $scope.ddlAlmacen = "";
        $scope.ddlGrupoCompras = "";
        $scope.ddlCatValoracion = "";
        $scope.ddlTipoAlmacen = "";
        $scope.ddlIndTipoAlmEnt = "";
        $scope.ddlIndTipoAlmSal = "";
        $scope.ddlIndAreaAlmacen = "";
        $scope.ddlCondAlmacen = "";
        $scope.ddlClListSurtidos = "";
        $scope.ddlEstatusMaterial = "";
        $scope.ddlEstatusVentas = "";
        $scope.ddlGrupoBalanzas = "";
        $scope.ddlNacionalImportado = "";
        $scope.ddlColeccion = "";
        $scope.ddlTemporada = "";
        $scope.ddlEstacion = "";
        $scope.ddlCantidadPedir = "";
        //Catalogacion
        $scope.p_Cat = {};
        $scope.grArtCatalogacion = [];
        $scope.p_Cat.idDetalle = "";
        $scope.p_Cat.catalogacion = "";
        $scope.p_Cat.desCatalogacion = "";
        $scope.p_Cat.accion = "";
        //Almacen

        $scope.p_Alm = {};
        $scope.grArtAlmacen = [];
        $scope.p_Alm.idDetalle = "";
        $scope.p_Alm.almacen = "";
        $scope.p_Alm.desAlmacen = "";
        $scope.p_Alm.accion = "";
        //IndTipoAlmEnt
        $scope.p_Iae = {};
        $scope.grArtIndTipoAlmEnt = [];
        $scope.p_Iae.idDetalle = "";
        $scope.p_Iae.indTipoAlmEnt = "";
        $scope.p_Iae.desIndTipoAlmEnt = "";
        $scope.p_Iae.accion = "";
        //IndTipoAlmSal
        $scope.p_Ias = {};
        $scope.grArtIndTipoAlmSal = [];
        $scope.p_Ias.idDetalle = "";
        $scope.p_Ias.indTipoAlmSal = "";
        $scope.p_Ias.desIndTipoAlmSal = "";
        $scope.p_Ias.accion = "";
        //IndAreaAlmacen
        $scope.p_Iaa = {};
        $scope.grArtIndAreaAlmacen = [];
        $scope.p_Iaa.idDetalle = "";
        $scope.p_Iaa.indAreaAlmacen = "";
        $scope.p_Iaa.desIndAreaAlmacen = "";
        $scope.p_Iaa.accion = "";
        //Medida
        $scope.p_Med = {};
        $scope.p_Med.idDetalle = "";
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.ddlUnidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUniMedConvers = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = "";
        $scope.p_Med.ancho = "";
        $scope.p_Med.altura = "";
        $scope.p_Med.volumen = "";
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.impVerde = false;
        $scope.p_Med.medBase = false;
        $scope.p_Med.medPedido = false;
        $scope.p_Med.medES = false;
        $scope.p_Med.medVenta = false;
        $scope.p_Med.accion = "";
        $scope.p_Med.estado = "";
        //Codigo Barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = "";
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.p_CBr.accion = "";
        //Imagenes
        $scope.p_Ima = {};
        $scope.p_Ima.idDetalle = "";
        $scope.p_Ima.path = "";
        $scope.p_Ima.idDocAdjunto = "";
        $scope.p_Ima.nomArchivo = "";
        $scope.p_Ima.accion = "";
        //Limpia Grids
        $scope.grArtMedida = [];
        $scope.grArtCodBarra = [];
        $scope.grArtImagen = [];

        $scope.ddlAgrupacion = "";
        $scope.CaractArtTMP = [];
    }





    $scope.CargaConsulta();
}
]);

'use strict';
app.controller('ConsArticuloController', ['$scope', '$location', '$http', 'ConsArticuloService', 'ngAuthSettings', 'localStorageService', 'authService', function ($scope, $location, $http, ConsArticuloService, ngAuthSettings, localStorageService, authService) {

    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Mensaje ERROR
    $scope.MenjError = "";

    //Variable de Grid
    $scope.GridArticulo = [];
    var _GridArticulo = [];
    $scope.pagesArt = [];
    $scope.pageContentArt = [];

    //DataSources
    $scope.GrupoArticuloDS = [];
    $scope.LineaNegocio = [];
    $scope.GrupoArticuloIni = [];
    //Para los controles
    $scope.ddlLineaNegocio = "";
    $scope.rdbCodigo = "1";
    $scope.rdbProv = "1";
    $scope.txtCodReferencia = "";
    $scope.txtCodSap = "";
    $scope.etiTotRegistros = "";
    $scope.txtCodProveedor = "";
    //Para multiseleccion de estados
    $scope.GrupoArticulo = [];
    $scope.SettingGrupoArt = { selectionLimit: '1', displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };


    ConsArticuloService.getCatalogo('tbl_GrupoArticulo').then(function (results) {
        $scope.GrupoArticuloIni = results.data;
    });

    ConsArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.LineaNegocio = results.data;
    });

    $scope.$watch('ddlLineaNegocio', function () {
 
        $scope.GrupoArticuloDS = [];
        var a = $scope.GrupoArticuloIni;
        var index;
        for (index = 0; index < a.length; ++index) {
            if (a[index].codigo.charAt(0) == $scope.ddlLineaNegocio.codigo) {
                //Agrega los grupos por linea de negocio
                //$scope.gra = { codigo: a[index].codigo, detalle: a[index].detalle, descAlterno: '' };
                $scope.GrupoArticuloDS.push(a[index]);
            }
        }
    });


    $scope.CargaConsultaArt = function () {

        //Linea Negocio
        if ($scope.ddlLineaNegocio === "") {
            $scope.MenjError = "Debe seleccionar una línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlLineaNegocio === undefined) {
            $scope.MenjError = "Debe seleccionar una línea de negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
       
        var chkCodProv = ($scope.rdbProv === "1" ? "true" : "false");
        if (chkCodProv === "true" && $scope.txtCodProveedor === "") {
            $scope.MenjError = "Ingrese el código de proveedor.";
            $('#idMensajeError').modal('show');
            return;
        }
           

        //Para armar arreglo de estados seleccionados
        var chkCodRef = ($scope.rdbCodigo === "1" ? "true" : "false");
        if (chkCodRef === "true" && $scope.txtCodReferencia === "") {
            $scope.MenjError = "Ingrese el código de referencia.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkCodSap = ($scope.rdbCodigo === "3" ? "true" : "false");
        if (chkCodSap === "true" && $scope.txtCodSap === "") {
            $scope.MenjError = "Ingrese el código SAP.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkGrupoArt = ($scope.rdbCodigo === "4" ? "true" : "false");
        if (chkGrupoArt === "true" && $scope.GrupoArticulo.length === 0) {
            $scope.MenjError = "Debe seleccionar por lo menos un grupo de artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        var index;
        var myarray = new Array();
        var a = $scope.GrupoArticulo;
        for (index = 0; index < a.length; ++index) {
            myarray[index] = a[index].id;
        }

        //tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkGrupoArt, GrupoArt
        $scope.myPromise = ConsArticuloService.getArticulos("1", "0", chkCodRef, $scope.txtCodReferencia, chkCodSap, $scope.txtCodSap, chkGrupoArt, myarray, "true", $scope.ddlLineaNegocio.codigo, "", "0", $scope.txtCodProveedor).then(function (results) {
            if (results.data.success) {
       

                $scope.Datos = [];
                $scope.DatosIni = results.data.root[0];

                for (var i = 0, len = $scope.DatosIni.length; i < len; i++) {
                    $scope.p_Dato = {};
                    $scope.p_Dato.chekeado = false;
                    $scope.p_Dato.codSap = $scope.DatosIni[i].codSap;
                    $scope.p_Dato.codReferencia = $scope.DatosIni[i].codReferencia;
                    $scope.p_Dato.marca = $scope.DatosIni[i].marca;
                    $scope.p_Dato.descripcion = $scope.DatosIni[i].descripcion;
                    $scope.p_Dato.contacto = $scope.DatosIni[i].contacto;
                    $scope.p_Dato.email = $scope.DatosIni[i].email;
                    $scope.p_Dato.paisOrigen = $scope.DatosIni[i].paisOrigen;
                    $scope.p_Dato.tipoArticulo = $scope.DatosIni[i].tipoArticulo;
                    $scope.Datos.push($scope.p_Dato);
                }
                $scope.GridArticulo = $scope.Datos;
                $scope.etiTotRegistros = $scope.GridArticulo.length;
                if ($scope.etiTotRegistros == 0) {
                    $scope.MenjError = "No existe resultado para su consulta.";
                    $('#idMensajeInformativo').modal('show');
                    $scope.GridArticulo = [];
                    return;
                }
                setTimeout(function () { $('#btnConsulta').focus(); }, 100);
                setTimeout(function () { $('#rbtArtRef').focus(); }, 150);
            }
            else {
                $scope.MenjError = results.data.mensaje;
                $('#idMensajeError').modal('show');
                return;
            }

        }, function (error) {
        });
    }

    $scope.ModificarArt = function () {
        
        $scope.Ids = [];
        var aux = true;
        for (var i = 0, len = $scope.GridArticulo.length; i < len; i++) {
            if ($scope.GridArticulo[i].chekeado) {
                $scope.p_Ids = {};
                $scope.p_Ids.id = $scope.GridArticulo[i].codSap;
                $scope.Ids.push($scope.p_Ids);
                aux = false;
            }
        }
        if (aux) {
            $scope.MenjError = "Debe seleccionar por lo menos un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        localStorageService.set('CodSapArticulo', $scope.Ids);
        localStorageService.set('CodSapProveedor', $scope.txtCodProveedor);
        localStorageService.set('TipoMod', '2');
        window.location = '/Articulos/frmModificaArticulo';
    }

    $scope.Seleccion = function (Id, codSapProv) {
        localStorageService.set('CodSapArticulo', Id);
        localStorageService.set('CodSapProveedor', $scope.txtCodProveedor);
        localStorageService.set('TipoMod', '1');
        window.location = 'frmModificaArticulo';
    }

    //$scope.CargaConsulta();

}
]);

//Pantalla de Modificacion de Articulos
'use strict';
app.controller('ModArticuloController', ['$scope', '$location', '$http', 'ModArticuloService', 'IngArticuloService', 'ConsArticuloService', 'ngAuthSettings', '$filter', 'FileUploader', '$routeParams', 'localStorageService', 'authService', function ($scope, $location, $http, ModArticuloService, IngArticuloService, ConsArticuloService, ngAuthSettings, $filter, FileUploader, $routeParams, localStorageService, authService) {

    //Loading....
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.datosArtModifica = [];
    //Grids
    $scope.grArticulo = [];
    $scope.grArticuloDet = [];
    $scope.grArtMedida = [];
    $scope.grArtCodBarra = [];
    $scope.grArtImagen = [];
    //Administrativo
    $scope.grArtCatalogacion = [];
    $scope.grArtAlmacen = [];
    $scope.grArtIndTipoAlmEnt = [];
    $scope.grArtIndTipoAlmSal = [];
    $scope.grArtIndAreaAlmacen = [];
    $scope.grArtObservacion = [];
    $scope.indAreaAlmNew = [];
    $scope.canalDistibucion = [];
    
    //DataSource Combos
    $scope.lineaNegocio = [];
    $scope.marca = [];
    $scope.paisOrigen = [];
    $scope.regionOrigen = [];
    $scope.regionOrigenTemp = [];
    $scope.talla = [];
    $scope.gradoAlcohol = [];
    $scope.color = [];
    $scope.fragancia = [];
    $scope.tipos = [];
    $scope.sabor = [];
    $scope.modelo = [];
    $scope.coleccion = [];
    $scope.temporada = [];
    $scope.estacion = [];
    //De uno a muchos
    $scope.catalogacion = [];
    $scope.almacen = [];
    $scope.indTipoAlmEnt = [];
    $scope.indTipoAlmSal = [];
    $scope.indAreaAlmacen = [];
    $scope.tipoEANart = [];
    $scope.unidadMedida = [];
    $scope.catalogoVolumen = [];
    $scope.tipoUnidadMedida = [];
    $scope.uniMedConvers = [];
    $scope.clasificacionFiscal = [];
    $scope.ddlVolumen = [];
    //SI/NO Deducible y Retención
    $scope.sn_ded = [];
    $scope.sn_ret = [];
    $scope.SINO = [];
    $scope.sn = { codigo: 'S', detalle: 'SI' };
    $scope.SINO.push($scope.sn);
    $scope.sn = { codigo: 'N', detalle: 'NO' };
    $scope.SINO.push($scope.sn);
    //$scope.sn_ded = $scope.SINO;
    //$scope.sn_ret = $scope.SINO;

    //modelo caracteristicas
    $scope.CaractArt = [];
    $scope.CaractArtTMP = [];
    $scope.p_CaractArt = {};
    $scope.p_CaractArt.idDetalle = "";
    $scope.p_CaractArt.idCaract = "";
    $scope.p_CaractArt.idValor = "";
    $scope.p_CaractArt.idAgrupacion = "";
    $scope.p_CaractArt.accion = "";
    $scope.idSeleccionado = "";
    //modelo Detalle
    $scope.Det = [];
    $scope.p_Det = {};
    $scope.p_Det.idDetalle = "";
    $scope.p_Det.codReferencia = "";
    $scope.p_Det.marca = "";
    $scope.p_Det.desMarca = "";
    $scope.ddlMarca = "";
    $scope.p_Det.paisOrigen = "";
    $scope.ddlPaisOrigen = "";
    $scope.p_Det.regionOrigen = "";
    $scope.ddlRegionOrigen = "";
    $scope.p_Det.gradoAlcohol = "";
    $scope.ddlGradoAlcohol = "";
    $scope.p_Det.talla = "";
    $scope.ddlTalla = "";
    $scope.p_Det.color = "";
    $scope.ddlColor = "";
    $scope.p_Det.fragancia = "";
    $scope.ddlFragancia = "";
    $scope.p_Det.tipos = "";
    $scope.ddlTipos = "";
    $scope.p_Det.sabor = "";
    $scope.ddlSabor = "";
    $scope.p_Det.modelo = "";
    $scope.ddlModelo = "";
    $scope.p_Det.coleccion = "";
    $scope.ddlColeccion = "";
    $scope.p_Det.temporada = "";
    $scope.ddlTemporada = "";
    $scope.p_Det.estacion = "";
    $scope.ddlCantidadPedir = "";
    $scope.ddlEstacion = "";
    $scope.p_Det.iva = "";
    $scope.ddlIva = "";
    $scope.p_Det.deducible = "";
    $scope.ddlDeducible = "";
    $scope.p_Det.retencion = "";
    $scope.ddlRetencion = "";
    $scope.p_Det.descripcion = "";
    $scope.p_Det.marcaNueva = "";
    $scope.p_Det.viewMarcaNueva = true; //Para activar o inactivar el campo
    $scope.p_Det.tamArticulo = "";
    $scope.p_Det.otroId = "";
    $scope.p_Det.contAlcohol = false;
    $scope.verDetVariantes = false;
    $scope.p_Det.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Det.accion = "";
    $scope.p_Det.precioBruto = "";
    $scope.p_Det.descuento1 = "";
    $scope.p_Det.descuento2 = "";
    $scope.p_Det.impVerde = false;
    $scope.codSapProv = "";
    $scope.codSapArt = "";
    //modelo Detalle de actual articulo
    $scope.Det2 = [];
    $scope.p_Det2 = {};
    $scope.p_Det2.idDetalle = "";
    $scope.p_Det2.codReferencia = "";
    $scope.p_Det2.marca = "";
    $scope.p_Det2.desMarca = "";
    $scope.ddlMarca2 = "";
    $scope.p_Det2.paisOrigen = "";
    $scope.ddlPaisOrigen2 = "";
    $scope.p_Det2.regionOrigen = "";
    $scope.ddlRegionOrigen = "";
    $scope.p_Det2.gradoAlcohol = "";
    $scope.ddlGradoAlcohol2 = "";
    $scope.p_Det2.talla = "";
    $scope.ddlTalla2 = "";
    $scope.p_Det2.color = "";
    $scope.ddlColor2 = "";
    $scope.p_Det2.fragancia = "";
    $scope.ddlFragancia2 = "";
    $scope.p_Det2.tipos = "";
    $scope.ddlTipos2 = "";
    $scope.p_Det2.sabor = "";
    $scope.ddlSabor2 = "";
    $scope.p_Det2.modelo = "";
    $scope.ddlModelo2 = "";
    $scope.p_Det2.coleccion = "";
    $scope.ddlColeccion2 = "";
    $scope.p_Det2.temporada = "";
    $scope.ddlTemporada2 = "";
    $scope.p_Det2.estacion = "";
    $scope.ddlEstacion2 = "";
    $scope.p_Det2.iva = "";
    $scope.ddlIva2 = "";
    $scope.p_Det2.deducible = "";
    $scope.ddlDeducible = "";
    $scope.p_Det2.retencion = "";
    $scope.ddlRetencion = "";
    $scope.p_Det2.descripcion = "";
    $scope.p_Det2.marcaNueva = "";
    $scope.p_Det2.viewMarcaNueva = true; //Para activar o inactivar el campo
    $scope.p_Det2.tamArticulo = "";
    $scope.p_Det2.otroId = "";
    $scope.p_Det2.contAlcohol = false;
    $scope.p_Det2.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Det2.accion = "";

    //modelo Medida
    $scope.Med = [];
    $scope.p_Med = {};
    $scope.p_Med.idDetalle = "";
    $scope.p_Med.unidadMedida = "";
    $scope.p_Med.desUnidadMedida = "";
    $scope.ddlUnidadMedida = "";
    $scope.p_Med.tipoUnidadMedida = "";
    $scope.p_Med.desTipoUnidadMedida = "";
    $scope.ddlTipoUnidadMedida = "";
    $scope.p_Med.uniMedConvers = "";
    $scope.p_Med.desUniMedConvers = "";
    $scope.ddlUniMedConvers = "";
    $scope.p_Med.factorCon = "";
    $scope.p_Med.pesoNeto = "";
    $scope.p_Med.pesoBruto = "";
    $scope.p_Med.longitud = 0;
    $scope.p_Med.ancho = 0;
    $scope.p_Med.altura = 0;
    $scope.p_Med.volumen = 0;
    $scope.p_Med.precioBruto = "";
    $scope.p_Med.descuento1 = "";
    $scope.p_Med.descuento2 = "";
    $scope.p_Med.impVerde = false;
    $scope.p_Med.estado = ""; //Parea sutitución, bloqueo/desbloque y cambio de precio
    $scope.p_Med.accion = "";

    //modelo Codigo Barra
    $scope.CBr = [];
    $scope.p_CBr = {};
    $scope.p_CBr.idDetalle = "";
    $scope.p_CBr.unidadMedida = "";
    $scope.p_CBr.numeroEan = "";
    $scope.p_CBr.tipoEan = "";
    $scope.p_CBr.descripcionEan = "";
    $scope.p_CBr.principal = false;
    $scope.p_CBr.accion = "";

    //modelo Imagenes
    $scope.Ima = [];
    $scope.p_Ima = {};
    $scope.p_Ima.idDetalle = "";
    $scope.p_Ima.path = "";
    $scope.p_Ima.idDocAdjunto = "";
    $scope.p_Ima.nomArchivo = "";
    $scope.p_Ima.accion = "";

    //modelo Compras
    $scope.Com = [];
    $scope.p_Com = {};
    $scope.p_Com.idDetalle = "";
    $scope.p_Com.codSap = "";
    $scope.p_Com.codLegacy = "";
    $scope.p_Com.costoFOB = "";
    $scope.p_Com.validoDesde = "";
    $scope.p_Com.observaciones = "";
    $scope.p_Com.organizacionCompras = "";
    $scope.p_Com.frecuenciaEntrega = "";
    $scope.p_Com.tipoMaterial = "";
    $scope.p_Com.categoriaMaterial = "";
    $scope.p_Com.grupoArticulo = "";
    $scope.p_Com.seccionArticulo = "";
    $scope.p_Com.surtidoParcial = "";
    $scope.p_Com.materia = "";
    $scope.p_Com.indPedido = "";
    $scope.p_Com.perfilDistribucion = "";
    $scope.p_Com.grupoCompra = "";
    $scope.p_Com.categoriaValoracion = "";
    $scope.p_Com.tipoAlamcen = "";
    $scope.p_Com.condicionAlmacen = "";
    $scope.p_Com.clListaSurtido = "";
    $scope.p_Com.estatusMaterial = "";
    $scope.p_Com.estatusVenta = "";
    $scope.p_Com.grupoBalanzas = "";
    $scope.p_Com.nacionalImportado = "";
    $scope.p_Com.coleccion = "";
    $scope.p_Com.temporada = "";
    $scope.p_Com.estacion = "";
    //Nuevos
    $scope.p_Com.jerarquiaProd = ""; //Jerarquía de Productos
    $scope.p_Com.susceptBonifEsp = ""; //El material es susceptible de bonificación en especie
    $scope.p_Com.procedimCatalog = ""; //Procedimiento de Catalogación
    $scope.p_Com.caracterPlanNec = ""; //Característica de planificación de necesidades
    $scope.p_Com.fuenteProvision = ""; //Fuente aprovisionamiento

    $scope.p_Com.estado = "";
    $scope.p_Com.motivoRechazo = "";
    /*Pasan de uno a muchos necesitan su propio modelo
    $scope.p_Com.catalogacion = "";
    $scope.p_Com.almacen = "";
    $scope.p_Com.indalmaentrada = "";
    $scope.p_Com.indalmasalida = "";
    $scope.p_Com.indareaalmacen = "";*/
    //Combos
    $scope.ddlOrgCompras = "";
    $scope.ddlFrecEntrega = "";
    $scope.ddlTipoMaterial = "";
    $scope.ddlCatMaterial = "";
    $scope.ddlGrupoArticulo = "";
    $scope.ddlSeccArticulo = "";
    $scope.ddlSurtParcial = "";
    $scope.ddlMateria = "";
    $scope.ddlIndPedido = "";
    $scope.ddlperfDistribucion = "";
    $scope.ddlGrupoCompras = "";
    $scope.ddlCatValoracion = "";
    $scope.ddlTipoAlmacen = "";
    $scope.ddlCondAlmacen = "";
    $scope.ddlClListSurtidos = "";
    $scope.ddlEstatusMaterial = "";
    $scope.ddlEstatusVentas = "";
    $scope.ddlGrupoBalanzas = "";
    $scope.ddlNacionalImportado = "";
    $scope.ddlColeccion = "";
    $scope.ddlTemporada = "";
    $scope.ddlCantidadPedir = "";
    $scope.ddlEstacion = "";
    //De uno a muchos
    $scope.ddlCatalogacion = "";
    $scope.ddlAlmacen = "";
    $scope.ddlIndTipoAlmEnt = "";
    $scope.ddlIndTipoAlmSal = "";
    $scope.ddlIndAreaAlmacen = "";

    //modelo Catalogacion
    $scope.Cat = [];
    $scope.p_Cat = {};
    $scope.p_Cat.idDetalle = "";
    $scope.p_Cat.catalogacion = "";
    $scope.p_Cat.desCatalogacion = "";
    $scope.p_Cat.accion = "";
    //modelo Almacen
    $scope.Alm = [];
    $scope.p_Alm = {};
    $scope.p_Alm.idDetalle = "";
    $scope.p_Alm.almacen = "";
    $scope.p_Alm.desAlmacen = "";
    $scope.p_Alm.accion = "";
    //modelo IndTipoAlmEnt
    $scope.Iae = [];
    $scope.p_Iae = {};
    $scope.p_Iae.idDetalle = "";
    $scope.p_Iae.indTipoAlmEnt = "";
    $scope.p_Iae.desIndTipoAlmEnt = "";
    $scope.p_Iae.accion = "";
    //modelo IndTipoAlmSal
    $scope.Ias = [];
    $scope.p_Ias = {};
    $scope.p_Ias.idDetalle = "";
    $scope.p_Ias.indTipoAlmSal = "";
    $scope.p_Ias.desIndTipoAlmSal = "";
    $scope.p_Ias.accion = "";
    //modelo IndAreaAlmacen
    $scope.Iaa = [];
    $scope.p_Iaa = {};
    $scope.p_Iaa.idDetalle = "";
    $scope.p_Iaa.indAreaAlmacen = "";
    $scope.p_Iaa.desIndAreaAlmacen = "";
    $scope.p_Iaa.accion = "";
    //modelo IndAreaAlmacen
    $scope.Cen = [];
    

    //Cabecera
    $scope.ddlLineaNegocio = "";
    $scope.txtIdSolicitud = "";
    $scope.txtContRegistros = "";

    //Mensaje Error
    $scope.MsjConfirmacion = "";
    $scope.accion = "";

    //path Imagenes
    $scope.Rutas = [];
    $scope.Ruta = {};
    $scope.Ruta.codReferencia = "";
    $scope.Ruta.path = "";
    //Observacion y cambio de estado
    $scope.TextoBoton = "Grabar";
    $scope.ViewBoton = false;
    $scope.observEstado = "";
    $scope.ViewAsisCompras = true;
    $scope.ViewGernCompras = true;
    $scope.ViewDatosMaestr = true;
    //A nivel de la solicitud
    $scope.ViewMotRechazo = true;
    $scope.motivoRechazo = [];
    $scope.ddlMotivoRechazo = "";
    //A nivel del articulo
    $scope.ViewMotRechazoArt = true;
    $scope.motivoRechazoArt = [];
    $scope.ddlMotivoRechazoArt = "";

    //boolean para inactivar botones, cajas de texto y tabs
    $scope.inactivaBot = false;
    $scope.inactivaTab = true;
    $scope.bloqueaBotInicio = false;
    $scope.isReadOnly = false;
    $scope.bloqueaCodReferencia = true;
    $scope.bloqCamposModif = false;
    $scope.inactivaCodEAN = false;

    //Lista de elementos eliminados
    $scope.listaDetallesSolEliminados = [];
    $scope.listaMedidadSolEliminados = [];
    $scope.listaCodBarrasSolEliminados = [];
    $scope.listaImagenesSolEliminados = [];
    $scope.listaCatalogosSolEliminados = [];
    $scope.listaAlmacenSolEliminados = [];
    $scope.listaTipoAlmacenESolEliminados = [];
    $scope.listaTipoAlmacenSSolEliminados = [];
    $scope.listaAreaAlmacenSolEliminados = [];
    $scope.tipoAlmacenBod = [];
    $scope.etiTotRegistros = "";
    $scope.enviaNotificacionSN = false;
    $scope.agrupacion = [];
    $scope.codAgrupacion = {};

    //Carga Combos
    $scope.myPromise = ModArticuloService.getConsCaracteristicas("999").then(function (results) {
     
        if (results.data.success) {
            //var descAux = "";
            $scope.listaCaracteristicas = results.data.root[0];
            for (var idx = 0; idx < $scope.listaCaracteristicas.length; idx++) {
                $scope.codAgrupacion = {};
                $scope.codAgrupacion.codigo = $scope.listaCaracteristicas[idx];
                $scope.codAgrupacion.detalle = $scope.listaCaracteristicas[idx];
                $scope.agrupacion.push($scope.codAgrupacion);
            }

        }
        else {
            $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
            $('#idMensajeError').modal('show');

        }
    }, function (error) {
        $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
        $('#idMensajeError').modal('show');

    });

    //Mostrar caracteristicas dependiendo de grupo
    $scope.$watch('ddlAgrupacion', function () {

        $scope.CaracteristicasART = [];

        if ($scope.ddlAgrupacion == null) return;

        $scope.myPromise = ModArticuloService.getConsCaracteristicas($scope.ddlAgrupacion.codigo).then(function (results) {
     
            if (results.data.success) {
                $scope.CaracteristicasART = results.data.root[0];
            }
            else {
                $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
                $('#idMensajeError').modal('show');

            }
        }, function (error) {
            $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
            $('#idMensajeError').modal('show');

        });





    });

    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_MarcaArticulo').then(function (results) {
        $scope.marca = results.data;
        //Agrega el campo para "Otros"
        $scope.mar = { codigo: '-1', detalle: 'OTROS (Especifique)', descAlterno: '' };
        $scope.marca.push($scope.mar);
    });
    ;
    //Cuando la marca sea OTROS activa 
    $scope.$watch('ddlMarca', function () {
        $scope.p_Det.marcaNueva = "";
        if ($scope.ddlMarca.codigo === "-1") {
            $scope.p_Det.viewMarcaNueva = false;
        } else {
            $scope.p_Det.viewMarcaNueva = true;
        }
    });
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Pais').then(function (results) {
        $scope.paisOrigen = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Region').then(function (results) {
        $scope.regionOrigenTemp = results.data;
    });
    ;
    $scope.$watch('ddlPaisOrigen', function () {
        $scope.regionOrigen = [];
        if ($scope.ddlPaisOrigen != '' && angular.isUndefined($scope.ddlPaisOrigen) != true) {
            $scope.regionOrigen = $filter('filter')($scope.regionOrigenTemp, { descAlterno: $scope.ddlPaisOrigen.codigo });
        }
    });
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_TallaArticulo').then(function (results) {
        $scope.talla = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_GradoAlcohol').then(function (results) {
        $scope.gradoAlcohol = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Color').then(function (results) {
        $scope.color = results.data;
    });
    ;

    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Canaldistribucion').then(function (results) {
        $scope.canalDistibucion = results.data;
    });
    ;

    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_TipoAlmBod').then(function (results) {
        $scope.tipoAlmacenBod = results.data;
    });
    ;

    $scope.myPromise =
  IngArticuloService.getCatalogo('tbl_Indareaalmacen').then(function (results) {
      $scope.indAreaAlmNew = results.data;
  });
    ;
    //$scope.myPromise =
    //IngArticuloService.getCatalogo('tbl_Fragancia').then(function (results) {
    //    $scope.fragancia = results.data;
    //});
    //;
    //$scope.myPromise =
    //IngArticuloService.getCatalogo('tbl_TiposArticulo').then(function (results) {
    //    $scope.tipos = results.data;
    //});
    //;
    //$scope.myPromise =
    //IngArticuloService.getCatalogo('tbl_Sabor').then(function (results) {
    //    $scope.sabor = results.data;
    //});
    //;
    //$scope.myPromise =
    //IngArticuloService.getCatalogo('tbl_ModeloArticulo').then(function (results) {
    //    $scope.modelo = results.data;
    //});
    //;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_UnidadMedidaArt').then(function (results) {
        $scope.unidadMedida = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_TipoUnidMedArt').then(function (results) {
        $scope.tipoUnidadMedida = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_UniMedConverArt').then(function (results) {
        $scope.uniMedConvers = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.lineaNegocio = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_TiposEAN').then(function (results) {
        $scope.tipoEANart = results.data;
    });
    ;

    //IngArticuloService.getCatalogo('tbl_ClasificacionFiscal').then(function (results) {
    //    $scope.clasificacionFiscal = results.data;
    //});

    //Administrativo
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Orgcompras').then(function (results) {
        $scope.orgCompras = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Seccarticulo').then(function (results) {
        $scope.seccArticulo = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Catalogacion').then(function (results) {
        $scope.catalogacion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_lgnum').then(function (results) {
        $scope.almacen = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Tipoalmacen').then(function (results) {
        $scope.tipoAlmacen = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Indtipoalment').then(function (results) {
        $scope.indTipoAlmEnt = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Indtipoalmsal').then(function (results) {
        $scope.indTipoAlmSal = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Indareaalmacen').then(function (results) {
        $scope.indAreaAlmacen = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Condalmacen').then(function (results) {
        $scope.condAlmacen = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Cllistsurtidos').then(function (results) {
        $scope.clListSurtidos = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Estatusmaterial').then(function (results) {
        $scope.estatusMaterial = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Estatusventas').then(function (results) {
        $scope.estatusVentas = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Grupobalanzas').then(function (results) {
        $scope.grupoBalanzas = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Nacional_Importado').then(function (results) {
        $scope.nacionalImportado = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_CatMaterial').then(function (results) {
        $scope.catMaterial = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_CatValoracion').then(function (results) {
        $scope.catValoracion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_FrecEntrega').then(function (results) {
        $scope.frecEntrega = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_GrupoArticulo').then(function (results) {
        $scope.grupoArticulo = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_GrupoCompras').then(function (results) {
        $scope.grupoCompras = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_IndPedido').then(function (results) {
        $scope.indPedido = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Materia_Art').then(function (results) {
        $scope.materia = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_PerfDistribucion').then(function (results) {
        $scope.perfDistribucion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_SurtParcial').then(function (results) {
        $scope.surtParcial = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_tipoMaterial_Art').then(function (results) {
        $scope.tipoMaterial = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_ClasificacionFiscal').then(function (results) {
        $scope.clasificacionFiscal = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_Deducible').then(function (results) {
        $scope.sn_ded = results.data;
    });
    ;

    $scope.myPromise =
   IngArticuloService.getCatalogo('tbl_volumen').then(function (results) {
       $scope.catalogoVolumen = results.data;
   });
    ;

    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_IndicadorReten').then(function (results) {
        $scope.sn_ret = results.data;
    });
    ;
    $scope.myPromise =
    IngArticuloService.getCatalogo('tbl_MotivoRechazo_Art').then(function (results) {
        $scope.motivoRechazo = results.data;
        $scope.motivoRechazoArt = results.data;
    });
    ;
    $scope.calculaVolumen = function () {

        if ($scope.p_Med.longitud == "") $scope.p_Med.longitud = 0;
        if ($scope.p_Med.ancho == "") $scope.p_Med.ancho = 0;
        if ($scope.p_Med.altura == "") $scope.p_Med.altura = 0;
        $scope.p_Med.volumen = $scope.p_Med.longitud * $scope.p_Med.ancho * $scope.p_Med.altura;

    }

    //Ver detalle de variantes
    $scope.QuitarVariante = function (registro) {
        var resGenerico = 0;
        var varianteModicar = $filter('filter')($scope.grArticuloDet, { codSAPart: registro.codSAPart }, true)[0];
        resGenerico = varianteModicar.codGenerico;
        varianteModicar.codGenerico = 0;
        if (varianteModicar.accion != "I")
            varianteModicar.accion = "U";
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: resGenerico }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;
    }


    //Agregar variante a Generico
    $scope.agregarVariante = function (idPadre) {
        var artiGenericoSel = $filter('filter')($scope.grArticulo, { seleccionaGenerico: true });
        if (artiGenericoSel.length == 0) {
            $scope.MenjError = "Seleccione al menos un artículo.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        var valVariantes = false;
        var codRefval = "";
        for (var idx = 0 ; idx < artiGenericoSel.length; idx++) {
            if (!artiGenericoSel[idx].isVariante) {
                valVariantes = true;
                codRefval = artiGenericoSel[idx].codReferencia;
                break;
            }

        }

        if (valVariantes) {
            $scope.MenjError = "El artículo " + codRefval + " no es variante.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        for (var idx = 0; idx < artiGenericoSel.length; idx++) {
            var update = artiGenericoSel[idx];
            update.codGenerico = idPadre;
            update.seleccionaGenerico = false;
            update.accion = "U";
        }

        var auxgrArticulo = $filter('filter')($scope.grArticulo, { codGenerico: "0" }, true);
        $scope.etiTotRegistros = auxgrArticulo.length;

        $scope.verDetVariantes = true;
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: idPadre }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;




    }
    //Ver detalle de variantes
    $scope.verVariantes = function (registro) {
        $scope.verDetVariantes = true;
        $scope.grArticuloDet = $filter('filter')($scope.grArticulo, { codGenerico: registro.idDetalle }, true);
        $scope.etiTotRegistros2 = $scope.grArticuloDet.length;
        $scope.codArticulo = registro.codReferencia;
        $scope.idArticuloGen = registro.idDetalle;

    }

    //Actualizar Unidades de medida desde Grid
    //BASE
    $scope.actualizaUniMedB = function (uniMed, uniMedBase) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {

            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    update3.uniMedBase = false;
                    //update.uniMedPedido = $scope.p_Med.medPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedBase = false;
            }


            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = $scope.p_Med.medPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //PEDIDO
    $scope.actualizaUniMedP = function (uniMed, uniMedPedido) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    update3.uniMedPedido = false;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedPedido = false;
            }



            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    update.uniMedPedido = uniMedPedido;
                    //update.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //ES
    $scope.actualizaUniMedES = function (uniMed, uniMedES) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    //update3.uniMedPedido = false;
                    update3.uniMedES = $scope.p_Med.medES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedES = false;
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = uniMedPedido;
                    update.uniMedES = uniMedES;
                    //update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }
    //Venta
    $scope.actualizaUniMedV = function (uniMed, uniMedV) {
        var edita = false;
        if ($scope.grArtMedida.length != 0) {

            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update3 = $scope.Med[i];
                if (update3.idDetalle == $scope.p_Det.idDetalle) {
                    //update3.uniMedBase = uniMedBase;
                    //update3.uniMedPedido = false;
                    //update3.uniMedES = $scope.p_Med.medES;
                    update3.uniMedVenta = $scope.p_Med.medVenta;
                    if (update3.accion != "I")
                        update3.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }

            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                var update2 = $scope.grArtMedida[i];
                update2.uniMedVenta = false;
            }



            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == uniMed) {
                    edita = true;
                    break;
                }
            }
        }


        if (edita === true) {
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == uniMed) {
                    //update.uniMedBase = uniMedBase;
                    //update.uniMedPedido = uniMedPedido;
                    //update.uniMedES = uniMedES;
                    update.uniMedVenta = uniMedV;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        }

    }

    //Validar Tipo de EAN
    $scope.$watch('p_CBr.tipoEanCat', function () {
        if ($scope.p_CBr.tipoEanCat != undefined) {
            if ($scope.p_CBr.tipoEanCat.codigo == 'IE') {
                $scope.inactivaCodEAN = true;
                $scope.p_CBr.numeroEan = "9999999999999";
            }
            else {
                $scope.inactivaCodEAN = false;
                $scope.p_CBr.numeroEan = "";
            }

        }
    });

    //Validacion al realizar ingreso de medida    
    $scope.validarUnidadMedida = function () {
      
        //Limpia Unidad Medida
        $scope.p_Med = {};
        //$scope.p_Med.unidadMedida = "";
        //$scope.p_Med.tipoUnidadMedida = "";
        //$scope.p_Med.uniMedConvers = "";
        //$scope.p_Med.desUnidadMedida = "";
        //$scope.p_Med.desTipoUnidadMedida = "";
        //$scope.p_Med.desUniMedConvers = "";
        //$scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.ddlUniMedConvers = "";
        //$scope.p_Med.factorCon = "";
        //$scope.p_Med.pesoNeto = "";
        //$scope.p_Med.pesoBruto = "";
        //$scope.p_Med.longitud = "";
        //$scope.p_Med.ancho = "";
        //$scope.p_Med.altura = "";
        //$scope.p_Med.precioBruto = "";
        //$scope.p_Med.descuento1 = "";
        //$scope.p_Med.descuento2 = "";
        //$scope.p_Med.estado = "";
        //$scope.p_Med.impVerde = false;
        //Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
        if ($scope.ddlUnidadMedida != undefined)
            $scope.SeleccionMed($scope.ddlUnidadMedida.codigo);
        //alert("llego");


    }

    //Generar Reportes
    $scope.exportar = function (Id, tipoSol) {
        debugger;
        var repCab = {};
        var repDet = {};
        var repListaDet = [];
        repCab.tipo = Id;
        repCab.tipoSolicitud = tipoSol;
        repCab.usuario = authService.authentication.Usuario;
        repCab.proveedor = authService.authentication.NombreParticipante;
        repCab.numSolicitud = $scope.txtIdSolicitud;
        var pais = "";
        var ciudad = "";
        var po = $scope.paisOrigen;
        for (var index = 0; index < po.length; ++index) {
            if (po[index].codigo === $scope.grArticulo[0].paisOrigen)
                pais = po[index];
        }
        //Region Origen
        var ro = $scope.regionOrigenTemp;
        for (var index = 0; index < ro.length; ++index) {
            if (ro[index].codigo === $scope.grArticulo[0].regionOrigen)
                ciudad = ro[index];
        }

        repCab.pais = pais.detalle + "/" + ciudad.detalle;

        for (var l = 0 ; l < $scope.grArticulo.length; l++) {
            repDet = {};
            repDet.codigo = $scope.grArticulo[l].codReferencia;
            repDet.marca = $scope.grArticulo[l].desMarca;
            repDet.descripcion = $scope.grArticulo[l].descripcion;
            repDet.texto = $scope.grArticulo[l].otroId;
            repDet.presentacion = $scope.grArticulo[l].tamArticulo;
            repDet.modelo = $scope.grArticulo[l].modelo;
            //Grado Alcohol
            var grAlcohol = "";
            var ga = $scope.gradoAlcohol;
            if ($scope.grArticulo[l].gradoAlcohol != "" && $scope.grArticulo[l].gradoAlcohol != undefined) {
                for (index = 0; index < ga.length; ++index) {
                    if (ga[index].codigo === $scope.grArticulo[l].gradoAlcohol)
                        grAlcohol = ga[index];
                }
                repDet.alcohol = grAlcohol.detalle;
            }
            else {
                repDet.alcohol = "";
            }
            //iva
            var iva = "";
            var cl = $scope.clasificacionFiscal;
            for (index = 0; index < cl.length; ++index) {
                if (cl[index].codigo === $scope.grArticulo[l].iva)
                { iva = cl[index]; break; }
            }

            repDet.iva = iva.detalle;
            //deducible
            var deduc = ""
            var te = $scope.sn_ded;
            for (index = 0; index < te.length; ++index) {
                if (te[index].codigo === $scope.grArticulo[l].deducible)
                { deduc = te[index]; break }
            }
            repDet.deducible = deduc.detalle;

            //retencion
            var reten = ""
            var es = $scope.sn_ret;
            for (index = 0; index < es.length; ++index) {
                if (es[index].codigo === $scope.grArticulo[l].retencion)
                { reten = es[index]; break; }
            }


            repDet.retencion = reten.detalle;
            repListaDet.push(repDet);
        }

        $scope.myPromise =
            IngArticuloService.getReporteSolicitud(repCab, repListaDet).then(function (response) {
       
                if (response.data != "") {


                    window.open(response.data, '_blank', '');
                }
                else {

                    $scope.MenjError = "Error al generar reporte.";
                    $('#idMensajeError').modal('show');
                    return;
                }


            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        ;

    }

    //Cuando entra a la pagina carga los datos de la cosulta
    $scope.CargaConsulta = function () {

        if ($scope.txtIdSolicitud != "0") {
            //tipo, codigo Solo necesita el tipo y el codigo            
            $scope.myPromise = IngArticuloService.getArticulos("2", $scope.txtIdSolicitud, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0", authService.authentication.CodSAP).then(function (results) {

           
                //Linea de Negocio
                var index;
                var linea = {};
                linea = results.data.root[0];
                if (linea[0].estado == 'NE' || linea[0].estado == 'DC') {
                    $scope.isReadOnly = false;
                    $scope.inactivaBot = false;
                }
                else {
                    $scope.isReadOnly = true;
                    $scope.inactivaBot = true;

                }



                var ln = $scope.lineaNegocio;
                for (index = 0; index < ln.length; ++index) {
                    if (ln[index].codigo === linea[0].lineaNegocio)
                        $scope.ddlLineaNegocio = ln[index];

                    $scope.lineaNegocio2 = $scope.ddlLineaNegocio.detalle;
                }

                $scope.Det = results.data.root[1];
                $scope.codSapProv = $scope.Det[0].codProveedor;
                $scope.grArticulo = $scope.Det;
                $scope.etiTotRegistros = $scope.grArticulo.length;
               
                //for (var i = 0 ; i < $scope.grArticulo.length; i++)
                //{ 
                //    if ($scope.grArticulo[i].estado == 'APD')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Aprobado' }
                //    if ($scope.grArticulo[i].estado == 'RED')
                //    { $scope.grArticulo[i].estadoDescripcion = 'Rechazado' }

                //}


                //bloquear linea de negocio
                if ($scope.grArticulo.length > 0) {
                    $scope.bloqueaBotInicio = true;
                }
                $scope.Med = results.data.root[2];
                $scope.CBr = results.data.root[3];
                $scope.Ima = results.data.root[4];
                $scope.Rutas = results.data.root[5];
                $scope.Com = results.data.root[6];
                $scope.Cat = results.data.root[7];
                $scope.Alm = results.data.root[8];
                $scope.Iae = results.data.root[9];
                $scope.Ias = results.data.root[10];
                
                $scope.Iaa = results.data.root[11];
                $scope.grArtObservacion = results.data.root[12];
                $scope.CaractArt = results.data.root[13];
       
                $scope.ListacodLegacy = results.data.root[14];
                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }, function (error) {
            });
        }
    }
   

    //Carga la unidad de medida seleccionada en el combo para la seccion de codigo de barras
    $scope.$watch('ddlUnidadMedida', function () {
   
        if ($scope.ddlUnidadMedida != undefined) {
            $scope.p_CBr.unidadMedida = $scope.ddlUnidadMedida.codigo;
            $scope.desnumeroEan = $scope.ddlUnidadMedida.detalle;
        }
        else
        {
            $scope.p_CBr.unidadMedida = "";
            $scope.desnumeroEan = "";
        }
          


        
   
   




    });

    //Eliminar Catalogacion
    $scope.EliminaCat = function (Id) {

        for (var i = 0, len = $scope.grArtCatalogacion.length; i < len; i++) {
            if ($scope.grArtCatalogacion[i].catalogacion === Id) {
                break;

            }
        }

  
        $scope.grArtCatalogacion.splice(i, 1);
        for (var i = 0, len = $scope.Cat.length; i < len; i++) {
            if ($scope.Cat[i].catalogacion === Id && $scope.Cat[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Cat[i].accion = 'D';
        $scope.listaCatalogosSolEliminados[$scope.listaCatalogosSolEliminados.length] = $scope.Cat[i];
        $scope.Cat.splice(i, 1);

    }

    //Eliminar Almacen
    $scope.EliminaAlm = function (Id) {

        for (var i = 0, len = $scope.grArtAlmacen.length; i < len; i++) {
            if ($scope.grArtAlmacen[i].almacen === Id) {
                break;

            }
        }

       
        $scope.grArtAlmacen.splice(i, 1);
        for (var i = 0, len = $scope.Alm.length; i < len; i++) {
            if ($scope.Alm[i].almacen === Id && $scope.Alm[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Alm[i].accion = 'D';
        $scope.listaAlmacenSolEliminados[$scope.listaAlmacenSolEliminados.length] = $scope.Alm[i];
        $scope.Alm.splice(i, 1);

    }

    //Eliminar tipo de Almacen entrada
    $scope.EliminaIae = function (Id) {

        for (var i = 0, len = $scope.grArtIndTipoAlmEnt.length; i < len; i++) {
            if ($scope.grArtIndTipoAlmEnt[i].indTipoAlmEnt === Id) {
                break;

            }
        }

   
        $scope.grArtIndTipoAlmEnt.splice(i, 1);
        for (var i = 0, len = $scope.Iae.length; i < len; i++) {
            if ($scope.Iae[i].indTipoAlmEnt === Id && $scope.Iae[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Iae[i].accion = 'D';
        $scope.listaTipoAlmacenESolEliminados[$scope.listaTipoAlmacenESolEliminados.length] = $scope.Iae[i];
        $scope.Iae.splice(i, 1);

    }

    //Eliminar tipo de Almacen salida
    $scope.EliminaIas = function (Id) {

        for (var i = 0, len = $scope.grArtIndTipoAlmSal.length; i < len; i++) {
            if ($scope.grArtIndTipoAlmSal[i].indTipoAlmSal === Id) {
                break;

            }
        }

    
        $scope.grArtIndTipoAlmSal.splice(i, 1);
        for (var i = 0, len = $scope.Ias.length; i < len; i++) {
            if ($scope.Ias[i].indTipoAlmSal === Id && $scope.Ias[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Ias[i].accion = 'D';
        $scope.listaTipoAlmacenSSolEliminados[$scope.listaTipoAlmacenSSolEliminados.length] = $scope.Ias[i];
        $scope.Ias.splice(i, 1);

    }

    //Eliminar tipo de Area Almacen 
    $scope.EliminaIaa = function (Id) {

        for (var i = 0, len = $scope.grArtIndAreaAlmacen.length; i < len; i++) {
            if ($scope.grArtIndAreaAlmacen[i].indAreaAlmacen === Id) {
                break;

            }
        }

       
        $scope.grArtIndAreaAlmacen.splice(i, 1);
        for (var i = 0, len = $scope.Iaa.length; i < len; i++) {
            if ($scope.Iaa[i].indAreaAlmacen === Id && $scope.Iaa[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Iaa[i].accion = 'D';
        $scope.listaAreaAlmacenSolEliminados[$scope.listaAreaAlmacenSolEliminados.length] = $scope.Iaa[i];
        $scope.Iaa.splice(i, 1);

    }

    //Quitar articulo    
    $scope.QuitarArt = function (Id) {
      
        var existe = false;

        if ($scope.grArticulo.length != 0) {
            for (var i = 0, len = $scope.grArticulo.length; i < len; i++) {
                if ($scope.grArticulo[i].codReferencia === Id) {
                    existe = true;
                    break;
                }
            }
            if (existe) {
              
                for (var j = 0 ; j < $scope.CaractArt.length; j++) {
                    var update = $scope.CaractArt[j];
                    if (update.idDetalle == $scope.grArticulo[i].idDetalle) {

                        update.accion = "D";
                    }

                }


                //Quitar catalogacion
                for (var j = 0, len = $scope.Cat.length; j < len; j++) {
                    if ($scope.Cat.length == 0)
                    { break; }
                    if (j < $scope.Cat.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Cat[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Cat[j].accion = 'D';
                        $scope.listaCatalogosSolEliminados[$scope.listaCatalogosSolEliminados.length] = $scope.Cat[j];
                        $scope.Cat.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacenes
                for (var j = 0, len = $scope.Alm.length; j < len; j++) {
                    if ($scope.Alm.length == 0)
                    { break; }
                    if (j < $scope.Alm.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Alm[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Alm[j].accion = 'D';
                        $scope.listaAlmacenSolEliminados[$scope.listaAlmacenSolEliminados.length] = $scope.Alm[j];
                        $scope.Alm.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen entrada
                for (var j = 0, len = $scope.Iae.length; j < len; j++) {
                    if ($scope.Iae.length == 0)
                    { break; }
                    if (j < $scope.Iae.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Iae[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Iae[j].accion = 'D';
                        $scope.listaTipoAlmacenESolEliminados[$scope.listaTipoAlmacenESolEliminados.length] = $scope.Iae[j];
                        $scope.Iae.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen salida
                for (var j = 0, len = $scope.Ias.length; j < len; j++) {
                    if ($scope.Ias.length == 0)
                    { break; }
                    if (j < $scope.Ias.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Ias[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Ias[j].accion = 'D';
                        $scope.listaTipoAlmacenSSolEliminados[$scope.listaTipoAlmacenSSolEliminados.length] = $scope.Ias[j];
                        $scope.Ias.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar Almacen Area
                for (var j = 0, len = $scope.Iaa.length; j < len; j++) {
                    if ($scope.Iaa.length == 0)
                    { break; }
                    if (j < $scope.Iaa.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Iaa[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Iaa[j].accion = 'D';
                        $scope.listaAreaAlmacenSolEliminados[$scope.listaAreaAlmacenSolEliminados.length] = $scope.Iaa[j];
                        $scope.Iaa.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar unidades de medidas
                for (var j = 0, len = $scope.Med.length; j < len; j++) {
                    if ($scope.Med.length == 0)
                    { break; }
                    if (j < $scope.Med.length)
                        var x;
                    else
                    { break; }
                    if ($scope.Med[j].idDetalle === $scope.grArticulo[i].idDetalle) {  //$scope.grArticulo[i].idDetalle
                        $scope.Med[j].accion = 'D';
                        $scope.listaMedidadSolEliminados[$scope.listaMedidadSolEliminados.length] = $scope.Med[j];
                        $scope.Med.splice(j, 1);
                        j = j - 1;

                    }

                }

                //Quitar codigo de barras
                for (var j = 0, len = $scope.CBr.length; j < len; j++) {
                    if ($scope.CBr.length == 0)
                    { break; }
                    if (j < $scope.CBr.length)
                        var x;
                    else
                    { break; }
                    if ($scope.CBr[j].idDetalle === $scope.grArticulo[i].idDetalle) {

                        $scope.CBr[j].accion = 'D';
                        $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[j];
                        $scope.CBr.splice(j, 1);
                        j = j - 1;
                    }
                }
                //Quitar adjuntos
                for (var j = 0, len = $scope.Ima.length; j < len; j++) {
                    if ($scope.Ima.length == 0)
                    { break; }
                    if (j < $scope.Ima.length)
                        var x;
                    else
                    { break; }

                    if ($scope.Ima[j].idDetalle === $scope.grArticulo[i].idDetalle) {
                        $scope.Ima[j].accion = 'D';
                        $scope.listaImagenesSolEliminados[$scope.listaImagenesSolEliminados.length] = $scope.Ima[j];
                        $scope.Ima.splice(j, 1);
                        j = j - 1;
                    }

                }

               
            
                $scope.grArticulo[i].accion = 'D';
                $scope.listaDetallesSolEliminados[$scope.listaDetallesSolEliminados.length] = $scope.grArticulo[i];
                $scope.grArticulo.splice(i, 1);
                $scope.etiTotRegistros = $scope.grArticulo.length;
                $scope.Limpia();
                if ($scope.grArticulo.length > 0) {
                    $scope.bloqueaBotInicio = true;
                }
                else
                    $scope.bloqueaBotInicio = false;
                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }
        }

    }

    $scope.agregarCaracteristicas = function () {

        //Agregar caracteristicas
     
        $scope.CaractArtTMP = [];
        for (var idx = 0; idx < $scope.CaracteristicasART.length; idx++) {

            $scope.p_CaractArt = {};
            $scope.p_CaractArt.idDetalle = $scope.p_Det.idDetalle;
            var idBus = "#selectValorCaract" + $scope.CaracteristicasART[idx].id;
            var valor = $(idBus + " option:selected").html();
            $scope.p_CaractArt.idCaract = $scope.CaracteristicasART[idx].id;
            $scope.p_CaractArt.idAgrupacion = $scope.CaracteristicasART[idx].agrupacion;
            $scope.p_CaractArt.idValor = valor;
            $scope.p_CaractArt.accion = "I";

            $scope.CaractArtTMP.push($scope.p_CaractArt);

        }

    }

    $scope.cargarCaracteristicas = function (idDet) {

        for (var u = 0 ; u < $scope.CaracteristicasART.length; u++) {

            var idBus = "selectValorCaract" + $scope.CaracteristicasART[u].id;
            if ($scope.CaracteristicasART[u].lista) {
                var caracSelec;
                caracSelec = $filter('filter')($scope.CaractArt, { idDetalle: idDet, idCaract: $scope.CaracteristicasART[u].id });

                if (caracSelec.length > 0) {

                    var lisValores = $scope.CaracteristicasART[u].listaValor;
                    for (var x = 0; x < lisValores.length; x++) {
                        if (lisValores[x].detalle == caracSelec[0].idValor)
                            break;

                    }

                    document.getElementById(idBus).selectedIndex = x + 1;

                }

            }

        }


    }

    function pruebaTime() {
       
        $scope.cargarCaracteristicas($scope.idSeleccionado);
    }
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("href") // activated tab
        $scope.agregarCaracteristicas();
    });

    $scope.SeleccionArt = function (Id) {
     
        $scope.bloqueaCodReferencia = true;
        if ($scope.isReadOnly == true) {
            $scope.inactivaTab = true;
            $scope.inactivaBot = true;
        }
        else {
            $scope.inactivaTab = false;
            $scope.inactivaBot = false;
        }

        $scope.Limpia();
        $scope.p_Det.idDetalle = Id;
        $scope.idSeleccionado = Id;
        //Cargar caracteristicas
        var busCaract = false;
        for (var bus = 0 ; bus < $scope.CaractArt.length; bus++) {
            if ($scope.CaractArt[bus].idDetalle == Id) {
                busCaract = true;
                break;
            }

        }
        if (busCaract) {

            for (var i = 0; i < $scope.agrupacion.length; i++) {
                if ($scope.agrupacion[i].detalle == $scope.CaractArt[bus].idAgrupacion)
                    $scope.ddlAgrupacion = $scope.agrupacion[i];
            }

            $scope.myPromise = IngArticuloService.getConsCaracteristicas($scope.CaractArt[bus].idAgrupacion).then(function (results) {


                if (results.data.success) {
                    $scope.CaracteristicasART = results.data.root[0];
                }
                else {
                    $scope.MenjError = 'Error al consultar caracteristicas de artículos: ' + results.data.mensaje;
                    $('#idMensajeError').modal('show');

                }

                setTimeout(pruebaTime, 500);

            }, function (error) {
                $scope.MenjError = 'Error en comunicación: getConsCaracteristicas().';
                $('#idMensajeError').modal('show');

            });
        }


        //Carga datos y grid filtrados
        for (var i = 0, len = $scope.Det.length; i < len; i++) {
            if ($scope.Det[i].idDetalle === Id) {
                var index;
                if ($scope.Det[i].isVariante && $scope.Det[i].codGenerico != 0)
                    $scope.verDetVariantes = true;
                else
                    $scope.verDetVariantes = false;
                //marca
                var ma = $scope.marca;
                for (index = 0; index < ma.length; ++index) {
                    if (ma[index].codigo === $scope.Det[i].marca)
                        $scope.ddlMarca = ma[index];
                }
                //Pais Origen
                var po = $scope.paisOrigen;
                for (index = 0; index < po.length; ++index) {
                    if (po[index].codigo === $scope.Det[i].paisOrigen)
                        $scope.ddlPaisOrigen = po[index];
                }
                //Region Origen
                var ro = $scope.regionOrigenTemp;
                for (index = 0; index < ro.length; ++index) {
                    if (ro[index].codigo === $scope.Det[i].regionOrigen)
                        $scope.ddlRegionOrigen = ro[index];
                }
                //Talla Articulo
                var ta = $scope.talla;
                for (index = 0; index < ta.length; ++index) {
                    if (ta[index].codigo === $scope.Det[i].talla)
                        $scope.ddlTalla = ta[index];
                }
                //Grado Alcohol
                var ga = $scope.gradoAlcohol;
                for (index = 0; index < ga.length; ++index) {
                    if (ga[index].codigo === $scope.Det[i].gradoAlcohol)
                        $scope.ddlGradoAlcohol = ga[index];
                }
                //color
                var co = $scope.color;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Det[i].color)
                        $scope.ddlColor = co[index];
                }
                //fragancia
                var fa = $scope.fragancia;
                for (index = 0; index < fa.length; ++index) {
                    if (fa[index].codigo === $scope.Det[i].fragancia)
                        $scope.ddlFragancia = fa[index];
                }
                //tipos
                var ti = $scope.tipos;
                for (index = 0; index < ti.length; ++index) {
                    if (ti[index].codigo === $scope.Det[i].tipos)
                        $scope.ddlTipos = ti[index];
                }
                //sabor
                var sa = $scope.sabor;
                for (index = 0; index < sa.length; ++index) {
                    if (sa[index].codigo === $scope.Det[i].sabor)
                        $scope.ddlSabor = sa[index];
                }
                //modelo
                $scope.ddlModelo = $scope.Det[i].modelo;
                //var mo = $scope.modelo;
                //for (index = 0; index < mo.length; ++index) {
                //    if (mo[index].codigo === $scope.Det[i].modelo)
                //        $scope.ddlModelo = mo[index];
                //}
                //iva
                var cl = $scope.clasificacionFiscal;
                for (index = 0; index < cl.length; ++index) {
                    if (cl[index].codigo === $scope.Det[i].iva)
                        $scope.ddlIva = cl[index];
                }
                //deducible
                var te = $scope.sn_ded;
                for (index = 0; index < te.length; ++index) {
                    if (te[index].codigo === $scope.Det[i].deducible)
                        $scope.ddlDeducible = te[index];
                }
                //retencion
                var es = $scope.sn_ret;
                for (index = 0; index < es.length; ++index) {
                    if (es[index].codigo === $scope.Det[i].retencion)
                        $scope.ddlRetencion = es[index];
                }

                //Coleccion
                var co = $scope.coleccion;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Det[i].coleccion)
                        $scope.ddlColeccion = co[index];
                }

                //Temporada
                var tp = $scope.temporada;
                for (index = 0; index < tp.length; ++index) {
                    if (tp[index].codigo === $scope.Det[i].temporada)
                        $scope.ddlTemporada = tp[index];
                }
                $scope.ddlEstacion = $scope.Det[i].estacion;
                $scope.ddlCantidadPedir = $scope.Det[i].cantidadPedir;
                $scope.p_Det.idDetalle = $scope.Det[i].idDetalle;
                $scope.p_Det.codReferencia = $scope.Det[i].codReferencia;
                $scope.p_Det.marcaNueva = $scope.Det[i].marcaNueva;
                $scope.codSapProv = $scope.Det[i].codProveedor;
                $scope.codSapArt = $scope.Det[i].codSAPart;
                $scope.p_Det.precioBruto = $scope.Det[i].precioBruto;
                $scope.p_Det.descuento1 = $scope.Det[i].descuento1;
                $scope.p_Det.descuento2 = $scope.Det[i].descuento2;
                $scope.p_Det.impVerde = $scope.Det[i].impVerde;
                $scope.p_Det.descripcion = $scope.Det[i].descripcion;
                $scope.p_Det.tamArticulo = $scope.Det[i].tamArticulo;
                $scope.p_Det.otroId = $scope.Det[i].otroId;
                $scope.p_Det.contAlcohol = $scope.Det[i].contAlcohol;
                $scope.Det2 = $scope.datosArtModifica[1];
                if ($scope.Det2 != undefined) {
                    //Buscar articulo seleccionado por codigo SAP
                    for (var l = 0 ; l < $scope.Det2.length; l++) {
                        if ($scope.Det[i].codSAPart == $scope.Det2[l].codSAPart) {
                            $scope.p_Det2.descripcion = $scope.Det2[l].descripcion;
                            $scope.p_Det2.otroId = $scope.Det2[l].otroId;
                            if ($scope.Det[i].modelo != $scope.Det2[l].modelo)
                                $scope.ddlModelo2 = $scope.Det2[l].modelo;
                            else
                                $scope.ddlModelo2 = "";



                        }
                    }
                }



            }
        }
   
        //Carga el Administrativo
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            if ($scope.Com[i].idDetalle === $scope.p_Det.idDetalle) {
                var index;
                //organizacionCompras
                var oc = $scope.orgCompras;
                for (index = 0; index < oc.length; ++index) {
                    if (oc[index].codigo === $scope.Com[i].organizacionCompras)
                        $scope.ddlOrgCompras = oc[index];
                }
                //frecuenciaEntrega
                var fe = $scope.frecEntrega;
                for (index = 0; index < fe.length; ++index) {
                    if (fe[index].codigo === $scope.Com[i].frecuenciaEntrega)
                        $scope.ddlFrecEntrega = fe[index];
                }
                //tipoMaterial
                var tm = $scope.tipoMaterial;
                for (index = 0; index < tm.length; ++index) {
                    if (tm[index].codigo === $scope.Com[i].tipoMaterial)
                        $scope.ddlTipoMaterial = tm[index];
                }
                //categoriaMaterial
                var cm = $scope.catMaterial;
                for (index = 0; index < cm.length; ++index) {
                    if (cm[index].codigo === $scope.Com[i].categoriaMaterial)
                        $scope.ddlCatMaterial = cm[index];
                }
                //grupoArticulo
                var ga = $scope.grupoArticulo;
                for (index = 0; index < ga.length; ++index) {
                    if (ga[index].codigo === $scope.Com[i].grupoArticulo)
                        $scope.ddlGrupoArticulo = ga[index];
                }
                //seccionArticulo
                var sa = $scope.seccArticulo;
                for (index = 0; index < sa.length; ++index) {
                    if (sa[index].codigo === $scope.Com[i].seccionArticulo)
                        $scope.ddlSeccArticulo = sa[index];
                }
                //catalogacion
                //var ca = $scope.catalogacion;
                //for (index = 0; index < ca.length; ++index) {
                //    if (ca[index].codigo === $scope.Com[i].catalogacion)
                //        $scope.ddlCatalogacion = ca[index];
                //}
                //surtidoParcial
                var sp = $scope.surtParcial;
                for (index = 0; index < sp.length; ++index) {
                    if (sp[index].codigo === $scope.Com[i].surtidoParcial)
                        $scope.ddlSurtParcial = sp[index];
                }
                //materia
                var ma = $scope.materia;
                for (index = 0; index < ma.length; ++index) {
                    if (ma[index].codigo === $scope.Com[i].materia)
                        $scope.ddlMateria = ma[index];
                }
                //indPedido
                var ip = $scope.indPedido;
                for (index = 0; index < ip.length; ++index) {
                    if (ip[index].codigo === $scope.Com[i].indPedido)
                        $scope.ddlIndPedido = ip[index];
                }
                //perfilDistribucion
                
                var pd = $scope.perfDistribucion;
                for (index = 0; index < pd.length; ++index) {
                    if (pd[index].codigo === $scope.Com[i].perfilDistribucion)
                        $scope.ddlperfDistribucion = pd[index];
                }
                //almacen
                //var al = $scope.almacen;
                //for (index = 0; index < al.length; ++index) {
                //    if (al[index].codigo === $scope.Com[i].almacen)
                //        $scope.ddlAlmacen = al[index];
                //}
                //grupoCompra
                var gc = $scope.grupoCompras;
                for (index = 0; index < gc.length; ++index) {
                    if (gc[index].codigo === $scope.Com[i].grupoCompra)
                        $scope.ddlGrupoCompras = gc[index];
                }
                //categoriaValoracion
                var cv = $scope.catValoracion;
                for (index = 0; index < cv.length; ++index) {
                    if (cv[index].codigo === $scope.Com[i].categoriaValoracion)
                        $scope.ddlCatValoracion = cv[index];
                }
                //tipoAlamcen
                var ta = $scope.tipoAlmacen;
                for (index = 0; index < ta.length; ++index) {
                    if (ta[index].codigo === $scope.Com[i].tipoAlamcen)
                        $scope.ddlTipoAlmacen = ta[index];
                }
                //indalmaentrada
                //var ae = $scope.indTipoAlmEnt;
                //for (index = 0; index < ae.length; ++index) {
                //    if (ae[index].codigo === $scope.Com[i].indalmaentrada)
                //        $scope.ddlIndTipoAlmEnt = ae[index];
                //}
                //indalmasalida
                //var as = $scope.indTipoAlmSal;
                //for (index = 0; index < as.length; ++index) {
                //    if (as[index].codigo === $scope.Com[i].indalmasalida)
                //        $scope.ddlIndTipoAlmSal = as[index];
                //}
                //indareaalmacen
                //var aa = $scope.indAreaAlmacen;
                //for (index = 0; index < aa.length; ++index) {
                //    if (aa[index].codigo === $scope.Com[i].indareaalmacen)
                //        $scope.ddlIndAreaAlmacen = aa[index];
                //}
                //condicionAlmacen
                var co = $scope.condAlmacen;
                for (index = 0; index < co.length; ++index) {
                    if (co[index].codigo === $scope.Com[i].condicionAlmacen)
                        $scope.ddlCondAlmacen = co[index];
                }
                //clListaSurtido
                var ls = $scope.clListSurtidos;
                for (index = 0; index < ls.length; ++index) {
                    if (ls[index].codigo === $scope.Com[i].clListaSurtido)
                        $scope.ddlClListSurtidos = ls[index];
                }
                //estatusMaterial
                var em = $scope.estatusMaterial;
                for (index = 0; index < em.length; ++index) {
                    if (em[index].codigo === $scope.Com[i].estatusMaterial)
                        $scope.ddlEstatusMaterial = em[index];
                }
                //estatusVenta
                var ev = $scope.estatusVentas;
                for (index = 0; index < ev.length; ++index) {
                    if (ev[index].codigo === $scope.Com[i].estatusVenta)
                        $scope.ddlEstatusVentas = ev[index];
                }
                //grupoBalanzas
                var gb = $scope.grupoBalanzas;
                for (index = 0; index < gb.length; ++index) {
                    if (gb[index].codigo === $scope.Com[i].grupoBalanzas)
                        $scope.ddlGrupoBalanzas = gb[index];
                }
                //Nacional importado

                var ni = $scope.nacionalImportado;
                for (index = 0; index < ni.length; ++index) {
                    if (ni[index].codigo === $scope.Com[i].jerarquiaProd) {
                        $scope.ddlNacionalImportado = ni[index];

                    }
                }
                //coleccion
                //var cl = $scope.coleccion;
                //for (index = 0; index < cl.length; ++index) {
                //    if (cl[index].codigo === $scope.Com[i].coleccion)
                //        $scope.ddlColeccion = cl[index];
                //}
                ////temporada
                //var te = $scope.temporada;
                //for (index = 0; index < te.length; ++index) {
                //    if (te[index].codigo === $scope.Com[i].temporada)
                //        $scope.ddlTemporada = te[index];
                //}
                ////estacion
                //var et = $scope.estacion;
                //for (index = 0; index < et.length; ++index) {
                //    if (et[index].codigo === $scope.Com[i].estacion)
                //        $scope.ddlEstacion = et[index];
                //}
        
                $scope.p_Com.idDetalle = $scope.Com[i].idDetalle;
                $scope.p_Com.codSap = $scope.codSapArt;
                
                if ($scope.p_Com.codSap != "") {
                    $scope.bloqCamposModif = true;
                }
                else {
                    $scope.bloqCamposModif = false;
                }
                $scope.p_Com.codLegacy = $scope.Com[i].codLegacy;
                $scope.p_Com.costoFOB = $scope.Com[i].costoFOB;
                $scope.p_Com.validoDesde = new Date($scope.Com[i].validoDesde);
                $scope.p_Com.observaciones = $scope.Com[i].observaciones;
                //Nuevos
                //$scope.p_Com.jerarquiaProd = "0001";
                $scope.p_Com.susceptBonifEsp = $scope.Com[i].susceptBonifEsp;
                $scope.p_Com.procedimCatalog = $scope.Com[i].procedimCatalog;
                $scope.p_Com.caracterPlanNec = $scope.Com[i].caracterPlanNec;
                $scope.p_Com.fuenteProvision = $scope.Com[i].fuenteProvision;
               
                
            }
        }


        //Carga la ruta para nuevas imagenes
        if ($scope.Rutas.length > 0) {
            //Validar que exista el detalle en arrego 
            var valido = false;
            for (var l = 0 ; l < $scope.Rutas.length; l++) {
                if ($scope.Rutas[l].idDetalle == $scope.p_Det.idDetalle) {
                    valido = true;
                }
            }
            if (valido) {
                var direc = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path //obtiene la ruta segun el articulo
                $scope.Ruta = {};
                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                $scope.Ruta.codReferencia = $scope.p_Det.idDetalle;
                $scope.Ruta.path = direc;
                $scope.uploader2.url = Ruta;
            }
            else {
                IngArticuloService.getSecuenciaDirectorio("Notificacion").then(function (results) {
                    if (results.data.success) {
                        var secuencia = results.data.root[0];
                        var direc = "Art_" + secuencia;

                        //Graba rutas por articulos
                        $scope.Ruta = {};
                        $scope.Ruta.idDetalle = $scope.p_Det.idDetalle;
                        $scope.Ruta.path = direc;
                        $scope.Rutas.push($scope.Ruta);

                        var serviceBase = ngAuthSettings.apiServiceBaseUri;
                        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                        $scope.uploader2.url = Ruta;
                    }
                }, function (error) {
                });

            }
        }
        else {
            IngArticuloService.getSecuenciaDirectorio("Notificacion").then(function (results) {
                if (results.data.success) {
                    var secuencia = results.data.root[0];
                    var direc = "Art_" + secuencia;

                    //Graba rutas por articulos
                    $scope.Ruta = {};
                    $scope.Ruta.idDetalle = $scope.p_Det.idDetalle;
                    $scope.Ruta.path = direc;
                    $scope.Rutas.push($scope.Ruta);

                    var serviceBase = ngAuthSettings.apiServiceBaseUri;
                    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                    $scope.uploader2.url = Ruta;
                }
            }, function (error) {
            });
        }

        if ($scope.Med.length > 0) {
        
            $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
            //for (var j = 0; j < $scope.grArtMedida.length; j++) {
            //    $scope.SeleccionMed($scope.grArtMedida[j].unidadMedida);
            //}
        }

        //if ($scope.Med.length > 0) {

        //    $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });

        //}
        if ($scope.Med.length > 0) {
            $scope.grArtImagen = $filter('filter')($scope.Ima, { idDetalle: $scope.p_Det.idDetalle });
        }

    
        //Adminsitrativo

        $scope.grArtCatalogacion = $filter('filter')($scope.Cat, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtCentros = $filter('filter')($scope.Cen, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtAlmacen = $filter('filter')($scope.Alm, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmEnt = $filter('filter')($scope.Iae, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmSal = $filter('filter')($scope.Ias, { idDetalle: $scope.p_Det.idDetalle });

        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });


        $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
    }

    

    $scope.SeleccionMed = function (Id) {
        //Carga datos

        for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
            if ($scope.grArtMedida[i].unidadMedida === Id && $scope.grArtMedida[i].idDetalle == $scope.p_Det.idDetalle) {
                var index;
                //Unidad Medida
                var um = $scope.unidadMedida;
                for (index = 0; index < um.length; ++index) {
                    if (um[index].codigo === $scope.grArtMedida[i].unidadMedida)
                        $scope.ddlUnidadMedida = um[index];
                }
                //Tipo Unidad Medida
                var tu = $scope.tipoUnidadMedida;
                for (index = 0; index < tu.length; ++index) {
                    if (tu[index].codigo === $scope.grArtMedida[i].tipoUnidadMedida)
                        $scope.ddlTipoUnidadMedida = tu[index];
                }
                //Tipo Unidad Medida
                var mc = $scope.uniMedConvers;
                for (index = 0; index < mc.length; ++index) {
                    if (mc[index].codigo === $scope.grArtMedida[i].uniMedConvers)
                        $scope.ddlUniMedConvers = mc[index];
                }

                //Unidad Medida de Volumen
                var mv = $scope.catalogoVolumen;
                for (index = 0; index < mv.length; ++index) {
                    if (mv[index].codigo === $scope.grArtMedida[i].uniMedidaVolumen)
                        $scope.ddlVolumen = mv[index];
                }

                $scope.temp_med = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
                $scope.p_Med.idDetalle = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].idDetalle;
                $scope.p_Med.codReferencia = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].idDetalle;

                $scope.p_Med.factorCon = $scope.grArtMedida[i].factorCon;
                $scope.p_Med.pesoNeto = $scope.grArtMedida[i].pesoNeto;
                $scope.p_Med.pesoBruto = $scope.grArtMedida[i].pesoBruto;
                $scope.p_Med.longitud = $scope.grArtMedida[i].longitud;
                $scope.p_Med.ancho = $scope.grArtMedida[i].ancho;
                $scope.p_Med.altura = $scope.grArtMedida[i].altura;
                $scope.p_Med.volumen = $scope.grArtMedida[i].volumen;
                $scope.p_Med.precioBruto = $scope.grArtMedida[i].precioBruto;
                $scope.p_Med.descuento1 = $scope.grArtMedida[i].descuento1;
                $scope.p_Med.descuento2 = $scope.grArtMedida[i].descuento2;
                $scope.p_Med.impVerde = $scope.grArtMedida[i].impVerde;
                $scope.p_Med.medBase = $scope.grArtMedida[i].uniMedBase;
                $scope.p_Med.medPedido = $scope.grArtMedida[i].uniMedPedido;
                $scope.p_Med.medES = $scope.grArtMedida[i].uniMedES;
                $scope.p_Med.medVenta = $scope.grArtMedida[i].uniMedVenta;
            }
        }
        //Carga grid codigo barra
  
        if ($scope.p_Med.idDetalle == "")
        { $scope.p_Med.idDetalle = 0; }
        $scope.temp_cbr = $filter('filter')($scope.CBr, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtCodBarra = $filter('filter')($scope.temp_cbr, { unidadMedida: Id });
    }

    //Quitar Medidas
    $scope.QuitarMed = function (Id) {
        //Quitar medidas
        for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
            if ($scope.grArtMedida[i].unidadMedida === Id) {
                break;

            }
        }

   
        $scope.grArtMedida.splice(i, 1);
        for (var i = 0, len = $scope.Med.length; i < len; i++) {
            if ($scope.Med[i].unidadMedida === Id && $scope.Med[i].idDetalle == $scope.p_Det.idDetalle) {
                break;

            }
        }
        $scope.Med[i].accion = 'D';
        $scope.listaMedidadSolEliminados[$scope.listaMedidadSolEliminados.length] = $scope.Med[i];
        $scope.Med.splice(i, 1);


        //QUitar codigo de barras
        for (var j = 0, len = $scope.CBr.length; j < len; j++) {
            if ($scope.CBr.length == 0)
            { break; }
            if (j < $scope.CBr.length)
                var x;
            else
            { break; }
            if ($scope.CBr[j].idDetalle === $scope.p_Det.idDetalle && $scope.CBr[j].unidadMedida == Id) {
                $scope.CBr[j].accion = 'D';
                $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[j];
                $scope.CBr.splice(j, 1);
                j = j - 1;
            }
        }

        //QUitar codigo de barras temporal
        for (var j = 0, len = $scope.temp_cbr.length; j < len; j++) {
            if ($scope.temp_cbr.length == 0)
            { break; }
            if (j < $scope.temp_cbr.length)
                var x;
            else
            { break; }
            if ($scope.temp_cbr[j].unidadMedida === Id) {
                $scope.temp_cbr.splice(j, 1);
                j = j - 1;
            }
        }

        //Limpia Unidad Medida
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.p_Med.desUniMedConvers = "";
        $scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = 0;
        $scope.p_Med.ancho = 0;
        $scope.p_Med.altura = 0
        $scope.p_Med.volumen = 0;
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.estado = "";
        $scope.p_Med.impVerde = false;
        //Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
    }

    $scope.SeleccionCbr = function (Id) {
        //Carga los datos desde el grid
   
        for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
            if ($scope.grArtCodBarra[i].numeroEan === Id) {
                $scope.p_CBr.numeroEan = $scope.grArtCodBarra[i].numeroEan;
                $scope.p_CBr.tipoEan = $scope.grArtCodBarra[i].tipoEan;
                $scope.p_CBr.descripcionEan = $scope.grArtCodBarra[i].descripcionEan;
                $scope.p_CBr.principal = $scope.grArtCodBarra[i].principal;
            }
        }
    }
    $scope.QuitarCbr = function (Id) {
        //Carga los datos desde el grid

        var unidadMedidaSelec = ""
        for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
            if ($scope.grArtCodBarra[i].numeroEan === Id) {
                unidadMedidaSelec = $scope.grArtCodBarra[i].unidadMedida;
                break;
            }
        }
        $scope.grArtCodBarra.splice(i, 1);
        for (var i = 0, len = $scope.CBr.length; i < len; i++) {
            if ($scope.CBr[i].numeroEan === Id && $scope.CBr[i].idDetalle == $scope.p_Det.idDetalle && $scope.CBr[i].unidadMedida == unidadMedidaSelec) {
                break;
            }
        }
        $scope.CBr[i].accion = 'D';
        $scope.listaCodBarrasSolEliminados[$scope.listaCodBarrasSolEliminados.length] = $scope.CBr[i];
        $scope.CBr.splice(i, 1);
        for (var i = 0, len = $scope.temp_cbr.length; i < len; i++) {
            if ($scope.temp_cbr[i].numeroEan === Id) {
                break;
            }
        }
        $scope.temp_cbr.splice(i, 1);


    }

    $scope.SeleccionIma = function (Id) {

        //Obtiene la ruta segun el articulo
        var pathima = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path;
        //Validar cuando sea consulta o cuando sea una nueva solicitud
        $scope.myPromise =
        IngArticuloService.getBajaTempArchivo(pathima, Id).then(function (results) {
         
            $scope.imagenurl = results.data;
            $('#idMuestraImagen').modal('show');
        }, function (error) {
        });
        ;
    }

    $scope.QuitarIma = function (Id) {
   
        //Quitar adjuntos
        for (var j = 0, len = $scope.Ima.length; j < len; j++) {
            if ($scope.Ima[j].idDocAdjunto === Id && $scope.Ima[j].idDetalle == $scope.p_Det.idDetalle) {

                $scope.Ima[j].accion = 'D';
                $scope.listaImagenesSolEliminados[$scope.listaImagenesSolEliminados.length] = $scope.Ima[j];
                $scope.Ima.splice(j, 1);
                break;
            }

        }
        for (var j = 0, len = $scope.grArtImagen.length; j < len; j++) {
            if ($scope.grArtImagen[j].idDocAdjunto === Id && $scope.grArtImagen[j].idDetalle == $scope.p_Det.idDetalle) {
                $scope.grArtImagen.splice(j, 1);
                break;
            }

        }

    }

    //Muestra el popup para confimar si desea o no grabar la solicitud
    $scope.ConfirmGrabar = function () {
        //Validar que exista articulos en la solicitud
        if ($scope.grArticulo.length == 0)
        {

            $scope.MenjError = "Ingrese al menos un artículo a la solicitud.";
            $('#idMensajeError').modal('show');
            return;

        }


        $scope.MenjConfirmacion = "¿Está seguro que desea grabar la solicitud?";
        $scope.accion = 1;
        $('#idMensajeConfirmacion').modal('show');
    }
    //Muestra el popup para confimar si desea quitar o no articulo
    $scope.ConfirmQuitarArticulo = function (id) {
        $scope.MenjConfirmacion = "¿Está seguro que desea quitar artículo?";
        $scope.accion = 999;
        $scope.CodigoArticulo = id;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Muestra el popup para confimar si desea o no enviar la solicitud
    $scope.ConfirmEnviar = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea enviar la solicitud?";
        $scope.accion = 2;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Evento click boton Grabar
    $scope.grabar = function () {
        
        if ($scope.accion == 998) {
            $scope.AddArticuloAdmin();
            $('#modal-form-observacion_2').modal('hide');
            return;
        }
        if ($scope.accion == 999) {
            $scope.QuitarArt($scope.CodigoArticulo);
            return;
        }
        if ($scope.accion == 999) {
            $scope.QuitarArt($scope.CodigoArticulo);
            return;
        }
        //Valida tipo de accion
        if ($scope.accion === 3) {
            window.location = 'frmConsArticulo';
        } else {

            if ($scope.accion === 2)
                $scope.enviaNotificacionSN = true;

            //Valida si selecciono la Linea de Negocio
            //if ($scope.ddlLineaNegocio === "") {
            //    $scope.MenjError = "Debe seleccionar una Línea de Negocio.";
            //    $('#idMensajeError').modal('show');
            //    return;
            //}
            //if ($scope.ddlLineaNegocio === null) {
            //    $scope.MenjError = "Debe seleccionar una Línea de Negocio.";
            //    $('#idMensajeError').modal('show');
            //    return;
            //}

            //Valida que por lo menos haya un articulo ingresado
            if ($scope.grArticulo.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos un artículo.";
                $('#idMensajeError').modal('show');
                return;
            }

            //Para validar el estado
            var estado = ($scope.accion === 1 ? "grabado" : "enviado");
            var idsolicitud = ($scope.txtIdSolicitud === "" ? "0" : $scope.txtIdSolicitud);

            //Arma la cabecera
            $scope.Cab = [];
            $scope.p_Cab = {};
            $scope.p_Cab.codproveedor = $scope.Det[0].codProveedor;
            $scope.p_Cab.Usuario = authService.authentication.userName;
            $scope.p_Cab.tiposolicitud = "2"; //Modificacion Articulo
            $scope.p_Cab.lineanegocio = $scope.ddlLineaNegocio.codigo;
            $scope.p_Cab.idsolicitud = idsolicitud;
   
            $scope.p_Cab.accion = ($scope.txtIdSolicitud === "0" || angular.isUndefined($scope.txtIdSolicitud) ? "I" : "U");
            $scope.p_Cab.estado = "EC";
            $scope.p_Cab.observacion = "";
            //if ($scope.enviaNotificacionSN)
            //{ $scope.p_Cab.enviaNotificacion = "S"; }
            //else
            //{ 
            $scope.p_Cab.enviaNotificacion = "N"; 
            $scope.p_Cab.ruc = "";//authService.authentication.ruc;
            $scope.p_Cab.razonSocial = "";//authService.authentication.NombreParticipante;
            $scope.Cab.push($scope.p_Cab);

            //Solo para administrativo
            //$scope.Com = [];
            //$scope.Cat = [];
            //$scope.Alm = [];
            //$scope.Iae = [];
            //$scope.Ias = [];
            //$scope.Iaa = [];

            //Eliminar detalles de articulos
            for (var i = 0 ; i < $scope.listaDetallesSolEliminados.length; i++)
                $scope.Det[$scope.Det.length] = $scope.listaDetallesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaMedidadSolEliminados.length; i++)
                $scope.Med[$scope.Med.length] = $scope.listaMedidadSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCodBarrasSolEliminados.length; i++)
                $scope.CBr[$scope.CBr.length] = $scope.listaCodBarrasSolEliminados[i];
            for (var i = 0 ; i < $scope.listaImagenesSolEliminados.length; i++)
                $scope.Ima[$scope.Ima.length] = $scope.listaImagenesSolEliminados[i];
            for (var i = 0 ; i < $scope.listaCatalogosSolEliminados.length; i++)
                $scope.Cat[$scope.Cat.length] = $scope.listaCatalogosSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAlmacenSolEliminados.length; i++)
                $scope.Alm[$scope.Alm.length] = $scope.listaAlmacenSolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenESolEliminados.length; i++)
                $scope.Iae[$scope.Iae.length] = $scope.listaTipoAlmacenESolEliminados[i];
            for (var i = 0 ; i < $scope.listaTipoAlmacenSSolEliminados.length; i++)
                $scope.Ias[$scope.Ias.length] = $scope.listaTipoAlmacenSSolEliminados[i];
            for (var i = 0 ; i < $scope.listaAreaAlmacenSolEliminados.length; i++)
                $scope.Iaa[$scope.Iaa.length] = $scope.listaAreaAlmacenSolEliminados[i];

            //Lama el metodo
       
            $scope.myPromise =
            IngArticuloService.getGrabaSolicitud($scope.Cab, $scope.Det, $scope.Med, $scope.CBr, $scope.Ima, $scope.Com, $scope.Cat,
                                                 $scope.Alm, $scope.Iae, $scope.Ias, $scope.Iaa, $scope.Cen, $scope.CaractArt).then(function (response) {
                                              
                                                     if (response.data.success) {
                                                         $scope.MenjError = 'Se ha ' + estado + ' exitosamente la solicitud';
                                                         $scope.accion = 3;
                                                         $('#idMensajeOk').modal('show');
                                                     }
                                                     else {

                                                         $scope.MenjError = response.data.mensaje;
                                                         $('#idMensajeError').modal('show');
                                                         return;
                                                     }

                                                 },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
            ;
        }
    }

    $("#btnMensajeOK").click(function () {
        if ($scope.accion == 3)
            $scope.grabar();
    });
    $scope.nextCompras = function () {
        $('.nav-tabs a[href="#Compras"]').tab('show');

    }
    //A nivel de articulo
    $scope.AprobarArticulo = function () {
        $scope.MenjConfirmacion = "¿Está seguro que desea Aprobar el Artículo?";
        $scope.accionArt = 1;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.RechazarArticulo = function () {
        $scope.MsjConfirmacionArt = "¿Está seguro que desea rechazar el artículo?";
        $scope.accionArt = 2;
        $scope.accion = 998;
        $scope.ViewMotRechazoArt = false;
        $('#modal-form-observacion_2').modal('show');
    }

    $scope.AddCatalogacion = function () {
      
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlCatalogacion === "") {
            $scope.MenjError = "Debe seleccionar una catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCatalogacion === undefined) {
            $scope.MenjError = "Debe seleccionar una catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtCatalogacion.length != 0) {
            for (var i = 0, len = $scope.grArtCatalogacion.length; i < len; i++) {
                if ($scope.grArtCatalogacion[i].catalogacion === $scope.ddlCatalogacion.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Cat = {};
            $scope.p_Cat.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Cat.catalogacion = $scope.ddlCatalogacion.codigo;
            $scope.p_Cat.desCatalogacion = $scope.ddlCatalogacion.detalle;
            $scope.p_Cat.accion = "I";
            $scope.Cat.push($scope.p_Cat);
        }

        //Filtra y muestra
        $scope.grArtCatalogacion = $filter('filter')($scope.Cat, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddAlmacen = function () {
   
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlAlmacen === "") {
            $scope.MenjError = "Debe seleccionar un almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlAlmacen === undefined) {
            $scope.MenjError = "Debe seleccionar un almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtAlmacen.length != 0) {
            for (var i = 0, len = $scope.grArtAlmacen.length; i < len; i++) {
                if ($scope.grArtAlmacen[i].almacen === $scope.ddlAlmacen.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Alm = {};
            $scope.p_Alm.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Alm.almacen = $scope.ddlAlmacen.codigo;
            $scope.p_Alm.desAlmacen = $scope.ddlAlmacen.detalle;
            $scope.p_Alm.accion = "I";
            $scope.Alm.push($scope.p_Alm);
        }

        //Filtra y muestra
        $scope.grArtAlmacen = $filter('filter')($scope.Alm, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddIndTipoAlmEnt = function () {

        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmEnt === "") {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén entrada.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmEnt === undefined) {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén entrada.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndTipoAlmEnt.length != 0) {
            for (var i = 0, len = $scope.grArtIndTipoAlmEnt.length; i < len; i++) {
                if ($scope.grArtIndTipoAlmEnt[i].indTipoAlmEnt === $scope.ddlIndTipoAlmEnt.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Iae = {};
            $scope.p_Iae.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Iae.indTipoAlmEnt = $scope.ddlIndTipoAlmEnt.codigo;
            $scope.p_Iae.desIndTipoAlmEnt = $scope.ddlIndTipoAlmEnt.detalle;
            $scope.p_Iae.accion = "I";
            $scope.Iae.push($scope.p_Iae);
        }

        //Filtra y muestra
        $scope.grArtIndTipoAlmEnt = $filter('filter')($scope.Iae, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddIndTipoAlmSal = function () {
        
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmSal === "") {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén salida.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmSal === undefined) {
            $scope.MenjError = "Debe seleccionar un indicador tipo almacén salida.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndTipoAlmSal.length != 0) {
            for (var i = 0, len = $scope.grArtIndTipoAlmSal.length; i < len; i++) {
                if ($scope.grArtIndTipoAlmSal[i].indTipoAlmSal === $scope.ddlIndTipoAlmSal.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Ias = {};
            $scope.p_Ias.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Ias.indTipoAlmSal = $scope.ddlIndTipoAlmSal.codigo;
            $scope.p_Ias.desIndTipoAlmSal = $scope.ddlIndTipoAlmSal.detalle;
            $scope.p_Ias.accion = "I";
            $scope.Ias.push($scope.p_Ias);
        }

        //Filtra y muestra
        $scope.grArtIndTipoAlmSal = $filter('filter')($scope.Ias, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddIndAreaAlmacen = function () {
       
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndAreaAlmacen === "") {
            $scope.MenjError = "Debe seleccionar un indicador área almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndAreaAlmacen === undefined) {
            $scope.MenjError = "Debe seleccionar un indicador área almacén.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe
        var existe = false;
        if ($scope.grArtIndAreaAlmacen.length != 0) {
            for (var i = 0, len = $scope.grArtIndAreaAlmacen.length; i < len; i++) {
                if ($scope.grArtIndAreaAlmacen[i].indAreaAlmacen === $scope.ddlIndAreaAlmacen.codigo) {
                    existe = true;
                    break;
                }
            }
        }

        if (existe === true) {
            $scope.MenjError = "Ya existe!";
            $('#idMensajeError').modal('show');
            return;
        } else {
            $scope.p_Iaa = {};
            $scope.p_Iaa.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Iaa.indAreaAlmacen = $scope.ddlIndAreaAlmacen.codigo;
            $scope.p_Iaa.desIndAreaAlmacen = $scope.ddlIndAreaAlmacen.detalle;
            $scope.p_Iaa.accion = "I";
            $scope.Iaa.push($scope.p_Iaa);
        }

        //Filtra y muestra
        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });
    }

    $scope.AddArticulo = function () {
    
        $scope.p_Det.descripcion = $scope.p_Det.descripcion.toUpperCase();
        $scope.validaDatos = false;
        //Validar que se han seleccionado todas las unidades de medida
        var exisUMbase = false;
        var exisUMpedido = false;
        var exisUMes = false;
        var exisUMventa = false;

        for (var k = 0 ; k < $scope.grArtMedida.length ; k++) {


            if ($scope.grArtMedida[k].uniMedBase) exisUMbase = true;
            if ($scope.grArtMedida[k].uniMedPedido) exisUMpedido = true;
            if ($scope.grArtMedida[k].uniMedVenta) exisUMventa = true;
            if ($scope.grArtMedida[k].uniMedES) exisUMes = true;
        }


        if (!exisUMbase) {
            $scope.MenjError = "Debe seleccionar Unidad Base en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMpedido) {
            $scope.MenjError = "Debe seleccionar Unidad Pedido en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMes) {
            $scope.MenjError = "Debe seleccionar Unidad E/S en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if (!exisUMventa) {
            $scope.MenjError = "Debe seleccionar Unidad Venta en al menos una unidad de medida.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        //Validar Canal de distribucion
        if ($scope.grArtIndAreaAlmacen.length == 0) {
            $scope.MenjError = "Debe ingresar al menos un canal de distribución.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Validar que se ingrese valores obligatorios
        if ($scope.p_Det.codReferencia == "") {
            $scope.MenjError = "Debe ingresar código de referencia del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlMarca == "" || $scope.ddlMarca == undefined) {
            $scope.MenjError = "Debe ingresar marca del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descripcion == "") {
            $scope.MenjError = "Debe ingresar descripción del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.tamArticulo == "") {
            $scope.MenjError = "Debe ingresar tamaño o presentación del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.precioBruto == "") {
            $scope.MenjError = "Debe ingresar precio bruto del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }


        if ($scope.ddlIva == "" || $scope.ddlIva == undefined) {
            $scope.MenjError = "Debe ingresar valor de clasificación fiscal.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlDeducible == "" || $scope.ddlDeducible == undefined) {
            $scope.MenjError = "Debe ingresar Deducible.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlRetencion == "" || $scope.ddlRetencion == undefined) {
            $scope.MenjError = "Debe ingresar retención.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si ha ingresado una medida
        if ($scope.grArtMedida.length === 0) {
            $scope.MenjError = "Debe ingresar por lo menos una unidad de medida del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida si ha ingresado una medida
        if ($scope.grArtMedida.length === 0) {
            $scope.MenjError = "Debe ingresar por lo menos una unidad de medida del artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.descuento1 > 100) {
            $scope.MenjError = "Descuento 1 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.descuento2 > 100) {
            $scope.MenjError = "Descuento 2 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }



        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        var ingresa = false;
        if ($scope.grArticulo.length != 0) {
            for (var i = 0, len = $scope.grArticulo.length; i < len; i++) {
                if ($scope.grArticulo[i].idDetalle === $scope.p_Det.idDetalle) {
                    edita = true;
                    if ($scope.grArticulo[i].accion == "I") ingresa = true;
                    break;
                }
            }
        }



        //Valida que haya ingresado una nueva marca en caso de que haya seleccionado OTROS
        if ($scope.ddlMarca.codigo === "-1" && $scope.Det.marcaNueva === "") {
            $scope.MenjError = "Ingrese la nueva marca del articulo si seleccionó <OTROS>.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Obtiene y carga los datos de los combos

        $scope.p_Det.marca = $scope.ddlMarca.codigo;
        $scope.p_Det.desMarca = $scope.ddlMarca.detalle;
        $scope.p_Det.paisOrigen = $scope.ddlPaisOrigen.codigo;
        $scope.p_Det.regionOrigen = $scope.ddlRegionOrigen.codigo;
        $scope.p_Det.talla = $scope.ddlTalla.codigo;
        $scope.p_Det.gradoAlcohol = $scope.ddlGradoAlcohol.codigo;
        $scope.p_Det.color = $scope.ddlColor.codigo;
        $scope.p_Det.fragancia = $scope.ddlFragancia.codigo;
        $scope.p_Det.tipos = $scope.ddlTipos.codigo;
        $scope.p_Det.sabor = $scope.ddlSabor.codigo;
        $scope.p_Det.modelo = $scope.ddlModelo;

        if ($scope.ddlColeccion != null)
            $scope.p_Det.coleccion = $scope.ddlColeccion.codigo;
        else
            $scope.p_Det.coleccion = "";
        if ($scope.ddlTemporada != null)
            $scope.p_Det.temporada = $scope.ddlTemporada.codigo;
        else
            $scope.p_Det.temporada = "";
        $scope.p_Det.estacion = $scope.ddlEstacion;
        $scope.p_Det.cantidadPedir = $scope.ddlCantidadPedir;


        $scope.p_Det.iva = $scope.ddlIva.codigo;
        $scope.p_Det.deducible = $scope.ddlDeducible.codigo;
        $scope.p_Det.retencion = $scope.ddlRetencion.codigo;
        $scope.p_Det.accion = "I";
        $scope.p_Det.estado = "NAR";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.Det.length; i < len; i++) {
                var update = $scope.Det[i];
                if (update.idDetalle === $scope.p_Det.idDetalle) {
                    update.codReferencia = $scope.p_Det.codReferencia;
                    update.marca = $scope.p_Det.marca;
                    update.desMarca = $scope.p_Det.desMarca;
                    update.paisOrigen = $scope.p_Det.paisOrigen;
                    update.regionOrigen = $scope.p_Det.regionOrigen;
                    update.tamArticulo = $scope.p_Det.tamArticulo;
                    update.precioBruto = $scope.p_Det.precioBruto;
                    update.descuento1 = $scope.p_Det.descuento1;
                    update.descuento2 = $scope.p_Det.descuento2;
                    update.impVerde = $scope.p_Det.impVerde;
                    update.gradoAlcohol = $scope.p_Det.gradoAlcohol;
                    update.talla = $scope.p_Det.talla;
                    update.color = $scope.p_Det.color;
                    update.fragancia = $scope.p_Det.fragancia;
                    update.tipos = $scope.p_Det.tipos;
                    update.sabor = $scope.p_Det.sabor;
                    update.modelo = $scope.p_Det.modelo;
                    update.coleccion = $scope.p_Det.coleccion;
                    update.temporada = $scope.p_Det.temporada;
                    update.estacion = $scope.p_Det.estacion;
                    update.cantidadPedir = $scope.p_Det.cantidadPedir;
                    update.iva = $scope.p_Det.iva;
                    update.deducible = $scope.p_Det.deducible;
                    update.retencion = $scope.p_Det.retencion;
                    update.descripcion = $scope.p_Det.descripcion;
                    update.otroId = $scope.p_Det.otroId;
                    update.contAlcohol = $scope.p_Det.contAlcohol;
                    if (ingresa)
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "I");
                    else
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Agrega nuevo articulo
            $scope.Det.push($scope.p_Det);
        }


        //Eliminar caracteristicas 
        var seguir = true;
        while (seguir) {
            seguir = false;
            for (var ini = 0 ; ini < $scope.CaractArt.length; ini++) {
                if ($scope.CaractArt[ini].idDetalle == $scope.p_Det.idDetalle) {
                    seguir = true;
                    break;
                }
            }
            $scope.CaractArt.splice(ini, 1);
        }

        for (var ini = 0 ; ini < $scope.CaractArtTMP.length; ini++) {
            $scope.p_CaractArt = {};
            $scope.p_CaractArt.idDetalle = $scope.CaractArtTMP[ini].idDetalle;
            $scope.p_CaractArt.idCaract = $scope.CaractArtTMP[ini].idCaract;
            $scope.p_CaractArt.idValor = $scope.CaractArtTMP[ini].idValor;
            $scope.p_CaractArt.accion = $scope.CaractArtTMP[ini].accion;
            $scope.p_CaractArt.idAgrupacion = $scope.CaractArtTMP[ini].idAgrupacion;

            $scope.CaractArt.push($scope.p_CaractArt);
        }
        $scope.CaractArtTMP = [];

        //Agrega el articulo y carga el grid
        $scope.grArticulo = $scope.Det;
        if ($scope.grArticulo.length > 0) {
            $scope.bloqueaBotInicio = true;
        }
        $scope.validaDatos = true;
       
    }

    $scope.AddArticuloAdmin = function () {
     
        $scope.AddArticulo();
        if (!$scope.validaDatos) return;
        //Carga los datos de Compras
        $scope.p_Com.idDetalle = $scope.p_Det.idDetalle;
        //Modifica los datos de compras
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            var update = $scope.Com[i];
            if (update.idDetalle === $scope.p_Com.idDetalle) {

                update.costoFOB = $scope.p_Com.costoFOB;
                update.observaciones = $scope.p_Com.observaciones;
                //Nuevos
                //update.jerarquiaProd = $scope.p_Com.jerarquiaProd;
             
                update.jerarquiaProd = $scope.ddlNacionalImportado.codigo;
                update.susceptBonifEsp = $scope.p_Com.susceptBonifEsp;
                update.procedimCatalog = $scope.p_Com.procedimCatalog;
                update.caracterPlanNec = $scope.p_Com.caracterPlanNec;
                update.fuenteProvision = $scope.p_Com.fuenteProvision;





                update.organizacionCompras = $scope.ddlOrgCompras.codigo;
                update.frecuenciaEntrega = $scope.ddlFrecEntrega.codigo;
                update.tipoMaterial = $scope.ddlTipoMaterial.codigo;
                update.categoriaMaterial = $scope.ddlCatMaterial.codigo;
                update.grupoArticulo = $scope.ddlGrupoArticulo.codigo;
                update.seccionArticulo = $scope.ddlSeccArticulo.codigo;
                //update.catalogacion = $scope.ddlCatalogacion.codigo;
                update.surtidoParcial = $scope.ddlSurtParcial.codigo;
                update.materia = $scope.ddlMateria.codigo;
                update.indPedido = $scope.ddlIndPedido.codigo;
                update.perfilDistribucion = $scope.ddlperfDistribucion.codigo;
                //update.almacen = $scope.ddlAlmacen.codigo;
                update.grupoCompra = $scope.ddlGrupoCompras.codigo;
                update.categoriaValoracion = $scope.ddlCatValoracion.codigo;
                update.tipoAlamcen = $scope.ddlTipoAlmacen.codigo;
                //update.indalmaentrada = $scope.ddlIndTipoAlmEnt.codigo;
                //update.indalmasalida = $scope.ddlIndTipoAlmSal.codigo;
                //update.indareaalmacen = $scope.ddlIndAreaAlmacen.codigo;
                update.condicionAlmacen = $scope.ddlCondAlmacen.codigo;
                update.clListaSurtido = $scope.ddlClListSurtidos.codigo;
                update.estatusMaterial = $scope.ddlEstatusMaterial.codigo;
                update.estatusVenta = $scope.ddlEstatusVentas.codigo;
                update.grupoBalanzas = $scope.ddlGrupoBalanzas.codigo;
                update.nacionalImportado = $scope.ddlNacionalImportado.codigo;
                update.coleccion = $scope.ddlColeccion.codigo;
                update.temporada = $scope.ddlTemporada.codigo;
                update.estacion = $scope.ddlEstacion.codigo;
                update.motivoRechazo = $scope.ddlMotivoRechazoArt.codigo;
                var d_fecha = "";
                if ($scope.p_Com.validoDesde != "")
                { d_fecha = $filter('date')($scope.p_Com.validoDesde, 'yyyy-MM-dd'); }
                update.validoDesde = d_fecha;
                //1: Asistente Compras //3: Datos Maestros
                update.estado = ($scope.accionArt === 1 ? ($scope.tipouser === "1" ? "APC" : "APD") : ($scope.tipouser === "1" ? "REC" : "RED"));
                for (var i = 0 ; i < $scope.grArticulo.length; i++) {
                    if ($scope.grArticulo[i].idDetalle == $scope.p_Com.idDetalle)
                        break;
                }

                if (update.estado == 'APC' || update.estado == 'APD')
                    $scope.grArticulo[i].estado = 'aprobado';
                if (update.estado == 'REC' || update.estado == 'RED')
                    $scope.grArticulo[i].estado = 'rechazado';
                update.accion = "U";
                break;
            }
        }



        //Agrega el articulo y carga el grid
        //$scope.grArticulo = $scope.Det;
        $scope.Limpia();

        $scope.inactivaTab = true;
        $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
       
    }

    $scope.AddMedida = function () {
   
        if (!$scope.frmDatosMedida.txtFactorCon.$valid) {
            $scope.MenjError = "Valor incorrecto en  factor de conversión. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtPesoNeto.$valid) {
            $scope.MenjError = "Valor incorrecto en peso neto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtPesoBruto.$valid) {
            $scope.MenjError = "Valor incorrecto en peso bruto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtLongitud.$valid) {
            $scope.MenjError = "Valor incorrecto en longitud. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtAncho.$valid) {
            $scope.MenjError = "Valor incorrecto en ancho. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtAltura.$valid) {
            $scope.MenjError = "Valor incorrecto en altura. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.frmDatosMedida.txtVolumen.$valid) {
            $scope.MenjError = "Valor incorrecto en volumen. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        //if (!$scope.frmDatosMedida.txtPrecioBruto.$valid) {
        //    $scope.MenjError = "Valor incorrecto en precio bruto. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}
        //if (!$scope.frmDatosMedida.txtDescuento1.$valid) {
        //    $scope.MenjError = "Valor incorrecto en descuento 1. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}
        //if (!$scope.frmDatosMedida.txtDescuento2.$valid) {
        //    $scope.MenjError = "Valor incorrecto en descuento 2. 99999999.999";
        //    $('#idMensajeError').modal('show');
        //    return;
        //}


        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe generar un nuevo artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //validar ingreso solo de una unidad de medida
        if ($scope.grArtMedida.length >= 1) {
            if ($scope.grArtMedida[0].unidadMedida != $scope.ddlUnidadMedida.codigo) {

                //$scope.MenjError = "Solo se puede agregar una unidad de medida.";
                //$('#idMensajeError').modal('show');
                //return;
            }

        }

        //No validar si la unidad de medida es CAJAMASTER METRO CUBICO O MEDIO PALET
        if ($scope.ddlUnidadMedida.codigo != 'KI' &&
                                                  $scope.ddlUnidadMedida.codigo != 'M3' &&
                                                  $scope.ddlUnidadMedida.codigo != 'MPL') {

            //Valida si ha ingresado un codigo de barra
            if ($scope.grArtCodBarra.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos el código de barra principal.";
                $('#idMensajeError').modal('show');
                return;
            } else {
                //Valida si hay un código de barra ingresado como el principal
                var contador = 0;
                for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                    if ($scope.grArtCodBarra[i].principal === true) {
                        contador += 1;
                    }
                }
                if (contador === 0) {
                    $scope.MenjError = "Un código de barra debe ser el principal.";
                    $('#idMensajeError').modal('show');
                    return;
                }
            }

        }


        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        if ($scope.grArtMedida.length != 0) {
            for (var i = 0, len = $scope.grArtMedida.length; i < len; i++) {
                if ($scope.grArtMedida[i].idDetalle === $scope.p_Det.idDetalle && $scope.grArtMedida[i].unidadMedida == $scope.ddlUnidadMedida.codigo) {
                    edita = true;
                    break;
                }
            }
        }

        //Obtiene y carga los datos de los combos
        $scope.p_Med.unidadMedida = $scope.ddlUnidadMedida.codigo;
        $scope.p_Med.desUnidadMedida = $scope.ddlUnidadMedida.detalle;
        $scope.p_Med.uniMedidaVolumen = $scope.ddlVolumen.codigo;
        $scope.p_Med.desUniMedidaVolumen = $scope.ddlVolumen.detalle;
        //for (var i = 0; i < $scope.tipoUnidadMedida.length;i++)
        //{
        //    $scope.p_Med.tipoUnidadMedida = $scope.tipoUnidadMedida[i].codigo;
        //    $scope.p_Med.desTipoUnidadMedida = $scope.tipoUnidadMedida[i].detalle;
        //}
        $scope.p_Med.tipoUnidadMedida = $scope.ddlTipoUnidadMedida.codigo;
        $scope.p_Med.desTipoUnidadMedida = $scope.ddlTipoUnidadMedida.detalle;
        $scope.p_Med.uniMedConvers = $scope.ddlUniMedConvers.codigo;
        $scope.p_Med.desUniMedConvers = $scope.ddlUniMedConvers.detalle;
        $scope.p_Med.uniMedBase = $scope.p_Med.medBase;
        $scope.p_Med.uniMedPedido = $scope.p_Med.medPedido;
        $scope.p_Med.uniMedES = $scope.p_Med.medES;
        $scope.p_Med.uniMedVenta = $scope.p_Med.medVenta;
        $scope.p_Med.estado = "1";
        $scope.p_Med.accion = "I";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.Med.length; i < len; i++) {
                var update = $scope.Med[i];
                if (update.idDetalle == $scope.p_Det.idDetalle && update.unidadMedida == $scope.ddlUnidadMedida.codigo) {
                    update.unidadMedida = $scope.p_Med.unidadMedida;
                    update.tipoUnidadMedida = $scope.p_Med.tipoUnidadMedida;
                    update.desTipoUnidadMedida = $scope.p_Med.desTipoUnidadMedida;
                    update.uniMedidaVolumen = $scope.p_Med.uniMedidaVolumen;
                    update.desUniMedidaVolumen = $scope.p_Med.desUniMedidaVolumen;
                    update.uniMedConvers = $scope.p_Med.uniMedConvers
                    update.desUniMedConvers = $scope.p_Med.desUniMedConvers;
                    update.factorCon = $scope.p_Med.factorCon;
                    update.pesoNeto = $scope.p_Med.pesoNeto;
                    update.pesoBruto = $scope.p_Med.pesoBruto;
                    update.longitud = $scope.p_Med.longitud;
                    update.ancho = $scope.p_Med.ancho;
                    update.altura = $scope.p_Med.altura;
                    update.volumen = $scope.p_Med.volumen;
                    update.precioBruto = $scope.p_Med.precioBruto;
                    update.descuento1 = $scope.p_Med.descuento1;
                    update.descuento2 = $scope.p_Med.descuento2;
                    update.impVerde = $scope.p_Med.impVerde;
                    update.uniMedBase = $scope.p_Med.medBase;
                    update.uniMedPedido = $scope.p_Med.medPedido;
                    update.uniMedES = $scope.p_Med.medES;
                    update.uniMedVenta = $scope.p_Med.medVenta;
                    if (update.accion != "I")
                        update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Agrega nuevo codigo de barra
            $scope.p_Med.idDetalle = $scope.p_Det.idDetalle;
            $scope.Med.push($scope.p_Med);
        }

        //Filtra y muestra las medidas
        //$scope.p_Med = {};
        $scope.p_Med.idDetalle = $scope.p_Det.idDetalle;
        $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Med.idDetalle });

        //Limpia Unidad Medida
        //$scope.p_Med.unidadMedida = "";
        //$scope.p_Med.tipoUnidadMedida = "";
        //$scope.p_Med.uniMedConvers = "";
        //$scope.p_Med.desUnidadMedida = "";
        //$scope.p_Med.desTipoUnidadMedida = "";
        //$scope.p_Med.desUniMedConvers = "";
        //$scope.ddlUnidadMedida = "";
        //$scope.ddlTipoUnidadMedida = "";
        //$scope.ddlUniMedConvers = "";
        //$scope.p_Med.factorCon = "";
        //$scope.p_Med.pesoNeto = "";
        //$scope.p_Med.pesoBruto = "";
        //$scope.p_Med.longitud = "";
        //$scope.p_Med.ancho = "";
        //$scope.p_Med.altura = "";
        //$scope.p_Med.precioBruto = "";
        //$scope.p_Med.descuento1 = "";
        //$scope.p_Med.descuento2 = "";
        //$scope.p_Med.estado = "";
        //$scope.p_Med.impVerde = false;
        $scope.p_Med = {};
        $scope.ddlUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        ////Limpia cod barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.grArtCodBarra = [];
        //$scope.NextImagen();
    }

    //Consulta codigos EAN    
    $scope.buscarCodEAN = function (codEAN, tipoEAN) {
     
        if ($scope.p_CBr.unidadMedida === "") {
            $scope.MenjError = "Debe seleccionar la unidad de medida.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida que haya consultado (BAPI) el codigo de barra ingresado
        if (codEAN === "") { //Cambiar despues por el tipo o descripción - deshabiltar controles cuando este la BAPI
            $scope.MenjError = "Debe digitar el código de barra.";
            $('#idMensajeError').modal('show');
            return;
        }

        $scope.myPromise =
            IngArticuloService.getConsultaEAN(codEAN, tipoEAN.codigo).then(function (response) {
         
                if (response.data.success || $scope.inactivaCodEAN) {

                    $scope.p_CBr.tipoEan = tipoEAN.codigo;
                    $scope.p_CBr.descripcionEan = tipoEAN.detalle;
                    $scope.AddCodBarra();
                }
                else {

                    $scope.MenjError = "Código EAN no valido o ya registrado.";
                    $('#idMensajeError').modal('show');
                    return;
                }


            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        ;



    }

    $scope.AddCodBarra = function () {
     
        //Valida que haya seleccionado la unidad de medida
        if ($scope.p_CBr.unidadMedida === "") {
            $scope.MenjError = "Debe seleccionar la unidad de medida.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya consultado (BAPI) el codigo de barra ingresado
        if ($scope.p_CBr.tipoEan === "" && $scope.p_CBr.descripcionEan === "") { //Cambiar despues por el tipo o descripción - deshabiltar controles cuando este la BAPI
            $scope.MenjError = "Debe digitar el código de barra y consultar el tipo y la descripción del mismo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida si existe; agrega si no existe; modifica si existe
        var edita = false;
        if ($scope.grArtCodBarra.length != 0) {
            for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                if ($scope.grArtCodBarra[i].numeroEan === $scope.p_CBr.numeroEan) {
                    edita = true;
                    break;
                }
            }
        }

        $scope.p_CBr.accion = "I";

        if (edita === true) {
            //Modifica codigo de barra
            for (var i = 0, len = $scope.CBr.length; i < len; i++) {
                var update = $scope.CBr[i];
                if (update.numeroEan === $scope.p_CBr.numeroEan) {
                    update.tipoEan = $scope.p_CBr.tipoEan;
                    update.descripcionEan = $scope.p_CBr.descripcionEan;
                    update.paisEan = $scope.p_CBr.paisEan;
                    update.paisDesEan = $scope.p_CBr.paisDesEan;
                    update.principal = $scope.p_CBr.principal;
                    update.accion = ($scope.txtIdSolicitud === "0" ? "I" : "U");
                    break;
                }
            }
        } else {
            //Valida si ya existe un principal
            if ($scope.grArtCodBarra.length != 0) {
                for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
                    if ($scope.grArtCodBarra[i].principal === true && $scope.p_CBr.principal === true) {
                        $scope.MenjError = "Ya existe un EAN principal.";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                }
            }
            //Agrega nuevo codigo de barra
            $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
            $scope.CBr.push($scope.p_CBr);
        }

        //Filtra y muestra los codigos de barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = $scope.p_Det.idDetalle;
        $scope.p_CBr.unidadMedida = $scope.ddlUnidadMedida.codigo;
        $scope.temp_cbr = $filter('filter')($scope.CBr, { idDetalle: $scope.p_CBr.idDetalle });
        $scope.grArtCodBarra = $filter('filter')($scope.temp_cbr, { unidadMedida: $scope.p_CBr.unidadMedida });





        //Limpia Codigo de Barra
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
    }

    


    $scope.NewArticulo = function () {
        $scope.codSapArt = "";
        $scope.MsjConfirmacionArt = "Agregar Artículo";
        $('#modal-form-observacion_Agregar').modal('show');

    }

    $scope.NewArticulo2 = function (codSap) {

        

    
        //Validar que no exista codsap en solicitud
        for (var l = 0; l < $scope.grArticulo.length; l++) {
            if ($scope.grArticulo[l].codSAPart == codSap) {
                $('#modal-form-observacion_Agregar').modal('hide');
                $scope.MenjError = "Código SAP ya agregado en solicitud.";
                $('#idMensajeError').modal('show');
                return;
            }

        }


        $scope.Ids = [];
        $scope.Id = "";
        $('#modal-form-observacion_Agregar').modal('hide');
        //$scope.txtIdSolicitud = (localStorageService.get('IdSolicitud') === null ? "0" : localStorageService.get('IdSolicitud'));
        //$scope.tipomod = (localStorageService.get('TipoMod') === null ? "0" : localStorageService.get('TipoMod'));
        $scope.tipomod = "1";
        if ($scope.tipomod === "1") {
            $scope.Id = codSap;
            var resSolicitud = $scope.txtIdSolicitud;
            $scope.txtIdSolicitud = "0";
            //} else {
            //    $scope.Ids = (localStorageService.get('CodSapArticulo') === null ? "0" : localStorageService.get('CodSapArticulo'));
            //    //$scope.Ids = 
            //}

            $scope.CargaConsulta33(resSolicitud);

        }
    }



    //Valida si es ingreso solicitud (carga datos solicitud) o modificacion (carga datos articulo)
    $scope.CargaConsulta33 = function (solic) {
 
        if ($scope.txtIdSolicitud != "0") {
            //tipo, codigo Solo necesita el tipo y el codigo
            debugger;
            $scope.myPromise = ModArticuloService.getArticulos("2", $scope.txtIdSolicitud, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0", $scope.codSapProv).then(function (results) {
           
                //Linea de Negocio
                var linea = {};
                linea = results.data.root[0];

                $scope.datosArtModifica = results.data.root2;
                if (linea[0].estado == 'NE' || linea[0].estado == 'DC') {
                    $scope.isReadOnly = false;
                    $scope.inactivaBot = false;
                }
                else {
                    $scope.isReadOnly = true;
                    $scope.inactivaBot = true;

                }
                var ln = $scope.lineaNegocio;
                for (var index = 0; index < ln.length; ++index) {
                    if (ln[index].codigo === linea[0].lineaNegocio)
                        $scope.ddlLineaNegocio = ln[index];
                }
                $scope.Det = results.data.root[1];
                $scope.codSapProv = $scope.Det[0].codProveedor;
                $scope.grArticulo = $scope.Det;
                $scope.etiTotRegistros = $scope.grArticulo.length;
                $scope.Med = results.data.root[2];
                $scope.CBr = results.data.root[3];
                $scope.Ima = results.data.root[4];
                $scope.Rutas = results.data.root[5];
                $scope.Com = results.data.root[6];
                $scope.Cat = results.data.root[7];
                $scope.Alm = results.data.root[8];
                $scope.Iae = results.data.root[9];
                $scope.Ias = results.data.root[10];
          
                $scope.Iaa = results.data.root[11];
                $scope.CaractArt = results.data.root[14];
                //$scope.grArtObservacion = results.data.root[12];
                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');

            }, function (error) {
            });
        } else {
            $scope.txtIdSolicitud = solic;
            //Consulta del articulo - CONSUMO BAPI
            if ($scope.Id == 0 && $scope.Ids == 0)
                return;

      
            $scope.codigos = "";
            if ($scope.tipomod === "1") {
                $scope.codigos = $scope.Id;
            } else {
                for (var i = 0, len = $scope.Ids.length; i < len; i++) {
                    $scope.codigos = $scope.codigos + "|" + $scope.Ids[i].id;
                }
            }
            debugger;
            $scope.myPromise = ConsArticuloService.getArticulos("2", $scope.codigos, "", "", "", "", "", "", "", "", "", "0", $scope.codSapProv).then(function (results) {
                if (results.data.success) {
               
                    //Linea de Negocio
                    var linea = {};
                    var linea2 = {};

                    linea = results.data.root[0];
                    linea2 = results.data.root[1];
                    
                    if (linea.length == 0) {
                        $scope.MenjError = "Código SAP no existe.";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                    
                    if ($scope.codSapProv != linea2[0].codProveedor && $scope.codSapProv != "") {
                        $scope.MenjError = "Artículo no pertenece al proveedor de la solicitud.";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                    //$scope.txtIdSolicitud = linea[0].idSolicitud;
                    $scope.lineaNegocio2 = linea[0].lineaNegocio;
                    if (linea2[0].isGenerico) {
                        $scope.MenjError = "Código SAP no existe.";
                        $('#idMensajeError').modal('show');
                        return;
                    }
                    
                    if ($scope.grArticulo.length != 0) {
                        if ($scope.lineaNegocio2 != $scope.ddlLineaNegocio.codigo) {
                            $scope.MenjError = "Artículo no pertenece a la línea de negocio de la solicitud.";
                            $('#idMensajeError').modal('show');
                            return;
                        }


                    }

                    //Validar para no ingresar codGenericos repetidos
                    debugger;
                    var eliminaGenerico = true;
                    var listGenerico = results.data.root[1];
                    while (eliminaGenerico)
                     {
                        eliminaGenerico = false;
                        for (var idx = 0 ; idx < listGenerico.length ; idx++)
                        {
                           
                            var codGenerico = $filter('filter')($scope.grArticulo, { codSAPart: listGenerico[idx].codSAPart }, true);
                            if (codGenerico.length > 0) {
                                eliminaGenerico = true;
                                break;
                            }
                            

                        }
                        if (eliminaGenerico) {
                            listGenerico.splice(idx, 1);
                            results.data.root[1].splice(idx, 1);
                        }

                    }
                    var ln = $scope.lineaNegocio;
                    for (var index = 0; index < ln.length; ++index) {
                        if (ln[index].codigo === $scope.lineaNegocio2)
                            $scope.ddlLineaNegocio = ln[index];
                    }
                   
                    for (var j = 0 ; j < results.data.root[1].length; j++) {
                        var index = $scope.Det.length;
                        var det = results.data.root[1];
                        $scope.Det[index] = det[j];
                         
                         $scope.grArticulo =  $filter('filter')($scope.Det, function (obj) {
                            return obj.accion == 'I' || obj.accion == 'U';
                                            });

                        
                    }
                    $scope.codSapProv = $scope.Det[0].codProveedor;


                    //for (var j = 0 ; j < $scope.Det.length; j++) {
                    //    var index = $scope.grArticulo.length;
                    //    var art = $scope.Det;
                    //    $scope.grArticulo[index] = art[j];
                    //}



                    for (var j = 0 ; j < results.data.root[2].length; j++) {
                        var index = $scope.Med.length;
                        var med = results.data.root[2];

                        //med[j].desUnidadMedida

                        for (var d = 0; d < $scope.unidadMedida.length; d++) {
                            if ($scope.unidadMedida[d].codigo == med[j].unidadMedida)
                                med[j].desUnidadMedida = $scope.unidadMedida[d].detalle;
                            //med[j].unidadMedida = $filter('filter')($scope.unidadMedida, { codigo: "ST" })[0].detalle;
                            if ($scope.unidadMedida[d].codigo == med[j].uniMedConvers)
                                med[j].desUniMedConvers = $scope.unidadMedida[d].detalle;
                            //med[j].desUniMedConvers = $filter('filter')($scope.unidadMedida, { codigo: med[j].uniMedConvers })[0].detalle;
                        }
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: med[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                            $scope.Med[index] = med[j];
                    }



                    for (var j = 0 ; j < results.data.root[3].length; j++) {
                        var index = $scope.CBr.length;
                        var cbr = results.data.root[3];
                        cbr[j].descripcionEan = $filter('filter')($scope.tipoEANart, { codigo: cbr[j].tipoEan })[0].detalle;
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: cbr[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.CBr[index] = cbr[j];

                    }


                    for (var j = 0 ; j < results.data.root[4].length; j++) {
                        var index = $scope.Ima.length;
                        var ima = results.data.root[4];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: ima[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Ima[index] = ima[j];
                    }



                    for (var j = 0 ; j < results.data.root[5].length; j++) {
                        var index = $scope.Rutas.length;
                        var rut = results.data.root[5];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: rut[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Rutas[index] = rut[j];
                    }

                    for (var j = 0 ; j < results.data.root[6].length; j++) {
                        var index = $scope.Com.length;
                        var com = results.data.root[6];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: com[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Com[index] = com[j];
                    }

                    for (var j = 0 ; j < results.data.root[7].length; j++) {
                        var index = $scope.Cat.length;
                        var cat = results.data.root[7];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: cat[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Cat[index] = cat[j];
                    }

                    for (var j = 0 ; j < results.data.root[8].length; j++) {
                        var index = $scope.Alm.length;

                        var alm = results.data.root[8];
                        //if (alm[j].desAlmacen == "" || alm[j].desAlmacen == null)
                        //{
                        //    for (var l = 0; l < $scope.almacen.length; l++)
                        //    {
                        //        if ($scope.almacen[l].codigo == alm[j].almacen)
                        //        {
                        //            alm[j].desAlmacen = $scope.almacen[l].detalle;
                        //        }
                        //    }
                        //}

                        //if (alm[j].desindAlmacenE == "" || alm[j].desindAlmacenE == null) {
                        //    for (var l = 0; l < $scope.indTipoAlmEnt.length; l++) {
                        //        if ($scope.indTipoAlmEnt[l].codigo == alm[j].indAlmacenE) {
                        //            alm[j].desindAlmacenE = $scope.indTipoAlmEnt[l].detalle;
                        //        }
                        //    }
                        //}

                        //if (alm[j].desindAlmacenS == "" || alm[j].desindAlmacenS == null) {
                        //    for (var l = 0; l < $scope.indTipoAlmSal.length; l++) {
                        //        if ($scope.indTipoAlmSal[l].codigo == alm[j].indAlmacenS) {
                        //            alm[j].desindAlmacenS = $scope.indTipoAlmSal[l].detalle;
                        //        }
                        //    }
                        //}

                        //if (alm[j].destipAlmacen == "" || alm[j].destipAlmacen == null) {
                        //    for (var l = 0; l < $scope.tipoAlmacen.length; l++) {
                        //        if ($scope.tipoAlmacen[l].codigo == alm[j].tipAlmacen) {
                        //            alm[j].destipAlmacen = $scope.tipoAlmacen[l].detalle;
                        //        }
                        //    }
                        //}
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: alm[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Alm[index] = alm[j];
                    }

                    for (var j = 0 ; j < results.data.root[9].length; j++) {
                        var index = $scope.Iae.length;
                        var iae = results.data.root[9];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: iae[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Iae[index] = iae[j];
                    }

                    for (var j = 0 ; j < results.data.root[10].length; j++) {
                        var index = $scope.Ias.length;
                        var ias = results.data.root[10];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: ias[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Ias[index] = ias[j];
                    }
            
                    for (var j = 0 ; j < results.data.root[11].length; j++) {
                        var index = $scope.Iaa.length;
                        var iaa = results.data.root[11];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: iaa[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Iaa[index] = iaa[j];
                    }

                    for (var j = 0 ; j < results.data.root[12].length; j++) {
                        var index = $scope.Cen.length;
                        var cen = results.data.root[12];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: cen[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.Cen[index] = cen[j];
                    }
                
                    for (var j = 0 ; j < results.data.root[13].length; j++) {
                        var index = $scope.CaractArt.length;
                        var car = results.data.root[13];
                        var detRecibido = $filter('filter')(results.data.root[1], { idDetalle: car[j].idDetalle }, true);
                        if (detRecibido.length > 0)
                        $scope.CaractArt[index] = car[j];
                    }
                    $scope.etiTotRegistros = $scope.grArticulo.length;
                    //$scope.grArtObservacion = results.data.root[12];
                    $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
                }

            }, function (error) {
            });
        }
    }
  

    $scope.Limpia = function () {
        //Detalle
        $scope.p_Det = {};
        $scope.p_Det.idDetalle = "";
        $scope.p_Det.codReferencia = "";
        $scope.p_Det.marca = "";
        $scope.p_Det.desMarca = "";
        $scope.ddlMarca = "";
        $scope.p_Det.paisOrigen = "";
        $scope.ddlPaisOrigen = "";
        $scope.p_Det.regionOrigen = "";
        $scope.ddlRegionOrigen = "";
        $scope.p_Det.gradoAlcohol = "";
        $scope.ddlGradoAlcohol = "";
        $scope.p_Det.talla = "";
        $scope.ddlTalla = "";
        $scope.p_Det.color = "";
        $scope.ddlColor = "";
        $scope.p_Det.fragancia = "";
        $scope.ddlFragancia = "";
        $scope.p_Det.tipos = "";
        $scope.ddlTipos = "";
        $scope.p_Det.sabor = "";
        $scope.ddlSabor = "";
        $scope.p_Det.modelo = "";
        $scope.ddlModelo = "";
        $scope.p_Det.coleccion = "";
        $scope.ddlColeccion = "";
        $scope.p_Det.temporada = "";
        $scope.ddlTemporada = "";
        $scope.p_Det.estacion = "";
        $scope.ddlEstacion = "";
        $scope.p_Det.iva = "";
        $scope.ddlIva = "";
        $scope.p_Det.deducible = "";
        $scope.ddlDeducible = "";
        $scope.p_Det.retencion = "";
        $scope.ddlRetencion = "";
        $scope.p_Det.descripcion = "";
        $scope.p_Det.otroId = "";
        $scope.p_Det.tamArticulo = "";
        $scope.p_Det.contAlcohol = false;
        $scope.p_Det.accion = "";
        $scope.p_Det.estado = "";
        //Compras
        $scope.p_Com = {};
        $scope.p_Com.idDetalle = "";
        $scope.p_Com.codSap = "";
        $scope.p_Com.codLegacy = "";
        $scope.p_Com.costoFOB = "";
        $scope.p_Com.validoDesde = "";
        $scope.p_Com.observaciones = "";
        $scope.p_Com.organizacionCompras = "";
        $scope.p_Com.frecuenciaEntrega = "";
        $scope.p_Com.tipoMaterial = "";
        $scope.p_Com.categoriaMaterial = "";
        $scope.p_Com.grupoArticulo = "";
        $scope.p_Com.seccionArticulo = "";
        $scope.p_Com.catalogacion = "";
        $scope.p_Com.surtidoParcial = "";
        $scope.p_Com.materia = "";
        $scope.p_Com.indPedido = "";
        $scope.p_Com.perfilDistribucion = "";
        $scope.p_Com.almacen = "";
        $scope.p_Com.grupoCompra = "";
        $scope.p_Com.categoriaValoracion = "";
        $scope.p_Com.tipoAlamcen = "";
        $scope.p_Com.indalmaentrada = "";
        $scope.p_Com.indalmasalida = "";
        $scope.p_Com.indareaalmacen = "";
        $scope.p_Com.condicionAlmacen = "";
        $scope.p_Com.clListaSurtido = "";
        $scope.p_Com.estatusMaterial = "";
        $scope.p_Com.estatusVenta = "";
        $scope.p_Com.grupoBalanzas = "";
        $scope.p_Com.nacionalImportado = "";
        $scope.p_Com.coleccion = "";
        $scope.p_Com.temporada = "";
        $scope.p_Com.estacion = "";
        $scope.p_Com.estado = "";
        $scope.p_Com.motivoRechazo = "";
        //Combos
        $scope.ddlOrgCompras = "";
        $scope.ddlFrecEntrega = "";
        $scope.ddlTipoMaterial = "";
        $scope.ddlCatMaterial = "";
        $scope.ddlGrupoArticulo = "";
        $scope.ddlSeccArticulo = "";
        $scope.ddlCatalogacion = "";
        $scope.ddlSurtParcial = "";
        $scope.ddlMateria = "";
        $scope.ddlIndPedido = "";
        $scope.ddlperfDistribucion = "";
        $scope.ddlAlmacen = "";
        $scope.ddlGrupoCompras = "";
        $scope.ddlCatValoracion = "";
        $scope.ddlTipoAlmacen = "";
        $scope.ddlIndTipoAlmEnt = "";
        $scope.ddlIndTipoAlmSal = "";
        $scope.ddlIndAreaAlmacen = "";
        $scope.ddlCondAlmacen = "";
        $scope.ddlClListSurtidos = "";
        $scope.ddlEstatusMaterial = "";
        $scope.ddlEstatusVentas = "";
        $scope.ddlGrupoBalanzas = "";
        $scope.ddlNacionalImportado = "";
        $scope.ddlColeccion = "";
        $scope.ddlTemporada = "";
        $scope.ddlEstacion = "";
        //Catalogacion
        $scope.p_Cat = {};
        $scope.grArtCatalogacion = [];
        $scope.p_Cat.idDetalle = "";
        $scope.p_Cat.catalogacion = "";
        $scope.p_Cat.desCatalogacion = "";
        $scope.p_Cat.accion = "";
        //Almacen
       
        $scope.p_Alm = {};
        $scope.grArtAlmacen = [];
        $scope.p_Alm.idDetalle = "";
        $scope.p_Alm.almacen = "";
        $scope.p_Alm.desAlmacen = "";
        $scope.p_Alm.accion = "";
        //IndTipoAlmEnt
        $scope.p_Iae = {};
        $scope.grArtIndTipoAlmEnt = [];
        $scope.p_Iae.idDetalle = "";
        $scope.p_Iae.indTipoAlmEnt = "";
        $scope.p_Iae.desIndTipoAlmEnt = "";
        $scope.p_Iae.accion = "";
        //IndTipoAlmSal
        $scope.p_Ias = {};
        $scope.grArtIndTipoAlmSal = [];
        $scope.p_Ias.idDetalle = "";
        $scope.p_Ias.indTipoAlmSal = "";
        $scope.p_Ias.desIndTipoAlmSal = "";
        $scope.p_Ias.accion = "";
        //IndAreaAlmacen
        $scope.p_Iaa = {};
        $scope.grArtIndAreaAlmacen = [];
        $scope.p_Iaa.idDetalle = "";
        $scope.p_Iaa.indAreaAlmacen = "";
        $scope.p_Iaa.desIndAreaAlmacen = "";
        $scope.p_Iaa.accion = "";
        //Medida
        $scope.p_Med = {};
        $scope.p_Med.idDetalle = "";
        $scope.p_Med.unidadMedida = "";
        $scope.p_Med.desUnidadMedida = "";
        $scope.ddlUnidadMedida = "";
        $scope.p_Med.tipoUnidadMedida = "";
        $scope.p_Med.desTipoUnidadMedida = "";
        $scope.ddlTipoUnidadMedida = "";
        $scope.p_Med.uniMedConvers = "";
        $scope.p_Med.desUniMedConvers = "";
        $scope.ddlUniMedConvers = "";
        $scope.p_Med.factorCon = "";
        $scope.p_Med.pesoNeto = "";
        $scope.p_Med.pesoBruto = "";
        $scope.p_Med.longitud = 0;
        $scope.p_Med.ancho = 0;
        $scope.p_Med.altura = 0;
        $scope.p_Med.volumen = 0;
        $scope.p_Med.precioBruto = "";
        $scope.p_Med.descuento1 = "";
        $scope.p_Med.descuento2 = "";
        $scope.p_Med.impVerde = false;
        $scope.p_Med.accion = "";
        $scope.p_Med.estado = "";
        //Codigo Barra
        $scope.p_CBr = {};
        $scope.p_CBr.idDetalle = "";
        $scope.p_CBr.unidadMedida = "";
        $scope.p_CBr.numeroEan = "";
        $scope.p_CBr.tipoEan = "";
        $scope.p_CBr.descripcionEan = "";
        $scope.p_CBr.principal = false;
        $scope.p_CBr.accion = "";
        //Imagenes
        $scope.p_Ima = {};
        $scope.p_Ima.idDetalle = "";
        $scope.p_Ima.path = "";
        $scope.p_Ima.idDocAdjunto = "";
        $scope.p_Ima.nomArchivo = "";
        $scope.p_Ima.accion = "";
        //Limpia Grids
        $scope.grArtMedida = [];
        $scope.grArtCodBarra = [];
        $scope.grArtImagen = [];
        $scope.CaractArtTMP = [];
        $scope.ddlAgrupacion = "";
    }

    $scope.NextMedida = function () {
        if (!$scope.requeridodet.txtPrecioBruto.$valid) {
            $scope.MenjError = "Valor incorrecto en precio bruto. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.requeridodet.txtDescuento1.$valid) {
            $scope.MenjError = "Valor incorrecto en descuento 1. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if (!$scope.requeridodet.txtDescuento2.$valid) {
            $scope.MenjError = "Valor incorrecto en descuento 2. 99999999.999";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_Det.descuento1 > 100) {
            $scope.MenjError = "Descuento 1 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.descuento2 > 100) {
            $scope.MenjError = "Descuento 2 no puede ser mayor a 100%";
            $('#idMensajeError').modal('show');
            return;
        }

        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Genere un nuevo artículo";
            $('#idMensajeError').modal('show');
            $('.nav-tabs a[href="#DatosGenerales"]').tab('show');
        } else {
            $('.nav-tabs a[href="#Medidas"]').tab('show');
        }
    }

    $scope.NextImagen = function () {
        if ($scope.grArtMedida.length != 0) {
            $('.nav-tabs a[href="#Imagenes"]').tab('show');
        } else {
            $scope.MenjError = "Debe ingresar por lo menos una medida.";
            $('#idMensajeError').modal('show');
            return;
        }
    }

    //INICIO CARGA DE IMAGENES 
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileArticulo/UploadFile/?path=';
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
       
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "jpg" || extension == "bmp" || extension == "png" || extension == "jpeg" || extension == "gif")
                return true;
            else {
                //alert('Formato de Imagen incorrecto. Por favor seleccione un archivo con los siguientes formatos jpg/bmp/png/jpeg o gif e intente de nuevo.');
                $scope.MenjError = "Formato de imagen incorrecto. Por favor seleccione un archivo con los siguientes formatos jpg/bmp/png/jpeg o gif e intente de nuevo.";
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
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });
    uploader2.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {
           
            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });
    // CALLBACKS
    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };


    uploader2.onAfterAddingFile = function (fileItem) {

        if ($scope.Ruta.path === "") {
            //Remueve las imagenes de la cola
            uploader2.clearQueue();
            $scope.MenjError = "Debe generar un nuevo código de artículo.";
            $('#idMensajeError').modal('show');
            return;
        } else {
            //Registra en el modelo
            $scope.p_Ima = {};
            $scope.p_Ima.idDetalle = $scope.p_Det.idDetalle;
            $scope.p_Ima.path = $scope.Ruta.path;
            $scope.p_Ima.idDocAdjunto = $scope.uploader2.queue.length;
            $scope.p_Ima.nomArchivo = fileItem.file.name;
            $scope.p_Ima.accion = "I";
            $scope.Ima.push($scope.p_Ima);
            //Registra en el grid
            $scope.grArtImagen.push(
                { idDocAdjunto: $scope.uploader2.queue.length, nomArchivo: fileItem.file.name });

            $scope.myPromise = uploader2.uploadAll();
        }

    };

    //Se dispara cuando realiza la carga
    uploader2.onSuccessItem = function (fileItem, response, status, headers) {
       
        if ($scope.uploader2.progress == 100) {

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
    console.info('uploader', uploader2);
    //FIN CARGA DE IMAGENES

    $scope.nomfile = "";
    $scope.direcXls = "Art_ExcelMasiva";

    $scope.MasivaPopup = function () {


        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + $scope.direcXls;
        $scope.uploader3.url = Ruta;
        $('#idCargaMasiva').modal('show');
    }

    $scope.CargaMasiva = function () {

    
        if (uploader3.queue.length === 0) {
            $scope.MenjError = "Seleccione un archivo.";
            $('#idMensajeError').modal('show');
        } else {
            IngArticuloService.getCargaMasiva($scope.direcXls, $scope.nomfile).then(function (results) {

                $scope.Det = results.data.root[0];

                $scope.grArticulo = $scope.Det;
                $scope.Med = results.data.root[1];
                $scope.CBr = results.data.root[2];
                $scope.accion = 0;
                $scope.MenjError = 'Se ha cargado exitosamente';
                $('#idMensajeOk').modal('show');
            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        }
    }

    //INICIO CARGA MASIVA
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileArticulo/UploadFile/?path=';
    var uploader3 = $scope.uploader3 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader3.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
       
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "xls" || extension == "xlsx")
                return true;
            else {
                alert('Formato de archivo incorrecto. Por favor seleccione un archivo con los siguientes formatos xls/xlsx e intente de nuevo.');
                return false;
            }
        }
    });
    uploader3.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 5)
                return true;
            else {
                alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                return false;
            }
        }
    });
    uploader3.filters.push({
        name: 'itemResetFilter',
        fn: function (item, options) {

            if (this.queue.length < 5)
                return true;
            else {
                alert('You have exceeded the limit of uploading files.');
                return false;
            }
        }
    });
    // CALLBACKS
    uploader3.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader3.onAfterAddingFile = function (fileItem) {
        $scope.nomfile = fileItem.file.name;
        uploader3.uploadAll();
    };
    //Se dispara cuando realiza la carga
    uploader3.onSuccessItem = function (fileItem, response, status, headers) {
      
        if ($scope.uploader3.progress == 100) {

        }
    };
    uploader3.onErrorItem = function (fileItem, response, status, headers) {
        alert('We were unable to upload your file. Please try again.');
    };
    uploader3.onCancelItem = function (fileItem, response, status, headers) {
        alert('File uploading has been cancelled.');
    };
    uploader3.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader3.onBeforeUploadItem = function (item) {
        console.info('onBeforeUploadItem', item);
    };
    uploader3.onProgressItem = function (fileItem, progress) {
        console.info('onProgressItem', fileItem, progress);
    };
    uploader3.onProgressAll = function (progress) {
        console.info('onProgressAll', progress);
    };
    uploader3.onCompleteItem = function (fileItem, response, status, headers) {
        console.info('onCompleteItem', fileItem, response, status, headers);
    };
    uploader3.onCompleteAll = function () {
        console.info('onCompleteAll');
    };
    console.info('uploader', uploader3);
    //FIN CARGA MASIVA



    $scope.Ids = [];
    $scope.Id = "";
    $scope.txtIdSolicitud = (localStorageService.get('IdSolicitud') === null ? "0" : localStorageService.get('IdSolicitud'));
    $scope.codSapProv = (localStorageService.get('CodSapProveedor') === null ? "0" : localStorageService.get('CodSapProveedor'));
    $scope.tipomod = (localStorageService.get('TipoMod') === null ? "0" : localStorageService.get('TipoMod'));
    if ($scope.tipomod === "1") {
        $scope.Id = (localStorageService.get('CodSapArticulo') === null ? "0" : localStorageService.get('CodSapArticulo'));
        $scope.inactivaBot = true;
        $scope.inactivaTab = true;
        $scope.isReadOnly = true;
    } else {
        $scope.Ids = (localStorageService.get('CodSapArticulo') === null ? "0" : localStorageService.get('CodSapArticulo'));
        $scope.inactivaBot = false;
        $scope.inactivaTab = false;
        $scope.isReadOnly = false;

        //$scope.Ids = 
    }
    //Limpia
    localStorageService.remove('IdSolicitud');
    localStorageService.remove('CodSapProveedor');
    localStorageService.remove('CodSapArticulo');
    localStorageService.remove('TipoMod');


    //$scope.CargaConsulta();
    $scope.CargaConsulta33();
}

]);



