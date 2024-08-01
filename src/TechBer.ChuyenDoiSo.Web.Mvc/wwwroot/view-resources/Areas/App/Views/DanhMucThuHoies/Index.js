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
                // {
                //     width: 120,
                //     targets: 0,
                //     data: null,
                //     orderable: false,
                //     autoWidth: false,
                //     defaultContent: '',
                //     rowAction: {
                //         cssClass: 'btn btn-brand dropdown-toggle',
                //         text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                //         items: [
                //             {
                //                 text: app.localize('View'),
                //                 action: function (data) {
                //                     _viewDanhMucThuHoiModal.open({id: data.record.danhMucThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('ChiTiet'),
                //                 action: function (data) {
                //                     _chitietThuHoiModal.open({id: data.record.danhMucThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('Edit'),
                //                 visible: function () {
                //                     return _permissions.edit;
                //                 },
                //                 action: function (data) {
                //                     _createOrEditModal.open({id: data.record.danhMucThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('History'),
                //                 visible: function () {
                //                     return entityHistoryIsEnabled();
                //                 },
                //                 action: function (data) {
                //                     _entityTypeHistoryModal.open({
                //                         entityTypeFullName: _entityTypeFullName,
                //                         entityId: data.record.danhMucThuHoi.id
                //                     });
                //                 }
                //             },
                //             {
                //                 text: app.localize('Delete'),
                //                 visible: function () {
                //                     return _permissions.delete;
                //                 },
                //                 action: function (data) {
                //                     deleteDanhMucThuHoi(data.record.danhMucThuHoi);
                //                 }
                //             }]
                //     }
                // },
                {
                    // width: 200,
                    targets: 0,
                    data: "danhMucThuHoi.id",
                    name: "id",
                    className: "text-center",
                    render: function (data) {
                        let chinhSuaDanhMucThuHoi = '<label class="badge badge-info chinhSuaDanhMucThuHoi" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pen" viewBox="0 0 16 16">\n' +
                            '  <path d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001m-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708z"/>\n' +
                            '</svg> Chỉnh sửa</label>';
                        
                        let chiTietDanhMucThuHoi = '<label class="badge badge-info chiTietDanhMucThuHoi" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-inboxes-fill" viewBox="0 0 16 16">\n' +
                            '  <path d="M4.98 1a.5.5 0 0 0-.39.188L1.54 5H6a.5.5 0 0 1 .5.5 1.5 1.5 0 0 0 3 0A.5.5 0 0 1 10 5h4.46l-3.05-3.812A.5.5 0 0 0 11.02 1zM3.81.563A1.5 1.5 0 0 1 4.98 0h6.04a1.5 1.5 0 0 1 1.17.563l3.7 4.625a.5.5 0 0 1 .106.374l-.39 3.124A1.5 1.5 0 0 1 14.117 10H1.883A1.5 1.5 0 0 1 .394 8.686l-.39-3.124a.5.5 0 0 1 .106-.374zM.125 11.17A.5.5 0 0 1 .5 11H6a.5.5 0 0 1 .5.5 1.5 1.5 0 0 0 3 0 .5.5 0 0 1 .5-.5h5.5a.5.5 0 0 1 .496.562l-.39 3.124A1.5 1.5 0 0 1 14.117 16H1.883a1.5 1.5 0 0 1-1.489-1.314l-.39-3.124a.5.5 0 0 1 .121-.393z"/>\n' +
                            '</svg> Danh sách</label>';
                        return chinhSuaDanhMucThuHoi + ' ' + chiTietDanhMucThuHoi;
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
                    data: "tongDuDanhMuc",
                    name: "tongDuDanhMuc",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                {
                    targets: 4,
                    data: "tongThuDanhMuc",
                    name: "tongThuDanhMuc",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                    {
                        targets: 5,
                        data: "tongThuDanhMuc",
                        name: "kinhPhiDanhMuc",
                        className: "text-right",
                        render: function (data, type, row, meta) {
                            var kinhPhiChuyen = parseFloat(row.tongDuDanhMuc) - parseFloat(row.tongThuDanhMuc);
                            return addCommas(kinhPhiChuyen) + " VNĐ"
                        }
                    },
                {
                    targets: 6,
                    data: "danhMucThuHoi.ghiChu",
                    name: "ghiChu"
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

        $(document).off('click', '.chinhSuaDanhMucThuHoi').on('click', '.chinhSuaDanhMucThuHoi', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _createOrEditModal.open({id: id});
        });
        $(document).off('click', '.chiTietDanhMucThuHoi').on('click', '.chiTietDanhMucThuHoi', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _chitietThuHoiModal.open({id: id});
        });
    }
})(jQuery);