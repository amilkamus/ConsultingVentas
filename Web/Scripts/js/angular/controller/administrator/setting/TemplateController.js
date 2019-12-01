app.controller('TemplateController', ['$rootScope', '$scope', 'blockUI', 'TemplateService', function($rootScope, $scope, blockUI, TemplateService) {
	$scope.page = {};
	$scope.typeAllowed = ['image/png','image/jpeg','image/jpg'];

    $scope.getDataTemplate = function(){
		$scope.page=undefined;$scope.page={};
		TemplateService.getDataTemplate().then(function(data){
			if(data.data){
				for (let i = 0, n = data.data.length; i < n; i++) {
					var arraySplit = data.data[i].split('/');
					if(arraySplit[2].toString()=='student')
						$scope.page.student =  data.data[i];
					if(arraySplit[2].toString()=='trainer')
						$scope.page.trainer =  data.data[i];
				}
				$scope.random = Math.floor((Math.random() * 100000) + 1);
			}else{
				$.notify({
					icon: 'fa fa-exclamation-triangle',
					title: 'Error!',
					message: 'Hubo problemas al obtener cierta información importante!',
				},{type: 'danger'});
			}
		});
    };

    $scope.selecttype = function(type){
		$scope.type = type;
	};
	
	$scope.upload = function(){
		var params = new FormData();
		var files = document.getElementById($scope.type).files[0];
		var ext = files.name.split('.');
		params.append('file',files,$scope.type+'.'+ext[ext.length-1]);
		if($scope.typeAllowed.indexOf(files['type'])>=0){
			TemplateService.uploadTemplate(params).then(function(data){
				if(data.data==="true"){
					$.notify({
						icon: 'fa fa-exclamation-triangle',
						title: 'Satisfactorio!',
						message: 'Se cargó la Plantilla Correctamente!',
					},{type: 'success'});
					document.getElementById($scope.type).value = '';
					$scope.getDataTemplate();
					$scope.random = Math.floor((Math.random() * 100000) + 1);
				}else{
					$.notify({
						icon: 'fa fa-exclamation-triangle',
						title: 'Alerta!',
						message: 'Ocurrió un error al subir la Plantilla',
					},{type: 'danger'});
				}
			});
		}else{
			$.notify({
				icon: 'fa fa-exclamation-triangle',
				title: 'Error!',
				message: 'Solo se admite archivos de tipo Imágen JPG-JPEG-PNG!',
			},{type: 'danger'});
		}
	};

    $scope.uploadFile = function()
	{
		if($scope.type=='trainer'){
			if(document.getElementById('trainer').value != ""){
				$scope.upload();
			}else{
				$.notify({
					icon: 'fa fa-exclamation-triangle',
					title: 'Error!',
					message: 'Deber cargar un archivo para subir!',
				},{type: 'danger'});
			}
		}
		if($scope.type=='student'){
			if(document.getElementById('student').value != ""){
				$scope.upload();
			}else{
				$.notify({
					icon: 'fa fa-exclamation-triangle',
					title: 'Error!',
					message: 'Deber cargar un archivo para subir!',
				},{type: 'danger'});
			}
		}
    };
    
    $scope.init = function()
	{
		$scope.getDataTemplate();
	}

	$scope.init();
}]);