(function ($) {
    app.modals.QuyTrinhDuAnLookupTableModal = function () {

        var _modalManager;

        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
        var _$quyTrinhDuAnTable = $('#QuyTrinhDuAnTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$quyTrinhDuAnTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAnsService.getAllQuyTrinhDuAnForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#QuyTrinhDuAnTableFilter').val()
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

        $('#QuyTrinhDuAnTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getQuyTrinhDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetQuyTrinhDuAnButton').click(function (e) {
            e.preventDefault();
            getQuyTrinhDuAn();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getQuyTrinhDuAn();
            }
        });

    };
})(jQuery);

