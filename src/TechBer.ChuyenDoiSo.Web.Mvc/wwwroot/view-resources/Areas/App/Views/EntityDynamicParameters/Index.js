(function () {
    $(function () {
        var _table = $('#EntityDynamicParametersTable');
        var _entityDynamicParametersAppService = abp.services.app.entityDynamicParameter;

        var _createModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/EntityDynamicParameter/CreateModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/EntityDynamicParameters/_CreateModal.js',
            modalClass: 'CreateEntityDynamicParameterModal',
            cssClass: 'scrollable-modal'
        });

        var _permissions = {
            delete: abp.auth.hasPermission('Pages.Administration.EntityDynamicParameters.Delete')
        };

        _table.find('tbody').on('click', 'tr', function () {
            var data = dataTable.row(this).data();
            if (data) {
                window.location = "/App/EntityDynamicParameter/Detail/" + data.entityFullName;
            }
        });

        $('#CreateNewEntityDynamicParameter').click(function () {
            _createModal.open();
        });

        $('#GetEntityDynamicParametersButton').click(function (e) {
            e.preventDefault();
            getEntityDynamicParameters();
        });
                
        var _dataTable = _table.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            listAction: {
                ajaxFunction: _entityDynamicParametersAppService.getAll
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
                    orderable: false,
                    data: "entityFullName",
                },
                {
                    targets: 2,
                    orderable: false,
                    data: "dynamicParameterName",
                },
                {
                    targets: 3,
                    data: null,
                    orderable: false,
                    defaultContent: '',
                    visible: _permissions.delete,
                    rowAction: {
                        element: $("<button/>")
                            .addClass("btn btn-danger")
                            .text(app.localize('Delete'))
                            .click(function () {
                                deleteEntityDynamicParameter($(this).data());
                            })
                    }
                }
            ]
        });

        function getEntityDynamicParameters() {
            _dataTable.ajax.reload();
        }

        abp.event.on('app.createOrEditEntityDynamicParametersModalSaved', function () {
            getEntityDynamicParameters();
        });

        function deleteEntityDynamicParameter(data) {
            abp.message.confirm(
                app.localize('DeleteEntityDynamicParameterMessage', data.entityFullName, data.dynamicParameterName),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        abp.ui.setBusy();
                        _entityDynamicParametersAppService.delete(data.id)
                            .done(function () {
                                getEntityDynamicParameters();
                                abp.notify.success(
                                    app.localize('SuccessfullyDeleted')
                                );
                            }).always(function () {
                                abp.ui.clearBusy();
                            });
                    }
                }
            );
        }
    });
})();
