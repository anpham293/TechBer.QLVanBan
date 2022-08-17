(function () {
    app.modals.ManageEntityDynamicParameterValuesModal = function () {
        var _modalManager;
        var _manageDynamicParameterValueBase = new ManageDynamicParameterValueBase();

        this.init = function (modalManager) {
            _modalManager = modalManager;
            initializePage();
        };

        function initializePage() {
            var _table = _modalManager.getModal().find("#EntityDynamicParameterValuesTable");
            _table.find("tbody").empty();
            _manageDynamicParameterValueBase.initialize({
                entityFullName: _modalManager.getModal().find("#EntityFullName").val(),
                entityId: _modalManager.getModal().find("#EntityId").val(),
                bodyElement: _table.find("tbody"),
                onDeleteValues: function() {
                    initializePage();
                }
            });
        }

        this.save = function () {
            _manageDynamicParameterValueBase.save(function () {
                _modalManager.close();
            });
        };
    };

    app.modals.ManageEntityDynamicParameterValuesModal.create = function () {
        return new app.ModalManager({
            viewUrl: abp.appPath + 'App/EntityDynamicParameterValue/ManageEntityDynamicParameterValuesModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/EntityDynamicParameterValues/ManageEntityDynamicParameterValuesModal.js',
            modalClass: 'ManageEntityDynamicParameterValuesModal',
            cssClass: 'scrollable-modal'
        });
    };
})();