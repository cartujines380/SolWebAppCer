
app.controller('NotificacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', 'FileUploader', '$sce', 'authService', '$http', '$filter', function ($scope, ngAuthSettings, NotificacionService, FileUploader, $sce, authService, $http, $filter) {
    $scope.Notificacion = [];
    $scope.Notificacion.ListaAdjuntos = [];
    $scope.Notificacion.Lista = [];
    $scope.ListaDowload = [];
    $scope.GridNotificacion = [];
    $scope.GridlistaProveedores = [];
    $scope.pagesNot = [];
    $scope.allNotificaciones = [];
    $scope.pagesLisProve = [];
    $scope.pageContentNot = [];
    $scope.Notificacion.ListaLineasSelec = [];
    $scope.Notificacion.ListaDepartamentos = [];
    $scope.pageContentLisProve = [];
    $scope.filtroTituloNot = "";
    $scope.filtroCategoriaNot = "";
    $scope.filtroPrioridadNot = "";
    $scope.filtroEstadoNot = "";
    $scope.filtroObligatorioNot = "";
    $scope.txtBusTitulo = "";
    $scope.PorDatosBusq = "1";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.filtroBusquedad = false;
    $scope.filtroObligatorio = false; 
    $scope.secconsultaProveedor = true;
    $scope.strict = false; 
    $scope.iscorreo = false;
    $scope.verFechaVencimiento = true;
    $scope.isnew = true; 
    $scope.isdowload = true; 
    $scope.isIngresada = true; 
    $scope.isLectura = false;
    $scope.isAgrEnv = true;
    $scope.listaAdjplus = false;
    $scope.verObservacion = false; 
    $scope.provisObligatorio = false;
    $scope.verButtonEliminar = false;    
    $scope.provisPendiente = true; 
    $scope.isOCultaOpciones = true;
    $scope.envioIndividual = false;
    $scope.notEnviada = false;
    $scope.ListaProveedores = [];
    $scope.ListaProveedores2 = [];
    //catalogos
    $scope.TipoCategoria = [];
    $scope.Notificacion.FechaVencimiento = new Date();
    $scope.rutaDirectorio = "";
    $scope.codigoNot = 0;
    $scope.rutaArchivos = "";
    $scope.isAprobada = false;
    $scope.agrupacionNotificacion = [];
    $scope.ddlenviaLinea = [];
    $scope.ddlenviaLinea.codigo = "";
    $scope.ddlenviaLinea.descripcion = "";
    $scope.ddlenviaLinea.chekeado = false;
    $scope.lineaNegocio = [];
    //$scope.archivourl = //localhost:26264/UploadedDocuments;
    //$scope.archivodir = "Not_150";
    //$scope.archivonom = "comunicado.pdf";
    //$scope.archivotipo = "pdf";
    $scope.archivopdf = $sce.trustAsResourceUrl("");
    //$scope.archivopdf = "//localhost:26264/UploadedDocuments/Not_150/comunicado.pdf";
    $scope.linNegocio = false;
    $scope.userSesion = authService.authentication.userName;
    $scope.ejecutafirst = true;
    $scope.enviaCorreo = false;
    $scope.notifGeneral = false;
    $scope.ruc = authService.authentication.ruc; 
    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    $scope.estadoObser = "";
    $scope.accion = "";
    $scope.estadoTrx = "";
    $scope.txtenviarA = "Todos los proveedores";
    $scope.txtTipoEnvio = "T";
    $scope.FuncionNot = [];
    $scope.DepFuncNot = [];
    $scope.DepartamentoNot = [];
    $scope.NotDepartamaneto = [];
    $scope.SettingDepartamentoNot = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };
    $scope.RolesNot = [];
    $scope.NotRoles = [];
    $scope.SettingRolesNot = { displayProp: 'pDescripcionRol', idProp: 'pIdRol', enableSearch: true, scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };


    $scope.Zonas = [];
    $scope.ZonasDS = [];
    $scope.SettingGrupoArt = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true,  scrollableHeight: '200px', scrollable: true, buttonClasses: 'btn btn-default btn-multiselect-chk' };

    $scope.DepFunIng = {
        IdDepartamento: '',
        DesDepartamento: '',
        ListaFunciones: [],
        expanded: false,
    }

    $scope.FuncIngreso = {
        IdDepartamento: '',
        IdFuncion: '',
        DesFuncion: '',
        isCheck: false,
    }


    $scope.eTipoNotif = 'P';
    $scope.nomUltFileUpload1 = "";
    var isVisualizacion = false;
    
    NotificacionService.getCatalogo('tbl_Ciudad').then(function (results) {
        $scope.ZonasDS = results.data;
    });

    NotificacionService.getCatalogo('tbl_DepartaContacto').then(function (results) {

        $scope.DepartamentoNot = results.data;
        NotificacionService.getCatalogo('tbl_FuncionContacto').then(function (results) {
            $scope.FuncionNot = results.data;
           
            

        }, function (error) {

        });

    }, function (error) {

    });

   

   


   

    $scope.myPromise = NotificacionService.getConsTodosRoles('').then(function (results) {
       
        $scope.RolesNot = results.data.root[0];
        if (results.data.root[0] != null) {
            for (var i = 0; i < $scope.RolesNot.length; i++) {
                $scope.RolesNot[i].pElegir = false;
            }
        }
    }, function (error) {
    });




    //Carga Catalogo Categorias Notificaciones
    NotificacionService.getCatalogo('tbl_Categorias').then(function (results) {
        $scope.TipoCategoria = results.data;
    }, function (error) {
    });

    //Carga Catalogo Prioridades Notificaciones
    NotificacionService.getCatalogo('tbl_Prioridad').then(function (results) {
        $scope.TipoPrioridad = results.data;
    }, function (error) {
    });

    //Carga Catalogo Estados Notificaciones
    NotificacionService.getCatalogo('tbl_EstadoNot').then(function (results) {
        $scope.EstadoNotif = results.data;
    }, function (error) {
    });
    //Carga Catalogo Tipo Notificaciones
    NotificacionService.getCatalogo('tbl_TipoNot').then(function (results) {
        $scope.TipoNotif = $filter('filter')(results.data, { codigo: "I" });;
    }, function (error) {
    });

    $scope.myPromise =
        NotificacionService.getCatalogo('tbl_LineaNegocio').then(function (results) {
            $scope.lineaNegocio = results.data;
            $scope.ddlenviaLinea = [];
            for (var j = 0 ; j < $scope.lineaNegocio.length; j++)
            {
                $scope.ddlenviaLinea.push(
                   { codigo: $scope.lineaNegocio[j].codigo, descripcion: $scope.lineaNegocio[j].detalle,  chekeado: false });
            }
        });
    ;

    $scope.toggleCategory = function (content) {

        content.expanded = !content.expanded;
    }
    $scope.ActConsultaNotificaciones = function (estado) {

        $scope.myPromise =
        NotificacionService.getConsultaNotificaciones(estado).then(function (results) {

            if (results.data.success) {
                
                $scope.allNotificaciones = results.data.root[0];
                $scope.EjecutaFiltro();
            }

        }, function (error) {
        });
        ;

    };

    //Carga lista de notificaciones registradas
    $scope.ConsultaNotificaciones = function (estado, isAgrupacion) {
       

        NotificacionService.getConsultaNotificaciones(estado).then(function (results) {
            if (results.data.success) {
               
                $scope.allNotificaciones = results.data.root[0];
             
                if (isAgrupacion != undefined)
                {
                    $scope.allNotificaciones = $filter('filter')($scope.allNotificaciones, { tipoCorreo: "N" });
                }
                
                if ($scope.ejecutafirst && estado == "T") {
                    $scope.busTodos = false;
                 
                    $scope.selectedEstadosb = ['Aprobado', 'Ingresado', 'Devuelto'];
                   
                   
                    setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);

                    $scope.GridNotificacion = $filter('filter')($scope.allNotificaciones, function (obj) {
                        return obj.estado == 'Aprobado' || obj.estado == 'Ingresado' || obj.estado == 'Devuelto';
                    });               

                    setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);

                    $scope.etiTotRegistros = $scope.GridNotificacion.length;

                }
                else
                {
                    //$scope.GridNotificacion = [];
                    //$scope.allNotificaciones = [];
                    //$scope.etiTotRegistros = $scope.GridNotificacion.length;
                    //$scope.allNotificaciones = results.data.root[0];
                    $scope.EjecutaFiltro('N');
                   
                }

            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }, function (error) {
        });
        
    }; 


    $scope.actualizaDepFunciones = function (estado) {
        //Carga de Departamentos y Funciones
        $scope.DepFuncNot = [];
        for (var index = 0; index < $scope.DepartamentoNot.length; index++) {

            $scope.DepFunIng = {}
            $scope.DepFunIng.expanded = false;
            $scope.DepFunIng.IdDepartamento = $scope.DepartamentoNot[index].codigo;
            $scope.DepFunIng.DesDepartamento = $scope.DepartamentoNot[index].detalle;
            var auxListaFuncion = $filter('filter')($scope.FuncionNot, { descAlterno: $scope.DepartamentoNot[index].codigo }, true);
            $scope.DepFunIng.ListaFunciones = [];
            for (var idx = 0; idx < auxListaFuncion.length; idx++) {
                $scope.FuncIngreso = {};
                $scope.FuncIngreso.IdDepartamento = $scope.DepartamentoNot[index].codigo;
                $scope.FuncIngreso.IdFuncion = auxListaFuncion[idx].codigo;
                $scope.FuncIngreso.DesFuncion = auxListaFuncion[idx].detalle;
                $scope.FuncIngreso.isCheck = false;

                $scope.DepFunIng.ListaFunciones.push($scope.FuncIngreso);
            }



            $scope.DepFuncNot.push($scope.DepFunIng);
        }
    }

    $scope.mostrarObseracion = function (estado) {

        $scope.estadoObser = estado;
        if (estado == 'R') {
            $scope.MenjConfirmacion = "¿Está seguro de rechazar la notificación?";

        }
        if (estado == 'D') {
            $scope.MenjConfirmacion = "¿Está seguro de devolver la notificación?";

        }
        $scope.accion = 9;

        $('#idMensajeConfirmacion').modal('show');       


        
    }

    $scope.cerrarPdf = function (estado) {

        document.getElementById("pdfvisualiza").setAttribute("src", "");
        $('#visualizarpdf').modal('hide');



    }

    $scope.mostrarObseracionTrx = function (estado) {        
        $scope.estadoObser = estado;
        $('#idObservavionMensaje').modal('show');      
    }

    $scope.isLeidoProv = function () {
    
        if ($scope.Notificacion.Obligatorio) {
            if ($scope.provleido)
                $scope.provisObligatorio = false;
            else
                $scope.provisObligatorio = true;
        }

    }

    $scope.leerNotProv = function () {
   
        if ($scope.provleido) {

            NotificacionService.getActualizaEstadoNot($scope.codigoNot, $scope.ruc).then(function (results) {
                $scope.ConsultaListaNotificacionesProv();
                
                $scope.provleido = false;
                $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');

            }, function (error) {
            });
        }

    }

    //Carga lista de notificaciones que tiene proveedor
    $scope.ConsultaListaNotificacionesProv = function () {
       
        var ruc = $scope.ruc;
        //ruc = '0100163112001';
        NotificacionService.getConsultaListaNotificacionesProv(ruc).then(function (results) {
            
            if (results.data.success) {
                $scope.GridNotificacion = results.data.root[0];
                if ($scope.ejecutafirst ) {
                    $scope.busTodos = false;
                   
                    $scope.selectedEstadosb = ['Enviada para Aprobación'];
                    $scope.ejecutafirst = false;
                    $scope.filterByEstados = function (content) {
                        return ($scope.selectedEstadosb.indexOf(content.estado) !== -1);
                    };
                }
                else
                    $scope.EjecutaFiltro('N');
            }
        }, function (error) {
        });
    };

    // Limpiar filtros de busqueda
    $scope.confirmaEliminar = function () {
        $scope.MenjConfirmacion = "¿Está seguro de eliminar la notificación?"
        $('#idMensajeConfirmacionEliminar').modal('show');      

    };

    $scope.Eliminar = function () {
        
        $scope.enviaAprobacionTrx('X');
    };

    // Confirmacion de eliminicacion de registros
    $scope.limpiaCamposBusqueda = function () {
        if ($scope.PorDatosBusq != 1) $scope.txtBusTitulo = "";
        if ($scope.PorDatosBusq != 2) $scope.selectedItemPrioridad = "";
        if ($scope.PorDatosBusq != 3) $scope.selectedItemEstadoNot = "";
    };

    // FIltros Angular de Notificaciones
    $scope.EjecutaFiltro = function (validar) {
        
        //$scope.limpiaCamposBusqueda();
     
        if (!$scope.busTodos && $scope.txtBusTitulo == "" && $scope.selectedItemPrioridad == null && $scope.selectedItemEstadoNot == null && !$scope.busObligatorio) {
            if (validar != 'N') {
                $scope.MenjError = "Indique al menos un criterio de búsqueda.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            $scope.filtroTituloNot = "";
            $scope.filtroCategoriaNot = "";
            $scope.filtroPrioridadNot = "";
            $scope.filtroEstadoNot = "";
            $scope.filtroObligatorioNot = "";
            $scope.strict = false;
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);        
            $scope.GridNotificacion = $filter('filter')($scope.allNotificaciones, { titulo: $scope.filtroTituloNot, prioridad: $scope.filtroPrioridadNot, estado: $scope.filtroEstadoNot, obligatorio: $scope.filtroObligatorioNot });
                  
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
            $scope.etiTotRegistros = $scope.GridNotificacion.length;
            return;

        }
        $scope.selectedEstadosb = ['Aprobado', 'Publicado', 'Ingresado', 'Enviada para Aprobación', 'Rechazado', 'Devuelto', 'Leido'];
       
        if ($scope.busTodos)
        {
            $scope.filtroTituloNot = "";
            $scope.filtroCategoriaNot = "";
            $scope.filtroPrioridadNot = "";
            $scope.filtroEstadoNot = "";
            $scope.filtroObligatorioNot = "";
            $scope.strict = false;
            
            
        }
        else
        {
            $scope.filtroTituloNot = $scope.txtBusTitulo;
            if ($scope.selectedItemTipoCategoria != null)
                $scope.filtroCategoriaNot = $scope.selectedItemTipoCategoria.detalle;
            else
                $scope.filtroCategoriaNot = "";
            if ($scope.filtroCategoriaNot == "") $scope.strict = false;
            else $scope.strict = true;
            if ($scope.selectedItemPrioridad != null)
                $scope.filtroPrioridadNot = $scope.selectedItemPrioridad.detalle;
            else
                $scope.filtroPrioridadNot = "";
            if ($scope.selectedItemEstadoNot != null)
                $scope.filtroEstadoNot = $scope.selectedItemEstadoNot.detalle;
            else
                if ($scope.ejecutafirst) $scope.ejecutafirst = false;
                else  $scope.filtroEstadoNot = ""; 
                
            if ($scope.busObligatorio)
            {
                if ($scope.txtBusObligatorio) $scope.filtroObligatorioNot = "Si";
                else $scope.filtroObligatorioNot = "No";
            }
            else
            {
                $scope.filtroObligatorioNot = "";
            }


            
        }
        setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
        $scope.GridNotificacion = $filter('filter')($scope.allNotificaciones, { titulo: $scope.filtroTituloNot, prioridad: $scope.filtroPrioridadNot, estado: $scope.filtroEstadoNot, obligatorio: $scope.filtroObligatorioNot });
        
        setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        $scope.etiTotRegistros = $scope.GridNotificacion.length;
        if ($scope.etiTotRegistros == 0 && validar != "N")
        {
            $scope.MenjError = "No existe resultado para su consulta.";
            $('#idMensajeInformativo').modal('show');

        }
    };

    //prueba de ventanas de notificacin
    $scope.Prueba = function () {
        $scope.MenjError = "Indicar Fecha de vencimiento de Notificación.";
        $('#idMensajeInformativo').modal('show');

    };
    //Ocultar o no Fecha de Vencimiento
    $scope.quitarAdjunto = function (nomArchivo) {
        
        
        
        var listaArchivos = $scope.uploader2.queue;
        for (var i = 0 ; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader2.queue[i]._file.name;
            if (nomArchivo == nomArchivo2)
            {
                $scope.uploader2.queue[i].remove();
            }            
        }

        for (var i = 0 ; i < $scope.Notificacion.ListaAdjuntos.length; i++)
        {
            if ($scope.Notificacion.ListaAdjuntos[i] == nomArchivo) break;

        }
        $scope.Notificacion.ListaAdjuntos.splice(i, 1);
        for (var i = 0 ; i < $scope.Notificacion.Lista.length; i++) {
            if ($scope.Notificacion.Lista[i].name == nomArchivo) break;

        }
        $scope.Notificacion.Lista.splice(i, 1);
        if ($scope.Notificacion.Lista.length > 1) $scope.listaAdjplus = true;
        else $scope.listaAdjplus = false;

        

    };
    //Envio de correo
    $scope.enviodeCorreo = function () {
        //if ($scope.etipoCorreo)
        //{
        //    $scope.enviaCorreo = true;
        //}
        //else
        //    $scope.enviaCorreo = false;
        //$scope.LimpiaNotificacion();
        if ($scope.eTipoNotif == 'P')
        {
            $scope.enviaCorreo = false;
            $scope.notifGeneral = false;
        }
        else if ($scope.eTipoNotif == 'C') {
            $scope.enviaCorreo = true;
            $scope.notifGeneral = false;
        }
        else if ($scope.eTipoNotif == 'G') {
            $scope.enviaCorreo = false;
            $scope.notifGeneral = true;
        }

        $scope.LimpiaNotificacion();

        if ($scope.eTipoNotif == 'G') {
            //$scope.Notificacion.Titulo = '[NOTIFICACION GENERAL]';
            if($scope.Notificacion.ListaAdjuntos.length > 0)
	        {
	          var nomGraNot =  $scope.Notificacion.ListaAdjuntos[0].split(".");;
              $scope.Notificacion.Titulo = nomGraNot[0];
            }
            $scope.Notificacion.Prioridad = $scope.TipoPrioridad[0];
            $scope.Notificacion.FechaPublicacion = new Date();
            $scope.Notificacion.FechaVencimiento = new Date();
            $scope.Notificacion.FechaVencimiento.setDate($scope.Notificacion.FechaVencimiento.getDate() + 1);
        }
    };

    //Ocultar o no Fecha de Vencimiento
    $scope.valFechaVencimiento = function () {
        if ($scope.valFecVenc)
        {
            $scope.verFechaVencimiento = false;
            $scope.Notificacion.FechaVencimiento = "";
        }
            
        else
        {
            $scope.verFechaVencimiento = true;
            //$scope.Notificacion.FechaVencimiento = "No vence"
            $scope.Notificacion.FechaVencimiento = new Date();
        }
            
    };
    //Ocultar o no parametros de busqueda para realizar filtros
    $scope.verFiltro = function () {
        if ($scope.busTodos) $scope.filtroBusquedad = true;
        else $scope.filtroBusquedad = false;
    };
    //Oculatar o no parametro de busqueda obligatorio
    $scope.verObligatorio = function () {
        if ($scope.busObligatorio) $scope.filtroObligatorio = true;
        else $scope.filtroObligatorio = false;
    };
    //Visualizar ventana modal de comunicado
    $scope.visualizarComunicado = function (nomArchivo) {
        if ($scope.notifGeneral) {
            var ArrFilePDF = [nomArchivo, $scope.rutaDirectorio];
            $scope.myPromise =
                NotificacionService.getDescargarArchivos(ArrFilePDF).then(function (results) {
                    $scope.rutaArchivos = results;
                    $scope.visualizarComunicado_2daParte(nomArchivo);
                }, function (error) {
                });
        }
        else {
            $scope.visualizarComunicado_2daParte(nomArchivo);
        }
    }
    $scope.visualizarComunicado_2daParte = function (nomArchivo) {
        //var urlpdf = $scope.rutaArchivos + nomArchivo;
        var urlpdf = $scope.rutaArchivos + "UploadedDocuments/PDF/" + $scope.rutaDirectorio + "/" + nomArchivo;  //jc
        document.getElementById("pdfvisualiza").setAttribute("src", urlpdf);
        $scope.archivopdf = $sce.trustAsResourceUrl(urlpdf);
        if ($scope.Notificacion.Obligatorio) $scope.provisObligatorio = true;
        else $scope.provisObligatorio = false;

        $('#visualizarpdf').modal('show');
    }
    

    //Visualizar ventana modal para seleccionar proveedores a enviar notificacion
    $scope.buscarProveedor = function () {
        //Seleccionar tipo de envio
        if ($scope.txtTipoEnvio == "T")
        {
            $scope.envioIndividual = false;
            $scope.linNegocio = false;
            $scope.tipoEnviarProveedor = [];
        }
        else
        {
            for (var l = 0; l < $scope.TipoNotif.length; l++) {
                if ($scope.TipoNotif[l].codigo == $scope.txtTipoEnvio) {
                    $scope.tipoEnviarProveedor = $scope.TipoNotif[l];
                }
            }
            
        }
        $scope.tipoEnvioProveedor();

        $scope.pageContentLisProve = [];
        for (var l=0; l < $scope.ListaProveedores.length; l++)
        {
            var res = $scope.ListaProveedores[l];
            $scope.pageContentLisProve.push(
                    { codProveedor: res.codProveedor, representante: res.representante, razonSocial: res.razonSocial, isSelecProve: true });

        }
                //$scope.pageContentLisProve = $scope.ListaProveedores.slice();        
        $scope.ListaProveedores2 = $scope.ListaProveedores.slice();        
        $scope.txtBusRuc = "";
        $('#proveedoresLista').modal('show');
       
    }
    //Mostrar seccion dependiendo tipo de envio a proveedor
    $scope.tipoEnvioProveedor = function () {
        if ($scope.tipoEnviarProveedor == null) {
            $scope.linNegocio = false;
            $scope.secconsultaProveedor = true;
            return;
        }
        if ($scope.tipoEnviarProveedor.codigo == "L")
        {
            $scope.linNegocio = true;
            $scope.secconsultaProveedor = true;
        }
        else
        {
            $scope.linNegocio = false;
            if ($scope.tipoEnviarProveedor != null) {
                if ($scope.tipoEnviarProveedor.codigo == "I")
                    $scope.secconsultaProveedor = false;
                else $scope.secconsultaProveedor = true;
            }
            else
            {
                $scope.secconsultaProveedor = true;
            }
      }
        
    }
    
    //Consultar lista de proveedores
    $scope.ConsultarProveedores = function () {
        var txtEnvRUC = $scope.txtBusRuc;
        if (txtEnvRUC == "") return;
        if (txtEnvRUC.length < 13) {
            $scope.MenjError = "RUC incorrecto."
            $('#idMensajeError').modal('show');
            return;
        }
        if (!validacedula(txtEnvRUC)) {
            return;
        }

        //Servicio para consultar proveedor
        NotificacionService.getConsultaLisProveedores("1", txtEnvRUC).then(function (results) {
     
            if (results.data.success) {

                //$scope.pageContentLisProve = $scope.ListaProveedores.slice();
                if (results.data.root.length > 0) {
                    var res = results.data.root[0];
                    var existe = false;
                    //validar si registro consultado ya existe en la lista
                    for (var il = 0; il < $scope.pageContentLisProve.length; il++) {
                        if ($scope.pageContentLisProve[il].codProveedor == res[0].codProveedor) {
                            existe = true;
                            break;
                        }

                    }
                    if (!existe) {
                        $scope.pageContentLisProve.push(
                        { codProveedor: res[0].codProveedor, representante: res[0].representante, razonSocial: res[0].razonSocial, isSelecProve: false });
                    }

                }
                else {
                    $scope.MenjError = "No existe resultado para su consulta.";
                    $('#idMensajeInformativo').modal('show');

                    return;
                }


            }

        }, function (error) {
        });
    };


    function validacedula(txtIdentificacion) {
        var campos = txtIdentificacion;
        if (campos.length >= 10) {
            numero = campos;
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
            d1 = numero.substr(0, 1);
            d2 = numero.substr(1, 1);
            d3 = numero.substr(2, 1);
            d4 = numero.substr(3, 1);
            d5 = numero.substr(4, 1);
            d6 = numero.substr(5, 1);
            d7 = numero.substr(6, 1);
            d8 = numero.substr(7, 1);
            d9 = numero.substr(8, 1);
            d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                $scope.MenjError = "El tercer dígito ingresado es inválido"
                $('#idMensajeError').modal('show');
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
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

            suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            residuo = suma % modulo;

            /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
            digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                if (digitoVerificador != d9) {
                    $scope.MenjError = "El ruc de la empresa del sector público es incorrecto."
                    $('#idMensajeError').modal('show');
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    $scope.MenjError = "El ruc de la empresa del sector público debe terminar con 0001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    $scope.MenjError = "El ruc de la empresa del sector privado es incorrecto."
                    $('#idMensajeError').modal('show');
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la empresa del sector privado debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }

            else if (nat == true) {
                if (digitoVerificador != d10) {
                    $scope.MenjError = "El número de cédula de la persona natural es incorrecto."
                    $('#idMensajeError').modal('show');
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    $scope.MenjError = "El ruc de la persona natural debe terminar con 001"
                    $('#idMensajeError').modal('show');
                    return false;
                }
            }
            return true;
        }

    }

    //Seleciconar grid de Línea de negocio
    $scope.validarLinSeleccionado = function (codlinea) {
        alert(codlinea.codigo);

    }

    //Agregar proveedor a enviar notificacion
    $scope.selGridProveedor = function (codProveedor, desProveedor, ruc) {
        var a = $scope.ListaProveedores;
        var index;
        $scope.txtBusRuc = "";
  
        var selc = "#CodigoProveedor" + codProveedor;
        
            if (codProveedor != undefined) {
                for (index = 0; index < a.length; ++index) {
                    if (a[index].codProveedor == codProveedor) {
                        break;
                    }
                }
                if ($(selc).is(':checked')) {
                    $scope.ListaProveedores.push(
                    { codProveedor: codProveedor, representante: ruc, razonSocial: desProveedor, isSelecProve: true });
                }
                else {
                    $scope.ListaProveedores.splice(index,1);
                }
            }
        
        
        
        

    }
    //Agregar todos los proveedor a enviar notificacion
    $scope.selAllGridProveedor = function () {
        $scope.ListaProveedores = [];
        $scope.ListaProveedores = $scope.GridlistaProveedores.slice();
    }
    //Quitar proveedor a enviar notificacion
    $scope.quitarProvLista = function () {
        var selProveedor = $scope.selectedItem;
        if ($scope.selectedItem != null) {
            var a = $scope.ListaProveedores;
            var index = 0;
            for (index = 0; index < a.length; ++index) {
                if (a[index].codProveedor == selProveedor[0].codProveedor) break;
            }
            $scope.ListaProveedores.splice(index, 1);
        }
    }
    //Quitar todos los proveedores a enviar notificacion
    $scope.eliminarProvLista = function () {
        $scope.ListaProveedores = [];
    }
    //Cancelar seleccion de proveedores a enviar notificacion
    $scope.cancelarListaProveedor = function () {
   
        $scope.ListaProveedores = $scope.ListaProveedores2.slice();
        

       
    }
    //Guardar seleccion de proveedores a enviar notificacion
    $scope.guardarListaProveedor = function () {
        //var lineaNegChekeado = $scope.ddlenviaLinea;
        //return;

        //Si es null a todos
        if($scope.tipoEnviarProveedor == null || $scope.tipoEnviarProveedor == undefined )
        {
            $scope.tipoEnviarProveedor = [];
            $scope.tipoEnviarProveedor.codigo = "T";
            $scope.envioIndividual = false;
            $scope.txtenviarA = "Todos los proveedores";
            $scope.txtTipoEnvio = 'T';
            $scope.LimpiaLNegSelec();
        }


        //Validar que se ingrese al menos un destinatario si es Individual
        if ($scope.tipoEnviarProveedor.codigo == "I")
        {
            
            if ($scope.ListaProveedores.length == 0)
            {
               
                $scope.MenjError = "No ha seleccionado ningun destinatario.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            $scope.envioIndividual = true;
            $scope.txtenviarA = "Individual";
            $scope.txtTipoEnvio = 'I';
            $scope.LimpiaLNegSelec();
        }

        //Validar que se seleccione al menos una linea de negocio
        if ($scope.tipoEnviarProveedor.codigo == "L") {


           
            var seleccionoLinea = false;
            for (var l = 0 ; l < $scope.ddlenviaLinea.length; l++) {
                if ($scope.ddlenviaLinea[l].chekeado)
                    seleccionoLinea = true;
            }

            if (!seleccionoLinea) {

                $scope.MenjError = "No ha seleccionado ninguna línea de negocio.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
            $scope.envioIndividual = true;
            $scope.txtenviarA = "Línea de negocio";
            $scope.txtTipoEnvio = 'L';
            $scope.envioIndividual = false;
            $scope.ListaProveedores = [];
            
        }

        
        if ($scope.tipoEnviarProveedor == "" || $scope.tipoEnviarProveedor  == undefined)
        {
            $scope.ListaProveedores = [];
            $scope.ListaProveedores2 = [];
            $scope.txtBusRuc = "";
            $scope.Notificacion.Proveedores = "[Todos]";
            $scope.txtenviarA = "Todos los proveedores";
            $scope.txtTipoEnvio = 'T';
            $scope.LimpiaLNegSelec();
        }
        else
        {
            var a = $scope.ListaProveedores;
            var index;
            var cadena = '';
            for (index = 0; index < a.length; ++index) {
                cadena = cadena + '[' + a[index].codProveedor + '-' + a[index].razonSocial + '];';

            }
            $scope.Notificacion.Proveedores = cadena;
        }
        $('#proveedoresLista').modal('hide');
        

    }
    //Borrar datos de Notificacion
    $scope.LimpiaNotificacion = function () {
        $scope.actualizaDepFunciones();

        $scope.iscorreo = false;
        $scope.notEnviada = false;
        $('#editor1').html("");
        $scope.verButtonEliminar = false;
        $scope.valFecVenc = true;
        $scope.verFechaVencimiento = false;
        $scope.verObservacion = false;
        $scope.isIngresada = true;
        $scope.isLectura = false;
        $scope.isAprobada = false;
        $scope.isdowload = true;
        $scope.rutaArchivos = "";
        $scope.Notificacion = [];
        $scope.Notificacion.ListaAdjuntos = [];
        $scope.Notificacion.Lista = [];
        $scope.ListaProveedores = [];         
        $scope.ListaProveedores2 = [];
   
        $scope.Notificacion.FechaPublicacion = "";
        $scope.Notificacion.FechaPublicacion2 = "";
        $scope.Notificacion.FechaVencimiento = "";
         uploader2.clearQueue();
         $scope.pageContentLisProve = [];
         $scope.Zonas = [];
         $scope.NotDepartamaneto = [];
         $scope.txtBusRuc = "";
         $scope.isnew = false;
         $('#upload2').val("");
         $('#upload1').val("");
         isVisualizacion = false;
         $scope.nomUltFileUpload1 = "";
        $scope.codigoNot = 0;
        var secuencia = "";
        NotificacionService.getSecuenciaDirectorio("Notificacion").then(function (results) {
            if (results.data.success) {
             
                secuencia = results.data.root[0];
                var direc = "Not_" + secuencia;
                $scope.rutaDirectorio = direc;
                var serviceBase = ngAuthSettings.apiServiceBaseUri;
                var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + direc;
                $scope.uploader2.url = Ruta;
            }
        }, function (error) {
        });
        $scope.LimpiaLNegSelec();
        $scope.txtenviarA = "Todos los proveedores";
        $scope.txtTipoEnvio = "T";
       
        ///CARGA DE ARCHIVO 
       
    };
    //Grabar Notificacion 
   
    //Limpiar lineas de negocios seleccionadas
    $scope.LimpiaLNegSelec = function () {
        
        for (var l = 0 ; l < $scope.ddlenviaLinea.length; l++) {
            $scope.ddlenviaLinea[l].chekeado = false;
              
        }

    };
    $scope.ConfirmargrabarCorreo = function () {
        var a = $('#editor1').html();
       
        //Validar asunto de correo
        if ($scope.Notificacion.Asunto == "" || $scope.Notificacion.Asunto == undefined) {
            $scope.MenjError = "Ingresar asunto de correo.";
            $('#idMensajeInformativo').modal('show');

            return;
        }
        //Validar mensaje de correo
        if (a == "") {
            $scope.MenjError = "Ingresar mensaje de correo.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if ($scope.Notificacion.FechaPublicacion2 == "" || $scope.Notificacion.FechaPublicacion2 == undefined) {
            $scope.MenjError = "Ingresar fecha de publicación.";
            $('#idMensajeInformativo').modal('show');

            return;
        }
        var hoy = new Date();
        var dia = hoy.getDate();
        var mes = hoy.getMonth();
        var anio = hoy.getFullYear();
       
        var fecha_actual = new Date(anio, mes, dia);

        var dia2 = $scope.Notificacion.FechaPublicacion2.getDate();
        var mes2 = $scope.Notificacion.FechaPublicacion2.getMonth();
        var anio2 = $scope.Notificacion.FechaPublicacion2.getFullYear();
        var fecha_actual2 = String(dia2 + "/" + mes2 + "/" + anio2);
        var fecha_actual2 = new Date(anio2, mes2, dia2);


        if (fecha_actual2 < fecha_actual) {
            $scope.MenjError = "Fecha de publicación de notificación debe de ser mayor o igual a fecha actual.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        $scope.MenjConfirmacion = "¿Está seguro que desea guardar correo?";
        $('#idMensajeConfirmacion').modal('show');

    };
    $scope.Confirmargrabar = function () {

        //Validar que se ingrese fecha de vencimiento
        if (!$scope.verFechaVencimiento)
        {
            if ($scope.Notificacion.FechaVencimiento == undefined)
            {
                $scope.MenjError = "Indicar fecha de vencimiento de notificación.";
                $('#idMensajeInformativo').modal('show');

                return;
            }
           
        }

        //Validar que se adjunte un documento
        if ($scope.Notificacion.ListaAdjuntos.length == 0) {
            $scope.MenjError = "Adjuntar al menos un documento";
            $('#idMensajeInformativo').modal('show');
           
            return;
        }

        //validar fecha de vencimiento y de publicacion

        var hoy = new Date();
        if (!$scope.verFechaVencimiento) {
            if ($scope.Notificacion.FechaVencimiento <= hoy)
            {
                $scope.MenjError = "Fecha de vencimiento de notificación debe de ser mayor a fecha actual.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }
        var dia = hoy.getDate();
        var mes = hoy.getMonth() ;
        var anio = hoy.getFullYear();
        //var fecha_actual = String(anio, mes,  dia );
        var fecha_actual = new Date(anio, mes, dia);

        var dia2 = $scope.Notificacion.FechaPublicacion.getDate();
        var mes2 = $scope.Notificacion.FechaPublicacion.getMonth() ;
        var anio2 = $scope.Notificacion.FechaPublicacion.getFullYear();
        var fecha_actual2 = String(dia2 + "/" + mes2 + "/" + anio2);
        var fecha_actual2 = new Date(anio2, mes2, dia2);

        if (fecha_actual2 < fecha_actual) {
            $scope.MenjError = "Fecha de publicación de notificación debe de ser mayor o igual a fecha actual.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if (!$scope.verFechaVencimiento) {
            if ($scope.Notificacion.FechaVencimiento <= $scope.Notificacion.FechaPublicacion) {
                $scope.MenjError = "Fecha de vencimiento de notificación debe de ser mayor a fecha de publicación.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }


        $scope.MenjConfirmacion = "¿Está seguro que desea guardar la notificación?";
        $('#idMensajeConfirmacion').modal('show');

           
    }
    

    $scope.ConfirmargrabarPdf = function () {


        //Validar ingreso de deparatamentos y zonas
        //Validar departamento
        //Validar que se adjunte un documento
        if ($scope.eTipoNotif == 'G') {
            //$scope.Notificacion.Titulo = '[NOTIFICACION GENERAL]';
            if($scope.Notificacion.ListaAdjuntos.length > 0)
	        {
	          var nomGraNot =  $scope.Notificacion.ListaAdjuntos[0].split(".");;
              $scope.Notificacion.Titulo = nomGraNot[0];
            }
            
            $scope.Notificacion.Prioridad = $scope.TipoPrioridad[0];
            $scope.Notificacion.FechaPublicacion = new Date();
            $scope.Notificacion.FechaVencimiento = new Date();
            $scope.Notificacion.FechaVencimiento.setDate($scope.Notificacion.FechaVencimiento.getDate() + 1);
        }

        if ($scope.Notificacion.ListaAdjuntos.length == 0) {
            $scope.MenjError = "Adjuntar al menos un documento";
            $('#idMensajeInformativo').modal('show');

            return;
        }

        $scope.MenjConfirmacion = "¿Está seguro que desea guardar la notificación?";
        $('#idMensajeConfirmacion').modal('show');

    }


    $scope.grabar = function () {

        if ($scope.accion == 1 || $scope.accion == 2 || $scope.accion == 3)
        {
            $scope.enviaAprobacionTrx($scope.estadoTrx);
            $scope.accion = "";
        }
        else
        {
            if ($scope.accion == 9)
            {
                $scope.mostrarObseracionTrx($scope.estadoObser);
                $scope.accion = "";
            }
            else {

                if ($scope.accion == 10) {
                    $scope.agrupaEnviaTrx();
                    $scope.accion = "";
                }

                else {

                    $scope.Notificacion.Comunicado = "prueba";
                    //Grabar nueva notificacion
                    $scope.notGrabar = new Object();
                    $scope.notGrabar.CodNotificacion = $scope.codigoNot;
                    $scope.notGrabar.Titulo = $scope.Notificacion.Titulo;
                    $scope.notGrabar.Categoria = "C";
                    if ($scope.Notificacion.Prioridad != undefined)
                        $scope.notGrabar.Prioridad = $scope.Notificacion.Prioridad.codigo;
                    $scope.notGrabar.Comunicado = $scope.Notificacion.ListaAdjuntos[0];
                    $scope.notGrabar.Estado = "I";
                    $scope.notGrabar.Obligatorio = "N";
                    if ($scope.Notificacion.Obligatorio) $scope.notGrabar.Obligatorio = "S";
                    $scope.notGrabar.Corporativo = "0";
                    if ($scope.Notificacion.Corporativo) $scope.notGrabar.Corporativo = "1";
            
                    $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos.slice();
                    //if ($scope.tipoEnviarProveedor == undefined) $scope.notGrabar.Tipo = "T"
                    //else
                    // $scope.notGrabar.Tipo = $scope.tipoEnviarProveedor.codigo;
                    $scope.notGrabar.Ruta = $scope.rutaDirectorio;
                    $scope.notGrabar.Usuario = $scope.userSesion;

                    //if ($scope.tipoEnviarProveedor == "" || $scope.tipoEnviarProveedor == undefined)
                    //{
                    //}

                    //Departamentos
                    if ($scope.NotDepartamaneto == undefined)
                        $scope.notGrabar.ListaDepartamentos = [];
                    else
                        $scope.notGrabar.ListaDepartamentos = $scope.NotDepartamaneto;
                    //for (var t = 0 ; t < $scope.NotDepartamaneto.length; t++)
                    //{
                    //    alert('llego');

                    //}

                    if ($scope.tipoEnviarProveedor == null) {
                        $scope.notGrabar.Tipo = "T";
                    }
                    else {
                        if ($scope.tipoEnviarProveedor.codigo == 'L') {
                            $scope.notGrabar.Tipo = "L";
                            $scope.notGrabar.ListaLineasNegocios = [];
                            for (var l = 0 ; l < $scope.ddlenviaLinea.length; l++) {
                                if ($scope.ddlenviaLinea[l].chekeado)
                                    $scope.notGrabar.ListaLineasNegocios.push($scope.ddlenviaLinea[l]);
                            }
                        }
                        else {
                            if ($scope.ListaProveedores == undefined) {
                                $scope.notGrabar.Tipo = "T";
                            }
                            else {
                                if ($scope.ListaProveedores.length > 0) {
                                    $scope.notGrabar.Tipo = "I";
                                    $scope.notGrabar.ListaProveedores = $scope.ListaProveedores;
                                }

                                else $scope.notGrabar.Tipo = "T";

                            }
                        }
                    }



                    if ($scope.valFecVenc) $scope.notGrabar.FechaVencimiento = $scope.Notificacion.FechaVencimiento;
                    $scope.notGrabar.FechaPublicacion = $scope.Notificacion.FechaPublicacion;


                
                    //Realizar Carga de archivos
                    if ($scope.uploader2.queue.length > 0) {
                        if ($scope.notifGeneral) {
                            $scope.grabaNotificacion();
                        }
                        else {
                            $scope.myPromise = uploader2.uploadAll();
                        }
                    }
                    else {
                        $scope.grabaNotificacion();
                    }
                }
            }
        
        }
       
    }



    //Grabar Notificacion despues de carga de archivos
    $scope.grabaNotificacion = function () {

        $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos;
        if ($scope.enviaCorreo) {
            $scope.notGrabar.TipoCorreo = "S";
            $scope.notGrabar.Titulo = $scope.Notificacion.Asunto;
            $scope.notGrabar.Prioridad = "N";
            $scope.notGrabar.FechaPublicacion = $scope.Notificacion.FechaPublicacion2;
        } else if ($scope.notifGeneral) {
            $scope.notGrabar.TipoCorreo = "G";
        } else {
            $scope.notGrabar.TipoCorreo = "N";
        }
        $scope.notGrabar.AsuntoCorreo = "";
        
        $scope.notGrabar.ListaZonasNot = $scope.Zonas;
        $scope.notGrabar.ListaDepFuncNot = $scope.DepFuncNot;
        $scope.notGrabar.ListaRolesNot = $scope.NotRoles;

        $scope.notGrabar.MensajeCorreo = $('#editor1').html();
        $scope.myPromise =
            NotificacionService.getGrabaNotificacion($scope.notGrabar).then(function (results) {
                if (results.data.success) {
                               
                        $scope.Notificacion = [];
                        $scope.Notificacion.ListaAdjuntos = [];
                        $scope.Notificacion.Lista = [];
                        $scope.ListaProveedores = [];
                        $scope.ListaProveedores2 = [];
                        uploader2.clearQueue();
                        $scope.isnew = true;
                        $scope.isdowload = true;
                        $scope.isIngresada = true;
                        $scope.ConsultaNotificaciones('T');
                        $scope.pageContentLisProve = [];
                        $scope.txtBusRuc = "";
                        $scope.LimpiaLNegSelec();
                        $scope.MenjError = "Notificación guardada correctamente";
                        $('#idMensajeOk').modal('show');
                        $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');
                }
                else {
                    $scope.MenjError = "Error al realizar transacción.";
                    $('#idMensajeError').modal('show');

                }
            }, function (error) {
                $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                $('#idMensajeError').modal('show');
            });
        ;

    };


    //Seleccionar Notificacion desde Grid
    $scope.SelecionarGridNotificacion = function (codNotificacion, tituloNot, fecVenNot, categNot, prioNot, obliNot, rutadir, estado, observacion, tipo, mensajeCorreo, tipoCorreo, fecPublicacion, corporativo) {
        //$scope.LimpiaNotificacion();   
     
        $scope.iscorreo = true;
        if (tipoCorreo == "S") {
            //$scope.etipoCorreo = true;
            $scope.eTipoNotif = 'C';
            $scope.enviaCorreo = true;
            $scope.notifGeneral = false;
           
            $scope.Notificacion.Asunto = tituloNot;
            $('#editor1').html(mensajeCorreo);
        } else if (tipoCorreo == "N") {
            //$scope.etipoCorreo = false;
            $scope.eTipoNotif = 'P';
            $scope.enviaCorreo = false;
            $scope.notifGeneral = false;
        }
        else {
            $scope.eTipoNotif = 'G';
            $scope.enviaCorreo = false;
            $scope.notifGeneral = true;
        }
        $scope.isOCultaOpciones = false;
        $scope.verButtonEliminar = true;
        $scope.verObservacion = false;
        if (estado == "Enviada para Aprobación") {
            $scope.provisPendiente = false;
        }
        else
        {
            $scope.provisPendiente = true;
        }
        if (estado == "Rechazado" || estado == "Devuelto") $scope.verObservacion = true;
       
        
        $scope.isdowload = false;
        $scope.isnew = false;
        
        $scope.envioIndividual = false;
        if (tipo == "T")
        {
            $scope.txtenviarA = "Todos los proveedores";
            $scope.txtTipoEnvio = "T";
            $scope.tipoEnviarProveedor = [];
            
        }
        else
        {

            if (tipo == "L") {
                $scope.txtenviarA = "Línea de negocio";
                $scope.txtTipoEnvio = "L";
                $scope.tipoEnviarProveedor = [];
               
            }
            if (tipo == "I") {
                $scope.txtenviarA = "Individual";
                $scope.txtTipoEnvio = "I";
                $scope.tipoEnviarProveedor = [];
                $scope.envioIndividual = true;
            }
            
        }
      
 
        if(estado == "Ingresado")
        {
            $scope.isIngresada = false;
            
        }
        else
            $scope.isIngresada = true;
        if (estado == "Enviada para Aprobación" || estado == "Aprobado" || estado == "Rechazado" || estado == "Publicado") {
            $scope.isLectura = true;
             

        }
        else
            $scope.isLectura = false;
        if (estado == "Aprobado" ) {
            $scope.isAprobada = true;

        }
        else {
            
            $scope.isAprobada = false;
        }
            
        
        $scope.rutaDirectorio = rutadir;
        $scope.codigoNot = codNotificacion;
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + rutadir;
        $scope.uploader2.url = Ruta;

        $('.nav-tabs a[href="#RegistroNotificacion"]').tab('show');
        $scope.Notificacion.Titulo = tituloNot;
        $scope.Notificacion.Observacion = observacion;
        
        for (var l = 0 ; l < $scope.TipoCategoria.length;l++)
        {
            if ($scope.TipoCategoria[l].detalle == categNot) break;

        }
        $scope.Notificacion.Categoria = $scope.TipoCategoria[l];
        for (var l = 0 ; l < $scope.TipoPrioridad.length; l++)
        {
            if ($scope.TipoPrioridad[l].detalle == prioNot) break;

        }
        $scope.Notificacion.Prioridad = $scope.TipoPrioridad[l];
        $scope.Notificacion.Obligatorio = false;
        if (obliNot == "Si") $scope.Notificacion.Obligatorio = true;
        $scope.Notificacion.Corporativo = false;
        if (corporativo == "1") $scope.Notificacion.Corporativo = true;
        if ("01/01/1900" != fecVenNot) {
            $scope.Notificacion.FechaVencimiento = "";
            $scope.valFecVenc = true;
            $scope.verFechaVencimiento = false;
            $scope.Notificacion.FechaVencimiento = new Date(fecVenNot);
        }
        else {
            $scope.valFecVenc = false;
            $scope.verFechaVencimiento = true;
            $scope.Notificacion.FechaVencimiento = new Date("1900-01-01");
        }
        $scope.Notificacion.FechaPublicacion = new Date(fecPublicacion);
        $scope.Notificacion.FechaPublicacion2 = new Date(fecPublicacion);
        //Consulta lista de proveedores y lista de archivos
        $scope.myPromise =
        NotificacionService.getConsultaLisProveedores("2", codNotificacion).then(function (results) {

            if (results.data.success) {
     
                var iscomunicado = false;
                $scope.ListaProveedores = results.data.root[0];
                $scope.Notificacion.ListaAdjuntos = results.data.root[1];
                $scope.Notificacion.ListaLineasSelec = results.data.root[2];
                $scope.Notificacion.ListaDepartamentos = results.data.root[3];
         
                $scope.Notificacion.Zonas = results.data.root[4];
                $scope.Notificacion.NotRoles = results.data.root[5];
                var listaDep = results.data.root[6];
                $scope.actualizaDepFunciones();
                //Carga de Departamentos y Funciones
                $scope.DepFuncNot = [];
                for (var index = 0; index < $scope.DepartamentoNot.length; index++) {

                    $scope.DepFunIng = {}
                    $scope.DepFunIng.expanded = false;
                    $scope.DepFunIng.IdDepartamento = $scope.DepartamentoNot[index].codigo;
                    $scope.DepFunIng.DesDepartamento = $scope.DepartamentoNot[index].detalle;
                    var auxListaFuncion = $filter('filter')($scope.FuncionNot, { descAlterno: $scope.DepartamentoNot[index].codigo }, true);
                    $scope.DepFunIng.ListaFunciones = [];
                    for (var idx = 0; idx < auxListaFuncion.length; idx++) {
                        $scope.FuncIngreso = {};
                        $scope.FuncIngreso.IdDepartamento = $scope.DepartamentoNot[index].codigo;
                        $scope.FuncIngreso.IdFuncion = auxListaFuncion[idx].codigo;
                        $scope.FuncIngreso.DesFuncion = auxListaFuncion[idx].detalle;
                        //Validar si esta registrado
                        var valDepFun = $filter('filter')(listaDep, { idDepartamento: $scope.DepartamentoNot[index].codigo, idFuncion: auxListaFuncion[idx].codigo }, true);
                        if (valDepFun.length>0)
                            $scope.FuncIngreso.isCheck = true;
                        else
                            $scope.FuncIngreso.isCheck = false;

                        $scope.DepFunIng.ListaFunciones.push($scope.FuncIngreso);
                    }
                    $scope.DepFuncNot.push($scope.DepFunIng);
                }

                //var listaDepartamentos = $scope.DepartamentoNot;
                //var listaDepartamentos2 = $scope.NotDepartamaneto;
                $scope.NotDepartamaneto = [];
                $scope.Zonas = [];
                for (var d = 0; d < $scope.Notificacion.Zonas.length; d++) {
                    $scope.Zonas.push(
                   { id: $scope.Notificacion.Zonas[d] });
                }

                $scope.NotRoles = [];
                for (var d = 0; d < $scope.Notificacion.NotRoles.length; d++) {
                    $scope.NotRoles.push(
                   { id: $scope.Notificacion.NotRoles[d] });
                }

                for (var d = 0; d < $scope.Notificacion.ListaDepartamentos.length;d++)
                {
                    $scope.NotDepartamaneto.push(
                   { id: $scope.Notificacion.ListaDepartamentos[d] });
                }
               

                $scope.nomUltFileUpload1 = "";

                //var listaDepartamentos3 = $scope.SettingDepartamentoNot;
                if ($scope.Notificacion.ListaLineasSelec.length > 0)
                {

                  $scope.ddlenviaLinea = [];
                  for (var j = 0 ; j < $scope.lineaNegocio.length; j++) {
                        $scope.ddlenviaLinea.push(
                           { codigo: $scope.lineaNegocio[j].codigo, descripcion: $scope.lineaNegocio[j].detalle, chekeado: false });

                    }

                for (var l = 0; l < $scope.ddlenviaLinea.length; l++)
                  {
                    for (var y = 0; y < $scope.Notificacion.ListaLineasSelec.length ; y++)
                    {
                        if ($scope.ddlenviaLinea[l].codigo == $scope.Notificacion.ListaLineasSelec[y])
                        {
                            $scope.ddlenviaLinea[l].chekeado = true;
                           
                        }
                    }
                     
                  }
                }
                $scope.Notificacion.Lista = [];
                for (var e = 0 ; e < $scope.Notificacion.ListaAdjuntos.length; e++) {
                    if (e == 0) iscomunicado = true
                    else iscomunicado = false

                    $scope.Notificacion.Lista.push(
                    { name: $scope.Notificacion.ListaAdjuntos[e], iscomunicado: iscomunicado });

                    if ($scope.notifGeneral && iscomunicado) { $scope.nomUltFileUpload1 = $scope.Notificacion.ListaAdjuntos[e]; }
                }
                if ($scope.Notificacion.ListaAdjuntos.length > 1) $scope.listaAdjplus = true;
                else $scope.listaAdjplus = false;

                $scope.ListaDowload = $scope.Notificacion.ListaAdjuntos.slice();
                $scope.ListaDowload.push(rutadir);
                $scope.myPromise =
                NotificacionService.getDescargarArchivos($scope.ListaDowload).then(function (results) {

                    $scope.rutaArchivos = results;

                }, function (error) {
                });
                ;
            }
        }, function (error) {
        });
        ;

        if (estado == "Publicado")
            $scope.notEnviada = true;
        else
            $scope.notEnviada = false;
        
     }

    //Confirmar Enviar aprobacion notificacion  
    $scope.enviaAprobacion = function (estado) {
     
        $scope.estadoTrx = estado;
        if ($scope.accion == 1)
        {
            $scope.MenjConfirmacion = "¿Está seguro de enviar la notificación para aprobación?";
            
        }
        if ($scope.accion == 2) {
            $scope.MenjConfirmacion = "¿Está Seguro de Publicar la Notificación para Lectura del Proveedor?";

        }
        if ($scope.accion == 3) {
            $scope.MenjConfirmacion = "¿Está seguro de aprobar la notificación?";

        }

        $('#idMensajeConfirmacion').modal('show');
        return;
        
    };

    //Enviar aprobacion notificacion    
    $scope.enviaAprobacionTrx = function (estado) {
        
        $('#idObservavionMensaje').modal('hide');
        if (estado == 'Z') estado = $scope.estadoObser;

        var observacion = "";
        if (estado == "R" || estado == "D")
            observacion = $scope.ObservacionIng;
        else
            observacion = "";
        $scope.ObservacionIng = "";
        $scope.estadoObser = "";
        $scope.myPromise =
        NotificacionService.getActualizaEstado($scope.codigoNot, estado, $scope.userSesion, observacion).then(function (results) {
            if (results.data.success) {
                var actEstado = 'P';
                if (estado == 'P') {
                    actEstado = 'T';
                    $scope.MenjError = "Notificación pendiente de aprobación.";

                }
                if (estado == 'X')
                { actEstado = 'T'; $scope.MenjError = "Notificación  eliminada correctamente.";  }
                if (estado == 'A')
                { $scope.MenjError = "Notificación aprobada correctamente."; }
                if (estado == 'R')
                { $scope.MenjError = "Notificación rechazada.";  }
                if (estado == 'D')
                { $scope.MenjError = "Notificación devuelta.";  }
                if (estado == 'E') {
                    actEstado = 'T';
                    $scope.MenjError = "Notificación Publicada Correctamente.";
                    
                }

                $scope.isOCultaOpciones = true;
                $scope.verButtonEliminar = false;
                $scope.isAprobada = false;
                $scope.isIngresada = true;
                if ($scope.MenjError == "") return;
                $('#idMensajeOk').modal('show');
                $scope.Notificacion = [];
                $scope.Notificacion.ListaAdjuntos = [];
                $scope.Notificacion.Lista = [];
                $scope.ListaProveedores = [];
                $scope.ListaProveedores2 = [];
                uploader2.clearQueue();
                $scope.isnew = true;

                $scope.isdowload = true;

                $scope.isIngresada = true;
                $scope.ConsultaNotificaciones(actEstado);
                $scope.pageContentLisProve = [];
                $scope.txtBusRuc = "";
                $scope.LimpiaLNegSelec();
                $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');

            }
            if (!results.data.success) {
                $scope.MenjError = results.data.msgError;
                $('#idMensajeError').modal('show');
            }
            
        }, function (error) {
        });
        ;



        ///CARGA DE ARCHIVO 
    };



    //agrupar notificaciones
    $scope.agrupaEnvia = function () {
        
        $scope.MenjConfirmacion = "¿Está Seguro de Publicar la Notificación para Lectura del Proveedor?";
        $scope.accion = 10;
        $('#idMensajeConfirmacion').modal('show');


    }

    //agrupar notificaciones
    $scope.agrupaEnviaTrx = function () {
        NotificacionService.getagrupaNotificacion($scope.agrupacionNotificacion[0], $scope.agrupacionNotificacion[1]).then(function (results) {

            $scope.MenjError = "Notificación Publicada Correctamente";
            $('#idMensajeOk').modal('show');
            $scope.ConsultaNotificaciones("A");
            $scope.isAgrEnv = true;

        }, function (error) {
        });
       

    }


    //agrupar notificaciones
    $scope.selCodAgrupar = function (codigo) {

        $scope.isAgrEnv = true;
        var longAgrupa = $scope.agrupacionNotificacion.length;
        var selc = "#CodigoAgrupar" + codigo;

        //Validar destinarios sean iguales
        if (longAgrupa == 1) {
            //Validar Tipos de Envios
            var tipoenvio1 = "X";
            var tipoenvio2 = "Z";
            var fecha1 = "X";
            var fecha2 = "Z";
            var fecha3 = "X";
            var fecha4 = "Z";
            for (var l = 0; l < $scope.GridNotificacion.length; l++) {
                if ($scope.GridNotificacion[l].codNotificacion == codigo) {
                    tipoenvio1 = $scope.GridNotificacion[l].tipo;
                    fecha1 = $scope.GridNotificacion[l].fechaVencimiento;
                    fecha3 = $scope.GridNotificacion[l].fechaPublicacion;
                }
                if ($scope.GridNotificacion[l].codNotificacion == $scope.agrupacionNotificacion[0]) {
                    tipoenvio2 = $scope.GridNotificacion[l].tipo;
                    fecha2 = $scope.GridNotificacion[l].fechaVencimiento;
                    fecha4 = $scope.GridNotificacion[l].fechaPublicacion;
                }
            }



            if (tipoenvio1 != tipoenvio2) {
                $scope.MenjError = "Debe seleccionar notificaciones con los mismos destinatarios.";
                $('#idMensajeInformativo').modal('show');
                $(selc).attr('checked', false);
                for (var i = 0; i < longAgrupa; i++) {
                    if ($scope.agrupacionNotificacion[i] == codigo) break;
                }
                $scope.agrupacionNotificacion.splice(i, 1);
                $scope.isAgrEnv = true;
                return;
            }

            if (fecha1 != fecha2) {
                $scope.MenjError = "Fecha de vencimiento de las notificaciones deben ser iguales.";
                $('#idMensajeInformativo').modal('show');
                $(selc).attr('checked', false);
                for (var i = 0; i < longAgrupa; i++) {
                    if ($scope.agrupacionNotificacion[i] == codigo) break;
                }
                $scope.agrupacionNotificacion.splice(i, 1);
                $scope.isAgrEnv = true;
                return;
            }
            if (fecha3 != fecha4) {
                $scope.MenjError = "Fecha de publicación de las notificaciones deben ser iguales.";
                $('#idMensajeInformativo').modal('show');
                $(selc).attr('checked', false);
                for (var i = 0; i < longAgrupa; i++) {
                    if ($scope.agrupacionNotificacion[i] == codigo) break;
                }
                $scope.agrupacionNotificacion.splice(i, 1);
                $scope.isAgrEnv = true;
                return;
            }

            var listaProveedores1 = [];
            var listaProveedores2 = [];
            var lineaNegocio1 = [];
            var lineaNegocio2 = [];
            $scope.myPromise =
              NotificacionService.getConsultaLisProveedores("2", codigo).then(function (results) {
                  if (results.data.success) {
                      listaProveedores1 = results.data.root[0];
                      lineaNegocio1 = results.data.root[2];
                      $scope.myPromise =
                      NotificacionService.getConsultaLisProveedores("2", $scope.agrupacionNotificacion[0]).then(function (results) {
                          if (results.data.success) {
                              listaProveedores2 = results.data.root[0];
                              lineaNegocio2 = results.data.root[2];
                              //Si es individual validar que sean iguales los destinatarios
                              if (tipoenvio1 == "I" && tipoenvio2 == "I") {
                                  //Validar que tengas la misma cantidad de destinatarios
                                  if (listaProveedores1.length != listaProveedores2.length) {
                                      $scope.MenjError = "Debe seleccionar notificaciones con los mismos destinatarios.";
                                      $('#idMensajeInformativo').modal('show');
                                      $(selc).attr('checked', false);
                                      for (var i = 0; i < longAgrupa; i++) {
                                          if ($scope.agrupacionNotificacion[i] == codigo) break;
                                      }
                                      $scope.agrupacionNotificacion.splice(i, 1);
                                      $scope.isAgrEnv = true;
                                      return;
                                  }
                                  //validar que sean los mismos destinarios
                                  var existe = false
                                  for (var x = 0; x < listaProveedores1.length; x++) {
                                      existe = false;
                                      for (var y = 0; y < listaProveedores2.length; y++) {
                                          if (listaProveedores2[y].codProveedor == listaProveedores1[x].codProveedor) {
                                              existe = true;
                                          }
                                      }
                                      if (!existe) break;
                                  }

                                  if (!existe) {
                                      $scope.MenjError = "Debe seleccionar notificaciones con los mismos destinatarios.";
                                      $('#idMensajeInformativo').modal('show');
                                      $(selc).attr('checked', false);
                                      for (var i = 0; i < longAgrupa; i++) {
                                          if ($scope.agrupacionNotificacion[i] == codigo) break;
                                      }
                                      $scope.agrupacionNotificacion.splice(i, 1);
                                      $scope.isAgrEnv = true;
                                      return;
                                  }

                              }
                              //Si es Linea de negocio validar que sean iguales
                              if (tipoenvio1 == "L" && tipoenvio2 == "L") {
                                  //Validar que tenga la misma cantidad de linead de negocios
                                  if (lineaNegocio1.length != lineaNegocio2.length) {
                                      $scope.MenjError = "Debe seleccionar notificaciones con los mismos destinatarios.";
                                      $('#idMensajeInformativo').modal('show');
                                      $(selc).attr('checked', false);
                                      for (var i = 0; i < longAgrupa; i++) {
                                          if ($scope.agrupacionNotificacion[i] == codigo) break;
                                      }
                                      $scope.agrupacionNotificacion.splice(i, 1);
                                      $scope.isAgrEnv = true;
                                      return;
                                  }
                                  //validar que sean las misma lienas de negocio
                                  var existe = false
                                  for (var x = 0; x < lineaNegocio1.length; x++) {
                                      existe = false;
                                      for (var y = 0; y < lineaNegocio2.length; y++) {
                                          if (lineaNegocio2[y] == lineaNegocio1[x]) {
                                              existe = true;
                                          }
                                      }
                                      if (!existe) break;
                                  }

                                  if (!existe) {
                                      $scope.MenjError = "Debe seleccionar notificaciones con los mismos destinatarios.";
                                      $('#idMensajeInformativo').modal('show');
                                      $(selc).attr('checked', false);
                                      for (var i = 0; i < longAgrupa; i++) {
                                          if ($scope.agrupacionNotificacion[i] == codigo) break;
                                      }
                                      $scope.agrupacionNotificacion.splice(i, 1);
                                      $scope.isAgrEnv = true;
                                      return;
                                  }


                              }

                          }
                      }, function (error) {
                      });
                      ;

                  }
              }, function (error) {
              });
            ;
            
        }


        if ($(selc).is(':checked')) {
            if (longAgrupa < 2) {
                $scope.agrupacionNotificacion.push(codigo);
            }
            else {
                $scope.MenjError = "Solo puede seleccionar dos notificaciones.";
                $('#idMensajeInformativo').modal('show');

                $(selc).attr('checked', false);

            }

        }
        else {
            for (var i = 0; i < longAgrupa; i++) {
                if ($scope.agrupacionNotificacion[i] == codigo) break;
            }
            $scope.agrupacionNotificacion.splice(i, 1);

        }


        longAgrupa = $scope.agrupacionNotificacion.length;
        if (longAgrupa == 2) $scope.isAgrEnv = false;

    }



    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=prueba';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });

    $scope.visualizar = function () {
        isVisualizacion = true;

        $('#visualizarpdf_G').modal('show');
    }

    $scope.getPreviewPDFrameSrc = function (pdfName) {
        if (pdfName == '')
            pdfName = 'PREVISUALIZACION.PDF';
        return '../PDF/' + pdfName;
    };

    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            
            var i = $scope.Notificacion.Lista.length;
            if (i == 0)
            { iscomunicado = true; $scope.listaAdjplus = false; }
            else
            { iscomunicado = false; $scope.listaAdjplus = true; }
            if ($scope.notifGeneral) { iscomunicado = true; }
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();

            if (iscomunicado && !$scope.enviaCorreo) {
                if (extension == "pdf" )
                    return true;
                else {
                   
                    $scope.MenjError = "El tipo de archivo para la notificación debe de ser de tipo PDF.";
                    $('#idMensajeInformativo').modal('show');
                    $('#upload2').val("");
                    //$('#upload1').val("");
                    return false;
                    
                }
            }
            else
            {
                if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "jpg" || extension == "xls" || extension == "xlsx")
                    return true;
                else {
                    $scope.MenjError = "Tipo de archivo invalido.";
                    $('#idMensajeError').modal('show');
                    $('#upload2').val("");
                    //$('#upload1').val("");
                    return false;
                }

            }
            
        }
    });
    uploader2.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / (1024 * 1024);
            if (fileSize <= 2)
                return true;
            else {
              
                $scope.MenjError = "Archivo demasiado grande. Maximo 2 MG.";
                $('#idMensajeError').modal('show');
                if ($scope.Notificacion.Lista.length > 1) $scope.listaAdjplus = true;
                else $scope.listaAdjplus = false;
              
              
                $('#upload2').val("");
                //$('#upload1').val("");
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
                $scope.MenjError = "Solo se puede adjuntar maximo 5 archivos .";
                $('#idMensajeError').modal('show');
                


                $('#upload2').val("");
                //$('#upload1').val("");
                return false;
            }
        }
    });
    // CALLBACKS
    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader2.onAfterAddingFile = function (fileItem) {
        $('#upload2').val("");
        //$('#upload1').val("");
        var iscomunicado = false;
        $scope.isIngresada = true;
        
        $scope.isdowload = true;
        var i = $scope.Notificacion.Lista.length;
        if (i == 0) iscomunicado = true;

        if ($scope.notifGeneral) {
            iscomunicado = true;
            $scope.Notificacion.ListaAdjuntos = [];
        }
        
        $scope.Notificacion.ListaAdjuntos.push(fileItem.file.name);
        //$scope.Notificacion.Lista.push();
        $scope.Notificacion.Lista.push(
            { name: fileItem.file.name, iscomunicado: iscomunicado });
        //alert('Files ready for upload.');
        if ($scope.notifGeneral) {
            isVisualizacion = true;
            $scope.notGrabar = new Object();
            $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos;
            $scope.nomUltFileUpload1 = "";
            $scope.myPromise = uploader2.uploadAll();
            $scope.nomUltFileUpload1 = fileItem.file.name;
        }
    };

    uploader2.onSuccessItem = function (fileItem, response, status, headers) {
        if ($scope.uploader2.progress == 100)
        {
            $scope.notGrabar.ListaAdjuntos.push($scope.rutaDirectorio); 
            $scope.myPromise =
            NotificacionService.getUploadFileSFTP($scope.notGrabar.ListaAdjuntos).then(function (results) {
                if (results.data.success) {
                    if (isVisualizacion) {
                        isVisualizacion = false;
                        $scope.notGrabar = new Object();
                        $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos;
                    }
                    else {
                        $scope.grabaNotificacion();
                    }

                }
                else {
                    $scope.MenjError = "Error al realizar carga de archivo(s).";
                    $('#idMensajeError').modal('show');
                }
                
            }, function (error) {

                $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
                $('#idMensajeError').modal('show');
            });
            ;
        }



            
    };
    uploader2.onErrorItem = function (fileItem, response, status, headers) {
        
    };
    uploader2.onCancelItem = function (fileItem, response, status, headers) {
     
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
    //FIN DE CARGA DE ARCHIVOS

    


    //Carga funcion al iniciar para no dar dos veces al boton consultar proveedor
    //$scope.ConsultarProveedores();

}
]);

