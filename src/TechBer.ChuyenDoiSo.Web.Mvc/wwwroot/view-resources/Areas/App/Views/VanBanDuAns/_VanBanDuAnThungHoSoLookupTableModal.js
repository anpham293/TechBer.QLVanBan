(function ($) {
    app.modals.ThungHoSoLookupTableModal = function () {

        var _modalManager;

        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
        var _$thungHoSoTable = $('#ThungHoSoTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$thungHoSoTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAnsService.getAllThungHoSoForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ThungHoSoTableFilter').val()
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

        $('#ThungHoSoTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetThungHoSoButton').click(function (e) {
            e.preventDefault();
            getDuAn();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAn();
            }
        });

    };
})(jQuery);

