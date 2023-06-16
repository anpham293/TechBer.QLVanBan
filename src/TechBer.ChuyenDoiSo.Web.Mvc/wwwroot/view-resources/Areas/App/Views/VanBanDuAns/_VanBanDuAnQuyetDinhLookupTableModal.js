(function ($) {
    app.modals.QuyetDinhLookupTableModal = function () {

        var _modalManager;

        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
        var _$quyetDinhTable = $('#QuyetDinhTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$quyetDinhTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAnsService.getAllQuyetDinhForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#QuyetDinhTableFilter').val()
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

        $('#QuyetDinhTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getQuyTrinhDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetQuyetDinhButton').click(function (e) {
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