app.controller('VisualizaNotificacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    $scope.Notificacion = [];
    $scope.Notificacion.ListaadjuntoProveedor = [];
    $scope.Notificacion.ListaAdjuntos = [];
    $scope.archivopdf = $sce.trustAsResourceUrl("");
    $scope.archivopdf2 = $sce.trustAsResourceUrl("");
    $scope.ruc = authService.authentication.ruc;
    $scope.indece = "0";
    $scope.verAceptar = true;
    $scope.leido = false;
    $scope.leidoAgrupado = false;
    $scope.titulo = "";
    $scope.rutalocal = "";
    $scope.totalAdjuntos = 0;
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;

    //Obtener todas las notificaciones pendientes
    //Servicio para consultar proveedor
    ;
    $scope.myPromise =
    NotificacionService.getConsultaLisProveedores("3", $scope.ruc).then(function (results) {
   
        if (results.data.success) {
            $scope.Notificacion = results.data.root[0];
            $scope.presentaNot();
        }
    }, function (error) {
    });
    ;
    //presentar notificacion
    $scope.presentaNot = function () {
        var index = $scope.indece;
       
       
            //actualiza estado de notificacion a leido
            if ($scope.leido) {
                
                NotificacionService.getActualizaEstadoNot($scope.Notificacion[index - 1].codNotificacion, $scope.ruc).then(function (results) {


                }, function (error) {
                });
            }

            if ($scope.leidoAgrupado) {

                NotificacionService.getActualizaEstadoNot($scope.Notificacion[index - 1].codNotificacion, $scope.ruc).then(function (results) {


                }, function (error) {
                });
                NotificacionService.getActualizaEstadoNot($scope.Notificacion[index - 1].prioridad, $scope.ruc).then(function (results) {


                }, function (error) {
                });
            }


            $scope.indece++;
            if (index >= $scope.Notificacion.length)
            { return; }
            var ListaDowload = [];

            $scope.titulo = $scope.Notificacion[index].titulo;
            if ($scope.Notificacion[index].obligatorio == "S")
            { $scope.verAceptar = true; }
            else { $scope.verAceptar = false; }
            //ver si es notificacion agrupada
            var isAgrupado = "N";
            if ($scope.Notificacion[index].prioridad > 0) {
                for (var indece = 0; indece < $scope.Notificacion.length ; indece++)
                {
                    if ($scope.Notificacion[indece].codNotificacion == $scope.Notificacion[index].prioridad)
                    {
                        isAgrupado = "S";
                        break;
                    }
                }
                if ($scope.Notificacion[index].prioridad > 0)
                {
                    ListaDowload.push($scope.Notificacion[indece].comunicado);
                    ListaDowload.push($scope.Notificacion[indece].ruta);
                    
                    $scope.myPromise =
                    NotificacionService.getDescargarArchivos(ListaDowload).then(function (results) {

                        var urlpdf2 = results + $scope.Notificacion[indece].comunicado;
                        $scope.archivopdf2 = $sce.trustAsResourceUrl(urlpdf2);
                        $scope.Notificacion.splice(indece, 1);

                    }, function (error) {
                    });
                    ;
                   
                }

                //$('#visualizarAgrupadopdf').modal('show');
            }
            ListaDowload = [];
            
        //Consulta  lista de archivos
          
            NotificacionService.getConsultaLisProveedores("2", $scope.Notificacion[index].codNotificacion).then(function (results) {
                if (results.data.success) {


                    $scope.Notificacion.ListaAdjuntos = results.data.root[1];
                    $scope.Notificacion.ListaadjuntoProveedor = [];
                    ListaDowload.push($scope.Notificacion[index].comunicado);
                    for (var e = 0 ; e < $scope.Notificacion.ListaAdjuntos.length; e++) {
                        if (e != 0) {
                            $scope.Notificacion.ListaadjuntoProveedor.push(
                            { nomAdjunto: $scope.Notificacion.ListaAdjuntos[e] });
                            ListaDowload.push($scope.Notificacion.ListaAdjuntos[e]);

                        }
                    }
                    ListaDowload.push($scope.Notificacion[index].ruta);
           
                    $scope.totalAdjuntos = ListaDowload.length - 2;
                    $scope.myPromise =
                    NotificacionService.getDescargarArchivos(ListaDowload).then(function (results) {
                        //Presentar documentos
                        $scope.rutalocal = results;
                        var urlpdf = results + $scope.Notificacion[index].comunicado;
                        $scope.archivopdf = $sce.trustAsResourceUrl(urlpdf);


                        if ($scope.Notificacion[index].prioridad > 0) {
                            $('#visualizarpdf').modal('hide');
                            $('#visualizarAgrupadopdf').modal('show');
                        }
                        else {
                            $('#visualizarAgrupadopdf').modal('hide');
                            $('#visualizarpdf').modal('show');
                        }
                        $scope.leidoAgrupado = false;
                        $scope.leido = false;
                        index++;

                    }, function (error) {
                    });
                    ;
                }
            }, function (error) {
            });
           
        }
        
    

    //presentar notificacion
    $scope.isLeido = function () {
        var index = $scope.indece - 1;
       
        if ($scope.Notificacion[index].obligatorio == "S")
        {
            if ($scope.leido)
                $scope.verAceptar = false;
            else
                $scope.verAceptar = true;
        }
        
    }

    $scope.isLeidoAgrupado = function () {
        var index = $scope.indece - 1;
      
        if ($scope.Notificacion[index].obligatorio == "S") {
            if ($scope.leidoAgrupado)
                $scope.verAceptar = false;
            else
                $scope.verAceptar = true;
        }

    }

    


}
]);

