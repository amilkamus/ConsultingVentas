app.controller('companiaController', ['$rootScope', '$scope', 'companiaService', function ($rootScope, $scope, companiaService) {
    
    $scope.model = {};
    $scope.elementos = { lista: [] };

    $scope.desabilitarCampos = true;

    $scope.guardarCompania = function () {
        var params = {
            companiaViewModel: $scope.model
        };
        if ($scope.desabilitarCampos == false) {
            companiaService.guardarCompania(params).then(function (data) {
                if (data.data.result_description) {
                    $scope.messageSuccess(data.data.result_description);
                    $scope.desabilitarCampos = true;
                } else {
                    $scope.messageError(data.data.error);
                }
            });
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'Tiene que estar activadas las casillas para poder guardar cambios.'
            }, { type: 'warning', z_index: 2000 });
        }
    }

    $scope.listarCompanias = function () {
        companiaService.listarCompania().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
                $scope.model.idCompania = $scope.elementos.lista[0].idCompania;
                $scope.model.razonSocial = data.data[0].razonSocial;
                $scope.model.ruc = data.data[0].ruc;
                $scope.model.domicilioFiscal = data.data[0].domicilioFiscal;
                $scope.model.correo = data.data[0].correo;

                $scope.model.idTitular = $scope.elementos.lista[0].idTitular;
                $scope.model.titularNombre = data.data[0].titularNombre;
                $scope.model.titularApellidos = data.data[0].titularApellidos;
                $scope.model.titularCorreo = data.data[0].titularCorreo;
                $scope.model.titularTelefono = data.data[0].titularTelefono;
                $scope.model.titularCargo = data.data[0].titularCargo;

                $scope.model.idContacto = $scope.elementos.lista[0].idContacto;
                $scope.model.contactoNombre = data.data[0].contactoNombre;
                $scope.model.contactoApellidos = data.data[0].contactoApellidos;
                $scope.model.contactoCorreo = data.data[0].contactoCorreo;
                $scope.model.contactoTelefono = data.data[0].contactoTelefono;
                $scope.model.contactoCargo = data.data[0].contactoCargo;
            } else {
                $scope.elementos.lista = data.data;
            }            
        });
    }
    
    $scope.editarCompania = function () {
        $scope.desabilitarCampos = false;
    }

    $scope.listarCompanias();

}]);