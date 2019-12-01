app.controller('InscribedController', ['$rootScope', '$scope', 'InscribedService', 'CommonService', function($rootScope, $scope, InscribedService, CommonService) {
    $scope.clone = {};
    $scope.page = { inscribed: [] };
    $scope.objectSelected = {};
    $scope.basics = {postponedpays:[]};
    $scope.typeAllowed = ['image/png','image/jpeg','image/jpg'];
    $scope.random = Math.floor((Math.random() * 100000) + 1);

    $scope.search = function(inputSearch) {
        if(inputSearch!=undefined && inputSearch!=''){
            var out = [];
            out = $rootScope.searchFilter($scope.page.inscribed, inputSearch);
            $scope.message = '<p class="badge badge-success" style="font-size: 14px">Mostrando Registros: ' + out.length + ' de ' + $scope.page.inscribed.length + '</p>';
            return out;
        }
    };

    $scope.validateForm = function(form) {
        if (form.$valid) {
            showModal('#showQuestion');
        } else {
            $.notify({
                icon: 'fa fa-exclamation-circle',
                title: 'Alerta!',
                message: 'Error de Validación Asegurese de Ingresar todos los campos (*)',
            }, { type: 'danger', z_index: 2000 });
        }
    };

    $scope.exist = function(data) {
        if ($scope.page.inscribed.length > 0) {
            for (let i = 0, n = $scope.page.inscribed.length; i < n; i++) {
                if ($scope.page.inscribed[i].id == data.id) { return true };
            }
            return false;
        }
        return false;
    };

    $scope.changePageCustom = function(number) {
        if ((number / $rootScope.pager.pageDivide >= 1 && (number % $rootScope.pager.pageDivide == 0 || number % 6 < $rootScope.pager.pageDivide))) {
            var n = parseInt(number / $rootScope.pager.pageDivide);
            if (n > 1) {
                for (let i = 0; i < n; i++) {
                    $rootScope.pager.currentPage = $rootScope.pager.pageDivide;
                    $rootScope.pager.pageDivide += 5;
                    $scope.getAllCourse();
                }
                $rootScope.pager.currentPage = number;
                $rootScope.pager.pageDivide = number + 5;
            } else {
                $rootScope.pager.pageDivide += 5;
                $scope.getAllCourse();
            }
        }
    };
    
    $scope.getAllBasic = function() {
        InscribedService.getActivityGroup().then(function(data) {
            $scope.basics.course = data.data;
        });
    };

    $scope.getPostponedPays = function(student,group){
        if(student!=undefined && group!=undefined && group!=''){
            InscribedService.getPostponedPays({student:student,group:group}).then(function(data) {
                $scope.basics.postponedpays = data.data;
            });
        }
    };

    $scope.getStudents = function(document){
        if(document!=undefined){
            InscribedService.getStudents({document:document}).then(function(data) {
                $scope.basics.students = data.data;
            });
        }
    };

    $scope.loadGroup = function(course){
        if(course!=undefined && course!=''){
            for (let i = 0,n=$scope.basics.course.length; i < n; i++) {
                if($scope.basics.course[i].id == course){
                    $scope.course = $scope.basics.course[i];
                    $scope.basics.group = $scope.course.group;
                    break;
                }
            }
        }
        $scope.setToPay();
    };

    $scope.toDebt = function(state=false){
        $scope.tmp.debt = state!=true?$scope.tmp.topay:parseInt(parseInt($scope.tmp.topay)-parseInt($scope.basics.postponedpays.pay_amount));
        $scope.objectSelected.old_pay = state!=true?null:$scope.basics.postponedpays;
    };

    $scope.setToPay = function(){
        if($scope.tmp.course != undefined && $scope.tmp.course!='' && $scope.objectSelected.student!=undefined &&
            $scope.objectSelected.student.role_id!=undefined && $scope.objectSelected.student.role_id!=''){
            for (let j = 0,n= $scope.course.cost.length; j < n; j++) {
                if($scope.course.cost[j].role_id.toString() == $scope.objectSelected.student.role_id.toString()){
                    $scope.tmp.topay =  $scope.course.cost[j].cost;
                    $scope.objectSelected.to_pay = $scope.course.cost[j].cost;
                    break;
                }
                $scope.tmp.topay = 'No Establecida';
            }
            $scope.toDebt($scope.tmp.usepay);
        }
    };

    $scope.setStudent = function(student){
        $scope.objectSelected.student = student;
        $scope.objectSelected.student_id = student.id;
        $scope.setToPay();
        $scope.getPostponedPays($scope.objectSelected.student_id,$scope.objectSelected.group_id);
    };

    $scope.setCourse = function(group){
        if(group!=undefined && group!=''){
            for (let i = 0,n=$scope.basics.group.length; i < n; i++) {
                if($scope.basics.group[i].id.toString()==group.toString()){
                    $scope.objectSelected.group = $scope.basics.group[i];
                    $scope.objectSelected.group_id = $scope.basics.group[i].id;
                    $scope.getPostponedPays($scope.objectSelected.student_id,$scope.objectSelected.group_id);
                    break;
                }
            }
        }        
    };

    $scope.select = function(input, type) {
        $scope.objectSelected = undefined;
        $scope.objectSelected = {old_pay:{}};
        $scope.type = type;
        if (input != undefined) {
            $scope.objectSelected = input;
            var group = input.group_id.toString();
            $scope.tmp.course = $scope.objectSelected.group_activity.activity_id.toString();
            $scope.loadGroup(input.group_activity.activity_id);
            $('#course').val($scope.objectSelected.group_activity.activity_id.toString()).trigger('change.select2');
            setTimeout(function() {
                $('#group').val(group.toString()).trigger('change.select2');
            }, 1000);
            $scope.setCourse(input.group_id);
        } else {
            $scope.objectSelected.id = 0;
            $('#course').val(undefined).trigger('change.select2');
            $('#group').val(undefined).trigger('change.select2');
        }
    };

    $scope.getAllPays = function(){
        $scope.objectSelected.pays = undefined;
        $scope.objectSelected.pays = [];
        var params = {inscription:$scope.objectSelected.id};
        InscribedService.getAllPays(params).then(function(data){
            if(data.data!='null')
                $scope.objectSelected.pays =  data.data; 
		});
    };

    $scope.toSavePay = function(){
        var pay = $scope.pay;
        pay.inscription_id = $scope.objectSelected.id;
        var params = {
            pay: pay
        };
        InscribedService.savePay(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                },{type: 'success', z_index: 2000 });
                $scope.getAllPays();
                $scope.cancelPay();
                $scope.objectSelected.pay_amount = parseInt($scope.objectSelected.pay_amount) + 
                parseInt(pay.pay_amount);
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger', z_index: 2000 });
            }
        });
    };

    $scope.savePay = function(){
        var files = document.getElementById('newvoucher').files[0];
        $scope.objectSelected.pay = $scope.pay;
        if(files==undefined){
            $scope.toSavePay();
        }else{
            if($scope.typeAllowed.indexOf(files['type'])>=0){
                $scope.upload('newvoucher');
            }else{
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Error!',
                    message: 'Solo se admite archivos de tipo Imágen JPG-JPEG-PNG!',
                },{type: 'danger'});
            }
        }
    };

    $scope.cancelPay = function(){
        $scope.pay = undefined;
        document.getElementById('newvoucher').value = '';
    };

    $scope.selectVoucher = function(voucher){
        $scope.img_voucher = undefined;
        $scope.random = Math.floor((Math.random() * 100000) + 1);
        $scope.img_voucher = 'voucher/'+$scope.objectSelected.student.code+'/'+voucher+'?'+$scope.random;
    };

    $scope.getAllInscribed = function() {
        $scope.clone = {};
        $scope.page.inscribed = [];
        var params = { start: $rootScope.pager.currentPage };
        InscribedService.getAllInscribed(params).then(function(data) {
            if (data.data.count>0) {
                for (let i = 0, n = data.data.data.length; i < n; i++) {
                    if (!$scope.exist(data.data.data[i]))
                        $scope.page.inscribed.push(data.data.data[i]);
                }
                $rootScope.pager.total = data.data.count;
                $rootScope.pager.pages = [];
                $rootScope.numberOfPages(data.data.count);
                $scope.clone = Object.assign({}, $scope.page);
            } else
                $scope.message = '<p class="badge badge-danger" style="font-size: 14px;margin: 0;">No se encontro Registro alguno (^_^)!!..</p>';
        });
    };

    $scope.upload = function(id='voucher'){
		var params = new FormData();
		var files = document.getElementById(id).files[0];
		var ext = files.name.split('.');
        params.append('file',files,$scope.objectSelected.student.code+'_'+
            $scope.objectSelected.student_id+'_'+$scope.objectSelected.group_id+'.'+ext[ext.length-1]);
		InscribedService.uploadVoucher(params).then(function(data){
            if(data.data!='false'){
                $scope.objectSelected.pay.voucher_photo = data.data; 
                $scope.type=='pay'?$scope.toSavePay():$scope.toSaveUpdate();
                document.getElementById(id).value = '';
            }
		});
    };
    
    $scope.saveUpdate = function(){
        var files = document.getElementById('voucher').files[0];
        if(files==undefined){
            $scope.toSaveUpdate();
        }else{
            if($scope.typeAllowed.indexOf(files['type'])>=0){
                $scope.upload();
            }else{
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Error!',
                    message: 'Solo se admite archivos de tipo Imágen JPG-JPEG-PNG!',
                },{type: 'danger'});
            }
        }
    };

    $scope.toSaveUpdate = function() {
        $scope.objectSelected.group.trainers=null;
        $scope.objectSelected.group.inscription=null;
        $scope.objectSelected.group.location=null;
        var params = {
            inscription: $scope.objectSelected
        };
        InscribedService.saveUpdate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                },{type: 'success'});
                $scope.getAllInscribed();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta!',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.postergate = function() {
        var params = {
            inscription: $scope.objectSelected
        };
        InscribedService.postergate(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllInscribed();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta! <br>',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.delete = function() {
        var params = {
            exam: $scope.objectSelected
        };
        InscribedService.delete(params).then(function(data) {
            if (data.data.status === "true") {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Satisfactorio!',
                    message: data.data.message,
                }, { type: 'success' });
                $scope.getAllInscribed();
            } else {
                $.notify({
                    icon: 'fa fa-exclamation-triangle',
                    title: 'Alerta! <br>',
                    message: data.data.message,
                }, { type: 'danger' });
            }
        });
    };

    $scope.action = function() {
        if ($scope.type == 'save' || $scope.type == 'edit')
            $scope.saveUpdate();
        else if ($scope.type == 'postergate')
            $scope.postergate();
        else if ($scope.type == 'delete')
            $scope.delete();
    };

    $scope.cancel = function() {
        $scope.page = Object.assign({}, $scope.clone);
    };

    $scope.init = function() {
        $scope.getAllBasic();
        $scope.getAllInscribed();
    };

    $scope.init();
}]);