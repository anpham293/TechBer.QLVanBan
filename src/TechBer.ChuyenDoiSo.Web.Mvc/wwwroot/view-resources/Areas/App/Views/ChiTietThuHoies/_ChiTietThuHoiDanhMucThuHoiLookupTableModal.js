(function ($) {
    app.modals.DanhMucThuHoiLookupTableModal = function () {

        var _modalManager;

        var _chiTietThuHoiesService = abp.services.app.chiTietThuHoies;
        var _$danhMucThuHoiTable = $('#DanhMucThuHoiTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$danhMucThuHoiTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chiTietThuHoiesService.getAllDanhMucThuHoiForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DanhMucThuHoiTableFilter').val()
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

        $('#DanhMucThuHoiTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDanhMucThuHoi() {
            dataTable.ajax.reload();
        }

        $('#GetDanhMucThuHoiButton').click(function (e) {
            e.preventDefault();
            getDanhMucThuHoi();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDanhMucThuHoi();
            }
        });

    };
})(jQuery);

