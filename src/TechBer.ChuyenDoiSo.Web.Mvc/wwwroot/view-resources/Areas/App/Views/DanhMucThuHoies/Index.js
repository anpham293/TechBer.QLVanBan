(function ($) {
    app.modals.DanhMucThuHoiModal = function () {

        var _$danhMucThuHoiesTable = $('#DanhMucThuHoiesTable');
        var _danhMucThuHoiesService = abp.services.app.danhMucThuHoies;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.DanhMucThuHoi';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DanhMucThuHoies.Create'),
            edit: abp.auth.hasPermission('Pages.DanhMucThuHoies.Edit'),
            'delete': abp.auth.hasPermission('Pages.DanhMucThuHoies.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhMucThuHoies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DanhMucThuHoies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDanhMucThuHoiModal'
        });

        var _viewDanhMucThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhMucThuHoies/ViewdanhMucThuHoiModal',
            modalClass: 'ViewDanhMucThuHoiModal'
        });

        var _chitietThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhMucThuHoies/ChiTietThuHoiModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThuHoies/Index.js',
            modalClass: 'ChiTietThuHoiModal',
            modalSize: 'modal-full-90 modal-dialog-scrollable'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$danhMucThuHoiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _danhMucThuHoiesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DanhMucThuHoiesTableFilter').val(),
                        sttFilter: $('#SttFilterId').val(),
                        tenFilter: $('#TenFilterId').val(),
                        ghiChuFilter: $('#GhiChuFilterId').val(),
                        minTypeFilter: $('#MinTypeFilterId').val(),
                        maxTypeFilter: $('#MaxTypeFilterId').val(),
                        duAnThuHoiMaDATHFilter: $("#DuAnThuHoiId").val()
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
                                    _viewDanhMucThuHoiModal.open({id: data.record.danhMucThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('ChiTiet'),
                                action: function (data) {
                                    _chitietThuHoiModal.open({id: data.record.danhMucThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.danhMucThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.danhMucThuHoi.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDanhMucThuHoi(data.record.danhMucThuHoi);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "danhMucThuHoi.stt",
                    name: "stt"
                },
                {
                    targets: 2,
                    data: "danhMucThuHoi.ten",
                    name: "ten"
                },
                {
                    targets: 3,
                    data: "danhMucThuHoi.ghiChu",
                    name: "ghiChu"
                },
                {
                    targets: 4,
                    data: "danhMucThuHoi.type",
                    name: "type"
                },
                {
                    targets: 5,
                    data: "duAnThuHoiMaDATH",
                    name: "duAnThuHoiFk.maDATH"
                }
            ]
        });

        function getDanhMucThuHoies() {
            dataTable.ajax.reload();
        }

        function deleteDanhMucThuHoi(danhMucThuHoi) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _danhMucThuHoiesService.delete({
                            id: danhMucThuHoi.id
                        }).done(function () {
                            getDanhMucThuHoies(true);
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

        $('#CreateNewDanhMucThuHoiButton').click(function () {
            _createOrEditModal.open({duAnThuHoiId: $("#DuAnThuHoiId").val()});
        });

        $('#ExportToExcelButton').click(function () {
            _danhMucThuHoiesService
                .getDanhMucThuHoiesToExcel({
                    filter: $('#DanhMucThuHoiesTableFilter').val(),
                    sttFilter: $('#SttFilterId').val(),
                    tenFilter: $('#TenFilterId').val(),
                    ghiChuFilter: $('#GhiChuFilterId').val(),
                    minTypeFilter: $('#MinTypeFilterId').val(),
                    maxTypeFilter: $('#MaxTypeFilterId').val(),
                    duAnThuHoiMaDATHFilter: $("#DuAnThuHoiId").val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDanhMucThuHoiModalSaved', function () {
            getDanhMucThuHoies();
        });

        $('#GetDanhMucThuHoiesButton').click(function (e) {
            e.preventDefault();
            getDanhMucThuHoies();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDanhMucThuHoies();
            }
        });
    }
})(jQuery);