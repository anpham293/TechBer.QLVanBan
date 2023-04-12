(function ($) {
    app.modals.DayKeLookupTableModal = function () {

        var _modalManager;

        var _thungHoSosService = abp.services.app.thungHoSos;
        var _$dayKeTable = $('#DayKeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$dayKeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _thungHoSosService.getAllDayKeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DayKeTableFilter').val()
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

        $('#DayKeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDayKe() {
            dataTable.ajax.reload();
        }

        $('#GetDayKeButton').click(function (e) {
            e.preventDefault();
            getDayKe();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDayKe();
            }
        });

    };
})(jQuery);

