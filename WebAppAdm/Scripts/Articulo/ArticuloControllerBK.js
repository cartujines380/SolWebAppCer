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
    $scope.usrRolSel = 0;

    //Para multiseleccion de estados
    $scope.EstadoSolicitud = [];
    $scope.SettingEstadoSol = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };

    ConsSolArticuloService.getCatalogo('tbl_EstadoSolArt').then(function (results) {

        $scope.EstadoSolicitudArt = results.data;

    }, function (error) {

    });

    ConsSolArticuloService.getCatalogo('tbl_TipoSolArticulo').then(function (results) {
        $scope.TipoSolicitud = results.data;
    });

    ConsSolArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.LineaNegocio = results.data;
    });

    $scope.CargaConsulta = function () {

        //Para armar arreglo de estados seleccionados
        var chkCodRef = ($scope.rdbCodigo === "2" ? "true" : "false");
        if (chkCodRef === "true" && $scope.txtCodReferencia === "") {
            $scope.MenjError = "Ingrese el Código de Referencia.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkCodSap = ($scope.rdbCodigo === "3" ? "true" : "false");
        if (chkCodSap === "true" && $scope.txtCodSap === "") {
            $scope.MenjError = "Ingrese el Código SAP.";
            $('#idMensajeError').modal('show');
            return;
        }
        var chkFecha = ($scope.rdbFecha === "1" ? "false" : "true");
        //Valida ingreso de fechas
        if (chkFecha === "true" && $scope.txtFechaDesde === "" && $scope.txtFechaHasta === "") {
            $scope.MenjError = "Ingrese el rango de fechas.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Valida que la fecha desde no sea mayor a la fecha hasta
        if (chkFecha === "true" && ($scope.txtFechaDesde > $scope.txtFechaHasta)) {
            alert("La fecha desde no puede ser mayor a la fecha hasta.");
            $('#idMensajeError').modal('show');
            return;
        }
        var chkEstado = ($scope.rdbEstado === "1" ? "false" : "true");
        if (chkEstado === "true" && $scope.EstadoSolicitud.length === 0) {
            $scope.MenjError = "Debe seleccionar por lo menos un estado.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Tipo Solicitud
        var chkTipoSol = ($scope.rdbTipoSol === "1" ? "false" : "true");
        if (chkTipoSol === "true" && $scope.ddlTipoSolicitud === "") {
            $scope.MenjError = "Debe seleccionar un Tipo de Solicitud.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkTipoSol === "true" && $scope.ddlTipoSolicitud === null) {
            $scope.MenjError = "Debe seleccionar un Tipo de Solicitud.";
            $('#idMensajeError').modal('show');
            return;
        }
        //Linea Negocio
        var chkLinea = ($scope.rdbLinea === "1" ? "false" : "true");
        if (chkLinea === "true" && $scope.ddlLineaNegocio === "") {
            $scope.MenjError = "Debe seleccionar una Linea de Negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        if (chkLinea === "true" && $scope.ddlLineaNegocio === null) {
            $scope.MenjError = "Debe seleccionar una Linea de Negocio.";
            $('#idMensajeError').modal('show');
            return;
        }
        var index;
        var estados = new Array();
        var a = $scope.EstadoSolicitud;
        for (index = 0; index < a.length; ++index) {
            estados[index] = a[index].id;
        }

        debugger;
        var d_desde = "";
        if ($scope.txtFechaDesde != "")
        { d_desde = $filter('date')($scope.txtFechaDesde, 'yyyy-MM-dd'); }
        var d_hasta = "";
        if ($scope.txtFechaHasta != "")
        { d_hasta = $filter('date')($scope.txtFechaHasta, 'yyyy-MM-dd'); }

        //tipo, codigo, chkCodRef, CodRef, chkCodSap, CodSap, chkFecha, FechaDesde, FechaHasta, chkEstado, estado, chkTipoSol, TipoSolicitud, chkLinea, LineaNegocio
        $scope.myPromise = ConsSolArticuloService.getArticulos("1", "0", chkCodRef, $scope.txtCodReferencia, chkCodSap, $scope.txtCodSap, chkFecha, d_desde, d_hasta, chkEstado, estados, chkTipoSol, $scope.ddlTipoSolicitud.codigo, chkLinea, $scope.ddlLineaNegocio.codigo).then(function (results) {
            if (results.data.success) {
                //debugger;
                $scope.Articulo = results.data.root[0];
                $scope.GridArticulo = results.data.root[0];
                $scope.selectedEstadosb = ['ENVIADO', 'EN REVISIÓN ASIST. COMPRAS'];
                $scope.filterByEstados = function (content) {
                    return ($scope.selectedEstadosb.indexOf(content.estadoSolicitud) !== -1);
                };
            }

        }, function (error) {
        });
        setTimeout(function () { $('#rbtArtAll').focus(); }, 100);
    }

    //******************************************************************************
    $scope.popuptipouser = function (Id) {
        debugger;
        $scope.IdSol = Id;
        $('#idTipoUsuario').modal('show');
    }
    $scope.verasiscompras = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 1);
        window.location = '/Articulos/frmAdminArticulo';
    }
    $scope.vergercompras = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 2);
        window.location = '/Articulos/frmAdminArticulo';
    }
    $scope.verdatosmaest = function () {
        localStorageService.set('IdSolicitud', $scope.IdSol);
        localStorageService.set('tipouser', 3);
        window.location = '/Articulos/frmAdminArticulo';
    }
    //******************************************************************************

    $scope.VerObservacion = function (Id) {
        $scope.ObservacionSol = $filter('filter')($scope.Articulo, { idSolicitud: Id })[0].observacion;
        $('#idVerObservacion').modal('show');
    }

    $scope.Seleccion = function (Id) {
        debugger;

        localStorageService.set('IdSolicitud', Id);
        $scope.usrRolSel = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
        localStorageService.set('tipouser', $scope.usrRolSel);
        window.location = '/Articulos/frmAdminArticulo';
    }

    $scope.SolicitudOPCION = function (opc) {
        debugger;
        $scope.usrRolSel = opc;
        localStorageService.set('tipouser', $scope.usrRolSel);
       
        window.location = '/Articulos/frmConsSolArticulo';
    }

  

    $scope.CargaConsulta();
}
]);

