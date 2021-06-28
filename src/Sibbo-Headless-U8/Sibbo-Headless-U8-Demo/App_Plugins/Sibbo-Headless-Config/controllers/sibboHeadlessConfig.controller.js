angular.module('umbraco').controller('sibboHeadlessConfigController', function ($http, $scope, $q, notificationsService, angularHelper, entityResource, contentResource) {
    $scope.model = {};
    $scope.propertyTypes = [];
    $scope.contentPicker = {
        view: 'contentPicker',
        config: {
            minNumber: 0,
            maxNumber: 1
        },
        value: 0
    };
    $scope.showSaveButton = false;
    $scope.isLoading = true;
    $scope.propertyTypeDoneLoading = true;
    $scope.saveButtonDisabled = false;

    function initConfig() {
        $scope.getCurrentFooterSettings(0).then(function (data) {
            $scope.propertyTypes = data.properties;
            $scope.contentPicker.value = data.ids.join();
            $scope.isLoading = false;
        });
    };

    $scope.getCurrentFooterSettings = function (val) {
        var dfrd = $q.defer();
        $http.get("/sibbo-headless-backend/getfooterproperties").then(function (result) {
            dfrd.resolve(result.data);
        });
        return dfrd.promise;
    }

    initConfig();

    $scope.$watch('contentPicker.value', function (newValue, oldValue) {
        if (oldValue != "") {
            $scope.propertyTypeDoneLoading = false;
        }
        if (newValue !== oldValue && newValue !== "" && newValue !== 0 && !$scope.propertyTypeDoneLoading) {
            getContent();
            $scope.propertyTypeDoneLoading = true;
        }
        else if (newValue === "") {
            $scope.propertyTypes = [];
        }
    });

    $scope.$watch('propertyTypes', function (newValue, oldValue) {
        if (newValue !== oldValue) {
            $scope.showSaveButton = newValue.some(word => word.selected == true);
        }
    }, true);

    getContent = function () {
        $scope.model.value = $scope.contentPicker.value;
        entityResource.getById($scope.contentPicker.value, "Document")
            .then(function (ent) {
                contentResource.getById(ent.key).then(function (data) {
                    $http.get("/sibbo-headless-backend/getproperties?docTypeId=" + data.documentType.id).then(function (result) {
                        $scope.propertyTypes = result.data.map(v => ({ ...v, selected: false }))
                    });
                });
            });
    };

    $scope.saveFooter = function () {
        $scope.saveButtonDisabled = true;
        notificationsService.add({
            headline: 'Opslaan...',
            sticky: false,
            type: 'ìnfo'
        });
        $http.post("/sibbo-headless-backend/setfooterproperties", { ids: $scope.contentPicker.value, id: $scope.contentPicker.value, properties: $scope.propertyTypes }).then(function (result) {
                    if (result.status >= 200 && result.status < 400) {
                        notificationsService.add({
                            headline: 'Saved!',
                            sticky: false,
                            type: 'success'
                        });
                        $scope.saveButtonDisabled = false;
                    }
                    else {
                        notificationsService.add({
                            headline: 'Er is iets misgegaan. Neem contact op met W3S!',
                            sticky: false,
                            type: 'error'
                        });
                        $scope.saveButtonDisabled = false;
                    }
                });
    }


});