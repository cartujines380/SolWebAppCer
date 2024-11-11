'use strict';
app.controller('ConsultaProveedorController', ['$scope', '$location', '$http', 'ModificacionProveedor', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', function ($scope, $location, $http, ModificacionProveedor, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService) {

    
    $scope.tipoidentificacionList = [];
    $scope.ListTipoIdentificacion = [];
    $scope.ListDocumentoAdjunto = [];
    $scope.ListSectorComercial = [];
    $scope.ListTipoProveedor = [];
    $scope.ListMotivoRechazoProveedor = [];
    $scope.ListSociedad = [];
    $scope.Sociedad = [];
    $scope.ListLinea = [];
    $scope.Linea = [];
    $scope.accion = "";
    $scope.hidecompras = true;
    $scope.hidegerentecompras = true;
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

  
    GeneralService.getCatalogo('tbl_Pais').then(function (results) {
        $scope.ListPais = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_Region').then(function (results) {
        $scope.ListRegion = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_Ciudad').then(function (results) {
        $scope.ListCiudad = results.data;
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
        //if (localStorageService.get('SolProv') && localStorageService.get('SolProv').tipoidentificacion != '')
        //    _tipoide_codigo = localStorageService.get('SolProv').tipoidentificacion;

        for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
            if ($scope.ListTipoIdentificacion[index].codigo === _tipoide_codigo) {
                $scope.Ingreso.tipoidentificacion = $scope.ListTipoIdentificacion[index];

                break;
            }
        }
    }, function (error) {
    });

    $scope.$watch('Ingreso.Pais', function () {

        $scope.ListRegionTemp = [];
        if ($scope.Ingreso.Pais != '' && angular.isUndefined($scope.Ingreso.Pais) != true) {
            $scope.ListRegionTemp = $filter('filter')($scope.ListRegion,
                                              { descAlterno: $scope.Ingreso.Pais.codigo });
        }
    });

    $scope.$watch('Ingreso.Region', function () {

        $scope.ListCiudadTemp = [];
        if ($scope.Ingreso.Region != '' && angular.isUndefined($scope.Ingreso.Region) != true) {
            $scope.ListCiudadTemp = $filter('filter')($scope.ListCiudad,
                                              { descAlterno: $scope.Ingreso.Region.codigo });
        }
    });
   


    //Recuperar datos de cache
  
    $scope.codSapProveedor = localStorageService.get('CodProveedor') === null ? "0" : localStorageService.get('CodProveedor');
    localStorageService.set('CodProveedor', "0");
    if ($scope.codSapProveedor == "0") {
       
        window.location = 'frmBandejaConsAdmin';
        return;
    }


   
    $scope.CargarProveedor = function () {
          
        
        
            
        //$scope.codSapProveedor = '102911';
       
        $scope.myPromise = ModificacionProveedor.getProveedor($scope.codSapProveedor).then(function (response) {
           
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
                    //$scope.p_SolProveedor.ViaPago = response.data[0].viaPago;
                    //$scope.p_SolProveedor.DescViaPago = response.data[0].descViaPago;
                    $scope.p_SolProveedor.CondicionPago = response.data[0].condicionPago;
                    $scope.p_SolProveedor.DescCondicionPago = response.data[0].descCondicionPago;

                   

                    if (response.data[0].genDocElec == "X") {
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
                    $scope.Estado = $scope.p_SolProveedor.Estado;
                    $scope.p_SolProveedor.LineaNegocio = response.data[0].lineaNegocio;


                    var index = 0;
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

                    //if ($scope.p_SolProveedor.ViaPago != '') {
                    //    for (index = 0; index < $scope.ListViaPago.length; index++) {
                    //        if ($scope.ListViaPago[index].codigo == $scope.p_SolProveedor.ViaPago) {
                    //            $scope.Ingreso.ViaPago = $scope.ListViaPago[index];
                    //            break;
                    //        }
                    //    }
                    //}


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
                }
            },
                function (err) {
                    $scope.MenjError = err.error_description;
                });

            //carga lista Direccion
            $scope.myPromise = null;
            $scope.myPromise = ModificacionProveedor.getProveedorDireccionList($scope.codSapProveedor).then(function (response) {
           
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
                    $scope.ListRegionTemp = [];
                    if ($scope.Ingreso.Pais != '' && angular.isUndefined($scope.Ingreso.Pais) != true) {
                        $scope.ListRegionTemp = $filter('filter')($scope.ListRegion,
                                                          { descAlterno: $scope.Ingreso.Pais.codigo });
                    }

                    if ($scope.p_SolProvDireccion.Provincia != '') {
                        for (index = 0; index < $scope.ListRegion.length; index++) {
                            if ($scope.ListRegion[index].codigo == $scope.p_SolProvDireccion.Provincia) {
                                $scope.Ingreso.Region = $scope.ListRegion[index];
                                break;
                            }
                        }
                    }

                    if ($scope.p_SolProvDireccion.Ciudad != '') {
                        
                        for (index = 0; index < $scope.ListCiudad.length; index++) {                            
                            var ciudadArray = $scope.p_SolProvDireccion.Ciudad.split("-");
                            if (parseInt($scope.ListCiudad[index].codigo) == parseInt(ciudadArray[0])) {
                                $scope.Ingreso.Ciudad = $scope.ListCiudad[index];
                                break;
                            }
                        }
                    }
                }
            },

            function (err) {
                $scope.MenjError = err.error_description;
            });

            
          
        

 
       

    };

 

    if ($scope.SolLinea.length <= 0) {
        $scope.hidelineapertenece = true;

    }
    //$scope.backdrop = true;

    $scope.Ingreso = {
        tipoidentificacion: "",
        SectorComercial: "",
        TipoProveedor: "",
        MotivoRechazoProveedor: "",
        Sociedad: "",
        Linea: "",
        GrupoTesoreria: "",
        CuentaAsociada: "",
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
        beneficiariobanco: ""
    };



    $scope.rutaDirectorio;
    $scope.codigoPre = 0;
  

    $scope.identificacion = '';
    $scope.tipoidentificacion = '';
    $scope.IdSolicitud = '';
    $scope.Estado = '';
    $scope.tiposolicitud = 'AT';
    $scope.indpage = 'ING';
    $scope.maxidentificacion = 0;
    $scope.maxbanco = 0;
    $scope.isDisabledApr = false;
    $scope.isDisabledNomBanco = false;
    $scope.mensajelogin = '';
    $scope.maxidramo = 0;
    $scope.maxcontacto = 0;

    

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var Ruta = serviceBase + 'api/Upload/UploadFile/?path=prueba';
    ///CARGA DE ARCHIVO 
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: Ruta
    });


   


    GeneralService.getCatalogo('tbl_beneficiariobanco').then(function (results) {
        $scope.Listbeneficiariobanco = results.data;
    }, function (error) {
    });



    GeneralService.getCatalogo('tbl_FuncionContacto').then(function (results) {
        $scope.ListFuncionContacto = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_DepartaContacto').then(function (results) {
        $scope.ListDepartaContacto = results.data;
    }, function (error) {
    });

    $scope.myPromise = null;
    $scope.myPromise = ModificacionProveedor.getBancoList("EC").then(function (results) {
      
        $scope.ListBanco = results.data;
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




    GeneralService.getCatalogo('tbl_DocuGeneralAdjunto').then(function (results) {
        //$scope.ListDocumentoAdjunto = results.data;

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
        PreFijo: '', DepCliente: '',  RepLegal: true,
        Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
        NotElectronica: false, Cargos : [],
        NotTransBancaria: false, id: 0, 
    }

    $scope.p_SolProvCargos = {
       Departamento: '', Funcion: '',
        DescDepartamento: '', DescFuncion: ''
    }


    $scope.p_SolProvBanco = {
        id: 0,
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



  


    




   


  
    $scope.CargarProveedor();
}]);