app.controller('InformacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    $scope.ConsultaListaProductos = function (tipoLista) {

        if (tipoLista == 1)       
            $scope.iscomisariato = true;       
        else
            $scope.iscomisariato = false;
        $('#proveedoresLista').modal('show');
        $scope.pageContentLisProve = [];
        $scope.GridlistaProveedores = [];
        $scope.pageContentCho = [];
        $scope.SolProveedor = [];
        $scope.pagesCho = [];
        $scope.pagesLisProve = [];
        $scope.pagesNot = [];
        
        var meses = new Array("","Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        $scope.fechaListaProductos = "31 de Agosto del 2015";
        var str = "";
        var fechaStr = "";
        var fecha = [];


        
        NotificacionService.getConsultaListaPrecios(tipoLista).then(function (results) {

            if (results.data.success) {
              
                $scope.SolProveedor = results.data.root[0].slice();
                //$scope.pageContentCho = results.data.root[0];
            
                str = $scope.SolProveedor[0].fechaPublicacion;
                fechaStr = str.substring(0, 10);
                fecha = fechaStr.split("-");
                $scope.fechaListaProductos = fecha[2] + " de " + meses[parseInt(fecha[1])] + " del " + fecha[0];
              
            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
        }, function (error) {
        });

    };
  }
]);

app.controller('TrabajeNosotrosController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    

    $scope.pageContentCla = [];
   

    //Carga lista de notificaciones registradas
    $scope.ConsultaClasificados = function () {
        NotificacionService.getConsultaClasificados("1").then(function (results) {

            if (results.data.success) {
       
                $scope.pageContentCla = results.data.root[0];
              

            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
        }, function (error) {
        });            

    };

    $scope.direccionarIngresar = function () {
        
        $('.nav-tabs a[href="#Registrate"]').tab('show');
    };

}
]);

