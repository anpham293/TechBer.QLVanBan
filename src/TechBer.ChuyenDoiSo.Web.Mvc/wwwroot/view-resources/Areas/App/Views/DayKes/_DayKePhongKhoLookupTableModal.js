(function ($) {
    app.modals.PhongKhoLookupTableModal = function () {

        var _modalManager;

        var _dayKesService = abp.services.app.dayKes;
        var _$phongKhoTable = $('#PhongKhoTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$phongKhoTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dayKesService.getAllPhongKhoForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#PhongKhoTableFilter').val()
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

        $('#PhongKhoTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getPhongKho() {
            dataTable.ajax.reload();
        }

        $('#GetPhongKhoButton').click(function (e) {
            e.preventDefault();
            getPhongKho();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getPhongKho();
            }
        });

    };
})(jQuery);

