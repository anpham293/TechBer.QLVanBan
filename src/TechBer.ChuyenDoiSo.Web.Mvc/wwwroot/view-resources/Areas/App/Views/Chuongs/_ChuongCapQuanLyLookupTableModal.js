(function ($) {
    app.modals.CapQuanLyLookupTableModal = function () {

        var _modalManager;

        var _chuongsService = abp.services.app.chuongs;
        var _$capQuanLyTable = $('#CapQuanLyTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$capQuanLyTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chuongsService.getAllCapQuanLyForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#CapQuanLyTableFilter').val()
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

        $('#CapQuanLyTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getCapQuanLy() {
            dataTable.ajax.reload();
        }

        $('#GetCapQuanLyButton').click(function (e) {
            e.preventDefault();
            getCapQuanLy();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getCapQuanLy();
            }
        });

    };
})(jQuery);

