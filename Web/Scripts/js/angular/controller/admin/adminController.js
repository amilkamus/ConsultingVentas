app.controller('rolController', ['$rootScope', '$scope', 'adminService', function ($rootScope, $scope, adminService) {
    $scope.model = {};
    $scope.elementos = { lista: [] };

    //$scope.init = function () {
    //    $scope.model.Name = "";
    //}

    // Guardar
    $scope.guardarRol = function () {
        var contador = 0;
        var params = {
            modelRole: $scope.model
        };

        if ($scope.model.Name != "") {
            if ($scope.model.Id == null ) {
                //console.log("El Id es nuevo");
                for (var i = 0; i < $scope.elementos.lista.length; i++) {
                    if ($scope.elementos.lista[i].Name == $scope.model.Name) {
                        contador = contador + 1;
                    }
                }
                if (contador == 0) {
                    adminService.guardarRol(params).then(function (data) {
                        if (data.data.result_description) {
                            $scope.messageSuccess(data.data.result_description);
                            $scope.model = {};
                            $scope.listarRoles();
                        } else {
                            $scope.messageError(data.data.error);
                        }
                    });
                } else {
                    $.notify({
                        icon: 'fa fa-exclamation-circle',
                        title: 'Alerta !',
                        message: 'El ROL ya existe, ingrese uno nuevo'
                    }, { type: 'warning', z_index: 2000 });
                    $scope.model = {};
                }
            } else {
                adminService.guardarRol(params).then(function (data) {
                    if (data.data.result_description) {
                        $scope.messageSuccess(data.data.result_description);
                        $scope.model = {};
                        $scope.listarRoles();
                    } else {
                        $scope.messageError(data.data.error);
                    }
                });
            }
        } else {
            $scope.model = {};
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'Ingresar datos a los campos'
            }, { type: 'warning', z_index: 2000 });
        }
        $('#modal-editarRol').modal('hide')
    }
    //Fin

    /* ---------------------------------------- */

    //Listar
    $scope.listarRoles = function () {
        adminService.listarRoles().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }
    //Fin

    // Eliminar/Editar
    $scope.cargarModal = function (rol) {
        $scope.Id = rol.Id;
        $scope.tmpRol = rol;
    }
    $scope.eliminarRol = function () {
        if ($scope.tmpRol.Name != "ADMINISTRADOR") {
            adminService.eliminarRol({ Id: $scope.Id }).then(function (data) {
                if (data.data.result_description) {
                    if (data.data.cod == 1) {
                        $scope.messageSuccess(data.data.result_description);
                        $scope.listarRoles();
                    }
                    if (data.data.cod == 2) {
                        $.notify({
                            icon: 'fa fa-exclamation-circle',
                            title: 'Alerta !',
                            message: data.data.result_description
                        }, { type: 'warning', z_index: 2000 });
                        $scope.listarRoles();
                    }
                } else {
                    $scope.messageError(data.data.error);
                }
            });
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta !',
                message: 'No se puede borrar el Rol: ' + $scope.tmpRol.Name
            }, { type: 'warning', z_index: 2000 });
            $scope.model = {};
        }
    }

    $scope.editarRol = function (rol) {
        $scope.model = rol;
    }
    //Fin

    $scope.listarRoles();
    //$scope.init();
}]);

app.controller('bandejaUsuariosController', ['$rootScope', '$scope', 'adminService', function ($rootScope, $scope, adminService) {
    $scope.elementos = { lista: [] };
    $scope.UserName = {};

    $scope.listarUsuarios = function () {
        adminService.listarUsuarios().then(function (data) {
            if (data.data) {
                $scope.elementos.lista = data.data;
            } else {
                $scope.elementos.lista = data.data;
            }
        });
    }

    $scope.cargarModal = function (UserId) {
        $scope.UserId = UserId;
    }
    $scope.eliminar = function () {
        adminService.eliminarUsuario({ UserId: $scope.UserId }).then(function (data) {
            if (data.data.result_description) {
                $scope.messageSuccess(data.data.result_description);
                $scope.listarUsuarios();
            } else {
                $scope.messageError(data.data.error);
            }

        });
    }

    $scope.listarUsuarios();
}]);