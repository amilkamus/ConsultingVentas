app.factory('FrontendService', function($http) {
    return {
        getUbigeo: function(params) {
            return $http.post('../../getubigeo', params);
        },
        getIdentitydocument: function(params) {
            return $http.post('../../getidentitydocument', params);
        },
    };
});
