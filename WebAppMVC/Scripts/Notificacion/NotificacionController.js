app.controller('NotificacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', 'FileUploader', '$sce', 'authService', 'localStorageService', '$filter', function ($scope, ngAuthSettings, NotificacionService, FileUploader, $sce, authService, localStorageService, $filter) {
    $scope.Notificacion = [];
    $scope.Notificacion.ListaAdjuntos = [];
    $scope.Notificacion.Lista = [];
    $scope.ListaDowload = [];
    $scope.GridNotificacion = [];
    $scope.GridlistaProveedores = [];
    $scope.pagesNot = [];
    $scope.pagesLisProve = [];
    $scope.pageContentNot = [];
    $scope.pageContentLisProve = [];
    $scope.allNotificaciones = [];
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
    $scope.NotificacionesPendientes = 0;
    $scope.ListadoNotificacionesPendientes = [];
    $scope.isBanderaProve = null;
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

    $scope.archivopdf = $sce.trustAsResourceUrl("");
        
    $scope.userSesion = authService.authentication.userName;
    $scope.ejecutafirst = true;
    $scope.ejecutaPrimera = true;
    $scope.ruc = authService.authentication.ruc; 
    //Variable de Mensaje
    $scope.MenjError = "";
    $scope.MenjConfirmacion = "";
    $scope.estadoObser = "";
    $scope.obliNot = "";
    $scope.usr = authService.authentication.userName;
    //Carga Catalogo Categorias Notificaciones
    NotificacionService.getCatalogo('tbl_Categorias').then(function (results) {
        $scope.TipoCategoria = results.data;
    }, function (error) {
    });

    //Carga Catalogo Prioridades Notificaciones
    $scope.myPromise =
    NotificacionService.getCatalogo('tbl_Prioridad').then(function (results) {
        $scope.TipoPrioridad = results.data;
    }, function (error) {
    });
    ;

    //Carga Catalogo Estados Notificaciones
    NotificacionService.getCatalogo('tbl_EstadoNot').then(function (results) {
        $scope.EstadoNotif = results.data;
    }, function (error) {
    });
    //Carga Catalogo Estados Notificaciones Proveedor
    NotificacionService.getCatalogo('tbl_EstadoNotProv').then(function (results) {
        $scope.EstadoNotifProv = results.data;
    }, function (error) {
    });
    //Carga Catalogo Tipo Notificaciones
    NotificacionService.getCatalogo('tbl_TipoNot').then(function (results) {
        $scope.TipoNotif = results.data;
    }, function (error) {
    });

    //Carga lista de notificaciones registradas
    $scope.ConsultaNotificaciones = function (estado) {
        NotificacionService.getConsultaNotificaciones(estado).then(function (results) {
            
            if (results.data.success) {
                $scope.pageContentNot = results.data.root[0];
                 
                if ($scope.ejecutafirst && estado== "T" ) {
                    $scope.busTodos = false;
                    $scope.selectedEstadosb = ['x', 'x'];
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

    $scope.cerrarPdf = function (estado) {

        document.getElementById("pdfvisualiza").setAttribute("src", "");
        $('#visualizarpdf').modal('hide');

    }

    $scope.mostrarObseracion = function (estado) {
      
        $scope.estadoObser = estado;
        $('#idObservavionMensaje').modal('show');
        
    }

    $scope.isLeidoProv = function (obligatorio) {
        if ($scope.obliNot == "Si") {
            if ($scope.provleido)
                $scope.provisObligatorio = false;
            else
                $scope.provisObligatorio = true;
        }

    }

    $scope.leerNotProv = function () {
        if ($scope.provleido) {
            $scope.provleido = false;
            $scope.myPromise =
            NotificacionService.getActualizaEstadoNot($scope.codigoNot, $scope.ruc, $scope.usr).then(function (results) {
                $scope.EjecutaFiltroProv();


            }, function (error) {
            });
            ;
        }
        else
            $scope.cerrarPdf();

    }
    

    // Limpiar filtros de busqueda
    $scope.confirmaEliminar = function () {
        $scope.MenjConfirmacion = "Estas seguro de eliminar la notificación?"
        $('#idMensajeConfirmacion').modal('show');
        
    };

    // Confirmacion de eliminicacion de registros
    $scope.limpiaCamposBusqueda = function () {
         
        if ($scope.PorDatosBusq != 1) $scope.txtBusTitulo = "";
        if ($scope.PorDatosBusq != 2) $scope.selectedItemPrioridad = "";
        if ($scope.PorDatosBusq != 3) $scope.selectedItemEstadoNot = "";

    };


    //Consultar Notificaciones de proveedor
    $scope.EjecutaFiltroProv = function (validar) {
        if ($scope.ejecutaPrimera)
        {
            $scope.ejecutaPrimera = false;
            return;
        }
       
        $scope.usr = authService.authentication.userName;
        $scope.myPromise =
                 NotificacionService.getConsultaListaNotificacionesProv($scope.ruc, $scope.usr).then(function (results) {
                     if (results.data.success) {                        
                         $scope.allNotificaciones = [];
                         var listaRecibida = results.data.root[0];
                         for (var i = 0; i < listaRecibida.length; i++) {
                            if (listaRecibida[i].categoria != 'M') { 
                                if (listaRecibida[i].estado != 'Oculto' || $scope.txtOcultos) {
                                     $scope.allNotificaciones.push(listaRecibida[i]);
                                }
                            }
                         }
                         if ($scope.allNotificaciones.length == 0)
                         {
                             $scope.MenjError = "No existe resultado para su consulta.";
                             $('#idMensajeInformativo').modal('show');
                             $scope.pageContentNot = [];
                             return;
                         }
                         else
                         $scope.EjecutaFiltro();
                     }                   
                 }, function (error) {
                 });
         ;     
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
            $scope.filtroEstadoNot = "Pendiente";
            $scope.filtroObligatorioNot = "";
            $scope.strict = false;
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.GridNotificacion = $filter('filter')($scope.allNotificaciones, { titulo: $scope.filtroTituloNot, prioridad: $scope.filtroPrioridadNot, estado: $scope.filtroEstadoNot, obligatorio: $scope.filtroObligatorioNot });
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
            $scope.etiTotRegistros = $scope.GridNotificacion.length;

            return;

        }
        
        $scope.selectedEstadosb = ['Aprobado','Enviado','Ingresado','Pendiente','Rechazado','Devuelto','Leido'];
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
        if ($scope.etiTotRegistros == 0 && validar != 'N') {
            $scope.MenjError = "No existe resultado para su consulta.";
            $('#idMensajeInformativo').modal('show');
           
            return;
        }
        
    };

    //Carga lista de notificaciones que tiene proveedor
    $scope.ConsultaListaNotificacionesProv = function () {

        var ruc = $scope.ruc;
        var indecePendientes = 0;
        //ruc = '0100163112001';
        $scope.myPromise =
        NotificacionService.getConsultaListaNotificacionesProv(ruc, $scope.usr).then(function (results) {

            if (results.data.success) {

                //$scope.pageContentNot = results.data.root[0];
                $scope.allNotificaciones = [];
                var listaRecibida = results.data.root[0];
                for (var i = 0; i < listaRecibida.length; i++) {
                    if (listaRecibida[i].estado != 'Oculto' && listaRecibida[i].categoria != 'M') {
                        $scope.allNotificaciones.push(listaRecibida[i]);
                    }
                }
                $scope.EjecutaFiltro("N");
              

            }
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }, function (error) {
        });
        ;

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
        var urlpdf = $scope.rutaArchivos + nomArchivo; 
        document.getElementById("pdfvisualiza").setAttribute("src", urlpdf);
        $scope.archivopdf = $sce.trustAsResourceUrl(urlpdf);      
        
        $('#visualizarpdf').modal('show');
    }
    

    //Visualizar ventana modal para seleccionar proveedores a enviar notificacion
    $scope.buscarProveedor = function () {
        
        $scope.pageContentLisProve = [];
        for (var l=0; l < $scope.ListaProveedores.length; l++)
        {
            var res = $scope.ListaProveedores[l];
            $scope.pageContentLisProve.push(
                    { codProveedor: res.codProveedor, representante: res.representante, razonSocial: res.razonSocial, isSelecProve: true });

        }
        $scope.ListaProveedores2 = $scope.ListaProveedores.slice();        
        $scope.txtBusRuc = "";
        $('#proveedoresLista').modal('show');
       
    }
    //Mostrar seccion dependiendo tipo de envio a proveedor
    $scope.tipoEnvioProveedor = function () {
        
        if ($scope.tipoEnviarProveedor != null) {
            if ($scope.tipoEnviarProveedor.codigo == "I") $scope.secconsultaProveedor = false;
            else $scope.secconsultaProveedor = true;

        }
        else
        {
            $scope.secconsultaProveedor = true;
        }
        
    }
    
    //Consultar lista de proveedores
    $scope.ConsultarProveedores = function () {
         
        var txtEnvRUC = $scope.txtBusRuc;
        if (txtEnvRUC == "") return;
        //Servicio para consultar proveedor
        NotificacionService.getConsultaLisProveedores("1",txtEnvRUC).then(function (results) {
            if (results.data.success) {
         
                $scope.pageContentLisProve = $scope.ListaProveedores.slice();
                var res = results.data.root[0];
                var existe = false;
                //validar si registro consultado ya existe en la lista
                for (var il = 0; il < $scope.pageContentLisProve.length;il++)
                {
                    if ($scope.pageContentLisProve[il].codProveedor == res[0].codProveedor)
                    {
                        existe = true;
                        break;
                    }

                }
                if (!existe)
                {
                    $scope.pageContentLisProve.push(
                    { codProveedor: res[0].codProveedor, representante: res[0].representante, razonSocial: res[0].razonSocial, isSelecProve: false });
                }
                
            }
            
        }, function (error) {
        });       
    };
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

        if ($scope.tipoEnviarProveedor == "" || $scope.tipoEnviarProveedor  == undefined)
        {
            $scope.ListaProveedores = [];
            $scope.ListaProveedores2 = [];
            $scope.txtBusRuc = "";
            $scope.Notificacion.Proveedores = "[Todos]";
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
    }
    //Borrar datos de Notificacion
    $scope.LimpiaNotificacion = function () {
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
         uploader2.clearQueue();
         $scope.pageContentLisProve = [];
         $scope.txtBusRuc = "";
         $scope.isnew = false;

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
      
        ///CARGA DE ARCHIVO 
        
    };
    //Grabar Notificacion 
   

    
    $scope.Confirmargrabar = function () {
         
        //Validar que se adjunte un documento
        if ($scope.Notificacion.ListaAdjuntos.length == 0) {
            $scope.MenjError = "Adjuntar al menos un documento";
            $('#idMensajeInformativo').modal('show');
           
            return;
        }
        
        var d = new Date();
     
        $scope.Notificacion.Comunicado = "prueba";
        //Grabar nueva notificacion
        $scope.notGrabar = new Object();
        $scope.notGrabar.CodNotificacion = $scope.codigoNot;
        $scope.notGrabar.Titulo = $scope.Notificacion.Titulo;
        $scope.notGrabar.Categoria = "C";
        $scope.notGrabar.Prioridad = $scope.Notificacion.Prioridad.codigo;
        $scope.notGrabar.Comunicado = $scope.Notificacion.ListaAdjuntos[0];
        $scope.notGrabar.Estado = "I";
        $scope.notGrabar.Obligatorio = "N";
        if ($scope.Notificacion.Obligatorio) $scope.notGrabar.Obligatorio = "S";
        $scope.notGrabar.ListaAdjuntos = $scope.Notificacion.ListaAdjuntos;
        
        $scope.notGrabar.Ruta = $scope.rutaDirectorio;
        $scope.notGrabar.Usuario = $scope.userSesion;

        if ($scope.ListaProveedores == undefined) {
            $scope.notGrabar.Tipo = "T";
        }
        else
        {
            if ($scope.ListaProveedores.length > 0)
            {
                $scope.notGrabar.Tipo = "I";
                $scope.notGrabar.ListaProveedores = $scope.ListaProveedores;
            }
                
            else $scope.notGrabar.Tipo = "T";
            
        }

        
        if ($scope.valFecVenc) $scope.notGrabar.FechaVencimiento = $scope.Notificacion.FechaVencimiento;
        
        
        $scope.myPromise =
        NotificacionService.getGrabaNotificacion($scope.notGrabar).then(function (results) {

            if (results.data.success) {
                 
                if ($scope.uploader2.queue.length == 0) {
                    $scope.MenjConfirmacion = "Notificación guardada correctamente";
                    $('#idMensajeOK').modal('show');

                    $scope.Notificacion = [];
                    $scope.Notificacion.ListaAdjuntos = [];
                    $scope.Notificacion.Lista = [];
                    $scope.ListaProveedores = [];
                    $scope.ListaProveedores2 = [];
                    uploader2.clearQueue();
                    $scope.isnew = true;

                    $scope.isdowload = true;

                    $scope.isIngresada = true;
                    $scope.myPromise =  $scope.ConsultaNotificaciones('T');
                    $scope.pageContentLisProve = [];
                    $scope.txtBusRuc = "";
                    $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');
                }
                else {
                     
                    if ($scope.uploader2.queue.length > 0) {
                        $scope.myPromise =  uploader2.uploadAll();
                    }
                }
            }
        }, function (error) {

        });
        ;
       
    }
   
    //Visualizar Notificacion 
    $scope.VisualizarGridNotificacion = function (codNotificacion, tituloNot, fecVenNot, categNot, prioNot, obliNot, rutadir, estado, observacion) {
        
        $scope.rutaDirectorio = rutadir;
        $scope.codigoNot = codNotificacion;
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + rutadir;
        $scope.uploader2.url = Ruta;
        
        //Consulta lista de proveedores y lista de archivos
        $scope.myPromise =
        NotificacionService.getConsultaLisProveedores("2", codNotificacion).then(function (results) {

            if (results.data.success) {
                var iscomunicado = false;
                var archivoComunicado = "";
                $scope.ListaProveedores = results.data.root[0];
                $scope.Notificacion.ListaAdjuntos = results.data.root[1];

                $scope.Notificacion.Lista = [];
                for (var e = 0 ; e < $scope.Notificacion.ListaAdjuntos.length; e++) {
                    if (e == 0) { iscomunicado = true; archivoComunicado = $scope.Notificacion.ListaAdjuntos[e]; }
                    else iscomunicado = false

                    $scope.Notificacion.Lista.push(
                    { name: $scope.Notificacion.ListaAdjuntos[e], iscomunicado: iscomunicado });
                }
                if ($scope.Notificacion.ListaAdjuntos.length > 1) $scope.listaAdjplus = true;
                else $scope.listaAdjplus = false;

                $scope.ListaDowload = $scope.Notificacion.ListaAdjuntos.slice();
                $scope.ListaDowload.push("PDF/" + rutadir);
                $scope.myPromise =
                NotificacionService.getDescargarArchivos($scope.ListaDowload).then(function (results) {
                    $scope.rutaArchivos = results;
                    //obliNot  estado
                    if (estado == 'Pendiente') {
                        $scope.provisPendiente = false;
                        
                        if (obliNot == 'Si')
                            $scope.provisObligatorio = true;
                        else
                            $scope.provisObligatorio = false;
                    }
                    else
                        $scope.provisPendiente = true;
                    $scope.obliNot = obliNot;
                    $scope.visualizarComunicado(archivoComunicado);
                }, function (error) {
                });
                ;
            }
        }, function (error) {
        });
        ;
        
    }

    //Descargar Adjuntos 
    $scope.DescargarGridNotificacion = function (codNotificacion, tituloNot, fecVenNot, categNot, prioNot, obliNot, rutadir, estado, observacion) {

        $scope.rutaDirectorio = rutadir;
        $scope.codigoNot = codNotificacion;
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var Ruta = serviceBase + 'api/Upload/UploadFile/?path=' + rutadir;
        $scope.uploader2.url = Ruta;

        //Consulta lista de proveedores y lista de archivos
        $scope.myPromise =
        NotificacionService.getConsultaLisProveedores("2", codNotificacion).then(function (results) {

            if (results.data.success) {
                var iscomunicado = false;
                var archivoComunicado = "";
                $scope.ListaProveedores = results.data.root[0];
                $scope.Notificacion.ListaAdjuntos = results.data.root[1];

                $scope.Notificacion.Lista = [];
                for (var e = 0 ; e < $scope.Notificacion.ListaAdjuntos.length; e++) {
                    if (e == 0) { iscomunicado = true; archivoComunicado = $scope.Notificacion.ListaAdjuntos[e]; }
                    else iscomunicado = false

                    $scope.Notificacion.Lista.push(
                    { name: $scope.Notificacion.ListaAdjuntos[e], iscomunicado: iscomunicado });
                }
                if ($scope.Notificacion.ListaAdjuntos.length > 1) $scope.listaAdjplus = true;
                else $scope.listaAdjplus = false;

                $scope.ListaDowload = $scope.Notificacion.ListaAdjuntos.slice();
                $scope.ListaDowload.push("PDF/" + rutadir);
                $scope.myPromise =
                NotificacionService.getDescargarArchivos($scope.ListaDowload).then(function (results) {
                    $scope.rutaArchivos = results;
                    //Descargar todos los archivos
                    for (var i = 0 ; i < $scope.Notificacion.ListaAdjuntos.length;i++)
                    {
                        var rutaArchivo = $scope.rutaArchivos + $scope.Notificacion.ListaAdjuntos[i];
                        var open = window.open(rutaArchivo, "_blank");
                        if (open == null || typeof (open) == 'undefined')
                            alert("Active ventanas emergentes para realizar esta accion");
                    }
                }, function (error) {
                });
                ;
            }
        }, function (error) {
        });
        ;

    }


    //Ocultar Notificacion 
    $scope.ocultarNotificacion = function (codNotificacion, tituloNot, fecVenNot, categNot, prioNot, obliNot, rutadir, estado, observacion) {
        $scope.provleido = true;
        $scope.codigoNot = codNotificacion;
        $scope.leerNotProv();

    }

    //Enviar aprobacion notificacion    
    $scope.enviaAprobacion = function (estado) {
         
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
                    $scope.MenjConfirmacion = "Notificación pendiente de aprobación.";

                }
                if (estado == 'X')
                { actEstado = 'T'; $scope.MenjConfirmacion = "Notificación  eliminada correctamente."; }
                if (estado == 'A') $scope.MenjConfirmacion = "Notificación aprobada correctamente.";
                if (estado == 'R') $scope.MenjConfirmacion = "Notificación rechazada.";
                if (estado == 'D') $scope.MenjConfirmacion = "Notificación devuelta.";
                if (estado == 'E') {
                    actEstado = 'T';
                    $scope.MenjConfirmacion = "Notificación enviada correctamente.";
                }


                $('#idMensajeOK').modal('show');
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
                $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');

            }
        }, function (error) {
        });
        ;
      

        ///CARGA DE ARCHIVO 
    };

    //agrupar notificaciones
    $scope.agrupaEnvia = function () {
        NotificacionService.getagrupaNotificacion($scope.agrupacionNotificacion[0], $scope.agrupacionNotificacion[1]).then(function (results) {
            
            $scope.MenjConfirmacion = "Notificacion agrupada correctamente";
            $('#idMensajeOK').modal('show');
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

        
            if ($(selc).is(':checked')) {
                if (longAgrupa < 2) {
                    $scope.agrupacionNotificacion.push(codigo);
                }
                else {   $(selc).attr('checked', false); }
               
            }
            else
            {
                for (var i = 0; i < longAgrupa;i++)
                {
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
     
    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            
            var i = $scope.Notificacion.Lista.length;
            if (i == 0)
            { iscomunicado = true; $scope.listaAdjplus = false; }
            else
            { iscomunicado = false; $scope.listaAdjplus = true; }
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (iscomunicado) {
                if (extension == "pdf" )
                    return true;
                else {
                    $scope.MenjError = "El tipo de archivo para la notificación debe de ser de tipo PDF.";
                    $('#idMensajeInformativo').modal('show');
                    return false;
                }
            }
            else
            {
                if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "jpg")
                    return true;
                else {
                    $scope.MenjError = "Tipo de archivo invalido.";
                    $('#idMensajeError').modal('show');
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
            if (fileSize <= 5)
                return true;
            else {
             
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
           
                return false;
            }
        }
    });
    // CALLBACKS
    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader2.onAfterAddingFile = function (fileItem) {
     
        var iscomunicado = false;
        $scope.isIngresada = true;
        
        $scope.isdowload = true;
        var i = $scope.Notificacion.Lista.length;
        if (i == 0) iscomunicado = true;
        
        $scope.Notificacion.ListaAdjuntos.push(fileItem.file.name);
        
        $scope.Notificacion.Lista.push(
            { name: fileItem.file.name, iscomunicado: iscomunicado });
    };
    uploader2.onSuccessItem = function (fileItem, response, status, headers) {
      
        if ($scope.uploader2.progress == 100)
        {
          
            $scope.notGrabar.ListaAdjuntos.push($scope.rutaDirectorio);
            $scope.myPromise =
            NotificacionService.getUploadFileSFTP($scope.notGrabar.ListaAdjuntos).then(function (results) {

                $scope.MenjConfirmacion = "Notificación guardada correctamente";
                $('#idMensajeOK').modal('show');

                $scope.Notificacion = [];
                $scope.Notificacion.ListaAdjuntos = [];
                $scope.Notificacion.Lista = [];
                $scope.ListaProveedores = [];
                $scope.ListaProveedores2 = [];
                uploader2.clearQueue();
                $scope.isnew = true;
                $scope.isdowload = true;
                $scope.ConsultaNotificaciones('T');
                $scope.pageContentLisProve = [];
                $scope.txtBusRuc = "";
                $('.nav-tabs a[href="#ListadoNotificaciones"]').tab('show');


            }, function (error) {
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
    $scope.busTodos = true;
    $scope.filtroBusquedad = true;
    $scope.ejecutafirst = false;

}
]);

app.controller('VisualizaNotificacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    $scope.Notificacion = [];
    $scope.Notificacion.ListaadjuntoProveedor = [];
    $scope.Notificacion.ListaAdjuntos = [];
    $scope.archivopdf = $sce.trustAsResourceUrl("");
    $scope.archivopdf2 = $sce.trustAsResourceUrl("");
    $scope.ruc = authService.authentication.ruc;
    $scope.usr = authService.authentication.userName;
    $scope.nombrecompleto = authService.authentication.NombreParticipante;
    $scope.indece = "0";
    $scope.verAceptar = true;
    $scope.leido = false;
    $scope.leidoAgrupado = false;
    $scope.titulo = "";
    $scope.rutalocal = "";
    $scope.totalAdjuntos = 0;
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.pone = true;
    $scope.archivoGeneral = "";
    $scope.retornoapoyo = 1;

    $scope.txtobservacion = "";
    $scope.motivosayuda = "";
    $scope.selectmotivo = "";

    $scope.motivolista = [];
    var serviceBase = ngAuthSettings.apiServiceBaseUri + 'PDF/';

    NotificacionService.getCatalogo('tbl_campania').then(function (results) {
        $scope.motivolista = results.data;

    }, function (error) {
    });

    //Obtener todas las notificaciones pendientes
    //Servicio para consultar proveedor
    $scope.levantaayuda= function(aux)
    {
        if (aux == "1")
        {
            $scope.motivosayuda="Motivo Aceptacion"
        }
        else
            $scope.motivosayuda = "Motivo Rechazo"
        $('#visualizarpdfapoyodetalle').modal('show');
    }
    $scope.cargarApoyo = function()
    {
        
        if($scope.retornoapoyo==0)
        {
            $('#visualizarpdfapoyo').modal('show');
        }else
        {
            $scope.myPromise = NotificacionService.getConsultaLisProveedores("3", $scope.ruc, $scope.usr).then(function (results) {

                if (results.data.success) {
                    $scope.Notificacion = results.data.root[0];
                    if ($scope.Notificacion.length == 0) {
                        $scope.myPromise = NotificacionService.getDescargaNotificacionG().then(function (results) {
                            
                            if (results.data.success) {
                                $scope.archivoGeneral = '../PDF/' + results.data.msgError + '?#scrollbar=0&toolbar=top&navpanes=0';
                                $scope.cargarinicial();
                            }
                            else {
                               $scope.cargarinicial();
                            }
                        }, function (error) {
                        });
                    } else {

                        $scope.presentaNot();
                    }
                }
            }, function (error) {
            });

        }

    }
   
    $scope.cargarinicial= function()
    {
	    $scope.pone = false;   
    }
    $scope.sacardiv = function()
    {
        $scope.pone = true;
    }
    
    //presentar notificacion
    $scope.presentaNot = function () {
        var index = $scope.indece;      
        //actualiza estado de notificacion a leido
        
        if ($scope.leido) {
            
            var CodNotificaActualiza = $scope.Notificacion[index - 1].codNotificacion.toString();
           
            $scope.myPromise =
            NotificacionService.getActualizaEstadoNot(CodNotificaActualiza, $scope.ruc, $scope.usr).then(function (results) {
                if (results.data.success)
                {
                        
                }
            }, function (error) {
                   
            });
        }

        if ($scope.leidoAgrupado) {
            $scope.myPromise =
                NotificacionService.getActualizaEstadoNot($scope.Notificacion[index - 1].codNotificacion, $scope.ruc, $scope.usr).then(function (results) {
                }, function (error) {
                });

            $scope.myPromise =
                NotificacionService.getActualizaEstadoNot($scope.Notificacion[index - 1].prioridad, $scope.ruc, $scope.usr).then(function (results) {
                }, function (error) {
                });
        }

        $scope.indece++;
        if (index >= $scope.Notificacion.length)
        {
            document.getElementById("pdfvisualiza").setAttribute("src", "");
            document.getElementById("pdfvisualiza2").setAttribute("src", "");
                
            $('#visualizarpdf').modal('hide');               
            $('#visualizarAgrupadopdf').modal('hide');
            $scope.cargarinicial();
            return;
        }
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
        }
        ListaDowload = [];
            
        //Consulta  lista de archivos
        $scope.myPromise =
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
                ListaDowload.push("PDF/"+ $scope.Notificacion[index].ruta);

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

    $scope.cargarApoyo();
}
]);

app.controller('InformacionController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    $scope.pageContentCho2 = [];
    $scope.SolProveedor2 = [];
    $scope.pagesCho2 = [];
    $scope.rutaArchivosRotProduc = "";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.ruc = authService.authentication.ruc;
    $scope.verListaProducto = false;
    $scope.folder = "RotacionProducto";

    $scope.ConsultaListaProductos = function (tipoLista, ver) {

        if (ver == 1)
           {
            $scope.verListaProducto = true;
        }

        $scope.pageContentCho = [];
        $scope.SolProveedor = [];
        $scope.pagesCho = [];

        var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        $scope.fechaListaProductos = "31 de Agosto del 2015";
        var str = "";
        var fechaStr = "";
        var fecha = [];


        $scope.myPromise =
        NotificacionService.getConsultaListaPrecios(tipoLista, $scope.ruc).then(function (results) {

            if (results.data.success) {

                $scope.SolProveedor = results.data.root[0];

                str = $scope.SolProveedor[0].fechaPublicacion;
                fechaStr = str.substring(0, 10);
                fecha = fechaStr.split("-");
                $scope.fechaListaProductos = fecha[2] + " de " + meses[parseInt(fecha[1])] + " del " + fecha[0];

            }

            if (tipoLista == 1) {
                $scope.iscomisariato = true;
                setTimeout(function () { $('#rbtPorFecha23').focus(); }, 100);
                setTimeout(function () { $('#proveedoresLista').focus(); }, 150);

            }
            else {
                $scope.iscomisariato = false;
                setTimeout(function () { $('#rbtPorFecha24').focus(); }, 100);

                setTimeout(function () { $('#proveedoresLista').focus(); }, 150);

            }

        }, function (error) {
        });
        ;
    };


    $scope.RotacionProductos = function (tipoLista) {
        $scope.ruc = authService.authentication.ruc;
        if (tipoLista == 1)
            $scope.iscomisariato = true;
        else
            $scope.iscomisariato = false;

        var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        $scope.fechaListaProductos = "31 de Agosto del 2015";
        var str = "";
        var fechaStr = "";
        var fecha = [];


        $scope.myPromise =
        NotificacionService.getConsultaListaPrecios(tipoLista, $scope.ruc).then(function (results) {
            if (results.data.success) {

                $scope.SolProveedor2 = results.data.root[0].slice();


            }

            setTimeout(function () { $('#rbtPorUsuario2').focus(); }, 10);
        }, function (error) {
        });
        ;

        setTimeout(function () { $('#rbtPorUsuario').focus(); }, 10);

    };

    $scope.descargaArchivo = function (nomArchivo) {
        $scope.ListaDowload = [];
        $scope.ListaDowload.push(nomArchivo);
        $scope.ListaDowload.push($scope.folder);
        $scope.myPromise =
                NotificacionService.getDescargarArchivos($scope.ListaDowload).then(function (results) {
                    $scope.rutaArchivosRotProduc = results  + nomArchivo;
                    var open = window.open($scope.rutaArchivosRotProduc, "_blank");
                }, function (error) {
                });
        ;

    };

   $scope.verListaLocales = function () {

        $scope.verListaProducto = false;
    };

}
]);

app.controller('ListaPrecioController', ['$scope', 'ngAuthSettings', 'NotificacionService', '$sce', 'authService', function ($scope, ngAuthSettings, NotificacionService, $sce, authService) {
    $scope.pageContentCho2 = [];
    $scope.SolProveedor2 = [];
    $scope.pagesCho2 = [];
    $scope.rutaArchivosRotProduc = "../PDF/";
    $scope.message = 'Por Favor Espere...';
    $scope.myPromise = null;
    $scope.ruc = authService.authentication.ruc;
    $scope.tLista = 0;
    //Registro actual
    $scope.reg = 1;
    //Total Registro por pagina
    $scope.regPag = 100;
    //Total de registro
    $scope.totReg = 0;

    $scope.BloqFirst = true;
    $scope.BloqBack = true;
    $scope.BloqNext = true;
    $scope.BloqLast = true;

    $scope.guardarDatosProv = function () {

        NotificacionService.getRegistraNuevoProv($scope.Nombre, $scope.Email, $scope.Telefono, $scope.Celular, $scope.ObservacionIng).then(function (results) {

            if (results.data.success) {
                $scope.MenjError = "Datos registrados correctamente.";
                $('#idMensajeOk').modal('show');
                $scope.Nombre = "";
                $scope.Email = "";
                $scope.Telefono = "";
                $scope.Celular = "";
                $scope.ObservacionIng = "";

            }
            else {
                $scope.MenjError = "Error al realizar transacción.";
                $('#idMensajeError').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Error al realizar transacción.";
            $('#idMensajeError').modal('show');
        });
    };


    //Pagineo para lista de precios
    $scope.First = function () {

        $scope.reg = 1;
        $scope.regF = $scope.regPag;

        $scope.myPromise =
        NotificacionService.getConsultaListaPreciosg($scope.tLista, $scope.ruc, $scope.reg, $scope.regF).then(function (results) {

           if (results.data.success) {
               $scope.BloqFirst = true;
               $scope.BloqBack = true;
               $scope.BloqNext = false;
               $scope.BloqLast = false;
               $scope.pageContentCho = results.data.root[0];
               if ($scope.pageContentCho.length == 0) return;
               $scope.totReg = results.data.msgError;
           }

       }, function (error) {
       });
    }

    $scope.Last = function () {

        var div = $scope.totReg / $scope.regPag;
        $scope.reg = Math.floor(div) * 100 + 1
        $scope.regF = $scope.totReg;

        if ($scope.reg > $scope.regF)
        {
            $scope.reg = $scope.regF - $scope.regPag +1;

        }
        $scope.BloqNext = true;
        $scope.BloqLast = true;
        $scope.BloqFirst = false;
        $scope.BloqBack = false;
        $scope.myPromise =
       NotificacionService.getConsultaListaPreciosg($scope.tLista, $scope.ruc, $scope.reg, $scope.regF).then(function (results) {

           if (results.data.success) {

               $scope.pageContentCho = results.data.root[0];
               if ($scope.pageContentCho.length == 0) return;

               $scope.totReg = results.data.msgError;

           }

       }, function (error) {
       });
    }

    $scope.Next = function () {
        
        $scope.reg = $scope.regF + 1;
        $scope.regF = $scope.regF + $scope.regPag ;
       
        $scope.myPromise =
       NotificacionService.getConsultaListaPreciosg($scope.tLista, $scope.ruc, $scope.reg, $scope.regF).then(function (results) {

           if (results.data.success) {

               if ($scope.reg != 1) {
                   $scope.BloqFirst = false;
                   $scope.BloqBack = false;
               }
              
               $scope.pageContentCho = results.data.root[0];
               if ($scope.pageContentCho.length == 0) return;
              
               $scope.totReg = results.data.msgError;
               if ($scope.regF >= $scope.totReg)
               {
                   $scope.BloqNext = true;
                   $scope.BloqLast = true;
               }

           }
          
       }, function (error) {
       });
        
    }

    $scope.Back = function () {
        $scope.reg = $scope.reg - $scope.regPag ;
        $scope.regF = $scope.reg + $scope.regPag -1;
        $scope.BloqNext = false;
        $scope.BloqLast = false;
        $scope.myPromise =
        NotificacionService.getConsultaListaPreciosg($scope.tLista, $scope.ruc, $scope.reg, $scope.regF).then(function (results) {

            if (results.data.success) {

                if ($scope.reg == 1) {
                   $scope.BloqFirst = true;
                   $scope.BloqBack = true;
               }

                $scope.pageContentCho = results.data.root[0];
                if ($scope.pageContentCho.length == 0) return;
                $scope.totReg = results.data.msgError;

            }

        }, function (error) {
        });
        
    }
    $scope.ConsultaListaProductos = function (tipoLista, ver) {

        $scope.pageContentCho = [];
        $scope.SolProveedor = [];
        $scope.pagesCho = [];
        $scope.tLista = tipoLista;
        $scope.regF = $scope.reg + $scope.regPag - 1;

        var meses = new Array("", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre");
        $scope.fechaListaProductos = "31 de Agosto del 2015";
        var str = "";
        var fechaStr = "";
        var fecha = [];


        $scope.myPromise =
        NotificacionService.getConsultaListaPreciosg(tipoLista, $scope.ruc, $scope.reg, $scope.regF).then(function (results) {

            if (results.data.success) {

                $scope.SolProveedor = results.data.root[0];
                $scope.pageContentCho = results.data.root[0];
                

                if ($scope.SolProveedor.length == 0) return;

                $scope.totReg = results.data.msgError;
                if ($scope.totReg > $scope.regPag)
                {
                    $scope.BloqNext = false;
                    $scope.BloqLast = false;
                }
                str = $scope.SolProveedor[0].fechaPublicacion;
                fechaStr = str.substring(0, 10);
                fecha = fechaStr.split("-");
                $scope.fechaListaProductos = fecha[2] + " de " + meses[parseInt(fecha[1])] + " del " + fecha[0];

            }

        }, function (error) {
        });
        ;
    }
   
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

app.controller('NotPendienteController', ['$scope', 'ngAuthSettings', 'NotificacionService', 'FileUploader', '$sce', 'authService', 'localStorageService', function ($scope, ngAuthSettings, NotificacionService, FileUploader, $sce, authService, localStorageService) {

    $scope.ruc = authService.authentication.ruc;
    $scope.pCodSAP = authService.authentication.CodSAP;

    $scope.ConsultaListaPedidosProv = function () {
        $scope.PedidosPendientes = 0;
    }

    $scope.SelecionarGridNotificacionProv = function (codNotificacion, tituloNot, fecVenNot, categNot, prioNot, obliNot, rutadir, estado, observacion) {
         
        localStorageService.remove('NotPend');
        var someSessionObj = {

            'codnot': codNotificacion,
            'titulo': tituloNot,
            'fecha': fecVenNot,
            'categoria': categNot,
            'prioridad': prioNot,
            'obligatorio': obliNot,
            'ruta': rutadir,
            'estado': estado,
            'observacion': observacion
        };

        localStorageService.set('NotPend', someSessionObj);

    }

    //Carga lista de notificaciones que tiene proveedor
    $scope.ConsultaListaNotificacionesProv = function () {
         
        $scope.usr = authService.authentication.userName;
        var ruc = $scope.ruc;
        var indecePendientes = 0;

        NotificacionService.getConsultaListaNotificacionesProv(ruc, $scope.usr).then(function (results) {

            if (results.data.success) {
                $scope.pageContentNot = results.data.root[0];
                $scope.ListadoNotificacionesPendientes = [];
                $scope.NotificacionesPendientes = 0;
                indecePendientes = 0;
                for (var i = 0; i < $scope.pageContentNot.length; i++) {
                    if ($scope.pageContentNot[i].estado == "Pendiente") {
                        $scope.ListadoNotificacionesPendientes[indecePendientes] = $scope.pageContentNot[i];
                        indecePendientes++;
                    }

                }
                $scope.NotificacionesPendientes = $scope.ListadoNotificacionesPendientes.length;

            }
        }, function (error) {
        });

    };

    $scope.verificaContratoMsn = function (titulo) {
        cadena = "Contrato";
        patt = new RegExp(cadena);
        var n = titulo.search(patt);

        if (n > 0)
            return 2;
        else
            return 1;
    }
    
    $scope.IdMsgPagos = 0;

    $('#Msg').on('show.bs.modal', function (e) {
        var $modal = $(this),
            Id = e.relatedTarget.id,
            Titulo = e.relatedTarget.title,
            Mensaje = e.relatedTarget.value;

        $scope.IdMsgPagos = Id;

        $modal.find('.edit-title').html(Id + ' - '+ Titulo);
        $modal.find('.edit-content').html('<p style="color: #006865">' + Mensaje + '</p>');
    });

    $('#btnMsgOK').on('click', function (e) {
        var indicePendientes = 0;
        NotificacionService.actualizaMensajesPagosProv($scope.ruc, $scope.IdMsgPagos,'').then(function (results) {

            if (results.data.success) {
                $scope.ConsultaListaNotificacionesProv()
            }
        }, function (error) {
        });
    });

    
    $scope.IdMsgPagos = 0;

    $('#Msg').on('show.bs.modal', function (e) {
        var $modal = $(this),
            Id = e.relatedTarget.id,
            Titulo = e.relatedTarget.title,
            Mensaje = e.relatedTarget.value;

        $scope.IdMsgPagos = Id;

        $modal.find('.edit-title').html(Id + ' - '+ Titulo);
        $modal.find('.edit-content').html('<p style="color: #006865">' + Mensaje + '</p>');
    });

    $('#btnMsgOK').on('click', function (e) {
        var indicePendientes = 0;
        NotificacionService.actualizaMensajesPagosProv($scope.ruc, $scope.IdMsgPagos,'').then(function (results) {

            if (results.data.success) {
                $scope.ConsultaListaNotificacionesProv()
            }
        }, function (error) {
        });
    });

}

    

]);

