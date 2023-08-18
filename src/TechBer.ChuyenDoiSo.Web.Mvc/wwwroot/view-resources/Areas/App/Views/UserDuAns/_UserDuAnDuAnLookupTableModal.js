(function ($) {
    app.modals.DuAnLookupTableModal = function () {

        var _modalManager;

        var _userDuAnsService = abp.services.app.userDuAns;
        var _$duAnTable = $('#DuAnTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$duAnTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _userDuAnsService.getAllDuAnForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DuAnTableFilter').val()
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

        $('#DuAnTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetDuAnButton').click(function (e) {
            e.preventDefault();
            getDuAn();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAn();
            }
        });

    };
})(jQuery);

