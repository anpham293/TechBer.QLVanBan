(function ($) {
    app.modals.LoaiKhoanLookupTableModal = function () {
        var _modalManager;

        var _duAnsService = abp.services.app.duAns;
        var _$loaiKhoanTable = $('#LoaiKhoanTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$loaiKhoanTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _duAnsService.getAllLoaiKhoanForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#LoaiKhoanTableFilter').val()
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
                    data: "maSo"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 2,
                    data: "ten"
                }
            ]
        });

        $('#LoaiKhoanTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getLoaiDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetLoaiDuAnButton').click(function (e) {
            e.preventDefault();
            getLoaiDuAn();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLoaiDuAn();
            }
        });

    };
})(jQuery);