//Pantalla de Adminstracion de Articulo
'use strict';
app.controller('AdmArticuloController', ['$scope', '$location', '$http', 'AdmArticuloService', 'ngAuthSettings', '$filter', 'FileUploader', '$routeParams', 'localStorageService', 'authService', function ($scope, $location, $http, AdmArticuloService, ngAuthSettings, $filter, FileUploader, $routeParams, localStorageService, authService) {

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
    $scope.grArtAlmacen = [];
    $scope.grArtIndTipoAlmEnt = [];
    $scope.grArtIndTipoAlmSal = [];
    $scope.grArtIndAreaAlmacen = [];
    $scope.grArtObservacion = [];

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
    $scope.unidadMedida = [];
    $scope.tipoUnidadMedida = [];
    $scope.uniMedConvers = [];
    $scope.clasificacionFiscal = [];
    //Administrativo
    $scope.orgCompras = [];
    $scope.frecEntrega = [];
    $scope.tipoMaterial = [];
    $scope.catMaterial = [];
    $scope.grupoArticulo = [];
    $scope.seccArticulo = [];
    $scope.surtParcial = [];
    $scope.materia = [];
    $scope.indPedido = [];
    $scope.perfDistribucion = [];
    $scope.grupoCompras = [];
    $scope.catValoracion = [];
    $scope.tipoAlmacen = [];
    $scope.condAlmacen = [];
    $scope.clListSurtidos = [];
    $scope.estatusMaterial = [];
    $scope.estatusVentas = [];
    $scope.grupoBalanzas = [];
    $scope.coleccion = [];
    $scope.temporada = [];
    $scope.estacion = [];
    //De uno a muchos
    $scope.catalogacion = [];
    $scope.almacen = [];
    $scope.indTipoAlmEnt = [];
    $scope.indTipoAlmSal = [];
    $scope.indAreaAlmacen = [];

    //SI/NO Deducible y Retención
    $scope.sn_ded = [];
    $scope.sn_ret = [];
    $scope.SINO = [];
    $scope.sn = { codigo: 'S', detalle: 'SI' };
    $scope.SINO.push($scope.sn);
    $scope.sn = { codigo: 'N', detalle: 'NO' };
    $scope.SINO.push($scope.sn);
    $scope.sn_ded = $scope.SINO;
    $scope.sn_ret = $scope.SINO;

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
    $scope.ddlColeccion = "";
    $scope.ddlTemporada = "";
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

    //Cabecera
    $scope.ddlLineaNegocio = "";
    $scope.txtIdSolicitud = "";
    $scope.txtContRegistros = "";

    //Mensaje Confirm
    $scope.MsjConfirmacion = "";
    $scope.accion = "";
    //Mensaje Confirm Articulo
    $scope.MsjConfirmacionArt = "";
    $scope.accionArt = "";

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

    //Carga Combos
    AdmArticuloService.getCatalogo('tbl_MarcaArticulo').then(function (results) {
        $scope.marca = results.data;
        //Agrega el campo para "Otros"
        $scope.mar = { codigo: '-1', detalle: 'OTROS (Especifique)', descAlterno: '' };
        $scope.marca.push($scope.mar);
    });

    //Cuando la marca sea OTROS activa 
    $scope.$watch('ddlMarca', function () {
        $scope.p_Det.marcaNueva = "";
        if ($scope.ddlMarca.codigo === "-1") {
            $scope.p_Det.viewMarcaNueva = false;
        } else {
            $scope.p_Det.viewMarcaNueva = true;
        }
    });

    AdmArticuloService.getCatalogo('tbl_Pais').then(function (results) {
        $scope.paisOrigen = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Region').then(function (results) {
        $scope.regionOrigenTemp = results.data;
    });

    $scope.$watch('ddlPaisOrigen', function () {
        $scope.regionOrigen = [];
        if ($scope.ddlPaisOrigen != '' && angular.isUndefined($scope.ddlPaisOrigen) != true) {
            $scope.regionOrigen = $filter('filter')($scope.regionOrigenTemp, { descAlterno: $scope.ddlPaisOrigen.codigo });
        }
    });

    AdmArticuloService.getCatalogo('tbl_TallaArticulo').then(function (results) {
        $scope.talla = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_GradoAlcohol').then(function (results) {
        $scope.gradoAlcohol = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Color').then(function (results) {
        $scope.color = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Fragancia').then(function (results) {
        $scope.fragancia = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_TiposArticulo').then(function (results) {
        $scope.tipos = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Sabor').then(function (results) {
        $scope.sabor = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_ModeloArticulo').then(function (results) {
        $scope.modelo = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_UnidadMedidaArt').then(function (results) {
        $scope.unidadMedida = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_TipoUnidMedArt').then(function (results) {
        $scope.tipoUnidadMedida = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_UniMedConverArt').then(function (results) {
        $scope.uniMedConvers = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_LineaNegocio').then(function (results) {
        $scope.lineaNegocio = results.data;
    });

    //Administrativo

    AdmArticuloService.getCatalogo('tbl_Orgcompras').then(function (results) {
        $scope.orgCompras = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Seccarticulo').then(function (results) {
        $scope.seccArticulo = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Catalogacion').then(function (results) {
        $scope.catalogacion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Almacen').then(function (results) {
        $scope.almacen = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Tipoalmacen').then(function (results) {
        $scope.tipoAlmacen = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Indtipoalment').then(function (results) {
        $scope.indTipoAlmEnt = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Indtipoalmsal').then(function (results) {
        $scope.indTipoAlmSal = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Indareaalmacen').then(function (results) {
        $scope.indAreaAlmacen = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Condalmacen').then(function (results) {
        $scope.condAlmacen = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Cllistsurtidos').then(function (results) {
        $scope.clListSurtidos = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Estatusmaterial').then(function (results) {
        $scope.estatusMaterial = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Estatusventas').then(function (results) {
        $scope.estatusVentas = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Grupobalanzas').then(function (results) {
        $scope.grupoBalanzas = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_CatMaterial').then(function (results) {
        $scope.catMaterial = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_CatValoracion').then(function (results) {
        $scope.catValoracion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_FrecEntrega').then(function (results) {
        $scope.frecEntrega = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_GrupoArticulo').then(function (results) {
        $scope.grupoArticulo = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_GrupoCompras').then(function (results) {
        $scope.grupoCompras = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_IndPedido').then(function (results) {
        $scope.indPedido = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Materia_Art').then(function (results) {
        $scope.materia = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_PerfDistribucion').then(function (results) {
        $scope.perfDistribucion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_SurtParcial').then(function (results) {
        $scope.surtParcial = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_tipoMaterial_Art').then(function (results) {
        $scope.tipoMaterial = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_ColeccionArtic').then(function (results) {
        $scope.coleccion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Temporada').then(function (results) {
        $scope.temporada = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_Estacion').then(function (results) {
        $scope.estacion = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_ClasificacionFiscal').then(function (results) {
        $scope.clasificacionFiscal = results.data;
    });

    AdmArticuloService.getCatalogo('tbl_MotivoRechazo_Art').then(function (results) {
        $scope.motivoRechazo = results.data;
        $scope.motivoRechazoArt = results.data;
    });

    debugger;
    //Si no es consulta de una solicitud no puede acceder a la pantalla
    if (localStorageService.get('IdSolicitud') === undefined) {
        $scope.MsjConfirmacion = 'Consulte una Solicitud para acceder a esta Pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeConfirmacion').modal('show');
    }
    if (localStorageService.get('IdSolicitud') === null) {
        $scope.MsjConfirmacion = 'Consulte una Solicitud para acceder a esta Pantalla.';
        $scope.accion = 9;
        $scope.TextoBoton = "Aceptar";
        $scope.ViewBoton = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.txtIdSolicitud = (localStorageService.get('IdSolicitud') === null ? "0" : localStorageService.get('IdSolicitud'));
    debugger;
    $scope.tipouser = (localStorageService.get('tipouser') === null ? "0" : localStorageService.get('tipouser'));
    //Limpia
    localStorageService.remove('IdSolicitud');
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
            $scope.myPromise = AdmArticuloService.getArticulos("2", $scope.txtIdSolicitud, "", "", "", "", "", "", "", "", "", "", "", "", "").then(function (results) {

                debugger;
                //Linea de Negocio
                var index;
                var linea = {};
                linea = results.data.root[0];
                var ln = $scope.lineaNegocio;
                for (index = 0; index < ln.length; ++index) {
                    if (ln[index].codigo === linea[0].lineaNegocio)
                        $scope.ddlLineaNegocio = ln[index];
                }

                $scope.Det = results.data.root[1];
                $scope.grArticulo = $scope.Det;
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

                $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
            }, function (error) {
            });
        }
    }

    //Carga la unidad de medida seleccionada en el combo para la seccion de codigo de barras
    $scope.$watch('ddlUnidadMedida', function () {
        $scope.p_CBr.unidadMedida = $scope.ddlUnidadMedida.codigo;
    });

    $scope.SeleccionArt = function (Id) {
        $scope.Limpia();
        $scope.p_Det.codReferencia = Id;
        //Carga datos y grid filtrados
        for (var i = 0, len = $scope.Det.length; i < len; i++) {
            if ($scope.Det[i].codReferencia === Id) {
                debugger;
                var index;
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
                var ro = $scope.regionOrigen;
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
                var mo = $scope.modelo;
                for (index = 0; index < mo.length; ++index) {
                    if (mo[index].codigo === $scope.Det[i].modelo)
                        $scope.ddlModelo = mo[index];
                }
                //iva
                var iv = $scope.clasificacionFiscal;
                for (index = 0; index < iv.length; ++index) {
                    if (iv[index].codigo === $scope.Det[i].iva)
                        $scope.ddlIva = iv[index];
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
                $scope.p_Det.idDetalle = $scope.Det[i].idDetalle;
                $scope.p_Det.codReferencia = Id;
                $scope.p_Det.marcaNueva = $scope.Det[i].marcaNueva;
                $scope.p_Det.descripcion = $scope.Det[i].descripcion;
                $scope.p_Det.tamArticulo = $scope.Det[i].tamArticulo;
                $scope.p_Det.otroId = $scope.Det[i].otroId;
                $scope.p_Det.contAlcohol = $scope.Det[i].contAlcohol;
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
                //coleccion
                var cl = $scope.coleccion;
                for (index = 0; index < cl.length; ++index) {
                    if (cl[index].codigo === $scope.Com[i].coleccion)
                        $scope.ddlColeccion = cl[index];
                }
                //temporada
                var te = $scope.temporada;
                for (index = 0; index < te.length; ++index) {
                    if (te[index].codigo === $scope.Com[i].temporada)
                        $scope.ddlTemporada = te[index];
                }
                //estacion
                var et = $scope.estacion;
                for (index = 0; index < et.length; ++index) {
                    if (et[index].codigo === $scope.Com[i].estacion)
                        $scope.ddlEstacion = et[index];
                }

                $scope.p_Com.idDetalle = $scope.Com[i].idDetalle;
                $scope.p_Com.codSap = $scope.Com[i].codSap;
                $scope.p_Com.codLegacy = $scope.Com[i].codLegacy;
                $scope.p_Com.costoFOB = $scope.Com[i].costoFOB;
                $scope.p_Com.validoDesde = new Date($scope.Com[i].validoDesde);
                $scope.p_Com.observaciones = $scope.Com[i].observaciones;
                //Nuevos
                $scope.p_Com.jerarquiaProd = $scope.Com[i].jerarquiaProd;
                $scope.p_Com.susceptBonifEsp = $scope.Com[i].susceptBonifEsp;
                $scope.p_Com.procedimCatalog = $scope.Com[i].procedimCatalog;
                $scope.p_Com.caracterPlanNec = $scope.Com[i].caracterPlanNec;
                $scope.p_Com.fuenteProvision = $scope.Com[i].fuenteProvision;
            }
        }

        //Carga la ruta para nuevas imagenes
        var direc = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path //obtiene la ruta segun el articulo
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
        $scope.uploader2.url = Ruta;

        $scope.grArtMedida = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtImagen = $filter('filter')($scope.Ima, { idDetalle: $scope.p_Det.idDetalle });
        //Adminsitrativo
        $scope.grArtCatalogacion = $filter('filter')($scope.Cat, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtAlmacen = $filter('filter')($scope.Alm, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmEnt = $filter('filter')($scope.Iae, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndTipoAlmSal = $filter('filter')($scope.Ias, { idDetalle: $scope.p_Det.idDetalle });
        $scope.grArtIndAreaAlmacen = $filter('filter')($scope.Iaa, { idDetalle: $scope.p_Det.idDetalle });

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

                $scope.temp_med = $filter('filter')($scope.Med, { idDetalle: $scope.p_Det.idDetalle });
                $scope.p_Med.idDetalle = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].idDetalle;
                $scope.p_Med.codReferencia = $filter('filter')($scope.temp_med, { unidadMedida: Id })[0].codReferencia;

                $scope.p_Med.factorCon = $scope.grArtMedida[i].factorCon;
                $scope.p_Med.pesoNeto = $scope.grArtMedida[i].pesoNeto;
                $scope.p_Med.pesoBruto = $scope.grArtMedida[i].pesoBruto;
                $scope.p_Med.longitud = $scope.grArtMedida[i].longitud;
                $scope.p_Med.ancho = $scope.grArtMedida[i].ancho;
                $scope.p_Med.altura = $scope.grArtMedida[i].altura;
                $scope.p_Med.precioBruto = $scope.grArtMedida[i].precioBruto;
                $scope.p_Med.descuento1 = $scope.grArtMedida[i].descuento1;
                $scope.p_Med.descuento2 = $scope.grArtMedida[i].descuento2;
                $scope.p_Med.impVerde = $scope.grArtMedida[i].impVerde;
            }
        }
        //Carga grid codigo barra
        debugger;
        $scope.temp_cbr = $filter('filter')($scope.CBr, { idDetalle: $scope.p_Med.idDetalle });
        $scope.grArtCodBarra = $filter('filter')($scope.temp_cbr, { unidadMedida: Id });
    }

    $scope.SeleccionCbr = function (Id) {
        //Carga los datos desde el grid
        debugger;
        for (var i = 0, len = $scope.grArtCodBarra.length; i < len; i++) {
            if ($scope.grArtCodBarra[i].numeroEan === Id) {
                $scope.p_CBr.numeroEan = $scope.grArtCodBarra[i].numeroEan;
                $scope.p_CBr.tipoEan = $scope.grArtCodBarra[i].tipoEan;
                $scope.p_CBr.descripcionEan = $scope.grArtCodBarra[i].descripcionEan;
                $scope.p_CBr.principal = $scope.grArtCodBarra[i].principal;
            }
        }
    }

    $scope.SeleccionIma = function (Id) {
        debugger;
        //Obtiene la ruta segun el articulo
        var pathima = $filter('filter')($scope.Rutas, { idDetalle: $scope.p_Det.idDetalle })[0].path;
        //Validar cuando sea consulta o cuando sea una nueva solicitud
        AdmArticuloService.getBajaTempArchivo(pathima, Id).then(function (results) {
            $scope.imagenurl = results.data;
            $('#idMuestraImagen').modal('show');
        }, function (error) {
        });
    }

    //Asistente de Compras
    $scope.ObservGrabar = function () {
        $scope.MsjConfirmacion = "¿Está seguro que desea Grabar la Solicitud?";
        $scope.accion = 1;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservRevisado = function () {
        $scope.MsjConfirmacion = "Ingrese una Observación";
        $scope.accion = 2;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservDevolver = function () {
        $scope.MsjConfirmacion = "Ingrese una Observación";
        $scope.accion = 3;
        $scope.ViewMotRechazo = false;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Gerente de Compras
    $scope.ObservAprobar = function () {
        $scope.MsjConfirmacion = "¿Está seguro que desea Aprobar la Solicitud?";
        $scope.accion = 4;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservDevolverGC = function () {
        $scope.MsjConfirmacion = "Ingrese una Observación";
        $scope.accion = 5;
        $scope.ViewMotRechazo = false;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Datos Maestros
    $scope.ObservGrabarDM = function () {
        $scope.MsjConfirmacion = "¿Está seguro que desea Grabar la Solicitud?";
        $scope.accion = 6;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservAprobarDM = function () {
        $scope.MsjConfirmacion = "¿Está seguro que desea Aprobar la Solicitud?";
        $scope.accion = 7;
        $scope.ViewMotRechazo = true;
        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.ObservDevolverDM = function () {
        $scope.MsjConfirmacion = "Ingrese una Observación";
        $scope.accion = 8;
        $scope.ViewMotRechazo = false;
        $('#idMensajeConfirmacion').modal('show');
    }

    //Evento click boton Grabar
    $scope.Grabar = function () {
        debugger;
        //Valida tipo de accion
        if ($scope.accion === 9) {
            window.location = '/Articulos/frmConsSolArticulo';
        } else {

            //Valida si selecciono la Linea de Negocio
            if ($scope.ddlLineaNegocio === "") {
                $scope.MenjError = "Debe seleccionar una Línea de Negocio.";
                $('#idMensajeError').modal('show');
                return;
            }
            if ($scope.ddlLineaNegocio === null) {
                $scope.MenjError = "Debe seleccionar una Línea de Negocio.";
                $('#idMensajeError').modal('show');
                return;
            }

            //Valida que por lo menos haya un articulo ingresado
            if ($scope.grArticulo.length === 0) {
                $scope.MenjError = "Debe ingresar por lo menos un Artículo.";
                $('#idMensajeError').modal('show');
                return;
            }

            //Arma la cabecera
            $scope.Cab = [];
            $scope.p_Cab = {};
            $scope.p_Cab.codproveedor = authService.authentication.CodSAP;
            $scope.p_Cab.Usuario = authService.authentication.userName;
            $scope.p_Cab.tiposolicitud = "1"; //Nuevo Articulo
            $scope.p_Cab.lineanegocio = $scope.ddlLineaNegocio.codigo;
            $scope.p_Cab.idsolicitud = $scope.txtIdSolicitud;
            debugger;
            $scope.p_Cab.accion = "U";
            $scope.p_Cab.estado = ($scope.accion === 1 ? "EC" : ($scope.accion === 2 ? "RC" : ($scope.accion === 3 ? "DC" : ($scope.accion === 4 ? "AG" : ($scope.accion === 5 ? "DG" : ($scope.accion === 6 ? "ED" : ($scope.accion === 7 ? "AD" : "DD")))))));
            $scope.p_Cab.observacion = $scope.observEstado;
            $scope.p_Cab.motivoRechazo = $scope.ddlMotivoRechazo.codigo;
            $scope.Cab.push($scope.p_Cab);

            //Lama el metodo
            $scope.myPromise = AdmArticuloService.getGrabaSolicitud($scope.Cab, $scope.Det, $scope.Med, $scope.CBr, $scope.Ima, $scope.Com, $scope.Cat, $scope.Alm, $scope.Iae, $scope.Ias, $scope.Iaa).then(function (response) {
                $scope.MsjConfirmacion = 'Se ha grabado exitosamente la Solicitud';
                $scope.accion = 9;
                $scope.TextoBoton = "Aceptar";
                $scope.ViewBoton = true;
                $('#idMensajeConfirmacion').modal('show');
            },
             function (err) {
                 $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                 $('#idMensajeError').modal('show');
             });
        }
    }

    //A nivel de articulo
    $scope.AprobarArticulo = function () {
        $scope.MsjConfirmacionArt = "¿Está seguro que desea Aprobar el Artículo?";
        $scope.accionArt = 1;
        $scope.ViewMotRechazoArt = true;
        $('#idMensajeConfirmacionArt').modal('show');
    }

    $scope.RechazarArticulo = function () {
        $scope.MsjConfirmacion = "¿Está seguro que desea Rechazar el Artículo?";
        $scope.accionArt = 2;
        $scope.ViewMotRechazoArt = false;
        $('#idMensajeConfirmacionArt').modal('show');
    }

    $scope.AddCatalogacion = function () {
        debugger;
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un Artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlCatalogacion === "") {
            $scope.MenjError = "Debe seleccionar una Catalogación.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlCatalogacion === undefined) {
            $scope.MenjError = "Debe seleccionar una Catalogación.";
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
        debugger;
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un Artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlAlmacen === "") {
            $scope.MenjError = "Debe seleccionar un Almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlAlmacen === undefined) {
            $scope.MenjError = "Debe seleccionar un Almacén.";
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
        debugger;
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un Artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmEnt === "") {
            $scope.MenjError = "Debe seleccionar un Indicador Tipo Almacén Entrad.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmEnt === undefined) {
            $scope.MenjError = "Debe seleccionar un Indicador Tipo Almacén Entrad.";
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
        debugger;
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un Artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndTipoAlmSal === "") {
            $scope.MenjError = "Debe seleccionar un Indicador Tipo Almacén Salida.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndTipoAlmSal === undefined) {
            $scope.MenjError = "Debe seleccionar un Indicador Tipo Almacén Salida.";
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
        debugger;
        //Valida que haya seleccionado un articulo
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Debe seleccionar un Artículo.";
            $('#idMensajeError').modal('show');
            return;
        }

        //Valida que haya seleccionado 
        if ($scope.ddlIndAreaAlmacen === "") {
            $scope.MenjError = "Debe seleccionar una Indicador Área Almacén.";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.ddlIndAreaAlmacen === undefined) {
            $scope.MenjError = "Debe seleccionar una Indicador Área Almacén.";
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
        debugger;

        //Carga los datos de Compras
        $scope.p_Com.idDetalle = $scope.p_Det.idDetalle;
        //Modifica los datos de compras
        for (var i = 0, len = $scope.Com.length; i < len; i++) {
            var update = $scope.Com[i];
            if (update.idDetalle === $scope.p_Com.idDetalle) {

                update.costoFOB = $scope.p_Com.costoFOB;
                update.observaciones = $scope.p_Com.observaciones;
                //Nuevos
                update.jerarquiaProd = $scope.p_Com.jerarquiaProd;
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
                update.accion = "U";
                break;
            }
        }

        //Modifica los datos del detalle
        //Colocar los datos de los combos OJO
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
                update.coleccion = $scope.p_Det.coleccion;
                update.temporada = $scope.p_Det.temporada;
                update.estacion = $scope.p_Det.estacion;
                update.iva = $scope.p_Det.iva;
                update.deducible = $scope.p_Det.deducible;
                update.retencion = $scope.p_Det.retencion;
                update.descripcion = $scope.p_Det.descripcion;
                update.otroId = $scope.p_Det.otroId;
                update.contAlcohol = $scope.p_Det.contAlcohol;
                update.accion = "U";
                break;
            }
        }

        //Agrega el articulo y carga el grid
        $scope.grArticulo = $scope.Det;
        $scope.Limpia();
        $('.nav-tabs a[href="#ListaArticulos"]').tab('show');
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
        $scope.ddlColeccion = "";
        $scope.ddlTemporada = "";
        $scope.ddlEstacion = "";
        //Catalogacion
        $scope.p_Cat = {};
        $scope.p_Cat.idDetalle = "";
        $scope.p_Cat.catalogacion = "";
        $scope.p_Cat.desCatalogacion = "";
        $scope.p_Cat.accion = "";
        //Almacen
        $scope.p_Alm = {};
        $scope.p_Alm.idDetalle = "";
        $scope.p_Alm.almacen = "";
        $scope.p_Alm.desAlmacen = "";
        $scope.p_Alm.accion = "";
        //IndTipoAlmEnt
        $scope.p_Iae = {};
        $scope.p_Iae.idDetalle = "";
        $scope.p_Iae.indTipoAlmEnt = "";
        $scope.p_Iae.desIndTipoAlmEnt = "";
        $scope.p_Iae.accion = "";
        //IndTipoAlmSal
        $scope.p_Ias = {};
        $scope.p_Ias.idDetalle = "";
        $scope.p_Ias.indTipoAlmSal = "";
        $scope.p_Ias.desIndTipoAlmSal = "";
        $scope.p_Ias.accion = "";
        //IndAreaAlmacen
        $scope.p_Iaa = {};
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
        $scope.grArtCatalogacion = [];
        $scope.grArtAlmacen = [];
        $scope.grArtIndTipoAlmEnt = [];
        $scope.grArtIndTipoAlmSal = [];
        $scope.grArtIndAreaAlmacen = [];
    }

    $scope.NextMedida = function () {
        if ($scope.p_Det.idDetalle === "") {
            $scope.MenjError = "Genere un Nuevo Artículo";
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
            $scope.MenjError = "Debe ingresar por lo menos una Medida.";
            $('#idMensajeError').modal('show');
            return;
        }
    }

    $scope.NextCompras = function () {
        $('.nav-tabs a[href="#Compras"]').tab('show');
    }

    //INICIO CARGA DE ARCHIVO 
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var Ruta = serviceBase + 'api/FileArticulo/UploadFile/?path=';
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            //debugger;
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
            //debugger;
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
        debugger;
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
    //FIN CARGA DE ARCHIVOS

    $scope.CargaConsulta();
}
]);