(function ($) {
    app.modals.ChuongLookupTableModal = function () {
        var _modalManager;

        var _duAnsService = abp.services.app.duAns;
        var _$chuongTable = $('#ChuongTable');
        
            // $('#CapQuanLyFilterId').select2({
            //     placeholder: 'Tất cả',
            //     allowClear: true
            // }).val(null).trigger("change");

        
        this.init = function (modalManager) {
            _modalManager = modalManager;
        };

        var dataTable = _$chuongTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _duAnsService.getAllChuongForLookupTable,
                inputFilter: function () {
                    return {
                        capQuanLyFilterId : $('#CapQuanLyFilterId').val(),
                        filter: $('#ChuongTableFilter').val()
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

        $('#ChuongTable tbody').on('click', '[id*=selectbtn]', function () {
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

