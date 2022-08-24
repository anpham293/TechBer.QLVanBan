(function ($) {
    app.modals.LoaiDuAnLookupTableModal = function () {

        var _modalManager;

        var _quyTrinhDuAnAssignedsService = abp.services.app.quyTrinhDuAnAssigneds;
        var _$loaiDuAnTable = $('#LoaiDuAnTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$loaiDuAnTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quyTrinhDuAnAssignedsService.getAllLoaiDuAnForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#LoaiDuAnTableFilter').val()
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

        $('#LoaiDuAnTable tbody').on('click', '[id*=selectbtn]', function () {
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

