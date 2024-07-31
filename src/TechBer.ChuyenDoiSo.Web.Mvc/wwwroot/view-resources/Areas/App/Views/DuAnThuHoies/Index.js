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
                                    _viewDuAnThuHoiModal.open({id: data.record.duAnThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('ChiTiet'),
                                action: function (data) {
                                    _danhMucThuHoiModal.open({id: data.record.duAnThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.duAnThuHoi.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDuAnThuHoi(data.record.duAnThuHoi);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "duAnThuHoi.maDATH",
                    name: "maDATH"
                },
                {
                    targets: 2,
                    data: "duAnThuHoi.ten",
                    name: "ten"
                },
                {
                    targets: 3,
                    data: "duAnThuHoi.namQuanLy",
                    name: "namQuanLy"
                },
                {
                    targets: 4,
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
                    targets: 5,
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
                    targets: 6,
                    data: "duAnThuHoi.ghiChu",
                    name: "ghiChu"
                },
                {
                    targets: 7,
                    data: "duAnThuHoi.trangThai",
                    name: "trangThai"
                }
            ]
        });

        function getDuAnThuHoies() {
            dataTable.ajax.reload();
        }

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
    });
})();