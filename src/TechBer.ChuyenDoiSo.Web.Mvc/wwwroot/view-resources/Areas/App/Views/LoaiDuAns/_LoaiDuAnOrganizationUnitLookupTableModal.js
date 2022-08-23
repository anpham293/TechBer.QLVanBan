(function ($) {
    app.modals.OrganizationUnitLookupTableModal = function () {

        var _modalManager;

        var _loaiDuAnsService = abp.services.app.loaiDuAns;
        var _$organizationUnitTable = $('#OrganizationUnitTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$organizationUnitTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _loaiDuAnsService.getAllOrganizationUnitForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#OrganizationUnitTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#OrganizationUnitTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getOrganizationUnit() {
            dataTable.ajax.reload();
        }

        $('#GetOrganizationUnitButton').click(function (e) {
            e.preventDefault();
            getOrganizationUnit();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getOrganizationUnit();
            }
        });

    };
})(jQuery);

