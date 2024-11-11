'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};


    //var _authData = {
    //    isAuth: false,
    //    userName: '',
    //    userRetreived: false,
    //    firstName: '',
    //    lastName: '',
    //    email: '',
    //    roles: []
    //};

    var _authentication = {
        isAuth: false,
        ruc:"",
        userName: "",
        useRefreshTokens: false,
        userRetreived: false,
        firstName: '',
        lastName: '',
        email: '',
        roles: [],
        tokenUser: "",
        usernamefrm: "",
        IdParticipante: "",
        NombreParticipante: "",
        IdentParticipante: "",
        Estado: "",
        PS_Token: "",
        EsClaveNuevo: "",
        EsClaveCambio: "",
        EsClaveBloqueo: "",
        CargoEmpleado: ""

    };

    
    


    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        // var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var data = "grant_type=password&ruc=" + loginData.ruc + "&userName=" + loginData.usernamefrm + "&password=" + loginData.password + "&tokenUser=" + loginData.tokenUser + "&sl=" + loginData.sl;

      
        loginData.useRefreshTokens = true;

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        var terminanombre=0;

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, ruc: loginData.ruc, userName: loginData.usernamefrm, NombreParticipante: response.NombreParticipante, IdParticipante: response.IdParticipante, IdentParticipante: response.IdentParticipante, Estado: response.Estado, PS_Token: response.PS_Token, EsClaveNuevo: response.EsClaveNuevo, EsClaveCambio: response.EsClaveCambio, EsClaveBloqueo: response.EsClaveBloqueo, refreshToken: response.refresh_token, useRefreshTokens: true, tokenUser: response.tokenUser, sl: loginData.sl, CodSAP: response.CodSAP, CorreoE: response.CorreoE, Celular: response.Celular, EsAdmin: response.EsAdmin, Usuario: response.Usuario , CargoEmpleado : response.CargoEmpleado });
            }
            else {
                localStorageService.set('authorizationData', { token: response.access_token, ruc: loginData.ruc, userName: loginData.usernamefrm, NombreParticipante: response.NombreParticipante, IdParticipante: response.IdParticipante, IdentParticipante: response.IdentParticipante, Estado: response.Estado, PS_Token: response.PS_Token, EsClaveNuevo: response.EsClaveNuevo, EsClaveCambio: response.EsClaveCambio, EsClaveBloqueo: response.EsClaveBloqueo, refreshToken: "", useRefreshTokens: false, tokenUser: response.tokenUser, sl: loginData.sl, CodSAP: response.CodSAP, CorreoE: response.CorreoE, Celular: response.Celular, EsAdmin: response.EsAdmin, Usuario: response.Usuario, CargoEmpleado: response.CargoEmpleado });
            }

            _authentication.isAuth = true;
            _authentication.ruc = loginData.ruc;
            //_authentication.userName = loginData.usernamefrm;
            _authentication.userName = response.IdentParticipante;
            
            _authentication.useRefreshTokens = loginData.useRefreshTokens;
            _authentication.tokenUser = response.tokenUser;
            _authentication.sl = loginData.sl;
            _authentication.IdParticipante = response.IdParticipante;
            _authentication.NombreParticipante = response.NombreParticipante;
            _authentication.IdentParticipante = response.IdentParticipante;
            _authentication.Estado = response.Estado;
            _authentication.PS_Token = response.PS_Token;
            _authentication.EsClaveNuevo = response.EsClaveNuevo;
            _authentication.EsClaveCambio = response.EsClaveCambio;
            _authentication.EsClaveBloqueo = response.EsClaveBloqueo

            
      
            _authentication.CodSAP = response.CodSAP;
            _authentication.CorreoE = response.CorreoE;
            _authentication.Celular = response.Celular;
            _authentication.EsAdmin = response.EsAdmin;
            _authentication.Usuario = response.Usuario;
            _authentication.CargoEmpleado = response.CargoEmpleado;
            

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

   

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.ruc = "";
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;
        _authentication.tokenUser = "";
        _authentication.sl = "";

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.ruc = authData.ruc;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
            _authentication.tokenUser = authData.tokenUser;
            _authentication.NombreParticipante = authData.NombreParticipante;
            _authentication.sl = authData.sl;
            _authentication.IdParticipante = authData.IdParticipante;
            _authentication.IdentParticipante = authData.IdentParticipante;
            _authentication.Estado = authData.Estado;
            _authentication.PS_Token = authData.PS_Token;
            _authentication.EsClaveNuevo = authData.EsClaveNuevo;
            _authentication.EsClaveCambio = authData.EsClaveCambio;
            _authentication.EsClaveBloqueo = authData.EsClaveBloqueo;

            _authentication.CodSAP = authData.CodSAP;
            _authentication.CorreoE = authData.CorreoE;
            _authentication.Celular = authData.Celular;
            _authentication.EsAdmin = authData.EsAdmin;
            _authentication.Usuario = authData.Usuario;
            _authentication.CargoEmpleado = response.CargoEmpleado;

        

        }

    };

    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, UserName: response.username, refreshToken: response.refresh_token, useRefreshTokens: true, tokenUser: response.tokenUser });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false, tokenUser: response.tokenUser });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.CargoEmpleado = response.CargoEmpleado;
            _authentication.useRefreshTokens = false;
            _authentication.tokenUser = response.tokenUser;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.CargoEmpleado = response.CargoEmpleado;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}]);