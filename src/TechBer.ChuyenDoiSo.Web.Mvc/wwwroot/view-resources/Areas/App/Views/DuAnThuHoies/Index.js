(function () {
    $(function () {

        var _$duAnThuHoiesTable = $('#DuAnThuHoiesTable');
        var _duAnThuHoiesService = abp.services.app.duAnThuHoies;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DuAnThuHoies.Create'),
            edit: abp.auth.hasPermission('Pages.DuAnThuHoies.Edit'),
            'delete': abp.auth.hasPermission('Pages.DuAnThuHoies.Delete')
        };

        var _createOrEditModal = new app.ModalManager({ 
            viewUrl: abp.appPath + 'App/DuAnThuHoies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAnThuHoies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDuAnThuHoiModal',
            modalSize: 'modal-xl modal-dialog-scrollable'
        });

        var _viewDuAnThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAnThuHoies/ViewduAnThuHoiModal',
            modalClass: 'ViewDuAnThuHoiModal'
        });
        var _danhMucThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAnThuHoies/DanhMucThuHoiModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DanhMucThuHoies/Index.js',
            modalClass: 'DanhMucThuHoiModal',
            modalSize: 'modal-full modal-dialog-scrollable'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$duAnThuHoiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _duAnThuHoiesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DuAnThuHoiesTableFilter').val(),
                        maDATHFilter: $('#MaDATHFilterId').val(),
                        tenFilter: $('#TenFilterId').val(),
                        minNamQuanLyFilter: $('#MinNamQuanLyFilterId').val(),
                        maxNamQuanLyFilter: $('#MaxNamQuanLyFilterId').val(),
                        minThoiHanBaoLanhHopDongFilter: getDateFilter($('#MinThoiHanBaoLanhHopDongFilterId')),
                        maxThoiHanBaoLanhHopDongFilter: getDateFilter($('#MaxThoiHanBaoLanhHopDongFilterId')),
                        minThoiHanBaoLanhTamUngFilter: getDateFilter($('#MinThoiHanBaoLanhTamUngFilterId')),
                        maxThoiHanBaoLanhTamUngFilter: getDateFilter($('#MaxThoiHanBaoLanhTamUngFilterId')),
                        ghiChuFilter: $('#GhiChuFilterId').val(),
                        minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                        maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val()
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
                //                     _viewDuAnThuHoiModal.open({id: data.record.duAnThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('ChiTiet'),
                //                 action: function (data) {
                //                     _danhMucThuHoiModal.open({id: data.record.duAnThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('Edit'),
                //                 visible: function () {
                //                     return _permissions.edit;
                //                 },
                //                 action: function (data) {
                //                     _createOrEditModal.open({id: data.record.duAnThuHoi.id});
                //                 }
                //             },
                //             {
                //                 text: app.localize('Delete'),
                //                 visible: function () {
                //                     return _permissions.delete;
                //                 },
                //                 action: function (data) {
                //                     deleteDuAnThuHoi(data.record.duAnThuHoi);
                //                 }
                //             }]
                //     }
                // },
                {
                    // width: 200,
                    targets: 0,
                    data: "duAnThuHoi.id",
                    name: "id",
                    className: "text-center",
                    render: function (data) {
                        let chinhSuaDuAnThuHoi = '<label class="badge badge-info chinhSuaDuAnThuHoi" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pen" viewBox="0 0 16 16">\n' +
                            '  <path d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001m-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708z"/>\n' +
                            '</svg> Chỉnh sửa</label>';

                        let danhMucThuHoiModal = '<label class="badge badge-info danhMucThuHoiModal" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-inboxes-fill" viewBox="0 0 16 16">\n' +
                            '  <path d="M4.98 1a.5.5 0 0 0-.39.188L1.54 5H6a.5.5 0 0 1 .5.5 1.5 1.5 0 0 0 3 0A.5.5 0 0 1 10 5h4.46l-3.05-3.812A.5.5 0 0 0 11.02 1zM3.81.563A1.5 1.5 0 0 1 4.98 0h6.04a1.5 1.5 0 0 1 1.17.563l3.7 4.625a.5.5 0 0 1 .106.374l-.39 3.124A1.5 1.5 0 0 1 14.117 10H1.883A1.5 1.5 0 0 1 .394 8.686l-.39-3.124a.5.5 0 0 1 .106-.374zM.125 11.17A.5.5 0 0 1 .5 11H6a.5.5 0 0 1 .5.5 1.5 1.5 0 0 0 3 0 .5.5 0 0 1 .5-.5h5.5a.5.5 0 0 1 .496.562l-.39 3.124A1.5 1.5 0 0 1 14.117 16H1.883a1.5 1.5 0 0 1-1.489-1.314l-.39-3.124a.5.5 0 0 1 .121-.393z"/>\n' +
                            '</svg> Danh sách</label>';
                        
                        let ketXuatExcel = '<label class="badge badge-info ketXuatExcel" data-rowId="'+ data +'"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-file-earmark-excel" viewBox="0 0 16 16">\n' +
                            '  <path d="M5.884 6.68a.5.5 0 1 0-.768.64L7.349 10l-2.233 2.68a.5.5 0 0 0 .768.64L8 10.781l2.116 2.54a.5.5 0 0 0 .768-.641L8.651 10l2.233-2.68a.5.5 0 0 0-.768-.64L8 9.219l-2.116-2.54z"/>\n' +
                            '  <path d="M14 14V4.5L9.5 0H4a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2M9.5 3A1.5 1.5 0 0 0 11 4.5h2V14a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1h5.5z"/>\n' +
                            '</svg> Báo cáo</label>';
                        return chinhSuaDuAnThuHoi + ' ' + danhMucThuHoiModal + ' ' + ketXuatExcel;
                    }
                },
                {
                    targets: 1,
                    data: "duAnThuHoi.maDATH",
                    name: "maDATH"
                },
                {
                    width: 250,
                    targets: 2,
                    data: "duAnThuHoi.ten",
                    name: "ten",
                    render: function (data) {
                        return '<a style="white-space: normal">'+data+'</a>';
                    }
                },
                {
                    targets: 3,
                    data: "duAnThuHoi.namQuanLy",
                    name: "namQuanLy"
                },
                {
                    targets: 4,
                    data: "tongDuDuAn",
                    name: "tongDuDuAn",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                {
                    targets: 5,
                    data: "tongThuDuAn",
                    name: "tongThuDuAn",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        return addCommas(data) + " VNĐ"
                    }
                },
                {
                    targets: 6,
                    data: "tongThuDuAn",
                    name: "kinhPhiDuAn",
                    className: "text-right",
                    render: function (data, type, row, meta) {
                        var kinhPhiChuyen = parseFloat(row.tongDuDuAn) - parseFloat(row.tongThuDuAn);
                        return addCommas(kinhPhiChuyen) + " VNĐ"
                    }
                },
                {
                    targets: 7,
                    data: "duAnThuHoi.thoiHanBaoLanhHopDong",
                    name: "thoiHanBaoLanhHopDong",
                    render: function (thoiHanBaoLanhHopDong) {
                        if (thoiHanBaoLanhHopDong) {
                            return moment(thoiHanBaoLanhHopDong).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 8,
                    data: "duAnThuHoi.thoiHanBaoLanhTamUng",
                    name: "thoiHanBaoLanhTamUng",
                    render: function (thoiHanBaoLanhTamUng) {
                        if (thoiHanBaoLanhTamUng) {
                            return moment(thoiHanBaoLanhTamUng).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 9,
                    data: "duAnThuHoi.ghiChu",
                    name: "ghiChu"
                },
                {
                    targets: 10,
                    data: "duAnThuHoi.trangThai",
                    name: "trangThai"
                }
            ]
        });

        function getDuAnThuHoies() {
            dataTable.ajax.reload();
        }

        $("#SendZalo").on('click', function () {
            abp.ui.setBusy($("body"));
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _duAnThuHoiesService.sendZalo().done(function (result) {
                            abp.ui.clearBusy($("body"));
                        });
                    } else {
                        abp.ui.clearBusy($("body"));
                    }
                }
            );

        });
        
        $("#docPDF").on('click', function () {
            abp.ui.setBusy($("body"));
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _duAnThuHoiesService.docPDF().done(function (result) {
                            console.log(result);
                            abp.ui.clearBusy($("body"));
                        });
                    } else {
                        abp.ui.clearBusy($("body"));
                    }
                }
            );

        });

        function deleteDuAnThuHoi(duAnThuHoi) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _duAnThuHoiesService.delete({
                            id: duAnThuHoi.id
                        }).done(function () {
                            getDuAnThuHoies(true);
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

        $('#CreateNewDuAnThuHoiButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _duAnThuHoiesService
                .getDuAnThuHoiesToExcel({
                    filter: $('#DuAnThuHoiesTableFilter').val(),
                    maDATHFilter: $('#MaDATHFilterId').val(),
                    tenFilter: $('#TenFilterId').val(),
                    minNamQuanLyFilter: $('#MinNamQuanLyFilterId').val(),
                    maxNamQuanLyFilter: $('#MaxNamQuanLyFilterId').val(),
                    minThoiHanBaoLanhHopDongFilter: getDateFilter($('#MinThoiHanBaoLanhHopDongFilterId')),
                    maxThoiHanBaoLanhHopDongFilter: getDateFilter($('#MaxThoiHanBaoLanhHopDongFilterId')),
                    minThoiHanBaoLanhTamUngFilter: getDateFilter($('#MinThoiHanBaoLanhTamUngFilterId')),
                    maxThoiHanBaoLanhTamUngFilter: getDateFilter($('#MaxThoiHanBaoLanhTamUngFilterId')),
                    ghiChuFilter: $('#GhiChuFilterId').val(),
                    minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                    maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDuAnThuHoiModalSaved', function () {
            getDuAnThuHoies();
        });

        $('#GetDuAnThuHoiesButton').click(function (e) {
            e.preventDefault();
            getDuAnThuHoies();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAnThuHoies();
            }
        });

        $(document).off('click', '.chinhSuaDuAnThuHoi').on('click', '.chinhSuaDuAnThuHoi', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _createOrEditModal.open({id: id});
        });
        $(document).off('click', '.danhMucThuHoiModal').on('click', '.danhMucThuHoiModal', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _danhMucThuHoiModal.open({id: id});
        });
        $(document).off('click', '.ketXuatExcel').on('click', '.ketXuatExcel', function () {
            var self = $(this);
            var id = self.attr('data-rowId');
            _duAnThuHoiesService
                .baoCaoDuAnThuHoiToExcel({
                    id: id,
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });
        
    });
})();