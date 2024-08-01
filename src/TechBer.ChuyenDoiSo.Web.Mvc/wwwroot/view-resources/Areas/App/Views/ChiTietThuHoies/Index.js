(function ($) {
    app.modals.ChiTietThuHoiModal = function () {

        var _$chiTietThuHoiesTable = $('#ChiTietThuHoiesTable');
        var _chiTietThuHoiesService = abp.services.app.chiTietThuHoies;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.ChiTietThuHoi';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChiTietThuHoies.Create'),
            edit: abp.auth.hasPermission('Pages.ChiTietThuHoies.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChiTietThuHoies.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThuHoies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThuHoies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChiTietThuHoiModal',
            modalSize: 'modal-full-85 modal-dialog-scrollable'
        });

        var _viewChiTietThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThuHoies/ViewchiTietThuHoiModal',
            modalClass: 'ViewChiTietThuHoiModal'
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

        var dataTable = _$chiTietThuHoiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chiTietThuHoiesService.getAll,
                inputFilter: function () {
                    return {
                        // filter: $('#ChiTietThuHoiesTableFilter').val(),
                        // minDu1Filter: $('#MinDu1FilterId').val(),
                        // maxDu1Filter: $('#MaxDu1FilterId').val(),
                        // minDu2Filter: $('#MinDu2FilterId').val(),
                        // maxDu2Filter: $('#MaxDu2FilterId').val(),
                        // minDu3Filter: $('#MinDu3FilterId').val(),
                        // maxDu3Filter: $('#MaxDu3FilterId').val(),
                        // minDu4Filter: $('#MinDu4FilterId').val(),
                        // maxDu4Filter: $('#MaxDu4FilterId').val(),
                        // minDu5Filter: $('#MinDu5FilterId').val(),
                        // maxDu5Filter: $('#MaxDu5FilterId').val(),
                        // minDu6Filter: $('#MinDu6FilterId').val(),
                        // maxDu6Filter: $('#MaxDu6FilterId').val(),
                        // minDu7Filter: $('#MinDu7FilterId').val(),
                        // maxDu7Filter: $('#MaxDu7FilterId').val(),
                        // minDu8Filter: $('#MinDu8FilterId').val(),
                        // maxDu8Filter: $('#MaxDu8FilterId').val(),
                        // minDu9Filter: $('#MinDu9FilterId').val(),
                        // maxDu9Filter: $('#MaxDu9FilterId').val(),
                        // minDu10Filter: $('#MinDu10FilterId').val(),
                        // maxDu10Filter: $('#MaxDu10FilterId').val(),
                        // minDu11Filter: $('#MinDu11FilterId').val(),
                        // maxDu11Filter: $('#MaxDu11FilterId').val(),
                        // minDu12Filter: $('#MinDu12FilterId').val(),
                        // maxDu12Filter: $('#MaxDu12FilterId').val(),
                        // minThu1Filter: $('#MinThu1FilterId').val(),
                        // maxThu1Filter: $('#MaxThu1FilterId').val(),
                        // minThu2Filter: $('#MinThu2FilterId').val(),
                        // maxThu2Filter: $('#MaxThu2FilterId').val(),
                        // minThu3Filter: $('#MinThu3FilterId').val(),
                        // maxThu3Filter: $('#MaxThu3FilterId').val(),
                        // minThu4Filter: $('#MinThu4FilterId').val(),
                        // maxThu4Filter: $('#MaxThu4FilterId').val(),
                        // minThu5Filter: $('#MinThu5FilterId').val(),
                        // maxThu5Filter: $('#MaxThu5FilterId').val(),
                        // minThu6Filter: $('#MinThu6FilterId').val(),
                        // maxThu6Filter: $('#MaxThu6FilterId').val(),
                        // minThu7Filter: $('#MinThu7FilterId').val(),
                        // maxThu7Filter: $('#MaxThu7FilterId').val(),
                        // minThu8Filter: $('#MinThu8FilterId').val(),
                        // maxThu8Filter: $('#MaxThu8FilterId').val(),
                        // minThu9Filter: $('#MinThu9FilterId').val(),
                        // maxThu9Filter: $('#MaxThu9FilterId').val(),
                        // minThu10Filter: $('#MinThu10FilterId').val(),
                        // maxThu10Filter: $('#MaxThu10FilterId').val(),
                        // minThu11Filter: $('#MinThu11FilterId').val(),
                        // maxThu11Filter: $('#MaxThu11FilterId').val(),
                        // minThu12Filter: $('#MinThu12FilterId').val(),
                        // maxThu12Filter: $('#MaxThu12FilterId').val(),
                        // ghiChuFilter: $('#GhiChuFilterId').val(),
                        // tenFilter: $('#TenFilterId').val(),
                        danhMucThuHoiTenFilter: $('#DanhMucThuHoiId').val()
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
                //                     _viewChiTietThuHoiModal.open({id: data.record.chiTietThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('Edit'),
                //                 visible: function () {
                //                     return _permissions.edit;
                //                 },
                //                 action: function (data) {
                //                     _createOrEditModal.open({id: data.record.chiTietThuHoi.id});
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
                //                         entityId: data.record.chiTietThuHoi.id
                //                     });
                //                 }
                //             },
                //             {
                //                 text: app.localize('Delete'),
                //                 visible: function () {
                //                     return _permissions.delete;
                //                 },
                //                 action: function (data) {
                //                     deleteChiTietThuHoi(data.record.chiTietThuHoi);
                //                 }
                //             }]
                //     }
                // },
                {
                    // width: 200,
                    targets: 0,
                    data: "chiTietThuHoi.id",
                    name: "id",
                    className: "text-center",
                    render: function (data) {
                        let chinhSuaChiTietThuHoi = '<label class="badge badge-info chinhSuaChiTietThuHoi" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pen" viewBox="0 0 16 16">\n' +
                            '  <path d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001m-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708z"/>\n' +
                            '</svg> Chỉnh sửa</label>';
                        return chinhSuaChiTietThuHoi;
                    }
                },
                {
                    targets: 1,
                    data: "chiTietThuHoi.ten",
                    name: "ten"
                },
                {
                    targets: 2,
                    data: "chiTietThuHoi.tongDu",
                    name: "tongDu",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                {
                    targets: 3,
                    data: "chiTietThuHoi.tongThu",
                    name: "tongThu",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                {
                    targets: 4,
                    data: "chiTietThuHoi.tongThu",
                    name: "tongThu",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        var kinhPhiChuyen = parseFloat(row.chiTietThuHoi.tongDu) - parseFloat(row.chiTietThuHoi.tongThu)
                        return addCommas(kinhPhiChuyen) + " VNĐ"
                    }
                },
                {
                    targets: 5,
                    data: "chiTietThuHoi.ghiChu",
                    name: "ghiChu"
                }
            ]
        });

        function getChiTietThuHoies() {
            dataTable.ajax.reload();
        }

        function deleteChiTietThuHoi(chiTietThuHoi) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chiTietThuHoiesService.delete({
                            id: chiTietThuHoi.id
                        }).done(function () {
                            getChiTietThuHoies(true);
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

        $('#CreateNewChiTietThuHoiButton').click(function () {
            _createOrEditModal.open({danhMucThuHoiId: $("#DanhMucThuHoiId").val()});
        });

        $('#ExportToExcelButton').click(function () {
            _chiTietThuHoiesService
                .getChiTietThuHoiesToExcel({
                    // filter: $('#ChiTietThuHoiesTableFilter').val(),
                    // minDu1Filter: $('#MinDu1FilterId').val(),
                    // maxDu1Filter: $('#MaxDu1FilterId').val(),
                    // minDu2Filter: $('#MinDu2FilterId').val(),
                    // maxDu2Filter: $('#MaxDu2FilterId').val(),
                    // minDu3Filter: $('#MinDu3FilterId').val(),
                    // maxDu3Filter: $('#MaxDu3FilterId').val(),
                    // minDu4Filter: $('#MinDu4FilterId').val(),
                    // maxDu4Filter: $('#MaxDu4FilterId').val(),
                    // minDu5Filter: $('#MinDu5FilterId').val(),
                    // maxDu5Filter: $('#MaxDu5FilterId').val(),
                    // minDu6Filter: $('#MinDu6FilterId').val(),
                    // maxDu6Filter: $('#MaxDu6FilterId').val(),
                    // minDu7Filter: $('#MinDu7FilterId').val(),
                    // maxDu7Filter: $('#MaxDu7FilterId').val(),
                    // minDu8Filter: $('#MinDu8FilterId').val(),
                    // maxDu8Filter: $('#MaxDu8FilterId').val(),
                    // minDu9Filter: $('#MinDu9FilterId').val(),
                    // maxDu9Filter: $('#MaxDu9FilterId').val(),
                    // minDu10Filter: $('#MinDu10FilterId').val(),
                    // maxDu10Filter: $('#MaxDu10FilterId').val(),
                    // minDu11Filter: $('#MinDu11FilterId').val(),
                    // maxDu11Filter: $('#MaxDu11FilterId').val(),
                    // minDu12Filter: $('#MinDu12FilterId').val(),
                    // maxDu12Filter: $('#MaxDu12FilterId').val(),
                    // minThu1Filter: $('#MinThu1FilterId').val(),
                    // maxThu1Filter: $('#MaxThu1FilterId').val(),
                    // minThu2Filter: $('#MinThu2FilterId').val(),
                    // maxThu2Filter: $('#MaxThu2FilterId').val(),
                    // minThu3Filter: $('#MinThu3FilterId').val(),
                    // maxThu3Filter: $('#MaxThu3FilterId').val(),
                    // minThu4Filter: $('#MinThu4FilterId').val(),
                    // maxThu4Filter: $('#MaxThu4FilterId').val(),
                    // minThu5Filter: $('#MinThu5FilterId').val(),
                    // maxThu5Filter: $('#MaxThu5FilterId').val(),
                    // minThu6Filter: $('#MinThu6FilterId').val(),
                    // maxThu6Filter: $('#MaxThu6FilterId').val(),
                    // minThu7Filter: $('#MinThu7FilterId').val(),
                    // maxThu7Filter: $('#MaxThu7FilterId').val(),
                    // minThu8Filter: $('#MinThu8FilterId').val(),
                    // maxThu8Filter: $('#MaxThu8FilterId').val(),
                    // minThu9Filter: $('#MinThu9FilterId').val(),
                    // maxThu9Filter: $('#MaxThu9FilterId').val(),
                    // minThu10Filter: $('#MinThu10FilterId').val(),
                    // maxThu10Filter: $('#MaxThu10FilterId').val(),
                    // minThu11Filter: $('#MinThu11FilterId').val(),
                    // maxThu11Filter: $('#MaxThu11FilterId').val(),
                    // minThu12Filter: $('#MinThu12FilterId').val(),
                    // maxThu12Filter: $('#MaxThu12FilterId').val(),
                    // ghiChuFilter: $('#GhiChuFilterId').val(),
                    // tenFilter: $('#TenFilterId').val(),
                    danhMucThuHoiTenFilter: $('#DanhMucThuHoiId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChiTietThuHoiModalSaved', function () {
            getChiTietThuHoies();
        });

        $('#GetChiTietThuHoiesButton').click(function (e) {
            e.preventDefault();
            getChiTietThuHoies();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getChiTietThuHoies();
            }
        });

        $(document).off('click', '.chinhSuaChiTietThuHoi').on('click', '.chinhSuaChiTietThuHoi', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _createOrEditModal.open({id: id});
        });
    }
})(jQuery);