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

        rolAdm: "",
        nomEmpresa: "",
        rucEmpresa:"",

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
        bandera: "0",
        sociedad: "",
        esEtiqueta: true

    };

    
    


    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();
        
        return $http.post(serviceBase + 'api/account/register', registration).success(function (response) {
            return response;
        },   function (error) {
            var errors = [];
            for (var key in error.data.modelState) {
                for (var i = 0; i < error.data.modelState[key].length; i++) {
                    errors.push(error.data.modelState[key][i]);
                }
            }
            $scope.showMessage('E', "Error en comunicación: " + errors.join(' '));
        });
    };

    var _login = function (loginData) {

        // var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        var data = "grant_type=password&ruc=" + loginData.ruc + "&userName=" + loginData.usernamefrm + "&password=" + loginData.password + "&tokenUser=" + loginData.tokenUser + "&sl=" + loginData.sl + "&sociedad=" + loginData.Sociedad + "&apl=1";
      
        loginData.useRefreshTokens = true;

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', { token: response.access_token, ruc: loginData.ruc, userName: loginData.usernamefrm, NombreParticipante: response.NombreParticipante, IdParticipante: response.IdParticipante, IdentParticipante: response.IdentParticipante, Estado: response.Estado, PS_Token: response.PS_Token, EsClaveNuevo: response.EsClaveNuevo, EsClaveCambio: response.EsClaveCambio, EsClaveBloqueo: response.EsClaveBloqueo, refreshToken: response.refresh_token, useRefreshTokens: true, tokenUser: response.tokenUser, sl: loginData.sl, CodSAP: response.CodSAP, CorreoE: response.CorreoE, Celular: response.Celular, EsAdmin: response.EsAdmin, Usuario: response.Usuario, Roles: response.roles, rolAdm: response.rolAdm, nomEmpresa: response.nomEmpresa, rucEmpresa:response.rucEmpresa , esEtiqueta:response.esEtiqueta,sociedad: response.sociedad });
            }
            else {
                localStorageService.set('authorizationData', { token: response.access_token, ruc: loginData.ruc, userName: loginData.usernamefrm, NombreParticipante: response.NombreParticipante, IdParticipante: response.IdParticipante, IdentParticipante: response.IdentParticipante, Estado: response.Estado, PS_Token: response.PS_Token, EsClaveNuevo: response.EsClaveNuevo, EsClaveCambio: response.EsClaveCambio, EsClaveBloqueo: response.EsClaveBloqueo, refreshToken: "", useRefreshTokens: false, tokenUser: response.tokenUser, sl: loginData.sl, CodSAP: response.CodSAP, CorreoE: response.CorreoE, Celular: response.Celular, EsAdmin: response.EsAdmin, Usuario: response.Usuario, rolAdm: response.rolAdm, nomEmpresa: response.nomEmpresa, rucEmpresa: response.rucEmpresa, esEtiqueta: response.esEtiqueta, sociedad: response.sociedad });
            }
          
            _authentication.isAuth = true;
            _authentication.ruc = loginData.ruc;
            _authentication.userName = loginData.usernamefrm;
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
            _authentication.EsClaveBloqueo = response.EsClaveBloqueo;

            _authentication.CodSAP = response.CodSAP;
            _authentication.CorreoE = response.CorreoE;
            _authentication.Celular = response.Celular;
            _authentication.EsAdmin = response.EsAdmin;
            _authentication.Usuario = response.Usuario;

            _authentication.rolAdm = response.rolAdm;
            _authentication.nomEmpresa = response.nomEmpresa;
            _authentication.rucEmpresa = response.rucEmpresa;
            _authentication.esEtiqueta = response.esEtiqueta;
            //response.esEtiqueta;
           
            var rol = response.roles.split(",");
            _authentication.roles = rol;
            _authentication.sociedad = response.sociedad;

            deferred.resolve(response);

        }).error(function (error, status) {
            _logOut();
            deferred.reject(error);
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


            _authentication.rucEmpresa = authData.rucEmpresa;
            _authentication.nomEmpresa = authData.nomEmpresa;
            _authentication.rolAdm = authData.rolAdm;

            _authentication.CodSAP = authData.CodSAP;

            _authentication.EsAdmin = authData.EsAdmin;
            _authentication.Usuario = authData.Usuario;

            _authentication.esEtiqueta = authData.esEtiqueta;
            //authData.esEtiqueta;
            _authentication.sociedad = authData.sociedad;
            if (authData.Roles != undefined) {
                var roltmp = authData.Roles;
                var rol = roltmp.split(',');
                _authentication.roles = rol;
            }
            if(_authentication.bandera=="0")
            {
                _authentication.CorreoE = authData.CorreoE;
                _authentication.Celular = authData.Celular;
            }
       

        }

    };

    var _correo = function (bandera, correo, celular) {
        _authentication.bandera = bandera;
        _authentication.CorreoE = correo;
        _authentication.Celular = celular;
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
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

     
    var _forgotPassword = function (data) {

        return $http.post(serviceBase + 'api/account/forgotPassword', data).then(function (response) {
            return response;
          });
 
    }

    var _resetPassword = function (data) {

        return $http.post(serviceBase + 'api/account/resetPassword', data).then(function (response) {
            return response;
        });

    }

    var _ConfirmEmailRuc = function (data) {

        return $http.post(serviceBase + 'api/account/ConfirmEmail', data).then(function (response) {
            return response;
        });

    }

  
    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;
    authServiceFactory.correo = _correo;


    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    authServiceFactory.forgotPassword = _forgotPassword;
    authServiceFactory.resetPassword = _resetPassword;
    authServiceFactory.confirmEmailRuc = _ConfirmEmailRuc;
     

    return authServiceFactory;
}]);