(function ($) {
    app.modals.DuAnThuHoiLookupTableModal = function () {

        var _modalManager;

        var _danhMucThuHoiesService = abp.services.app.danhMucThuHoies;
        var _$duAnThuHoiTable = $('#DuAnThuHoiTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$duAnThuHoiTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _danhMucThuHoiesService.getAllDuAnThuHoiForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DuAnThuHoiTableFilter').val()
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

        $('#DuAnThuHoiTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDuAnThuHoi() {
            dataTable.ajax.reload();
        }

        $('#GetDuAnThuHoiButton').click(function (e) {
            e.preventDefault();
            getDuAnThuHoi();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAnThuHoi();
            }
        });

    };
})(jQuery);

