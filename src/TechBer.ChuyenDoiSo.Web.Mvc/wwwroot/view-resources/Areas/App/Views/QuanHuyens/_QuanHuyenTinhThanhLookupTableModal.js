(function ($) {
    app.modals.TinhThanhLookupTableModal = function () {

        var _modalManager;

        var _quanHuyensService = abp.services.app.quanHuyens;
        var _$tinhThanhTable = $('#TinhThanhTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$tinhThanhTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quanHuyensService.getAllTinhThanhForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#TinhThanhTableFilter').val()
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

        $('#TinhThanhTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getTinhThanh() {
            dataTable.ajax.reload();
        }

        $('#GetTinhThanhButton').click(function (e) {
            e.preventDefault();
            getTinhThanh();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getTinhThanh();
            }
        });

    };
})(jQuery);

