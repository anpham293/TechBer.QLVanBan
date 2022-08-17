(function () {
    app.modals.CreateEntityDynamicParameterModal = function () {
        var _modalManager;
        var _entityDynamicParametersAppService = abp.services.app.entityDynamicParameter;

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        this.save = function () {
            var entityDynamicParameter = {
                entityFullName: _modalManager.getModal().find("#edp-create-modal-entityFullName").val(),
                dynamicParameterId: _modalManager.getModal().find("#edp-create-modal-dynamicParameterId").val(),
                tenantId: abp.session.tenantId
            };

            _modalManager.setBusy(true);

            _entityDynamicParametersAppService.add(entityDynamicParameter)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditEntityDynamicParametersModalSaved');
                }).always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})();