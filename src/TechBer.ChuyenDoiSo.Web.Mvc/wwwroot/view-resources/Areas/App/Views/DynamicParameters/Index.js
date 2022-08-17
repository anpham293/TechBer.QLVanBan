(function () {
    $(function () {
        var _table = $('#DynamicParametersTable');
        var _dynamicParametersAppService = abp.services.app.dynamicParameter;

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DynamicParameter/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DynamicParameters/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDynamicParameterModal',
            cssClass: 'scrollable-modal'
        });

        var dataTable = _table.DataTable({
            paging: false,
            serverSide: false,
            processing: false,
            listAction: {
                ajaxFunction: _dynamicParametersAppService.getAll,
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
                    defaultContent: '',
                    rowAction: {
                        element: $("<button/>")
                            .addClass("btn btn-primary btn-sm")
                            .text(app.localize('Detail'))
                            .click(function () {
                                window.location = "/App/DynamicParameter/Detail/" + $(this).data().id;
                            })
                    }
                },
                {
                    targets: 2,
                    data: "parameterName",
                },
                {
                    targets: 3,
                    data: "inputType",
                },
                {
                    targets: 4,
                    data: "permission",
                }
            ]
        });

        $('#CreateNewDynamicParameter').click(function () {
            _createOrEditModal.open();
        });

        $('#GetDynamicParametersButton').click(function (e) {
            e.preventDefault();
            getDynamicParameters();
        });

        function getDynamicParameters() {
            dataTable.ajax.reload();
        }

        abp.event.on('app.createOrEditDynamicParametersModalSaved', function () {
            getDynamicParameters();
        });
    });
})();
