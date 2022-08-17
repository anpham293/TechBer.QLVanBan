(function () {
    app.modals.CreateOrEditDynamicParameterValueModal = function () {
        var _modalManager;
        var _dynamicParameterValueAppServices = abp.services.app.dynamicParameterValue;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {
            var dynamicParameter = {
                id: _modalManager.getModal().find("input[name=id]").val(),
                dynamicParameterId: _modalManager.getModal().find("input[name=dynamicParameterId]").val(),
                value: _modalManager.getModal().find("input[name=dynamicParameterValue]").val(),
            };

            _modalManager.setBusy(true);

            if (dynamicParameter.id) {
                _dynamicParameterValueAppServices.update(dynamicParameter)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditDynamicParameterValueModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            } else {
                _dynamicParameterValueAppServices.add(dynamicParameter)
                    .done(function () {
                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.createOrEditDynamicParameterValueModalSaved');
                    }).always(function () {
                        _modalManager.setBusy(false);
                    });
            }
        };
    };
})();