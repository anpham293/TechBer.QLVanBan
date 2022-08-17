(function () {
    app.modals.CreateOrEditDynamicParameterModal = function () {
        var _modalManager;
        var _dynamicParametersAppServices = abp.services.app.dynamicParameter;

        var _$permissionFilterModal = app.modals.PermissionTreeModal.create({
            singleSelect: true,
            onSelectionDone: function (filteredPermissions) {
                if (filteredPermissions && filteredPermissions.length > 0) {
                    _modalManager.getModal().find('input[name=permission]').val(filteredPermissions[0])
                }
            }
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {
            var dynamicParameter = {
                id: _modalManager.getModal().find("input[name=Id]").val(),
                parameterName: _modalManager.getModal().find("input[name=parameterName]").val(),
                permission: _modalManager.getModal().find("input[name=permission]").val(),
                inputType: _modalManager.getModal().find("select[name=inputType]").val(),
            };
            if (dynamicParameter.parameterName.trim() === "") {
                abp.notify.success(app.localize("XCanNotBeNullOrEmpty", app.localize("ParameterName")))
                return;
            }
            _modalManager.setBusy(true);

            if (dynamicParameter.id) {
                _dynamicParametersAppServices.update(dynamicParameter)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditDynamicParametersModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            } else {
                _dynamicParametersAppServices.add(dynamicParameter)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditDynamicParametersModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            }
        };

        $("#FilterByPermissionsButton").click(function () {
            _$permissionFilterModal.open({ grantedPermissionNames: [] });
        });
    };
})();