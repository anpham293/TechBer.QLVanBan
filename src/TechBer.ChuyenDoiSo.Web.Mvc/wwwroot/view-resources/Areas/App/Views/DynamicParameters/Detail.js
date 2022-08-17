(function () {
    $(function () {
        var _table = $('#DynamicParameterValuesTable');
        var _dynamicParameterAppService = abp.services.app.dynamicParameter;
        var _dynamicParameterValueAppService = abp.services.app.dynamicParameterValue;
        var _inputTypeManager;
        var dataTable;
        var initialized = false;
        var _permissions = {
            dynamicParameterValue_edit: abp.auth.hasPermission('Pages.Administration.DynamicParameterValue.Edit'),
            dynamicParameterValue_delete: abp.auth.hasPermission('Pages.Administration.DynamicParameterValue.Delete')
        };

        var _createOrEditDynamicParameterModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicParameter/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicParameters/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDynamicParameterModal',
            cssClass: 'scrollable-modal'
        });

        var _createOrEditDynamicParameterValueModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicParameter/CreateOrEditValueModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicParameters/_CreateOrEditValueModal.js',
            modalClass: 'CreateOrEditDynamicParameterValueModal',
            cssClass: 'scrollable-modal'
        });


        function loadValuesDataTable() {
            dataTable = _table.DataTable({
                paging: false,
                serverSide: true,
                processing: false,
                listAction: {
                    ajaxFunction: _dynamicParameterValueAppService.getAllValuesOfDynamicParameter,
                    inputFilter: function () {
                        return {
                            id: dynamicParameterId
                        };
                    }
                },
                columnDefs: [
                    {
                        className: 'control responsive',
                        orderable: false,
                        render: function () {
                            return '';
                        },
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: null,
                        orderable: false,
                        autoWidth: false,
                        defaultContent: '',
                        rowAction: {
                            text: '<i class="fa fa-cog"></i><span class="caret"></span>',
                            items: [{
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return _permissions.dynamicParameterValue_edit;
                                },
                                action: function (data) {
                                    _createOrEditDynamicParameterValueModal.open({ id: data.record.id });
                                }
                            }, {
                                text: app.localize('Delete'),
                                visible: function (data) {
                                    return _permissions.dynamicParameterValue_delete;
                                },
                                action: function (data) {
                                    deleteValue(data.record.id);
                                }
                            }]
                        }
                    },
                    {
                        targets: 2,
                        orderable: false,
                        data: "value",
                    }
                ]
            });
        }

        function deleteValue(id) {
            abp.message.confirm(
                app.localize('DeleteDynamicParameterValueMessage'),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _dynamicParameterValueAppService.delete(id)
                            .done(function () {
                                reloadValues();
                                abp.notify.success(
                                    app.localize('SuccessfullyDeleted')
                                );
                            }).always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                }
            );
        };

        function deleteParameter(id) {
            abp.message.confirm(
                app.localize('DeleteDynamicParameterMessage'),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _dynamicParameterAppService.delete(id)
                            .done(function () {
                                window.location.href = "/App/DynamicParameter";

                                abp.notify.success(
                                    app.localize('SuccessfullyDeleted')
                                );
                            }).always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                }
            );
        };

        $('#dynamic-parameter-delete').click(function () {
            deleteParameter(dynamicParameterId);
        });

        $('#dynamic-parameter-edit').click(function () {
            _createOrEditDynamicParameterModal.open({ id: dynamicParameterId });
        });

        $('#add-new-dynamic-parameter-value').click(function () {
            _createOrEditDynamicParameterValueModal.open({ dynamicParameterId: dynamicParameterId });
        });

        abp.event.on('app.createOrEditDynamicParametersModalSaved', function () {
            window.location.reload();
        });

        abp.event.on('app.createOrEditDynamicParameterValueModalSaved', function () {
            reloadValues();
        });

        $('#refresh-dynamic-parameter-values-btn').click(function (e) {
            reloadValues();
        });

        function reloadValues() {
            if (initialized && dataTable) {
                dataTable.ajax.reload();
            }
        }

        function manageValueArea() {
            if (!_inputTypeManager) {
                abp.notify.error("Unknown input type");
                return;
            }

            if (typeof _inputTypeManager.hasValues !== 'boolean') {
                abp.notify.error(`Input type manager ${_inputTypeManager} must have "hasValues" field typed boolean`);
                return;
            }

            if (_inputTypeManager.hasValues) {
                $("#portlet-dynamic-parameter-values").removeClass("d-none");
                loadValuesDataTable();
            } else {
                $("#portlet-dynamic-parameter-values").hide();
            }

            initialized = true;
        }

        function initialize() {
            _dynamicParameterAppService.findAllowedInputType(currentInputType)
                .done(function (inputType) {
                    if (inputType) {
                        _inputTypeManager = abp.inputTypeProviders.getInputTypeInstance({ inputType: inputType });
                        manageValueArea();
                    } else {
                        abp.notify.error("Unknown input type");
                    }

                });
        }

        initialize();
    });
})();