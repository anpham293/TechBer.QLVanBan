(function () {
    $(function () {

        var _$duAnsTable = $('#DuAnsTable');
        var _duAnsService = abp.services.app.duAns;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DuAns.Create'),
            edit: abp.auth.hasPermission('Pages.DuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.DuAns.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDuAnModal'
        });

        var _viewDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/ViewduAnModal',
            modalClass: 'ViewDuAnModal'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$duAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _duAnsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DuAnsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionsFilter: $('#DescriptionsFilterId').val(),
                        loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewDuAnModal.open({id: data.record.duAn.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.duAn.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDuAn(data.record.duAn);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "duAn.name",
                    name: "name"
                },
                {
                    targets: 2,
                    data: "duAn.descriptions",
                    name: "descriptions"
                },
                {
                    targets: 3,
                    data: "loaiDuAnName",
                    name: "loaiDuAnFk.name"
                },
                {
                    targets: 4,
                    render: function (displayName, type, row, meta) {
                        return "<a class='btn btn-info' href='/App/VanBanDuAns?duanid=" + row.duAn.id + "' style='color:white'>Danh sách văn bản >></a>";
                    }
                }
            ]
        });

        function getDuAns() {
            dataTable.ajax.reload();
        }

        function deleteDuAn(duAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _duAnsService.delete({
                            id: duAn.id
                        }).done(function () {
                            getDuAns(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewDuAnButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _duAnsService
                .getDuAnsToExcel({
                    filter: $('#DuAnsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionsFilter: $('#DescriptionsFilterId').val(),
                    loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDuAnModalSaved', function () {
            getDuAns();
        });

        $('#GetDuAnsButton').click(function (e) {
            e.preventDefault();
            getDuAns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAns();
            }
        });
    });
})();