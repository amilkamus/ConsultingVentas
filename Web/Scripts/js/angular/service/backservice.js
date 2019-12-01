app.factory('CommonService', function($http) {
    return {
        getLocation: function(params) {
            return $http.post('../../getlocation', params);
        },
        getUbigeo: function(params) {
            return $http.post('../../getubigeo', params);
        },
        getPersonalData: function(params) {
            return $http.post('../../getpersonaldata', params);
        },
        getAllRoleApp: function(params) {
            return $http.post('../../getroleapp', params);
        }
    };
});