app.controller('IndicadoresController', ['$scope', 'ngAuthSettings', 'IndicadoresService', 'FileUploader', '$sce', 'authService', '$http', '$filter', function ($scope, ngAuthSettings, IndicadoresService, FileUploader, $sce, authService, $http, $filter) {
    $scope.MenjError = "";

    $scope.totalVentas = 0;
    $scope.crecimientoVentas = 0;
    $scope.porcentajeVentas = 0;
    $scope.presupuestoVentas = 0;
    $scope.facturaVentas = 0;
    $scope.peridoIni = '';
    $scope.peridoFin = '';
    $scope.signo = '';
    $scope.arrow = '';
    $scope.unida = '';
    $scope.unidb = '';
    $scope.unidc = '';
    $scope.periodo = '';

    $scope.stockCosto = 0;
    $scope.stockCostoA = 0;
    $scope.costoNeto = 0;
    $scope.costoNetoA = 0;
    $scope.stockUnidades = 0;
    $scope.stockUnidadesA = 0;
    $scope.increaseDiasInv = 0;
    $scope.increaseStockCosto = 0;
    $scope.increaseCostoNeto = 0;
    $scope.increaseStockUnidades = 0;
    $scope.diasInv = 0;
    $scope.diasInvA = 0;
    $scope.invUnid = '';
    $scope.signoDiasInv;
    $scope.signoStockCosto;
    $scope.signoStockUnidades;

    $scope.totalGlobal = 0;
    $scope.avgCobInventario = 0;
    $scope.avgCobRotacion = 0;
    $scope.coberturaInvSKU = 0;
    $scope.coberturaRotacion = 0;

    //FILTROS
    $scope.marcas = {};
    $scope.proveedores = {};
    $scope.lineaProveedorList = {};
    $scope.canalList = {};
    $scope.subCanalList = {};

    $scope.proveedores = {};

    $scope.myPromise = null;
   
    var barColors = [
        "#c5d140",
        "#ffffff",
    ];

    const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Augosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ];

    var fecIni = '';
    var fecFin = '';

    var fecIniA = '';
    var fecFinA = '';

    IndicadoresService.getMarcas("MAR").then(function (results) {
        $scope.marcas = results.data.root;
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
        $('#idMensajeError').modal('show');
    });

    IndicadoresService.getMarcas("SEG").then(function (results) {
        $scope.lineaProveedorList = results.data.root;
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
        $('#idMensajeError').modal('show');
    });

    IndicadoresService.getMarcas("CAN").then(function (results) {
        $scope.canalList = results.data.root;
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
        $('#idMensajeError').modal('show');
    });

    IndicadoresService.getMarcas("SCA").then(function (results) {
        $scope.subCanalList = results.data.root;
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
        $('#idMensajeError').modal('show');
    });

    IndicadoresService.getProveedores().then(function (results) {
        $scope.proveedores = results.data.root;
    }, function (error) {
        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
        $('#idMensajeError').modal('show');
    });
    $scope.cbCodSegmento = -1;
    $scope.cbCanal = -1;
    $scope.cbSubCanal = -1;
    $scope.cbMarca = -1;
    $scope.cbProveedor = -1;
    $scope.cbPeriodo = -1;
    $scope.cbProveedorUsr = -1;
    $scope.proveedorUsr = 'Tecnoquimicas - Genericos';
    GraficoVentas($scope.porcentajeVentas);

    GraficoCobertura(0);

    var d = new Date(2021, 11, 30);//desarrollo p<ra pruebas en produccion quitarlo
    var m_months = [monthNames[d.getMonth() - 3], monthNames[d.getMonth() - 2], monthNames[d.getMonth() - 1]];
    var dataTrend = [0, 0, 0];

    var trend = [];

    $scope.trendMonths = m_months[0] + ' ' + d.getFullYear() + ' - ' + m_months[2] + ' ' + d.getFullYear();

    var dicc_months = {};
    dicc_months[monthNames[d.getMonth() - 3]] = 0;
    dicc_months[monthNames[d.getMonth() - 2]] = 0;
    dicc_months[monthNames[d.getMonth() - 1]] = 0;


    GraficoTendencia(dicc_months, m_months);

    $scope.getColorIncrease = function (item) {
        if ($scope.signo == '+')
            return { 'color': '#006865', 'font-family': 'Lato-Regular' }
        else
            return { 'color': 'darkred', 'font-family': 'Lato-Regular' }
    }

    $scope.getColorIncreaseP = function (item) {
        if (item == '+')
            return { 'color': '#006865', 'font-family': 'Lato-Regular' }
        else
            return { 'color': 'darkred', 'font-family': 'Lato-Regular' }
    }

    $scope.Consultar = function () {

        console.log($scope.cbProveedor);
        debugger; 
        var fecha = new Date();
        var dFechaIni = new Date();
        var dFechaFin = new Date();
        var dFechaIniA = new Date();
        var dFechaFinA = new Date();

        var dFechaIniCob = new Date();
        var dFechaFinCob = new Date();

        if ($scope.cbPeriodo == 'YTD') {
            $scope.periodo = 'YTD1';
            dFechaIni = new Date(fecha.getFullYear(), 0, 1);
            dFechaFin = new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());

            dFechaIniA = new Date(fecha.getFullYear() - 1, 0, 1);
            dFechaFinA = new Date(fecha.getFullYear() - 1, fecha.getMonth(), fecha.getDate());
        }
        else if ($scope.cbPeriodo == 'MTH') {
            $scope.periodo = 'MTH1';
            dFechaIni = new Date(fecha.getFullYear(), fecha.getMonth(), 1);
            dFechaFin = new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());

            dFechaIniA = new Date(fecha.getFullYear()-1, fecha.getMonth(), 1);
            dFechaFinA = new Date(fecha.getFullYear() - 1, fecha.getMonth(), fecha.getDate());
        }
        else {
            $scope.periodo = 'MAT1';
            dFechaIni = new Date(fecha.getFullYear() - 1, fecha.getMonth(), fecha.getDate());
            dFechaFin = new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate());

            dFechaIniA = new Date(fecha.getFullYear() - 2, fecha.getMonth(), fecha.getDate());
            dFechaFinA = dFechaIni;
        }

        console.log(dFechaIni);
        console.log(dFechaFin);
        console.log(dFechaIniA);
        console.log(dFechaFinA);

        fecIni = String(dFechaIni.getFullYear()) + String(dFechaIni.getMonth() + 1).padStart(2, '0') + String(dFechaIni.getDate()).padStart(2, '0');
        fecFin = String(dFechaFin.getFullYear()) + String(dFechaFin.getMonth() + 1).padStart(2, '0') + String(dFechaFin.getDate()).padStart(2, '0');


        fecIniA = String(dFechaIniA.getFullYear()) + String(dFechaIniA.getMonth() + 1).padStart(2, '0') + String(dFechaIniA.getDate()).padStart(2, '0');
        fecFinA = String(dFechaFinA.getFullYear()) + String(dFechaFinA.getMonth() + 1).padStart(2, '0') + String(dFechaFinA.getDate()).padStart(2, '0');

        var yIni = fecIni.substring(0, 4);
        var mIni = fecIni.substring(4, 6);
        var dIni = fecIni.substring(6, 8);
        //var dFechaIni = new Date(yIni + '-' + mIni + '-' + dIni);


        $scope.peridoIni = dFechaIni.toLocaleString('default', { month: 'short' }) + ',' + yIni;


        $scope.peridoFin = dFechaFin.toLocaleString('default', { month: 'short' }) + ',' + dFechaFin.getFullYear();
               
        var dataActual = {};
        var dataAnteior = {};

        console.log(fecIni);
        console.log(fecFin);
        console.log(fecIniA);
        console.log(fecFinA);

        //calculamos para el perido actual
        IndicadoresService.getResumenVentas(
            fecIni, fecFin, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
            , $scope.cbCodSegmento, $scope.cbSubCanal
        ).then(function (results) {


            $scope.etiTotRegistros = "";

            if (results.data != undefined) {
                $scope.allNotificaciones = results.data.root[0];

                dataActual = results.data.root[0][0];

                if (results.data != undefined) {
                    //calculamos para el periodo anterior
                    IndicadoresService.getResumenVentas(
                        fecIniA, fecFinA, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
                        , $scope.cbCodSegmento, $scope.cbSubCanal
                    ).then(function (results) {


                        $scope.etiTotRegistros = "";

                        if (results.data != undefined) {
                            $scope.allNotificaciones = results.data.root[0];

                            dataAnteior = results.data.root[0][0];

                            $scope.totalVentas = (dataActual.sumaVenta).toFixed(2);
                            $scope.unida = dataActual.unida;
                            $scope.crecimientoVentas = (dataAnteior.sumaVenta).toFixed(2);
                            $scope.unidb = dataActual.unida;
                            if ($scope.crecimientoVentas < $scope.totalVentas) {
                                $scope.signo = '+';
                                $scope.arrow = '';
                            }
                            else
                                $scope.signo = '-';




                            $scope.porcentajeVentas = ((dataAnteior.sumaVenta / (dataActual.sumaVenta == 0 ? 1 : dataActual.sumaVenta)) * 100).toFixed(2);
                            $scope.presupuestoVentas = dataActual.presupuesto.toFixed(2);
                            $scope.unidc = dataActual.unida;


                            GraficoVentas($scope.porcentajeVentas);
                        }
                        if ($scope.etiTotRegistros == "0") {
                            $scope.MenjError = 'No existen datos para mostrar.';
                            $('#idMensajeInformativo').modal('show');
                        }

                    }, function (error) {
                        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
                        $('#idMensajeError').modal('show');
                    });

                }

            }
            if ($scope.etiTotRegistros == "0") {
                $scope.MenjError = 'No existen datos para mostrar.';
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });
        

        var dataActualInv = {};
        var dataAnteiorInv = {};
        //calculamos para el perido actual
        IndicadoresService.getResumenInventario(
            fecIniA, fecFinA, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
            , $scope.cbCodSegmento, $scope.cbSubCanal
        ).then(function (results) {


            $scope.etiTotRegistros = "";

            if (results.data != undefined) {
                $scope.allNotificaciones = results.data.root[0];

                dataActualInv = results.data.root[0][0];

                if (results.data != undefined) {
                    //calculamos para el periodo anterior
                    IndicadoresService.getResumenInventario(
                        fecIniA, fecFinA, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
                        , $scope.cbCodSegmento, $scope.cbSubCanal
                    ).then(function (results) {


                        $scope.etiTotRegistros = "";

                        if (results.data != undefined) {
                            $scope.allNotificaciones = results.data.root[0];

                            dataAnteiorInv = results.data.root[0][0];



                            $scope.stockCosto = dataActualInv.stockCosto.toFixed(2);
                            $scope.stockCostoA = dataAnteiorInv == undefined ? 0 : dataAnteiorInv.stockCosto.toFixed(2);
                            $scope.costoNeto = dataActualInv.costoNeto.toFixed(2);
                            $scope.costoNetoA = dataAnteiorInv == undefined ? 0 :dataAnteiorInv.costoNeto.toFixed(2);
                            $scope.stockUnidades = dataActualInv.stockUnidades.toFixed(2);
                            $scope.stockUnidadesA = dataAnteiorInv == undefined ? 0 :dataAnteiorInv.stockUnidades.toFixed(2);
                            $scope.diasInv = dataActualInv.diasInv.toFixed(2);
                            $scope.diasInvA = dataAnteiorInv == undefined ? 0 : dataAnteiorInv.diasInv.toFixed(2);

                            $scope.increaseDiasInv = dataAnteiorInv == undefined ? 0 : CalculaIncrementoInv(dataAnteiorInv.diasInv, dataActualInv.diasInv);
                            $scope.increaseStockCosto = dataAnteiorInv == undefined ? 0 : CalculaIncrementoInv(dataAnteiorInv.stockCosto, dataActualInv.stockCosto);
                            $scope.increaseCostoNeto = dataAnteiorInv == undefined ? 0 : CalculaIncrementoInv(dataAnteiorInv.costoNeto, dataActualInv.costoNeto);
                            $scope.increaseStockUnidades = dataAnteiorInv == undefined ? 0 : CalculaIncrementoInv(dataAnteiorInv.stockUnidades, dataActualInv.stockUnidades);
                            
                            $scope.invUnid = dataActualInv.unida;

                            
                            if ($scope.stockCostoA < $scope.stockCosto) 
                                $scope.signoStockCosto = '+';
                            else
                                $scope.signoStockCosto = '-';

                            if ($scope.stockUnidadesA < $scope.stockUnidades)
                                $scope.signoStockUnidades = '+';
                            else
                                $scope.signoStockUnidades = '-';

                            if ($scope.diasInvA < $scope.diasInv)
                                $scope.signoDiasInv = '+';
                            else
                                $scope.signoDiasInv = '-';
                           

                            
                        }
                        if ($scope.etiTotRegistros == "0") {
                            $scope.MenjError = 'No existen datos para mostrar.';
                            $('#idMensajeInformativo').modal('show');
                        }

                    }, function (error) {
                        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
                        $('#idMensajeError').modal('show');
                    });

                }

            }
            if ($scope.etiTotRegistros == "0") {
                $scope.MenjError = 'No existen datos para mostrar.';
                $('#idMensajeInformativo').modal('show');
            }

        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });

        var fecha_test = new Date(2021, 11, 30);//solo para probar

        var mes1 = monthNames[fecha_test.getMonth() - 1];
        var mes2 = monthNames[fecha_test.getMonth() - 2];
        var mes3 = monthNames[fecha_test.getMonth() - 3];

        var ind1 = fecha_test.getMonth();
        var ind2 = fecha_test.getMonth() - 1;
        var ind3 = fecha_test.getMonth() - 2;

        var dicc_months = {};
        var dicc_trend = {};

        dicc_months[mes1] = ind1;
        dicc_months[mes2] = ind2;
        dicc_months[mes3] = ind3;




        //calculamos la coberturas        
        //periodo 1

        var mes = dicc_months[mes1]
        var year = fecha_test.getFullYear();


        dFechaIniCob = new Date(year, mes, 1);
        dFechaFinCob = new Date(year, mes, fecha_test.getDate());

        console.log('dFechaIniCob=' + dFechaIniCob);
        console.log('dFechaFinCob=' + dFechaFinCob);

        var fec_iniCob = String(dFechaIniCob.getFullYear()) + String(dFechaIniCob.getMonth()).padStart(2, '0') + String(dFechaIniCob.getDate()).padStart(2, '0');
        var fec_finCob = String(dFechaFinCob.getFullYear()) + String(dFechaFinCob.getMonth()).padStart(2, '0') + String(dFechaFinCob.getDate()).padStart(2, '0');

        console.log('fec_iniCob=' + fec_iniCob);
        console.log('fec_finCob=' + fec_finCob);

        IndicadoresService.getResumenCobertura(
            fec_iniCob, fec_finCob, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
            , $scope.cbCodSegmento, $scope.cbSubCanal
        ).then(function (results) {


            $scope.etiTotRegistros = "";

            if (results.data != undefined) {
                $scope.allNotificaciones = results.data.root[0];



                if (results.data != undefined) {

                    var cobData = results.data.root[0][0];

                    console.log('periodo=' + cobData.periodo);

                    console.log(cobData);


                    $scope.totalGlobal = cobData.totalGlobal;
                    $scope.avgCobInventario = cobData.avgCobInventario;
                    $scope.avgCobRotacion = cobData.avgCobRotacion;
                    $scope.coberturaRotacion = cobData.coberturaRotacion.toFixed(2);

                    GraficoCobertura($scope.coberturaRotacion);


                    $scope.coberturaInvSKU = cobData.coberturaInvSKU.toFixed(2);
                    //trend.push(cobData.coberturaInvSKU.toFixed(2));
                    dicc_trend[mes1] = cobData.coberturaInvSKU.toFixed(2);

                    console.log(dicc_trend);


                    //periodo 2

                    mes = dicc_months[mes2]
                    year = fecha_test.getFullYear();


                    dFechaIniCob = new Date(year, mes, 1);
                    dFechaFinCob = new Date(year, mes, fecha_test.getDate());

                    console.log('dFechaIniCob=' + dFechaIniCob);
                    console.log('dFechaFinCob=' + dFechaFinCob);

                    var fec_iniCob = String(dFechaIniCob.getFullYear()) + String(dFechaIniCob.getMonth()).padStart(2, '0') + String(dFechaIniCob.getDate()).padStart(2, '0');
                    var fec_finCob = String(dFechaFinCob.getFullYear()) + String(dFechaFinCob.getMonth()).padStart(2, '0') + String(dFechaFinCob.getDate()).padStart(2, '0');

                    console.log('fec_iniCob=' + fec_iniCob);
                    console.log('fec_finCob=' + fec_finCob);


                    IndicadoresService.getResumenCobertura(
                        fec_iniCob, fec_finCob, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
                        , $scope.cbCodSegmento, $scope.cbSubCanal
                    ).then(function (results) {


                        $scope.etiTotRegistros = "";

                        if (results.data != undefined) {
                            $scope.allNotificaciones = results.data.root[0];



                            if (results.data != undefined) {

                                var cobData = results.data.root[0][0];

                                console.log('periodo=' + cobData.periodo);

                                console.log(cobData);



                                $scope.coberturaInvSKU = cobData.coberturaInvSKU.toFixed(2);

                                dicc_trend[mes2] = cobData.coberturaInvSKU.toFixed(2);

                                console.log(dicc_trend);


                                //periodo 3

                                mes = dicc_months[mes3]
                                year = fecha_test.getFullYear();


                                dFechaIniCob = new Date(year, mes, 1);
                                dFechaFinCob = new Date(year, mes, fecha_test.getDate());

                                console.log('dFechaIniCob=' + dFechaIniCob);
                                console.log('dFechaFinCob=' + dFechaFinCob);

                                var fec_iniCob = String(dFechaIniCob.getFullYear()) + String(dFechaIniCob.getMonth()).padStart(2, '0') + String(dFechaIniCob.getDate()).padStart(2, '0');
                                var fec_finCob = String(dFechaFinCob.getFullYear()) + String(dFechaFinCob.getMonth()).padStart(2, '0') + String(dFechaFinCob.getDate()).padStart(2, '0');

                                console.log('fec_iniCob=' + fec_iniCob);
                                console.log('fec_finCob=' + fec_finCob);

                                IndicadoresService.getResumenCobertura(
                                    fec_iniCob, fec_finCob, $scope.cbCanal, $scope.cbMarca, $scope.cbProveedor, $scope.cbProveedorUsr
                                    , $scope.cbCodSegmento, $scope.cbSubCanal
                                ).then(function (results) {


                                    $scope.etiTotRegistros = "";

                                    if (results.data != undefined) {
                                        $scope.allNotificaciones = results.data.root[0];



                                        if (results.data != undefined) {

                                            var cobData = results.data.root[0][0];

                                            console.log('periodo=' + cobData.periodo);

                                            console.log(cobData);


                                            $scope.coberturaInvSKU = cobData.coberturaInvSKU.toFixed(2);
                                            //trend.push(cobData.coberturaInvSKU.toFixed(2));
                                            dicc_trend[mes3] = cobData.coberturaInvSKU.toFixed(2);

                                            console.log(dicc_trend);


                                            var dicc_reverse = {};

                                            var properties = Object.keys(dicc_trend).reverse();
                                            properties.forEach(prop => {
                                                dicc_reverse[prop] = dicc_trend[prop];
                                                console.log(`PropertyName: ${prop}, its Value: ${dicc_trend[prop]}`)
                                            });

                                            console.log(dicc_reverse);

                                            GraficoTendencia(dicc_reverse, m_months);

                                        }

                                    }
                                    if ($scope.etiTotRegistros == "0") {
                                        $scope.MenjError = 'No existen datos para mostrar.';
                                        $('#idMensajeInformativo').modal('show');
                                    }



                                }, function (error) {
                                    $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
                                    $('#idMensajeError').modal('show');
                                });



                            }

                        }
                        if ($scope.etiTotRegistros == "0") {
                            $scope.MenjError = 'No existen datos para mostrar.';
                            $('#idMensajeInformativo').modal('show');
                        }



                    }, function (error) {
                        $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
                        $('#idMensajeError').modal('show');
                    });


                }

            }
            if ($scope.etiTotRegistros == "0") {
                $scope.MenjError = 'No existen datos para mostrar.';
                $('#idMensajeInformativo').modal('show');
            }



        }, function (error) {
            $scope.MenjError = "Se ha producido el siguiente error: " + error.error_description;
            $('#idMensajeError').modal('show');
        });
    }

    Date.isLeapYear = function (year) {
        return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
    };

    Date.getDaysInMonth = function (year, month) {
        return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
    };

    Date.prototype.isLeapYear = function () {
        return Date.isLeapYear(this.getFullYear());
    };

    Date.prototype.getDaysInMonth = function () {
        return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
    };

    Date.prototype.addMonths = function (value) {
        var n = this.getDate();
        this.setDate(1);
        this.setMonth(this.getMonth() + value);
        this.setDate(Math.min(n, this.getDaysInMonth()));
        return this;
    };

    function addYears(date, years) {
        date.setFullYear(date.getFullYear() + years);

        return date;
    }

    function getUnid(monto) {
        if (monto >= 1000 && monto < 10000)
            return 'k'
        else if (monto >= 10000 && monto < 100000)
            return 'k'
        else if (monto >= 100000 && monto < 1000000)
            return 'k'
        else if (monto >= 1000000 && monto < 10000000)
            return 'M'
        else if (monto >= 10000000 && monto < 100000000)
            return 'M'
        else if (monto >= 100000000 && monto < 1000000000)
            return 'M'
        else if (monto >= 1000000000 && monto < 10000000000)
            return 'B'
        else
            return '';
    }

    function GraficoVentas(porcentaje) {
        const ctx = document.getElementById('myChart');

        var xValues = ["Si", "NO"];
        var yValues = [porcentaje, 100 - porcentaje];

        new Chart("myChart", {
            type: "doughnut",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    borderColor: '#c5d140',
                    data: yValues
                }]
            },
            options: {
                title: {
                    display: true,
                    text: "Cumplimiento",
                    fontFamily: "Lato-Regular" // right here

                },
                legend: {
                    display: false
                }
            },
            //plugins: [centerText]
        });
    }

    function GraficoCobertura(porcentaje) {

        const ctxCover = document.getElementById('myChartCover');
        var xValues = ["Si", "NO"];
        var yValues = [porcentaje, 100 - porcentaje];

        new Chart("myChartCover", {
            type: "doughnut",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    borderColor: '#c5d140',
                    data: yValues//[50, 50]
                }]
            },
            options: {
                title: {
                    display: true,
                    text: "Cobertura",
                    fontFamily: "Lato-Regular"
                },
                legend: {
                    display: false
                }

            },
            //plugins: [centerText]
        });
    }

    var chart_trend;

    function GraficoTendencia(dataTrend, dataMonths) {
        const ctxTrend = document.getElementById('myChartTrend');

        if (chart_trend != null && chart_trend != undefined)
            chart_trend.destroy();

        chart_trend = new Chart("myChartTrend", {
            type: "line",
            data: {
                labels: Object.keys(dataTrend),// dataMonths,//["Mayo", "Junio", "Julio"],
                datasets: [{
                    backgroundColor: [
                        "#006865",
                        "#006865",
                        "#006865"
                    ],
                    borderColor: '#5ab146',
                    data: Object.values(dataTrend),//[200, 225, 250],
                    fill: false,
                    tension: 0.1
                }]
            },
            options: {
                legend: {
                    display: false,
                    fontFamily: "Lato-Regular"
                }

            }
        });


    }

    function CalculaIncrementoInv(ValorUno, ValorDos) {
        var result = 0;

        if (ValorUno === undefined || ValorUno == 0 || ValorUno == "0") {
            return 0;
        }
        else {
            result = ((ValorUno / ValorDos) * 100).toFixed(2)
        }
        return parseFloat(result); // Devuelve el resultado como un número decimal.

    }




}]);