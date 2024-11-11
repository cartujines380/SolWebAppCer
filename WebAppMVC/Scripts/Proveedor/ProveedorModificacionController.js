'use strict';
app.controller('ProveedorModificacionController', ['$scope', '$location', '$http', 'ModificacionProveedor', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', function ($scope, $location, $http, ModificacionProveedor, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService) {

    $scope.codSapProveedor = authService.authentication.CodSAP;
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
    $scope.ListaLineaNegocio = [];
    $scope.LineasNegocios = {
        codigo: "",
        detalle: "",
        chekeado: "",
        principal: ""
    };
    $scope.GrupoCompraFilt = [];
    $scope.GrupoCompra = [];
    //integracion
    $scope.ListTipoActividad = [];

    GeneralService.getCatalogo('tbl_ProvGrupoCompras').then(function (results) {
        $scope.GrupoCompra = results.data;

    }, function (error) {

    });

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

        for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
            if ($scope.ListTipoIdentificacion[index].codigo === _tipoide_codigo) {
                $scope.Ingreso.tipoidentificacion = $scope.ListTipoIdentificacion[index];

                break;
            }
        }
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_tipoActividad').then(function (results) {
        $scope.ListTipoActividad = results.data;

        var index = 0;

        for (index = 0; index < $scope.ListTipoActividad.length; index++) {
                $scope.Ingreso.tipoActividad = $scope.ListTipoActividad[index];    
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
        if ($scope.Ingreso.Region != '' && angular.isUndefined($scope.Ingreso.Region) != true) {
            $scope.ListCiudadTemp = $filter('filter')($scope.ListCiudad,
                                              { descAlterno: $scope.Ingreso.Region.codigo });
        }
    });
    $scope.$watch('p_SolProvContacto.Nombre1', function (val) {
        $scope.p_SolProvContacto.Nombre1 = $filter('uppercase')(val);
    }, true);
    $scope.$watch('p_SolProvContacto.Nombre2', function (val) {
        $scope.p_SolProvContacto.Nombre2 = $filter('uppercase')(val);
    }, true);

    $scope.$watch('p_SolProvContacto.Apellido1', function (val) {
        $scope.p_SolProvContacto.Apellido1 = $filter('uppercase')(val);
    }, true);

    $scope.$watch('p_SolProvContacto.Apellido2', function (val) {
        $scope.p_SolProvContacto.Apellido2 = $filter('uppercase')(val);
    }, true);
    $scope.$watch('p_SolProvContacto.EMAIL', function (val) {
        $scope.p_SolProvContacto.EMAIL = $filter('uppercase')(val);
    }, true);

    $scope.CargarProveedor = function () {
        if ($scope.p_SolProveedor.IdSolicitud == "" || $scope.p_SolProveedor.IdSolicitud == "0" || $scope.Estado == "AP")
        {
            $scope.myPromise = null;
        
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
                    $scope.p_SolProveedor.GrupoCompra = response.data[0].grupoCompra;

                    var index = 0;
                    debugger;
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


            //Carga lista de lineas de negocios
            //Carga lista de lineas de negocios
            $scope.myPromise = ModificacionProveedor.getConsLienasNeg($scope.codSapProveedor).then(function (response) {

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

            //carga lista Contacto
            $scope.myPromise = null;


        }

 
       

    };

 

    if ($scope.SolLinea.length <= 0) {
        $scope.hidelineapertenece = true;

    }

    $scope.Ingreso = {
        tipoActividad: "",
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

    if (localStorageService.get('SolProv')) {
        $scope.identificacion = localStorageService.get('SolProv').identificacion;
        $scope.tipoidentificacion = localStorageService.get('SolProv').tipoidentificacion;
        $scope.IdSolicitud = localStorageService.get('SolProv').IdSolicitud;
        $scope.Estado = localStorageService.get('SolProv').Estado;
        $scope.tiposolicitud = localStorageService.get('SolProv').tiposolicitud;
        $scope.indpage = localStorageService.get('SolProv').indpage;
        $scope.mensajelogin = localStorageService.get('SolProv').MenjError;

    }

    if ($scope.indpage == 'ING' || $scope.indpage == '') {
        $scope.hidedatosmaestrocom = true;
    }


    if ($scope.mensajelogin != '' && $scope.indpage == 'ING') {
     

        if ($scope.Estado != 'DP' && $scope.Estado != 'AP') {
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


    $scope.confirmaenviar = function () {
       

        if ($scope.indpage == 'ING') {
            $scope.MenjConfirmacion = '¿Está seguro de enviar la información?';
        }

        if ($scope.indpage == 'APR') {
            $scope.MenjConfirmacion = '¿Está seguro de rechazar la información?';
        }

        $('#idMensajeConfirmacion').modal('show');
    }

    $scope.quitarAdjunto = function (nomArchivo) {


        var listaArchivos = $scope.uploader2.queue;
        for (var i = 0 ; i < listaArchivos.length; i++) {
            var nomArchivo2 = $scope.uploader2.queue[i]._file.name;
            if (nomArchivo.NomArchivo == nomArchivo2) {
                $scope.uploader2.queue[i].remove();
            }
        }

        for (var i = 0 ; i < $scope.SolDocAdjunto.length; i++) {
            if ($scope.SolDocAdjunto[i] == nomArchivo) break;

        }
        $scope.SolDocAdjunto.splice(i, 1);



    };

    $scope.enviarobservacion = function (accion, valido) {


        $scope.accion = accion;


        $('#modal-form-observacion').modal('show');
    }


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

 

    $scope.$watch('Ingreso.Paisbanco', function () {
      
        $scope.ListRegionbancoTemp = [];
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

    $scope.Limpiasecuenciaarchivo();


    // FILTERS
    uploader2.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
           
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension == "pdf" || extension == "doc" || extension == "docx" || extension == "rtf" || extension == "txt")
                return true;
            else {
                $scope.MenjError = "Debe seleccionar archivos con formato pdf/doc/docs or rtf.";
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
            if (fileSize <= $scope.Ingreso.MaxMegaProveedor)
                return true;
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
   
            if (this.queue.length < $scope.Ingreso.MaxCantAdjProveedor)
                return true;
            else {
                $scope.MenjError = "has execido la cantidad de archivos permitidos ";
                $('#idMensajeError').modal('show');

                $('#adjuntoarchivo').val('');
                return false;
            }
        }
    });

    uploader2.onWhenAddingFileFailed = function (item, filter, options) {
   

        console.info('onWhenAddingFileFailed', item, filter, options);
    };
    uploader2.onAfterAddingFile = function (fileItem) {

        $scope.p_SolDocAdjunto.NomArchivo = fileItem.file.name;
        $scope.p_SolDocAdjunto.Archivo = $scope.rutaDirectorio;
        $scope.p_SolDocAdjunto.FechaCarga = new Date();

    };
    uploader2.onSuccessItem = function (fileItem, response, status, headers) {

        if ($scope.uploader2.progress == 100) {
            $('#adjuntoarchivo').val(null).clone(true);
            $('#adjuntoarchivo').reset
            $scope.uploader2.progress = 0;
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
        TipoActividad: ''
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
        NotTransBancaria: false, id: 0
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


    $scope.cargasolicitud = function (IdSolicitud) {
    
            if (IdSolicitud != '' && $scope.Estado!="AP") {
            $scope.myPromise = null;
            $scope.myPromise = ModificacionProveedor.getSolProveedorList($scope.IdSolicitud).then(function (response) {
             
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
                    $scope.p_SolProveedor.CondicionPago = response.data[0].condicionPago;
                    $scope.p_SolProveedor.DescCondicionPago = response.data[0].descCondicionPago;

                  
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
                    $scope.Estado = $scope.p_SolProveedor.Estado;
                    $scope.p_SolProveedor.LineaNegocio = response.data[0].lineaNegocio;
                    $scope.p_SolProveedor.GrupoCompra = response.data[0].grupoCompra;

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
                            if ($scope.ListIdioma[index].descAlterno == $scope.p_SolProveedor.Idioma) {
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
            $scope.myPromise = ModificacionProveedor.getSolProvDireccionList($scope.IdSolicitud).then(function (response) {
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
                            if ($scope.ListCiudad[index].codigo == $scope.p_SolProvDireccion.Ciudad) {
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

            //carga lista Contacto
            $scope.myPromise = null;
            $scope.myPromise = ModificacionProveedor.getSolProvContactoList($scope.IdSolicitud).then(function (response) {
           
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
            $scope.myPromise = ModificacionProveedor.getSolDocAdjuntoList($scope.IdSolicitud).then(function (response) {
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


            //carga lista Ramo
            $scope.myPromise = null;
            $scope.myPromise = ModificacionProveedor.getRamoList($scope.IdSolicitud).then(function (response) {
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
            $scope.myPromise = ModificacionProveedor.getSolZonaList($scope.IdSolicitud).then(function (response) {
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
            $scope.myPromise = ModificacionProveedor.getViaList($scope.IdSolicitud).then(function (response) {
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
            $scope.myPromise = ModificacionProveedor.getSolProvHistEstadoList($scope.IdSolicitud).then(function (response) {
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
            $scope.myPromise = ModificacionProveedor.getSolProvBancoList($scope.IdSolicitud).then(function (response) {
                
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

    if ($scope.IdSolicitud != '') {
        $scope.cargasolicitud($scope.IdSolicitud);

    }

    else {

        $scope.p_SolProveedor.EMAILCorp = authService.authentication.userName;
        if ($scope.tiposolicitud == 'AT') {

            //Cargar la Información de la bapi para Actualizacion de Datos del Proveedor
        }

    }




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
            solpath = content.IdSolicitud;
        }

        $scope.myPromise = null;
        $scope.myPromise = ModificacionProveedor.getrutaarchivos(solpath, content.NomArchivo).then(function (response) {
            
            window.open(response.data);
            return;

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
        $('#idMensajeError').modal('show'); fv
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

        //modificacion de Ecetre Solo Ruc


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
            id: 0,
            IdSolBanco: '', IdSolicitud: '', Extrangera: true, CodSapBanco: '',
            NomBanco: '', Pais: '', DescPAis: '', TipoCuenta: '',
            DesCuenta: '', NumeroCuenta: '', TitularCuenta: '', ReprCuenta: '',
            CodSwift: '', CodBENINT: '', CodABA: '', Principal: true,
            Estado: true, Provincia: '', DescProvincia: '', DirBancoExtranjero: '',
            BancoExtranjero: ''
        }




    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

    ///adiona Banco
    $scope.adicionabanco = function () {
        
        if ($scope.p_SolProvBanco != "") {


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


            if ($scope.Ingreso.TipoCuenta != '') {
                $scope.p_SolProvBanco.TipoCuenta = $scope.Ingreso.TipoCuenta.codigo;
                $scope.p_SolProvBanco.DesCuenta = $scope.Ingreso.TipoCuenta.detalle;
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



            if ($scope.isDisabledbanco == true) {//edita
                var index = 0
                for (index = 0; index < $scope.SolProvBanco.length; index++) {
                    if ($scope.SolProvBanco[index].id == $scope.p_SolProvBanco.id) {

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


                        break;
                    }
                }
            }
            else {


                $scope.p_SolProvBanco.id = $scope.maxbanco + 1;
                $scope.maxbanco = $scope.maxbanco + 1;
                $scope.SolProvBanco.push($scope.p_SolProvBanco);

                
            }


            $scope.MenjError = "Banco ingresado correctamente "
            $('#idMensajeOk').modal('show');

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
     
        if ($scope.p_SolDocAdjunto != "") {
            if ($scope.p_SolDocAdjunto.Archivo == "") {
                $scope.MenjError = "Seleccione el Archivo... "
                $('#idMensajeError').modal('show');
                return;
            }

            $scope.p_SolDocAdjunto.CodDocumento = documento.codigo;
            $scope.p_SolDocAdjunto.DescDocumento = documento.detalle;
            
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

            $scope.p_SolDocAdjunto = {};

            $scope.MenjError = "Adjunto ingresado correctamente "
            $('#idMensajeOk').modal('show');
            var idDoc = "#" + documento.codigo;
            var fileupload = $(idDoc);
            fileupload.replaceWith(fileupload.clone(true));
            documento.ver = false
            $('#modal-form-adjunto').modal('hide');
            $scope.limpiaadjunto();
        }
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };

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
            $scope.Grabaradjunto = "Addicionar";
        }

        $('#modal-form-adjunto').modal('show');
        return false;
    }



    $scope.limpiacontacto = function () {
       


        $scope.p_SolProvContacto = {

            IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
            Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
            PreFijo: '', DepCliente: '', Departamento: '', Funcion: '', RepLegal: true,
            Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
            DescDepartamento: '', DescFuncion: '', NotElectronica: false,
            NotTransBancaria: false, id: 0
        }

    }
    , function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
        $('#idMensajeError').modal('show');
    };
    ///adiona conacto
    $scope.adicionacontacto = function () {
     

        if ($scope.isDisabledApr) {
            $scope.MenjError = "No Puede Modificar el Conatcto... ";
            $('#idMensajeError').modal('show');
            return;
        }


        if ($scope.p_SolProvContacto != "") {

            if ($scope.Ingreso.Contactipoidentificacion != '') {
                $scope.p_SolProvContacto.TipoIdentificacion = $scope.Ingreso.Contactipoidentificacion.codigo;
            }

            if ($scope.Ingreso.Tratamiento != '') {
                $scope.p_SolProvContacto.PreFijo = $scope.Ingreso.Tratamiento.codigo;
            }

            if ($scope.Ingreso.DepartaContacto != '') {
                $scope.p_SolProvContacto.Departamento = $scope.Ingreso.DepartaContacto.codigo;
                $scope.p_SolProvContacto.DescDepartamento = $scope.Ingreso.DepartaContacto.detalle;
            }

            if ($scope.Ingreso.FuncionContacto != '') {
                $scope.p_SolProvContacto.Funcion = $scope.Ingreso.FuncionContacto.codigo;
                $scope.p_SolProvContacto.DescFuncion = $scope.Ingreso.FuncionContacto.detalle;
            }

            if ($scope.validorCedulaguradr($scope.p_SolProvContacto.Identificacion) != true) return true;

            if ($scope.isDisabledContact == true) {//edita
                var index = 0
                for (index = 0; index < $scope.SolProvContacto.length; index++) {



                    if ($scope.SolProvContacto[index].id == $scope.p_SolProvContacto.id) {

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
                        break;
                    }
                }


            }
            else {//agrega
                for (index = 0; index < $scope.SolProvContacto.length; index++) {
                    if ($scope.SolProvContacto[index].TipoIdentificacion == $scope.Ingreso.Contactipoidentificacion.codigo &&
                        $scope.SolProvContacto[index].Identificacion == $scope.p_SolProvContacto.Identificacion) {

                        $scope.MenjError = "Ya existe un contacto con esta identifiación... "
                        $('#idMensajeError').modal('show');
                        return;

                    }
                }

                $scope.maxcontacto = $scope.maxcontacto + 1;
                $scope.p_SolProvContacto.id = $scope.maxcontacto;

                if ($scope.SolProvContacto != "") {
                    $scope.SolProvContacto.push($scope.p_SolProvContacto);
                }
                else {

                    $scope.SolProvContacto = [$scope.p_SolProvContacto];
                }
            }

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

            for (index = 0; index < $scope.ListFuncionContacto.length; index++) {
                if ($scope.ListFuncionContacto[index].codigo === content.Funcion) {
                    $scope.Ingreso.FuncionContacto = $scope.ListFuncionContacto[index];

                    break;
                }
            }

            for (index = 0; index < $scope.ListTratamiento.length; index++) {
                if ($scope.ListTratamiento[index].codigo === content.PreFijo) {
                    $scope.Ingreso.Tratamiento = $scope.ListTratamiento[index];

                    break;
                }
            }

            $scope.Grabarcontacto = "Modificar";
        }
        else {
            $scope.limpiacontacto();
            $scope.Grabarcontacto = "Addicionar";
        }

        $('#modal-form').modal('show');
        return false;
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
            $scope.p_SolProvBanco.NomBanco = content.nomBanco;
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



            $scope.Grabarbanco = "Modificar";
        }
        else {
            $scope.limpiabanco();
            $scope.Grabarbanco = "Addicionar";
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

                    $scope.cargasolicitud($scope.IdSolicitud);
                }
                else {
                    window.location = "/Notificacion/frmVisualizaNotificaciones";
                }

            }

            if ($scope.indpage == 'APR' || $scope.indpage == 'APG' || $scope.indpage == 'APM') {
                window.location = "/Proveedor/frmBandejaSolicitud";
            }
        }

    },
     function (error) {
       
         $scope.MenjError = "Se ha producido el siguiente error: " + err.error_description;
         $('#idMensajeError').modal('show');
     };


    ///grabar solicitud proveedor
    $scope.SaveProveedorList = function (bandera, valido) {
        $scope.salir = 0;
        $scope.bandera = bandera;
               
        $scope.SolProveedor = [];
        $scope.SolProvDireccion = [];

        if ($scope.Ingreso.GrupoCompra != '') {
            $scope.p_SolProveedor.GrupoCompra = $scope.Ingreso.GrupoCompra.codigo;
        }

        if ($scope.Ingreso.Pais != '') {
            $scope.p_SolProvDireccion.Pais = $scope.Ingreso.Pais.codigo;
        }

        if ($scope.Ingreso.Region != '') {
            $scope.p_SolProvDireccion.Provincia = $scope.Ingreso.Region.codigo;
        }

        if ($scope.Ingreso.Ciudad != '' && $scope.Ingreso.Ciudad != null) {
            $scope.p_SolProvDireccion.Ciudad = $scope.Ingreso.Ciudad.codigo;
        }
        if ($scope.Ingreso.SectorComercial != '') {
            $scope.p_SolProveedor.SectorComercial = $scope.Ingreso.SectorComercial.codigo;
        }

        if ($scope.Ingreso.TipoProveedor != '') {
            $scope.p_SolProveedor.TipoProveedor = $scope.Ingreso.TipoProveedor.codigo;
        }

        if ($scope.Ingreso.Idioma != '') {
            $scope.p_SolProveedor.Idioma = $scope.Ingreso.Idioma.codigo;
        }

        if ($scope.Ingreso.TipoProveedor != '') {
            $scope.p_SolProveedor.TipoProveedor = $scope.Ingreso.TipoProveedor.codigo;
        }

        if ($scope.Ingreso.ClaseImpuesto != '') {
            $scope.p_SolProveedor.ClaseContribuyente = $scope.Ingreso.ClaseImpuesto.codigo;
        }

        if ($scope.Ingreso.LineaNegocio != '') {
            $scope.p_SolProveedor.LineaNegocio = $scope.Ingreso.LineaNegocio.codigo;
        }

        if ($scope.p_SolProveedor.CodGrupoProveedor != '' && $scope.p_SolProveedor.CodGrupoProveedor !=null) {

            if ($scope.p_SolProveedor.CodGrupoProveedor.length < 10) {
                $scope.MenjError = "RUC Proveedor Padre es Inconrecto ";
                $('#idMensajeError').modal('show');

                return;
            }

            if ($scope.validorCedulaguradr($scope.p_SolProveedor.CodGrupoProveedor) != true) return true;

        }



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        if ($scope.Ingreso.RetencionIva != '') {
            $scope.p_SolProveedor.RetencionIva = $scope.Ingreso.RetencionIva.codigo;
        }

        if ($scope.Ingreso.RetencionIva2 != '') {
            $scope.p_SolProveedor.RetencionIva2 = $scope.Ingreso.RetencionIva2.codigo;
        }


        if ($scope.Ingreso.RetencionFuente != '') {
            $scope.p_SolProveedor.RetencionFuente = $scope.Ingreso.RetencionFuente.codigo;
        }

        if ($scope.Ingreso.RetencionFuente2 != '') {
            $scope.p_SolProveedor.RetencionFuente2 = $scope.Ingreso.RetencionFuente2.codigo;
        }

        if ($scope.Ingreso.CondicionPago != '') {
            $scope.p_SolProveedor.CondicionPago = $scope.Ingreso.CondicionPago.codigo;
        }

        if ($scope.Ingreso.GrupoCuenta != '') {
            $scope.p_SolProveedor.GrupoCuenta = $scope.Ingreso.GrupoCuenta.codigo;
        }

        if ($scope.Ingreso.DespachaProvincia != '') {
            $scope.p_SolProveedor.DespachaProvincia = $scope.Ingreso.DespachaProvincia.codigo;
        }


        if ($scope.Ingreso.autorizacion != '') {
            $scope.p_SolProveedor.Autorizacion = $scope.Ingreso.autorizacion.codigo;
        }

        if ($scope.Ingreso.GrupoTesoreria != '') {
            $scope.p_SolProveedor.GrupoTesoreria = $scope.Ingreso.GrupoTesoreria.codigo;
        }

        if (($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') && $scope.indpage == 'ING') {
        
            if (($scope.Estado != '' && ($scope.Estado != "AC" || $scope.Estado != "RE")) && $scope.Estado != "PA" && $scope.Estado != "IN") {
                if ($scope.Estado != 'DP' && $scope.Estado !=null) {
                    $scope.MenjError = "Tiene Una solicitud pendiente de Aprobar ";
                    $('#idMensajeError').modal('show');

                    return;
                }
            }

            if (bandera == '0') {
                $scope.p_SolProveedor.Estado = 'IN';
            }
            else {


                if ($scope.p_SolProvDireccion.Pais != "EC") {

                    if ($scope.SolProvBanco != "" && $scope.SolProvBanco.length > $scope.MaxCantCBanExtrPrv) {
                        $scope.MenjError = "Ingrese al Hasta   " + $scope.MaxCantCBanExtrPrv + " Cuentas Bancarias ";
                        $('#idMensajeError').modal('show');

                        return;
                    }
                }


              
               


                $scope.p_SolProveedor.Estado = 'EN';
            }
        }


        //Validar que seleccione al menos una linea de negocio como principal
        var valLinNegocioP = $filter('filter')($scope.ListaLineaNegocio, { principal: true }, true);
        if (valLinNegocioP.length == 0) {
            $scope.MenjError = "Seleccione al menos una línea de negocio como principal";
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

       



        
        $scope.p_SolProveedor.CodSapProveedor = authService.authentication.CodSAP;
        $scope.p_SolProveedor.TipoSolicitud = $scope.tiposolicitud;
        $scope.SolProveedor.push($scope.p_SolProveedor);
        $scope.SolProvDireccion.push($scope.p_SolProvDireccion);
        $scope.salir = 1;
        $scope.myPromise = null;
        var listaLinNegocio = $filter('filter')($scope.ListaLineaNegocio, { chekeado: true }, true);
        $scope.myPromise = ModificacionProveedor.getPostSolicitudList($scope.SolProveedor, $scope.SolProvContacto, $scope.SolProvBanco, $scope.SolProvDireccion, $scope.SolDocAdjunto, $scope.ViaPago, $scope.SolRamo, $scope.SolZona, $scope.SolProvHistEstado, listaLinNegocio).then(function (response) {
            
            $scope.IdSolicitud = response.data;
            if ($scope.tiposolicitud == 'NU' || $scope.tiposolicitud == 'AT') {


                if ($scope.indpage == 'ING') {
                    if (bandera == '0') {
                        $scope.MenjError = 'Registro guardado correctamente';
                    }
                    else {
                        $scope.MenjError = 'Registro enviado correctamente';
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
            }
            $scope.salir = 1;
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
    $scope.CargarProveedor();
}]);




'use strict';
app.controller('ContactosController', ['$scope', '$location', '$http', 'ModificacionProveedor', 'GeneralService', 'ngAuthSettings', '$cookies', '$filter', 'FileUploader', 'authService', 'localStorageService', '$sce', function ($scope, $location, $http, ModificacionProveedor, GeneralService, ngAuthSettings, $cookies, $filter, FileUploader, authService, localStorageService, $sce) {


    //Declaracion de variables
    //Variable de Grid

    $scope.ListDgCargos = [];
    var _GridContacto = [];
    $scope.pagesCon = [];
    $scope.pageContentCon = [];


    $scope.ListTipoIdentificacion = [];
    $scope.ListTratamiento = [];
    $scope.ListDepartaContacto = [];
    $scope.ListFuncionContactoT = [];
    $scope.ListFuncionContacto = [];
    $scope.ListRolDepartamento = [];
    $scope.SolProvContactoF = [];
    $scope.SolProvContacto = [];
    $scope.SolProvContactoG = [];
    $scope.ListDgZonas = [];
    $scope.ListDgAlmacenes = [];
    $scope.ListDgAlmacenesT = [];
    $scope.ListDgAlmSeleccionados = [];
    $scope.ListDgAlmSeleccionadosR = [];
    $scope.ListDgAlmSeleccionadosUsr = [];
    $scope.cmbestadolistado = [];
    $scope.cmbrecActaslistado = [];
    $scope.codigoCiudad = 0;
    $scope.recActasSN = false;
    $scope.allChecksSel = false;
    $scope.message = 'Por Favor Espere...';
    $scope.IdentificacionRes = "";
    $scope.myPromise = null;
    $scope.CodSapProveedor = authService.authentication.CodSAP;
    $scope.txtnombre = "";
    $scope.$watch('p_SolProvContacto.Nombre1', function (val) {
        $scope.p_SolProvContacto.Nombre1 = $filter('uppercase')(val);
    }, true);
    $scope.$watch('p_SolProvContacto.Apellido1', function (val) {
        $scope.p_SolProvContacto.Apellido1 = $filter('uppercase')(val);
    }, true);

    $scope.Ingreso = {
        Id: "",
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
        beneficiariobanco: ""
    };

    $scope.p_SolProvContacto = {

        IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
        Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
        PreFijo: '', DepCliente: '', RepLegal: true,
        Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
        NotElectronica: false, Cargos: [],
        NotTransBancaria: false, id: 0, pRecActas: false, 
    }

    $scope.p_SolProvContactoG = {

        IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
        Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
        PreFijo: '', DepCliente: '', RepLegal: true,
        Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
        NotElectronica: false, Departamento: '', Funcion: '',
        DescDepartamento: '', DescFuncion: '',
        NotTransBancaria: false, id: 0, pRecActas: false, 
    }

    $scope.p_SolProvCargos = {
        Departamento: '', Funcion: '',
        DescDepartamento: '', DescFuncion: ''
    }

    $scope.GrupoArticulo = [];
    $scope.GrupoArticuloDS = [];
    $scope.SettingGrupoArt = { displayProp: 'detalle', idProp: 'codigo', enableSearch: true, scrollableHeight: '200px', scrollable: true };
    $scope.rdbCodigo = "1";
    $scope.rdbTipoSol = "1";
    $scope.rdbEstado = "1";
    $scope.rdbRecibe = "1";
    $scope.txtCodReferencia = ""
    $scope.filtroIdentif = "";
    $scope.filtroDepartam = "";
    $scope.etiTotRegistros = "";
    GeneralService.getCatalogo('tbl_Ciudad').then(function (results) {
        $scope.GrupoArticuloDS = results.data;
    });
    //Cargar catalogos
    GeneralService.getCatalogo('tbl_tipoIdentificacion').then(function (results) {
        $scope.ListTipoIdentificacion = results.data;

    }, function (error) {
    });

    $scope.myPromise = GeneralService.getCatalogo('tbl_EstadoUsuarios').then(function (results) {
       
       
        $scope.cmbestadolistado = results.data;
    }, function (error) {
    });

    
    $scope.myPromise = GeneralService.getCatalogo('tbl_DespachaProvincia').then(function (results) {


        $scope.cmbrecActaslistado = results.data;
    }, function (error) {
    });


    GeneralService.getCatalogo('tbl_ProvGrupoCompras').then(function (results) {
        $scope.GrupoCompra = results.data;

    }, function (error) {
        
    });

    $scope.myPromise = ModificacionProveedor.getConsAlmacenes("7").then(function (results) {
        if (results.data.success) {
      
            var listAlmacen = results.data.root[0];
            $scope.cboAlmacenList1T = listAlmacen;


            for (var idx = 0; idx < $scope.cboAlmacenList1T.length; idx++) {


                $scope.cboAlmacenList1T[idx].pCodCiudad = $scope.cboAlmacenList1T[idx].pCodCiudad;
                $scope.ListDgAlmacenesT.push({
                    pCodUsuario: "",                   
                    pCodAlmacen: $scope.cboAlmacenList1T[idx].pCodAlmacen,
                    pNomAlmacen: $scope.cboAlmacenList1T[idx].pNomAlmacen,
                    pCodCiudad: $scope.cboAlmacenList1T[idx].pCodCiudad,
                    pElegir: false
                });

                
            }
        }
        else {
            $scope.showMessage('E', 'Error al consultar almacenes: ' + results.data.msgError);
        }
    }, function (error) {
        $scope.showMessage('E', "Error en comunicación: getConsAlmacenes().");
    });


    $scope.myPromise = ModificacionProveedor.getConsTodasZonas('').then(function (results) {

        $scope.ListDgZonas = results.data.root[0];
        if (results.data.root[0] != null) {
            for (var i = 0; i < $scope.ListDgZonas.length; i++) {
                $scope.ListDgZonas[i].pElegir = false;
            }
        }
    }, function (error) {
    });

    $scope.myPromise = ModificacionProveedor.getConsAlmacenes("5").then(function (results) {
        if (results.data.success) {
            $scope.ListRolDepartamento = results.data.root[0];
     

        }
        else {
            
            $scope.MenjError = 'Error al consultar RolDepartamento: ' + results.data.msgError;
            $('#idMensajeError').modal('show');
        }
    }, function (error) {
        $scope.MenjError = 'Error en comunicación: getConsRolesDepartamento(). ';
        $('#idMensajeError').modal('show');

    });

    GeneralService.getCatalogo('tbl_Tratamiento').then(function (results) {
        $scope.ListTratamiento = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_DepartaContacto').then(function (results) {
        $scope.ListDepartaContacto = results.data;
    }, function (error) {
    });

    GeneralService.getCatalogo('tbl_FuncionContacto').then(function (results) {
        $scope.ListFuncionContactoT = results.data;
    }, function (error) {
    });


    //Filtrar funciones por departamentos
    $scope.$watch('Ingreso.DepartaContacto', function () {
        if ($scope.Ingreso.DepartaContacto == null) { $scope.ListFuncionContacto = []; return; };
        var realizoFiltro = false;
        $scope.ListFuncionContacto = [];
        $scope.ListFuncionContacto = $filter('filter')($scope.ListFuncionContactoT, { descAlterno: $scope.Ingreso.DepartaContacto.codigo }, true);

    });

    $scope.actIdentif = function () {
     
      
        if ($scope.IdentificacionRes != $scope.p_SolProvContacto.Identificacion)
        {
            if ($scope.p_SolProvContacto.Identificacion == "") {
                return;
            }
            
            for (var e = 0; e < $scope.ListDgAlmSeleccionados.length; e++) {
                if ($scope.IdentificacionRes == $scope.ListDgAlmSeleccionados[e].pCodUsuario) {
                    $scope.ListDgAlmSeleccionados[e].pCodUsuario = $scope.p_SolProvContacto.Identificacion;

                }
            }

            $scope.IdentificacionRes = $scope.p_SolProvContacto.Identificacion;
            
        }

        
    }

    $scope.toggleCategory = function (content) {

        content.expanded = !content.expanded;
    }
    //Eliminar cargos
    $scope.eliminarCargo = function (registro) {

        for (var id = 0 ; id < $scope.ListDgCargos.length; id++) {
            if ($scope.ListDgCargos[id].Funcion == registro.Funcion
                && $scope.ListDgCargos[id].Departamento == registro.Departamento) {
                break
                return;
            }
        }
        $scope.ListDgCargos.splice(id, 1);



    }


    $scope.selAlmacen = function (codciudad, desciudad) {
        $scope.allChecksSel = false;
      
        
        if ($scope.p_SolProvContacto.Identificacion == "" || $scope.p_SolProvContacto.Identificacion == undefined) {

            $scope.showMessage('I', "Ingrese datos generales del contacto.");

            return;
        }
        $scope.ListDgAlmacenes = $filter('filter')($scope.ListDgAlmacenesT, { pCodCiudad: codciudad }, true);

        $scope.codigoCiudad = codciudad;
       
        //Almacenes que tiene el usuario asignado
        var lisAlma = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: $scope.p_SolProvContacto.Identificacion }, true);
        for (var id = 0 ; id < $scope.ListDgAlmacenes.length ; id++) {
            var isSeleccion = $filter('filter')(lisAlma, { pCodAlmacen: $scope.ListDgAlmacenes[id].pCodAlmacen }, true);
            if (isSeleccion.length > 0) {
                $scope.ListDgAlmacenes[id].pElegir = true;
            }
            else {
                $scope.ListDgAlmacenes[id].pElegir = false;
            }

        }



        $scope.desCiudad = desciudad;
        
        $("#selAlmc").prop("checked", "");
        $('#almacenesLista').modal('show');
        
        
    }

    $scope.muestraAlmacenes = function (valor, codciudad, desciudad) {
        $scope.allChecksSel = false;
        if ($scope.p_SolProvContacto.Identificacion == "" || $scope.p_SolProvContacto.Identificacion == undefined) {

            $scope.showMessage('I', "Ingrese datos generales del contacto.");
    
            var reg = $filter('filter')($scope.ListDgZonas, { pCodZona: codciudad }, true);
            reg[0].pElegir = false;
            return;
        }
        if (valor) {
            $scope.desCiudad = desciudad;
            
 
            $scope.codigoCiudad = codciudad;
            $scope.ListDgAlmacenes = $filter('filter')($scope.ListDgAlmacenesT, { pCodCiudad: codciudad }, true);
            var lisAlma = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: $scope.p_SolProvContacto.Identificacion }, true);
            for (var id = 0 ; id < $scope.ListDgAlmacenes.length ; id++) {
                var isSeleccion = $filter('filter')(lisAlma, { pCodAlmacen: $scope.ListDgAlmacenes[id].pCodAlmacen }, true);
                if (isSeleccion.length > 0) {
                    $scope.ListDgAlmacenes[id].pElegir = true;
                }
                else {
                    $scope.ListDgAlmacenes[id].pElegir = false;
                }

            }
            $("#selAlmc").prop("checked", "");
            $('#almacenesLista').modal('show');

        }
        else {
            
           
            var seguir = true;
            while (seguir) {
                var elimina = false;
                for (var e = 0; e < $scope.ListDgAlmSeleccionados.length; e++) {
                    if ($scope.p_SolProvContacto.Identificacion == $scope.ListDgAlmSeleccionados[e].pCodUsuario && codciudad == $scope.ListDgAlmSeleccionados[e].pCodCiudad) {
                        elimina = true;
                        break;
                    }
                }
                if (elimina)
                    $scope.ListDgAlmSeleccionados.splice(e, 1);
                else
                    seguir = false;
                if ($scope.ListDgAlmSeleccionados.length == 0)
                    seguir = false;

                var listUsr = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: $scope.p_SolProvContacto.Identificacion, pCodCiudad: codciudad }, true);
                if (listUsr.length > 0)
                    seguir = true;
                else
                    seguir = false;
            }


        }
    }

    $scope.quitarAlmacenes = function (usr) {
        var seguir = true;
        while (seguir) {
            var elimina = false;
            for (var e = 0; e < $scope.ListDgAlmSeleccionados.length; e++) {
                if ( $scope.ListDgAlmSeleccionados[e].pCodUsuario   == usr ) {
                    elimina = true;
                    break;
                }
            }
            if (elimina)
                $scope.ListDgAlmSeleccionados.splice(e, 1);
            else
                seguir = false;
            if ($scope.ListDgAlmSeleccionados.length == 0)
                seguir = false;

            var listUsr = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: usr }, true);
            if (listUsr.length > 0)
                seguir = true;
            else
                seguir = false;
        }
    }

    $scope.marcaChecksS = function (valor) {
        
            for (var i = 0; i < $scope.ListDgAlmacenes.length; i++)
            {
                var updt = $scope.ListDgAlmacenes[i];
                updt.pElegir = valor;
            }
        
    }

    $scope.marcaChecksA = function (valor) {
        

        if ($scope.p_SolProvContacto.Identificacion == "") {
            
            $scope.showMessage('I', "Ingrese datos generales del contacto.");
            $scope.allChecksCiu = false;
            $scope.allChecksAlm = false;
            return;
        }


        if (valor) {
            $scope.quitarAlmacenes($scope.p_SolProvContacto.Identificacion);
            for (var j = 0; j < $scope.ListDgAlmacenesT.length; j++) {
                
                var update = $scope.ListDgAlmacenesT[j];
                $scope.ListDgAlmSeleccionados.push({
                    pCodUsuario: $scope.p_SolProvContacto.Identificacion,
                    pCodAlmacen: update.pCodAlmacen,
                    pNomAlmacen: update.pNomAlmacen,
                    pCodCiudad: update.pCodCiudad

                });

            }
        }
        else {
            $scope.quitarAlmacenes($scope.p_SolProvContacto.Identificacion);
        }

        

    }

    $scope.marcaChecksC = function (valor) {

        if ($scope.p_SolProvContacto.Identificacion == "" || $scope.p_SolProvContacto.Identificacion == undefined) {
            $scope.showMessage('I', "Ingrese datos generales del contacto.");
            $scope.allChecksCiu = false;
            $scope.allChecksAlm = false;
            return;
        }
        if (!valor)
            $scope.quitarAlmacenes($scope.p_SolProvContacto.Identificacion);

        for (var j = 0; j < $scope.ListDgZonas.length; j++) {
            $scope.ListDgZonas[j].pElegir = valor;
        }

        if (!valor) {
            $scope.allChecksAlm = valor;
            for (var j = 0; j < $scope.ListDgAlmacenesT.length; j++) {
                $scope.ListDgAlmacenesT[j].pElegir = valor;
            }
        }

    }

    $scope.actualizaAlmacenes = function (pCiudad) {
        
       
        var pCiudad = $scope.codigoCiudad;
        //Eliminar por usuario
        var seguir = true;
        while(seguir)
        {
            var elimina = false;
            for (var e = 0; e < $scope.ListDgAlmSeleccionados.length; e++)
            {
                if ($scope.p_SolProvContacto.Identificacion == $scope.ListDgAlmSeleccionados[e].pCodUsuario && pCiudad == $scope.ListDgAlmSeleccionados[e].pCodCiudad)
                {
                    elimina = true;
                    break;
                }
            }
            if (elimina)
                $scope.ListDgAlmSeleccionados.splice(e, 1);
            else
                seguir = false;
            if ($scope.ListDgAlmSeleccionados.length == 0 )
                seguir = false;
            
            var listUsr = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: $scope.p_SolProvContacto.Identificacion, pCodCiudad: pCiudad }, true);
            if (listUsr.length > 0)
                seguir = true;
            else
                seguir = false;
        }
        



        for (var i = 0 ; i < $scope.ListDgAlmacenes.length ; i++)
        {
            var update = $scope.ListDgAlmacenes[i];


            if (update.pElegir == true) {
                $scope.ListDgAlmSeleccionados.push({
                    pCodUsuario: $scope.p_SolProvContacto.Identificacion,
                    pCodAlmacen: update.pCodAlmacen,
                    pNomAlmacen: update.pNomAlmacen,
                    pCodCiudad: update.pCodCiudad
                    
                });
            }
        }
        

        $scope.IdentificacionRes = $scope.p_SolProvContacto.Identificacion;

        var reg = $filter('filter')($scope.ListDgAlmacenes, { pElegir: true }, true);
        if (reg.length < 1) {
            $scope.showMessage('I', 'Seleccione al menos un almacén.');
        }
        else {
            $('#almacenesLista').modal('hide');
        }


    }

    //Agregar cargos
    $scope.agregarCargo = function () {


        if ($scope.Ingreso.DepartaContacto == null || $scope.Ingreso.FuncionContacto == "") {
            $scope.MenjError = "Seleccione departamento de contacto.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if ($scope.Ingreso.FuncionContacto == null || $scope.Ingreso.FuncionContacto == "") {
            $scope.MenjError = "Seleccione función de contacto.";
            $('#idMensajeInformativo').modal('show');
            return;
        }


        //Validar que no se ingrese el mismo DEPARTAMENTO Y FUNCION
        for (var id = 0 ; id < $scope.ListDgCargos.length; id++) {
            if ($scope.ListDgCargos[id].Funcion == $scope.Ingreso.FuncionContacto.codigo
                && $scope.ListDgCargos[id].Departamento == $scope.Ingreso.DepartaContacto.codigo) {
                $scope.MenjError = "Departamento y función ya registrado.";
                $('#idMensajeInformativo').modal('show');
                return;
            }
        }

        $scope.p_SolProvCargos = {};
        $scope.p_SolProvCargos.Funcion = $scope.Ingreso.FuncionContacto.codigo;
        $scope.p_SolProvCargos.DescFuncion = $scope.Ingreso.FuncionContacto.detalle;
        $scope.p_SolProvCargos.Departamento = $scope.Ingreso.DepartaContacto.codigo;
        $scope.p_SolProvCargos.DescDepartamento = $scope.Ingreso.DepartaContacto.detalle;
        if ($scope.Ingreso.FuncionContacto.detalle != "" && $scope.Ingreso.DepartaContacto.detalle != "" &&
             $scope.Ingreso.FuncionContacto.detalle != null && $scope.Ingreso.DepartaContacto.detalle != null) {

            $scope.ListDgCargos.push($scope.p_SolProvCargos);

        }




    }



    //Realizar filtros
    $scope.filtroConsulta = function () {

        if ($scope.rdbCodigo == 1) {

            $scope.txtCodReferencia = "";
            $scope.filtroIdentif = "";
        }

        if ($scope.rdbTipoSol == 1) {


            $scope.filtroDepartam = "";
            $scope.ddlTipoSolicitud = undefined;
        }

        if ($scope.rdbCodigo == 2 && $scope.txtCodReferencia == "") {

            $scope.MenjError = "Ingrese identificación a consultar.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        if ($scope.rdbTipoSol == 2 && !$scope.ddlTipoSolicitud) {

            $scope.MenjError = "Seleccione departamento a consultar.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if ($scope.rdbEstado == 2 && !$scope.cmbestado) {

            $scope.MenjError = "Seleccione estado a consultar.";
            $('#idMensajeInformativo').modal('show');
            return;
        }
        if ($scope.rdbRecibe == 2 && !$scope.cmbestadoAct) {

            $scope.MenjError = "Seleccione SI/NO recibe actas.";
            $('#idMensajeInformativo').modal('show');
            return;
        }

        


        //Realizar filtros
        setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
        $scope.SolProvContactoF = $scope.SolProvContacto.slice();
        setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        if ($scope.rdbCodigo == 2 && $scope.txtCodReferencia != "") {
            $scope.filtroIdentif = $scope.txtCodReferencia;
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.SolProvContactoF = $filter('filter')($scope.SolProvContactoF, { Identificacion: $scope.filtroIdentif }, true);
                           

            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }


        if ($scope.txtnombre != "")
        {
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            
            $scope.SolProvContactoF = $filter('filter')($scope.SolProvContactoF, function (obj) {
                return obj.Nombre1.toUpperCase() == $scope.txtnombre.toUpperCase() ||
                       obj.Nombre2.toUpperCase() == $scope.txtnombre.toUpperCase() ||
                       obj.Apellido1.toUpperCase() == $scope.txtnombre.toUpperCase() ||
                       obj.Apellido2.toUpperCase() == $scope.txtnombre.toUpperCase();
            });

            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }

        if ($scope.rdbEstado == 2 ) {
            var estCons = false;
            if ($scope.cmbestado.codigo == "A")
                estCons = true;
            else
                estCons = false;
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.SolProvContactoF = $filter('filter')($scope.SolProvContactoF, { Estado: estCons }, true);
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }
          
        if ($scope.rdbRecibe == 2) {
            var valorFil = false
            if ($scope.cmbestadoAct.codigo == 'S')
                valorFil = true;
            if ($scope.cmbestadoAct.codigo == 'N')
                valorFil = false;
            setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);
            $scope.SolProvContactoF = $filter('filter')($scope.SolProvContactoF, { pRecActas: valorFil }, true);
            setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
        }


        $scope.etiTotRegistros = $scope.SolProvContactoF.length;
        if ($scope.etiTotRegistros == 0) {
            $scope.MenjError = "No existe resultado para su consulta.";
            $('#idMensajeInformativo').modal('show');
        }



    }





    ///habilita identificacion contacto
    $scope.disableClickcontacto = function (ban, content) {
        debugger;
        $('#Contactoform').modal('show');
        $scope.IdentificacionRes = "";
        $scope.ListDgAlmSeleccionadosR = $scope.ListDgAlmSeleccionados.slice();
       
       


        if ($('#accordion_default').hasClass('collapsed')) {
            setTimeout(function () { angular.element('#accordion_default').trigger('click'); }, 150);
        }
        if (ban == 1) {
            $scope.isDisabledContact = true;
            $scope.allChecksAlm = false;
            $scope.allChecksCiu = false;
            $scope.allChecksSel = false;
        }
        else {
            $scope.isDisabledContact = false;
            $scope.allChecksAlm = false;
            $scope.allChecksCiu = false;
            $scope.allChecksSel = false;
            for (var id = 0 ; id < $scope.ListDgAlmacenes.length ; id++) {
              
                 $scope.ListDgAlmacenes[id].pElegir = false;
               

            }


        }
        if (content != "") {

            

            $scope.limpiacontacto();

            //Ciudades que tiene el usuario asignado
         
            var lisZonas = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: content.Identificacion }, true);
            for (var id = 0 ; id < $scope.ListDgZonas.length ; id++)
            {
                var isSeleccion = $filter('filter')(lisZonas, { pCodCiudad: $scope.ListDgZonas[id].pCodZona }, true);
                if (isSeleccion.length > 0) {
                    $scope.ListDgZonas[id].pElegir = true;
                }
                else
                {
                    $scope.ListDgZonas[id].pElegir = false;
                }

            }


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


            $scope.p_SolProvContacto.RepLegal = content.RepLegal;
            $scope.p_SolProvContacto.Estado = content.Estado;
            $scope.p_SolProvContacto.TelfFijo = content.TelfFijo;
            $scope.p_SolProvContacto.TelfFijoEXT = content.TelfFijoEXT;
            $scope.p_SolProvContacto.TelfMovil = content.TelfMovil;
            $scope.p_SolProvContacto.EMAIL = content.EMAIL;
            $scope.p_SolProvContacto.NotElectronica = content.NotElectronica;
            $scope.p_SolProvContacto.NotTransBancaria = content.NotTransBancaria;
            $scope.p_SolProvContacto.pRecActas = content.pRecActas;
            $scope.p_SolProvContacto.Id = content.Id;
            $scope.ListDgCargos = content.Cargos.slice();



            var index = 0;

            for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                if ($scope.ListTipoIdentificacion[index].codigo === content.TipoIdentificacion) {
                    $scope.Ingreso.Contactipoidentificacion = $scope.ListTipoIdentificacion[index];

                    break;
                }
            }

            

            for (index = 0; index < $scope.ListTratamiento.length; index++) {
                if ($scope.ListTratamiento[index].detalle.toUpperCase() === content.PreFijo.toUpperCase()) {
                    $scope.Ingreso.Tratamiento = $scope.ListTratamiento[index];

                    break;
                }
            }

            $scope.Grabarcontacto = "Guardar";
        }
        else {
            $scope.limpiacontacto();
            for (var id = 0 ; id < $scope.ListDgZonas.length ; id++) {
               
                    $scope.ListDgZonas[id].pElegir = false;          

            }
            $scope.Grabarcontacto = "Grabar";

            for (index = 0; index < $scope.ListTipoIdentificacion.length; index++) {
                if ($scope.ListTipoIdentificacion[index].codigo === "CD") {
                    $scope.Ingreso.Contactipoidentificacion = $scope.ListTipoIdentificacion[index];

                    break;
                }
            }
        }

        


        return false;
    }


    //Activar 

    //Eliminar Contacto
    $scope.ActInaContacto = function (valor) {
    
        

        var existe = false;
        for (var i = 0, len = $scope.SolProvContacto.length; i < len; i++) {
            if ($scope.SolProvContacto[i].Id === $scope.IdEliminar) {
                existe = true;
                $scope.SolProvContacto[i].Estado = valor;
                break;
            }
        }

        if (existe) {
            
        }

        var existe = false;
        for (var i = 0, len = $scope.SolProvContactoF.length; i < len; i++) {
            if ($scope.SolProvContactoF[i].Id === $scope.IdEliminar) {
                existe = true;
                $scope.SolProvContactoF[i].Estado = valor;
                break;
            }
        }

        if (existe) {
            
            
        }
        if (!valor)
            $scope.ConfirmGrabar("3");
        else
            $scope.ConfirmGrabar("4");

    }

    $scope.eliminarClickcontacto = function (content) {
       
        content.Estado = false;

    }

    $scope.reactivarClickcontacto = function (content) {
   

        
        content.Estado = true;

    }

    $scope.confirmaAgregarcontacto = function () {
        $scope.p_SolProvContacto.Nombre1 = $scope.p_SolProvContacto.Nombre1.toUpperCase();
        $scope.p_SolProvContacto.Nombre2 = $scope.p_SolProvContacto.Nombre2.toUpperCase();
        $scope.p_SolProvContacto.Apellido1 = $scope.p_SolProvContacto.Apellido1.toUpperCase();
        $scope.p_SolProvContacto.Apellido2 = $scope.p_SolProvContacto.Apellido2.toUpperCase();
        $scope.p_SolProvContacto.EMAIL = $scope.p_SolProvContacto.EMAIL;
        
        $scope.grabarContactopantalla();
      

    }

    $scope.reversarAlm = function () {
        $scope.ListDgAlmSeleccionados = $scope.ListDgAlmSeleccionadosR.slice();
        $scope.limpiacontacto();
        $scope.allChecksCiu = false;
        $scope.allChecksAlm = false;
        $scope.allChecksSel = false;
    }
    //Encerar variables Contacto
    $scope.limpiacontacto = function () {
       
        $scope.p_SolProvContacto = {

            IdSolContacto: '', IdSolicitud: '', TipoIdentificacion: '', DescTipoIdentificacion: '', Identificacion: '',
            Nombre2: '', Nombre1: '', Apellido2: '', Apellido1: '', CodSapContacto: '',
            PreFijo: '', DepCliente: '', RepLegal: true,
            Estado: true, TelfFijo: '', TelfFijoEXT: '', TelfMovil: '', EMAIL: '',
            NotElectronica: false, Cargos: [],
            NotTransBancaria: false, id: 0, pRecActas: false,
        }

        $scope.p_SolProvCargos = {
            Departamento: '', Funcion: '',
            DescDepartamento: '', DescFuncion: ''
        }
        $scope.Ingreso.DepartaContacto = "";
        $scope.Ingreso.FuncionContacto = "";
        $scope.Ingreso.Tratamiento = "";
        $scope.GrupoArticulo = [];
        $scope.ListDgCargos = [];
    }

    //valida ruc losefocus
    $scope.validorCedulaguradr = function (txtIdentificacion) {
      
        var campos = txtIdentificacion;
        if ($scope.Ingreso.Contactipoidentificacion.codigo == 'CD') {
            if (txtIdentificacion.length != 10) {
                $scope.MenjError = "Identificación ingresada es incorrecta.";
                $('#idMensajeError').modal('show');

                return false;
            }
        }

        if ($scope.Ingreso.Contactipoidentificacion.codigo == 'RC') {
            if (txtIdentificacion.length != 13) {
                $scope.MenjError = "RUC ingresado es inválido.";
                $('#idMensajeError').modal('show');

                return false;
            }
        }

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

    ///Agregar/Modificar contacto
    $scope.grabarContactopantalla = function () {

        //Acciones 
        //Inactivar
        if ($scope.accion == 997)
        {
            $scope.ActInaContacto(false);
            return;
        }
        //Activar
        if ($scope.accion == 996) {
            $scope.ActInaContacto(true);
            return;
        }

        var datos = 0;
        for (var i = 0; i < $scope.ListDgCargos.length; i++) {
            if ($scope.ListDgCargos[i].Funcion == "11") {
                datos = 1;
            }
        }
        if ($scope.ListDgCargos.length == 0) {
            $scope.MenjError = "Ingrese al menos un departamento y función para el contacto. "
            $scope.showMessage('I', $scope.MenjError);
            return;
        } else {
            if ($scope.ListDgCargos.length == 1 && datos == 1) {
                $scope.MenjError = "Ingrese al menos un departamento y función para el contacto. "
                $scope.showMessage('I', $scope.MenjError);
                return;
            }
        }
        if ($scope.p_SolProvContacto.pRecActas) {
         
            var existeRolDep = $filter('filter')($scope.ListDgCargos, { Departamento: "0006" }, true);

            if (existeRolDep == undefined) existeRolDep = [];
            
            var ciudadSelec = $filter('filter')($scope.ListDgZonas, { pElegir: true }, true);
            if (ciudadSelec.length < 1) {
                $scope.showMessage('I', "Debe seleccionar al menos una ciudad.");
                return;
            }

            for (var r = 0 ; r < $scope.ListDgZonas.length ; r++) {

                if ($scope.ListDgZonas[r].pElegir) {
                
                    var almUsr = $filter('filter')($scope.ListDgAlmSeleccionados, { pCodUsuario: $scope.p_SolProvContacto.Identificacion }, true);
                    //Validar que se seleccione al menos un almacen
                    var existeAlm = false;
                    for (var idx = 0 ; idx < almUsr.length; idx++) {
                        if (almUsr[idx].pCodCiudad == $scope.ListDgZonas[r].pCodZona) {
                            existeAlm = true;
                            break;
                        }
                    }

                    if (!existeAlm) {
                        $scope.showMessage('I', 'Seleccione al menos un almacén/agencia para la ciudad de ' + $scope.ListDgZonas[r].pDescripcion);
                        return;
                    }
                }

            }

        }
        else {
            $scope.quitarAlmacenes($scope.p_SolProvContacto.Identificacion);

        }


        if ($scope.isDisabledApr) {
            $scope.MenjError = "No Puede Modificar el Contacto... ";
            $('#idMensajeError').modal('show');
            return;
        }
        if ($scope.p_SolProvContacto != "") {

            if ($scope.Ingreso.Contactipoidentificacion != '') {
                $scope.p_SolProvContacto.TipoIdentificacion = $scope.Ingreso.Contactipoidentificacion.codigo;
            }

            if ($scope.Ingreso.Tratamiento != '') {
                $scope.p_SolProvContacto.PreFijo = $scope.Ingreso.Tratamiento.detalle;
            }

            if ($scope.Ingreso.DepartaContacto != '') {
                $scope.p_SolProvContacto.Departamento = $scope.Ingreso.DepartaContacto.codigo;
                $scope.p_SolProvContacto.DescDepartamento = $scope.Ingreso.DepartaContacto.detalle;
            }

            if ($scope.Ingreso.FuncionContacto != '' && $scope.Ingreso.FuncionContacto != null) {
                $scope.p_SolProvContacto.Funcion = $scope.Ingreso.FuncionContacto.codigo;
                $scope.p_SolProvContacto.DescFuncion = $scope.Ingreso.FuncionContacto.detalle;
            }
            else {
                $scope.p_SolProvContacto.Funcion = "";
                $scope.p_SolProvContacto.DescFuncion = "";
            }

            //PASAPORTE & RUC EXTRANJEROS no validar identificacion
            if ($scope.p_SolProvContacto.TipoIdentificacion != "PS" && $scope.p_SolProvContacto.TipoIdentificacion != "RE") {
                if (isNaN($scope.p_SolProvContacto.Identificacion)) {
                    $scope.MenjError = "Identificación incorrecta.";
                    $('#idMensajeError').modal('show');
                    return true;
                }
                $scope.MenjError = "";
                if ($scope.validorCedulaguradr($scope.p_SolProvContacto.Identificacion) != true) {
                    if ($scope.MenjError == "") {
                        $scope.MenjError = "Identificación incorrecta.";
                        $('#idMensajeError').modal('show');
                    }
                    return true;
                }
            }
            if ($scope.isDisabledContact == true) {//edita
                
                var index = 0;
                for (index = 0; index < $scope.SolProvContacto.length; index++) {
            



                    if ($scope.SolProvContacto[index].Id == $scope.p_SolProvContacto.Id) {
                        $scope.SolProvContacto[index].Identificacion = $scope.p_SolProvContacto.Identificacion;
                        $scope.SolProvContacto[index].TipoIdentificacion = $scope.p_SolProvContacto.TipoIdentificacion;
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
                        $scope.SolProvContacto[index].pRecActas = $scope.p_SolProvContacto.pRecActas;
                        $scope.SolProvContacto[index].NotElectronica = $scope.p_SolProvContacto.NotElectronica;
                        $scope.SolProvContacto[index].Cargos = [];
                        $scope.SolProvContacto[index].Cargos = $scope.ListDgCargos.slice();
                        
                        break;
                    }
                }


            }
            else {//agrega
                //Obtener Secuencial si es nuevo contacto
                var maxIndex = $scope.SolProvContacto.length - 1;
                if (maxIndex >= 0)
                    $scope.p_SolProvContacto.Id = parseInt($scope.SolProvContacto[maxIndex].Id) + 1;
                else
                    $scope.p_SolProvContacto.Id = 1;


                //Cargos
                $scope.p_SolProvContacto.Cargos = $scope.ListDgCargos.slice();

                if ($scope.SolProvContacto != "") {


                    $scope.SolProvContacto.push($scope.p_SolProvContacto);

                    $scope.SolProvContactoF.push($scope.p_SolProvContacto);

                    

                }
                else {

                    $scope.SolProvContacto = [$scope.p_SolProvContacto];
                    $scope.SolProvContactoF.push($scope.p_SolProvContacto);
                    
                }
                $scope.filtroConsulta();
            }


            $scope.showMessage("I", "Contacto creado. Recuerde grabar los cambios realizados al dar clic en el botón Guardar de la Bandeja de Consulta de Contactos. NOTA: Si no realiza esta acción, no grabará ninguna información");
            $('#Contactoform').modal('hide');

            

        }
    }

    $scope.GrabarGeneral=function()
    {
        for (var i = 0; i < $scope.SolProvContactoF.length; i++) {
            if ($scope.SolProvContactoF[i].Identificacion=="") {
                $scope.MenjError = "Todos los contactos debe tener Identificacion. "
                $('#idMensajeError').modal('show');
                return;
            }
        }
        debugger;
        $scope.MenjConfirmacion = "¿Está seguro que desea Guardar los contactos?";
        $scope.accion = 999;
        $('#idMensajeConfirmacion').modal('show');

    }


    $scope.grabar = function (tipo) {
        //Revisar datos completos que se envia a grabar
        
        debugger;
       
        //Armar de cab-det a lineal SAP no soporta cab-det
        $scope.SolProvContactoG = [];
        debugger;
        for (var indice = 0; indice < $scope.SolProvContactoF.length; indice++) {


                var listaCargos = $scope.SolProvContactoF[indice].Cargos;

            for (var j = 0; j < listaCargos.length; j++) {
                
                $scope.p_SolProvContactoG = {};
                $scope.p_SolProvContactoG.IdSolContacto = $scope.SolProvContactoF[indice].IdSolContacto;
                $scope.p_SolProvContactoG.IdSolicitud = $scope.SolProvContactoF[indice].IdSolicitud;
                $scope.p_SolProvContactoG.TipoIdentificacion = $scope.SolProvContactoF[indice].TipoIdentificacion;
                $scope.p_SolProvContactoG.DescTipoIdentificacion = $scope.SolProvContactoF[indice].DescTipoIdentificacion;
                $scope.p_SolProvContactoG.Identificacion = $scope.SolProvContactoF[indice].Identificacion;
                $scope.p_SolProvContactoG.Nombre2 = $scope.SolProvContactoF[indice].Nombre2;
                $scope.p_SolProvContactoG.Nombre1 = $scope.SolProvContactoF[indice].Nombre1;
                $scope.p_SolProvContactoG.Apellido2 = $scope.SolProvContactoF[indice].Apellido2;
                $scope.p_SolProvContactoG.Apellido1 = $scope.SolProvContactoF[indice].Apellido1;
                $scope.p_SolProvContactoG.CodSapContacto = $scope.SolProvContactoF[indice].CodSapContacto;
                $scope.p_SolProvContactoG.PreFijo = $scope.SolProvContactoF[indice].PreFijo;
                $scope.p_SolProvContactoG.DepCliente = $scope.SolProvContactoF[indice].DepCliente;
                $scope.p_SolProvContactoG.RepLegal = $scope.SolProvContactoF[indice].RepLegal;
                $scope.p_SolProvContactoG.Estado = $scope.SolProvContactoF[indice].Estado;
                $scope.p_SolProvContactoG.TelfFijo = $scope.SolProvContactoF[indice].TelfFijo;
                $scope.p_SolProvContactoG.TelfFijoEXT = $scope.SolProvContactoF[indice].TelfFijoEXT;
                $scope.p_SolProvContactoG.TelfMovil = $scope.SolProvContactoF[indice].TelfMovil;
                $scope.p_SolProvContactoG.EMAIL = $scope.SolProvContactoF[indice].EMAIL;
                $scope.p_SolProvContactoG.NotElectronica = $scope.SolProvContactoF[indice].NotElectronica;
                $scope.p_SolProvContactoG.NotTransBancaria = $scope.SolProvContactoF[indice].NotTransBancaria;
                $scope.p_SolProvContactoG.pRecActas = $scope.SolProvContactoF[indice].pRecActas;
                $scope.p_SolProvContactoG.id = $scope.SolProvContactoF[indice].id;

                $scope.p_SolProvContactoG.Departamento = listaCargos[j].Departamento;
                $scope.p_SolProvContactoG.Funcion = listaCargos[j].Funcion;
                $scope.p_SolProvContactoG.DescDepartamento = listaCargos[j].DescDepartamento;
                $scope.p_SolProvContactoG.DescFuncion = listaCargos[j].DescFuncion;
                $scope.SolProvContactoG.push($scope.p_SolProvContactoG);

            }
          }


        

        //Service para grabar contacto
        
    var almSeleccionados = $scope.ListDgAlmSeleccionados;


        $scope.myPromise = ModificacionProveedor.getGrabaContacto($scope.SolProvContactoG, $scope.CodSapProveedor, "", "", "S", almSeleccionados).then(function (results) {
            if (results.data.success) {
                $scope.p_SolProvContacto = {};
                if (tipo == "1")
                    $scope.MenjError = "Contacto grabado correctamente.";
                else
                {
                    if (tipo == "2")
                        $scope.MenjError = "Contactos guardados correctamente.";
                    else
                    {
                        if (tipo == "3")
                            $scope.MenjError = "Contacto inactivado correctamente.";
                        else
                            $scope.MenjError = "Contactos guardados correctamente.";
                    }
                       
                }
                    

                $scope.p_SolProvContacto = {};
                $('#idMensajeOk').modal('show');
            }
            else {
                $scope.MenjError = results.data.mensaje;
                $('#idMensajeError').modal('show');
                $('#Contactoform').modal('hide');
            }
        }, function (error) {
        });
    }

    $("#idMensajeOk").click(function () {
        $('#Contactoform').modal('hide');
        $scope.consultaContacto();
    });
    ///Agregar/Modificar contacto
    $scope.grabarContacto = function (txtIdentificacion) {

        //Armar de cab-det a lineal SAP no soporta cab-det
        $scope.SolProvContactoG = [];
        for (var indice = 0; indice < $scope.SolProvContacto.length; indice++) {
            if (txtIdentificacion == $scope.SolProvContacto[indice].Identificacion) {
                var listaCargos = $scope.SolProvContacto[indice].Cargos;

                for (var j = 0 ; j < listaCargos.length; j++) {

                    $scope.p_SolProvContactoG = {};
                    $scope.p_SolProvContactoG.IdSolContacto = $scope.SolProvContacto[indice].IdSolContacto;
                    $scope.p_SolProvContactoG.IdSolicitud = $scope.SolProvContacto[indice].IdSolicitud;
                    $scope.p_SolProvContactoG.TipoIdentificacion = $scope.SolProvContacto[indice].TipoIdentificacion;
                    $scope.p_SolProvContactoG.DescTipoIdentificacion = $scope.SolProvContacto[indice].DescTipoIdentificacion;
                    $scope.p_SolProvContactoG.Identificacion = $scope.SolProvContacto[indice].Identificacion;
                    $scope.p_SolProvContactoG.Nombre2 = $scope.SolProvContacto[indice].Nombre2;
                    $scope.p_SolProvContactoG.Nombre1 = $scope.SolProvContacto[indice].Nombre1;
                    $scope.p_SolProvContactoG.Apellido2 = $scope.SolProvContacto[indice].Apellido2;
                    $scope.p_SolProvContactoG.Apellido1 = $scope.SolProvContacto[indice].Apellido1;
                    $scope.p_SolProvContactoG.CodSapContacto = $scope.SolProvContacto[indice].CodSapContacto;
                    $scope.p_SolProvContactoG.PreFijo = $scope.SolProvContacto[indice].PreFijo;
                    $scope.p_SolProvContactoG.DepCliente = $scope.SolProvContacto[indice].DepCliente;
                    $scope.p_SolProvContactoG.RepLegal = $scope.SolProvContacto[indice].RepLegal;
                    $scope.p_SolProvContactoG.Estado = $scope.SolProvContacto[indice].Estado;
                    $scope.p_SolProvContactoG.TelfFijo = $scope.SolProvContacto[indice].TelfFijo;
                    $scope.p_SolProvContactoG.TelfFijoEXT = $scope.SolProvContacto[indice].TelfFijoEXT;
                    $scope.p_SolProvContactoG.TelfMovil = $scope.SolProvContacto[indice].TelfMovil;
                    $scope.p_SolProvContactoG.EMAIL = $scope.SolProvContacto[indice].EMAIL;
                    $scope.p_SolProvContactoG.NotElectronica = $scope.SolProvContacto[indice].NotElectronica;
                    $scope.p_SolProvContactoG.NotTransBancaria = $scope.SolProvContacto[indice].NotTransBancaria;
                    $scope.p_SolProvContactoG.pRecActas = $scope.SolProvContacto[indice].pRecActas;
                    $scope.p_SolProvContactoG.id = $scope.SolProvContacto[indice].id;

                    $scope.p_SolProvContactoG.Departamento = listaCargos[j].Departamento;
                    $scope.p_SolProvContactoG.Funcion = listaCargos[j].Funcion;
                    $scope.p_SolProvContactoG.DescDepartamento = listaCargos[j].DescDepartamento;
                    $scope.p_SolProvContactoG.DescFuncion = listaCargos[j].DescFuncion;
                    $scope.SolProvContactoG.push($scope.p_SolProvContactoG);

                }
            }


        }

        if ($scope.Grabarcontacto == "Adicionar") {
            $scope.ConfirmGrabar("1");
        }
        else { $scope.ConfirmGrabar("2"); }


        
    }

    ///Consultar lista de contactos BD
    $scope.consultaContacto = function () {
        //carga lista Contacto
        $scope.myPromise = null;
        $scope.myPromise = ModificacionProveedor.getContactoList($scope.CodSapProveedor).then(function (response) {

            if (response.data != null && response.data.length > 0) {

                $scope.SolProvContacto = [];
                $scope.ListDgAlmSeleccionados = response.data;



                $scope.GridContactos = [];
                $scope.etiTotRegistros = "";
                if (response.data.length == 0) return;
                var data = response.data;
                var index = 0;
                for (index = 0; index < data.length; index++) {
                    $scope.limpiacontacto();

                    //Buscar su ya existe contacto 
                    var nuevoContacto = true;

                    for (var idx = 0 ; idx < $scope.SolProvContacto.length; idx++) {
                        var edita = $scope.SolProvContacto[idx];
                        if (data[index].identificacion == $scope.SolProvContacto[idx].Identificacion && $scope.SolProvContacto[idx].Identificacion != "") {


                            $scope.p_SolProvCargos.Funcion = data[index].funcion;
                            $scope.p_SolProvCargos.DescFuncion = data[index].descFuncion;
                            $scope.p_SolProvCargos.Departamento = data[index].departamento;
                            $scope.p_SolProvCargos.DescDepartamento = data[index].descDepartamento;

                                edita.Cargos.push($scope.p_SolProvCargos);
                            nuevoContacto = false;
                        }
                        else {
                            if (data[index].nombre1 == $scope.SolProvContacto[idx].Nombre1 && data[index].nombre2 == $scope.SolProvContacto[idx].Nombre2
                              && data[index].apellido2 == $scope.SolProvContacto[idx].Apellido2 && data[index].apellido1 == $scope.SolProvContacto[idx].Apellido1
                                && data[index].identificacion == $scope.SolProvContacto[idx].Identificacion) {
                                nuevoContacto = false;
                            }
                        }
                    }

                    $scope.p_SolProvContacto.Id = data[index].id;
                    $scope.p_SolProvContacto.IdSolContacto = data[index].idSolContacto;
                    $scope.p_SolProvContacto.IdSolicitud = data[index].idSolicitud;
                    $scope.p_SolProvContacto.TipoIdentificacion = data[index].tipoIdentificacion;
                    $scope.p_SolProvContacto.DescTipoIdentificacion = data[index].descTipoIdentificacion;
                    $scope.p_SolProvContacto.Identificacion = data[index].identificacion;
                    $scope.p_SolProvContacto.Nombre2 = data[index].nombre2;
                    $scope.p_SolProvContacto.Nombre1 = data[index].nombre1;
                    $scope.p_SolProvContacto.Apellido2 = data[index].apellido2;
                    $scope.p_SolProvContacto.Apellido1 = data[index].apellido1;
                    $scope.p_SolProvContacto.CodSapContacto = data[index].codSapContacto;
                    $scope.p_SolProvContacto.PreFijo = data[index].preFijo;
                    $scope.p_SolProvContacto.DepCliente = data[index].depCliente;


                    $scope.p_SolProvContacto.RepLegal = data[index].repLegal;
                    $scope.p_SolProvContacto.Estado = data[index].estado;
                    $scope.p_SolProvContacto.TelfFijo = data[index].telfFijo;
                    $scope.p_SolProvContacto.TelfFijoEXT = data[index].telfFijoEXT;
                    $scope.p_SolProvContacto.TelfMovil = data[index].telfMovil;
                    $scope.p_SolProvContacto.EMAIL = data[index].email;
                    $scope.p_SolProvContacto.NotTransBancaria = data[index].notTransBancaria;
                    $scope.p_SolProvContacto.pRecActas = data[index].pRecActas;
                    $scope.p_SolProvContacto.NotElectronica = data[index].notElectronica;

                    

                    $scope.p_SolProvCargos.Funcion = data[index].funcion;
                    $scope.p_SolProvCargos.DescFuncion = data[index].descFuncion;
                    $scope.p_SolProvCargos.Departamento = data[index].departamento;
                    $scope.p_SolProvCargos.DescDepartamento = data[index].descDepartamento;

                        $scope.p_SolProvContacto.Cargos.push($scope.p_SolProvCargos);

                    if (nuevoContacto)
                        $scope.SolProvContacto.push($scope.p_SolProvContacto);
                }
                setTimeout(function () { $('#rbtPorUsuario').focus(); }, 100);


                $scope.SolProvContactoF = $scope.SolProvContacto.slice();


                $scope.SolProvContactoF = $filter('filter')($scope.SolProvContactoF, { Estado: true }, true);
                
                setTimeout(function () { $('#cargaNotificacion').focus(); }, 150);
                $scope.etiTotRegistros = $scope.SolProvContactoF.length;
            }
            else {
                $scope.MenjError = "Ocurrio un Error. ";
                $('#idMensajeError').modal('show');
            }
        },
        function (err) {
            $scope.MenjError = err.error_description;
        });

    }


    $scope.showMessage = function (tipo, mensaje) {

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

}]);


