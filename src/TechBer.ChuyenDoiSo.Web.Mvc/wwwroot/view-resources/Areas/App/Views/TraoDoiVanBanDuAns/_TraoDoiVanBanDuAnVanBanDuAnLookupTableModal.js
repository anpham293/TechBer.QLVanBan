(function ($) {
    app.modals.VanBanDuAnLookupTableModal = function () {

        var _modalManager;

        var _traoDoiVanBanDuAnsService = abp.services.app.traoDoiVanBanDuAns;
        var _$vanBanDuAnTable = $('#VanBanDuAnTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$vanBanDuAnTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _traoDoiVanBanDuAnsService.getAllVanBanDuAnForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#VanBanDuAnTableFilter').val()
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

        $('#VanBanDuAnTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getVanBanDuAn() {
            dataTable.ajax.reload();
        }

        $('#GetVanBanDuAnButton').click(function (e) {
            e.preventDefault();
            getVanBanDuAn();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getVanBanDuAn();
            }
        });

    };
})(jQuery);

